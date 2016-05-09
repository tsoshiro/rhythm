using UnityEngine;
using System.Collections;

public class Supporter{
	public int level; //現在のレベル
	public int pointPerSecond; // 現在の1秒あたりポイント
	public int nextLevelCoin; // 次のレベルに必要なコイン

	public void levelUp(int pAddLevel) {
		level += pAddLevel;
	}
}
