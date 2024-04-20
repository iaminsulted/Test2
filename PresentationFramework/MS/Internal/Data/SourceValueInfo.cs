using System;
using MS.Utility;

namespace MS.Internal.Data
{
	// Token: 0x02000239 RID: 569
	internal struct SourceValueInfo
	{
		// Token: 0x06001595 RID: 5525 RVA: 0x001556AB File Offset: 0x001546AB
		public SourceValueInfo(SourceValueType t, DrillIn d, string n)
		{
			this.type = t;
			this.drillIn = d;
			this.name = n;
			this.paramList = null;
			this.propertyName = null;
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x001556D0 File Offset: 0x001546D0
		public SourceValueInfo(SourceValueType t, DrillIn d, FrugalObjectList<IndexerParamInfo> list)
		{
			this.type = t;
			this.drillIn = d;
			this.name = null;
			this.paramList = list;
			this.propertyName = null;
		}

		// Token: 0x04000C20 RID: 3104
		public SourceValueType type;

		// Token: 0x04000C21 RID: 3105
		public DrillIn drillIn;

		// Token: 0x04000C22 RID: 3106
		public string name;

		// Token: 0x04000C23 RID: 3107
		public FrugalObjectList<IndexerParamInfo> paramList;

		// Token: 0x04000C24 RID: 3108
		public string propertyName;
	}
}
