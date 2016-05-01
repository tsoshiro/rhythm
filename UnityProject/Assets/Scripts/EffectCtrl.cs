using UnityEngine;
using System.Collections;

public class EffectCtrl : MonoBehaviour {
	public GameObject _effectContainer;
	public GameObject _effectPref;

	GameObject[] _effectArray;
	int effectNum = 3;
	int pos = 0;

	// Use this for initialization
	void Start () {
		_effectArray = new GameObject[effectNum];
		for (int i = 0; i < effectNum; i++) {
			GameObject go = (GameObject)Instantiate (_effectPref);
			_effectArray [i] = go;
			go.transform.parent = _effectContainer.transform;
			go.SetActive (false);
		}
	}

	public void showEffect() {
		_effectArray [pos].SetActive (true);
		if (pos == effectNum - 1) {
			_effectArray [0].SetActive (false);
		} else {
			_effectArray [pos + 1].SetActive (false);
		}

		pos++;
		if (pos == effectNum) {
			pos = 0;
		}
	}
}
