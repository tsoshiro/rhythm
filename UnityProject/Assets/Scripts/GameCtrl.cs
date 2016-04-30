using UnityEngine;
using System.Collections;

public class GameCtrl : MonoBehaviour {
	[Header("Control Settings")]
	public AudioCtrl 	_audioCtrl;
	public EffectCtrl 	_effectCtrl;
	public TimeCtrl 	_timeCtrl;
	public PlayerCtrl	_playerCtrl;
	public EnemyCtrl	_enemyCtrl;

	[Header("Display Settings")]
	public GameObject _circle;
	public GameObject _targetCircle;

	public GameObject _cubeMoving;
	public GameObject _cubeTarget;
	Vector3 _cubeMovingPosition;

	public enum DISPLAY_MODE {
		CIRCLE,
		CUBE,
	}
	public	DISPLAY_MODE display_mode;


	public TextMesh _tapResultText;
	public TextMesh _ScoreText;
	public TextMesh _killCountLabel;

	[Header("Game Settings")]
	public float _BPM = 120;
	public enum TIMING {
		BAD,
		GOOD,
		GREAT,
		EXCELLENT,
		PERFECT,
	}

	// Game Logic
	int comboCount;
	int score = 0;
	int killCount = 0;

	[Header("Circle Display Logic")]
	Vector3 _movingCircleScale;

	[Header("Cube Display Logic")]
	float CUBE_WIDTH = 3f;
	Vector3 _cubeTargetScale;

	[Header("Debug Settings")]
	public int SCORE_VAL_BAD 		= 10;
	public int SCORE_VAL_GOOD 		= 30;
	public int SCORE_VAL_GREAT  	= 100;
	public int SCORE_VAL_EXCELLENT 	= 150;
	public int SCORE_VAL_PERFECT 	= 300;
		   
		   
	public float DIF_VAL_GOOD 		= 0.1f;
	public float DIF_VAL_GREAT  	= 0.05f;
	public float DIF_VAL_EXCELLENT 	= 0.01f;
	public float DIF_VAL_PERFECT 	= 0.001f;
		   
	public float TARGET_CIRCLE_SCALE_AMOUNT = 0.12f;
	public float TARGET_CUBE_SCALE_AMOUNT = 2f;
	public float SCALE_TIME 	= 0.2f;
	public float RESULT_TEXT_SCALE_AMOUNT = 0.2f;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;

		// Ctrl
		_timeCtrl.Init(this);

		//COMMON
		_movingCircleScale = _circle.transform.localScale;
		_cubeMovingPosition = _cubeMoving.transform.position;
		_cubeTargetScale = _cubeTarget.transform.localScale;

		// DISPLAY MODE INITIALIZATION
		if (display_mode == DISPLAY_MODE.CIRCLE) {
			enableCube (false);
		} else {
			//DISABLE
			enableCircle(false);
		}

		score = 0;
	}

	// Update is called once per frame
	void Update () 
	{
		// BPMに合わせて時間を動かす
		_timeCtrl.setLoopTimeFromBPM (_BPM);

		// 表示判定
		if (display_mode == DISPLAY_MODE.CIRCLE) {
			stretchCircle ();
		} else {
			moveCube ();
		}


		// タップを判定する
		if (Input.GetMouseButtonDown (0) ||
			Input.GetKeyDown(KeyCode.Space))
		{
			tap ();
		}
	}

	#region DISPLAY_MODE
	public void setMode(DISPLAY_MODE pMode) {
		display_mode = pMode;
	}

	public void setModeCircle() {
		display_mode = DISPLAY_MODE.CIRCLE;
		enableCircle (true);
		enableCube (false);
	}

	public void setModeBar() {
		display_mode = DISPLAY_MODE.CUBE;
		enableCube (true);
		enableCircle (false);
	}

	void enableCube(bool pFlg) {
		_cubeMoving.SetActive (pFlg);
		_cubeTarget.SetActive (pFlg);
	}

	void enableCircle(bool pFlg) {
		_circle.SetActive (pFlg);
		_targetCircle.SetActive (pFlg);
	}
	#endregion

	void tap() {
		// タップのタイミングが、Timerとどれくらいずれているかを検証。近さに応じてBAD/GOOD/GREAT/EXCELLENTの4段階評価を表示する

		// タップしたときのTimerの値を取得し、近さに応じて評価を出す
		TIMING tapResult = _timeCtrl.getTapResult();
		// コンボかどうかをカウントする
		if (tapResult == TIMING.EXCELLENT || tapResult == TIMING.GREAT) {
			comboCount++;
		} else {
			comboCount = 0;
		}
			
		// タップ演出
		PlaySE (AudioCtrl.SE_HAT);
		if (display_mode == DISPLAY_MODE.CIRCLE) {
			iTween.ScaleFrom (_targetCircle, Vector3.one * TARGET_CIRCLE_SCALE_AMOUNT, SCALE_TIME); 
		} else {
			iTween.ScaleFrom (_cubeTarget, _cubeTargetScale * TARGET_CUBE_SCALE_AMOUNT, SCALE_TIME); 
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

		// プレイヤーのPPTの値から敵に与えるダメージを算出
		float point = (float)addScore / 100 * _playerCtrl.getPPT ();
		sendPointToEnemy (point);
	}

	void sendPointToEnemy(float pPoint) {
		_enemyCtrl.hitPoint (pPoint);
	}
		
	public void killEnemy() {
		killCount++;
		_killCountLabel.text = "KILL COUNT: " + killCount;

		_playerCtrl.addCoin (100);
	}

	// x秒ごとに円が収縮を繰り返す
	void stretchCircle() {
		float rate = _timeCtrl.getGaugeRate ();

		// 大きい→小さい　と動くようにrateを逆にする
		_circle.transform.localScale = _movingCircleScale * (rate - 1.0f);
	}

	// 数秒ごとにバーが左から右い流れていく
	void moveCube() {
		float rate = _timeCtrl.getGaugeRate ();

		// 0 → 1を -3 → 3に変換し、x座標に代入
		rate *= CUBE_WIDTH * 2;
		rate -= CUBE_WIDTH;

		Vector3 pos = _cubeMovingPosition;
		pos.x = rate;

		_cubeMoving.transform.position = pos;
	}

	// Audio
	public void PlaySE(int pSeNumber) {
		_audioCtrl.PlaySE (pSeNumber);
	}
}