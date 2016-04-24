using UnityEngine;
using System.Collections;

public class TapCtrl : MonoBehaviour {
	public AudioCtrl _audioCtrl;

	public GameObject _circle;
	public TextMesh _tapResultText;
	public float _BPM = 120;
	float loop_time = 1.0f;
	float timer = 0.0f;
	float rate; // 割合。rate = 0.5fがドンピシャタイミング
	int comboCount;

	enum TIMING {
		BAD,
		GOOD,
		GREAT,
		EXCELLENT,
	}

	Vector3 _circleScale;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;

		_circleScale = _circle.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () 
	{
		loop_time = getLoopTimeFromBPM (_BPM);
		stretchCircle ();
		// タップを判定する
		if (Input.GetMouseButtonDown (0)) {
			tap ();
		}
	}

	void tap() {
		// タップのタイミングが、Timerとどれくらいずれているかを検証。近さに応じてBAD/GOOD/GREAT/EXCELLENTの4段階評価を表示する
		_audioCtrl.PlayHat ();

		// タップしたときのTimerの値を取得し、近さに応じて評価を出す
		TIMING tapResult = getTapResult(Mathf.Abs(rate));

		// コンボかどうかをカウントする
		if (tapResult == TIMING.EXCELLENT || tapResult == TIMING.GREAT) {
			comboCount++;
		} else {
			comboCount = 0;
		}

		// 評価を画面に表示する
		showTapResult(tapResult);
	}

	void showTapResult(TIMING pTapResult) {
		string resultText = "";
		switch (pTapResult) {
		case TIMING.EXCELLENT:
			resultText = "EXCELLENT!\n"+comboCount+" COMBO!";
			break;
		case TIMING.GREAT:
			resultText = "GREAT!\n"+comboCount+" COMBO!";
			break;
		case TIMING.GOOD:
			resultText = "GOOD!";
			break;
		case TIMING.BAD:
			resultText = "BAD!";
			break;
		default:
			resultText = "BAD!";
			break;
		}
		_tapResultText.text = resultText;
	}
		
	float DIF_VAL_GOOD = 0.3f;
	float DIF_VAL_GREAT  = 0.2f;
	float DIF_VAL_EXCELLENT = 0.1f;

	// タップしたタイミングとターゲットタイミングとの差に応じて評価を返す
	TIMING getTapResult(float tapRate) {
		float targetRate = 0.5f;
		TIMING result = TIMING.BAD;

		float difference = Mathf.Abs (tapRate - targetRate);
		Debug.Log ("tapRate : " + tapRate + " / difference : " + difference);
		if (difference <= DIF_VAL_EXCELLENT) {
			result = TIMING.EXCELLENT;
		} else if (difference <= DIF_VAL_GREAT) {
			result = TIMING.GREAT;
		} else if (difference <= DIF_VAL_GOOD) {
			result = TIMING.GOOD;
		} else {
			result = TIMING.BAD;
		}

		return result;
	}

	// x秒ごとに円が収縮を繰り返す
	void stretchCircle() {
		timer += Time.deltaTime;
		if (timer >= loop_time) {
			float amari = timer - loop_time;
			timer = 0;
			timer += amari;
			_audioCtrl.PlayKick ();
		}
		rate = timer / loop_time;
		rate -= 1.0f;
		_circle.transform.localScale = _circleScale * rate;
	}

	// BPMの設定に合わせて円のスピードを変える
	float getLoopTimeFromBPM(float pBpm) {
		// BPM=Beat Per Minute 1分間の拍の数
		// BPM120 → 120 beat / 60 sec → 2 beat / 1 sec
		// 1sec/2beat = loop_time
		// loop_timeは0.5f
		float lt = _BPM / 60;
		lt = 1 / lt;
		return lt;
	}
}
