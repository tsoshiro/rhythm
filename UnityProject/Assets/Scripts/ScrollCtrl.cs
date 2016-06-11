using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class ScrollCtrl : MonoBehaviour {

	[SerializeField]
	RectTransform prefab = null;

	GameCtrl _gameCtrl;

	// Use this for initialization
	void Start () {
		_gameCtrl = GameObject.Find("GameCtrl").GetComponent<GameCtrl>();
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
			//Button btn = item.GetComponentInChildren<Button>();
			//UnityAction<int> onClickAction = _gameCtrl.purchaseSupporter(aSupporter.id);
			//btn.onClick.AddListener();

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
