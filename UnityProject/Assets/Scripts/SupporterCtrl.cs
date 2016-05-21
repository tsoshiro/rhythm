using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SupporterCtrl : MonoBehaviour {
	public GameCtrl _gameCtrl;
	List<SupporterMaster> _supporterMasterList = new List<SupporterMaster>();
	List<SupporterLevelMaster> _supporterLevelMasterList = new List<SupporterLevelMaster>();

	// Supporters
	List<Supporter> supportersList;
	// Level Cost Table
	int[] supporterNextLevelCoin;


	// Use this for initialization
	void Start () {
		_gameCtrl = this.transform.parent.GetComponent<GameCtrl> ();
	}

	// Supporter関連のマスターデータ取得
	void initData() 
	{
		var entityMasterTable = new SupporterMasterTable ();
		entityMasterTable.Load ();
		foreach (var entityMaster in entityMasterTable.All) {
			_supporterMasterList.Add (entityMaster);
		}
		var entityLevelMasterTable = new SupporterLevelMasterTable ();
		entityLevelMasterTable.Load ();
		foreach (var entityMaster in entityLevelMasterTable.All) {
			_supporterLevelMasterList.Add (entityMaster);
		}
	}

	// ユーザーデータからSupporterクラスを生成・管理
	void initUserData() 
	{
		// コスト情報を取得

		// サポーターの種類を取得
		int availableSupportersNumber = _supporterMasterList.Count;

		supporterNextLevelCoin = new int[availableSupportersNumber];
		for (int i = 0; i < availableSupportersNumber; i++) {
			float value = (float)_supporterMasterList [i].base_coin * _supporterLevelMasterList [i].multiple_rate_coin;
			supporterNextLevelCoin [i] = Mathf.FloorToInt (value);
		}

		supportersList = new List<Supporter>();

		// 所持サポーター数
		int count = 1;
		int id = 1;
		for (int i = 0; i < count; i++) {
			Supporter sp = new Supporter ();
			sp.initSupporter (id, 1);
			Debug.Log ("SP ID:" + sp.id + " NAME:"+sp.name+"PPS:" + sp.pointPerSecond);
			supportersList.Add (sp);

			// 解放済みサポーターのコストを更新する
			supporterNextLevelCoin[id - 1] = sp.nextLevelCoin;
		}
	}

	// サポーターのポイントを取得し、反映させる
	void getSupporterValues() {
		//		if (supportersList.Count <= 0) {
		//			return;
		//		}
		//
		//		float supportersPoint = 0.0f;
		//		for (int i = 0; i < supportersList.Count; i++) {
		//			supportersPoint += supportersList [i].getPointUpdate ();
		//		}
		//		sendPointToEnemy (supportersPoint, false);
	}

}
