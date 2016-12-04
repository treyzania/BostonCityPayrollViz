using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BostonViz {

	public class GeorefPositionKeeper : MonoBehaviour {

		public Vector2 latLon;
		public float speed = 1F;

		private Georeferencer geo;

		void Start () {
			this.geo = this.GetComponentInParent<Georeferencer> ();
		}
		
		void Update () {
			
			Vector2 grPos = this.geo.MeanPositionGeoreference (this.latLon);
			this.transform.position = Vector3.Lerp (this.transform.position, new Vector3 (grPos.x, 0, grPos.y), Time.deltaTime * this.speed);

		}

	}

}