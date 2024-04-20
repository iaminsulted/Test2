using System;
using System.Collections.Generic;

namespace System.Windows.Baml2006
{
	// Token: 0x0200040E RID: 1038
	internal struct Baml6ConstructorInfo
	{
		// Token: 0x06002D2E RID: 11566 RVA: 0x001AB520 File Offset: 0x001AA520
		public Baml6ConstructorInfo(List<Type> types, Func<object[], object> ctor)
		{
			this._types = types;
			this._constructor = ctor;
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x06002D2F RID: 11567 RVA: 0x001AB530 File Offset: 0x001AA530
		public List<Type> Types
		{
			get
			{
				return this._types;
			}
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06002D30 RID: 11568 RVA: 0x001AB538 File Offset: 0x001AA538
		public Func<object[], object> Constructor
		{
			get
			{
				return this._constructor;
			}
		}

		// Token: 0x04001BA8 RID: 7080
		private List<Type> _types;

		// Token: 0x04001BA9 RID: 7081
		private Func<object[], object> _constructor;
	}
}
