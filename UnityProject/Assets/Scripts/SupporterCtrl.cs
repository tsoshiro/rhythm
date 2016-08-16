using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SupporterCtrl : MonoBehaviour {
	public GameCtrl _gameCtrl;
	public ScrollCtrl _scrollCtrl;

	List<SupporterMaster> _supporterMasterList = new List<SupporterMaster>();
	List<SupporterLevelMaster> _supporterLevelMasterList = new List<SupporterLevelMaster>();

	// Supporters
	List<Supporter> supportersList;
	// Level Cost Table
	int[] supporterNextLevelCoin;
	int[] supportersData;

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

		// 基本のコストを取得
		supporterNextLevelCoin = new int[availableSupportersNumber];
		for (int i = 0; i < availableSupportersNumber; i++) {
			float value = (float)_supporterMasterList [i].base_coin * _supporterLevelMasterList[0].multiple_rate_coin;
			supporterNextLevelCoin [i] = Mathf.FloorToInt (value);
		}

		supportersList = new List<Supporter>();

		// 所持サポーター情報を取得
		supportersData = PlayerPrefsX.GetIntArray(Const.PREF_SUPPORTER_LEVELS);
		if (supportersData.Length <= 0) { // PlayerPrefsが空の場合
			supportersData = new int[availableSupportersNumber];
			for (int i = 0; i < availableSupportersNumber; i++) {
				supportersData [i] = 0;
			}
			PlayerPrefsX.SetIntArray (Const.PREF_SUPPORTER_LEVELS);
		}

		for (int i = 0; i < supportersData.Length; i++) {
			Debug.Log ("SP DATA:"+supportersData [i]);
		}

		for (int i = 0; i < supportersData.Length; i++) {
			// クラスを生成
			int aLv;

			// 解放済みサポーターか判定
			bool aIsReleased = (supportersData [i] > 0) ? true : false;
			if (aIsReleased) {
				aLv = supportersData [i];
			} else {
				aLv = 0;
			}

			Supporter sp = getSupporterClass (i+1, aLv);
			Debug.Log ("SP ID:" + sp.id + " NAME:"+sp.name+"PPS:"+sp.pointPerSecond);
			supportersList.Add (sp);

			if (aIsReleased) {
				// 解放済みサポーターのコストを更新する
				supporterNextLevelCoin [i] = sp.nextLevelCoin;
			}
		}

		for (int i = 0; i < supportersData.Length; i++) {
			SupporterMaster spMaster = _supporterMasterList [i];
			Debug.Log ("ID: " + spMaster.id + " NAME: "+spMaster.name
				+" LEVEL: "+supportersData[i]+" NEXT_COIN: "+supporterNextLevelCoin[i]);
		}
		_scrollCtrl.InitSupportersList (supportersList);
	}

	// idとレベルから、ppa、next_level_coinを算出し、supporterクラスを生成
	Supporter getSupporterClass(int pId, int pLevel) 
	{
		Supporter sp = new Supporter ();

		string name 	= _supporterMasterList [pId - 1].name;
		float basePpa 	= _supporterMasterList [pId - 1].base_ppa;
		float atkInt	= _supporterMasterList [pId - 1].atk_interval;
		int baseCoin 	= _supporterMasterList [pId - 1].base_coin;
		string imagePath = "images/supporters/" + _supporterMasterList [pId - 1].image_path;

		Sprite image	= Resources.Load<Sprite>(imagePath) as Sprite;
		if (image != null) {
			Debug.Log ("imagePath: " + imagePath + " successfully loaded! " +image);
		}

		string growthType = _supporterLevelMasterList [pId - 1].growth_type;

		float multipleRatePpa = 1;
		float multipleRateCoin = 1;

		if (pLevel >= 1) {
			for (int i = 0; i < _supporterLevelMasterList.Count; i++) {
				if (_supporterLevelMasterList [i].growth_type == growthType &&
				    _supporterLevelMasterList [i].level == pLevel) {
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
		sp.image = image;

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

	public int getSupporterCost(int pId) {
		return supporterNextLevelCoin[pId - 1];
	}

	// 現在のLVに+1
	public bool raiseSupporterLevel(int pId) {
		Debug.Log("raiseSupporterLevel.pId:" + pId);
		if (pId == 0)
		{
			return false;
		}

		// LV UP後のデータを取得
		int lvNow = supportersList[pId - 1].level;
		if (setSupporterLevel(pId, lvNow + 1))
		{
			return true;
		}
		return false;
	}

	// LVを指定
	public bool setSupporterLevel(int pId, int pLevel) {
		Supporter sp = getSupporterClass(pId, pLevel);
		supportersList[pId - 1] = sp;

		// 次のLVまでのコストを取得
		supporterNextLevelCoin[pId - 1] = sp.nextLevelCoin;

		// PlayerPrefsにセーブ
		supportersData[pId - 1] = sp.level;
		saveSupporterData();

		// 表記を更新
		_scrollCtrl.updateSupportersInfo(sp);

		return true;
	}

	void saveSupporterData() {
		PlayerPrefsX.SetIntArray(Const.PREF_SUPPORTER_LEVELS, supportersData);
	}

	#region debug
	public void resetSupporter() {
		for (int i = 0; i < supportersData.Length; i++) {
			supportersData[i] = 0;
			setSupporterLevel(i + 1, 0);
		}
		saveSupporterData();
	}
	#endregion
}