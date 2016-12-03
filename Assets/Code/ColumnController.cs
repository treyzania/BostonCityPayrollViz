using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BostonViz {

	public class ColumnController : MonoBehaviour {

		public float targetHeigtScale = 1F;
		public float lerpFactor = 1F;

		void Start () {

		}

		void Update () {
			this.transform.localScale = Vector3.Lerp (this.transform.localScale, new Vector3 (1f, this.targetHeigtScale, 1F), Time.deltaTime * this.lerpFactor);
		}

	}

}
