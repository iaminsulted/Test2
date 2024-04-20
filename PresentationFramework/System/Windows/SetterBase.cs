using System;

namespace System.Windows
{
	// Token: 0x02000399 RID: 921
	[Localizability(LocalizationCategory.Ignore)]
	public abstract class SetterBase
	{
		// Token: 0x0600254D RID: 9549 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		internal SetterBase()
		{
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x0600254E RID: 9550 RVA: 0x00186220 File Offset: 0x00185220
		public bool IsSealed
		{
			get
			{
				return this._sealed;
			}
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x00186228 File Offset: 0x00185228
		internal virtual void Seal()
		{
			this._sealed = true;
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x00186231 File Offset: 0x00185231
		protected void CheckSealed()
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"SetterBase"
				}));
			}
		}

		// Token: 0x04001184 RID: 4484
		private bool _sealed;
	}
}
