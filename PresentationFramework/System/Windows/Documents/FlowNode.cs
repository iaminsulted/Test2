using System;

namespace System.Windows.Documents
{
	// Token: 0x0200061C RID: 1564
	internal sealed class FlowNode : IComparable
	{
		// Token: 0x06004CED RID: 19693 RVA: 0x0023DF77 File Offset: 0x0023CF77
		internal FlowNode(int scopeId, FlowNodeType type, object cookie)
		{
			this._scopeId = scopeId;
			this._type = type;
			this._cookie = cookie;
		}

		// Token: 0x06004CEE RID: 19694 RVA: 0x0023DF94 File Offset: 0x0023CF94
		public static bool IsNull(FlowNode flow)
		{
			return flow == null;
		}

		// Token: 0x06004CEF RID: 19695 RVA: 0x0023DF9A File Offset: 0x0023CF9A
		public override int GetHashCode()
		{
			return this._scopeId.GetHashCode() ^ this._fp.GetHashCode();
		}

		// Token: 0x06004CF0 RID: 19696 RVA: 0x0023DFB4 File Offset: 0x0023CFB4
		public override bool Equals(object o)
		{
			if (o == null || base.GetType() != o.GetType())
			{
				return false;
			}
			FlowNode flowNode = (FlowNode)o;
			return this._fp == flowNode._fp;
		}

		// Token: 0x06004CF1 RID: 19697 RVA: 0x0023DFF0 File Offset: 0x0023CFF0
		public int CompareTo(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			FlowNode flowNode = o as FlowNode;
			if (flowNode == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					o.GetType(),
					typeof(FlowNode)
				}), "o");
			}
			if (this == flowNode)
			{
				return 0;
			}
			int num = this._fp - flowNode._fp;
			if (num == 0)
			{
				return 0;
			}
			if (num < 0)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x06004CF2 RID: 19698 RVA: 0x0023E066 File Offset: 0x0023D066
		internal void SetFp(int fp)
		{
			this._fp = fp;
		}

		// Token: 0x06004CF3 RID: 19699 RVA: 0x0023E06F File Offset: 0x0023D06F
		internal void IncreaseFp()
		{
			this._fp++;
		}

		// Token: 0x06004CF4 RID: 19700 RVA: 0x0023E07F File Offset: 0x0023D07F
		internal void DecreaseFp()
		{
			this._fp--;
		}

		// Token: 0x170011CB RID: 4555
		// (get) Token: 0x06004CF5 RID: 19701 RVA: 0x0023E08F File Offset: 0x0023D08F
		internal int Fp
		{
			get
			{
				return this._fp;
			}
		}

		// Token: 0x170011CC RID: 4556
		// (get) Token: 0x06004CF6 RID: 19702 RVA: 0x0023E097 File Offset: 0x0023D097
		internal int ScopeId
		{
			get
			{
				return this._scopeId;
			}
		}

		// Token: 0x170011CD RID: 4557
		// (get) Token: 0x06004CF7 RID: 19703 RVA: 0x0023E09F File Offset: 0x0023D09F
		internal FlowNodeType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x170011CE RID: 4558
		// (get) Token: 0x06004CF8 RID: 19704 RVA: 0x0023E0A7 File Offset: 0x0023D0A7
		internal object Cookie
		{
			get
			{
				return this._cookie;
			}
		}

		// Token: 0x170011CF RID: 4559
		// (get) Token: 0x06004CF9 RID: 19705 RVA: 0x0023E0AF File Offset: 0x0023D0AF
		// (set) Token: 0x06004CFA RID: 19706 RVA: 0x0023E0B7 File Offset: 0x0023D0B7
		internal FixedSOMElement[] FixedSOMElements
		{
			get
			{
				return this._elements;
			}
			set
			{
				this._elements = value;
			}
		}

		// Token: 0x06004CFB RID: 19707 RVA: 0x0023E0C0 File Offset: 0x0023D0C0
		internal void AttachElement(FixedElement fixedElement)
		{
			this._cookie = fixedElement;
		}

		// Token: 0x040027EC RID: 10220
		private readonly int _scopeId;

		// Token: 0x040027ED RID: 10221
		private readonly FlowNodeType _type;

		// Token: 0x040027EE RID: 10222
		private int _fp;

		// Token: 0x040027EF RID: 10223
		private object _cookie;

		// Token: 0x040027F0 RID: 10224
		private FixedSOMElement[] _elements;
	}
}
