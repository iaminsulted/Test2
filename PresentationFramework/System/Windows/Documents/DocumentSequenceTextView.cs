using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x020005EB RID: 1515
	internal sealed class DocumentSequenceTextView : TextViewBase
	{
		// Token: 0x06004996 RID: 18838 RVA: 0x002309E6 File Offset: 0x0022F9E6
		internal DocumentSequenceTextView(FixedDocumentSequenceDocumentPage docPage)
		{
			this._docPage = docPage;
		}

		// Token: 0x06004997 RID: 18839 RVA: 0x002309F8 File Offset: 0x0022F9F8
		internal override ITextPointer GetTextPositionFromPoint(Point point, bool snapToText)
		{
			DocumentSequenceTextPointer documentSequenceTextPointer = null;
			LogicalDirection gravity = LogicalDirection.Forward;
			if (this.ChildTextView != null)
			{
				ITextPointer textPositionFromPoint = this.ChildTextView.GetTextPositionFromPoint(point, snapToText);
				if (textPositionFromPoint != null)
				{
					documentSequenceTextPointer = new DocumentSequenceTextPointer(this.ChildBlock, textPositionFromPoint);
					gravity = textPositionFromPoint.LogicalDirection;
				}
			}
			if (documentSequenceTextPointer != null)
			{
				return DocumentSequenceTextPointer.CreatePointer(documentSequenceTextPointer, gravity);
			}
			return null;
		}

		// Token: 0x06004998 RID: 18840 RVA: 0x00230A44 File Offset: 0x0022FA44
		internal override Rect GetRawRectangleFromTextPosition(ITextPointer position, out Transform transform)
		{
			DocumentSequenceTextPointer documentSequenceTextPointer = null;
			transform = Transform.Identity;
			if (position != null)
			{
				documentSequenceTextPointer = this._docPage.FixedDocumentSequence.TextContainer.VerifyPosition(position);
			}
			if (documentSequenceTextPointer != null && this.ChildTextView != null && this.ChildTextView.TextContainer == documentSequenceTextPointer.ChildBlock.ChildContainer)
			{
				return this.ChildTextView.GetRawRectangleFromTextPosition(documentSequenceTextPointer.ChildPointer.CreatePointer(position.LogicalDirection), out transform);
			}
			return Rect.Empty;
		}

		// Token: 0x06004999 RID: 18841 RVA: 0x00230ABC File Offset: 0x0022FABC
		internal override Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
		{
			if (startPosition != null && endPosition != null && this.ChildTextView != null)
			{
				DocumentSequenceTextPointer documentSequenceTextPointer = this._docPage.FixedDocumentSequence.TextContainer.VerifyPosition(startPosition);
				DocumentSequenceTextPointer documentSequenceTextPointer2 = this._docPage.FixedDocumentSequence.TextContainer.VerifyPosition(endPosition);
				if (documentSequenceTextPointer != null && documentSequenceTextPointer2 != null)
				{
					return this.ChildTextView.GetTightBoundingGeometryFromTextPositions(documentSequenceTextPointer.ChildPointer, documentSequenceTextPointer2.ChildPointer);
				}
			}
			return new PathGeometry();
		}

		// Token: 0x0600499A RID: 18842 RVA: 0x00230B2C File Offset: 0x0022FB2C
		internal override ITextPointer GetPositionAtNextLine(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved)
		{
			newSuggestedX = suggestedX;
			linesMoved = count;
			DocumentSequenceTextPointer thisTp = null;
			LogicalDirection gravity = LogicalDirection.Forward;
			DocumentSequenceTextPointer documentSequenceTextPointer = null;
			if (position != null)
			{
				documentSequenceTextPointer = this._docPage.FixedDocumentSequence.TextContainer.VerifyPosition(position);
			}
			if (documentSequenceTextPointer != null && this.ChildTextView != null && this.ChildTextView.TextContainer == documentSequenceTextPointer.ChildBlock.ChildContainer)
			{
				ITextPointer positionAtNextLine = this.ChildTextView.GetPositionAtNextLine(documentSequenceTextPointer.ChildPointer.CreatePointer(position.LogicalDirection), suggestedX, count, out newSuggestedX, out linesMoved);
				if (positionAtNextLine != null)
				{
					thisTp = new DocumentSequenceTextPointer(this.ChildBlock, positionAtNextLine);
					gravity = positionAtNextLine.LogicalDirection;
				}
			}
			return DocumentSequenceTextPointer.CreatePointer(thisTp, gravity);
		}

		// Token: 0x0600499B RID: 18843 RVA: 0x00230BC8 File Offset: 0x0022FBC8
		internal override bool IsAtCaretUnitBoundary(ITextPointer position)
		{
			Invariant.Assert(position != null);
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			Invariant.Assert(this.ChildTextView != null);
			DocumentSequenceTextPointer documentSequenceTextPointer = this.DocumentSequenceTextContainer.VerifyPosition(position);
			return this.ChildTextView.IsAtCaretUnitBoundary(documentSequenceTextPointer.ChildPointer);
		}

		// Token: 0x0600499C RID: 18844 RVA: 0x00230C18 File Offset: 0x0022FC18
		internal override ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			Invariant.Assert(position != null);
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			Invariant.Assert(this.ChildTextView != null);
			DocumentSequenceTextPointer documentSequenceTextPointer = this.DocumentSequenceTextContainer.VerifyPosition(position);
			return this.ChildTextView.GetNextCaretUnitPosition(documentSequenceTextPointer.ChildPointer, direction);
		}

		// Token: 0x0600499D RID: 18845 RVA: 0x00230C6C File Offset: 0x0022FC6C
		internal override ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			Invariant.Assert(position != null);
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			Invariant.Assert(this.ChildTextView != null);
			DocumentSequenceTextPointer documentSequenceTextPointer = this.DocumentSequenceTextContainer.VerifyPosition(position);
			return this.ChildTextView.GetBackspaceCaretUnitPosition(documentSequenceTextPointer.ChildPointer);
		}

		// Token: 0x0600499E RID: 18846 RVA: 0x00230CBC File Offset: 0x0022FCBC
		internal override TextSegment GetLineRange(ITextPointer position)
		{
			if (position != null && this.ChildTextView != null)
			{
				DocumentSequenceTextPointer documentSequenceTextPointer = this._docPage.FixedDocumentSequence.TextContainer.VerifyPosition(position);
				if (this.ChildTextView.TextContainer == documentSequenceTextPointer.ChildBlock.ChildContainer)
				{
					TextSegment lineRange = this.ChildTextView.GetLineRange(documentSequenceTextPointer.ChildPointer.CreatePointer(position.LogicalDirection));
					if (!lineRange.IsNull)
					{
						ITextPointer startPosition = new DocumentSequenceTextPointer(this.ChildBlock, lineRange.Start);
						DocumentSequenceTextPointer endPosition = new DocumentSequenceTextPointer(this.ChildBlock, lineRange.End);
						return new TextSegment(startPosition, endPosition, true);
					}
				}
			}
			return TextSegment.Null;
		}

		// Token: 0x0600499F RID: 18847 RVA: 0x001056E1 File Offset: 0x001046E1
		internal override ReadOnlyCollection<GlyphRun> GetGlyphRuns(ITextPointer start, ITextPointer end)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060049A0 RID: 18848 RVA: 0x00230D68 File Offset: 0x0022FD68
		internal override bool Contains(ITextPointer position)
		{
			DocumentSequenceTextPointer documentSequenceTextPointer = null;
			if (position != null)
			{
				documentSequenceTextPointer = this._docPage.FixedDocumentSequence.TextContainer.VerifyPosition(position);
			}
			return documentSequenceTextPointer != null && this.ChildTextView != null && this.ChildTextView.TextContainer == documentSequenceTextPointer.ChildBlock.ChildContainer && this.ChildTextView.Contains(documentSequenceTextPointer.ChildPointer.CreatePointer(position.LogicalDirection));
		}

		// Token: 0x060049A1 RID: 18849 RVA: 0x00230DD2 File Offset: 0x0022FDD2
		internal override bool Validate()
		{
			if (this.ChildTextView != null)
			{
				this.ChildTextView.Validate();
			}
			return ((ITextView)this).IsValid;
		}

		// Token: 0x060049A2 RID: 18850 RVA: 0x00230DEE File Offset: 0x0022FDEE
		internal override bool Validate(Point point)
		{
			if (this.ChildTextView != null)
			{
				this.ChildTextView.Validate(point);
			}
			return ((ITextView)this).IsValid;
		}

		// Token: 0x17001087 RID: 4231
		// (get) Token: 0x060049A3 RID: 18851 RVA: 0x00230E0C File Offset: 0x0022FE0C
		internal override UIElement RenderScope
		{
			get
			{
				Visual visual = this._docPage.Visual;
				while (visual != null && !(visual is UIElement))
				{
					visual = (VisualTreeHelper.GetParent(visual) as Visual);
				}
				return visual as UIElement;
			}
		}

		// Token: 0x17001088 RID: 4232
		// (get) Token: 0x060049A4 RID: 18852 RVA: 0x00230E44 File Offset: 0x0022FE44
		internal override ITextContainer TextContainer
		{
			get
			{
				return this._docPage.FixedDocumentSequence.TextContainer;
			}
		}

		// Token: 0x17001089 RID: 4233
		// (get) Token: 0x060049A5 RID: 18853 RVA: 0x00230E56 File Offset: 0x0022FE56
		internal override bool IsValid
		{
			get
			{
				return this.ChildTextView == null || this.ChildTextView.IsValid;
			}
		}

		// Token: 0x1700108A RID: 4234
		// (get) Token: 0x060049A6 RID: 18854 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal override bool RendersOwnSelection
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700108B RID: 4235
		// (get) Token: 0x060049A7 RID: 18855 RVA: 0x00230E70 File Offset: 0x0022FE70
		internal override ReadOnlyCollection<TextSegment> TextSegments
		{
			get
			{
				if (this._textSegments == null)
				{
					ReadOnlyCollection<TextSegment> textSegments = this.ChildTextView.TextSegments;
					if (textSegments != null)
					{
						List<TextSegment> list = new List<TextSegment>(textSegments.Count);
						foreach (TextSegment textSegment in textSegments)
						{
							DocumentSequenceTextPointer startPosition = this._docPage.FixedDocumentSequence.TextContainer.MapChildPositionToParent(textSegment.Start);
							DocumentSequenceTextPointer endPosition = this._docPage.FixedDocumentSequence.TextContainer.MapChildPositionToParent(textSegment.End);
							list.Add(new TextSegment(startPosition, endPosition, true));
						}
						this._textSegments = new ReadOnlyCollection<TextSegment>(list);
					}
				}
				return this._textSegments;
			}
		}

		// Token: 0x1700108C RID: 4236
		// (get) Token: 0x060049A8 RID: 18856 RVA: 0x00230F38 File Offset: 0x0022FF38
		private ITextView ChildTextView
		{
			get
			{
				if (this._childTextView == null)
				{
					IServiceProvider serviceProvider = this._docPage.ChildDocumentPage as IServiceProvider;
					if (serviceProvider != null)
					{
						this._childTextView = (ITextView)serviceProvider.GetService(typeof(ITextView));
					}
				}
				return this._childTextView;
			}
		}

		// Token: 0x1700108D RID: 4237
		// (get) Token: 0x060049A9 RID: 18857 RVA: 0x00230F82 File Offset: 0x0022FF82
		private ChildDocumentBlock ChildBlock
		{
			get
			{
				if (this._childBlock == null)
				{
					this._childBlock = this._docPage.FixedDocumentSequence.TextContainer.FindChildBlock(this._docPage.ChildDocumentReference);
				}
				return this._childBlock;
			}
		}

		// Token: 0x1700108E RID: 4238
		// (get) Token: 0x060049AA RID: 18858 RVA: 0x00230E44 File Offset: 0x0022FE44
		private DocumentSequenceTextContainer DocumentSequenceTextContainer
		{
			get
			{
				return this._docPage.FixedDocumentSequence.TextContainer;
			}
		}

		// Token: 0x04002674 RID: 9844
		private readonly FixedDocumentSequenceDocumentPage _docPage;

		// Token: 0x04002675 RID: 9845
		private ITextView _childTextView;

		// Token: 0x04002676 RID: 9846
		private ReadOnlyCollection<TextSegment> _textSegments;

		// Token: 0x04002677 RID: 9847
		private ChildDocumentBlock _childBlock;
	}
}
