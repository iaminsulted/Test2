using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000624 RID: 1572
	internal class Highlights
	{
		// Token: 0x06004D72 RID: 19826 RVA: 0x0023FCF7 File Offset: 0x0023ECF7
		internal Highlights(ITextContainer textContainer)
		{
			this._textContainer = textContainer;
		}

		// Token: 0x06004D73 RID: 19827 RVA: 0x0023FD08 File Offset: 0x0023ED08
		internal virtual object GetHighlightValue(StaticTextPointer textPosition, LogicalDirection direction, Type highlightLayerOwnerType)
		{
			object obj = DependencyProperty.UnsetValue;
			for (int i = 0; i < this.LayerCount; i++)
			{
				HighlightLayer layer = this.GetLayer(i);
				if (layer.OwnerType == highlightLayerOwnerType)
				{
					obj = layer.GetHighlightValue(textPosition, direction);
					if (obj != DependencyProperty.UnsetValue)
					{
						break;
					}
				}
			}
			return obj;
		}

		// Token: 0x06004D74 RID: 19828 RVA: 0x0023FD54 File Offset: 0x0023ED54
		internal virtual bool IsContentHighlighted(StaticTextPointer textPosition, LogicalDirection direction)
		{
			int num = 0;
			while (num < this.LayerCount && !this.GetLayer(num).IsContentHighlighted(textPosition, direction))
			{
				num++;
			}
			return num < this.LayerCount;
		}

		// Token: 0x06004D75 RID: 19829 RVA: 0x0023FD8C File Offset: 0x0023ED8C
		internal virtual StaticTextPointer GetNextHighlightChangePosition(StaticTextPointer textPosition, LogicalDirection direction)
		{
			StaticTextPointer staticTextPointer = StaticTextPointer.Null;
			for (int i = 0; i < this.LayerCount; i++)
			{
				StaticTextPointer nextChangePosition = this.GetLayer(i).GetNextChangePosition(textPosition, direction);
				if (!nextChangePosition.IsNull)
				{
					if (staticTextPointer.IsNull)
					{
						staticTextPointer = nextChangePosition;
					}
					else if (direction == LogicalDirection.Forward)
					{
						staticTextPointer = StaticTextPointer.Min(staticTextPointer, nextChangePosition);
					}
					else
					{
						staticTextPointer = StaticTextPointer.Max(staticTextPointer, nextChangePosition);
					}
				}
			}
			return staticTextPointer;
		}

		// Token: 0x06004D76 RID: 19830 RVA: 0x0023FDEC File Offset: 0x0023EDEC
		internal virtual StaticTextPointer GetNextPropertyChangePosition(StaticTextPointer textPosition, LogicalDirection direction)
		{
			StaticTextPointer staticTextPointer;
			switch (textPosition.GetPointerContext(direction))
			{
			case TextPointerContext.None:
				return StaticTextPointer.Null;
			case TextPointerContext.Text:
			{
				staticTextPointer = this.GetNextHighlightChangePosition(textPosition, direction);
				StaticTextPointer nextContextPosition = textPosition.GetNextContextPosition(LogicalDirection.Forward);
				if (staticTextPointer.IsNull || nextContextPosition.CompareTo(staticTextPointer) < 0)
				{
					return nextContextPosition;
				}
				return staticTextPointer;
			}
			}
			staticTextPointer = textPosition.CreatePointer(1);
			return staticTextPointer;
		}

		// Token: 0x06004D77 RID: 19831 RVA: 0x0023FE5C File Offset: 0x0023EE5C
		internal void AddLayer(HighlightLayer highlightLayer)
		{
			if (this._layers == null)
			{
				this._layers = new ArrayList(1);
			}
			Invariant.Assert(!this._layers.Contains(highlightLayer));
			this._layers.Add(highlightLayer);
			highlightLayer.Changed += this.OnLayerChanged;
			this.RaiseChangedEventForLayerContent(highlightLayer);
		}

		// Token: 0x06004D78 RID: 19832 RVA: 0x0023FEB8 File Offset: 0x0023EEB8
		internal void RemoveLayer(HighlightLayer highlightLayer)
		{
			Invariant.Assert(this._layers != null && this._layers.Contains(highlightLayer));
			this.RaiseChangedEventForLayerContent(highlightLayer);
			highlightLayer.Changed -= this.OnLayerChanged;
			this._layers.Remove(highlightLayer);
		}

		// Token: 0x06004D79 RID: 19833 RVA: 0x0023FF08 File Offset: 0x0023EF08
		internal HighlightLayer GetLayer(Type highlightLayerType)
		{
			for (int i = 0; i < this.LayerCount; i++)
			{
				if (highlightLayerType == this.GetLayer(i).OwnerType)
				{
					return this.GetLayer(i);
				}
			}
			return null;
		}

		// Token: 0x170011FA RID: 4602
		// (get) Token: 0x06004D7A RID: 19834 RVA: 0x0023FF43 File Offset: 0x0023EF43
		protected ITextContainer TextContainer
		{
			get
			{
				return this._textContainer;
			}
		}

		// Token: 0x140000B7 RID: 183
		// (add) Token: 0x06004D7B RID: 19835 RVA: 0x0023FF4C File Offset: 0x0023EF4C
		// (remove) Token: 0x06004D7C RID: 19836 RVA: 0x0023FF84 File Offset: 0x0023EF84
		internal event HighlightChangedEventHandler Changed;

		// Token: 0x06004D7D RID: 19837 RVA: 0x0023FFB9 File Offset: 0x0023EFB9
		private HighlightLayer GetLayer(int index)
		{
			return (HighlightLayer)this._layers[index];
		}

		// Token: 0x06004D7E RID: 19838 RVA: 0x0023FFCC File Offset: 0x0023EFCC
		private void OnLayerChanged(object sender, HighlightChangedEventArgs args)
		{
			if (this.Changed != null)
			{
				this.Changed(this, args);
			}
		}

		// Token: 0x06004D7F RID: 19839 RVA: 0x0023FFE4 File Offset: 0x0023EFE4
		private void RaiseChangedEventForLayerContent(HighlightLayer highlightLayer)
		{
			if (this.Changed != null)
			{
				List<TextSegment> list = new List<TextSegment>();
				StaticTextPointer staticTextPointer = this._textContainer.CreateStaticPointerAtOffset(0);
				for (;;)
				{
					if (!highlightLayer.IsContentHighlighted(staticTextPointer, LogicalDirection.Forward))
					{
						staticTextPointer = highlightLayer.GetNextChangePosition(staticTextPointer, LogicalDirection.Forward);
						if (staticTextPointer.IsNull)
						{
							break;
						}
					}
					StaticTextPointer staticTextPointer2 = staticTextPointer;
					staticTextPointer = highlightLayer.GetNextChangePosition(staticTextPointer, LogicalDirection.Forward);
					Invariant.Assert(!staticTextPointer.IsNull, "Highlight start not followed by highlight end!");
					list.Add(new TextSegment(staticTextPointer2.CreateDynamicTextPointer(LogicalDirection.Forward), staticTextPointer.CreateDynamicTextPointer(LogicalDirection.Forward)));
				}
				if (list.Count > 0)
				{
					this.Changed(this, new Highlights.LayerHighlightChangedEventArgs(new ReadOnlyCollection<TextSegment>(list), highlightLayer.OwnerType));
				}
			}
		}

		// Token: 0x170011FB RID: 4603
		// (get) Token: 0x06004D80 RID: 19840 RVA: 0x0024008D File Offset: 0x0023F08D
		private int LayerCount
		{
			get
			{
				if (this._layers != null)
				{
					return this._layers.Count;
				}
				return 0;
			}
		}

		// Token: 0x0400280D RID: 10253
		private readonly ITextContainer _textContainer;

		// Token: 0x0400280E RID: 10254
		private ArrayList _layers;

		// Token: 0x02000B3C RID: 2876
		private class LayerHighlightChangedEventArgs : HighlightChangedEventArgs
		{
			// Token: 0x06008CBE RID: 36030 RVA: 0x0033E141 File Offset: 0x0033D141
			internal LayerHighlightChangedEventArgs(ReadOnlyCollection<TextSegment> ranges, Type ownerType)
			{
				this._ranges = ranges;
				this._ownerType = ownerType;
			}

			// Token: 0x17001ED0 RID: 7888
			// (get) Token: 0x06008CBF RID: 36031 RVA: 0x0033E157 File Offset: 0x0033D157
			internal override IList Ranges
			{
				get
				{
					return this._ranges;
				}
			}

			// Token: 0x17001ED1 RID: 7889
			// (get) Token: 0x06008CC0 RID: 36032 RVA: 0x0033E15F File Offset: 0x0033D15F
			internal override Type OwnerType
			{
				get
				{
					return this._ownerType;
				}
			}

			// Token: 0x04004851 RID: 18513
			private readonly ReadOnlyCollection<TextSegment> _ranges;

			// Token: 0x04004852 RID: 18514
			private readonly Type _ownerType;
		}
	}
}
