using UnityEngine;
using System.Collections;

public class TimeCtrl : MonoBehaviour {
	GameCtrl _gameCtrl;

	float loopTime;
	float timer;

	public void Init(GameCtrl pGameCtrl) {
		_gameCtrl = pGameCtrl;
	}

	// BPMの設定に合わせて1周スピードを変える
	public float getLoopTimeFromBPM(float pBpm) {
		// BPM=Beat Per Minute 1分間の拍の数
		// BPM120 → 120 beat / 60 sec → 2 beat / 1 sec
		// 1sec/2beat = loop_time
		// loop_timeは0.5f
		float lt = pBpm / 60;
		lt = 1 / lt;
		return lt;
	}

	public float getGaugeRate(float pRate, float pTimer, float pLoopTime) {
		pTimer += Time.deltaTime;
		if (pTimer >= pLoopTime) {
			float amari = pTimer - pLoopTime;
			pTimer = 0;
			pTimer += amari;
			_gameCtrl.PlaySE(AudioCtrl.SE_KICK);
		}
		pRate = pTimer / pLoopTime;
		pRate -= 1.0f;

		return pRate;
	}

	// タップしたタイミングとターゲットタイミングとの差に応じて評価を返す
	public GameCtrl.TIMING getTapResult(float tapRate) {
		float targetRate = 0.5f;
		GameCtrl.TIMING result = GameCtrl.TIMING.BAD;

		float difference = Mathf.Abs (tapRate - targetRate);
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

}
