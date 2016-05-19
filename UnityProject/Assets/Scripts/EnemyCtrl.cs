using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemyCtrl : MonoBehaviour {
	GameCtrl _gameCtrl;
	public HpCtrl _hpCtrl;

	float MAX_HP;
	float hp;
	int dropCoin;
	string imagePath;

	public GameObject _enemies;
	public GameObject[] _enemiesArray;
	int enemyNum = 0;

	List<EnemyMaster> _enemyMasterList = new List<EnemyMaster> ();
	List<EnemyLevelMaster> _enemyLevelMasterList = new List<EnemyLevelMaster>();

	public int getMaxLevel() {return _enemyLevelMasterList.Count;}

	void Start() {
		_gameCtrl = this.transform.parent.GetComponent<GameCtrl> ();
		initEnemyData ();
		initEnemiesArray ();
	}

	void initEnemyData() {
		var enemyMasterTable = new EnemyMasterTable ();
		enemyMasterTable.Load ();
		foreach (var enemyMaster in enemyMasterTable.All) {
			_enemyMasterList.Add (enemyMaster);
		}
		var enemyLevelMasterTable = new EnemyLevelMasterTable ();
		enemyLevelMasterTable.Load ();
		foreach (var enemyMaster in enemyLevelMasterTable.All) {
			_enemyLevelMasterList.Add (enemyMaster);
		}
	}

	void initEnemiesArray() {
		int enemyNumber = _enemies.transform.childCount;
		_enemiesArray = new GameObject[enemyNumber];
		for (int i = 0; i < enemyNumber; i++) {
			_enemiesArray [i] = _enemies.transform.GetChild (i).gameObject;
		}
	}

	public void hitPoint(float pPoint) {
		hp -= pPoint;
		if (hp <= 0) {
			_gameCtrl.killEnemy ();
		}
		_hpCtrl.setValue (hp);
	}

	public void spawnEnemy(int pKillCount) {
		initEnemyFromDB (pKillCount);
	}

	public void resetEnemyMAXHP() {
		hp = MAX_HP;
	}

	public void initEnemyFromDB(int pKillCount)
	{
		// 敵の種類とレベルを取得
		int lv = pKillCount / _enemyMasterList.Count;
		if (lv <= 0) {
			lv = 1;
		}
		if (lv >= getMaxLevel ()) {
			lv = getMaxLevel();
		}
		int startId = pKillCount % 3;

		enemyNum = startId;
		int hpBase = _enemyMasterList [startId].base_hp;
		float hpLvRate = _enemyLevelMasterList [lv - 1].multiple_rate;
		MAX_HP = (float)hpBase * hpLvRate;
		hp = MAX_HP;

		_hpCtrl.initHp (hp);
		_hpCtrl.setValue (hp);

		dropCoin = (int) ((float)_enemyMasterList[startId].drop_coin * _enemyLevelMasterList [lv - 1].multiple_rate);
		imagePath = _enemyMasterList [startId].image_path;

		Debug.Log ("id:" + startId + " lv:" + lv + " hpBase:" + hpBase + " hpLvRate:" + hpLvRate + " hp:" + hp+" drop_coin:"+dropCoin+" image_path:"+imagePath);
		Sprite sp = Resources.Load<Sprite> (imagePath);

		for (int i = 0; i < _enemiesArray.Length; i++) {
			if (i == enemyNum) {
				_enemiesArray [i].GetComponent<SpriteRenderer> ().sprite = sp;
				iTween.FadeTo (_enemiesArray [i], iTween.Hash ("a", 1, "time", 0.5f));
			} else {
				iTween.FadeTo (_enemiesArray [i], iTween.Hash ("a", 0, "time", 0.5f));
			}
		}
	}

	public int getEnemyNum() {
		return enemyNum;
	}

	public int getDropCoinValue() {
		return dropCoin;
	}
}
