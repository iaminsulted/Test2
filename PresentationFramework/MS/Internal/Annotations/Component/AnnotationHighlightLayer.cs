using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using MS.Internal.Annotations.Anchoring;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020002C9 RID: 713
	internal class AnnotationHighlightLayer : HighlightLayer
	{
		// Token: 0x06001A98 RID: 6808 RVA: 0x00164A27 File Offset: 0x00163A27
		internal AnnotationHighlightLayer()
		{
			this._segments = new List<AnnotationHighlightLayer.HighlightSegment>();
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x00164A3C File Offset: 0x00163A3C
		internal void AddRange(IHighlightRange highlightRange)
		{
			Invariant.Assert(highlightRange != null, "the owner is null");
			ITextPointer start = highlightRange.Range.Start;
			ITextPointer end = highlightRange.Range.End;
			if (start.CompareTo(end) == 0)
			{
				return;
			}
			if (this._segments.Count == 0)
			{
				object textContainer = start.TextContainer;
				this.IsFixedContainer = (textContainer is FixedTextContainer || textContainer is DocumentSequenceTextContainer);
			}
			ITextPointer start2;
			ITextPointer end2;
			this.ProcessOverlapingSegments(highlightRange, out start2, out end2);
			if (this.Changed != null && this.IsFixedContainer)
			{
				this.Changed(this, new AnnotationHighlightLayer.AnnotationHighlightChangedEventArgs(start2, end2));
			}
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x00164AD8 File Offset: 0x00163AD8
		internal void RemoveRange(IHighlightRange highlightRange)
		{
			if (highlightRange.Range.Start.CompareTo(highlightRange.Range.End) == 0)
			{
				return;
			}
			int num;
			int num2;
			this.GetSpannedSegments(highlightRange.Range.Start, highlightRange.Range.End, out num, out num2);
			ITextPointer start = this._segments[num].Segment.Start;
			ITextPointer end = this._segments[num2].Segment.End;
			int i = num;
			while (i <= num2)
			{
				AnnotationHighlightLayer.HighlightSegment highlightSegment = this._segments[i];
				if (highlightSegment.RemoveOwner(highlightRange) == 0)
				{
					this._segments.Remove(highlightSegment);
					num2--;
				}
				else
				{
					i++;
				}
			}
			if (this.Changed != null && this.IsFixedContainer)
			{
				this.Changed(this, new AnnotationHighlightLayer.AnnotationHighlightChangedEventArgs(start, end));
			}
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x00164BBC File Offset: 0x00163BBC
		internal void ModifiedRange(IHighlightRange highlightRange)
		{
			Invariant.Assert(highlightRange != null, "null range data");
			if (highlightRange.Range.Start.CompareTo(highlightRange.Range.End) == 0)
			{
				return;
			}
			int num;
			int num2;
			this.GetSpannedSegments(highlightRange.Range.Start, highlightRange.Range.End, out num, out num2);
			for (int i = num; i < num2; i++)
			{
				this._segments[i].UpdateOwners();
			}
			ITextPointer start = this._segments[num].Segment.Start;
			ITextPointer end = this._segments[num2].Segment.End;
			if (this.Changed != null && this.IsFixedContainer)
			{
				this.Changed(this, new AnnotationHighlightLayer.AnnotationHighlightChangedEventArgs(start, end));
			}
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x00164C90 File Offset: 0x00163C90
		internal void ActivateRange(IHighlightRange highlightRange, bool activate)
		{
			Invariant.Assert(highlightRange != null, "null range data");
			if (highlightRange.Range.Start.CompareTo(highlightRange.Range.End) == 0)
			{
				return;
			}
			int num;
			int num2;
			this.GetSpannedSegments(highlightRange.Range.Start, highlightRange.Range.End, out num, out num2);
			ITextPointer start = this._segments[num].Segment.Start;
			ITextPointer end = this._segments[num2].Segment.End;
			for (int i = num; i <= num2; i++)
			{
				if (activate)
				{
					this._segments[i].AddActiveOwner(highlightRange);
				}
				else
				{
					this._segments[i].RemoveActiveOwner(highlightRange);
				}
			}
			if (this.Changed != null && this.IsFixedContainer)
			{
				this.Changed(this, new AnnotationHighlightLayer.AnnotationHighlightChangedEventArgs(start, end));
			}
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x00164D80 File Offset: 0x00163D80
		internal override object GetHighlightValue(StaticTextPointer textPosition, LogicalDirection direction)
		{
			object result = DependencyProperty.UnsetValue;
			for (int i = 0; i < this._segments.Count; i++)
			{
				AnnotationHighlightLayer.HighlightSegment highlightSegment = this._segments[i];
				if (highlightSegment.Segment.Start.CompareTo(textPosition) > 0 || (highlightSegment.Segment.Start.CompareTo(textPosition) == 0 && direction == LogicalDirection.Backward))
				{
					break;
				}
				if (highlightSegment.Segment.End.CompareTo(textPosition) > 0 || (highlightSegment.Segment.End.CompareTo(textPosition) == 0 && direction == LogicalDirection.Backward))
				{
					result = highlightSegment;
					break;
				}
			}
			return result;
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x00164E20 File Offset: 0x00163E20
		internal override bool IsContentHighlighted(StaticTextPointer staticTextPosition, LogicalDirection direction)
		{
			return this.GetHighlightValue(staticTextPosition, direction) != DependencyProperty.UnsetValue;
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x00164E34 File Offset: 0x00163E34
		internal override StaticTextPointer GetNextChangePosition(StaticTextPointer textPosition, LogicalDirection direction)
		{
			ITextPointer textPointer;
			if (direction == LogicalDirection.Forward)
			{
				textPointer = this.GetNextForwardPosition(textPosition);
			}
			else
			{
				textPointer = this.GetNextBackwardPosition(textPosition);
			}
			if (textPointer != null)
			{
				return textPointer.CreateStaticPointer();
			}
			return StaticTextPointer.Null;
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06001AA0 RID: 6816 RVA: 0x00164E66 File Offset: 0x00163E66
		internal override Type OwnerType
		{
			get
			{
				return typeof(HighlightComponent);
			}
		}

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x06001AA1 RID: 6817 RVA: 0x00164E74 File Offset: 0x00163E74
		// (remove) Token: 0x06001AA2 RID: 6818 RVA: 0x00164EAC File Offset: 0x00163EAC
		internal override event HighlightChangedEventHandler Changed;

		// Token: 0x06001AA3 RID: 6819 RVA: 0x00164EE4 File Offset: 0x00163EE4
		private void ProcessOverlapingSegments(IHighlightRange highlightRange, out ITextPointer invalidateStart, out ITextPointer invalidateEnd)
		{
			ReadOnlyCollection<TextSegment> textSegments = highlightRange.Range.TextSegments;
			invalidateStart = null;
			invalidateEnd = null;
			int num = 0;
			IEnumerator<TextSegment> enumerator = textSegments.GetEnumerator();
			TextSegment textSegment = enumerator.MoveNext() ? enumerator.Current : TextSegment.Null;
			while (num < this._segments.Count && !textSegment.IsNull)
			{
				AnnotationHighlightLayer.HighlightSegment highlightSegment = this._segments[num];
				if (highlightSegment.Segment.Start.CompareTo(textSegment.Start) <= 0)
				{
					if (highlightSegment.Segment.End.CompareTo(textSegment.Start) > 0)
					{
						IList<AnnotationHighlightLayer.HighlightSegment> list = highlightSegment.Split(textSegment.Start, textSegment.End, highlightRange);
						if (!list.Contains(highlightSegment))
						{
							highlightSegment.ClearOwners();
						}
						this._segments.Remove(highlightSegment);
						this._segments.InsertRange(num, list);
						num = num + list.Count - 1;
						if (textSegment.End.CompareTo(highlightSegment.Segment.End) <= 0)
						{
							textSegment = (enumerator.MoveNext() ? enumerator.Current : TextSegment.Null);
						}
						else
						{
							textSegment = new TextSegment(highlightSegment.Segment.End, textSegment.End);
						}
						if (invalidateStart == null)
						{
							invalidateStart = highlightSegment.Segment.Start;
						}
					}
					else
					{
						num++;
					}
				}
				else
				{
					if (invalidateStart == null)
					{
						invalidateStart = textSegment.Start;
					}
					if (textSegment.End.CompareTo(highlightSegment.Segment.Start) > 0)
					{
						AnnotationHighlightLayer.HighlightSegment item = new AnnotationHighlightLayer.HighlightSegment(textSegment.Start, highlightSegment.Segment.Start, highlightRange);
						this._segments.Insert(num++, item);
						textSegment = new TextSegment(highlightSegment.Segment.Start, textSegment.End);
					}
					else
					{
						this._segments.Insert(num++, new AnnotationHighlightLayer.HighlightSegment(textSegment.Start, textSegment.End, highlightRange));
						textSegment = (enumerator.MoveNext() ? enumerator.Current : TextSegment.Null);
					}
				}
			}
			if (!textSegment.IsNull)
			{
				if (invalidateStart == null)
				{
					invalidateStart = textSegment.Start;
				}
				this._segments.Insert(num++, new AnnotationHighlightLayer.HighlightSegment(textSegment.Start, textSegment.End, highlightRange));
			}
			while (enumerator.MoveNext())
			{
				List<AnnotationHighlightLayer.HighlightSegment> segments = this._segments;
				int index = num++;
				TextSegment textSegment2 = enumerator.Current;
				ITextPointer start = textSegment2.Start;
				textSegment2 = enumerator.Current;
				segments.Insert(index, new AnnotationHighlightLayer.HighlightSegment(start, textSegment2.End, highlightRange));
			}
			if (invalidateStart != null)
			{
				if (num == this._segments.Count)
				{
					num--;
				}
				invalidateEnd = this._segments[num].Segment.End;
			}
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x001651B4 File Offset: 0x001641B4
		private ITextPointer GetNextForwardPosition(StaticTextPointer pos)
		{
			for (int i = 0; i < this._segments.Count; i++)
			{
				AnnotationHighlightLayer.HighlightSegment highlightSegment = this._segments[i];
				if (pos.CompareTo(highlightSegment.Segment.Start) < 0)
				{
					return highlightSegment.Segment.Start;
				}
				if (pos.CompareTo(highlightSegment.Segment.End) < 0)
				{
					return highlightSegment.Segment.End;
				}
			}
			return null;
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x00165234 File Offset: 0x00164234
		private ITextPointer GetNextBackwardPosition(StaticTextPointer pos)
		{
			for (int i = this._segments.Count - 1; i >= 0; i--)
			{
				AnnotationHighlightLayer.HighlightSegment highlightSegment = this._segments[i];
				if (pos.CompareTo(highlightSegment.Segment.End) > 0)
				{
					return highlightSegment.Segment.End;
				}
				if (pos.CompareTo(highlightSegment.Segment.Start) > 0)
				{
					return highlightSegment.Segment.Start;
				}
			}
			return null;
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x001652B8 File Offset: 0x001642B8
		private void GetSpannedSegments(ITextPointer start, ITextPointer end, out int startSeg, out int endSeg)
		{
			startSeg = -1;
			endSeg = -1;
			for (int i = 0; i < this._segments.Count; i++)
			{
				AnnotationHighlightLayer.HighlightSegment highlightSegment = this._segments[i];
				if (highlightSegment.Segment.Start.CompareTo(start) == 0)
				{
					startSeg = i;
				}
				if (highlightSegment.Segment.End.CompareTo(end) == 0)
				{
					endSeg = i;
					break;
				}
			}
			if (startSeg >= 0 && endSeg >= 0)
			{
				int num = startSeg;
				int num2 = endSeg;
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06001AA7 RID: 6823 RVA: 0x00165334 File Offset: 0x00164334
		// (set) Token: 0x06001AA8 RID: 6824 RVA: 0x0016533C File Offset: 0x0016433C
		private bool IsFixedContainer
		{
			get
			{
				return this._isFixedContainer;
			}
			set
			{
				this._isFixedContainer = value;
			}
		}

		// Token: 0x04000DC0 RID: 3520
		private List<AnnotationHighlightLayer.HighlightSegment> _segments;

		// Token: 0x04000DC1 RID: 3521
		private bool _isFixedContainer;

		// Token: 0x02000A17 RID: 2583
		private class AnnotationHighlightChangedEventArgs : HighlightChangedEventArgs
		{
			// Token: 0x060084D1 RID: 34001 RVA: 0x00326F64 File Offset: 0x00325F64
			internal AnnotationHighlightChangedEventArgs(ITextPointer start, ITextPointer end)
			{
				TextSegment[] list = new TextSegment[]
				{
					new TextSegment(start, end)
				};
				this._ranges = new ReadOnlyCollection<TextSegment>(list);
			}

			// Token: 0x17001DDB RID: 7643
			// (get) Token: 0x060084D2 RID: 34002 RVA: 0x00326F98 File Offset: 0x00325F98
			internal override IList Ranges
			{
				get
				{
					return this._ranges;
				}
			}

			// Token: 0x17001DDC RID: 7644
			// (get) Token: 0x060084D3 RID: 34003 RVA: 0x00164E66 File Offset: 0x00163E66
			internal override Type OwnerType
			{
				get
				{
					return typeof(HighlightComponent);
				}
			}

			// Token: 0x04004097 RID: 16535
			private readonly ReadOnlyCollection<TextSegment> _ranges;
		}

		// Token: 0x02000A18 RID: 2584
		internal sealed class HighlightSegment : Shape
		{
			// Token: 0x060084D4 RID: 34004 RVA: 0x00326FA0 File Offset: 0x00325FA0
			internal HighlightSegment(ITextPointer start, ITextPointer end, IHighlightRange owner)
			{
				List<IHighlightRange> list = new List<IHighlightRange>(1);
				list.Add(owner);
				this.Init(start, end, list);
				this._owners = list;
				this.UpdateOwners();
			}

			// Token: 0x060084D5 RID: 34005 RVA: 0x00326FF0 File Offset: 0x00325FF0
			internal HighlightSegment(ITextPointer start, ITextPointer end, IList<IHighlightRange> owners)
			{
				this.Init(start, end, owners);
				this._owners = new List<IHighlightRange>(owners.Count);
				this._owners.AddRange(owners);
				this.UpdateOwners();
			}

			// Token: 0x060084D6 RID: 34006 RVA: 0x00327048 File Offset: 0x00326048
			private void Init(ITextPointer start, ITextPointer end, IList<IHighlightRange> owners)
			{
				for (int i = 0; i < owners.Count; i++)
				{
				}
				this._segment = new TextSegment(start, end);
				base.IsHitTestVisible = false;
				object textContainer = start.TextContainer;
				this._isFixedContainer = (textContainer is FixedTextContainer || textContainer is DocumentSequenceTextContainer);
				this.GetContent();
			}

			// Token: 0x060084D7 RID: 34007 RVA: 0x003270A4 File Offset: 0x003260A4
			internal void AddOwner(IHighlightRange owner)
			{
				for (int i = 0; i < this._owners.Count; i++)
				{
					if (this._owners[i].Priority < owner.Priority)
					{
						this._owners.Insert(i, owner);
						this.UpdateOwners();
						return;
					}
				}
				this._owners.Add(owner);
				this.UpdateOwners();
			}

			// Token: 0x060084D8 RID: 34008 RVA: 0x00327108 File Offset: 0x00326108
			internal int RemoveOwner(IHighlightRange owner)
			{
				if (this._owners.Contains(owner))
				{
					if (this._activeOwners.Contains(owner))
					{
						this._activeOwners.Remove(owner);
					}
					this._owners.Remove(owner);
					this.UpdateOwners();
				}
				return this._owners.Count;
			}

			// Token: 0x060084D9 RID: 34009 RVA: 0x0032715C File Offset: 0x0032615C
			internal void AddActiveOwner(IHighlightRange owner)
			{
				if (this._owners.Contains(owner))
				{
					this._activeOwners.Add(owner);
					this.UpdateOwners();
				}
			}

			// Token: 0x060084DA RID: 34010 RVA: 0x0032717E File Offset: 0x0032617E
			private void AddActiveOwners(List<IHighlightRange> owners)
			{
				this._activeOwners.AddRange(owners);
				this.UpdateOwners();
			}

			// Token: 0x060084DB RID: 34011 RVA: 0x00327192 File Offset: 0x00326192
			internal void RemoveActiveOwner(IHighlightRange owner)
			{
				if (this._activeOwners.Contains(owner))
				{
					this._activeOwners.Remove(owner);
					this.UpdateOwners();
				}
			}

			// Token: 0x060084DC RID: 34012 RVA: 0x003271B5 File Offset: 0x003261B5
			internal void ClearOwners()
			{
				this._owners.Clear();
				this._activeOwners.Clear();
				this.UpdateOwners();
			}

			// Token: 0x060084DD RID: 34013 RVA: 0x003271D4 File Offset: 0x003261D4
			internal IList<AnnotationHighlightLayer.HighlightSegment> Split(ITextPointer ps, LogicalDirection side)
			{
				IList<AnnotationHighlightLayer.HighlightSegment> list = null;
				if (ps.CompareTo(this._segment.Start) == 0 || ps.CompareTo(this._segment.End) == 0)
				{
					if ((ps.CompareTo(this._segment.Start) == 0 && side == LogicalDirection.Forward) || (ps.CompareTo(this._segment.End) == 0 && side == LogicalDirection.Backward))
					{
						list = new List<AnnotationHighlightLayer.HighlightSegment>(1);
						list.Add(this);
					}
				}
				else if (this._segment.Contains(ps))
				{
					list = new List<AnnotationHighlightLayer.HighlightSegment>(2);
					list.Add(new AnnotationHighlightLayer.HighlightSegment(this._segment.Start, ps, this._owners));
					list.Add(new AnnotationHighlightLayer.HighlightSegment(ps, this._segment.End, this._owners));
					list[0].AddActiveOwners(this._activeOwners);
					list[1].AddActiveOwners(this._activeOwners);
				}
				return list;
			}

			// Token: 0x060084DE RID: 34014 RVA: 0x003272C0 File Offset: 0x003262C0
			internal IList<AnnotationHighlightLayer.HighlightSegment> Split(ITextPointer ps1, ITextPointer ps2, IHighlightRange newOwner)
			{
				IList<AnnotationHighlightLayer.HighlightSegment> list = new List<AnnotationHighlightLayer.HighlightSegment>();
				if (ps1.CompareTo(ps2) == 0)
				{
					if (this._segment.Start.CompareTo(ps1) > 0 || this._segment.End.CompareTo(ps1) < 0)
					{
						return list;
					}
					if (this._segment.Start.CompareTo(ps1) < 0)
					{
						list.Add(new AnnotationHighlightLayer.HighlightSegment(this._segment.Start, ps1, this._owners));
					}
					list.Add(new AnnotationHighlightLayer.HighlightSegment(ps1, ps1, this._owners));
					if (this._segment.End.CompareTo(ps1) > 0)
					{
						list.Add(new AnnotationHighlightLayer.HighlightSegment(ps1, this._segment.End, this._owners));
					}
					using (IEnumerator<AnnotationHighlightLayer.HighlightSegment> enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							AnnotationHighlightLayer.HighlightSegment highlightSegment = enumerator.Current;
							highlightSegment.AddActiveOwners(this._activeOwners);
						}
						goto IL_193;
					}
				}
				if (this._segment.Contains(ps1))
				{
					IList<AnnotationHighlightLayer.HighlightSegment> list2 = this.Split(ps1, LogicalDirection.Forward);
					for (int i = 0; i < list2.Count; i++)
					{
						if (list2[i].Segment.Contains(ps2))
						{
							IList<AnnotationHighlightLayer.HighlightSegment> list3 = list2[i].Split(ps2, LogicalDirection.Backward);
							for (int j = 0; j < list3.Count; j++)
							{
								list.Add(list3[j]);
							}
							if (!list3.Contains(list2[i]))
							{
								list2[i].Discard();
							}
						}
						else
						{
							list.Add(list2[i]);
						}
					}
				}
				else
				{
					list = this.Split(ps2, LogicalDirection.Backward);
				}
				IL_193:
				if (list != null && list.Count > 0 && newOwner != null)
				{
					if (list.Count == 3)
					{
						list[1].AddOwner(newOwner);
					}
					else if (list[0].Segment.Start.CompareTo(ps1) == 0 || list[0].Segment.End.CompareTo(ps2) == 0)
					{
						list[0].AddOwner(newOwner);
					}
					else
					{
						list[1].AddOwner(newOwner);
					}
				}
				return list;
			}

			// Token: 0x060084DF RID: 34015 RVA: 0x003274F0 File Offset: 0x003264F0
			internal void UpdateOwners()
			{
				if (this._cachedTopOwner != this.TopOwner)
				{
					if (this._cachedTopOwner != null)
					{
						this._cachedTopOwner.RemoveChild(this);
					}
					this._cachedTopOwner = this.TopOwner;
					if (this._cachedTopOwner != null)
					{
						this._cachedTopOwner.AddChild(this);
					}
				}
				base.Fill = this.OwnerColor;
			}

			// Token: 0x060084E0 RID: 34016 RVA: 0x0032754B File Offset: 0x0032654B
			internal void Discard()
			{
				if (this.TopOwner != null)
				{
					this.TopOwner.RemoveChild(this);
				}
				this._activeOwners.Clear();
				this._owners.Clear();
			}

			// Token: 0x060084E1 RID: 34017 RVA: 0x00327578 File Offset: 0x00326578
			private void GetSegmentGeometry(GeometryGroup geometry, TextSegment segment, ITextView parentView)
			{
				foreach (ITextView view in TextSelectionHelper.GetDocumentPageTextViews(segment))
				{
					Geometry pageGeometry = this.GetPageGeometry(segment, view, parentView);
					if (pageGeometry != null)
					{
						geometry.Children.Add(pageGeometry);
					}
				}
			}

			// Token: 0x060084E2 RID: 34018 RVA: 0x003275E0 File Offset: 0x003265E0
			private Geometry GetPageGeometry(TextSegment segment, ITextView view, ITextView parentView)
			{
				if (!view.IsValid || !parentView.IsValid)
				{
					return null;
				}
				if (view.RenderScope == null || parentView.RenderScope == null)
				{
					return null;
				}
				Geometry tightBoundingGeometryFromTextPositions = view.GetTightBoundingGeometryFromTextPositions(segment.Start, segment.End);
				if (tightBoundingGeometryFromTextPositions != null && parentView != null)
				{
					Transform transform = (Transform)view.RenderScope.TransformToVisual(parentView.RenderScope);
					if (tightBoundingGeometryFromTextPositions.Transform != null)
					{
						tightBoundingGeometryFromTextPositions.Transform = new TransformGroup
						{
							Children = 
							{
								tightBoundingGeometryFromTextPositions.Transform,
								transform
							}
						};
					}
					else
					{
						tightBoundingGeometryFromTextPositions.Transform = transform;
					}
				}
				return tightBoundingGeometryFromTextPositions;
			}

			// Token: 0x060084E3 RID: 34019 RVA: 0x00327684 File Offset: 0x00326684
			private void GetContent()
			{
				this._contentSegments.Clear();
				ITextPointer textPointer = this._segment.Start.CreatePointer();
				ITextPointer textPointer2 = null;
				while (textPointer.CompareTo(this._segment.End) < 0)
				{
					TextPointerContext pointerContext = textPointer.GetPointerContext(LogicalDirection.Forward);
					if (pointerContext == TextPointerContext.ElementStart)
					{
						Type elementType = textPointer.GetElementType(LogicalDirection.Forward);
						if (typeof(Run).IsAssignableFrom(elementType) || typeof(BlockUIContainer).IsAssignableFrom(elementType))
						{
							this.OpenSegment(ref textPointer2, textPointer);
						}
						else if (typeof(Table).IsAssignableFrom(elementType) || typeof(Floater).IsAssignableFrom(elementType) || typeof(Figure).IsAssignableFrom(elementType))
						{
							this.CloseSegment(ref textPointer2, textPointer, this._segment.End);
						}
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
						if (typeof(Run).IsAssignableFrom(elementType) || typeof(BlockUIContainer).IsAssignableFrom(elementType))
						{
							textPointer.MoveToElementEdge(ElementEdge.AfterEnd);
						}
					}
					else if (pointerContext == TextPointerContext.ElementEnd)
					{
						Type parentType = textPointer.ParentType;
						if (typeof(TableCell).IsAssignableFrom(parentType) || typeof(Floater).IsAssignableFrom(parentType) || typeof(Figure).IsAssignableFrom(parentType))
						{
							this.CloseSegment(ref textPointer2, textPointer, this._segment.End);
						}
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					}
					else if (pointerContext == TextPointerContext.Text || pointerContext == TextPointerContext.EmbeddedElement)
					{
						this.OpenSegment(ref textPointer2, textPointer);
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					}
					else
					{
						Invariant.Assert(false, "unexpected TextPointerContext");
					}
				}
				this.CloseSegment(ref textPointer2, textPointer, this._segment.End);
			}

			// Token: 0x060084E4 RID: 34020 RVA: 0x00327833 File Offset: 0x00326833
			private void OpenSegment(ref ITextPointer segmentStart, ITextPointer cursor)
			{
				if (segmentStart == null)
				{
					segmentStart = cursor.GetInsertionPosition(LogicalDirection.Forward);
				}
			}

			// Token: 0x060084E5 RID: 34021 RVA: 0x00327844 File Offset: 0x00326844
			private void CloseSegment(ref ITextPointer segmentStart, ITextPointer cursor, ITextPointer end)
			{
				if (segmentStart != null)
				{
					if (cursor.CompareTo(end) > 0)
					{
						cursor = end;
					}
					ITextPointer insertionPosition = cursor.GetInsertionPosition(LogicalDirection.Backward);
					if (segmentStart.CompareTo(insertionPosition) < 0)
					{
						this._contentSegments.Add(new TextSegment(segmentStart, insertionPosition));
					}
					segmentStart = null;
				}
			}

			// Token: 0x17001DDD RID: 7645
			// (get) Token: 0x060084E6 RID: 34022 RVA: 0x0032788C File Offset: 0x0032688C
			protected override Geometry DefiningGeometry
			{
				get
				{
					if (this._isFixedContainer)
					{
						return Geometry.Empty;
					}
					ITextView documentPageTextView = TextSelectionHelper.GetDocumentPageTextView(this.TopOwner.Range.Start.CreatePointer(LogicalDirection.Forward));
					GeometryGroup geometryGroup = new GeometryGroup();
					if (this.TopOwner.HighlightContent)
					{
						using (List<TextSegment>.Enumerator enumerator = this._contentSegments.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								TextSegment segment = enumerator.Current;
								this.GetSegmentGeometry(geometryGroup, segment, documentPageTextView);
							}
							goto IL_85;
						}
					}
					this.GetSegmentGeometry(geometryGroup, this._segment, documentPageTextView);
					IL_85:
					UIElement uielement = this.TopOwner as UIElement;
					if (uielement != null)
					{
						uielement.RenderTransform = Transform.Identity;
					}
					return geometryGroup;
				}
			}

			// Token: 0x17001DDE RID: 7646
			// (get) Token: 0x060084E7 RID: 34023 RVA: 0x0032794C File Offset: 0x0032694C
			internal TextSegment Segment
			{
				get
				{
					return this._segment;
				}
			}

			// Token: 0x17001DDF RID: 7647
			// (get) Token: 0x060084E8 RID: 34024 RVA: 0x00327954 File Offset: 0x00326954
			internal IHighlightRange TopOwner
			{
				get
				{
					if (this._activeOwners.Count != 0)
					{
						return this._activeOwners[0];
					}
					if (this._owners.Count <= 0)
					{
						return null;
					}
					return this._owners[0];
				}
			}

			// Token: 0x17001DE0 RID: 7648
			// (get) Token: 0x060084E9 RID: 34025 RVA: 0x0032798C File Offset: 0x0032698C
			private Brush OwnerColor
			{
				get
				{
					if (this._activeOwners.Count != 0)
					{
						return new SolidColorBrush(this._activeOwners[0].SelectedBackground);
					}
					if (this._owners.Count <= 0)
					{
						return null;
					}
					return new SolidColorBrush(this._owners[0].Background);
				}
			}

			// Token: 0x04004098 RID: 16536
			private TextSegment _segment;

			// Token: 0x04004099 RID: 16537
			private List<TextSegment> _contentSegments = new List<TextSegment>(1);

			// Token: 0x0400409A RID: 16538
			private readonly List<IHighlightRange> _owners;

			// Token: 0x0400409B RID: 16539
			private List<IHighlightRange> _activeOwners = new List<IHighlightRange>();

			// Token: 0x0400409C RID: 16540
			private IHighlightRange _cachedTopOwner;

			// Token: 0x0400409D RID: 16541
			private bool _isFixedContainer;
		}
	}
}
