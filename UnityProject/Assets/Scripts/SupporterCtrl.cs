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

	public void init() {
		initSupporterMasterData ();
		initUserSupporterData ();
	}

	// Supporter関連のマスターデータ取得
	void initSupporterMasterData() 
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
	void initUserSupporterData() 
	{
		// ユーザーのテーブル構成
		// ID / LV ... LV.0 = 未開放
		// コスト情報を取得

		// サポーターのマスターデータを取得
		int availableSupportersNumber = _supporterMasterList.Count;

		supporterNextLevelCoin = new int[availableSupportersNumber];
		for (int i = 0; i < availableSupportersNumber; i++) {
			float value = (float)_supporterMasterList [i].base_coin * _supporterLevelMasterList[0].multiple_rate_coin;
			supporterNextLevelCoin [i] = Mathf.FloorToInt (value);
		}

		supportersList = new List<Supporter>();

		// 所持サポーター数
		int[] supportersData = PlayerPrefsX.GetIntArray(Const.PREF_SUPPORTER_LEVELS);
		if (supportersData.Length <= 0) {
			supportersData = new int[availableSupportersNumber];
			for (int i = 0; i < availableSupportersNumber; i++) {
				supportersData [i] = 0;
			}
			PlayerPrefsX.SetIntArray (Const.PREF_SUPPORTER_LEVELS);
		}

		Dictionary<int, int> idLevel = new Dictionary<int,int> ();

		// Sample Data
		int[] id = {1,2};
		int[] lv = {1,5};

		for (int i = 0; i < supportersData.Length; i++) {
			if (i + 1 == id [0]) {
				supportersData [i] = lv [0];
			}
			if (i + 1 == id [1]) {
				supportersData [i] = lv [1];
			}				
		}
	

		for (int i = 0; i < supportersData.Length; i++) {
			if (supportersData [i] > 0) { // 解放済み
				// クラスを生成
				Supporter sp = getSupporterClass (id[i], lv[i]);
				Debug.Log ("SP ID:" + sp.id + " NAME:"+sp.name+"PPS:"+sp.pointPerSecond);
				supportersList.Add (sp);

				// 解放済みサポーターのコストを更新する
				supporterNextLevelCoin[id[i] - 1] = sp.nextLevelCoin;
			
			} else { //未開放
				// 解放のためのコストを取得
				idLevel.Add(i+1, _supporterMasterList[i].base_coin);
			}
		}

		for (int i = 0; i < supportersData.Length; i++) {
			SupporterMaster spMaster = _supporterMasterList [i];
			Debug.Log ("ID: " + spMaster.id + " NAME: "+spMaster.name
				+" LEVEL: "+supportersData[i]+" NEXT_COIN: "+supporterNextLevelCoin[i]);
		}
	}

	// idとレベルから、ppa、next_level_coinを算出し、supporterクラスを生成
	Supporter getSupporterClass(int pId, int pLevel) 
	{
		Supporter sp = new Supporter ();

		string name = _supporterMasterList [pId - 1].name;
		float basePpa = _supporterMasterList [pId - 1].base_ppa;
		float atkInt = _supporterMasterList [pId - 1].atk_interval;
		int baseCoin = _supporterMasterList [pId - 1].base_coin;

		string growthType = _supporterLevelMasterList [pId - 1].growth_type;

		float multipleRatePpa = 1;
		float multipleRateCoin = 1;

		for (int i = 0; i < _supporterLevelMasterList.Count; i++) {
			if (_supporterLevelMasterList[i].growth_type == growthType) {
				if (_supporterLevelMasterList [i].level == pLevel) {
					multipleRatePpa = _supporterLevelMasterList [i].multiple_rate_ppa;
					multipleRateCoin = _supporterLevelMasterList [i].multiple_rate_coin;
				}
			}
		}

		sp.id = pId;
		sp.level = pLevel;
		sp.name = name;
		sp.pointPerAttack = basePpa * multipleRatePpa;
		sp.attackInterval = atkInt;
		sp.pointPerSecond = sp.getPps (sp.attackInterval, sp.pointPerAttack);
		sp.nextLevelCoin = Mathf.FloorToInt((float)baseCoin * multipleRateCoin);

		return sp;
	}

	// サポーターのポイントを取得し、反映させる
	public float getSupporterValues() {
		if (supportersList.Count <= 0) {
			return 0;
		}
	
		float supportersPoint = 0.0f;
		for (int i = 0; i < supportersList.Count; i++) {
			supportersPoint += supportersList [i].getPointUpdate ();
		}
		return supportersPoint;

	}

}
