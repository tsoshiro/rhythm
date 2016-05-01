using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingCtrl : MonoBehaviour {
	public Text _label;
	public Slider _slider;
	string _labelName;

	void Start() {
		_slider = this.gameObject.GetComponent<Slider> ();
		_labelName = this.gameObject.name;
	}

	// Update is called once per frame
	void Update () {
		_label.text = _labelName + ":" + _slider.value;	
	}
}
