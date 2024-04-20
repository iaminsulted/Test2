using System;

namespace System.Windows
{
	// Token: 0x020003B2 RID: 946
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class StyleTypedPropertyAttribute : Attribute
	{
		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06002622 RID: 9762 RVA: 0x0018B440 File Offset: 0x0018A440
		// (set) Token: 0x06002623 RID: 9763 RVA: 0x0018B448 File Offset: 0x0018A448
		public string Property
		{
			get
			{
				return this._property;
			}
			set
			{
				this._property = value;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06002624 RID: 9764 RVA: 0x0018B451 File Offset: 0x0018A451
		// (set) Token: 0x06002625 RID: 9765 RVA: 0x0018B459 File Offset: 0x0018A459
		public Type StyleTargetType
		{
			get
			{
				return this._styleTargetType;
			}
			set
			{
				this._styleTargetType = value;
			}
		}

		// Token: 0x040011F1 RID: 4593
		private string _property;

		// Token: 0x040011F2 RID: 4594
		private Type _styleTargetType;
	}
}
