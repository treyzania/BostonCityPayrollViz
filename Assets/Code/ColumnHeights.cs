using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BostonViz {

	public class ColumnHeights : MonoBehaviour {

		public void SetHeight(string code, float height) {

			Transform zipTrans = this.transform.FindChild(code);
			if (zipTrans) zipTrans.localScale = new Vector3(1, height, 1);
			
		}

	}

}
