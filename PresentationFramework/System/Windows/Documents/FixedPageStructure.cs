using System;
using System.Collections.Generic;

namespace System.Windows.Documents
{
	// Token: 0x020005FE RID: 1534
	internal sealed class FixedPageStructure
	{
		// Token: 0x06004AEA RID: 19178 RVA: 0x002351D0 File Offset: 0x002341D0
		internal FixedPageStructure(int pageIndex)
		{
			this._pageIndex = pageIndex;
			this._flowStart = new FlowNode(-1, FlowNodeType.Virtual, pageIndex);
			this._flowEnd = this._flowStart;
			this._fixedStart = FixedNode.Create(pageIndex, 1, int.MinValue, -1, null);
			this._fixedEnd = FixedNode.Create(pageIndex, 1, int.MaxValue, -1, null);
		}

		// Token: 0x06004AEB RID: 19179 RVA: 0x00235232 File Offset: 0x00234232
		internal void SetupLineResults(FixedLineResult[] lineResults)
		{
			this._lineResults = lineResults;
		}

		// Token: 0x06004AEC RID: 19180 RVA: 0x0023523C File Offset: 0x0023423C
		internal FixedNode[] GetNextLine(int line, bool forward, ref int count)
		{
			if (forward)
			{
				while (line < this._lineResults.Length - 1)
				{
					if (count <= 0)
					{
						break;
					}
					line++;
					count--;
				}
			}
			else
			{
				while (line > 0 && count > 0)
				{
					line--;
					count--;
				}
			}
			if (count <= 0)
			{
				line = Math.Max(0, Math.Min(line, this._lineResults.Length - 1));
				return this._lineResults[line].Nodes;
			}
			return null;
		}

		// Token: 0x06004AED RID: 19181 RVA: 0x002352B0 File Offset: 0x002342B0
		internal FixedNode[] FindSnapToLine(Point pt)
		{
			FixedLineResult fixedLineResult = null;
			FixedLineResult fixedLineResult2 = null;
			double num = double.MaxValue;
			double num2 = double.MaxValue;
			double num3 = double.MaxValue;
			foreach (FixedLineResult fixedLineResult3 in this._lineResults)
			{
				double num4 = Math.Max(0.0, (pt.Y > fixedLineResult3.LayoutBox.Y) ? (pt.Y - fixedLineResult3.LayoutBox.Bottom) : (fixedLineResult3.LayoutBox.Y - pt.Y));
				double num5 = Math.Max(0.0, (pt.X > fixedLineResult3.LayoutBox.X) ? (pt.X - fixedLineResult3.LayoutBox.Right) : (fixedLineResult3.LayoutBox.X - pt.X));
				if (num4 == 0.0 && num5 == 0.0)
				{
					return fixedLineResult3.Nodes;
				}
				if (num4 < num || (num4 == num && num5 < num2))
				{
					num = num4;
					num2 = num5;
					fixedLineResult = fixedLineResult3;
				}
				double num6 = 5.0 * num4 + num5;
				if (num6 < num3 && num4 < fixedLineResult3.LayoutBox.Height)
				{
					num3 = num6;
					fixedLineResult2 = fixedLineResult3;
				}
			}
			if (fixedLineResult == null)
			{
				return null;
			}
			if (fixedLineResult2 != null && (fixedLineResult2.LayoutBox.Left > fixedLineResult.LayoutBox.Right || fixedLineResult.LayoutBox.Left > fixedLineResult2.LayoutBox.Right))
			{
				return fixedLineResult2.Nodes;
			}
			return fixedLineResult.Nodes;
		}

		// Token: 0x06004AEE RID: 19182 RVA: 0x0023547F File Offset: 0x0023447F
		internal void SetFlowBoundary(FlowNode flowStart, FlowNode flowEnd)
		{
			this._flowStart = flowStart;
			this._flowEnd = flowEnd;
		}

		// Token: 0x06004AEF RID: 19183 RVA: 0x0023548F File Offset: 0x0023448F
		public void ConstructFixedSOMPage(List<FixedNode> fixedNodes)
		{
			this._fixedSOMPageConstructor.ConstructPageStructure(fixedNodes);
		}

