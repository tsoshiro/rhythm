using UnityEngine;
using System.Collections;

public class EnemyLevelMasterTable : MasterTableBase<EnemyLevelMaster>
{
	public void Load() {
		Load(convertClassToFilePath (this.GetType ().Name));
	}
}

public class EnemyLevelMaster : MasterBase
{
	public string id { get; private set; }
	public int level { get; private set;}
	public float multiple_rate{ get; private set; }
}
