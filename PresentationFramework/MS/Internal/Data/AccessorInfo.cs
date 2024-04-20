using System;

namespace MS.Internal.Data
{
	// Token: 0x020001FD RID: 509
	internal sealed class AccessorInfo
	{
		// Token: 0x060012B3 RID: 4787 RVA: 0x0014B982 File Offset: 0x0014A982
		internal AccessorInfo(object accessor, Type propertyType, object[] args)
		{
			this._accessor = accessor;
			this._propertyType = propertyType;
			this._args = args;
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x060012B4 RID: 4788 RVA: 0x0014B99F File Offset: 0x0014A99F
		internal object Accessor
		{
			get
			{
				return this._accessor;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x060012B5 RID: 4789 RVA: 0x0014B9A7 File Offset: 0x0014A9A7
		internal Type PropertyType
		{
			get
			{
				return this._propertyType;
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x060012B6 RID: 4790 RVA: 0x0014B9AF File Offset: 0x0014A9AF
		internal object[] Args
		{
			get
			{
				return this._args;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x060012B7 RID: 4791 RVA: 0x0014B9B7 File Offset: 0x0014A9B7
		// (set) Token: 0x060012B8 RID: 4792 RVA: 0x0014B9BF File Offset: 0x0014A9BF
		internal int Generation
		{
			get
			{
				return this._generation;
			}
			set
			{
				this._generation = value;
			}
		}

		// Token: 0x04000B4B RID: 2891
		private object _accessor;

		// Token: 0x04000B4C RID: 2892
		private Type _propertyType;

		// Token: 0x04000B4D RID: 2893
		private object[] _args;

		// Token: 0x04000B4E RID: 2894
		private int _generation;
	}
}
