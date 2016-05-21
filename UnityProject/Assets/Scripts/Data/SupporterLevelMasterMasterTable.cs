using UnityEngine;
using System.Collections;

public class SupporterLevelMasterTable : MasterTableBase<SupporterLevelMaster>{
	public void Load() {
		Load(convertClassToFilePath (this.GetType ().Name));
	}
}

public class SupporterLevelMaster : MasterBase {
	public string id { get; private set; }
	public int level { get; private set; }
	public string growth_type { get; private set; }
	public float multiple_rate_ppa { get; private set; }
	public float multiple_rate_coin { get; private set; }
}
