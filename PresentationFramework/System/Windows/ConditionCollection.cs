using System;
using System.Collections.ObjectModel;

namespace System.Windows
{
	// Token: 0x0200034A RID: 842
	public sealed class ConditionCollection : Collection<Condition>
	{
		// Token: 0x06002003 RID: 8195 RVA: 0x0017430D File Offset: 0x0017330D
		protected override void ClearItems()
		{
			this.CheckSealed();
			base.ClearItems();
		}

		// Token: 0x06002004 RID: 8196 RVA: 0x0017431B File Offset: 0x0017331B
		protected override void InsertItem(int index, Condition item)
		{
			this.CheckSealed();
			this.ConditionValidation(item);
			base.InsertItem(index, item);
		}

		// Token: 0x06002005 RID: 8197 RVA: 0x00174332 File Offset: 0x00173332
		protected override void RemoveItem(int index)
		{
			this.CheckSealed();
			base.RemoveItem(index);
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x00174341 File Offset: 0x00173341
		protected override void SetItem(int index, Condition item)
		{
			this.CheckSealed();
			this.ConditionValidation(item);
			base.SetItem(index, item);
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06002007 RID: 8199 RVA: 0x00174358 File Offset: 0x00173358
		public bool IsSealed
		{
			get
			{
				return this._sealed;
			}
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x00174360 File Offset: 0x00173360
		internal void Seal(ValueLookupType type)
		{
			this._sealed = true;
			for (int i = 0; i < base.Count; i++)
			{
				base[i].Seal(type);
			}
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x00174392 File Offset: 0x00173392
		private void CheckSealed()
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"ConditionCollection"
				}));
			}
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x001743BA File Offset: 0x001733BA
		private void ConditionValidation(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is Condition))
			{
				throw new ArgumentException(SR.Get("MustBeCondition"));
			}
		}

		// Token: 0x04000FB6 RID: 4022
		private bool _sealed;
	}
}
