﻿using UnityEngine;
using System.Collections;

public class GameCtrl : MonoBehaviour {
	//Ctrl Settings
	public AudioCtrl 	_audioCtrl;
	public EffectCtrl 	_effectCtrl;
	public TimeCtrl 	_timeCtrl;

	// Display Settings
	public GameObject _circle;
	public GameObject _targetCircle;
	public TextMesh _tapResultText;
	public TextMesh _ScoreText;

	// Game Settings
	public float _BPM = 120;
	public enum TIMING {
		BAD,
		GOOD,
		GREAT,
		EXCELLENT,
		PERFECT,
	}

	// Game Logic
	float loop_time = 1.0f;
	float timer = 0.0f;
	float rate; // 割合。rate = 0.5fがドンピシャタイミング
	int comboCount;
	int score = 0;

	// Display Logic
	Vector3 _movingCircleScale;


	// DEBUG SETTINGS
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

		// Ctrl
		_timeCtrl.Init(this);

		_movingCircleScale = _circle.transform.localScale;
		score = 0;
	}

	// Update is called once per frame
	void Update () 
	{
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

		// タップしたときのTimerの値を取得し、近さに応じて評価を出す
		TIMING tapResult = _timeCtrl.getTapResult(Mathf.Abs(rate));
		// コンボかどうかをカウントする
		if (tapResult == TIMING.EXCELLENT || tapResult == TIMING.GREAT) {
			comboCount++;
		} else {
			comboCount = 0;
		}
			
		// タップ演出
		PlaySE (AudioCtrl.SE_HAT);
		iTween.ScaleFrom (_targetCircle, Vector3.one * TARGET_CIRCLE_SCALE_AMOUNT, SCALE_TIME); 


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
			addScore += Mathf.RoundToInt (asc);
			_effectCtrl.showEffect ();
			break;
		case TIMING.EXCELLENT:
			resultText = "EXCELLENT!\n"+comboCount+" COMBO!";
			asc = (float)SCORE_VAL_EXCELLENT * comboCount * 1.1f;
			addScore += Mathf.RoundToInt(asc);
			_effectCtrl.showEffect ();
			break;
		case TIMING.GREAT:
			resultText = "GREAT!\n"+comboCount+" COMBO!";
			asc = (float)SCORE_VAL_GOOD * comboCount * 1.1f;
			addScore += Mathf.RoundToInt(asc);
			_effectCtrl.showEffect ();
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

	// x秒ごとに円が収縮を繰り返す
	void stretchCircle() {
		loop_time = _timeCtrl.getLoopTimeFromBPM (_BPM);

		rate = _timeCtrl.getGaugeRate (rate, timer, loop_time);
		_circle.transform.localScale = _movingCircleScale * rate;
	}

	// Audio
	public void PlaySE(int pSeNumber) {
		_audioCtrl.PlaySE (pSeNumber);
	}
}
