using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020001F1 RID: 497
	internal sealed class TextContentRange
	{
		// Token: 0x060011AE RID: 4526 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		internal TextContentRange()
		{
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x001450AC File Offset: 0x001440AC
		internal TextContentRange(int cpFirst, int cpLast, ITextContainer textContainer)
		{
			Invariant.Assert(cpFirst <= cpLast);
			Invariant.Assert(cpFirst >= 0);
			Invariant.Assert(textContainer != null);
			Invariant.Assert(cpLast <= textContainer.SymbolCount);
			this._cpFirst = cpFirst;
			this._cpLast = cpLast;
			this._size = 0;
			this._ranges = null;
			this._textContainer = textContainer;
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x00145114 File Offset: 0x00144114
		internal void Merge(TextContentRange other)
		{
			Invariant.Assert(other != null);
			if (other._textContainer == null)
			{
				return;
			}
			if (this._textContainer == null)
			{
				this._cpFirst = other._cpFirst;
				this._cpLast = other._cpLast;
				this._textContainer = other._textContainer;
				this._size = other._size;
				if (this._size != 0)
				{
					Invariant.Assert(other._ranges != null);
					Invariant.Assert(other._ranges.Length >= other._size * 2);
					this._ranges = new int[this._size * 2];
					for (int i = 0; i < this._ranges.Length; i++)
					{
						this._ranges[i] = other._ranges[i];
					}
				}
			}
			else
			{
				Invariant.Assert(this._textContainer == other._textContainer);
				if (other.IsSimple)
				{
					this.Merge(other._cpFirst, other._cpLast);
				}
				else
				{
					for (int j = 0; j < other._size; j++)
					{
						this.Merge(other._ranges[j * 2], other._ranges[j * 2 + 1]);
					}
				}
			}
			this.Normalize();
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x0014523C File Offset: 0x0014423C
		internal ReadOnlyCollection<TextSegment> GetTextSegments()
		{
			List<TextSegment> list;
			if (this._textContainer == null)
			{
				list = new List<TextSegment>();
			}
			else if (this.IsSimple)
			{
				list = new List<TextSegment>(1);
				list.Add(new TextSegment(this._textContainer.CreatePointerAtOffset(this._cpFirst, LogicalDirection.Forward), this._textContainer.CreatePointerAtOffset(this._cpLast, LogicalDirection.Backward), true));
			}
			else
			{
				list = new List<TextSegment>(this._size);
				for (int i = 0; i < this._size; i++)
				{
					list.Add(new TextSegment(this._textContainer.CreatePointerAtOffset(this._ranges[i * 2], LogicalDirection.Forward), this._textContainer.CreatePointerAtOffset(this._ranges[i * 2 + 1], LogicalDirection.Backward), true));
				}
			}
			return new ReadOnlyCollection<TextSegment>(list);
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x001452FC File Offset: 0x001442FC
		internal bool Contains(ITextPointer position, bool strict)
		{
			bool result = false;
			int offset = position.Offset;
			if (this.IsSimple)
			{
				if (offset >= this._cpFirst && offset <= this._cpLast)
				{
					result = true;
					if (strict && this._cpFirst != this._cpLast && ((offset == this._cpFirst && position.LogicalDirection == LogicalDirection.Backward) || (offset == this._cpLast && position.LogicalDirection == LogicalDirection.Forward)))
					{
						result = false;
					}
				}
			}
			else
			{
				int i = 0;
				while (i < this._size)
				{
					if (offset >= this._ranges[i * 2] && offset <= this._ranges[i * 2 + 1])
					{
						result = true;
						if (strict && ((offset == this._ranges[i * 2] && position.LogicalDirection == LogicalDirection.Backward) || (offset == this._ranges[i * 2 + 1] && position.LogicalDirection == LogicalDirection.Forward)))
						{
							result = false;
							break;
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
			return result;
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x060011B3 RID: 4531 RVA: 0x001453D8 File Offset: 0x001443D8
		internal ITextPointer StartPosition
		{
			get
			{
				ITextPointer result = null;
				if (this._textContainer != null)
				{
					result = this._textContainer.CreatePointerAtOffset(this.IsSimple ? this._cpFirst : this._ranges[0], LogicalDirection.Forward);
				}
				return result;
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x060011B4 RID: 4532 RVA: 0x00145418 File Offset: 0x00144418
		internal ITextPointer EndPosition
		{
			get
			{
				ITextPointer result = null;
				if (this._textContainer != null)
				{
					result = this._textContainer.CreatePointerAtOffset(this.IsSimple ? this._cpLast : this._ranges[(this._size - 1) * 2 + 1], LogicalDirection.Backward);
				}
				return result;
			}
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00145460 File Offset: 0x00144460
		private void Merge(int cpFirst, int cpLast)
		{
			if (!this.IsSimple)
			{
				int i;
				for (i = 0; i < this._size; i++)
				{
					if (cpLast < this._ranges[i * 2])
					{
						this.EnsureSize();
						for (int j = this._size * 2 - 1; j >= i * 2; j--)
						{
							this._ranges[j + 2] = this._ranges[j];
						}
						this._ranges[i * 2] = cpFirst;
						this._ranges[i * 2 + 1] = cpLast;
						this._size++;
						break;
					}
					if (cpFirst <= this._ranges[i * 2 + 1])
					{
						this._ranges[i * 2] = Math.Min(this._ranges[i * 2], cpFirst);
						this._ranges[i * 2 + 1] = Math.Max(this._ranges[i * 2 + 1], cpLast);
						while (this.MergeWithNext(i))
						{
						}
						break;
					}
				}
				if (i >= this._size)
				{
					this.EnsureSize();
					this._ranges[this._size * 2] = cpFirst;
					this._ranges[this._size * 2 + 1] = cpLast;
					this._size++;
				}
				return;
			}
			if (cpFirst <= this._cpLast && cpLast >= this._cpFirst)
			{
				this._cpFirst = Math.Min(this._cpFirst, cpFirst);
				this._cpLast = Math.Max(this._cpLast, cpLast);
				return;
			}
			this._size = 2;
			this._ranges = new int[8];
			if (cpFirst > this._cpLast)
			{
				this._ranges[0] = this._cpFirst;
				this._ranges[1] = this._cpLast;
				this._ranges[2] = cpFirst;
				this._ranges[3] = cpLast;
				return;
			}
			this._ranges[0] = cpFirst;
			this._ranges[1] = cpLast;
			this._ranges[2] = this._cpFirst;
			this._ranges[3] = this._cpLast;
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x00145638 File Offset: 0x00144638
		private bool MergeWithNext(int pos)
		{
			if (pos < this._size - 1 && this._ranges[pos * 2 + 1] >= this._ranges[(pos + 1) * 2])
			{
				this._ranges[pos * 2 + 1] = Math.Max(this._ranges[pos * 2 + 1], this._ranges[(pos + 1) * 2 + 1]);
				for (int i = (pos + 1) * 2; i < (this._size - 1) * 2; i++)
				{
					this._ranges[i] = this._ranges[i + 2];
				}
				this._size--;
				return true;
			}
			return false;
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x001456D4 File Offset: 0x001446D4
		private void EnsureSize()
		{
			Invariant.Assert(this._size > 0);
			Invariant.Assert(this._ranges != null);
			if (this._ranges.Length < (this._size + 1) * 2)
			{
				int[] array = new int[this._ranges.Length * 2];
				for (int i = 0; i < this._size * 2; i++)
				{
					array[i] = this._ranges[i];
				}
				this._ranges = array;
			}
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x00145746 File Offset: 0x00144746
		private void Normalize()
		{
			if (this._size == 1)
			{
				this._cpFirst = this._ranges[0];
				this._cpLast = this._ranges[1];
				this._size = 0;
				this._ranges = null;
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x060011B9 RID: 4537 RVA: 0x0014577B File Offset: 0x0014477B
		private bool IsSimple
		{
			get
			{
				return this._size == 0;
			}
		}

		// Token: 0x04000B18 RID: 2840
		private int _cpFirst;

		// Token: 0x04000B19 RID: 2841
		private int _cpLast;

		// Token: 0x04000B1A RID: 2842
		private int _size;

		// Token: 0x04000B1B RID: 2843
		private int[] _ranges;

		// Token: 0x04000B1C RID: 2844
		private ITextContainer _textContainer;
	}
}
