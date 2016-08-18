using UnityEngine;
using System.Collections;

public class TapCtrl : MonoBehaviour {
	float screenWidth;
	float screenHeight;
	public float widthRate = 0.99f;
	public float heightRate = 0.6f;
	float x, y, w, h;

	Rect rect;

	void Awake() {
		setRect();
	}

	void Update() {
		rect = new Rect();
		setRect();
	}

	void setRect() {
		w = Screen.width * widthRate;
		h = Screen.height * heightRate;
		x = (Screen.width - w) / 2;
		y = (Screen.height - h) / 2;

		rect.x = x;
		rect.y = y;
		rect.width = w;
		rect.height = h;
	}

	public Rect getTapArea() {
		return rect;
	}

	//void OnGUI()
	//{
	//	GUI.Box(rect, "");
	//}
}
