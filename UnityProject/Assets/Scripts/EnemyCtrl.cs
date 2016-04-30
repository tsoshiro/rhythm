using UnityEngine;
using System.Collections;

public class EnemyCtrl : MonoBehaviour {
	GameCtrl _gameCtrl;
	public HpCtrl _hpCtrl;

	float MAX_HP = 1000.0f;
	float hp;

	void Start() {
		_gameCtrl = this.transform.parent.GetComponent<GameCtrl> ();
		initEnemy ();
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

		initEnemy ();
	}

	// Use this for initialization
	void initEnemy() {
		hp = MAX_HP;
		_hpCtrl.initHp (hp);
	}	
}