		// Token: 0x17001127 RID: 4391
		// (get) Token: 0x06004AF0 RID: 19184 RVA: 0x0023549E File Offset: 0x0023449E
		internal FixedNode[] LastLine
		{
			get
			{
				if (this._lineResults.Length != 0)
				{
					return this._lineResults[this._lineResults.Length - 1].Nodes;
				}
				return null;
			}
		}

		// Token: 0x17001128 RID: 4392
		// (get) Token: 0x06004AF1 RID: 19185 RVA: 0x002354C1 File Offset: 0x002344C1
		internal FixedNode[] FirstLine
		{
			get
			{
				if (this._lineResults.Length != 0)
				{
					return this._lineResults[0].Nodes;
				}
				return null;
			}
		}

		// Token: 0x17001129 RID: 4393
		// (get) Token: 0x06004AF2 RID: 19186 RVA: 0x002354DB File Offset: 0x002344DB
		internal int PageIndex
		{
			get
			{
				return this._pageIndex;
			}
		}

		// Token: 0x1700112A RID: 4394
		// (get) Token: 0x06004AF3 RID: 19187 RVA: 0x002354E3 File Offset: 0x002344E3
		internal bool Loaded
		{
			get
			{
				return this._flowStart != null && this._flowStart.Type != FlowNodeType.Virtual;
			}
		}

		// Token: 0x1700112B RID: 4395
		// (get) Token: 0x06004AF4 RID: 19188 RVA: 0x00235501 File Offset: 0x00234501
		internal FlowNode FlowStart
		{
			get
			{
				return this._flowStart;
			}
		}

		// Token: 0x1700112C RID: 4396
		// (get) Token: 0x06004AF5 RID: 19189 RVA: 0x00235509 File Offset: 0x00234509
		internal FlowNode FlowEnd
		{
			get
			{
				return this._flowEnd;
			}
		}

		// Token: 0x1700112D RID: 4397
		// (get) Token: 0x06004AF6 RID: 19190 RVA: 0x00235511 File Offset: 0x00234511
		// (set) Token: 0x06004AF7 RID: 19191 RVA: 0x00235519 File Offset: 0x00234519
		internal FixedSOMPage FixedSOMPage
		{
			get
			{
				return this._fixedSOMPage;
			}
			set
			{
				this._fixedSOMPage = value;
			}
		}

		// Token: 0x1700112E RID: 4398
		// (get) Token: 0x06004AF8 RID: 19192 RVA: 0x00235522 File Offset: 0x00234522
		// (set) Token: 0x06004AF9 RID: 19193 RVA: 0x0023552A File Offset: 0x0023452A
		internal FixedDSBuilder FixedDSBuilder
		{
			get
			{
				return this._fixedDSBuilder;
			}
			set
			{
				this._fixedDSBuilder = value;
			}
		}

		// Token: 0x1700112F RID: 4399
		// (get) Token: 0x06004AFA RID: 19194 RVA: 0x00235533 File Offset: 0x00234533
		// (set) Token: 0x06004AFB RID: 19195 RVA: 0x0023553B File Offset: 0x0023453B
		internal FixedSOMPageConstructor PageConstructor
		{
			get
			{
				return this._fixedSOMPageConstructor;
			}
			set
			{
				this._fixedSOMPageConstructor = value;
			}
		}

		// Token: 0x04002742 RID: 10050
		private readonly int _pageIndex;

		// Token: 0x04002743 RID: 10051
		private FlowNode _flowStart;

		// Token: 0x04002744 RID: 10052
		private FlowNode _flowEnd;

		// Token: 0x04002745 RID: 10053
		private FixedNode _fixedStart;

		// Token: 0x04002746 RID: 10054
		private FixedNode _fixedEnd;

		// Token: 0x04002747 RID: 10055
		private FixedSOMPageConstructor _fixedSOMPageConstructor;

		// Token: 0x04002748 RID: 10056
		private FixedSOMPage _fixedSOMPage;

		// Token: 0x04002749 RID: 10057
		private FixedDSBuilder _fixedDSBuilder;

		// Token: 0x0400274A RID: 10058
		private FixedLineResult[] _lineResults;
	}
}
