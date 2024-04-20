using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Xml;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006B8 RID: 1720
	public class TextRange : ITextRange
	{
		// Token: 0x06005812 RID: 22546 RVA: 0x00271344 File Offset: 0x00270344
		public TextRange(TextPointer position1, TextPointer position2) : this(position1, position2)
		{
		}

		// Token: 0x06005813 RID: 22547 RVA: 0x0027134E File Offset: 0x0027034E
		internal TextRange(ITextPointer position1, ITextPointer position2) : this(position1, position2, false)
		{
		}

		// Token: 0x06005814 RID: 22548 RVA: 0x00271359 File Offset: 0x00270359
		internal TextRange(TextPointer position1, TextPointer position2, bool useRestrictiveXamlXmlReader) : this(position1, position2)
		{
			this._useRestrictiveXamlXmlReader = useRestrictiveXamlXmlReader;
		}

		// Token: 0x06005815 RID: 22549 RVA: 0x0027136C File Offset: 0x0027036C
		internal TextRange(ITextPointer position1, ITextPointer position2, bool ignoreTextUnitBoundaries)
		{
			if (position1 == null)
			{
				throw new ArgumentNullException("position1");
			}
			if (position2 == null)
			{
				throw new ArgumentNullException("position2");
			}
			this.SetFlags(ignoreTextUnitBoundaries, TextRange.Flags.IgnoreTextUnitBoundaries);
			ValidationHelper.VerifyPosition(position1.TextContainer, position1, "position1");
			ValidationHelper.VerifyPosition(position1.TextContainer, position2, "position2");
			TextRangeBase.Select(this, position1, position2);
		}

		// Token: 0x06005816 RID: 22550 RVA: 0x002713CD File Offset: 0x002703CD
		bool ITextRange.Contains(ITextPointer position)
		{
			return TextRangeBase.Contains(this, position);
		}

		// Token: 0x06005817 RID: 22551 RVA: 0x002713D6 File Offset: 0x002703D6
		void ITextRange.Select(ITextPointer position1, ITextPointer position2)
		{
			TextRangeBase.Select(this, position1, position2);
		}

		// Token: 0x06005818 RID: 22552 RVA: 0x002713E0 File Offset: 0x002703E0
		void ITextRange.SelectWord(ITextPointer position)
		{
			TextRangeBase.SelectWord(this, position);
		}

		// Token: 0x06005819 RID: 22553 RVA: 0x002713E9 File Offset: 0x002703E9
		void ITextRange.SelectParagraph(ITextPointer position)
		{
			TextRangeBase.SelectParagraph(this, position);
		}

		// Token: 0x0600581A RID: 22554 RVA: 0x002713F2 File Offset: 0x002703F2
		void ITextRange.ApplyTypingHeuristics(bool overType)
		{
			TextRangeBase.ApplyTypingHeuristics(this, overType);
		}

		// Token: 0x0600581B RID: 22555 RVA: 0x002713FB File Offset: 0x002703FB
		object ITextRange.GetPropertyValue(DependencyProperty formattingProperty)
		{
			return TextRangeBase.GetPropertyValue(this, formattingProperty);
		}

		// Token: 0x0600581C RID: 22556 RVA: 0x00271404 File Offset: 0x00270404
		UIElement ITextRange.GetUIElementSelected()
		{
			return TextRangeBase.GetUIElementSelected(this);
		}

		// Token: 0x0600581D RID: 22557 RVA: 0x0027140C File Offset: 0x0027040C
		bool ITextRange.CanSave(string dataFormat)
		{
			return TextRangeBase.CanSave(this, dataFormat);
		}

		// Token: 0x0600581E RID: 22558 RVA: 0x00271415 File Offset: 0x00270415
		void ITextRange.Save(Stream stream, string dataFormat)
		{
			TextRangeBase.Save(this, stream, dataFormat, false);
		}

		// Token: 0x0600581F RID: 22559 RVA: 0x00271420 File Offset: 0x00270420
		void ITextRange.Save(Stream stream, string dataFormat, bool preserveTextElements)
		{
			TextRangeBase.Save(this, stream, dataFormat, preserveTextElements);
		}

		// Token: 0x06005820 RID: 22560 RVA: 0x0027142B File Offset: 0x0027042B
		void ITextRange.BeginChange()
		{
			TextRangeBase.BeginChange(this);
		}

		// Token: 0x06005821 RID: 22561 RVA: 0x00271433 File Offset: 0x00270433
		void ITextRange.BeginChangeNoUndo()
		{
			TextRangeBase.BeginChangeNoUndo(this);
		}

		// Token: 0x06005822 RID: 22562 RVA: 0x0027143B File Offset: 0x0027043B
		void ITextRange.EndChange()
		{
			TextRangeBase.EndChange(this, false, false);
		}

		// Token: 0x06005823 RID: 22563 RVA: 0x00271445 File Offset: 0x00270445
		void ITextRange.EndChange(bool disableScroll, bool skipEvents)
		{
			TextRangeBase.EndChange(this, disableScroll, skipEvents);
		}

		// Token: 0x06005824 RID: 22564 RVA: 0x0027144F File Offset: 0x0027044F
		IDisposable ITextRange.DeclareChangeBlock()
		{
			return new TextRange.ChangeBlock(this, false);
		}

		// Token: 0x06005825 RID: 22565 RVA: 0x00271458 File Offset: 0x00270458
		IDisposable ITextRange.DeclareChangeBlock(bool disableScroll)
		{
			return new TextRange.ChangeBlock(this, disableScroll);
		}

		// Token: 0x06005826 RID: 22566 RVA: 0x00271461 File Offset: 0x00270461
		void ITextRange.NotifyChanged(bool disableScroll, bool skipEvents)
		{
			TextRangeBase.NotifyChanged(this, disableScroll);
		}

		// Token: 0x1700149E RID: 5278
		// (get) Token: 0x06005827 RID: 22567 RVA: 0x0027146A File Offset: 0x0027046A
		bool ITextRange.IgnoreTextUnitBoundaries
		{
			get
			{
				return this.CheckFlags(TextRange.Flags.IgnoreTextUnitBoundaries);
			}
		}

		// Token: 0x1700149F RID: 5279
		// (get) Token: 0x06005828 RID: 22568 RVA: 0x00271473 File Offset: 0x00270473
		ITextPointer ITextRange.Start
		{
			get
			{
				return TextRangeBase.GetStart(this);
			}
		}

		// Token: 0x170014A0 RID: 5280
		// (get) Token: 0x06005829 RID: 22569 RVA: 0x0027147B File Offset: 0x0027047B
		ITextPointer ITextRange.End
		{
			get
			{
				return TextRangeBase.GetEnd(this);
			}
		}

		// Token: 0x170014A1 RID: 5281
		// (get) Token: 0x0600582A RID: 22570 RVA: 0x00271483 File Offset: 0x00270483
		bool ITextRange.IsEmpty
		{
			get
			{
				return TextRangeBase.GetIsEmpty(this);
			}
		}

		// Token: 0x170014A2 RID: 5282
		// (get) Token: 0x0600582B RID: 22571 RVA: 0x0027148B File Offset: 0x0027048B
		List<TextSegment> ITextRange.TextSegments
		{
			get
			{
				return TextRangeBase.GetTextSegments(this);
			}
		}

		// Token: 0x170014A3 RID: 5283
		// (get) Token: 0x0600582C RID: 22572 RVA: 0x00271494 File Offset: 0x00270494
		bool ITextRange.HasConcreteTextContainer
		{
			get
			{
				Invariant.Assert(this._textSegments != null, "_textSegments must not be null");
				Invariant.Assert(this._textSegments.Count > 0, "_textSegments.Count must be > 0");
				return this._textSegments[0].Start is TextPointer;
			}
		}

		// Token: 0x170014A4 RID: 5284
		// (get) Token: 0x0600582D RID: 22573 RVA: 0x002714E8 File Offset: 0x002704E8
		// (set) Token: 0x0600582E RID: 22574 RVA: 0x002714F0 File Offset: 0x002704F0
		string ITextRange.Text
		{
			get
			{
				return TextRangeBase.GetText(this);
			}
			set
			{
				TextRangeBase.SetText(this, value);
			}
		}

		// Token: 0x170014A5 RID: 5285
		// (get) Token: 0x0600582F RID: 22575 RVA: 0x002714F9 File Offset: 0x002704F9
		string ITextRange.Xml
		{
			get
			{
				return TextRangeBase.GetXml(this);
			}
		}

		// Token: 0x170014A6 RID: 5286
		// (get) Token: 0x06005830 RID: 22576 RVA: 0x00271501 File Offset: 0x00270501
		int ITextRange.ChangeBlockLevel
		{
			get
			{
				return TextRangeBase.GetChangeBlockLevel(this);
			}
		}

		// Token: 0x170014A7 RID: 5287
		// (get) Token: 0x06005831 RID: 22577 RVA: 0x00271509 File Offset: 0x00270509
		bool ITextRange.IsTableCellRange
		{
			get
			{
				return TextRangeBase.GetIsTableCellRange(this);
			}
		}

		// Token: 0x140000CF RID: 207
		// (add) Token: 0x06005832 RID: 22578 RVA: 0x00271511 File Offset: 0x00270511
		// (remove) Token: 0x06005833 RID: 22579 RVA: 0x0027151A File Offset: 0x0027051A
		event EventHandler ITextRange.Changed
		{
			add
			{
				this.Changed += value;
			}
			remove
			{
				this.Changed -= value;
			}
		}

		// Token: 0x06005834 RID: 22580 RVA: 0x00271523 File Offset: 0x00270523
		void ITextRange.FireChanged()
		{
			if (this.Changed != null)
			{
				this.Changed(this, EventArgs.Empty);
			}
		}

		// Token: 0x170014A8 RID: 5288
		// (get) Token: 0x06005835 RID: 22581 RVA: 0x0027153E File Offset: 0x0027053E
		// (set) Token: 0x06005836 RID: 22582 RVA: 0x00271547 File Offset: 0x00270547
		bool ITextRange._IsTableCellRange
		{
			get
			{
				return this.CheckFlags(TextRange.Flags.IsTableCellRange);
			}
			set
			{
				this.SetFlags(value, TextRange.Flags.IsTableCellRange);
			}
		}

		// Token: 0x170014A9 RID: 5289
		// (get) Token: 0x06005837 RID: 22583 RVA: 0x00271551 File Offset: 0x00270551
		// (set) Token: 0x06005838 RID: 22584 RVA: 0x00271559 File Offset: 0x00270559
		List<TextSegment> ITextRange._TextSegments
		{
			get
			{
				return this._textSegments;
			}
			set
			{
				this._textSegments = value;
			}
		}

		// Token: 0x170014AA RID: 5290
		// (get) Token: 0x06005839 RID: 22585 RVA: 0x00271562 File Offset: 0x00270562
		// (set) Token: 0x0600583A RID: 22586 RVA: 0x0027156A File Offset: 0x0027056A
		int ITextRange._ChangeBlockLevel
		{
			get
			{
				return this._changeBlockLevel;
			}
			set
			{
				this._changeBlockLevel = value;
			}
		}

		// Token: 0x170014AB RID: 5291
		// (get) Token: 0x0600583B RID: 22587 RVA: 0x00271573 File Offset: 0x00270573
		// (set) Token: 0x0600583C RID: 22588 RVA: 0x0027157B File Offset: 0x0027057B
		ChangeBlockUndoRecord ITextRange._ChangeBlockUndoRecord
		{
			get
			{
				return this._changeBlockUndoRecord;
			}
			set
			{
				this._changeBlockUndoRecord = value;
			}
		}

		// Token: 0x170014AC RID: 5292
		// (get) Token: 0x0600583D RID: 22589 RVA: 0x00271584 File Offset: 0x00270584
		// (set) Token: 0x0600583E RID: 22590 RVA: 0x0027158C File Offset: 0x0027058C
		bool ITextRange._IsChanged
		{
			get
			{
				return this._IsChanged;
			}
			set
			{
				this._IsChanged = value;
			}
		}

		// Token: 0x170014AD RID: 5293
		// (get) Token: 0x0600583F RID: 22591 RVA: 0x00271595 File Offset: 0x00270595
		// (set) Token: 0x06005840 RID: 22592 RVA: 0x0027159D File Offset: 0x0027059D
		uint ITextRange._ContentGeneration
		{
			get
			{
				return this._ContentGeneration;
			}
			set
			{
				this._ContentGeneration = value;
			}
		}

		// Token: 0x06005841 RID: 22593 RVA: 0x002715A6 File Offset: 0x002705A6
		public bool Contains(TextPointer textPointer)
		{
			return ((ITextRange)this).Contains(textPointer);
		}

		// Token: 0x06005842 RID: 22594 RVA: 0x002715AF File Offset: 0x002705AF
		public void Select(TextPointer position1, TextPointer position2)
		{
			((ITextRange)this).Select(position1, position2);
		}

		// Token: 0x06005843 RID: 22595 RVA: 0x002715B9 File Offset: 0x002705B9
		internal void SelectWord(TextPointer textPointer)
		{
			((ITextRange)this).SelectWord(textPointer);
		}

		// Token: 0x06005844 RID: 22596 RVA: 0x002715C2 File Offset: 0x002705C2
		internal void SelectParagraph(ITextPointer position)
		{
			((ITextRange)this).SelectParagraph(position);
		}

		// Token: 0x06005845 RID: 22597 RVA: 0x002715CB File Offset: 0x002705CB
		public void ApplyPropertyValue(DependencyProperty formattingProperty, object value)
		{
			this.ApplyPropertyValue(formattingProperty, value, false, PropertyValueAction.SetValue);
		}

		// Token: 0x06005846 RID: 22598 RVA: 0x002715D7 File Offset: 0x002705D7
		internal void ApplyPropertyValue(DependencyProperty formattingProperty, object value, bool applyToParagraphs)
		{
			this.ApplyPropertyValue(formattingProperty, value, applyToParagraphs, PropertyValueAction.SetValue);
		}

		// Token: 0x06005847 RID: 22599 RVA: 0x002715E4 File Offset: 0x002705E4
		internal void ApplyPropertyValue(DependencyProperty formattingProperty, object value, bool applyToParagraphs, PropertyValueAction propertyValueAction)
		{
			Invariant.Assert(this.HasConcreteTextContainer, "Can't apply property to non-TextContainer range!");
			if (formattingProperty == null)
			{
				throw new ArgumentNullException("formattingProperty");
			}
			if (!TextSchema.IsCharacterProperty(formattingProperty) && !TextSchema.IsParagraphProperty(formattingProperty))
			{
				throw new ArgumentException(SR.Get("TextEditorPropertyIsNotApplicableForTextFormatting", new object[]
				{
					formattingProperty.Name
				}));
			}
			if (value is string && formattingProperty.PropertyType != typeof(string))
			{
				TypeConverter converter = TypeDescriptor.GetConverter(formattingProperty.PropertyType);
				Invariant.Assert(converter != null);
				value = converter.ConvertFromString((string)value);
			}
			if (!formattingProperty.IsValidValue(value) && (!(formattingProperty.PropertyType == typeof(Thickness)) || !(value is Thickness)))
			{
				throw new ArgumentException(SR.Get("TextEditorTypeOfParameterIsNotAppropriateForFormattingProperty", new object[]
				{
					(value == null) ? "null" : value.GetType().Name,
					formattingProperty.Name
				}), "value");
			}
			if (propertyValueAction != PropertyValueAction.SetValue && propertyValueAction != PropertyValueAction.IncreaseByAbsoluteValue && propertyValueAction != PropertyValueAction.DecreaseByAbsoluteValue && propertyValueAction != PropertyValueAction.IncreaseByPercentageValue && propertyValueAction != PropertyValueAction.DecreaseByPercentageValue)
			{
				throw new ArgumentException(SR.Get("TextRange_InvalidParameterValue"), "propertyValueAction");
			}
			if (propertyValueAction != PropertyValueAction.SetValue && !TextSchema.IsPropertyIncremental(formattingProperty))
			{
				throw new ArgumentException(SR.Get("TextRange_PropertyCannotBeIncrementedOrDecremented", new object[]
				{
					formattingProperty.Name
				}), "propertyValueAction");
			}
			this.ApplyPropertyToTextVirtual(formattingProperty, value, applyToParagraphs, propertyValueAction);
		}

		// Token: 0x06005848 RID: 22600 RVA: 0x0027174A File Offset: 0x0027074A
		public void ClearAllProperties()
		{
			Invariant.Assert(this.HasConcreteTextContainer, "Can't clear properties in non-TextContainer range");
			this.ClearAllPropertiesVirtual();
		}

		// Token: 0x06005849 RID: 22601 RVA: 0x00271764 File Offset: 0x00270764
		public object GetPropertyValue(DependencyProperty formattingProperty)
		{
			if (formattingProperty == null)
			{
				throw new ArgumentNullException("formattingProperty");
			}
			if (!TextSchema.IsCharacterProperty(formattingProperty) && !TextSchema.IsParagraphProperty(formattingProperty))
			{
				throw new ArgumentException(SR.Get("TextEditorPropertyIsNotApplicableForTextFormatting", new object[]
				{
					formattingProperty.Name
				}));
			}
			return ((ITextRange)this).GetPropertyValue(formattingProperty);
		}

		// Token: 0x0600584A RID: 22602 RVA: 0x002717B5 File Offset: 0x002707B5
		internal UIElement GetUIElementSelected()
		{
			return ((ITextRange)this).GetUIElementSelected();
		}

		// Token: 0x0600584B RID: 22603 RVA: 0x002717BD File Offset: 0x002707BD
		public bool CanSave(string dataFormat)
		{
			return ((ITextRange)this).CanSave(dataFormat);
		}

		// Token: 0x0600584C RID: 22604 RVA: 0x002717C6 File Offset: 0x002707C6
		public bool CanLoad(string dataFormat)
		{
			return TextRangeBase.CanLoad(this, dataFormat);
		}

		// Token: 0x0600584D RID: 22605 RVA: 0x002717CF File Offset: 0x002707CF
		public void Save(Stream stream, string dataFormat)
		{
			((ITextRange)this).Save(stream, dataFormat);
		}

		// Token: 0x0600584E RID: 22606 RVA: 0x002717D9 File Offset: 0x002707D9
		public void Save(Stream stream, string dataFormat, bool preserveTextElements)
		{
			((ITextRange)this).Save(stream, dataFormat, preserveTextElements);
		}

		// Token: 0x0600584F RID: 22607 RVA: 0x002717E4 File Offset: 0x002707E4
		public void Load(Stream stream, string dataFormat)
		{
			this.LoadVirtual(stream, dataFormat);
		}

		// Token: 0x06005850 RID: 22608 RVA: 0x002717EE File Offset: 0x002707EE
		internal void InsertEmbeddedUIElement(FrameworkElement embeddedElement)
		{
			Invariant.Assert(embeddedElement != null);
			this.InsertEmbeddedUIElementVirtual(embeddedElement);
		}

		// Token: 0x06005851 RID: 22609 RVA: 0x00271800 File Offset: 0x00270800
		internal void InsertImage(Image image)
		{
			BitmapSource bitmapSource = (BitmapSource)image.Source;
			Invariant.Assert(bitmapSource != null);
			if (double.IsNaN(image.Height))
			{
				if ((double)bitmapSource.PixelHeight < 300.0)
				{
					image.Height = (double)bitmapSource.PixelHeight;
				}
				else
				{
					image.Height = 300.0;
				}
			}
			if (double.IsNaN(image.Width))
			{
				if ((double)bitmapSource.PixelHeight < 300.0)
				{
					image.Width = (double)bitmapSource.PixelWidth;
				}
				else
				{
					image.Width = 300.0 / (double)bitmapSource.PixelHeight * (double)bitmapSource.PixelWidth;
				}
			}
			this.InsertEmbeddedUIElement(image);
		}

		// Token: 0x06005852 RID: 22610 RVA: 0x002718B3 File Offset: 0x002708B3
		internal virtual void SetXmlVirtual(TextElement fragment)
		{
			if (!this.IsTableCellRange)
			{
				TextRangeSerialization.PasteXml(this, fragment);
			}
		}

		// Token: 0x06005853 RID: 22611 RVA: 0x002718C4 File Offset: 0x002708C4
		internal virtual void LoadVirtual(Stream stream, string dataFormat)
		{
			TextRangeBase.Load(this, stream, dataFormat);
		}

		// Token: 0x06005854 RID: 22612 RVA: 0x002718CE File Offset: 0x002708CE
		internal Table InsertTable(int rowCount, int columnCount)
		{
			Invariant.Assert(this.HasConcreteTextContainer, "InsertTable: TextRange must belong to non-abstract TextContainer");
			return this.InsertTableVirtual(rowCount, columnCount);
		}

		// Token: 0x06005855 RID: 22613 RVA: 0x002718E8 File Offset: 0x002708E8
		internal TextRange InsertRows(int rowCount)
		{
			Invariant.Assert(this.HasConcreteTextContainer, "InsertRows: TextRange must belong to non-abstract TextContainer");
			return this.InsertRowsVirtual(rowCount);
		}

		// Token: 0x06005856 RID: 22614 RVA: 0x00271901 File Offset: 0x00270901
		internal bool DeleteRows()
		{
			Invariant.Assert(this.HasConcreteTextContainer, "DeleteRows: TextRange must belong to non-abstract TextContainer");
			return this.DeleteRowsVirtual();
		}

		// Token: 0x06005857 RID: 22615 RVA: 0x00271919 File Offset: 0x00270919
		internal TextRange InsertColumns(int columnCount)
		{
			Invariant.Assert(this.HasConcreteTextContainer, "InsertColumns: TextRange must belong to non-abstract TextContainer");
			return this.InsertColumnsVirtual(columnCount);
		}

		// Token: 0x06005858 RID: 22616 RVA: 0x00271932 File Offset: 0x00270932
		internal bool DeleteColumns()
		{
			Invariant.Assert(this.HasConcreteTextContainer, "DeleteColumns: TextRange must belong to non-abstract TextContainer");
			return this.DeleteColumnsVirtual();
		}

		// Token: 0x06005859 RID: 22617 RVA: 0x0027194A File Offset: 0x0027094A
		internal TextRange MergeCells()
		{
			Invariant.Assert(this.HasConcreteTextContainer, "MergeCells: TextRange must belong to non-abstract TextContainer");
			return this.MergeCellsVirtual();
		}

		// Token: 0x0600585A RID: 22618 RVA: 0x00271962 File Offset: 0x00270962
		internal TextRange SplitCell(int splitCountHorizontal, int splitCountVertical)
		{
			Invariant.Assert(this.HasConcreteTextContainer, "SplitCells: TextRange must belong to non-abstract TextContainer");
			return this.SplitCellVirtual(splitCountHorizontal, splitCountVertical);
		}

		// Token: 0x170014AE RID: 5294
		// (get) Token: 0x0600585B RID: 22619 RVA: 0x0027197C File Offset: 0x0027097C
		public TextPointer Start
		{
			get
			{
				return (TextPointer)((ITextRange)this).Start;
			}
		}

		// Token: 0x170014AF RID: 5295
		// (get) Token: 0x0600585C RID: 22620 RVA: 0x00271989 File Offset: 0x00270989
		public TextPointer End
		{
			get
			{
				return (TextPointer)((ITextRange)this).End;
			}
		}

		// Token: 0x170014B0 RID: 5296
		// (get) Token: 0x0600585D RID: 22621 RVA: 0x00271996 File Offset: 0x00270996
		public bool IsEmpty
		{
			get
			{
				return ((ITextRange)this).IsEmpty;
			}
		}

		// Token: 0x170014B1 RID: 5297
		// (get) Token: 0x0600585E RID: 22622 RVA: 0x0027199E File Offset: 0x0027099E
		internal bool HasConcreteTextContainer
		{
			get
			{
				return ((ITextRange)this).HasConcreteTextContainer;
			}
		}

		// Token: 0x170014B2 RID: 5298
		// (get) Token: 0x0600585F RID: 22623 RVA: 0x002719A6 File Offset: 0x002709A6
		internal FrameworkElement ContainingFrameworkElement
		{
			get
			{
				if (this.HasConcreteTextContainer)
				{
					return this.Start.ContainingFrameworkElement;
				}
				return null;
			}
		}

		// Token: 0x170014B3 RID: 5299
		// (get) Token: 0x06005860 RID: 22624 RVA: 0x002719BD File Offset: 0x002709BD
		// (set) Token: 0x06005861 RID: 22625 RVA: 0x002719C5 File Offset: 0x002709C5
		public string Text
		{
			get
			{
				return ((ITextRange)this).Text;
			}
			set
			{
				((ITextRange)this).Text = value;
			}
		}

		// Token: 0x170014B4 RID: 5300
		// (get) Token: 0x06005862 RID: 22626 RVA: 0x002719CE File Offset: 0x002709CE
		// (set) Token: 0x06005863 RID: 22627 RVA: 0x002719D8 File Offset: 0x002709D8
		internal string Xml
		{
			get
			{
				return ((ITextRange)this).Xml;
			}
			set
			{
				TextRangeBase.BeginChange(this);
				try
				{
					TextElement textElement = XamlReader.Load(new XmlTextReader(new StringReader(value)), this._useRestrictiveXamlXmlReader) as TextElement;
					if (textElement != null)
					{
						this.SetXmlVirtual(textElement);
					}
				}
				finally
				{
					TextRangeBase.EndChange(this);
				}
			}
		}

		// Token: 0x170014B5 RID: 5301
		// (get) Token: 0x06005864 RID: 22628 RVA: 0x00271A2C File Offset: 0x00270A2C
		internal bool IsTableCellRange
		{
			get
			{
				return ((ITextRange)this).IsTableCellRange;
			}
		}

		// Token: 0x140000D0 RID: 208
		// (add) Token: 0x06005865 RID: 22629 RVA: 0x00271A34 File Offset: 0x00270A34
		// (remove) Token: 0x06005866 RID: 22630 RVA: 0x00271A6C File Offset: 0x00270A6C
		public event EventHandler Changed;

		// Token: 0x06005867 RID: 22631 RVA: 0x00271AA1 File Offset: 0x00270AA1
		internal void BeginChange()
		{
			((ITextRange)this).BeginChange();
		}

		// Token: 0x06005868 RID: 22632 RVA: 0x00271AA9 File Offset: 0x00270AA9
		internal void EndChange()
		{
			((ITextRange)this).EndChange();
		}

		// Token: 0x06005869 RID: 22633 RVA: 0x00271AB1 File Offset: 0x00270AB1
		internal IDisposable DeclareChangeBlock()
		{
			return ((ITextRange)this).DeclareChangeBlock();
		}

		// Token: 0x0600586A RID: 22634 RVA: 0x00271AB9 File Offset: 0x00270AB9
		internal IDisposable DeclareChangeBlock(bool disableScroll)
		{
			return ((ITextRange)this).DeclareChangeBlock(disableScroll);
		}

		// Token: 0x170014B6 RID: 5302
		// (get) Token: 0x0600586B RID: 22635 RVA: 0x00271AC2 File Offset: 0x00270AC2
		// (set) Token: 0x0600586C RID: 22636 RVA: 0x00271ACB File Offset: 0x00270ACB
		internal bool _IsChanged
		{
			get
			{
				return this.CheckFlags(TextRange.Flags.IsChanged);
			}
			set
			{
				this.SetFlags(value, TextRange.Flags.IsChanged);
			}
		}

		// Token: 0x0600586D RID: 22637 RVA: 0x00271AD8 File Offset: 0x00270AD8
		internal virtual void InsertEmbeddedUIElementVirtual(FrameworkElement embeddedElement)
		{
			Invariant.Assert(this.HasConcreteTextContainer, "Can't insert embedded object to non-TextContainer range!");
			Invariant.Assert(embeddedElement != null);
			TextRangeBase.BeginChange(this);
			try
			{
				this.Text = string.Empty;
				Paragraph paragraph = TextRangeEditTables.EnsureInsertionPosition(this.Start).Paragraph;
				if (paragraph != null)
				{
					if (Paragraph.HasNoTextContent(paragraph))
					{
						BlockUIContainer blockUIContainer = new BlockUIContainer(embeddedElement);
						blockUIContainer.TextAlignment = TextRangeEdit.GetTextAlignmentFromHorizontalAlignment(embeddedElement.HorizontalAlignment);
						paragraph.SiblingBlocks.InsertAfter(paragraph, blockUIContainer);
						paragraph.SiblingBlocks.Remove(paragraph);
						this.Select(blockUIContainer.ContentStart, blockUIContainer.ContentEnd);
					}
					else
					{
						InlineUIContainer inlineUIContainer = new InlineUIContainer(embeddedElement);
						TextRangeEdit.SplitFormattingElements(this.Start, false).InsertTextElement(inlineUIContainer);
						this.Select(inlineUIContainer.ElementStart, inlineUIContainer.ElementEnd);
					}
				}
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x0600586E RID: 22638 RVA: 0x00271BB8 File Offset: 0x00270BB8
		internal virtual void ApplyPropertyToTextVirtual(DependencyProperty formattingProperty, object value, bool applyToParagraphs, PropertyValueAction propertyValueAction)
		{
			TextRangeBase.BeginChange(this);
			try
			{
				for (int i = 0; i < this._textSegments.Count; i++)
				{
					TextSegment textSegment = this._textSegments[i];
					if (formattingProperty == FrameworkElement.FlowDirectionProperty)
					{
						if (applyToParagraphs || this.IsEmpty || TextRangeBase.IsParagraphBoundaryCrossed(this))
						{
							TextRangeEdit.SetParagraphProperty((TextPointer)textSegment.Start, (TextPointer)textSegment.End, formattingProperty, value, propertyValueAction);
						}
						else
						{
							TextRangeEdit.SetInlineProperty((TextPointer)textSegment.Start, (TextPointer)textSegment.End, formattingProperty, value, propertyValueAction);
						}
					}
					else if (TextSchema.IsCharacterProperty(formattingProperty))
					{
						TextRangeEdit.SetInlineProperty((TextPointer)textSegment.Start, (TextPointer)textSegment.End, formattingProperty, value, propertyValueAction);
					}
					else if (TextSchema.IsParagraphProperty(formattingProperty))
					{
						if (formattingProperty.PropertyType == typeof(Thickness) && (FlowDirection)textSegment.Start.GetValue(Block.FlowDirectionProperty) == FlowDirection.RightToLeft)
						{
							value = new Thickness(((Thickness)value).Right, ((Thickness)value).Top, ((Thickness)value).Left, ((Thickness)value).Bottom);
						}
						TextRangeEdit.SetParagraphProperty((TextPointer)textSegment.Start, (TextPointer)textSegment.End, formattingProperty, value, propertyValueAction);
					}
				}
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x0600586F RID: 22639 RVA: 0x00271D50 File Offset: 0x00270D50
		internal virtual void ClearAllPropertiesVirtual()
		{
			TextRangeBase.BeginChange(this);
			try
			{
				TextRangeEdit.CharacterResetFormatting(this.Start, this.End);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x06005870 RID: 22640 RVA: 0x00271D90 File Offset: 0x00270D90
		internal virtual Table InsertTableVirtual(int rowCount, int columnCount)
		{
			TextRangeBase.BeginChange(this);
			Table result;
			try
			{
				result = TextRangeEditTables.InsertTable(this.End, rowCount, columnCount);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x06005871 RID: 22641 RVA: 0x00271DCC File Offset: 0x00270DCC
		internal virtual TextRange InsertRowsVirtual(int rowCount)
		{
			TextRangeBase.BeginChange(this);
			TextRange result;
			try
			{
				result = TextRangeEditTables.InsertRows(this, rowCount);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x06005872 RID: 22642 RVA: 0x00271E04 File Offset: 0x00270E04
		internal virtual bool DeleteRowsVirtual()
		{
			TextRangeBase.BeginChange(this);
			bool result;
			try
			{
				result = TextRangeEditTables.DeleteRows(this);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x06005873 RID: 22643 RVA: 0x00271E38 File Offset: 0x00270E38
		internal virtual TextRange InsertColumnsVirtual(int columnCount)
		{
			TextRangeBase.BeginChange(this);
			TextRange result;
			try
			{
				result = TextRangeEditTables.InsertColumns(this, columnCount);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x06005874 RID: 22644 RVA: 0x00271E70 File Offset: 0x00270E70
		internal virtual bool DeleteColumnsVirtual()
		{
			TextRangeBase.BeginChange(this);
			bool result;
			try
			{
				result = TextRangeEditTables.DeleteColumns(this);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x06005875 RID: 22645 RVA: 0x00271EA4 File Offset: 0x00270EA4
		internal virtual TextRange MergeCellsVirtual()
		{
			TextRangeBase.BeginChange(this);
			TextRange result;
			try
			{
				result = TextRangeEditTables.MergeCells(this);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x06005876 RID: 22646 RVA: 0x00271ED8 File Offset: 0x00270ED8
		internal virtual TextRange SplitCellVirtual(int splitCountHorizontal, int splitCountVertical)
		{
			TextRangeBase.BeginChange(this);
			TextRange result;
			try
			{
				result = TextRangeEditTables.SplitCell(this, splitCountHorizontal, splitCountVertical);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x170014B7 RID: 5303
		// (get) Token: 0x06005877 RID: 22647 RVA: 0x00271562 File Offset: 0x00270562
		internal int ChangeBlockLevel
		{
			get
			{
				return this._changeBlockLevel;
			}
		}

		// Token: 0x06005878 RID: 22648 RVA: 0x00271F10 File Offset: 0x00270F10
		private void SetFlags(bool value, TextRange.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x06005879 RID: 22649 RVA: 0x00271F2E File Offset: 0x00270F2E
		private bool CheckFlags(TextRange.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x04002FE0 RID: 12256
		private List<TextSegment> _textSegments;

		// Token: 0x04002FE1 RID: 12257
		private int _changeBlockLevel;

		// Token: 0x04002FE2 RID: 12258
		private ChangeBlockUndoRecord _changeBlockUndoRecord;

		// Token: 0x04002FE3 RID: 12259
		private uint _ContentGeneration;

		// Token: 0x04002FE4 RID: 12260
		private TextRange.Flags _flags;

		// Token: 0x04002FE5 RID: 12261
		private bool _useRestrictiveXamlXmlReader;

		// Token: 0x02000B6D RID: 2925
		private class ChangeBlock : IDisposable
		{
			// Token: 0x06008DF4 RID: 36340 RVA: 0x0033FEA0 File Offset: 0x0033EEA0
			internal ChangeBlock(ITextRange range, bool disableScroll)
			{
				this._range = range;
				this._disableScroll = disableScroll;
				this._range.BeginChange();
			}

			// Token: 0x06008DF5 RID: 36341 RVA: 0x0033FEC1 File Offset: 0x0033EEC1
			void IDisposable.Dispose()
			{
				this._range.EndChange(this._disableScroll, false);
				GC.SuppressFinalize(this);
			}

			// Token: 0x040048D9 RID: 18649
			private readonly ITextRange _range;

			// Token: 0x040048DA RID: 18650
			private readonly bool _disableScroll;
		}

		// Token: 0x02000B6E RID: 2926
		[Flags]
		private enum Flags
		{
			// Token: 0x040048DC RID: 18652
			IgnoreTextUnitBoundaries = 1,
			// Token: 0x040048DD RID: 18653
			IsChanged = 2,
			// Token: 0x040048DE RID: 18654
			IsTableCellRange = 4
		}
	}
}
