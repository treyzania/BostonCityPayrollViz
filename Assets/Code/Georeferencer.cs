using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BostonViz {

	public class Georeferencer : MonoBehaviour {

		public GameObject georeferenceContainer;
		
		public Vector3 MeanPositionGeoreference(Vector2 given) {
			
			GeorefWaypoint[] wpts = this.georeferenceContainer.GetComponentsInChildren<GeorefWaypoint> ();

			Vector2 sum = Vector2.zero;
			int components = 0;

			// It's okay that we technically do each of these twice, because they all get the double weight anyways.
			foreach (GeorefWaypoint w1 in wpts) {
				
				foreach (GeorefWaypoint w2 in wpts) {

					// Don't do anything with itself because that gets messy really fast.
					if (w1 != w2) {


						// FIXME Make this a more general algorithm that actually takes into account how it's 3D and stuff.

						// Pull out useful values.
						float w1x = w1.transform.position.x;
						float w2x = w2.transform.position.x;
						float dx = w2x - w1x;
						float w1y = w1.transform.position.y;
						float w2y = w2.transform.position.y;
						float dy = w2x - w1y;

						// Do the transformation.
						float nx = (given.x - w1x) / dx + w1x;
						float ny = (given.y - w1y) / dy + w1y;

						// Add it to the accumulator
						sum += new Vector2 (nx, ny);
						components++;

					}

				}

			}

			// Calculate the average vector and return.
			return new Vector2 (sum.x / components, sum.y / components);

		}

	}

}
