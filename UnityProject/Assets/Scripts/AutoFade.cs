using UnityEngine;
using System.Collections;

public class AutoFade : MonoBehaviour {
	TextMesh me;
	float alpha = 1.0f;
	Color myColor;

	// Use this for initialization
	void Start () {
		me = this.gameObject.GetComponent<TextMesh> ();
		myColor = me.color;
		alpha = myColor.a;
	}
		
	// Update is called once per frame
	void Update () {
		alpha -= 0.1f / 15;
		myColor.a = alpha;
		me.color = myColor;
	}

	public void resetColor() {
		alpha = 1.0f;
		myColor.a = alpha;
		me.color = myColor;
	}
}
