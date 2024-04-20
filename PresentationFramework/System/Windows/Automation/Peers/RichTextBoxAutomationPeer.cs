using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using MS.Internal.Automation;
using MS.Internal.Documents;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000588 RID: 1416
	public class RichTextBoxAutomationPeer : TextAutomationPeer
	{
		// Token: 0x06004546 RID: 17734 RVA: 0x002233D5 File Offset: 0x002223D5
		public RichTextBoxAutomationPeer(RichTextBox owner) : base(owner)
		{
			this._textPattern = new TextAdaptor(this, owner.TextContainer);
		}

		// Token: 0x06004547 RID: 17735 RVA: 0x002233F0 File Offset: 0x002223F0
		protected override string GetClassNameCore()
		{
			return "RichTextBox";
		}

		// Token: 0x06004548 RID: 17736 RVA: 0x001FD726 File Offset: 0x001FC726
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Document;
		}

		// Token: 0x06004549 RID: 17737 RVA: 0x002233F8 File Offset: 0x002223F8
		public override object GetPattern(PatternInterface patternInterface)
		{
			object obj = null;
			RichTextBox richTextBox = (RichTextBox)base.Owner;
			if (patternInterface == PatternInterface.Text)
			{
				if (this._textPattern == null)
				{
					this._textPattern = new TextAdaptor(this, richTextBox.TextContainer);
				}
				return this._textPattern;
			}
			if (patternInterface == PatternInterface.Scroll)
			{
				if (richTextBox.ScrollViewer != null)
				{
					obj = richTextBox.ScrollViewer.CreateAutomationPeer();
					((AutomationPeer)obj).EventsSource = this;
				}
			}
			else
			{
				obj = base.GetPattern(patternInterface);
			}
			return obj;
		}

		// Token: 0x0600454A RID: 17738 RVA: 0x00223468 File Offset: 0x00222468
		protected override List<AutomationPeer> GetChildrenCore()
		{
			RichTextBox richTextBox = (RichTextBox)base.Owner;
			return TextContainerHelper.GetAutomationPeersFromRange(richTextBox.TextContainer.Start, richTextBox.TextContainer.End, null);
		}

		// Token: 0x0600454B RID: 17739 RVA: 0x002234A0 File Offset: 0x002224A0
		internal override List<AutomationPeer> GetAutomationPeersFromRange(ITextPointer start, ITextPointer end)
		{
			base.GetChildren();
			RichTextBox richTextBox = (RichTextBox)base.Owner;
			return TextContainerHelper.GetAutomationPeersFromRange(start, end, richTextBox.TextContainer.Start);
		}

		// Token: 0x04002541 RID: 9537
		private TextAdaptor _textPattern;
	}
}
