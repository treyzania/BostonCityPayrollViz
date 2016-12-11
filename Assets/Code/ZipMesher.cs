using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BostonViz {
	
	[RequireComponent(typeof(Georeferencer))]
	public class ZipMesher : MonoBehaviour {

		public GameObject prefab;
		public Transform destination;

		public string stateString;

		private Georeferencer geo;

		public string jsonPath = "foo";
		private TextAsset jsonAsset;

		void Start () {
			
			this.geo = this.GetComponent<Georeferencer> ();

			this.jsonAsset = Resources.Load<TextAsset> (this.jsonPath);

			this.StartCoroutine (GenerateMeshes());

		}

		private IEnumerator GenerateMeshes () {

			Dictionary<string, object> root = JsonConvert.DeserializeObject<Dictionary<string, object>> (this.jsonAsset.text);
			Debug.Log(root["features"].GetType());
			JArray feats = (JArray) root ["features"];

			Debug.Log ("Loaded " + feats.Count + " features.");

			List<ZipCode> zips = new List<ZipCode>();

			this.stateString = "Parsing raw GeoJSON";

			foreach (JToken token in feats) {

				JObject feat = (JObject) token;

				JObject props = (JObject) feat ["properties"];
				
				// Process the metadata.
				string zipNumber = (string) ((JValue) props ["ZIP_CODE"]).ToObject(typeof(string));
				string name = (string) ((JValue) props ["NAME"]).ToObject(typeof(string));
				float pop = (float) ((JValue) props ["POPULATION"]).ToObject(typeof(float));
				float area = (float) ((JValue) props ["SQMI"]).ToObject(typeof(float));
				ZipCode zip = new ZipCode(zipNumber, name, pop, area);

				// Process the polygon verticies.
				JArray coordset = (JArray) ((JObject) feat ["geometry"]) ["coordinates"];
				JArray polys = (JArray) coordset [0];
				JArray poly = (JArray) polys [0];
				foreach (JToken vertToken in poly) {
					
					float lat = (float) vertToken [0].ToObject(typeof(float));
					float lon = (float) vertToken [1].ToObject(typeof(float));
					zip.points.Add(this.geo.MeanPositionGeoreference(new Vector2(lat, lon)));

				}

				zips.Add(zip);

				yield return null;

			}

			Debug.Log ("Generated " + zips.Count + " ZIP code objects.");

			this.stateString = "Generating meshes for polygons";

			int totalVerts = 0;
			int totalTris = 0;
			
			foreach (ZipCode zip in zips) {

				Vector3[] verts = new Vector3 [zip.points.Count * 2];
				List<Tri> tris = new List<Tri> ();

				// Generate the vertices themselves.
				for (int i = 0; i < zip.points.Count; i++) {

					Vector2 p = zip.points [i];
					verts [i * 2] = new Vector3 (p.x, 0, p.y);
					verts [(i * 2) + 1] = new Vector3 (p.x, 1, p.y);

					totalVerts += 2;

				}

				// Generate the triangles around the side.
				for (int i = 0; i < verts.Length; i += 2) {

					// Do this clockwise, because fuck logic.
					tris.Add(new Tri(i, (i + 3) % verts.Length, (i + 1) % verts.Length));
					tris.Add(new Tri(i, (i + 2) % verts.Length, (i + 3) % verts.Length));

					totalTris += 2;

				}

				// Convert the Tri array into the kind of array Unity likes.
				int[] triArray = new int[tris.Count * 3];
				for (int i = 0; i < tris.Count; i++) {

					Tri t = tris [i];

					triArray [i * 3] = t.a;
					triArray [(i * 3) + 1] = t.b;
					triArray [(i * 3) + 2] = t.c;

				}

				// Mesh for the sides.
				Mesh side = new Mesh ();
				side.vertices = verts;
				side.triangles = triArray;

				// Make ourselves the top of the pillar.  First, set up the points.
				Vector3[] topVerts = new Vector3 [zip.points.Count]; 
				for (int i = 0; i < topVerts.Length; i++) {
					topVerts [i] = new Vector3 (zip.points [i].x, 1, zip.points [i].y);
				}

				// Naive method to make the triangles.  Only works for convex polygons.
				List<Tri> topTris = new List<Tri> ();
				for (int i = 0; i < topVerts.Length; i++) {
					topTris.Add (new Tri (0, i, (i + 1) % topVerts.Length));
				}

				// Convert the tris to the native kind.
				int[] topTriVerts = new int [topTris.Count * 3];
				for (int i = 0; i < topTris.Count; i++) {

					Tri t = topTris [i];

					topTriVerts [i * 3] = t.a;
					topTriVerts [(i * 3) + 1] = t.b;
					topTriVerts [(i * 3) + 2] = t.c; 

				}
				
				// Mesh for the top.
				Mesh top = new Mesh();
				top.vertices = topVerts;
				top.triangles = topTriVerts;

				// Combine the meshes.
				Mesh m = new Mesh();
				CombineInstance combSide = new CombineInstance ();
				combSide.mesh = side;
				combSide.subMeshIndex = 0;
				combSide.transform = Matrix4x4.identity;
				CombineInstance combTop = new CombineInstance ();
				combTop.mesh = top;
				combTop.subMeshIndex = 0;
				combTop.transform = Matrix4x4.identity;
				m.CombineMeshes (new CombineInstance [] { combSide, combTop });

				// Cleanup
				m.RecalculateBounds ();
				m.RecalculateNormals ();

				zip.mesh = m;

				yield return null;

			}

			Debug.Log ("Generated a total of " + totalVerts + " verts making up a total of " + totalTris + " tris.");
			this.stateString = "Spawning GameObjects";

			// Create the GameObjects for the things.
			foreach (ZipCode zip in zips) {

				GameObject go = GameObject.Instantiate (this.prefab, this.destination);
				go.transform.position = this.transform.position;

				// Set the name.
				go.name = zip.id;

				// Set the mesh to render.
				MeshFilter filter = go.GetComponentInChildren<MeshFilter> ();
				filter.mesh = zip.mesh;

				// Turn off shadowcasting because that shit gets expensive.
				MeshRenderer renderer = go.GetComponentInChildren<MeshRenderer> ();
				renderer.shadowCastingMode = ShadowCastingMode.Off;

				yield return null;

			}

			this.stateString = "Done!";

			this.enabled = false;
			yield return null;

		}

	}

}
