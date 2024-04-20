using System;
using System.Collections.Generic;

namespace System.Windows.Documents
{
	// Token: 0x0200060C RID: 1548
	internal class FixedSOMLineRanges
	{
		// Token: 0x06004B5B RID: 19291 RVA: 0x00236D4C File Offset: 0x00235D4C
		public void AddRange(double start, double end)
		{
			int i = 0;
			while (i < this.Start.Count)
			{
				if (start > this.End[i] + 3.0)
				{
					i++;
				}
				else
				{
					if (end + 3.0 < this.Start[i])
					{
						this.Start.Insert(i, start);
						this.End.Insert(i, end);
						return;
					}
					if (this.Start[i] < start)
					{
						start = this.Start[i];
					}
					if (this.End[i] > end)
					{
						end = this.End[i];
					}
					this.Start.RemoveAt(i);
					this.End.RemoveAt(i);
				}
			}
			this.Start.Add(start);
			this.End.Add(end);
		}

		// Token: 0x06004B5C RID: 19292 RVA: 0x00236E34 File Offset: 0x00235E34
		public int GetLineAt(double line)
		{
			int num = 0;
			int i = this.Start.Count - 1;
			while (i > num)
			{
				int num2 = num + i >> 1;
				if (line > this.End[num2])
				{
					num = num2 + 1;
				}
				else
				{
					i = num2;
				}
			}
			if (num == i && line <= this.End[num] && line >= this.Start[num])
			{
				return num;
			}
			return -1;
		}

		// Token: 0x17001150 RID: 4432
		// (get) Token: 0x06004B5E RID: 19294 RVA: 0x00236EA2 File Offset: 0x00235EA2
		// (set) Token: 0x06004B5D RID: 19293 RVA: 0x00236E99 File Offset: 0x00235E99
		public double Line
		{
			get
			{
				return this._line;
			}
			set
			{
				this._line = value;
			}
		}

		// Token: 0x17001151 RID: 4433
		// (get) Token: 0x06004B5F RID: 19295 RVA: 0x00236EAA File Offset: 0x00235EAA
		public List<double> Start
		{
			get
			{
				if (this._start == null)
				{
					this._start = new List<double>();
				}
				return this._start;
			}
		}

		// Token: 0x17001152 RID: 4434
		// (get) Token: 0x06004B60 RID: 19296 RVA: 0x00236EC5 File Offset: 0x00235EC5
		public List<double> End
		{
			get
			{
				if (this._end == null)
				{
					this._end = new List<double>();
				}
				return this._end;
			}
		}

		// Token: 0x17001153 RID: 4435
		// (get) Token: 0x06004B61 RID: 19297 RVA: 0x00236EE0 File Offset: 0x00235EE0
		public int Count
		{
			get
			{
				return this.Start.Count;
			}
		}

		// Token: 0x17001154 RID: 4436
		// (get) Token: 0x06004B62 RID: 19298 RVA: 0x00236EED File Offset: 0x00235EED
		public static double MinLineSeparation
		{
			get
			{
				return 3.0;
			}
		}

		// Token: 0x0400277D RID: 10109
		private double _line;

		// Token: 0x0400277E RID: 10110
		private List<double> _start;

		// Token: 0x0400277F RID: 10111
		private List<double> _end;

		// Token: 0x04002780 RID: 10112
		private const double _minLineSeparation = 3.0;
	}
}
