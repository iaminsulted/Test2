using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using MS.Internal.KnownBoxes;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200054A RID: 1354
	public class ComboBoxAutomationPeer : SelectorAutomationPeer, IValueProvider, IExpandCollapseProvider
	{
		// Token: 0x060042CC RID: 17100 RVA: 0x0021C8AC File Offset: 0x0021B8AC
		public ComboBoxAutomationPeer(ComboBox owner) : base(owner)
		{
		}

		// Token: 0x060042CD RID: 17101 RVA: 0x0021C8B5 File Offset: 0x0021B8B5
		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			return new ListBoxItemAutomationPeer(item, this);
		}

		// Token: 0x060042CE RID: 17102 RVA: 0x001E977A File Offset: 0x001E877A
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.ComboBox;
		}

		// Token: 0x060042CF RID: 17103 RVA: 0x0021C8BE File Offset: 0x0021B8BE
		protected override string GetClassNameCore()
		{
			return "ComboBox";
		}

		// Token: 0x060042D0 RID: 17104 RVA: 0x0021C8C8 File Offset: 0x0021B8C8
		public override object GetPattern(PatternInterface pattern)
		{
			object result = null;
			ComboBox comboBox = (ComboBox)base.Owner;
			if (pattern == PatternInterface.Value)
			{
				if (comboBox.IsEditable)
				{
					result = this;
				}
			}
			else if (pattern == PatternInterface.ExpandCollapse)
			{
				result = this;
			}
			else if (pattern == PatternInterface.Scroll && !comboBox.IsDropDownOpen)
			{
				result = this;
			}
			else
			{
				result = base.GetPattern(pattern);
			}
			return result;
		}

		// Token: 0x060042D1 RID: 17105 RVA: 0x0021C914 File Offset: 0x0021B914
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = base.GetChildrenCore();
			TextBox editableTextBoxSite = ((ComboBox)base.Owner).EditableTextBoxSite;
			if (editableTextBoxSite != null)
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(editableTextBoxSite);
				if (automationPeer != null)
				{
					if (list == null)
					{
						list = new List<AutomationPeer>();
					}
					list.Insert(0, automationPeer);
				}
			}
			return list;
		}

		// Token: 0x060042D2 RID: 17106 RVA: 0x0021C958 File Offset: 0x0021B958
		protected override void SetFocusCore()
		{
			ComboBox comboBox = (ComboBox)base.Owner;
			if (comboBox.Focusable)
			{
				if (!comboBox.Focus())
				{
					if (!comboBox.IsEditable)
					{
						throw new InvalidOperationException(SR.Get("SetFocusFailed"));
					}
					TextBox editableTextBoxSite = comboBox.EditableTextBoxSite;
					if (editableTextBoxSite == null || !editableTextBoxSite.IsKeyboardFocused)
					{
						throw new InvalidOperationException(SR.Get("SetFocusFailed"));
					}
				}
				return;
			}
			throw new InvalidOperationException(SR.Get("SetFocusFailed"));
		}

		// Token: 0x060042D3 RID: 17107 RVA: 0x0021C9CC File Offset: 0x0021B9CC
		internal void ScrollItemIntoView(object item)
		{
			if (((IExpandCollapseProvider)this).ExpandCollapseState == ExpandCollapseState.Expanded)
			{
				ComboBox comboBox = (ComboBox)base.Owner;
				if (comboBox.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
				{
					comboBox.OnBringItemIntoView(item);
					return;
				}
				base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(comboBox.OnBringItemIntoView), item);
			}
		}

		// Token: 0x060042D4 RID: 17108 RVA: 0x0021CA1F File Offset: 0x0021BA1F
		void IValueProvider.SetValue(string val)
		{
			if (val == null)
			{
				throw new ArgumentNullException("val");
			}
			ComboBox comboBox = (ComboBox)base.Owner;
			if (!comboBox.IsEnabled)
			{
				throw new ElementNotEnabledException();
			}
			comboBox.SetCurrentValueInternal(ComboBox.TextProperty, val);
		}

		// Token: 0x17000F13 RID: 3859
		// (get) Token: 0x060042D5 RID: 17109 RVA: 0x0021CA53 File Offset: 0x0021BA53
		string IValueProvider.Value
		{
			get
			{
				return ((ComboBox)this.Owner).Text;
			}
		}

		// Token: 0x17000F14 RID: 3860
		// (get) Token: 0x060042D6 RID: 17110 RVA: 0x0021CA68 File Offset: 0x0021BA68
		bool IValueProvider.IsReadOnly
		{
			get
			{
				ComboBox comboBox = (ComboBox)base.Owner;
				return !comboBox.IsEnabled || comboBox.IsReadOnly;
			}
		}

		// Token: 0x060042D7 RID: 17111 RVA: 0x0021CA91 File Offset: 0x0021BA91
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseValuePropertyChangedEvent(string oldValue, string newValue)
		{
			if (oldValue != newValue)
			{
				base.RaisePropertyChangedEvent(ValuePatternIdentifiers.ValueProperty, oldValue, newValue);
			}
		}

		// Token: 0x060042D8 RID: 17112 RVA: 0x0021CAA9 File Offset: 0x0021BAA9
		void IExpandCollapseProvider.Expand()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			((ComboBox)this.Owner).SetCurrentValueInternal(ComboBox.IsDropDownOpenProperty, BooleanBoxes.TrueBox);
		}

		// Token: 0x060042D9 RID: 17113 RVA: 0x0021CAD3 File Offset: 0x0021BAD3
		void IExpandCollapseProvider.Collapse()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			((ComboBox)this.Owner).SetCurrentValueInternal(ComboBox.IsDropDownOpenProperty, BooleanBoxes.FalseBox);
		}

		// Token: 0x17000F15 RID: 3861
		// (get) Token: 0x060042DA RID: 17114 RVA: 0x0021CAFD File Offset: 0x0021BAFD
		ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState
		{
			get
			{
				if (!((ComboBox)this.Owner).IsDropDownOpen)
				{
					return ExpandCollapseState.Collapsed;
				}
				return ExpandCollapseState.Expanded;
			}
		}

		// Token: 0x060042DB RID: 17115 RVA: 0x0021CB14 File Offset: 0x0021BB14
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseExpandCollapseAutomationEvent(bool oldValue, bool newValue)
		{
			base.RaisePropertyChangedEvent(ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty, oldValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed, newValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed);
		}
	}
}
