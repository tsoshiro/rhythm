using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour {
	int 	coin;

	[SerializeField]
	float 	ppt; // point per tap

	public TextMesh _pptLabel;
	public TextMesh _coinLabel;

	public void setUserData(UserData pUser) {
		ppt = (float)pUser.pointPerTap;
	}

	void setPPT(float pPPT) {
		ppt = pPPT;
		_pptLabel.text = "PointPerTap:"+ppt;
	}

	public float getPPT() {
		return ppt;
	}

	public int getCoin() {
		return coin;
	}

	public void addCoin(int pCoin) {
		coin += pCoin;
		_coinLabel.text = "C:"+coin;
	}

	public void useCoin(int pCoin) {
		coin -= pCoin;
		_coinLabel.text = "C:" + coin;
	}

	public void setCoin(int pCoin) {
		coin = pCoin;
		_coinLabel.text = "C:" + coin;
	}
}
