using System;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000165 RID: 357
	internal class ManagedFullPropSpec
	{
		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000BEC RID: 3052 RVA: 0x0012EEBA File Offset: 0x0012DEBA
		internal Guid Guid
		{
			get
			{
				return this._guid;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000BED RID: 3053 RVA: 0x0012EEC2 File Offset: 0x0012DEC2
		internal ManagedPropSpec Property
		{
			get
			{
				return this._property;
			}
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x0012EECA File Offset: 0x0012DECA
		internal ManagedFullPropSpec(Guid guid, uint propId)
		{
			this._guid = guid;
			this._property = new ManagedPropSpec(propId);
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x0012EEE5 File Offset: 0x0012DEE5
		internal ManagedFullPropSpec(FULLPROPSPEC nativePropSpec)
		{
			this._guid = nativePropSpec.guid;
			this._property = new ManagedPropSpec(nativePropSpec.property);
		}

		// Token: 0x04000906 RID: 2310
		private Guid _guid;

		// Token: 0x04000907 RID: 2311
		private ManagedPropSpec _property;
	}
}
