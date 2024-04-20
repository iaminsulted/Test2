using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using MS.Internal.Automation;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000598 RID: 1432
	public class TextBoxAutomationPeer : TextAutomationPeer, IValueProvider
	{
		// Token: 0x060045BA RID: 17850 RVA: 0x0022472D File Offset: 0x0022372D
		public TextBoxAutomationPeer(TextBox owner) : base(owner)
		{
			this._textPattern = new TextAdaptor(this, owner.TextContainer);
		}

		// Token: 0x060045BB RID: 17851 RVA: 0x00224748 File Offset: 0x00223748
		protected override string GetClassNameCore()
		{
			return "TextBox";
		}

		// Token: 0x060045BC RID: 17852 RVA: 0x001FC019 File Offset: 0x001FB019
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Edit;
		}

		// Token: 0x060045BD RID: 17853 RVA: 0x00224750 File Offset: 0x00223750
		public override object GetPattern(PatternInterface patternInterface)
		{
			object obj = null;
			if (patternInterface == PatternInterface.Value)
			{
				obj = this;
			}
			if (patternInterface == PatternInterface.Text)
			{
				if (this._textPattern == null)
				{
					this._textPattern = new TextAdaptor(this, ((TextBoxBase)base.Owner).TextContainer);
				}
				return this._textPattern;
			}
			if (patternInterface == PatternInterface.Scroll)
			{
				TextBox textBox = (TextBox)base.Owner;
				if (textBox.ScrollViewer != null)
				{
					obj = textBox.ScrollViewer.CreateAutomationPeer();
					((AutomationPeer)obj).EventsSource = this;
				}
			}
			if (patternInterface == PatternInterface.SynchronizedInput)
			{
				obj = base.GetPattern(patternInterface);
			}
			return obj;
		}

		// Token: 0x17000F9A RID: 3994
		// (get) Token: 0x060045BE RID: 17854 RVA: 0x002247D3 File Offset: 0x002237D3
		bool IValueProvider.IsReadOnly
		{
			get
			{
				return ((TextBox)base.Owner).IsReadOnly;
			}
		}

		// Token: 0x17000F9B RID: 3995
		// (get) Token: 0x060045BF RID: 17855 RVA: 0x002247E5 File Offset: 0x002237E5
		string IValueProvider.Value
		{
			get
			{
				return ((TextBox)base.Owner).Text;
			}
		}

		// Token: 0x060045C0 RID: 17856 RVA: 0x002247F7 File Offset: 0x002237F7
		void IValueProvider.SetValue(string value)
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			TextBox textBox = (TextBox)base.Owner;
			if (textBox.IsReadOnly)
			{
				throw new ElementNotEnabledException();
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			textBox.Text = value;
		}

		// Token: 0x060045C1 RID: 17857 RVA: 0x0021CA91 File Offset: 0x0021BA91
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseValuePropertyChangedEvent(string oldValue, string newValue)
		{
			if (oldValue != newValue)
			{
				base.RaisePropertyChangedEvent(ValuePatternIdentifiers.ValueProperty, oldValue, newValue);
			}
		}

		// Token: 0x060045C2 RID: 17858 RVA: 0x002230CF File Offset: 0x002220CF
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseIsReadOnlyPropertyChangedEvent(bool oldValue, bool newValue)
		{
			if (oldValue != newValue)
			{
				base.RaisePropertyChangedEvent(ValuePatternIdentifiers.IsReadOnlyProperty, oldValue, newValue);
			}
		}

		// Token: 0x060045C3 RID: 17859 RVA: 0x002230EC File Offset: 0x002220EC
		internal override List<AutomationPeer> GetAutomationPeersFromRange(ITextPointer start, ITextPointer end)
		{
			return new List<AutomationPeer>();
		}

		// Token: 0x04002544 RID: 9540
		private TextAdaptor _textPattern;
	}
}
