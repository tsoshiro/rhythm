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
			Supporter aSupporter = pSupportersList[i];

			var item = Instantiate (prefab) as RectTransform;
			item.SetParent (transform, false);

			// Setting
			Text[] text = item.GetComponentsInChildren<Text> ();
			for (int j = 0; j < text.Length; j++) {
				if (text [j].name == "Name") { // Name
					text[j].text = aSupporter.name + " Lv." + aSupporter.level;
				} else if (text [j].name == "PPS") { // PPS
					text[j].text = aSupporter.pointPerSecond.ToString("F0") + " PPS";
				} else { // Button
					text[j].text = aSupporter.nextLevelCoin + " COIN";
				}
			}

			// 画像を当てる
			for (int j = 0; j < item.transform.childCount; j++) {
				Debug.Log ("item.name: " + item.GetChild (j).name);
				if (item.GetChild (j).name == "Image") {
					Image img = item.GetChild (j).GetComponent<Image> ();
					img.sprite = aSupporter.image;
				}
			}
		}	
	}
}
