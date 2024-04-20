using System;

namespace System.Windows.Documents
{
	// Token: 0x020005FC RID: 1532
	internal struct FixedNode : IComparable
	{
		// Token: 0x06004AA5 RID: 19109 RVA: 0x00234477 File Offset: 0x00233477
		internal static FixedNode Create(int pageIndex, int childLevels, int level1Index, int level2Index, int[] childPath)
		{
			if (childLevels == 1)
			{
				return new FixedNode(pageIndex, level1Index);
			}
			if (childLevels != 2)
			{
				return FixedNode.Create(pageIndex, childPath);
			}
			return new FixedNode(pageIndex, level1Index, level2Index);
		}

		// Token: 0x06004AA6 RID: 19110 RVA: 0x0023449C File Offset: 0x0023349C
		internal static FixedNode Create(int pageIndex, int[] childPath)
		{
			int[] array = new int[childPath.Length + 1];
			array[0] = pageIndex;
			childPath.CopyTo(array, 1);
			return new FixedNode(array);
		}

		// Token: 0x06004AA7 RID: 19111 RVA: 0x002344C6 File Offset: 0x002334C6
		private FixedNode(int page, int level1Index)
		{
			this._path = new int[2];
			this._path[0] = page;
			this._path[1] = level1Index;
		}

		// Token: 0x06004AA8 RID: 19112 RVA: 0x002344E6 File Offset: 0x002334E6
		private FixedNode(int page, int level1Index, int level2Index)
		{
			this._path = new int[3];
			this._path[0] = page;
			this._path[1] = level1Index;
			this._path[2] = level2Index;
		}

		// Token: 0x06004AA9 RID: 19113 RVA: 0x0023450F File Offset: 0x0023350F
		private FixedNode(int[] path)
		{
			this._path = path;
		}

		// Token: 0x06004AAA RID: 19114 RVA: 0x00234518 File Offset: 0x00233518
		public int CompareTo(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			if (o.GetType() != typeof(FixedNode))
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					o.GetType(),
					typeof(FixedNode)
				}), "o");
			}
			FixedNode fixedNode = (FixedNode)o;
			return this.CompareTo(fixedNode);
		}

		// Token: 0x06004AAB RID: 19115 RVA: 0x0023458C File Offset: 0x0023358C
		public int CompareTo(FixedNode fixedNode)
		{
			int num = this.Page.CompareTo(fixedNode.Page);
			if (num == 0)
			{
				int num2 = 1;
				while (num2 <= this.ChildLevels && num2 <= fixedNode.ChildLevels)
				{
					int num3 = this[num2];
					int num4 = fixedNode[num2];
					if (num3 != num4)
					{
						return num3.CompareTo(num4);
					}
					num2++;
				}
			}
			return num;
		}

		// Token: 0x06004AAC RID: 19116 RVA: 0x002345F4 File Offset: 0x002335F4
		internal int ComparetoIndex(int[] childPath)
		{
			int num = 0;
			while (num < childPath.Length && num < this._path.Length - 1)
			{
				if (childPath[num] != this._path[num + 1])
				{
					return childPath[num].CompareTo(this._path[num + 1]);
				}
				num++;
			}
			return 0;
		}

		// Token: 0x06004AAD RID: 19117 RVA: 0x00234643 File Offset: 0x00233643
		public static bool operator <(FixedNode fp1, FixedNode fp2)
		{
			return fp1.CompareTo(fp2) < 0;
		}

		// Token: 0x06004AAE RID: 19118 RVA: 0x00234650 File Offset: 0x00233650
		public static bool operator <=(FixedNode fp1, FixedNode fp2)
		{
			return fp1.CompareTo(fp2) <= 0;
		}

		// Token: 0x06004AAF RID: 19119 RVA: 0x00234660 File Offset: 0x00233660
		public static bool operator >(FixedNode fp1, FixedNode fp2)
		{
			return fp1.CompareTo(fp2) > 0;
		}

		// Token: 0x06004AB0 RID: 19120 RVA: 0x0023466D File Offset: 0x0023366D
		public static bool operator >=(FixedNode fp1, FixedNode fp2)
		{
			return fp1.CompareTo(fp2) >= 0;
		}

		// Token: 0x06004AB1 RID: 19121 RVA: 0x0023467D File Offset: 0x0023367D
		public override bool Equals(object o)
		{
			return o is FixedNode && this.Equals((FixedNode)o);
		}

		// Token: 0x06004AB2 RID: 19122 RVA: 0x00234695 File Offset: 0x00233695
		public bool Equals(FixedNode fixedp)
		{
			return this.CompareTo(fixedp) == 0;
		}

		// Token: 0x06004AB3 RID: 19123 RVA: 0x002346A1 File Offset: 0x002336A1
		public static bool operator ==(FixedNode fp1, FixedNode fp2)
		{
			return fp1.Equals(fp2);
		}

		// Token: 0x06004AB4 RID: 19124 RVA: 0x002346AB File Offset: 0x002336AB
		public static bool operator !=(FixedNode fp1, FixedNode fp2)
		{
			return !fp1.Equals(fp2);
		}

		// Token: 0x06004AB5 RID: 19125 RVA: 0x002346B8 File Offset: 0x002336B8
		public override int GetHashCode()
		{
			int num = 0;
			foreach (int num2 in this._path)
			{
				num = 43 * num + num2;
			}
			return num;
		}

		// Token: 0x1700111B RID: 4379
		// (get) Token: 0x06004AB6 RID: 19126 RVA: 0x002346E8 File Offset: 0x002336E8
		internal int Page
		{
			get
			{
				return this._path[0];
			}
		}

		// Token: 0x1700111C RID: 4380
		internal int this[int level]
		{
			get
			{
				return this._path[level];
			}
		}

		// Token: 0x1700111D RID: 4381
		// (get) Token: 0x06004AB8 RID: 19128 RVA: 0x002346FC File Offset: 0x002336FC
		internal int ChildLevels
		{
			get
			{
				return this._path.Length - 1;
			}
		}

		// Token: 0x04002736 RID: 10038
		private readonly int[] _path;
	}
}
