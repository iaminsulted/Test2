using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Windows.Documents
{
	// Token: 0x020005F6 RID: 1526
	internal sealed class FixedFlowMap
	{
		// Token: 0x06004A81 RID: 19073 RVA: 0x00233D99 File Offset: 0x00232D99
		internal FixedFlowMap()
		{
			this._Init();
		}

		// Token: 0x1700110C RID: 4364
		internal FlowNode this[int fp]
		{
			get
			{
				return this._flowOrder[fp];
			}
		}

		// Token: 0x06004A83 RID: 19075 RVA: 0x00233DB8 File Offset: 0x00232DB8
		internal void MappingReplace(FlowNode flowOld, List<FlowNode> flowNew)
		{
			int fp = flowOld.Fp;
			this._flowOrder.RemoveAt(fp);
			this._flowOrder.InsertRange(fp, flowNew);
			for (int i = fp; i < this._flowOrder.Count; i++)
			{
				this._flowOrder[i].SetFp(i);
			}
		}

		// Token: 0x06004A84 RID: 19076 RVA: 0x00233E10 File Offset: 0x00232E10
		internal FixedSOMElement MappingGetFixedSOMElement(FixedNode fixedp, int offset)
		{
			List<FixedSOMElement> list = this._GetEntry(fixedp);
			if (list != null)
			{
				foreach (FixedSOMElement fixedSOMElement in list)
				{
					if (offset >= fixedSOMElement.StartIndex && offset <= fixedSOMElement.EndIndex)
					{
						return fixedSOMElement;
					}
				}
			}
			return null;
		}

		// Token: 0x06004A85 RID: 19077 RVA: 0x00233E7C File Offset: 0x00232E7C
		internal FlowNode FlowOrderInsertBefore(FlowNode nextFlow, FlowNode newFlow)
		{
			this._FlowOrderInsertBefore(nextFlow, newFlow);
			return newFlow;
		}

		// Token: 0x06004A86 RID: 19078 RVA: 0x00233E87 File Offset: 0x00232E87
		internal void AddFixedElement(FixedSOMElement element)
		{
			this._AddEntry(element);
		}

		// Token: 0x1700110D RID: 4365
		// (get) Token: 0x06004A87 RID: 19079 RVA: 0x00233E90 File Offset: 0x00232E90
		internal FixedNode FixedStartEdge
		{
			get
			{
				return FixedFlowMap.s_FixedStart;
			}
		}

		// Token: 0x1700110E RID: 4366
		// (get) Token: 0x06004A88 RID: 19080 RVA: 0x00233E97 File Offset: 0x00232E97
		internal FlowNode FlowStartEdge
		{
			get
			{
				return this._flowStart;
			}
		}

		// Token: 0x1700110F RID: 4367
		// (get) Token: 0x06004A89 RID: 19081 RVA: 0x00233E9F File Offset: 0x00232E9F
		internal FlowNode FlowEndEdge
		{
			get
			{
				return this._flowEnd;
			}
		}

		// Token: 0x17001110 RID: 4368
		// (get) Token: 0x06004A8A RID: 19082 RVA: 0x00233EA7 File Offset: 0x00232EA7
		internal int FlowCount
		{
			get
			{
				return this._flowOrder.Count;
			}
		}

		// Token: 0x06004A8B RID: 19083 RVA: 0x00233EB4 File Offset: 0x00232EB4
		private void _Init()
		{
			this._flowStart = new FlowNode(int.MinValue, FlowNodeType.Boundary, null);
			this._flowEnd = new FlowNode(int.MinValue, FlowNodeType.Boundary, null);
			this._flowOrder = new List<FlowNode>();
			this._flowOrder.Add(this._flowStart);
			this._flowStart.SetFp(0);
			this._flowOrder.Add(this._flowEnd);
			this._flowEnd.SetFp(1);
			this._mapping = new Hashtable();
		}

		// Token: 0x06004A8C RID: 19084 RVA: 0x00233F38 File Offset: 0x00232F38
		internal void _FlowOrderInsertBefore(FlowNode nextFlow, FlowNode newFlow)
		{
			newFlow.SetFp(nextFlow.Fp);
			this._flowOrder.Insert(newFlow.Fp, newFlow);
			int i = newFlow.Fp + 1;
			int count = this._flowOrder.Count;
			while (i < count)
			{
				this._flowOrder[i].IncreaseFp();
				i++;
			}
		}

		// Token: 0x06004A8D RID: 19085 RVA: 0x00233F94 File Offset: 0x00232F94
		private List<FixedSOMElement> _GetEntry(FixedNode node)
		{
			if (this._cachedEntry == null || node != this._cachedFixedNode)
			{
				this._cachedEntry = (List<FixedSOMElement>)this._mapping[node];
				this._cachedFixedNode = node;
			}
			return this._cachedEntry;
		}

		// Token: 0x06004A8E RID: 19086 RVA: 0x00233FE0 File Offset: 0x00232FE0
		private void _AddEntry(FixedSOMElement element)
		{
			FixedNode fixedNode = element.FixedNode;
			List<FixedSOMElement> list;
			if (this._mapping.ContainsKey(fixedNode))
			{
				list = (List<FixedSOMElement>)this._mapping[fixedNode];
			}
			else
			{
				list = new List<FixedSOMElement>();
				this._mapping.Add(fixedNode, list);
			}
			list.Add(element);
		}

		// Token: 0x0400271B RID: 10011
		internal const int FixedOrderStartPage = -2147483648;

		// Token: 0x0400271C RID: 10012
		internal const int FixedOrderEndPage = 2147483647;

		// Token: 0x0400271D RID: 10013
		internal const int FixedOrderStartVisual = -2147483648;

		// Token: 0x0400271E RID: 10014
		internal const int FixedOrderEndVisual = 2147483647;

		// Token: 0x0400271F RID: 10015
		internal const int FlowOrderBoundaryScopeId = -2147483648;

		// Token: 0x04002720 RID: 10016
		internal const int FlowOrderVirtualScopeId = -1;

		// Token: 0x04002721 RID: 10017
		internal const int FlowOrderScopeIdStart = 0;

		// Token: 0x04002722 RID: 10018
		private List<FlowNode> _flowOrder;

		// Token: 0x04002723 RID: 10019
		private FlowNode _flowStart;

		// Token: 0x04002724 RID: 10020
		private FlowNode _flowEnd;

		// Token: 0x04002725 RID: 10021
		private static readonly FixedNode s_FixedStart = FixedNode.Create(int.MinValue, 1, int.MinValue, -1, null);

		// Token: 0x04002726 RID: 10022
		private static readonly FixedNode s_FixedEnd = FixedNode.Create(int.MaxValue, 1, int.MaxValue, -1, null);

		// Token: 0x04002727 RID: 10023
		private Hashtable _mapping;

		// Token: 0x04002728 RID: 10024
		private FixedNode _cachedFixedNode;

		// Token: 0x04002729 RID: 10025
		private List<FixedSOMElement> _cachedEntry;
	}
}
