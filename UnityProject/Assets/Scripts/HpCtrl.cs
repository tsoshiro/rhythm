using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HpCtrl : MonoBehaviour {
	public Slider 	_hpBar;
	public Text _hpText;

	float maxHp;
	float nowHp;

	public void initHp(float pMaxHp) {
		maxHp = pMaxHp;
	}

	public void setValue(float pNowHp) {
		nowHp = pNowHp;

		float rate = nowHp / maxHp;
		_hpText.text = nowHp+"/"+maxHp;
		_hpBar.value = rate;
	}
}
