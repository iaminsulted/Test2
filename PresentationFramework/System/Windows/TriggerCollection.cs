using System;
using System.Collections.ObjectModel;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x020003DB RID: 987
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public sealed class TriggerCollection : Collection<TriggerBase>
	{
		// Token: 0x0600297D RID: 10621 RVA: 0x00199C77 File Offset: 0x00198C77
		internal TriggerCollection() : this(null)
		{
		}

		// Token: 0x0600297E RID: 10622 RVA: 0x00199C80 File Offset: 0x00198C80
		internal TriggerCollection(FrameworkElement owner)
		{
			this._sealed = false;
			this._owner = owner;
		}

		// Token: 0x0600297F RID: 10623 RVA: 0x00199C96 File Offset: 0x00198C96
		protected override void ClearItems()
		{
			this.CheckSealed();
			this.OnClear();
			base.ClearItems();
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x00199CAA File Offset: 0x00198CAA
		protected override void InsertItem(int index, TriggerBase item)
		{
			this.CheckSealed();
			this.TriggerBaseValidation(item);
			this.OnAdd(item);
			base.InsertItem(index, item);
		}

		// Token: 0x06002981 RID: 10625 RVA: 0x00199CC8 File Offset: 0x00198CC8
		protected override void RemoveItem(int index)
		{
			this.CheckSealed();
			TriggerBase triggerBase = base[index];
			this.OnRemove(triggerBase);
			base.RemoveItem(index);
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x00199CF1 File Offset: 0x00198CF1
		protected override void SetItem(int index, TriggerBase item)
		{
			this.CheckSealed();
			this.TriggerBaseValidation(item);
			this.OnAdd(item);
			base.SetItem(index, item);
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x06002983 RID: 10627 RVA: 0x00199D0F File Offset: 0x00198D0F
		public bool IsSealed
		{
			get
			{
				return this._sealed;
			}
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x00199D18 File Offset: 0x00198D18
		internal void Seal()
		{
			this._sealed = true;
			for (int i = 0; i < base.Count; i++)
			{
				base[i].Seal();
			}
		}

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x06002985 RID: 10629 RVA: 0x00199D49 File Offset: 0x00198D49
		internal FrameworkElement Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x00199D51 File Offset: 0x00198D51
		private void CheckSealed()
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"TriggerCollection"
				}));
			}
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x00199D79 File Offset: 0x00198D79
		private void TriggerBaseValidation(TriggerBase triggerBase)
		{
			if (triggerBase == null)
			{
				throw new ArgumentNullException("triggerBase");
			}
		}

		// Token: 0x06002988 RID: 10632 RVA: 0x00199D89 File Offset: 0x00198D89
		private void OnAdd(TriggerBase triggerBase)
		{
			if (this.Owner != null && this.Owner.IsInitialized)
			{
				EventTrigger.ProcessOneTrigger(this.Owner, triggerBase);
			}
			InheritanceContextHelper.ProvideContextForObject(this.Owner, triggerBase);
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x00199DB8 File Offset: 0x00198DB8
		private void OnRemove(TriggerBase triggerBase)
		{
			if (this.Owner != null)
			{
				if (this.Owner.IsInitialized)
				{
					EventTrigger.DisconnectOneTrigger(this.Owner, triggerBase);
				}
				InheritanceContextHelper.RemoveContextFromObject(this.Owner, triggerBase);
			}
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x00199DE8 File Offset: 0x00198DE8
		private void OnClear()
		{
			if (this.Owner != null)
			{
				if (this.Owner.IsInitialized)
				{
					EventTrigger.DisconnectAllTriggers(this.Owner);
				}
				for (int i = base.Count - 1; i >= 0; i--)
				{
					InheritanceContextHelper.RemoveContextFromObject(this.Owner, base[i]);
				}
			}
		}

		// Token: 0x040014F5 RID: 5365
		private bool _sealed;

		// Token: 0x040014F6 RID: 5366
		private FrameworkElement _owner;
	}
}
