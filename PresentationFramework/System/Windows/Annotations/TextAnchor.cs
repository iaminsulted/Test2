using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Documents;
using MS.Internal;

namespace System.Windows.Annotations
{
	// Token: 0x02000863 RID: 2147
	public sealed class TextAnchor
	{
		// Token: 0x06007EBB RID: 32443 RVA: 0x0031A07E File Offset: 0x0031907E
		internal TextAnchor()
		{
		}

		// Token: 0x06007EBC RID: 32444 RVA: 0x0031A094 File Offset: 0x00319094
		internal TextAnchor(TextAnchor anchor)
		{
			Invariant.Assert(anchor != null, "Anchor to clone is null.");
			foreach (TextSegment textSegment in anchor.TextSegments)
			{
				this._segments.Add(new TextSegment(textSegment.Start, textSegment.End));
			}
		}

		// Token: 0x06007EBD RID: 32445 RVA: 0x0031A118 File Offset: 0x00319118
		internal bool Contains(ITextPointer textPointer)
		{
			if (textPointer == null)
			{
				throw new ArgumentNullException("textPointer");
			}
			if (textPointer.TextContainer != this.Start.TextContainer)
			{
				throw new ArgumentException(SR.Get("NotInAssociatedTree", new object[]
				{
					"textPointer"
				}));
			}
			if (textPointer.CompareTo(this.Start) < 0)
			{
				textPointer = textPointer.GetInsertionPosition(LogicalDirection.Forward);
			}
			else if (textPointer.CompareTo(this.End) > 0)
			{
				textPointer = textPointer.GetInsertionPosition(LogicalDirection.Backward);
			}
			for (int i = 0; i < this._segments.Count; i++)
			{
				if (this._segments[i].Contains(textPointer))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007EBE RID: 32446 RVA: 0x0031A1C8 File Offset: 0x003191C8
		internal void AddTextSegment(ITextPointer start, ITextPointer end)
		{
			Invariant.Assert(start != null, "Non-null start required to create segment.");
			Invariant.Assert(end != null, "Non-null end required to create segment.");
			TextSegment newSegment = TextAnchor.CreateNormalizedSegment(start, end);
			this.InsertSegment(newSegment);
		}

		// Token: 0x06007EBF RID: 32447 RVA: 0x0031A200 File Offset: 0x00319200
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06007EC0 RID: 32448 RVA: 0x0031A208 File Offset: 0x00319208
		public override bool Equals(object obj)
		{
			TextAnchor textAnchor = obj as TextAnchor;
			if (textAnchor == null)
			{
				return false;
			}
			if (textAnchor._segments.Count != this._segments.Count)
			{
				return false;
			}
			for (int i = 0; i < this._segments.Count; i++)
			{
				if (this._segments[i].Start.CompareTo(textAnchor._segments[i].Start) != 0 || this._segments[i].End.CompareTo(textAnchor._segments[i].End) != 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007EC1 RID: 32449 RVA: 0x0031A2B4 File Offset: 0x003192B4
		internal bool IsOverlapping(ICollection<TextSegment> textSegments)
		{
			Invariant.Assert(textSegments != null, "TextSegments must not be null.");
			textSegments = TextAnchor.SortTextSegments(textSegments, false);
			IEnumerator<TextSegment> enumerator = this._segments.GetEnumerator();
			IEnumerator<TextSegment> enumerator2 = textSegments.GetEnumerator();
			bool flag = enumerator.MoveNext();
			bool flag2 = enumerator2.MoveNext();
			while (flag && flag2)
			{
				TextSegment textSegment = enumerator.Current;
				TextSegment textSegment2 = enumerator2.Current;
				if (textSegment2.Start.CompareTo(textSegment2.End) == 0)
				{
					if (textSegment.Start.CompareTo(textSegment2.Start) == 0 && textSegment2.Start.LogicalDirection == LogicalDirection.Forward)
					{
						return true;
					}
					if (textSegment.End.CompareTo(textSegment2.End) == 0 && textSegment2.End.LogicalDirection == LogicalDirection.Backward)
					{
						return true;
					}
				}
				if (textSegment.Start.CompareTo(textSegment2.End) >= 0)
				{
					flag2 = enumerator2.MoveNext();
				}
				else
				{
					if (textSegment.End.CompareTo(textSegment2.Start) > 0)
					{
						return true;
					}
					flag = enumerator.MoveNext();
				}
			}
			return false;
		}

		// Token: 0x06007EC2 RID: 32450 RVA: 0x0031A3C0 File Offset: 0x003193C0
		internal static TextAnchor ExclusiveUnion(TextAnchor anchor, TextAnchor otherAnchor)
		{
			Invariant.Assert(anchor != null, "anchor must not be null.");
			Invariant.Assert(otherAnchor != null, "otherAnchor must not be null.");
			foreach (TextSegment newSegment in otherAnchor.TextSegments)
			{
				anchor.InsertSegment(newSegment);
			}
			return anchor;
		}

		// Token: 0x06007EC3 RID: 32451 RVA: 0x0031A42C File Offset: 0x0031942C
		internal static TextAnchor TrimToRelativeComplement(TextAnchor anchor, ICollection<TextSegment> textSegments)
		{
			Invariant.Assert(anchor != null, "Anchor must not be null.");
			Invariant.Assert(textSegments != null, "TextSegments must not be null.");
			textSegments = TextAnchor.SortTextSegments(textSegments, true);
			IEnumerator<TextSegment> enumerator = textSegments.GetEnumerator();
			bool flag = enumerator.MoveNext();
			int num = 0;
			TextSegment textSegment = TextSegment.Null;
			while (num < anchor._segments.Count && flag)
			{
				bool condition;
				if (!textSegment.Equals(TextSegment.Null) && !textSegment.Equals(enumerator.Current))
				{
					ITextPointer end = textSegment.End;
					TextSegment textSegment2 = enumerator.Current;
					condition = (end.CompareTo(textSegment2.Start) <= 0);
				}
				else
				{
					condition = true;
				}
				Invariant.Assert(condition, "TextSegments are overlapping or not ordered.");
				TextSegment textSegment3 = anchor._segments[num];
				textSegment = enumerator.Current;
				if (textSegment3.Start.CompareTo(textSegment.End) >= 0)
				{
					flag = enumerator.MoveNext();
				}
				else if (textSegment3.Start.CompareTo(textSegment.Start) >= 0)
				{
					if (textSegment3.End.CompareTo(textSegment.End) <= 0)
					{
						anchor._segments.RemoveAt(num);
					}
					else
					{
						anchor._segments[num] = TextAnchor.CreateNormalizedSegment(textSegment.End, textSegment3.End);
						flag = enumerator.MoveNext();
					}
				}
				else
				{
					if (textSegment3.End.CompareTo(textSegment.Start) > 0)
					{
						anchor._segments[num] = TextAnchor.CreateNormalizedSegment(textSegment3.Start, textSegment.Start);
						if (textSegment3.End.CompareTo(textSegment.End) > 0)
						{
							anchor._segments.Insert(num + 1, TextAnchor.CreateNormalizedSegment(textSegment.End, textSegment3.End));
							flag = enumerator.MoveNext();
						}
					}
					num++;
				}
			}
			if (anchor._segments.Count > 0)
			{
				return anchor;
			}
			return null;
		}

		// Token: 0x06007EC4 RID: 32452 RVA: 0x0031A614 File Offset: 0x00319614
		internal static TextAnchor TrimToIntersectionWith(TextAnchor anchor, ICollection<TextSegment> textSegments)
		{
			Invariant.Assert(anchor != null, "Anchor must not be null.");
			Invariant.Assert(textSegments != null, "TextSegments must not be null.");
			textSegments = TextAnchor.SortTextSegments(textSegments, true);
			TextSegment textSegment = TextSegment.Null;
			int num = 0;
			IEnumerator<TextSegment> enumerator = textSegments.GetEnumerator();
			bool flag = enumerator.MoveNext();
			while (num < anchor._segments.Count && flag)
			{
				bool condition;
				if (!textSegment.Equals(TextSegment.Null) && !textSegment.Equals(enumerator.Current))
				{
					ITextPointer end = textSegment.End;
					TextSegment textSegment2 = enumerator.Current;
					condition = (end.CompareTo(textSegment2.Start) <= 0);
				}
				else
				{
					condition = true;
				}
				Invariant.Assert(condition, "TextSegments are overlapping or not ordered.");
				TextSegment textSegment3 = anchor._segments[num];
				textSegment = enumerator.Current;
				if (textSegment3.Start.CompareTo(textSegment.End) >= 0)
				{
					flag = enumerator.MoveNext();
				}
				else if (textSegment3.End.CompareTo(textSegment.Start) <= 0)
				{
					anchor._segments.RemoveAt(num);
				}
				else if (textSegment3.Start.CompareTo(textSegment.Start) < 0)
				{
					anchor._segments[num] = TextAnchor.CreateNormalizedSegment(textSegment.Start, textSegment3.End);
				}
				else
				{
					if (textSegment3.End.CompareTo(textSegment.End) > 0)
					{
						anchor._segments[num] = TextAnchor.CreateNormalizedSegment(textSegment3.Start, textSegment.End);
						anchor._segments.Insert(num + 1, TextAnchor.CreateNormalizedSegment(textSegment.End, textSegment3.End));
						flag = enumerator.MoveNext();
					}
					else if (textSegment3.End.CompareTo(textSegment.End) == 0)
					{
						flag = enumerator.MoveNext();
					}
					num++;
				}
			}
			if (!flag && num < anchor._segments.Count)
			{
				anchor._segments.RemoveRange(num, anchor._segments.Count - num);
			}
			if (anchor._segments.Count == 0)
			{
				return null;
			}
			return anchor;
		}

		// Token: 0x17001D41 RID: 7489
		// (get) Token: 0x06007EC5 RID: 32453 RVA: 0x0031A82D File Offset: 0x0031982D
		public ContentPosition BoundingStart
		{
			get
			{
				return this.Start as ContentPosition;
			}
		}

		// Token: 0x17001D42 RID: 7490
		// (get) Token: 0x06007EC6 RID: 32454 RVA: 0x0031A83A File Offset: 0x0031983A
		public ContentPosition BoundingEnd
		{
			get
			{
				return this.End as ContentPosition;
			}
		}

		// Token: 0x17001D43 RID: 7491
		// (get) Token: 0x06007EC7 RID: 32455 RVA: 0x0031A848 File Offset: 0x00319848
		internal ITextPointer Start
		{
			get
			{
				if (this._segments.Count <= 0)
				{
					return null;
				}
				return this._segments[0].Start;
			}
		}

		// Token: 0x17001D44 RID: 7492
		// (get) Token: 0x06007EC8 RID: 32456 RVA: 0x0031A87C File Offset: 0x0031987C
		internal ITextPointer End
		{
			get
			{
				if (this._segments.Count <= 0)
				{
					return null;
				}
				return this._segments[this._segments.Count - 1].End;
			}
		}

		// Token: 0x17001D45 RID: 7493
		// (get) Token: 0x06007EC9 RID: 32457 RVA: 0x0031A8BC File Offset: 0x003198BC
		internal bool IsEmpty
		{
			get
			{
				return this._segments.Count == 1 && this._segments[0].Start == this._segments[0].End;
			}
		}

		// Token: 0x17001D46 RID: 7494
		// (get) Token: 0x06007ECA RID: 32458 RVA: 0x0031A904 File Offset: 0x00319904
		internal string Text
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this._segments.Count; i++)
				{
					stringBuilder.Append(TextRangeBase.GetTextInternal(this._segments[i].Start, this._segments[i].End));
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17001D47 RID: 7495
		// (get) Token: 0x06007ECB RID: 32459 RVA: 0x0031A967 File Offset: 0x00319967
		internal ReadOnlyCollection<TextSegment> TextSegments
		{
			get
			{
				return this._segments.AsReadOnly();
			}
		}

		// Token: 0x06007ECC RID: 32460 RVA: 0x0031A974 File Offset: 0x00319974
		private static ICollection<TextSegment> SortTextSegments(ICollection<TextSegment> textSegments, bool excludeZeroLength)
		{
			Invariant.Assert(textSegments != null, "TextSegments must not be null.");
			List<TextSegment> list = new List<TextSegment>(textSegments.Count);
			list.AddRange(textSegments);
			if (excludeZeroLength)
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					TextSegment item = list[i];
					if (item.Start.CompareTo(item.End) >= 0)
					{
						list.Remove(item);
					}
				}
			}
			if (list.Count > 1)
			{
				list.Sort(new TextAnchor.TextSegmentComparer());
			}
			return list;
		}

		// Token: 0x06007ECD RID: 32461 RVA: 0x0031A9F4 File Offset: 0x003199F4
		private void InsertSegment(TextSegment newSegment)
		{
			int num = 0;
			while (num < this._segments.Count && newSegment.Start.CompareTo(this._segments[num].Start) >= 0)
			{
				num++;
			}
			if (num > 0 && newSegment.Start.CompareTo(this._segments[num - 1].End) < 0)
			{
				throw new InvalidOperationException(SR.Get("TextSegmentsMustNotOverlap"));
			}
			if (num < this._segments.Count && newSegment.End.CompareTo(this._segments[num].Start) > 0)
			{
				throw new InvalidOperationException(SR.Get("TextSegmentsMustNotOverlap"));
			}
			this._segments.Insert(num, newSegment);
		}

		// Token: 0x06007ECE RID: 32462 RVA: 0x0031AAC4 File Offset: 0x00319AC4
		private static TextSegment CreateNormalizedSegment(ITextPointer start, ITextPointer end)
		{
			if (start.CompareTo(end) == 0)
			{
				if (!TextPointerBase.IsAtInsertionPosition(start, start.LogicalDirection))
				{
					start = start.GetInsertionPosition(start.LogicalDirection);
					end = start;
				}
			}
			else
			{
				if (!TextPointerBase.IsAtInsertionPosition(start, start.LogicalDirection))
				{
					start = start.GetInsertionPosition(LogicalDirection.Forward);
				}
				if (!TextPointerBase.IsAtInsertionPosition(end, start.LogicalDirection))
				{
					end = end.GetInsertionPosition(LogicalDirection.Backward);
				}
				if (start.CompareTo(end) >= 0)
				{
					if (start.LogicalDirection == LogicalDirection.Backward)
					{
						start = end.GetFrozenPointer(LogicalDirection.Backward);
					}
					end = start;
				}
			}
			return new TextSegment(start, end);
		}

		// Token: 0x04003B51 RID: 15185
		private List<TextSegment> _segments = new List<TextSegment>(1);

		// Token: 0x02000C59 RID: 3161
		private class TextSegmentComparer : IComparer<TextSegment>
		{
			// Token: 0x060091D5 RID: 37333 RVA: 0x0034ADE8 File Offset: 0x00349DE8
			public int Compare(TextSegment x, TextSegment y)
			{
				if (x.Equals(TextSegment.Null))
				{
					if (y.Equals(TextSegment.Null))
					{
						return 0;
					}
					return -1;
				}
				else
				{
					if (y.Equals(TextSegment.Null))
					{
						return 1;
					}
					int num = x.Start.CompareTo(y.Start);
					if (num != 0)
					{
						return num;
					}
					return x.End.CompareTo(y.End);
				}
			}
		}
	}
}
