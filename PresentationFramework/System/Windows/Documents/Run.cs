using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x02000683 RID: 1667
	[ContentProperty("Text")]
	public class Run : Inline
	{
		// Token: 0x060052A2 RID: 21154 RVA: 0x00243005 File Offset: 0x00242005
		public Run()
		{
		}

		// Token: 0x060052A3 RID: 21155 RVA: 0x00258BB5 File Offset: 0x00257BB5
		public Run(string text) : this(text, null)
		{
		}

		// Token: 0x060052A4 RID: 21156 RVA: 0x00258BC0 File Offset: 0x00257BC0
		public Run(string text, TextPointer insertionPosition)
		{
			if (insertionPosition != null)
			{
				insertionPosition.TextContainer.BeginChange();
			}
			try
			{
				if (insertionPosition != null)
				{
					insertionPosition.InsertInline(this);
				}
				if (text != null)
				{
					base.ContentStart.InsertTextInRun(text);
				}
			}
			finally
			{
				if (insertionPosition != null)
				{
					insertionPosition.TextContainer.EndChange();
				}
			}
		}

		// Token: 0x17001381 RID: 4993
		// (get) Token: 0x060052A5 RID: 21157 RVA: 0x00258C1C File Offset: 0x00257C1C
		// (set) Token: 0x060052A6 RID: 21158 RVA: 0x00258C2E File Offset: 0x00257C2E
		public string Text
		{
			get
			{
				return (string)base.GetValue(Run.TextProperty);
			}
			set
			{
				base.SetValue(Run.TextProperty, value);
			}
		}

		// Token: 0x060052A7 RID: 21159 RVA: 0x00258C3C File Offset: 0x00257C3C
		internal override void OnTextUpdated()
		{
			ValueSource valueSource = DependencyPropertyHelper.GetValueSource(this, Run.TextProperty);
			if (!this._isInsideDeferredSet && (this._changeEventNestingCount == 0 || (valueSource.BaseValueSource == BaseValueSource.Local && !valueSource.IsExpression)))
			{
				this._changeEventNestingCount++;
				this._isInsideDeferredSet = true;
				try
				{
					base.SetCurrentDeferredValue(Run.TextProperty, new DeferredRunTextReference(this));
				}
				finally
				{
					this._isInsideDeferredSet = false;
					this._changeEventNestingCount--;
				}
			}
		}

		// Token: 0x060052A8 RID: 21160 RVA: 0x00258CC8 File Offset: 0x00257CC8
		internal override void BeforeLogicalTreeChange()
		{
			this._changeEventNestingCount++;
		}

		// Token: 0x060052A9 RID: 21161 RVA: 0x00258CD8 File Offset: 0x00257CD8
		internal override void AfterLogicalTreeChange()
		{
			this._changeEventNestingCount--;
		}

		// Token: 0x17001382 RID: 4994
		// (get) Token: 0x060052AA RID: 21162 RVA: 0x001A519B File Offset: 0x001A419B
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 13;
			}
		}

		// Token: 0x060052AB RID: 21163 RVA: 0x0022BF83 File Offset: 0x0022AF83
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeText(XamlDesignerSerializationManager manager)
		{
			return manager != null && manager.XmlWriter == null;
		}

		// Token: 0x060052AC RID: 21164 RVA: 0x00258CE8 File Offset: 0x00257CE8
		private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Run run = (Run)d;
			if (run._changeEventNestingCount > 0)
			{
				return;
			}
			Invariant.Assert(!e.NewEntry.IsDeferredReference);
			string text = (string)e.NewValue;
			if (text == null)
			{
				text = string.Empty;
			}
			run._changeEventNestingCount++;
			try
			{
				TextContainer textContainer = run.TextContainer;
				textContainer.BeginChange();
				try
				{
					TextPointer contentStart = run.ContentStart;
					if (!run.IsEmpty)
					{
						textContainer.DeleteContentInternal(contentStart, run.ContentEnd);
					}
					contentStart.InsertTextInRun(text);
				}
				finally
				{
					textContainer.EndChange();
				}
			}
			finally
			{
				run._changeEventNestingCount--;
			}
			FlowDocument flowDocument = run.TextContainer.Parent as FlowDocument;
			if (flowDocument != null)
			{
				RichTextBox richTextBox = flowDocument.Parent as RichTextBox;
				if (richTextBox != null && run.HasExpression(run.LookupEntry(Run.TextProperty.GlobalIndex), Run.TextProperty))
				{
					UndoManager undoManager = richTextBox.TextEditor._GetUndoManager();
					if (undoManager != null && undoManager.IsEnabled)
					{
						undoManager.Clear();
					}
				}
			}
		}

		// Token: 0x060052AD RID: 21165 RVA: 0x00258E10 File Offset: 0x00257E10
		private static object CoerceText(DependencyObject d, object baseValue)
		{
			if (baseValue == null)
			{
				baseValue = string.Empty;
			}
			return baseValue;
		}

		// Token: 0x04002EB1 RID: 11953
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(Run), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Run.OnTextPropertyChanged), new CoerceValueCallback(Run.CoerceText)));

		// Token: 0x04002EB2 RID: 11954
		private int _changeEventNestingCount;

		// Token: 0x04002EB3 RID: 11955
		private bool _isInsideDeferredSet;
	}
}
