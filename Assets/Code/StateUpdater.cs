using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BostonViz {

	public class StateUpdater : MonoBehaviour {

		public ZipMesher mesher;

		private Text text;

		void Start () {
			this.text = this.GetComponent<Text> ();
		}

		void Update () {
			text.text = "State: " + mesher.stateString;
		}

	}

}
