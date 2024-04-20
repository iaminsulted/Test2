using System;

namespace System.Windows.Documents.DocumentStructures
{
	// Token: 0x02000702 RID: 1794
	public class NamedElement : BlockElement
	{
		// Token: 0x170015B4 RID: 5556
		// (get) Token: 0x06005DD0 RID: 24016 RVA: 0x0028DEB8 File Offset: 0x0028CEB8
		// (set) Token: 0x06005DD1 RID: 24017 RVA: 0x0028DEC0 File Offset: 0x0028CEC0
		public string NameReference
		{
			get
			{
				return this._reference;
			}
			set
			{
				this._reference = value;
			}
		}

		// Token: 0x0400316A RID: 12650
		private string _reference;
	}
}
