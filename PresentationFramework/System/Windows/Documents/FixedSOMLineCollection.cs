using System;
using System.Collections.Generic;

namespace System.Windows.Documents
{
	// Token: 0x0200060B RID: 1547
	internal sealed class FixedSOMLineCollection
	{
		// Token: 0x06004B52 RID: 19282 RVA: 0x00236B18 File Offset: 0x00235B18
		public FixedSOMLineCollection()
		{
			this._verticals = new List<FixedSOMLineRanges>();
			this._horizontals = new List<FixedSOMLineRanges>();
		}

		// Token: 0x06004B53 RID: 19283 RVA: 0x00236B36 File Offset: 0x00235B36
		public bool IsVerticallySeparated(double left, double top, double right, double bottom)
		{
			return this._IsSeparated(this._verticals, left, top, right, bottom);
		}

		// Token: 0x06004B54 RID: 19284 RVA: 0x00236B49 File Offset: 0x00235B49
		public bool IsHorizontallySeparated(double left, double top, double right, double bottom)
		{
			return this._IsSeparated(this._horizontals, top, left, bottom, right);
		}

		// Token: 0x06004B55 RID: 19285 RVA: 0x00236B5C File Offset: 0x00235B5C
		public void AddVertical(Point point1, Point point2)
		{
			this._AddLineToRanges(this._verticals, point1.X, point1.Y, point2.Y);
		}

		// Token: 0x06004B56 RID: 19286 RVA: 0x00236B7F File Offset: 0x00235B7F
		public void AddHorizontal(Point point1, Point point2)
		{
			this._AddLineToRanges(this._horizontals, point1.Y, point1.X, point2.X);
		}

		// Token: 0x06004B57 RID: 19287 RVA: 0x00236BA4 File Offset: 0x00235BA4
		private void _AddLineToRanges(List<FixedSOMLineRanges> ranges, double line, double start, double end)
		{
			if (start > end)
			{
				double num = start;
				start = end;
				end = num;
			}
			double num2 = 0.5 * FixedSOMLineRanges.MinLineSeparation;
			FixedSOMLineRanges fixedSOMLineRanges;
			for (int i = 0; i < ranges.Count; i++)
			{
				if (line < ranges[i].Line - num2)
				{
					fixedSOMLineRanges = new FixedSOMLineRanges();
					fixedSOMLineRanges.Line = line;
					fixedSOMLineRanges.AddRange(start, end);
					ranges.Insert(i, fixedSOMLineRanges);
					return;
				}
				if (line < ranges[i].Line + num2)
				{
					ranges[i].AddRange(start, end);
					return;
				}
			}
			fixedSOMLineRanges = new FixedSOMLineRanges();
			fixedSOMLineRanges.Line = line;
			fixedSOMLineRanges.AddRange(start, end);
			ranges.Add(fixedSOMLineRanges);
		}

		// Token: 0x06004B58 RID: 19288 RVA: 0x00236C4C File Offset: 0x00235C4C
		private bool _IsSeparated(List<FixedSOMLineRanges> lines, double parallelLowEnd, double perpLowEnd, double parallelHighEnd, double perpHighEnd)
		{
			int num = 0;
			int i = lines.Count;
			if (i == 0)
			{
				return false;
			}
			int num2 = 0;
			while (i > num)
			{
				num2 = num + i >> 1;
				if (lines[num2].Line < parallelLowEnd)
				{
					num = num2 + 1;
				}
				else
				{
					if (lines[num2].Line <= parallelHighEnd)
					{
						break;
					}
					i = num2;
				}
			}
			if (lines[num2].Line >= parallelLowEnd && lines[num2].Line <= parallelHighEnd)
			{
				do
				{
					num2--;
				}
				while (num2 >= 0 && lines[num2].Line >= parallelLowEnd);
				num2++;
				while (num2 < lines.Count && lines[num2].Line <= parallelHighEnd)
				{
					double num3 = (perpHighEnd - perpLowEnd) * 0.1;
					int lineAt = lines[num2].GetLineAt(perpLowEnd + num3);
					if (lineAt >= 0 && lines[num2].End[lineAt] >= perpHighEnd - num3)
					{
						return true;
					}
					num2++;
				}
			}
			return false;
		}

		// Token: 0x1700114E RID: 4430
		// (get) Token: 0x06004B59 RID: 19289 RVA: 0x00236D39 File Offset: 0x00235D39
		public List<FixedSOMLineRanges> HorizontalLines
		{
			get
			{
				return this._horizontals;
			}
		}

		// Token: 0x1700114F RID: 4431
		// (get) Token: 0x06004B5A RID: 19290 RVA: 0x00236D41 File Offset: 0x00235D41
		public List<FixedSOMLineRanges> VerticalLines
		{
			get
			{
				return this._verticals;
			}
		}

		// Token: 0x0400277A RID: 10106
		private List<FixedSOMLineRanges> _horizontals;

		// Token: 0x0400277B RID: 10107
		private List<FixedSOMLineRanges> _verticals;

		// Token: 0x0400277C RID: 10108
		private const double _fudgeFactor = 0.1;
	}
}
