using System;
using System.ComponentModel;
using System.Windows.Markup;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000687 RID: 1671
	[ContentProperty("Inlines")]
	public class Span : Inline
	{
		// Token: 0x060052C7 RID: 21191 RVA: 0x00243005 File Offset: 0x00242005
		public Span()
		{
		}

		// Token: 0x060052C8 RID: 21192 RVA: 0x0025921D File Offset: 0x0025821D
		public Span(Inline childInline) : this(childInline, null)
		{
		}

		// Token: 0x060052C9 RID: 21193 RVA: 0x00259228 File Offset: 0x00258228
		public Span(Inline childInline, TextPointer insertionPosition)
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
				if (childInline != null)
				{
					this.Inlines.Add(childInline);
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

		// Token: 0x060052CA RID: 21194 RVA: 0x00259284 File Offset: 0x00258284
		public Span(TextPointer start, TextPointer end)
		{
			if (start == null)
			{
				throw new ArgumentNullException("start");
			}
			if (end == null)
			{
				throw new ArgumentNullException("start");
			}
			if (start.TextContainer != end.TextContainer)
			{
				throw new ArgumentException(SR.Get("InDifferentTextContainers", new object[]
				{
					"start",
					"end"
				}));
			}
			if (start.CompareTo(end) > 0)
			{
				throw new ArgumentException(SR.Get("BadTextPositionOrder", new object[]
				{
					"start",
					"end"
				}));
			}
			start.TextContainer.BeginChange();
			try
			{
				start = TextRangeEditTables.EnsureInsertionPosition(start);
				Invariant.Assert(start.Parent is Run);
				end = TextRangeEditTables.EnsureInsertionPosition(end);
				Invariant.Assert(end.Parent is Run);
				if (start.Paragraph != end.Paragraph)
				{
					throw new ArgumentException(SR.Get("InDifferentParagraphs", new object[]
					{
						"start",
						"end"
					}));
				}
				Inline nonMergeableInlineAncestor;
				if ((nonMergeableInlineAncestor = start.GetNonMergeableInlineAncestor()) != null)
				{
					throw new InvalidOperationException(SR.Get("TextSchema_CannotSplitElement", new object[]
					{
						nonMergeableInlineAncestor.GetType().Name
					}));
				}
				if ((nonMergeableInlineAncestor = end.GetNonMergeableInlineAncestor()) != null)
				{
					throw new InvalidOperationException(SR.Get("TextSchema_CannotSplitElement", new object[]
					{
						nonMergeableInlineAncestor.GetType().Name
					}));
				}
				TextElement commonAncestor = TextElement.GetCommonAncestor((TextElement)start.Parent, (TextElement)end.Parent);
				while (start.Parent != commonAncestor)
				{
					start = this.SplitElement(start);
				}
				while (end.Parent != commonAncestor)
				{
					end = this.SplitElement(end);
				}
				if (start.Parent is Run)
				{
					start = this.SplitElement(start);
				}
				if (end.Parent is Run)
				{
					end = this.SplitElement(end);
				}
				Invariant.Assert(start.Parent == end.Parent);
				Invariant.Assert(TextSchema.IsValidChild(start, typeof(Span)));
				base.Reposition(start, end);
			}
			finally
			{
				start.TextContainer.EndChange();
			}
		}

		// Token: 0x17001389 RID: 5001
		// (get) Token: 0x060052CB RID: 21195 RVA: 0x002451C0 File Offset: 0x002441C0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public InlineCollection Inlines
		{
			get
			{
				return new InlineCollection(this, true);
			}
		}

		// Token: 0x060052CC RID: 21196 RVA: 0x0022BF83 File Offset: 0x0022AF83
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeInlines(XamlDesignerSerializationManager manager)
		{
			return manager != null && manager.XmlWriter == null;
		}

		// Token: 0x060052CD RID: 21197 RVA: 0x002594B4 File Offset: 0x002584B4
		private TextPointer SplitElement(TextPointer position)
		{
			if (position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				position = position.GetNextContextPosition(LogicalDirection.Backward);
			}
			else if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
			{
				position = position.GetNextContextPosition(LogicalDirection.Forward);
			}
			else
			{
				position = TextRangeEdit.SplitElement(position);
			}
			return position;
		}
	}
}
