using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameCtrl : MonoBehaviour {
	[Header("Control Settings")]
	public AudioCtrl 		_audioCtrl;
	public EffectCtrl 		_effectCtrl;
	public TimeCtrl 		_timeCtrl;
	public PlayerCtrl		_playerCtrl;
	public EnemyCtrl		_enemyCtrl;
	public SupporterCtrl	_supporterCtrl;

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
	public TextMesh _maxComboLabel;
	public TextMesh _killCountLabel;
	public TextMesh _damageText;

	// サポーターリスト
	public GameObject _supporterScrollView;

	[Header("Game Settings")]
	public GAME_MODE gameMode = GAME_MODE.STAND_BY;
	public enum GAME_MODE
	{
		STAND_BY,
		PLAY,
		PAUSE,
		DEBUG,
	}

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
	int maxCombo = 0;

	[Header("Circle Display Logic")]
	Vector3 _movingCircleScale;

	[Header("Cube Display Logic")]
	float CUBE_WIDTH = 3f;
	Vector3 _cubeTargetScale;

	[Header("Debug Settings")]
	public Text fpsDisplay;

	public GameObject debugControls;
	public GameObject debugPanel;
	public Slider	debugBPM;
	public Slider	debugGood;
	public Slider	debugGreat;
	public Slider	debugExcellent;
	public Slider 	debugPerfect;

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

	// 表示系
	public TextMesh _userLvLabel;
	public TextMesh _userPPTLabel;
	public Text _nextLevelLabel;
	public Text _purchaseBtnLabel;
	public Button _openSupporterListButton;

	UserData _userData;
	UserData _nextUserData;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
		QualitySettings.vSyncCount = 0;

		// Ctrl
		_timeCtrl.Init(this);
		_tapCtrl = this.GetComponentInChildren<TapCtrl>();

		// COMMON
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

		// 初期化処理
		initUserData ();
		showUserData ();
		score = 0;

		gameMode = GAME_MODE.PLAY;
	}

	void initUserData() {
		// ユーザーレベル取得
		int userLv = PlayerPrefs.GetInt (Const.PREF_USER_LEVEL);
		if (userLv == 0) {
			userLv = 1;
		}
		_userData = new UserData (userLv);
		_playerCtrl.setUserData (_userData);

		int nextUserLv = userLv + 1;
		_nextUserData = new UserData (nextUserLv);

		// サポーター情報取得
		setSupporters ();

		// 所持コイン取得
		int userCoin = PlayerPrefs.GetInt (Const.PREF_USER_COIN);
		_playerCtrl.addCoin (userCoin);

		// 最大コンボ数取得
		maxCombo = PlayerPrefs.GetInt(Const.PREF_USER_MAX_COMBO);

		// 討伐数取得
		killCount = PlayerPrefs.GetInt (Const.PREF_KILL_COUNT);

		// 敵取得
		_enemyCtrl.initEnemyFromDB (killCount);
	}

	// サポーター情報をセット
	void setSupporters() {
		_supporterCtrl.init ();
	}

	void showUserData() {
		//_userLvLabel.text = "Lv: "+_userData.level;
		//_userPPTLabel.text = "PPT: "+_userData.pointPerTap;

		//_nextLevelLabel.text = "Next Lv : " + _nextUserData.level + "\n"
		//				+ "PPT : " + _nextUserData.pointPerTap;

		_nextLevelLabel.text = "LV:" + _userData.level + "\n" + "PPT: " + _userData.pointPerTap;
		_purchaseBtnLabel.text = "LEVEL UP!\n" + _userData.nextLevelCoin + " COIN";

		//_maxComboLabel.text = "MaxCombo:" + maxCombo;
		//_killCountLabel.text = "Kill:" + killCount;
	}

	public void purchasePlayerLevel() {
		int useCoinValue = _userData.nextLevelCoin;
		if (_playerCtrl.getCoin () < useCoinValue) {
			Debug.Log ("CAN'T BUY");
			return;
		}
		_userData.setUserData (_userData.level + 1);
		_nextUserData.setUserData (_nextUserData.level + 1);

		_playerCtrl.setUserData (_userData);
		_playerCtrl.useCoin (useCoinValue);

		// LvUP音を再生
		PlaySE(Const.SE_LV_UP);

		showUserData ();
	}

	// サポーターの解放/レベルアップリストを開く
	public void openSupporterList() {
		gameMode = GAME_MODE.PAUSE;
		_supporterScrollView.SetActive(true);

		_openSupporterListButton.gameObject.SetActive(false);
	}

	// サポーターの解放/レベルップリストを閉じる
	public void closeSupporterList() {
		_supporterScrollView.SetActive(false);
		gameMode = GAME_MODE.PLAY;

		_openSupporterListButton.gameObject.SetActive(true);
	}
		
	// サポーターを解放/レベルアップ(引数にはサポーターのID)
	public void purchaseSupporter(int pId) {
		// 購入チェック
		int useCoinValue = _supporterCtrl.getSupporterCost(pId);
		Debug.Log("COIN:" + _playerCtrl.getCoin() + " supporterCost:" + useCoinValue);
		if (_playerCtrl.getCoin () < useCoinValue) {
			Debug.Log ("CAN'T BUY");
			return;
		}

		if (_supporterCtrl.raiseSupporterLevel(pId))
		{
			Debug.Log("SUCCESS");
		} else {
			Debug.Log("FAILED");
			return;
		}

		// コイン消費
		_playerCtrl.useCoin(useCoinValue);

		// セーブ


		// サポーターを解放済みならレベルップ/そうでないなら解放
		//if (isAvailableSupporter (pId)) {
		//	purchaseSupporterLevel (pId);
		//} else {
		//	purchaseNewSupporter (pId);
		//}
	}

	void purchaseNewSupporter(int pId) {
		// サポータークラスのインスタンスを生成
		// 初期ステータスを設定

		// サポーターリストに追加する
	}

	void purchaseSupporterLevel(int pId) {
		// サポーターリストから該当のサポーターが何番目かを取得

		// 次のレベルのデータを取得

		// サポーターのステータスを書き換える
	}

	// サポーターは解放済みかどうか
	bool isAvailableSupporter(int pId) {
		return true;
	}

	void saveData() {
		PlayerPrefs.SetInt (Const.PREF_USER_LEVEL, _userData.level);
		PlayerPrefs.SetInt (Const.PREF_KILL_COUNT, killCount);
		PlayerPrefs.SetInt (Const.PREF_USER_COIN, _playerCtrl.getCoin ());
		PlayerPrefs.SetInt (Const.PREF_ENMEY_NUM, _enemyCtrl.getEnemyNum());
		PlayerPrefs.SetInt (Const.PREF_USER_MAX_COMBO, maxCombo);
	}

	public void resetSaveData() {
		_userData.setUserData (1);
		_nextUserData.setUserData (2);
		showUserData ();

		killCount = 0;
		_killCountLabel.text = "KILL COUNT: " + killCount;

		_playerCtrl.setCoin (0);

		_enemyCtrl.resetEnemyMAXHP ();
		_enemyCtrl.initEnemyFromDB (killCount);

		_supporterCtrl.resetSupporter();

		PlayerPrefs.SetInt (Const.PREF_USER_LEVEL, 1);
		PlayerPrefs.SetInt (Const.PREF_KILL_COUNT, killCount);
		PlayerPrefs.SetInt (Const.PREF_USER_COIN, _playerCtrl.getCoin ());
		PlayerPrefs.SetInt (Const.PREF_ENMEY_NUM, 0);
	}

	void FixedUpdate() {
		if (gameMode != GAME_MODE.PLAY &&
		    gameMode != GAME_MODE.PAUSE) {
			return;
		}

		// BPMに合わせて時間を動かす
		_timeCtrl.setLoopTimeFromBPM (_BPM);	

		_timeCtrl.clockTime ();
	}

	// Update is called once per frame
	void Update () 
	{
		if (gameMode == GAME_MODE.PLAY ||
		    gameMode == GAME_MODE.PAUSE) {
			UpdatePlay();
		}
	}

	void UpdatePlay() {
		// 表示判定
		if (display_mode == DISPLAY_MODE.CIRCLE) {
			stretchCircle ();
		} else {
			moveCube ();
		}

		if (gameMode == GAME_MODE.PLAY)
		{
			// タップを判定する
			if (Input.GetMouseButtonDown(0) ||
				Input.GetKeyDown(KeyCode.Space))
			{
				Vector2 _touchPosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
				if (isTapCorrectArea(_touchPosition))
				{
					tap();
				}

			}

//#elif UNITY_IOS || UNITY_ANDROID
//			if (Input.touchCount > 0) {
//				Touch singleTouch = Input.GetTouch (0);
//				if (singleTouch.phase == TouchPhase.Began) {
//					Vector2 _touchPosition = new Vector2(singleTouch.position.x, Screen.height - singleTouch.position.y);
//			if (isTapCorrectArea(_touchPosition)) {
//						tap ();
//					}
//				}
//			}
//#endif
		}
		getSupporterValues ();

		showFPSDisplay ();		
	}

	#region SETTINGS
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

	public void openSettings() {
		gameMode = GAME_MODE.DEBUG;
		debugControls.SetActive (true);

		debugBPM.value 		= _BPM;
		debugGood.value		= DIF_VAL_GOOD;
		debugGreat.value 	= DIF_VAL_GREAT;
		debugExcellent.value= DIF_VAL_EXCELLENT;
		debugPerfect.value 	= DIF_VAL_PERFECT;
	}

	public void completeSettings() {
		_BPM				= debugBPM.value;
		DIF_VAL_GOOD 		= debugGood.value;
		DIF_VAL_GREAT		= debugGreat.value;
		DIF_VAL_EXCELLENT	= debugExcellent.value;
		DIF_VAL_PERFECT		= debugPerfect.value;

		debugControls.SetActive(false);
		gameMode = GAME_MODE.PLAY;
	}

	// デバッグ用にFPSを出力
	void showFPSDisplay() {
		float fps = 1f / Time.deltaTime;
		fpsDisplay.text = fps.ToString ("F2");
	}
	#endregion

	TapCtrl _tapCtrl;

	// タップした位置が所定の範囲内かを判定する
	bool isTapCorrectArea(Vector2 pTouchPosition) {
		Rect rect = _tapCtrl.getTapArea();
		if (pTouchPosition.x >= rect.xMin &&
		    pTouchPosition.x < rect.xMax &&
		    pTouchPosition.y >= rect.yMin &&
		    pTouchPosition.y < rect.yMax)
		{
			return true;
		}
		return false;
	}

	void tap() {
		// タップのタイミングが、Timerとどれくらいずれているかを検証。近さに応じてBAD/GOOD/GREAT/EXCELLENTの4段階評価を表示する

		// タップしたときのTimerの値を取得し、近さに応じて評価を出す
		TIMING tapResult = _timeCtrl.getTapResult();
		// コンボかどうかをカウントする
		if (tapResult == TIMING.EXCELLENT || tapResult == TIMING.GREAT) {
			comboCount++;
		} else {
			// コンボ失敗なら、最大コンボかチェック
			checkMaxCombo();
			comboCount = 0;
		}
			
		// タップ演出
		if (display_mode == DISPLAY_MODE.CIRCLE) {
			iTween.ScaleFrom (_targetCircle, Vector3.one * TARGET_CIRCLE_SCALE_AMOUNT, SCALE_TIME); 
		} else {
			iTween.ScaleFrom (_cubeTarget, _cubeTargetScale * TARGET_CUBE_SCALE_AMOUNT, SCALE_TIME); 
		}

		// 評価による処理振り分け
		showTapResult(tapResult);
	}

	void checkMaxCombo() {
		if (comboCount >= maxCombo) {
			// 最大コンボ数を更新
			maxCombo = comboCount;
			showUserData ();
			saveData();
		}
	}

	void showTapResult(TIMING pTapResult) {
		string resultText = "";
		int seNumber = Const.SE_HAT_BAD;
		int addScore = 0;
		float asc = 0.0f; // コンボ加算用float

		// 遅すぎか早すぎか判定し、早すぎなら << 遅すぎなら >> と表示する。
		bool isSlow = (_timeCtrl.getLastDifference () > 0);
		string slowSign = "<<";
		if (isSlow) {
			slowSign = ">>";
		}

		switch (pTapResult) {
		case TIMING.PERFECT:
			resultText = "PERFECT!";
			asc = getComboBonus (SCORE_VAL_PERFECT);
			seNumber = Const.SE_HAT_EXCELLENT;
			break;
		case TIMING.EXCELLENT:
			resultText = "EXCELLENT!";
			asc = getComboBonus (SCORE_VAL_PERFECT);
			seNumber = Const.SE_HAT_EXCELLENT;
			break;
		case TIMING.GREAT:
			resultText = "GREAT!";
			asc = getComboBonus (SCORE_VAL_PERFECT);
			seNumber = Const.SE_HAT_GREAT;
			break;
		case TIMING.GOOD:
			resultText = "GOOD!";
			addScore += SCORE_VAL_GOOD;
			seNumber = Const.SE_HAT_GOOD;
			break;
		case TIMING.BAD:
			resultText = "BAD!";
			seNumber = Const.SE_HAT_BAD;
			addScore += SCORE_VAL_BAD;
			break;
		}

		//評価によって音を変えて鳴らす
		_audioCtrl.PlaySE(seNumber);

		// 表記テキストの装飾
		if (isSlow) {
			resultText = resultText + slowSign;
		} else {
			resultText = slowSign + resultText;
		}

		// GREAT以上であればコンボ表記・エフェクト発生・コンボボーナス
		if (pTapResult == TIMING.PERFECT || pTapResult == TIMING.EXCELLENT || pTapResult == TIMING.GREAT) {
			resultText += "\n" + comboCount + " COMBO!";
			_effectCtrl.showEffect ();
			addScore += Mathf.RoundToInt (asc);
		}

		// スコア加算
		score += addScore;
		_tapResultText.text = resultText;
		_ScoreText.text = "S:"+score;
		iTween.ScaleFrom (_tapResultText.gameObject, Vector3.one * RESULT_TEXT_SCALE_AMOUNT, SCALE_TIME);

		// プレイヤーのPPTの値から敵に与えるダメージを算出
		float point = (float)addScore / 100 * _playerCtrl.getPPT ();
		sendPointToEnemy (point);
	}

	float getComboBonus(int pResultValue) {
		float addPercentage = (100 + (float)comboCount) / 100; // 1コンボあたり1%スコアがプラスされる
		return (float)pResultValue * addPercentage;
	}

	void sendPointToEnemy(float pPoint, bool pIsPlayer = true) {
		_enemyCtrl.hitPoint (pPoint);

		// サポーター攻撃であればダメージ表示は行わない
		if (!pIsPlayer) {
			return;
		}
		_damageText.text = ""+Mathf.RoundToInt(pPoint);
		_damageText.GetComponent<AutoFade> ().resetColor ();
		iTween.ScaleFrom (_damageText.gameObject, Vector3.one * RESULT_TEXT_SCALE_AMOUNT, SCALE_TIME);
	}
		
	public void killEnemy() {
		killCount++;
		_killCountLabel.text = "KILL COUNT: " + killCount;
		_playerCtrl.addCoin (_enemyCtrl.getDropCoinValue());

		_enemyCtrl.spawnEnemy (killCount);

		// 敵撃退音
		PlaySE (Const.SE_KILL_ENEMY);

		saveData ();
	}

	// サポーターのポイントを取得し、反映させる
	void getSupporterValues() {
		sendPointToEnemy (_supporterCtrl.getSupporterValues (), false);
	}

	#region Move Meters
	// x秒ごとに円が収縮を繰り返す
	void stretchCircle() {
		float rate = _timeCtrl.getGaugeRate ();

		// 大きい→小さい　と動くようにrateを逆にする
		_circle.transform.localScale = _movingCircleScale * (rate - 1.0f);
	}

	// 数秒ごとにバーが左から右い流れていく
	void moveCube() {
		float rate = _timeCtrl.getRate();

		// 0 → 1を -3 → 3に変換し、x座標に代入
		rate *= CUBE_WIDTH * 2;
		rate -= CUBE_WIDTH;

		Vector3 pos = _cubeMovingPosition;
		pos.x = rate;

		_cubeMoving.transform.position = pos;
	}
	#endregion

	#region Audio
	public void PlaySE(int pSeNumber) {
		_audioCtrl.PlaySE (pSeNumber);
	}

	// Interface
	int beatNum = 0;
	public bool isPlaySnare() {
		if (comboCount >= 4) {
			if (beatNum == 1) {
				beatNum = 0;
				return true;			
			}
			beatNum++;
		}
		return false;
	}
	#endregion
}