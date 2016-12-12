using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BostonViz {

	public class ColumnHeights : MonoBehaviour {

		public float lerpSpeed = 1F;

		private Dictionary<string, float> heights = new Dictionary<string, float> ();

		public void SetHeight (string code, float height) {

			Transform zipTrans = this.transform.FindChild(code);
			if (zipTrans) {

				GameObject go = zipTrans.gameObject;
				if (go) {
					
					ColumnController ctrl = go.GetComponent<ColumnController> ();
					if (ctrl) {
						ctrl.targetHeightScale = height;
					} else {
						Debug.LogWarning ("No col ctrl on " + go.name + "!");
					}

					this.heights [go.name] = height;

				} else {
					Debug.LogWarning ("No gameobject on transforms for " + code + "!?!?!?!");
				}
				
			}
			
		}

		private float GetScaleRatio () {

			float sum = 0F;
			int count = 0;
			foreach (string zip in this.heights.Keys) {
				
				sum += this.heights [zip];
				count ++;

			}

			return 1F / (count > 0 ? sum / count : 1F);

		}

		void Update () {
			this.transform.localScale = Vector3.Lerp (this.transform.localScale, new Vector3 (1, this.GetScaleRatio(), 1), Time.deltaTime * this.lerpSpeed);
		}

		public void ResetHeights () {

			ColumnController[] ctrls = this.GetComponentsInChildren<ColumnController> ();
			foreach (ColumnController ctrl in ctrls) {
				
				this.heights [ctrl.gameObject.name] = 0F;
				ctrl.targetHeightScale = 0F;

			}

		}

	}

}
