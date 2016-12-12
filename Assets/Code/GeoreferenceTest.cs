using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BostonViz {

	[RequireComponent(typeof(Georeferencer))]
	public class GeoreferenceTest : MonoBehaviour {

		public GameObject columnPrefab;
		public LatLonColumn[] cols;

		private Georeferencer georef;

		void Start () {

			this.georef = this.GetComponent<Georeferencer> ();

			foreach (LatLonColumn col in cols) {

				// Georeference
				Vector2 georefed = this.georef.MeanPositionGeoreference (col.postion);

				// Instantiate
				GameObject colObj = (GameObject) GameObject.Instantiate (this.columnPrefab, new Vector3 (georefed.x, 0, georefed.y), Quaternion.identity, this.transform);
				ColumnController ctrl = colObj.GetComponent<ColumnController> ();
				GeorefPositionKeeper keeper = colObj.GetComponent<GeorefPositionKeeper> ();

				// Set params
				ctrl.targetHeightScale = col.height;
				keeper.latLon = col.postion;

			}

		}

	}

}
