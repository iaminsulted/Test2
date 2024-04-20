using System;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000115 RID: 277
	internal sealed class DtrList
	{
		// Token: 0x06000727 RID: 1831 RVA: 0x0010C5AB File Offset: 0x0010B5AB
		internal DtrList()
		{
			this._dtrs = new DirtyTextRange[4];
			this._count = 0;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0010C5C8 File Offset: 0x0010B5C8
		internal void Merge(DirtyTextRange dtr)
		{
			bool flag = false;
			int i = 0;
			int num = dtr.StartIndex;
			if (this._count > 0)
			{
				while (i < this._count)
				{
					if (num < this._dtrs[i].StartIndex)
					{
						if (num + dtr.PositionsRemoved > this._dtrs[i].StartIndex)
						{
							flag = true;
							break;
						}
						break;
					}
					else
					{
						if (num <= this._dtrs[i].StartIndex + this._dtrs[i].PositionsAdded)
						{
							flag = true;
							break;
						}
						num -= this._dtrs[i].PositionsAdded - this._dtrs[i].PositionsRemoved;
						i++;
					}
				}
				dtr.StartIndex = num;
			}
			if (i < this._count)
			{
				if (flag)
				{
					if (dtr.StartIndex < this._dtrs[i].StartIndex)
					{
						int num2 = this._dtrs[i].StartIndex - dtr.StartIndex;
						int num3 = Math.Min(this._dtrs[i].PositionsAdded, dtr.PositionsRemoved - num2);
						this._dtrs[i].StartIndex = dtr.StartIndex;
						DirtyTextRange[] dtrs = this._dtrs;
						int num4 = i;
						dtrs[num4].PositionsAdded = dtrs[num4].PositionsAdded + (dtr.PositionsAdded - num3);
						DirtyTextRange[] dtrs2 = this._dtrs;
						int num5 = i;
						dtrs2[num5].PositionsRemoved = dtrs2[num5].PositionsRemoved + (dtr.PositionsRemoved - num3);
					}
					else
					{
						int num6 = dtr.StartIndex - this._dtrs[i].StartIndex;
						int num7 = Math.Min(dtr.PositionsRemoved, this._dtrs[i].PositionsAdded - num6);
						DirtyTextRange[] dtrs3 = this._dtrs;
						int num8 = i;
						dtrs3[num8].PositionsAdded = dtrs3[num8].PositionsAdded + (dtr.PositionsAdded - num7);
						DirtyTextRange[] dtrs4 = this._dtrs;
						int num9 = i;
						dtrs4[num9].PositionsRemoved = dtrs4[num9].PositionsRemoved + (dtr.PositionsRemoved - num7);
					}
					DirtyTextRange[] dtrs5 = this._dtrs;
					int num10 = i;
					dtrs5[num10].FromHighlightLayer = (dtrs5[num10].FromHighlightLayer & dtr.FromHighlightLayer);
				}
				else
				{
					if (this._count == this._dtrs.Length)
					{
						this.Resize();
					}
					Array.Copy(this._dtrs, i, this._dtrs, i + 1, this._count - i);
					this._dtrs[i] = dtr;
					this._count++;
				}
				this.MergeWithNext(i);
				return;
			}
			if (this._count == this._dtrs.Length)
			{
				this.Resize();
			}
			this._dtrs[this._count] = dtr;
			this._count++;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0010C888 File Offset: 0x0010B888
		internal DirtyTextRange GetMergedRange()
		{
			if (this._count > 0)
			{
				DirtyTextRange dirtyTextRange = this._dtrs[0];
				int startIndex = dirtyTextRange.StartIndex;
				int positionsAdded = dirtyTextRange.PositionsAdded;
				int positionsRemoved = dirtyTextRange.PositionsRemoved;
				bool flag = dirtyTextRange.FromHighlightLayer;
				for (int i = 1; i < this._count; i++)
				{
					dirtyTextRange = this._dtrs[i];
					int num = dirtyTextRange.StartIndex - startIndex;
					positionsAdded = num + dirtyTextRange.PositionsAdded;
					positionsRemoved = num + dirtyTextRange.PositionsRemoved;
					flag &= dirtyTextRange.FromHighlightLayer;
					startIndex = dirtyTextRange.StartIndex;
				}
				return new DirtyTextRange(this._dtrs[0].StartIndex, positionsAdded, positionsRemoved, flag);
			}
			return new DirtyTextRange(0, 0, 0, false);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0010C948 File Offset: 0x0010B948
		internal DtrList DtrsFromRange(int dcpNew, int cchOld)
		{
			DtrList dtrList = null;
			int i = 0;
			int num = 0;
			while (i < this._count && dcpNew > this._dtrs[i].StartIndex + num + this._dtrs[i].PositionsAdded)
			{
				num += this._dtrs[i].PositionsAdded - this._dtrs[i].PositionsRemoved;
				i++;
			}
			int num2 = i;
			while (i < this._count)
			{
				if (dcpNew - num + cchOld <= this._dtrs[i].StartIndex + this._dtrs[i].PositionsRemoved)
				{
					if (dcpNew - num + cchOld < this._dtrs[i].StartIndex)
					{
						i--;
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			int j = (i < this._count) ? i : (this._count - 1);
			if (j >= num2)
			{
				dtrList = new DtrList();
				while (j >= num2)
				{
					DirtyTextRange dtr = this._dtrs[num2];
					dtr.StartIndex += num;
					dtrList.Append(dtr);
					num2++;
				}
			}
			return dtrList;
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0010CA68 File Offset: 0x0010BA68
		private void MergeWithNext(int index)
		{
			while (index + 1 < this._count)
			{
				DirtyTextRange dirtyTextRange = this._dtrs[index + 1];
				if (dirtyTextRange.StartIndex > this._dtrs[index].StartIndex + this._dtrs[index].PositionsRemoved)
				{
					break;
				}
				DirtyTextRange[] dtrs = this._dtrs;
				dtrs[index].PositionsAdded = dtrs[index].PositionsAdded + dirtyTextRange.PositionsAdded;
				DirtyTextRange[] dtrs2 = this._dtrs;
				dtrs2[index].PositionsRemoved = dtrs2[index].PositionsRemoved + dirtyTextRange.PositionsRemoved;
				DirtyTextRange[] dtrs3 = this._dtrs;
				dtrs3[index].FromHighlightLayer = (dtrs3[index].FromHighlightLayer & dirtyTextRange.FromHighlightLayer);
				for (int i = index + 2; i < this._count; i++)
				{
					this._dtrs[i - 1] = this._dtrs[i];
				}
				this._count--;
			}
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0010CB5E File Offset: 0x0010BB5E
		private void Append(DirtyTextRange dtr)
		{
			if (this._count == this._dtrs.Length)
			{
				this.Resize();
			}
			this._dtrs[this._count] = dtr;
			this._count++;
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0010CB98 File Offset: 0x0010BB98
		private void Resize()
		{
			DirtyTextRange[] array = new DirtyTextRange[this._dtrs.Length * 2];
			Array.Copy(this._dtrs, array, this._dtrs.Length);
			this._dtrs = array;
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x0010CBD0 File Offset: 0x0010BBD0
		internal int Length
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x17000129 RID: 297
		internal DirtyTextRange this[int index]
		{
			get
			{
				return this._dtrs[index];
			}
		}

		// Token: 0x04000743 RID: 1859
		private DirtyTextRange[] _dtrs;

		// Token: 0x04000744 RID: 1860
		private const int _defaultCapacity = 4;

		// Token: 0x04000745 RID: 1861
		private int _count;
	}
}
