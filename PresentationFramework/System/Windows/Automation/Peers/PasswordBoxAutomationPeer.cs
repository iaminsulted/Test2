using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Documents;
using MS.Internal.Automation;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000582 RID: 1410
	public class PasswordBoxAutomationPeer : TextAutomationPeer, IValueProvider
	{
		// Token: 0x06004514 RID: 17684 RVA: 0x00222FF7 File Offset: 0x00221FF7
		public PasswordBoxAutomationPeer(PasswordBox owner) : base(owner)
		{
		}

		// Token: 0x06004515 RID: 17685 RVA: 0x00223000 File Offset: 0x00222000
		protected override string GetClassNameCore()
		{
			return "PasswordBox";
		}

		// Token: 0x06004516 RID: 17686 RVA: 0x001FC019 File Offset: 0x001FB019
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Edit;
		}

		// Token: 0x06004517 RID: 17687 RVA: 0x00223008 File Offset: 0x00222008
		public override object GetPattern(PatternInterface patternInterface)
		{
			object obj = null;
			if (patternInterface == PatternInterface.Value)
			{
				obj = this;
			}
			else if (patternInterface == PatternInterface.Text)
			{
				if (this._textPattern == null)
				{
					this._textPattern = new TextAdaptor(this, ((PasswordBox)base.Owner).TextContainer);
				}
				obj = this._textPattern;
			}
			else if (patternInterface == PatternInterface.Scroll)
			{
				PasswordBox passwordBox = (PasswordBox)base.Owner;
				if (passwordBox.ScrollViewer != null)
				{
					obj = passwordBox.ScrollViewer.CreateAutomationPeer();
					((AutomationPeer)obj).EventsSource = this;
				}
			}
			else
			{
				obj = base.GetPattern(patternInterface);
			}
			return obj;
		}

		// Token: 0x06004518 RID: 17688 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected override bool IsPasswordCore()
		{
			return true;
		}

		// Token: 0x17000F79 RID: 3961
		// (get) Token: 0x06004519 RID: 17689 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IValueProvider.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x0600451A RID: 17690 RVA: 0x0022308C File Offset: 0x0022208C
		string IValueProvider.Value
		{
			get
			{
				if (AccessibilitySwitches.UseNetFx47CompatibleAccessibilityFeatures)
				{
					throw new InvalidOperationException();
				}
				return string.Empty;
			}
		}

		// Token: 0x0600451B RID: 17691 RVA: 0x002230A0 File Offset: 0x002220A0
		void IValueProvider.SetValue(string value)
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			PasswordBox passwordBox = (PasswordBox)base.Owner;
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			passwordBox.Password = value;
		}

		// Token: 0x0600451C RID: 17692 RVA: 0x0021CA91 File Offset: 0x0021BA91
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseValuePropertyChangedEvent(string oldValue, string newValue)
		{
			if (oldValue != newValue)
			{
				base.RaisePropertyChangedEvent(ValuePatternIdentifiers.ValueProperty, oldValue, newValue);
			}
		}

		// Token: 0x0600451D RID: 17693 RVA: 0x002230CF File Offset: 0x002220CF
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseIsReadOnlyPropertyChangedEvent(bool oldValue, bool newValue)
		{
			if (oldValue != newValue)
			{
				base.RaisePropertyChangedEvent(ValuePatternIdentifiers.IsReadOnlyProperty, oldValue, newValue);
			}
		}

		// Token: 0x0600451E RID: 17694 RVA: 0x002230EC File Offset: 0x002220EC
		internal override List<AutomationPeer> GetAutomationPeersFromRange(ITextPointer start, ITextPointer end)
		{
			return new List<AutomationPeer>();
		}

		// Token: 0x04002540 RID: 9536
		private TextAdaptor _textPattern;
	}
}
