using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BostonViz {

	public class ColumnController : MonoBehaviour {

		public float targetHeightScale = 1F;
		public float lerpFactor = 1F;

		private MeshRenderer mr;

		void Start () {
			this.mr = this.transform.FindChild("Model").gameObject.GetComponent<MeshRenderer> ();
		}

		void Update () {
			
			this.transform.localScale = Vector3.Lerp (this.transform.localScale, new Vector3 (1f, this.targetHeightScale, 1F), Time.deltaTime * this.lerpFactor);
			this.mr.enabled = this.transform.localScale.y > 0.1F; 

		}

	}

}
