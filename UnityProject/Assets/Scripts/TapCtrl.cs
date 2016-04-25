using UnityEngine;
using System.Collections;

public class TapCtrl : MonoBehaviour {
	public AudioCtrl _audioCtrl;

	public GameObject _circle;
	public GameObject _targetCircle;
	public TextMesh _tapResultText;
	public TextMesh _ScoreText;
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
		PERFECT,
	}

	Vector3 _movingCircleScale;

	int score = 0;

	// SETTINGS
	public int SCORE_VAL_BAD 		= 50;
	public int SCORE_VAL_GOOD 		= 100;
	public int SCORE_VAL_GREAT  	= 200;
	public int SCORE_VAL_EXCELLENT 	= 300;
	public int SCORE_VAL_PERFECT 	= 400;


	public float DIF_VAL_GOOD 		= 0.1f;
	public float DIF_VAL_GREAT  	= 0.05f;
	public float DIF_VAL_EXCELLENT 	= 0.01f;
	public float DIF_VAL_PERFECT 	= 0.001f;

	public float TARGET_CIRCLE_SCALE_AMOUNT = 0.12f;
	public float SCALE_TIME 	= 0.1f;
	public float RESULT_TEXT_SCALE_AMOUNT = 0.2f;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
		_movingCircleScale = _circle.transform.localScale;
		score = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		loop_time = getLoopTimeFromBPM (_BPM);
		stretchCircle ();
		// タップを判定する
		if (Input.GetMouseButtonDown (0) ||
			Input.GetKeyDown(KeyCode.Space))
		{
			tap ();
		}
	}

	void tap() {
		// タップのタイミングが、Timerとどれくらいずれているかを検証。近さに応じてBAD/GOOD/GREAT/EXCELLENTの4段階評価を表示する
		_audioCtrl.PlayHat ();

		// タップしたときのTimerの値を取得し、近さに応じて評価を出す
		TIMING tapResult = getTapResult(Mathf.Abs(rate));
		iTween.ScaleFrom (_targetCircle, Vector3.one * TARGET_CIRCLE_SCALE_AMOUNT, SCALE_TIME); 

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
		int addScore = 0;
		float asc = 0.0f; // コンボ加算用float

		switch (pTapResult) {
		case TIMING.PERFECT:
			resultText = "PERFECT!\n" + comboCount + " COMBO!";
			asc = (float)SCORE_VAL_PERFECT * comboCount * 1.1f;
			addScore += Mathf.RoundToInt(asc);
			break;
		case TIMING.EXCELLENT:
			resultText = "EXCELLENT!\n"+comboCount+" COMBO!";
			asc = (float)SCORE_VAL_EXCELLENT * comboCount * 1.1f;
			addScore += Mathf.RoundToInt(asc);
			break;
		case TIMING.GREAT:
			resultText = "GREAT!\n"+comboCount+" COMBO!";
			asc = (float)SCORE_VAL_GOOD * comboCount * 1.1f;
			addScore += Mathf.RoundToInt(asc);
			break;
		case TIMING.GOOD:
			resultText = "GOOD!";
			addScore += SCORE_VAL_GOOD;
			break;
		case TIMING.BAD:
			resultText = "BAD!";
			addScore += SCORE_VAL_BAD;
			break;
		default:
			resultText = "BAD!";
			addScore += SCORE_VAL_BAD;
			break;
		}
		score += addScore;
		_tapResultText.text = resultText;
		_ScoreText.text = "SCORE\n"+score+"pt";
		iTween.ScaleFrom (_tapResultText.gameObject, Vector3.one * RESULT_TEXT_SCALE_AMOUNT, SCALE_TIME);
	}

	// タップしたタイミングとターゲットタイミングとの差に応じて評価を返す
	TIMING getTapResult(float tapRate) {
		float targetRate = 0.5f;
		TIMING result = TIMING.BAD;

		float difference = Mathf.Abs (tapRate - targetRate);
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
		_circle.transform.localScale = _movingCircleScale * rate;
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
