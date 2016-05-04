using UnityEngine;
using System.Collections;

public class UserData {
	public int level; //現在のレベル
	public int pointPerTap; // 現在のタップあたりポイント
	public int nextLevelCoin; // 次のレベルに必要なコイン

	int DEFAULT_PPT = 50;
	int DEFAULT_ADD_PPT = 100;
	int DEFAULT_COIN = 300;

	public UserData(int pLevel = 1) {
		setUserData (pLevel);
	}

	public void setUserData(int pLevel) {
		level = pLevel;
		pointPerTap = DEFAULT_PPT + DEFAULT_ADD_PPT * pLevel;
		nextLevelCoin = DEFAULT_COIN * pLevel;
	}

	public UserData getUserData(int pLevel) {
		return this;
	}

//	public UserData getNextUser(int pLevel) {
//		UserData ud = new UserData ();
//		ud = getInitUser (pLevel);
//		return ud;
//	}
}