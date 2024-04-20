using System;
using System.Collections;
using System.Windows.Controls;

namespace System.Windows.Documents
{
	// Token: 0x0200061D RID: 1565
	internal sealed class FlowPosition : IComparable
	{
		// Token: 0x06004CFC RID: 19708 RVA: 0x0023E0C9 File Offset: 0x0023D0C9
		internal FlowPosition(FixedTextContainer container, FlowNode node, int offset)
		{
			this._container = container;
			this._flowNode = node;
			this._offset = offset;
		}

		// Token: 0x06004CFD RID: 19709 RVA: 0x0023E0E6 File Offset: 0x0023D0E6
		public object Clone()
		{
			return new FlowPosition(this._container, this._flowNode, this._offset);
		}

		// Token: 0x06004CFE RID: 19710 RVA: 0x0023E100 File Offset: 0x0023D100
		public int CompareTo(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			FlowPosition flowPosition = o as FlowPosition;
			if (flowPosition == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					o.GetType(),
					typeof(FlowPosition)
				}), "o");
			}
			return this._OverlapAwareCompare(flowPosition);
		}

		// Token: 0x06004CFF RID: 19711 RVA: 0x0023E15D File Offset: 0x0023D15D
		public override int GetHashCode()
		{
			return this._flowNode.GetHashCode() ^ this._offset.GetHashCode();
		}

		// Token: 0x06004D00 RID: 19712 RVA: 0x0023E178 File Offset: 0x0023D178
		internal int GetDistance(FlowPosition flow)
		{
			if (this._flowNode.Equals(flow._flowNode))
			{
				return flow._offset - this._offset;
			}
			int num = this._OverlapAwareCompare(flow);
			FlowPosition flowPosition;
			FlowPosition flowPosition2;
			if (num == -1)
			{
				flowPosition = (FlowPosition)this.Clone();
				flowPosition2 = flow;
			}
			else
			{
				flowPosition = (FlowPosition)flow.Clone();
				flowPosition2 = this;
			}
			int num2 = 0;
			while (!flowPosition._IsSamePosition(flowPosition2))
			{
				if (flowPosition._flowNode.Equals(flowPosition2._flowNode))
				{
					num2 += flowPosition2._offset - flowPosition._offset;
					break;
				}
				int num3 = flowPosition._vScan(LogicalDirection.Forward, -1);
				num2 += num3;
			}
			return num * -1 * num2;
		}

		// Token: 0x06004D01 RID: 19713 RVA: 0x0023E216 File Offset: 0x0023D216
		internal TextPointerContext GetPointerContext(LogicalDirection dir)
		{
			return this._vGetSymbolType(dir);
		}

		// Token: 0x06004D02 RID: 19714 RVA: 0x0023E220 File Offset: 0x0023D220
		internal int GetTextRunLength(LogicalDirection dir)
		{
			FlowPosition clingPosition = this.GetClingPosition(dir);
			if (dir == LogicalDirection.Forward)
			{
				return clingPosition._NodeLength - clingPosition._offset;
			}
			return clingPosition._offset;
		}

		// Token: 0x06004D03 RID: 19715 RVA: 0x0023E250 File Offset: 0x0023D250
		internal int GetTextInRun(LogicalDirection dir, int maxLength, char[] chars, int startIndex)
		{
			FlowPosition clingPosition = this.GetClingPosition(dir);
			int nodeLength = clingPosition._NodeLength;
			int val;
			if (dir == LogicalDirection.Forward)
			{
				val = nodeLength - clingPosition._offset;
			}
			else
			{
				val = clingPosition._offset;
			}
			maxLength = Math.Min(maxLength, val);
			string flowText = this._container.FixedTextBuilder.GetFlowText(clingPosition._flowNode);
			if (dir == LogicalDirection.Forward)
			{
				Array.Copy(flowText.ToCharArray(clingPosition._offset, maxLength), 0, chars, startIndex, maxLength);
			}
			else
			{
				Array.Copy(flowText.ToCharArray(clingPosition._offset - maxLength, maxLength), 0, chars, startIndex, maxLength);
			}
			return maxLength;
		}

		// Token: 0x06004D04 RID: 19716 RVA: 0x0023E2DC File Offset: 0x0023D2DC
		internal object GetAdjacentElement(LogicalDirection dir)
		{
			FlowPosition clingPosition = this.GetClingPosition(dir);
			FlowNodeType type = clingPosition._flowNode.Type;
			if (type == FlowNodeType.Noop)
			{
				return string.Empty;
			}
			object @object = ((FixedElement)clingPosition._flowNode.Cookie).GetObject();
			Image image = @object as Image;
			if (type == FlowNodeType.Object && image != null)
			{
				FixedSOMElement[] fixedSOMElements = clingPosition._flowNode.FixedSOMElements;
				if (fixedSOMElements != null && fixedSOMElements.Length != 0)
				{
					FixedSOMImage fixedSOMImage = fixedSOMElements[0] as FixedSOMImage;
					if (fixedSOMImage != null)
					{
						image.Width = fixedSOMImage.BoundingRect.Width;
						image.Height = fixedSOMImage.BoundingRect.Height;
					}
				}
			}
			return @object;
		}

		// Token: 0x06004D05 RID: 19717 RVA: 0x0023E378 File Offset: 0x0023D378
		internal FixedElement GetElement(LogicalDirection dir)
		{
			return (FixedElement)this.GetClingPosition(dir)._flowNode.Cookie;
		}

		// Token: 0x06004D06 RID: 19718 RVA: 0x0023E390 File Offset: 0x0023D390
		internal FixedElement GetScopingElement()
		{
			FlowPosition flowPosition = (FlowPosition)this.Clone();
			int num = 0;
			TextPointerContext pointerContext;
			while (flowPosition.FlowNode.Fp > 0 && !this.IsVirtual(this._FixedFlowMap[flowPosition.FlowNode.Fp - 1]) && (pointerContext = flowPosition.GetPointerContext(LogicalDirection.Backward)) != TextPointerContext.None)
			{
				if (pointerContext == TextPointerContext.ElementStart)
				{
					if (num == 0)
					{
						return (FixedElement)flowPosition.GetClingPosition(LogicalDirection.Backward)._flowNode.Cookie;
					}
					num--;
				}
				else if (pointerContext == TextPointerContext.ElementEnd)
				{
					num++;
				}
				flowPosition.Move(LogicalDirection.Backward);
			}
			return this._container.ContainerElement;
		}

		// Token: 0x06004D07 RID: 19719 RVA: 0x0023E428 File Offset: 0x0023D428
		internal bool Move(int distance)
		{
			LogicalDirection dir = (distance >= 0) ? LogicalDirection.Forward : LogicalDirection.Backward;
			distance = Math.Abs(distance);
			FlowNode flowNode = this._flowNode;
			int offset = this._offset;
			while (distance > 0)
			{
				int num = this._vScan(dir, distance);
				if (num == 0)
				{
					this._flowNode = flowNode;
					this._offset = offset;
					return false;
				}
				distance -= num;
			}
			return true;
		}

		// Token: 0x06004D08 RID: 19720 RVA: 0x0023E47C File Offset: 0x0023D47C
		internal bool Move(LogicalDirection dir)
		{
			return this._vScan(dir, -1) > 0;
		}

		// Token: 0x06004D09 RID: 19721 RVA: 0x0023E48C File Offset: 0x0023D48C
		internal void MoveTo(FlowPosition flow)
		{
			this._flowNode = flow._flowNode;
			this._offset = flow._offset;
		}

		// Token: 0x06004D0A RID: 19722 RVA: 0x0023E4A6 File Offset: 0x0023D4A6
		internal void AttachElement(FixedElement e)
		{
			this._flowNode.AttachElement(e);
		}

		// Token: 0x06004D0B RID: 19723 RVA: 0x0023E4B4 File Offset: 0x0023D4B4
		internal void GetFlowNode(LogicalDirection direction, out FlowNode flowNode, out int offsetStart)
		{
			FlowPosition clingPosition = this.GetClingPosition(direction);
			offsetStart = clingPosition._offset;
			flowNode = clingPosition._flowNode;
		}

		// Token: 0x06004D0C RID: 19724 RVA: 0x0023E4DC File Offset: 0x0023D4DC
		internal void GetFlowNodes(FlowPosition pEnd, out FlowNode[] flowNodes, out int offsetStart, out int offsetEnd)
		{
			flowNodes = null;
			offsetStart = 0;
			offsetEnd = 0;
			FlowPosition clingPosition = this.GetClingPosition(LogicalDirection.Forward);
			offsetStart = clingPosition._offset;
			ArrayList arrayList = new ArrayList();
			int i = this.GetDistance(pEnd);
			while (i > 0)
			{
				int num = clingPosition._vScan(LogicalDirection.Forward, i);
				i -= num;
				if (clingPosition.IsRun || clingPosition.IsObject)
				{
					arrayList.Add(clingPosition._flowNode);
					offsetEnd = clingPosition._offset;
				}
			}
			flowNodes = (FlowNode[])arrayList.ToArray(typeof(FlowNode));
		}

		// Token: 0x06004D0D RID: 19725 RVA: 0x0023E564 File Offset: 0x0023D564
		internal FlowPosition GetClingPosition(LogicalDirection dir)
		{
			FlowPosition flowPosition = (FlowPosition)this.Clone();
			if (dir == LogicalDirection.Forward)
			{
				if (this._offset == this._NodeLength)
				{
					FlowNode flowNode = this._xGetNextFlowNode();
					if (!FlowNode.IsNull(flowNode))
					{
						flowPosition._flowNode = flowNode;
						flowPosition._offset = 0;
					}
				}
			}
			else if (this._offset == 0)
			{
				FlowNode flowNode = this._xGetPreviousFlowNode();
				if (!FlowNode.IsNull(flowNode))
				{
					flowPosition._flowNode = flowNode;
					flowPosition._offset = flowPosition._NodeLength;
				}
			}
			return flowPosition;
		}

		// Token: 0x06004D0E RID: 19726 RVA: 0x0023E5D9 File Offset: 0x0023D5D9
		internal bool IsVirtual(FlowNode flowNode)
		{
			return flowNode.Type == FlowNodeType.Virtual;
		}

		// Token: 0x170011D0 RID: 4560
		// (get) Token: 0x06004D0F RID: 19727 RVA: 0x0023E5E5 File Offset: 0x0023D5E5
		internal FixedTextContainer TextContainer
		{
			get
			{
				return this._container;
			}
		}

		// Token: 0x170011D1 RID: 4561
		// (get) Token: 0x06004D10 RID: 19728 RVA: 0x0023E5ED File Offset: 0x0023D5ED
		internal bool IsBoundary
		{
			get
			{
				return this._flowNode.Type == FlowNodeType.Boundary;
			}
		}

		// Token: 0x170011D2 RID: 4562
		// (get) Token: 0x06004D11 RID: 19729 RVA: 0x0023E5FD File Offset: 0x0023D5FD
		internal bool IsStart
		{
			get
			{
				return this._flowNode.Type == FlowNodeType.Start;
			}
		}

		// Token: 0x170011D3 RID: 4563
		// (get) Token: 0x06004D12 RID: 19730 RVA: 0x0023E60D File Offset: 0x0023D60D
		internal bool IsEnd
		{
			get
			{
				return this._flowNode.Type == FlowNodeType.End;
			}
		}

		// Token: 0x170011D4 RID: 4564
		// (get) Token: 0x06004D13 RID: 19731 RVA: 0x0023E620 File Offset: 0x0023D620
		internal bool IsSymbol
		{
			get
			{
				FlowNodeType type = this._flowNode.Type;
				return type == FlowNodeType.Start || type == FlowNodeType.End || type == FlowNodeType.Object;
			}
		}

		// Token: 0x170011D5 RID: 4565
		// (get) Token: 0x06004D14 RID: 19732 RVA: 0x0023E647 File Offset: 0x0023D647
		internal bool IsRun
		{
			get
			{
				return this._flowNode.Type == FlowNodeType.Run;
			}
		}

		// Token: 0x170011D6 RID: 4566
		// (get) Token: 0x06004D15 RID: 19733 RVA: 0x0023E657 File Offset: 0x0023D657
		internal bool IsObject
		{
			get
			{
				return this._flowNode.Type == FlowNodeType.Object;
			}
		}

		// Token: 0x170011D7 RID: 4567
		// (get) Token: 0x06004D16 RID: 19734 RVA: 0x0023E667 File Offset: 0x0023D667
		internal FlowNode FlowNode
		{
			get
			{
				return this._flowNode;
			}
		}

		// Token: 0x06004D17 RID: 19735 RVA: 0x0023E670 File Offset: 0x0023D670
		private int _vScan(LogicalDirection dir, int limit)
		{
			if (limit == 0)
			{
				return 0;
			}
			FlowNode flowNode = this._flowNode;
			int num = 0;
			if (dir == LogicalDirection.Forward)
			{
				if (this._offset == this._NodeLength || flowNode.Type == FlowNodeType.Boundary)
				{
					flowNode = this._xGetNextFlowNode();
					if (FlowNode.IsNull(flowNode))
					{
						return num;
					}
					this._flowNode = flowNode;
					num = this._NodeLength;
				}
				else
				{
					num = this._NodeLength - this._offset;
				}
				this._offset = this._NodeLength;
				if (limit > 0 && num > limit)
				{
					int num2 = num - limit;
					num = limit;
					this._offset -= num2;
				}
			}
			else
			{
				if (this._offset == 0 || flowNode.Type == FlowNodeType.Boundary)
				{
					flowNode = this._xGetPreviousFlowNode();
					if (FlowNode.IsNull(flowNode))
					{
						return num;
					}
					this._flowNode = flowNode;
					num = this._NodeLength;
				}
				else
				{
					num = this._offset;
				}
				this._offset = 0;
				if (limit > 0 && num > limit)
				{
					int num3 = num - limit;
					num = limit;
					this._offset += num3;
				}
			}
			return num;
		}

		// Token: 0x06004D18 RID: 19736 RVA: 0x0023E75C File Offset: 0x0023D75C
		private TextPointerContext _vGetSymbolType(LogicalDirection dir)
		{
			FlowNode flowNode = this._flowNode;
			if (dir == LogicalDirection.Forward)
			{
				if (this._offset == this._NodeLength)
				{
					flowNode = this._xGetNextFlowNode();
				}
				if (!FlowNode.IsNull(flowNode))
				{
					return this._FlowNodeTypeToTextSymbol(flowNode.Type);
				}
			}
			else
			{
				if (this._offset == 0)
				{
					flowNode = this._xGetPreviousFlowNode();
				}
				if (!FlowNode.IsNull(flowNode))
				{
					return this._FlowNodeTypeToTextSymbol(flowNode.Type);
				}
			}
			return TextPointerContext.None;
		}

		// Token: 0x06004D19 RID: 19737 RVA: 0x0023E7C4 File Offset: 0x0023D7C4
		private FlowNode _xGetPreviousFlowNode()
		{
			if (this._flowNode.Fp > 1)
			{
				FlowNode flowNode = this._FixedFlowMap[this._flowNode.Fp - 1];
				if (this.IsVirtual(flowNode))
				{
					this._FixedTextBuilder.EnsureTextOMForPage((int)flowNode.Cookie);
					flowNode = this._FixedFlowMap[this._flowNode.Fp - 1];
				}
				if (flowNode.Type != FlowNodeType.Boundary)
				{
					return flowNode;
				}
			}
			return null;
		}

		// Token: 0x06004D1A RID: 19738 RVA: 0x0023E83C File Offset: 0x0023D83C
		private FlowNode _xGetNextFlowNode()
		{
			if (this._flowNode.Fp < this._FixedFlowMap.FlowCount - 1)
			{
				FlowNode flowNode = this._FixedFlowMap[this._flowNode.Fp + 1];
				if (this.IsVirtual(flowNode))
				{
					this._FixedTextBuilder.EnsureTextOMForPage((int)flowNode.Cookie);
					flowNode = this._FixedFlowMap[this._flowNode.Fp + 1];
				}
				if (flowNode.Type != FlowNodeType.Boundary)
				{
					return flowNode;
				}
			}
			return null;
		}

		// Token: 0x06004D1B RID: 19739 RVA: 0x0023E8C0 File Offset: 0x0023D8C0
		private bool _IsSamePosition(FlowPosition flow)
		{
			return flow != null && this._OverlapAwareCompare(flow) == 0;
		}

		// Token: 0x06004D1C RID: 19740 RVA: 0x0023E8D4 File Offset: 0x0023D8D4
		private int _OverlapAwareCompare(FlowPosition flow)
		{
			if (this == flow)
			{
				return 0;
			}
			int num = this._flowNode.CompareTo(flow._flowNode);
			if (num < 0)
			{
				if (this._flowNode.Fp == flow._flowNode.Fp - 1 && this._offset == this._NodeLength && flow._offset == 0)
				{
					return 0;
				}
			}
			else if (num > 0)
			{
				if (flow._flowNode.Fp == this._flowNode.Fp - 1 && flow._offset == flow._NodeLength && this._offset == 0)
				{
					return 0;
				}
			}
			else
			{
				num = this._offset.CompareTo(flow._offset);
			}
			return num;
		}

		// Token: 0x06004D1D RID: 19741 RVA: 0x0023E978 File Offset: 0x0023D978
		private TextPointerContext _FlowNodeTypeToTextSymbol(FlowNodeType t)
		{
			switch (t)
			{
			case FlowNodeType.Start:
				return TextPointerContext.ElementStart;
			case FlowNodeType.Run:
				return TextPointerContext.Text;
			case (FlowNodeType)3:
				break;
			case FlowNodeType.End:
				return TextPointerContext.ElementEnd;
			default:
				if (t == FlowNodeType.Object || t == FlowNodeType.Noop)
				{
					return TextPointerContext.EmbeddedElement;
				}
				break;
			}
			return TextPointerContext.None;
		}

		// Token: 0x170011D8 RID: 4568
		// (get) Token: 0x06004D1E RID: 19742 RVA: 0x0023E9A6 File Offset: 0x0023D9A6
		private int _NodeLength
		{
			get
			{
				if (this.IsRun)
				{
					return (int)this._flowNode.Cookie;
				}
				return 1;
			}
		}

		// Token: 0x170011D9 RID: 4569
		// (get) Token: 0x06004D1F RID: 19743 RVA: 0x0023E9C2 File Offset: 0x0023D9C2
		private FixedTextBuilder _FixedTextBuilder
		{
			get
			{
				return this._container.FixedTextBuilder;
			}
		}

		// Token: 0x170011DA RID: 4570
		// (get) Token: 0x06004D20 RID: 19744 RVA: 0x0023E9CF File Offset: 0x0023D9CF
		private FixedFlowMap _FixedFlowMap
		{
			get
			{
				return this._container.FixedTextBuilder.FixedFlowMap;
			}
		}

		// Token: 0x040027F1 RID: 10225
		private FixedTextContainer _container;

		// Token: 0x040027F2 RID: 10226
		private FlowNode _flowNode;

		// Token: 0x040027F3 RID: 10227
		private int _offset;
	}
}
