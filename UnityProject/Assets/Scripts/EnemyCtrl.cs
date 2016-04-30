using UnityEngine;
using System.Collections;

public class EnemyCtrl : MonoBehaviour {
	GameCtrl _gameCtrl;
	public HpCtrl _hpCtrl;

	float MAX_HP = 1000.0f;
	float hp;

	public GameObject _enemies;
	public GameObject[] _enemiesArray;
	int enemyNum = 0;

	void Start() {
		_gameCtrl = this.transform.parent.GetComponent<GameCtrl> ();

		initEnemiesArray ();
		initEnemy ();
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
			dieEnemy ();
		}
		_hpCtrl.setValue (hp);
	}

	void dieEnemy() {
		_gameCtrl.killEnemy ();
		MAX_HP += 200;
		initEnemy ();
	}

	// Use this for initialization
	void initEnemy() {
		hp = MAX_HP;
		_hpCtrl.initHp (hp);

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
}
