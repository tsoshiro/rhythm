using UnityEngine;
using System.Collections;

public class TapCtrl : MonoBehaviour {
	public GameObject _circle;
	public float _BPM = 120;
	float loop_time = 1.0f;
	float timer = 0.0f;


	Vector3 _circleScale;

	// Use this for initialization
	void Start () {
		_circleScale = _circle.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () 
	{
		loop_time = getLoopTimeFromBPM (_BPM);
		// タップを判定する
		if (Input.GetMouseButtonDown (0)) {
		
		}
		stretchCircle ();
	}

	// x秒ごとに円が収縮を繰り返す
	void stretchCircle() {
		timer += Time.deltaTime;
		if (timer >= loop_time) {
			float amari = timer - loop_time;
			timer = 0;
			timer += amari;
		}
		float rate = timer / loop_time;
		Debug.Log ("rate : " + rate);
		rate -= 1.0f;
		_circle.transform.localScale = _circleScale * rate;
	}

	float getLoopTimeFromBPM(float pBpm) {
		// BPM=Beat Per Minute 1分間の拍の数
		// BPM120 → 120 beat / 60 sec → 2 beat / 1 sec
		// 1sec/2beat = loop_time
		// loop_timeは0.5f
		float lt = _BPM / 60;
		lt = 1 / lt;
		Debug.Log ("lt: " + lt);
		return lt;
	}
}
