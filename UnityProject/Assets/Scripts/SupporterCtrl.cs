using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SupporterCtrl : MonoBehaviour {
	public GameCtrl _gameCtrl;
	List<SupporterMaster> _supporterMasterList = new List<SupporterMaster>();
	List<SupporterLevelMaster> _supporterLevelMasterList = new List<SupporterLevelMaster>();

	// Use this for initialization
	void Start () {
		_gameCtrl = this.transform.parent.GetComponent<GameCtrl> ();


		// ユーザーデータからSupporterクラスを生成・管理
	}

	// Supporter関連のマスターデータ取得
	void initData() {
		var entityMasterTable = new SupporterMasterTable ();
		entityMasterTable.Load ();
		foreach (var entityMaster in entityMasterTable.All) {
			_supporterMasterList.Add (entityMaster);
		}
		var entityLevelMasterTable = new SupporterLevelMasterTable ();
		entityLevelMasterTable.Load ();
		foreach (var entityMaster in entityLevelMasterTable.All) {
			_supporterLevelMasterList.Add (entityMaster);
		}
	}
}
