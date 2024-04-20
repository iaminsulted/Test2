using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace System.Windows.Documents
{
	// Token: 0x0200069A RID: 1690
	internal class TextContainerChangedEventArgs : EventArgs
	{
		// Token: 0x06005524 RID: 21796 RVA: 0x00260436 File Offset: 0x0025F436
		internal TextContainerChangedEventArgs()
		{
			this._changes = new SortedList<int, TextChange>();
		}

		// Token: 0x06005525 RID: 21797 RVA: 0x00260449 File Offset: 0x0025F449
		internal void SetLocalPropertyValueChanged()
		{
			this._hasLocalPropertyValueChange = true;
		}

		// Token: 0x06005526 RID: 21798 RVA: 0x00260454 File Offset: 0x0025F454
		internal void AddChange(PrecursorTextChangeType textChange, int offset, int length, bool collectTextChanges)
		{
			if (textChange == PrecursorTextChangeType.ContentAdded || textChange == PrecursorTextChangeType.ElementAdded || textChange == PrecursorTextChangeType.ContentRemoved || textChange == PrecursorTextChangeType.ElementExtracted)
			{
				this._hasContentAddedOrRemoved = true;
			}
			if (!collectTextChanges)
			{
				return;
			}
			if (textChange == PrecursorTextChangeType.ElementAdded)
			{
				this.AddChangeToList(textChange, offset, 1);
				this.AddChangeToList(textChange, offset + length - 1, 1);
				return;
			}
			if (textChange == PrecursorTextChangeType.ElementExtracted)
			{
				this.AddChangeToList(textChange, offset + length - 1, 1);
				this.AddChangeToList(textChange, offset, 1);
				return;
			}
			if (textChange == PrecursorTextChangeType.PropertyModified)
			{
				return;
			}
			this.AddChangeToList(textChange, offset, length);
		}

		// Token: 0x1700141A RID: 5146
		// (get) Token: 0x06005527 RID: 21799 RVA: 0x002604C0 File Offset: 0x0025F4C0
		internal bool HasContentAddedOrRemoved
		{
			get
			{
				return this._hasContentAddedOrRemoved;
			}
		}

		// Token: 0x1700141B RID: 5147
		// (get) Token: 0x06005528 RID: 21800 RVA: 0x002604C8 File Offset: 0x0025F4C8
		internal bool HasLocalPropertyValueChange
		{
			get
			{
				return this._hasLocalPropertyValueChange;
			}
		}

		// Token: 0x1700141C RID: 5148
		// (get) Token: 0x06005529 RID: 21801 RVA: 0x002604D0 File Offset: 0x0025F4D0
		internal SortedList<int, TextChange> Changes
		{
			get
			{
				return this._changes;
			}
		}

		// Token: 0x0600552A RID: 21802 RVA: 0x002604D8 File Offset: 0x0025F4D8
		private void AddChangeToList(PrecursorTextChangeType textChange, int offset, int length)
		{
			int num = 0;
			bool flag = false;
			int num2 = this.Changes.IndexOfKey(offset);
			TextChange textChange2;
			if (num2 != -1)
			{
				textChange2 = this.Changes.Values[num2];
			}
			else
			{
				textChange2 = new TextChange();
				textChange2.Offset = offset;
				this.Changes.Add(offset, textChange2);
				num2 = this.Changes.IndexOfKey(offset);
			}
			if (textChange == PrecursorTextChangeType.ContentAdded || textChange == PrecursorTextChangeType.ElementAdded)
			{
				textChange2.AddedLength += length;
				num = length;
			}
			else if (textChange == PrecursorTextChangeType.ContentRemoved || textChange == PrecursorTextChangeType.ElementExtracted)
			{
				textChange2.RemovedLength += Math.Max(0, length - textChange2.AddedLength);
				textChange2.AddedLength = Math.Max(0, textChange2.AddedLength - length);
				num = -length;
				flag = true;
			}
			int i;
			if (num2 > 0 && textChange != PrecursorTextChangeType.PropertyModified)
			{
				i = num2 - 1;
				TextChange textChange3 = null;
				while (i >= 0)
				{
					TextChange textChange4 = this.Changes.Values[i];
					if (textChange4.Offset + textChange4.AddedLength >= offset && this.MergeTextChangeLeft(textChange4, textChange2, flag, length))
					{
						textChange3 = textChange4;
					}
					i--;
				}
				if (textChange3 != null)
				{
					textChange2 = textChange3;
				}
				num2 = this.Changes.IndexOfKey(textChange2.Offset);
			}
			i = num2 + 1;
			if (flag && i < this.Changes.Count)
			{
				while (i < this.Changes.Count && this.Changes.Values[i].Offset <= offset + length)
				{
					this.MergeTextChangeRight(this.Changes.Values[i], textChange2, offset, length);
				}
				num2 = this.Changes.IndexOfKey(textChange2.Offset);
			}
			if (num != 0)
			{
				SortedList<int, TextChange> sortedList = new SortedList<int, TextChange>(this.Changes.Count);
				for (i = 0; i < this.Changes.Count; i++)
				{
					TextChange textChange5 = this.Changes.Values[i];
					if (i > num2)
					{
						textChange5.Offset += num;
					}
					sortedList.Add(textChange5.Offset, textChange5);
				}
				this._changes = sortedList;
			}
			this.DeleteChangeIfEmpty(textChange2);
		}

		// Token: 0x0600552B RID: 21803 RVA: 0x002606DA File Offset: 0x0025F6DA
		private void DeleteChangeIfEmpty(TextChange change)
		{
			if (change.AddedLength == 0 && change.RemovedLength == 0)
			{
				this.Changes.Remove(change.Offset);
			}
		}

		// Token: 0x0600552C RID: 21804 RVA: 0x00260700 File Offset: 0x0025F700
		private bool MergeTextChangeLeft(TextChange oldChange, TextChange newChange, bool isDeletion, int length)
		{
			if (oldChange.Offset + oldChange.AddedLength >= newChange.Offset)
			{
				if (isDeletion)
				{
					int num = Math.Min(oldChange.AddedLength - (newChange.Offset - oldChange.Offset), newChange.RemovedLength);
					oldChange.AddedLength -= num;
					oldChange.RemovedLength += length - num;
				}
				else
				{
					oldChange.AddedLength += length;
				}
				this.Changes.Remove(newChange.Offset);
				return true;
			}
			return false;
		}

		// Token: 0x0600552D RID: 21805 RVA: 0x0026078C File Offset: 0x0025F78C
		private void MergeTextChangeRight(TextChange oldChange, TextChange newChange, int offset, int length)
		{
			int num = (oldChange.AddedLength > 0) ? (offset + length - oldChange.Offset) : 0;
			if (num >= oldChange.AddedLength)
			{
				newChange.RemovedLength += oldChange.RemovedLength - oldChange.AddedLength;
				this.Changes.Remove(oldChange.Offset);
				return;
			}
			newChange.RemovedLength += oldChange.RemovedLength - num;
			newChange.AddedLength += oldChange.AddedLength - num;
			this.Changes.Remove(oldChange.Offset);
		}

		// Token: 0x04002F14 RID: 12052
		private bool _hasContentAddedOrRemoved;

		// Token: 0x04002F15 RID: 12053
		private bool _hasLocalPropertyValueChange;

		// Token: 0x04002F16 RID: 12054
		private SortedList<int, TextChange> _changes;
	}
}
