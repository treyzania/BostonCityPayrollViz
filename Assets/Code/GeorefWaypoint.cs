using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BostonViz {

	public class GeorefWaypoint : MonoBehaviour {

		public Vector2 latLonPosition;

		public float Latitude {

			get {
				return this.latLonPosition.y;
			}

		}

		public float Longitude {

			get { 
				return this.latLonPosition.x;
			}

		}

	}

}
