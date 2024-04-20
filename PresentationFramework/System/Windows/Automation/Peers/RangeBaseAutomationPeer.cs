using System;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000586 RID: 1414
	public class RangeBaseAutomationPeer : FrameworkElementAutomationPeer, IRangeValueProvider
	{
		// Token: 0x06004534 RID: 17716 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public RangeBaseAutomationPeer(RangeBase owner) : base(owner)
		{
		}

		// Token: 0x06004535 RID: 17717 RVA: 0x0022329A File Offset: 0x0022229A
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.RangeValue)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x06004536 RID: 17718 RVA: 0x002232A9 File Offset: 0x002222A9
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseMinimumPropertyChangedEvent(double oldValue, double newValue)
		{
			base.RaisePropertyChangedEvent(RangeValuePatternIdentifiers.MinimumProperty, oldValue, newValue);
		}

		// Token: 0x06004537 RID: 17719 RVA: 0x002232C2 File Offset: 0x002222C2
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseMaximumPropertyChangedEvent(double oldValue, double newValue)
		{
			base.RaisePropertyChangedEvent(RangeValuePatternIdentifiers.MaximumProperty, oldValue, newValue);
		}

		// Token: 0x06004538 RID: 17720 RVA: 0x002232DB File Offset: 0x002222DB
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseValuePropertyChangedEvent(double oldValue, double newValue)
		{
			base.RaisePropertyChangedEvent(RangeValuePatternIdentifiers.ValueProperty, oldValue, newValue);
		}

		// Token: 0x06004539 RID: 17721 RVA: 0x002232F4 File Offset: 0x002222F4
		internal virtual void SetValueCore(double val)
		{
			RangeBase rangeBase = (RangeBase)base.Owner;
			if (val < rangeBase.Minimum || val > rangeBase.Maximum)
			{
				throw new ArgumentOutOfRangeException("val");
			}
			rangeBase.Value = val;
		}

		// Token: 0x0600453A RID: 17722 RVA: 0x00223332 File Offset: 0x00222332
		void IRangeValueProvider.SetValue(double val)
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			this.SetValueCore(val);
		}

		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x0600453B RID: 17723 RVA: 0x00223349 File Offset: 0x00222349
		double IRangeValueProvider.Value
		{
			get
			{
				return ((RangeBase)base.Owner).Value;
			}
		}

		// Token: 0x17000F81 RID: 3969
		// (get) Token: 0x0600453C RID: 17724 RVA: 0x0022335B File Offset: 0x0022235B
		bool IRangeValueProvider.IsReadOnly
		{
			get
			{
				return !base.IsEnabled();
			}
		}

		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x0600453D RID: 17725 RVA: 0x00223366 File Offset: 0x00222366
		double IRangeValueProvider.Maximum
		{
			get
			{
				return ((RangeBase)base.Owner).Maximum;
			}
		}

		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x0600453E RID: 17726 RVA: 0x00223378 File Offset: 0x00222378
		double IRangeValueProvider.Minimum
		{
			get
			{
				return ((RangeBase)base.Owner).Minimum;
			}
		}

		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x0600453F RID: 17727 RVA: 0x0022338A File Offset: 0x0022238A
		double IRangeValueProvider.LargeChange
		{
			get
			{
				return ((RangeBase)base.Owner).LargeChange;
			}
		}

		// Token: 0x17000F85 RID: 3973
		// (get) Token: 0x06004540 RID: 17728 RVA: 0x0022339C File Offset: 0x0022239C
		double IRangeValueProvider.SmallChange
		{
			get
			{
				return ((RangeBase)base.Owner).SmallChange;
			}
		}
	}
}
