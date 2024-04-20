using System;

namespace System.Windows.Documents
{
	// Token: 0x020006B0 RID: 1712
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class TextElementEditingBehaviorAttribute : Attribute
	{
		// Token: 0x1700147B RID: 5243
		// (get) Token: 0x06005718 RID: 22296 RVA: 0x0026CD15 File Offset: 0x0026BD15
		// (set) Token: 0x06005719 RID: 22297 RVA: 0x0026CD1D File Offset: 0x0026BD1D
		public bool IsMergeable
		{
			get
			{
				return this._isMergeable;
			}
			set
			{
				this._isMergeable = value;
			}
		}

		// Token: 0x1700147C RID: 5244
		// (get) Token: 0x0600571A RID: 22298 RVA: 0x0026CD26 File Offset: 0x0026BD26
		// (set) Token: 0x0600571B RID: 22299 RVA: 0x0026CD2E File Offset: 0x0026BD2E
		public bool IsTypographicOnly
		{
			get
			{
				return this._isTypographicOnly;
			}
			set
			{
				this._isTypographicOnly = value;
			}
		}

		// Token: 0x04002FB2 RID: 12210
		private bool _isMergeable;

		// Token: 0x04002FB3 RID: 12211
		private bool _isTypographicOnly;
	}
}
