using UnityEngine;
using System.Collections;

public class EnemyMasterTable : MasterTableBase<EnemyMaster>
{
	public void Load() {
		Load(convertClassToFilePath (this.GetType ().Name));
	}
}

public class EnemyMaster : MasterBase
{
	public string id { get; private set; }
	public string name { get; private set; }
	public int base_hp { get; private set; }
	public string image_path { get; private set;}
}
