using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BostonViz {
	
	public class CountViz : MonoBehaviour {

		public ColumnHeights collection;
		public PayrollParser payroll;

		public void Trigger () {

			Dictionary<string, int> dict = new Dictionary<string, int> ();

			foreach (PayrollEntry pe in payroll.entries) {

				if (dict.ContainsKey (pe.zip)) {
					dict [pe.zip] ++;
				} else {
					dict [pe.zip] = 1;
				}

			}

			// Set up the new things.
			this.collection.ResetHeights ();
			foreach (string zip in dict.Keys) {
				this.collection.SetHeight (zip, dict [zip]);
			}

		}
		
	}

}
