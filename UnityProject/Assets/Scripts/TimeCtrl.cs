using UnityEngine;
using System.Collections;

public class TimeCtrl : MonoBehaviour {
	GameCtrl _gameCtrl;

	[SerializeField]
	float loopTime;
	float timer;
	float rate;

	public void Init(GameCtrl pGameCtrl) {
		_gameCtrl = pGameCtrl;
	}

	// BPMの設定に合わせて1周スピードを変える
	public void setLoopTimeFromBPM(float pBpm) {
		// BPM=Beat Per Minute 1分間の拍の数
		// BPM120 → 120 beat / 60 sec → 2 beat / 1 sec
		// 1sec/2beat = loop_time
		// loop_timeは0.5f
		float lt = pBpm / 60;
		lt = 1 / lt;

		loopTime = lt;
	}

	public float getGaugeRate() {
		timer += Time.deltaTime;
		if (timer >= loopTime) {
			float amari = timer - loopTime;
			timer = 0;
			timer += amari;
			_gameCtrl.PlaySE(AudioCtrl.SE_KICK);
		}
		rate = timer / loopTime;

		return rate;
	}

	// タップしたタイミングとターゲットタイミングとの差に応じて評価を返す
	public GameCtrl.TIMING getTapResult() {
		float targetRate = 0.5f;
		GameCtrl.TIMING result = GameCtrl.TIMING.BAD;

		float difference = Mathf.Abs (rate - targetRate);
		Debug.Log ("rate : " + rate);
		if (difference <= _gameCtrl.DIF_VAL_EXCELLENT) {
			result = GameCtrl.TIMING.EXCELLENT;
		} else if (difference <= _gameCtrl.DIF_VAL_GREAT) {
			result = GameCtrl.TIMING.GREAT;
		} else if (difference <= _gameCtrl.DIF_VAL_GOOD) {
			result = GameCtrl.TIMING.GOOD;
		} else {
			result = GameCtrl.TIMING.BAD;
		}

		return result;
	}

	// 時間を計測するロジック
	// BPMから一拍の時間を算出
	// 取得時間を計測
	public void clockTime() {
		if (targetTime <= Time.realtimeSinceStartup) { //
			float leftOver = Time.realtimeSinceStartup - targetTime;
			initBeat(leftOver);
			rate = leftOver / loopTime;
		}
		float now = Time.realtimeSinceStartup - startTime;
		rate = now / loopTime;
	}

	public float getRate() {
		return rate;
	}

	public float getRateFromTime() {
		if (targetTime <= Time.realtimeSinceStartup) { //
			float leftOver = Time.realtimeSinceStartup - targetTime;
			initBeat(leftOver);
			return leftOver / loopTime;
		}
		float now = Time.realtimeSinceStartup - startTime;
		rate = now / loopTime;

		return rate;
	}
		
	float startTime;
	float targetTime;

	public void initBeat(float pLeftTime = 0) {
		startTime = Time.realtimeSinceStartup + pLeftTime;
		targetTime = startTime + loopTime;
		_gameCtrl.PlaySE (AudioCtrl.SE_KICK);
	}
}
