using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BostonViz { 
	
	public class PayPerCapitaViz : MonoBehaviour {

		public ColumnHeights container;
		public PayrollParser payroll;

		public void Trigger () {

			List<string> zips = new List<string> ();
			Dictionary<string, float> paysums = new Dictionary<string, float> ();
			Dictionary<string, int> capitas = new Dictionary<string, int> ();

			foreach (PayrollEntry pe in payroll.entries) {

				if (!zips.Contains (pe.zip)) zips.Add (pe.zip);

				if (paysums.ContainsKey (pe.zip)) {
					paysums [pe.zip] += pe.totalPay;
				} else {
					paysums [pe.zip] = pe.totalPay;
				}

				if (capitas.ContainsKey (pe.zip)) {
					capitas [pe.zip] ++;
				} else {
					capitas [pe.zip] = 1;
				}

			}

			Debug.Log("Found settings for " + zips.Count + " zips, vs " + paysums.Count + " and " + capitas.Count + ".");

			this.container.ResetHeights ();
			foreach (string zip in zips) {
				this.container.SetHeight (zip, paysums [zip] / capitas [zip]);
			}

		}

	}

}
