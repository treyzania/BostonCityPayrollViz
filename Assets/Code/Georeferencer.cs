using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BostonViz {

	public class Georeferencer : MonoBehaviour {

		public GameObject georeferenceContainer;
		
		public Vector3 MeanPositionGeoreference (Vector2 given) {

			Debug.Log ("Georeffing " + given);
			GeorefWaypoint[] wpts = this.georeferenceContainer.GetComponentsInChildren<GeorefWaypoint> ();

			List<Matrix4x4> mats = new List<Matrix4x4> ();

			// Find each "plane" and generate a matrix for each of them.
			for (int i = 0; i < wpts.Length; i++) {

				GeorefWaypoint w1 = wpts [i];

				for (int j = i; j < wpts.Length; j++) {

					GeorefWaypoint w2 = wpts [j];

					for (int k = j; k < wpts.Length; k++) {
						
						GeorefWaypoint w3 = wpts [k];

						// Don't do anything with itself because that gets messy really fast.
						if (w1 == w2 || w1 == w3 || w2 == w3) continue;

						// Transform the vectors from the input coords into real coordinates.
						Vector3 ll1 = new Vector3 (w1.Longitude, 1, w1.Latitude);
						Vector3 ll2 = new Vector3 (w2.Longitude, 1, w2.Latitude);
						Vector3 ll3 = new Vector3 (w3.Longitude, 1, w3.Latitude);

						// Make our matrix of input points.
						Matrix4x4 inMat = Matrix4x4.identity;
						inMat [0, 0] = ll1.x;
						inMat [1, 0] = ll1.y;
						inMat [2, 0] = ll1.z;
						inMat [0, 1] = ll2.x;
						inMat [1, 1] = ll2.y;
						inMat [2, 1] = ll2.z;
						inMat [0, 2] = ll3.x;
						inMat [1, 2] = ll3.y;
						inMat [2, 2] = ll3.z;

						// Make our matrix of output points.
						Matrix4x4 outMat = Matrix4x4.identity;
						outMat [0, 0] = w1.transform.position.x;
						outMat [1, 0] = w1.transform.position.y;
						outMat [2, 0] = w1.transform.position.z;
						outMat [0, 1] = w2.transform.position.x;
						outMat [1, 1] = w2.transform.position.y;
						outMat [2, 1] = w2.transform.position.z;
						outMat [0, 2] = w3.transform.position.x;
						outMat [1, 2] = w3.transform.position.y;
						outMat [2, 2] = w3.transform.position.z;

						// Figure out the transformation matrix for everything, biotch!
						mats.Add(outMat * inMat.inverse);

					}

				}

			}

			// Sanity check.
			if (mats.Count == 0) {

				Debug.Log ("Not enough valid georeference points!");
				return Vector2.zero;

			}

			// Average the result of all of the transformations.
			Vector4 realGiven = new Vector4 (given.x, 1, given.y, 1);
			Vector4 sum = Vector4.zero;
			foreach (Matrix4x4 mat in mats) {
				sum += mat * realGiven;
			}

			Debug.Log ("Using " + mats.Count + " matricies, matrix 0: " + mats[0]);

			// Calculate the average vector and return.
			return new Vector2 (sum.x / mats.Count, sum.z / mats.Count);

		}

	}

}
