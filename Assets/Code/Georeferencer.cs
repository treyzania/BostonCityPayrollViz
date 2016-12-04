using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BostonViz {

	public class Georeferencer : MonoBehaviour {

		public GameObject georeferenceContainer;
		
		public Vector3 MeanPositionGeoreference(Vector2 given) {

			Debug.Log ("Georeffing " + given);
			GeorefWaypoint[] wpts = this.georeferenceContainer.GetComponentsInChildren<GeorefWaypoint> ();

			Vector2 sum = Vector2.zero;
			int components = 0;

			// It's okay that we technically do each of these twice, because they all get the double weight anyways.
			for (int i = 0; i < wpts.Length; i++) {

				GeorefWaypoint w1 = wpts [i];

				for (int j = i; j < wpts.Length; j++) {

					GeorefWaypoint w2 = wpts [j];

					// Don't do anything with itself because that gets messy really fast.
					if (w1 == w2) continue;

					// FIXME Make this a more general algorithm that actually takes into account how it's 3D and stuff.

					// Pull out useful values.  The names aren't really consistent at this point, but don't think about that.
					float w1x = Mathf.Min (w1.transform.position.x, w2.transform.position.x);
					float w2x = Mathf.Max (w1.transform.position.x, w2.transform.position.x);
					float w1y = Mathf.Min (w1.transform.position.y, w2.transform.position.y);
					float w2y = Mathf.Max (w1.transform.position.y, w2.transform.position.y);
					float w1lat = Mathf.Min (w1.Latitude, w2.Latitude);
					float w1lon = Mathf.Min (w1.Longitude, w2.Longitude);
					float w2lat = Mathf.Max (w1.Latitude, w2.Latitude);
					float w2lon = Mathf.Max (w1.Longitude, w2.Longitude);

					// Perform the georeferencing calculation.
					float nx = ((given.x - w1lat) / (w2lat - w1lat)) * (w2x - w1x) + w1x;
					float ny = ((given.y - w1lon) / (w2lon - w1lon)) * (w2y - w1y) + w1y;

					// Add it to the accumulator
					Vector2 here = new Vector2 (nx, ny);
					Debug.Log ("adding sum: " + here + "\t(" + w1 + ", " + w2 + ")");
					sum += here;
					components++;

				}

			}

			// Calculate the average vector and return.
			return new Vector2 (sum.x / ((float) components), sum.y / ((float) components));

		}

	}

}
