using UnityEngine;
using System.Collections;

public class SupporterMasterTable : MasterTableBase<SupporterMaster> {
	public void Load() {
		Load(convertClassToFilePath (this.GetType ().Name));
	}
}

public class SupporterMaster : MasterBase
{
	public string id { get; private set; }
	public string name { get; private set; }
	public float atk_interval { get; private set; }
	public string image_path { get; private set; }
	public string growthType {get; private set; }
	public int base_coin {get; private set; }
}
