using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour {
	int 	coin;

	[SerializeField]
	float 	ppt; // point per tap

	public TextMesh _pptLabel;
	public TextMesh _coinLabel;

	void Start() {
		setPPT (ppt);
	}

	void setPPT(float pPPT) {
		ppt = pPPT;
		_pptLabel.text = "PointPerTap:"+ppt;
	}

	public float getPPT() {
		return ppt;
	}

	public void addCoin(int pCoin) {
		coin += pCoin;
		_coinLabel.text = "COIN:"+coin;
	}
}
