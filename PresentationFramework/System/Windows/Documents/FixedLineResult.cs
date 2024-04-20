using System;

namespace System.Windows.Documents
{
	// Token: 0x020005FB RID: 1531
	internal sealed class FixedLineResult : IComparable
	{
		// Token: 0x06004A9E RID: 19102 RVA: 0x0023438F File Offset: 0x0023338F
		internal FixedLineResult(FixedNode[] nodes, Rect layoutBox)
		{
			this._nodes = nodes;
			this._layoutBox = layoutBox;
		}

		// Token: 0x06004A9F RID: 19103 RVA: 0x002343A8 File Offset: 0x002333A8
		public int CompareTo(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			if (o.GetType() != typeof(FixedLineResult))
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					o.GetType(),
					typeof(FixedLineResult)
				}), "o");
			}
			FixedLineResult fixedLineResult = (FixedLineResult)o;
			return this.BaseLine.CompareTo(fixedLineResult.BaseLine);
		}

		// Token: 0x17001116 RID: 4374
		// (get) Token: 0x06004AA0 RID: 19104 RVA: 0x00234426 File Offset: 0x00233426
		internal FixedNode Start
		{
			get
			{
				return this._nodes[0];
			}
		}

		// Token: 0x17001117 RID: 4375
		// (get) Token: 0x06004AA1 RID: 19105 RVA: 0x00234434 File Offset: 0x00233434
		internal FixedNode End
		{
			get
			{
				return this._nodes[this._nodes.Length - 1];
			}
		}

		// Token: 0x17001118 RID: 4376
		// (get) Token: 0x06004AA2 RID: 19106 RVA: 0x0023444B File Offset: 0x0023344B
		internal FixedNode[] Nodes
		{
			get
			{
				return this._nodes;
			}
		}

		// Token: 0x17001119 RID: 4377
		// (get) Token: 0x06004AA3 RID: 19107 RVA: 0x00234454 File Offset: 0x00233454
		internal double BaseLine
		{
			get
			{
				return this._layoutBox.Bottom;
			}
		}

		// Token: 0x1700111A RID: 4378
		// (get) Token: 0x06004AA4 RID: 19108 RVA: 0x0023446F File Offset: 0x0023346F
		internal Rect LayoutBox
		{
			get
			{
				return this._layoutBox;
			}
		}

		// Token: 0x04002734 RID: 10036
		private readonly FixedNode[] _nodes;

		// Token: 0x04002735 RID: 10037
		private readonly Rect _layoutBox;
	}
}
