using System;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000222 RID: 546
	internal class DynamicObjectAccessor
	{
		// Token: 0x0600147B RID: 5243 RVA: 0x001523FC File Offset: 0x001513FC
		protected DynamicObjectAccessor(Type ownerType, string propertyName)
		{
			this._ownerType = ownerType;
			this._propertyName = propertyName;
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x0600147C RID: 5244 RVA: 0x00152412 File Offset: 0x00151412
		public Type OwnerType
		{
			get
			{
				return this._ownerType;
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x0600147D RID: 5245 RVA: 0x0015241A File Offset: 0x0015141A
		public string PropertyName
		{
			get
			{
				return this._propertyName;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x0600147E RID: 5246 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x0600147F RID: 5247 RVA: 0x00152422 File Offset: 0x00151422
		public Type PropertyType
		{
			get
			{
				return typeof(object);
			}
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x0015242E File Offset: 0x0015142E
		public static string MissingMemberErrorString(object target, string name)
		{
			return SR.Get("PropertyPathNoProperty", new object[]
			{
				target,
				"Items"
			});
		}

		// Token: 0x04000BD3 RID: 3027
		private Type _ownerType;

		// Token: 0x04000BD4 RID: 3028
		private string _propertyName;
	}
}
