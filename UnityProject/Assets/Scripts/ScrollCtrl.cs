using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScrollCtrl : MonoBehaviour {

	[SerializeField]
	RectTransform prefab = null;

	// Use this for initialization
	void Start () {
	}

 	// サポーターリストを作成するコード
	public void InitSupportersList(List<Supporter> pSupportersList) 
	{
		int count = pSupportersList.Count;
		for (int i = 0; i < count; i++) {
			var item = GameObject.Instantiate (prefab) as RectTransform;
			item.SetParent (transform, false);

			// Setting
			Text[] text = item.GetComponentsInChildren<Text> ();
			for (int j = 0; j < text.Length; j++) {
				if (text [j].name == "Name") { // Name
					text[j].text = pSupportersList[i].name;
				} else if (text [j].name == "PPS") { // PPS
					// text[j].text = i * 150 + " PPS";
					text[j].text = pSupportersList[i].pointPerSecond.ToString("F2") + " PPS";
				} else { // Button
					// text[j].text = i * 50 + " COIN";
					text[j].text = pSupportersList[i].nextLevelCoin + " COIN";
				}
			}

			for (int j = 0; j < item.transform.childCount; j++) {
				Debug.Log ("item.name: " + item.GetChild (j).name);
				if (item.GetChild (j).name == "Image") {
					Image img = item.GetChild (j).GetComponent<Image> ();
					img.sprite = pSupportersList [i].image;
				}
			}
		}	
	}
}
