using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollCtrl : MonoBehaviour {

	[SerializeField]
	RectTransform prefab = null;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 10; i++) {
			var item = GameObject.Instantiate (prefab) as RectTransform;
			item.SetParent (transform, false);

			// Setting
			Text[] text = item.GetComponentsInChildren<Text> ();
			for (int j = 0; j < text.Length; j++) {
				if (text [j].name == "Name") { // Name
					text[j].text = "Supporter No."+i;
				} else if (text [j].name == "PPS") { // PPS
					text[j].text = i * 150 + " PPS";
				} else { // Button
					text[j].text = i * 50 + " COIN";
				}
			}
		}	
	}
}
