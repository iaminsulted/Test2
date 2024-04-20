using System;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x020003D8 RID: 984
	public abstract class TriggerAction : DependencyObject
	{
		// Token: 0x0600293C RID: 10556 RVA: 0x00199514 File Offset: 0x00198514
		internal TriggerAction()
		{
		}

		// Token: 0x0600293D RID: 10557
		internal abstract void Invoke(FrameworkElement fe, FrameworkContentElement fce, Style targetStyle, FrameworkTemplate targetTemplate, long layer);

		// Token: 0x0600293E RID: 10558
		internal abstract void Invoke(FrameworkElement fe);

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x0600293F RID: 10559 RVA: 0x0019951C File Offset: 0x0019851C
		internal TriggerBase ContainingTrigger
		{
			get
			{
				return this._containingTrigger;
			}
		}

		// Token: 0x06002940 RID: 10560 RVA: 0x00199524 File Offset: 0x00198524
		internal void Seal(TriggerBase containingTrigger)
		{
			if (base.IsSealed && containingTrigger != this._containingTrigger)
			{
				throw new InvalidOperationException(SR.Get("TriggerActionMustBelongToASingleTrigger"));
			}
			this._containingTrigger = containingTrigger;
			this.Seal();
		}

		// Token: 0x06002941 RID: 10561 RVA: 0x00199554 File Offset: 0x00198554
		internal override void Seal()
		{
			if (base.IsSealed)
			{
				throw new InvalidOperationException(SR.Get("TriggerActionAlreadySealed"));
			}
			base.Seal();
		}

		// Token: 0x06002942 RID: 10562 RVA: 0x00199574 File Offset: 0x00198574
		internal void CheckSealed()
		{
			if (base.IsSealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"TriggerAction"
				}));
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x06002943 RID: 10563 RVA: 0x0019959C File Offset: 0x0019859C
		internal override DependencyObject InheritanceContext
		{
			get
			{
				return this._inheritanceContext;
			}
		}

		// Token: 0x06002944 RID: 10564 RVA: 0x001995A4 File Offset: 0x001985A4
		internal override void AddInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			InheritanceContextHelper.AddInheritanceContext(context, this, ref this._hasMultipleInheritanceContexts, ref this._inheritanceContext);
		}

		// Token: 0x06002945 RID: 10565 RVA: 0x001995B9 File Offset: 0x001985B9
		internal override void RemoveInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			InheritanceContextHelper.RemoveInheritanceContext(context, this, ref this._hasMultipleInheritanceContexts, ref this._inheritanceContext);
		}

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x06002946 RID: 10566 RVA: 0x001995CE File Offset: 0x001985CE
		internal override bool HasMultipleInheritanceContexts
		{
			get
			{
				return this._hasMultipleInheritanceContexts;
			}
		}

		// Token: 0x040014E6 RID: 5350
		private TriggerBase _containingTrigger;

		// Token: 0x040014E7 RID: 5351
		private DependencyObject _inheritanceContext;

		// Token: 0x040014E8 RID: 5352
		private bool _hasMultipleInheritanceContexts;
	}
}
