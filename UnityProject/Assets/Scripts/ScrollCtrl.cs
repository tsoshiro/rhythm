using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class ScrollCtrl : MonoBehaviour {

	[SerializeField]
	RectTransform prefab = null;

	GameCtrl _gameCtrl;
	List<SupporterPanelCtrl> itemList = new List<SupporterPanelCtrl>();

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
			Button btn = item.GetComponentInChildren<Button>();
			int aId = aSupporter.id;
			btn.onClick.AddListener(() =>
			{
				_gameCtrl.purchaseSupporter(aId);
			});

			SupporterPanelCtrl spPanel = item.GetComponent<SupporterPanelCtrl>();
			spPanel.init(aSupporter);

			itemList.Add(spPanel);
		}	
	}
}
