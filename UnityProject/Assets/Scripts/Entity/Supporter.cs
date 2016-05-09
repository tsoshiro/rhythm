using UnityEngine;
using System.Collections;

public class Supporter {
	public int id; // サポーターID
	public string name; // サポーター名
	public int level; //現在のレベル
	public int pointPerSecond; // 現在の1秒あたりポイント
	public int nextLevelCoin; // 次のレベルに必要なコイン


	public void levelUp(int pAddLevel) {
		level += pAddLevel;
	}

	public Supporter(int pId, int pLevel) {
		id = pId;
		name = getSupporterName (pId);
		level = pLevel;
		pointPerSecond = getSupporterPps (pId, pLevel);
	}

	string getSupporterName(int pId) {
		string result = "";
		return result;
	}

	int getSupporterPps(int pId, int pLevel) {
		int value = 0;
		return value;
	}
}
