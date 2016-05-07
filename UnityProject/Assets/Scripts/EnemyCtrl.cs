using UnityEngine;
using System.Collections;

public class EnemyCtrl : MonoBehaviour {
	GameCtrl _gameCtrl;
	public HpCtrl _hpCtrl;

	float MAX_HP = 1000.0f;
	float ADD_HP = 200.0f;
	float hp;

	public GameObject _enemies;
	public GameObject[] _enemiesArray;
	int enemyNum = 0;

	void Start() {
		_gameCtrl = this.transform.parent.GetComponent<GameCtrl> ();
		initEnemiesArray ();
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
		MAX_HP += 200;
		initEnemy (pKillCount);	
	}

	public void resetEnemyMAXHP() {
		hp = MAX_HP;
	}

	public void initEnemy(int pLastKillCount = 0, int pLastEnemyNum = -1) {
		Debug.Log ("pLastKillCount:" + pLastKillCount+" MAX_HP:"+MAX_HP);
		hp = MAX_HP + (float)pLastKillCount * ADD_HP;
		Debug.Log ("hp:" + hp);
		_hpCtrl.initHp (hp);
		_hpCtrl.setValue (hp);

		// ID指定があれば
		if (pLastEnemyNum >= 0) {
			enemyNum = pLastEnemyNum;
		}

		for (int i = 0; i < _enemiesArray.Length; i++) {
			if (i == enemyNum) {
				iTween.FadeTo (_enemiesArray [i], iTween.Hash ("a", 1, "time", 0.5f));
			} else {
				iTween.FadeTo (_enemiesArray [i], iTween.Hash ("a", 0, "time", 0.5f));
			}
		}
		enemyNum++;
		if (enemyNum == _enemiesArray.Length) {
			enemyNum = 0;
		}
	}

	public int getEnemyNum() {
		return enemyNum;
	}
}
