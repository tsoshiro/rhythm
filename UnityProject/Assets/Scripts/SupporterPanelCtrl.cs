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
		Text[] text = this.GetComponentsInChildren<Text>();
		for (int j = 0; j < text.Length; j++)
		{
			if (text[j].name == "Name")
			{ // Name
				text[j].text = pSupporter.name + " Lv." + pSupporter.level;
			}
			else if (text[j].name == "PPS")
			{ // PPS
				text[j].text = pSupporter.pointPerSecond.ToString("F0") + " PPS";
			}
			else { // Button
				text[j].text = pSupporter.nextLevelCoin + " COIN";
			}
		}

		// 画像を当てる
		for (int j = 0; j < this.transform.childCount; j++)
		{
			Debug.Log("item.name: " + this.transform.GetChild(j).name);
			if (this.transform.GetChild(j).name == "Image")
			{
				Image img = this.transform.GetChild(j).GetComponent<Image>();
				img.sprite = pSupporter.image;
			}
		}
	}

	// Update is called once per frame
	void Update () {
	}

	void setInformation(Supporter pSupporter) {
		_name.text = pSupporter.name;
		_pps.text = pSupporter.pointPerSecond.ToString();
		_nextCoin.text = pSupporter.nextLevelCoin.ToString();
	}
}
