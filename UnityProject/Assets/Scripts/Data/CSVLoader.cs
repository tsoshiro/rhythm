using UnityEngine;
using System.Collections;
using System.IO;

public class CSVLoader{
	TextAsset _data;

	public void readData(string pFileName) {
		_data = Resources.Load (pFileName) as TextAsset;
		StringReader sr = new StringReader (_data.text);
		while (sr.Peek() > -1) {
			string line = sr.ReadLine ();
			string[] values = line.Split(',');

			string str = "";
			for (int i = 0; i < values.Length; i++) {
				str += " "+values [i];
			}
			Debug.Log (str);
		}
	}
}
