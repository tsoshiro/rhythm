using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SupporterPanelCtrl : MonoBehaviour {
	public Text _name;
	public Text _pps;
	public Text _nextCoin;
	public Button _nextButton;
	public Image _image;

	void Start() {
		_nextCoin = _nextButton.GetComponentInChildren<Text>();
	}

	public void init(Supporter pSupporter) {
		_name.text = pSupporter.name;
		_pps.text = pSupporter.pointPerSecond.ToString();
		_nextCoin.text = pSupporter.nextLevelCoin.ToString();
//		_image.sprite = pSupporter.
	}

	// Update is called once per frame
	void Update () {
	



	}
}
