using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BostonViz {

	public class PayrollParser : MonoBehaviour {

		public string csvName = "foo";
		public List<PayrollEntry> entries;

		void Start () {

			TextAsset ta = Resources.Load<TextAsset> (this.csvName);
			string rawCsv = ta.text;

			this.entries = new List<PayrollEntry> ();

			string[] lines = rawCsv.Split ('\n');
			
			for (int i = 1; i < lines.Length; i++) {

				string[] line = lines [i].Split (',');

				if (line.Length != 13) continue; // Limit to things we can reason better about.

				// These numbers are weird because there's commas in the name field and C# is retarded.
				string zip = line [12];
				string totalStr = line [11];
				string dept = line [2];
				string title = line [3];

				PayrollEntry here = new PayrollEntry (zip, float.Parse (totalStr.Substring (1)), dept, title);
				this.entries.Add (here);

			}

			Debug.Log ("Parsed " + this.entries.Count + " payroll entries.");

		}
		
	}

}
