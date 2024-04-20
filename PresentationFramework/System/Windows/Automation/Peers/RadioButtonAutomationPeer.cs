using System;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using MS.Internal.KnownBoxes;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000585 RID: 1413
	public class RadioButtonAutomationPeer : ToggleButtonAutomationPeer, ISelectionItemProvider
	{
		// Token: 0x0600452A RID: 17706 RVA: 0x0021C89C File Offset: 0x0021B89C
		public RadioButtonAutomationPeer(RadioButton owner) : base(owner)
		{
		}

		// Token: 0x0600452B RID: 17707 RVA: 0x00223147 File Offset: 0x00222147
		protected override string GetClassNameCore()
		{
			return "RadioButton";
		}

		// Token: 0x0600452C RID: 17708 RVA: 0x001A519B File Offset: 0x001A419B
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.RadioButton;
		}

		// Token: 0x0600452D RID: 17709 RVA: 0x0022314E File Offset: 0x0022214E
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.SelectionItem)
			{
				return this;
			}
			if (patternInterface == PatternInterface.SynchronizedInput)
			{
				return base.GetPattern(patternInterface);
			}
			return null;
		}

		// Token: 0x0600452E RID: 17710 RVA: 0x00223165 File Offset: 0x00222165
		void ISelectionItemProvider.Select()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			((RadioButton)base.Owner).SetCurrentValueInternal(ToggleButton.IsCheckedProperty, BooleanBoxes.TrueBox);
		}

		// Token: 0x0600452F RID: 17711 RVA: 0x00223190 File Offset: 0x00222190
		void ISelectionItemProvider.AddToSelection()
		{
			bool? isChecked = ((RadioButton)base.Owner).IsChecked;
			bool flag = true;
			if (!(isChecked.GetValueOrDefault() == flag & isChecked != null))
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
		}

		// Token: 0x06004530 RID: 17712 RVA: 0x002231D4 File Offset: 0x002221D4
		void ISelectionItemProvider.RemoveFromSelection()
		{
			bool? isChecked = ((RadioButton)base.Owner).IsChecked;
			bool flag = true;
			if (isChecked.GetValueOrDefault() == flag & isChecked != null)
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
		}

		// Token: 0x17000F7E RID: 3966
		// (get) Token: 0x06004531 RID: 17713 RVA: 0x00223218 File Offset: 0x00222218
		bool ISelectionItemProvider.IsSelected
		{
			get
			{
				bool? isChecked = ((RadioButton)base.Owner).IsChecked;
				bool flag = true;
				return isChecked.GetValueOrDefault() == flag & isChecked != null;
			}
		}

		// Token: 0x17000F7F RID: 3967
		// (get) Token: 0x06004532 RID: 17714 RVA: 0x00109403 File Offset: 0x00108403
		IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06004533 RID: 17715 RVA: 0x0022324C File Offset: 0x0022224C
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal override void RaiseToggleStatePropertyChangedEvent(bool? oldValue, bool? newValue)
		{
			AutomationProperty isSelectedProperty = SelectionItemPatternIdentifiers.IsSelectedProperty;
			bool? flag = oldValue;
			bool flag2 = true;
			object oldValue2 = flag.GetValueOrDefault() == flag2 & flag != null;
			flag = newValue;
			flag2 = true;
			base.RaisePropertyChangedEvent(isSelectedProperty, oldValue2, flag.GetValueOrDefault() == flag2 & flag != null);
		}
	}
}
