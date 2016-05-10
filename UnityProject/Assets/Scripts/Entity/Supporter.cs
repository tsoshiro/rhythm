using UnityEngine;
using System.Collections;

public class Supporter {
	public int id; // サポーターID
	public string name; // サポーター名
	public int level; //現在のレベル
	public float pointPerSecond; // 現在の1秒あたりポイント
	public float nextLevelCoin; // 次のレベルに必要なコイン

	float attackInterval; // pointPerSecondを元に何秒に1回攻撃するか
	float pointPerAttack; // 1回の攻撃につき、何ポイントダメージを与えるか

	public void levelUp(int pAddLevel) {
		level += pAddLevel;
	}

	public Supporter() {
		//
	}

	public void initSupporter(int pId, int pLevel) {
		id = pId;
		name = getSupporterName (pId);
		level = pLevel;

		attackInterval = 1.0f;
		pointPerAttack = 4000;

		pointPerSecond = getPps (attackInterval, pointPerAttack);
	}

	string getSupporterName(int pId) {
		string result = "Sample Name";
		return result;
	}

	float getPps(float pTime, float pPPA) {
		return pPPA / pTime;
	}
		
	float timer;
	public float getPointUpdate() {
		// Updateごとに呼び出されAttackInterval秒ごとにPointPerAttackの値を返す
		timer += Time.deltaTime;
		if (timer >= attackInterval) {
			float amari = timer - attackInterval;
			timer = amari;

			// ATTACK
			return pointPerAttack;
		}
		return 0f;
	}
}
