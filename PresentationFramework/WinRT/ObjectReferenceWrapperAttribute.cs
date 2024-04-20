using System;
using System.ComponentModel;

namespace WinRT
{
	// Token: 0x0200009F RID: 159
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal sealed class ObjectReferenceWrapperAttribute : Attribute
	{
		// Token: 0x06000240 RID: 576 RVA: 0x000F99C4 File Offset: 0x000F89C4
		public ObjectReferenceWrapperAttribute(string objectReferenceField)
		{
			this.ObjectReferenceField = objectReferenceField;
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000241 RID: 577 RVA: 0x000F99D3 File Offset: 0x000F89D3
		public string ObjectReferenceField { get; }
	}
}
