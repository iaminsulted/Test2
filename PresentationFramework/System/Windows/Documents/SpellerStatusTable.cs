using System;
using System.Collections;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x0200068B RID: 1675
	internal class SpellerStatusTable
	{
		// Token: 0x06005314 RID: 21268 RVA: 0x0025AFA0 File Offset: 0x00259FA0
		internal SpellerStatusTable(ITextPointer textContainerStart, SpellerHighlightLayer highlightLayer)
		{
			this._highlightLayer = highlightLayer;
			this._runList = new ArrayList(1);
			this._runList.Add(new SpellerStatusTable.Run(textContainerStart, SpellerStatusTable.RunType.Dirty));
		}

		// Token: 0x06005315 RID: 21269 RVA: 0x0025AFD0 File Offset: 0x00259FD0
		internal void OnTextChange(TextContainerChangeEventArgs e)
		{
			if (e.TextChange == TextChangeType.ContentAdded)
			{
				this.OnContentAdded(e);
			}
			else if (e.TextChange == TextChangeType.ContentRemoved)
			{
				this.OnContentRemoved(e.ITextPosition);
			}
			else
			{
				ITextPointer textPointer = e.ITextPosition.CreatePointer(e.Count);
				textPointer.Freeze();
				this.MarkDirtyRange(e.ITextPosition, textPointer);
			}
			this.DebugAssertRunList();
		}

		// Token: 0x06005316 RID: 21270 RVA: 0x0025B030 File Offset: 0x0025A030
		internal void GetFirstDirtyRange(ITextPointer searchStart, out ITextPointer start, out ITextPointer end)
		{
			start = null;
			end = null;
			int num = this.FindIndex(searchStart.CreateStaticPointer(), LogicalDirection.Forward);
			while (num >= 0 && num < this._runList.Count)
			{
				SpellerStatusTable.Run run = this.GetRun(num);
				if (run.RunType == SpellerStatusTable.RunType.Dirty)
				{
					start = TextPointerBase.Max(searchStart, run.Position);
					end = this.GetRunEndPositionDynamic(num);
					return;
				}
				num++;
			}
		}

		// Token: 0x06005317 RID: 21271 RVA: 0x0025B092 File Offset: 0x0025A092
		internal void MarkCleanRange(ITextPointer start, ITextPointer end)
		{
			this.MarkRange(start, end, SpellerStatusTable.RunType.Clean);
			this.DebugAssertRunList();
		}

		// Token: 0x06005318 RID: 21272 RVA: 0x0025B0A3 File Offset: 0x0025A0A3
		internal void MarkDirtyRange(ITextPointer start, ITextPointer end)
		{
			this.MarkRange(start, end, SpellerStatusTable.RunType.Dirty);
			this.DebugAssertRunList();
		}

		// Token: 0x06005319 RID: 21273 RVA: 0x0025B0B4 File Offset: 0x0025A0B4
		internal void MarkErrorRange(ITextPointer start, ITextPointer end)
		{
			int num = this.FindIndex(start.CreateStaticPointer(), LogicalDirection.Forward);
			SpellerStatusTable.Run run = this.GetRun(num);
			Invariant.Assert(run.RunType == SpellerStatusTable.RunType.Clean);
			Invariant.Assert(run.Position.CompareTo(start) <= 0);
			Invariant.Assert(this.GetRunEndPosition(num).CompareTo(end) >= 0);
			if (run.Position.CompareTo(start) == 0)
			{
				run.RunType = SpellerStatusTable.RunType.Error;
			}
			else
			{
				this._runList.Insert(num + 1, new SpellerStatusTable.Run(start, SpellerStatusTable.RunType.Error));
				num++;
			}
			if (this.GetRunEndPosition(num).CompareTo(end) > 0)
			{
				this._runList.Insert(num + 1, new SpellerStatusTable.Run(end, SpellerStatusTable.RunType.Clean));
			}
			this._highlightLayer.FireChangedEvent(start, end);
			this.DebugAssertRunList();
		}

		// Token: 0x0600531A RID: 21274 RVA: 0x0025B184 File Offset: 0x0025A184
		internal bool IsRunType(StaticTextPointer textPosition, LogicalDirection direction, SpellerStatusTable.RunType runType)
		{
			int num = this.FindIndex(textPosition, direction);
			return num >= 0 && this.GetRun(num).RunType == runType;
		}

		// Token: 0x0600531B RID: 21275 RVA: 0x0025B1B0 File Offset: 0x0025A1B0
		internal StaticTextPointer GetNextErrorTransition(StaticTextPointer textPosition, LogicalDirection direction)
		{
			StaticTextPointer staticTextPointer = StaticTextPointer.Null;
			int num = this.FindIndex(textPosition, direction);
			if (num != -1)
			{
				if (direction == LogicalDirection.Forward)
				{
					if (this.IsErrorRun(num))
					{
						staticTextPointer = this.GetRunEndPosition(num);
					}
					else
					{
						for (int i = num + 1; i < this._runList.Count; i++)
						{
							if (this.IsErrorRun(i))
							{
								staticTextPointer = this.GetRun(i).Position.CreateStaticPointer();
								break;
							}
						}
					}
				}
				else if (this.IsErrorRun(num))
				{
					staticTextPointer = this.GetRun(num).Position.CreateStaticPointer();
				}
				else
				{
					for (int i = num - 1; i > 0; i--)
					{
						if (this.IsErrorRun(i))
						{
							staticTextPointer = this.GetRunEndPosition(i);
							break;
						}
					}
				}
			}
			Invariant.Assert(staticTextPointer.IsNull || textPosition.CompareTo(staticTextPointer) != 0);
			return staticTextPointer;
		}

		// Token: 0x0600531C RID: 21276 RVA: 0x0025B27C File Offset: 0x0025A27C
		internal bool GetError(StaticTextPointer textPosition, LogicalDirection direction, out ITextPointer start, out ITextPointer end)
		{
			start = null;
			end = null;
			int errorIndex = this.GetErrorIndex(textPosition, direction);
			if (errorIndex >= 0)
			{
				start = this.GetRun(errorIndex).Position;
				end = this.GetRunEndPositionDynamic(errorIndex);
			}
			return start != null;
		}

		// Token: 0x0600531D RID: 21277 RVA: 0x0025B2BC File Offset: 0x0025A2BC
		internal bool GetRun(StaticTextPointer position, LogicalDirection direction, out SpellerStatusTable.RunType runType, out StaticTextPointer end)
		{
			int num = this.FindIndex(position, direction);
			runType = SpellerStatusTable.RunType.Clean;
			end = StaticTextPointer.Null;
			if (num < 0)
			{
				return false;
			}
			SpellerStatusTable.Run run = this.GetRun(num);
			runType = run.RunType;
			end = ((direction == LogicalDirection.Forward) ? this.GetRunEndPosition(num) : run.Position.CreateStaticPointer());
			return true;
		}

		// Token: 0x0600531E RID: 21278 RVA: 0x0025B318 File Offset: 0x0025A318
		private int GetErrorIndex(StaticTextPointer textPosition, LogicalDirection direction)
		{
			int num = this.FindIndex(textPosition, direction);
			if (num >= 0)
			{
				SpellerStatusTable.Run run = this.GetRun(num);
				if (run.RunType == SpellerStatusTable.RunType.Clean || run.RunType == SpellerStatusTable.RunType.Dirty)
				{
					num = -1;
				}
			}
			return num;
		}

		// Token: 0x0600531F RID: 21279 RVA: 0x0025B350 File Offset: 0x0025A350
		private int FindIndex(StaticTextPointer position, LogicalDirection direction)
		{
			int num = -1;
			int i = 0;
			int num2 = this._runList.Count;
			while (i < num2)
			{
				num = (i + num2) / 2;
				SpellerStatusTable.Run run = this.GetRun(num);
				if ((direction == LogicalDirection.Forward && position.CompareTo(run.Position) < 0) || (direction == LogicalDirection.Backward && position.CompareTo(run.Position) <= 0))
				{
					num2 = num;
				}
				else
				{
					if ((direction != LogicalDirection.Forward || position.CompareTo(this.GetRunEndPosition(num)) < 0) && (direction != LogicalDirection.Backward || position.CompareTo(this.GetRunEndPosition(num)) <= 0))
					{
						break;
					}
					i = num + 1;
				}
			}
			if (i >= num2)
			{
				num = -1;
			}
			return num;
		}

		// Token: 0x06005320 RID: 21280 RVA: 0x0025B3E0 File Offset: 0x0025A3E0
		private void MarkRange(ITextPointer start, ITextPointer end, SpellerStatusTable.RunType runType)
		{
			if (start.CompareTo(end) == 0)
			{
				return;
			}
			Invariant.Assert(runType == SpellerStatusTable.RunType.Clean || runType == SpellerStatusTable.RunType.Dirty);
			int num = this.FindIndex(start.CreateStaticPointer(), LogicalDirection.Forward);
			int num2 = this.FindIndex(end.CreateStaticPointer(), LogicalDirection.Backward);
			Invariant.Assert(num >= 0);
			Invariant.Assert(num2 >= 0);
			if (num + 1 < num2)
			{
				for (int i = num + 1; i < num2; i++)
				{
					this.NotifyHighlightLayerBeforeRunChange(i);
				}
				this._runList.RemoveRange(num + 1, num2 - num - 1);
				num2 = num + 1;
			}
			if (num == num2)
			{
				this.AddRun(num, start, end, runType);
				return;
			}
			Invariant.Assert(num == num2 - 1);
			this.AddRun(num, start, end, runType);
			num2 = this.FindIndex(end.CreateStaticPointer(), LogicalDirection.Backward);
			Invariant.Assert(num2 >= 0);
			this.AddRun(num2, start, end, runType);
		}

		// Token: 0x06005321 RID: 21281 RVA: 0x0025B4B4 File Offset: 0x0025A4B4
		private void AddRun(int index, ITextPointer start, ITextPointer end, SpellerStatusTable.RunType runType)
		{
			Invariant.Assert(runType == SpellerStatusTable.RunType.Clean || runType == SpellerStatusTable.RunType.Dirty);
			Invariant.Assert(start.CompareTo(end) < 0);
			SpellerStatusTable.RunType runType2 = (runType == SpellerStatusTable.RunType.Clean) ? SpellerStatusTable.RunType.Dirty : SpellerStatusTable.RunType.Clean;
			SpellerStatusTable.Run run = this.GetRun(index);
			if (run.RunType == runType)
			{
				this.TryToMergeRunWithNeighbors(index);
				return;
			}
			if (run.RunType != runType2)
			{
				run.RunType = runType;
				ITextPointer position = run.Position;
				ITextPointer runEndPositionDynamic = this.GetRunEndPositionDynamic(index);
				this.TryToMergeRunWithNeighbors(index);
				this._highlightLayer.FireChangedEvent(position, runEndPositionDynamic);
				return;
			}
			if (run.Position.CompareTo(start) >= 0)
			{
				if (this.GetRunEndPosition(index).CompareTo(end) <= 0)
				{
					run.RunType = runType;
					this.TryToMergeRunWithNeighbors(index);
					return;
				}
				if (index > 0 && this.GetRun(index - 1).RunType == runType)
				{
					run.Position = end;
					return;
				}
				run.RunType = runType;
				SpellerStatusTable.Run value = new SpellerStatusTable.Run(end, runType2);
				this._runList.Insert(index + 1, value);
				return;
			}
			else
			{
				SpellerStatusTable.Run value;
				if (this.GetRunEndPosition(index).CompareTo(end) > 0)
				{
					value = new SpellerStatusTable.Run(start, runType);
					this._runList.Insert(index + 1, value);
					value = new SpellerStatusTable.Run(end, runType2);
					this._runList.Insert(index + 2, value);
					return;
				}
				if (index < this._runList.Count - 1 && this.GetRun(index + 1).RunType == runType)
				{
					this.GetRun(index + 1).Position = start;
					return;
				}
				value = new SpellerStatusTable.Run(start, runType);
				this._runList.Insert(index + 1, value);
				return;
			}
		}

		// Token: 0x06005322 RID: 21282 RVA: 0x0025B640 File Offset: 0x0025A640
		private void TryToMergeRunWithNeighbors(int index)
		{
			SpellerStatusTable.Run run = this.GetRun(index);
			if (index > 0 && this.GetRun(index - 1).RunType == run.RunType)
			{
				this._runList.RemoveAt(index);
				index--;
			}
			if (index < this._runList.Count - 1 && this.GetRun(index + 1).RunType == run.RunType)
			{
				this._runList.RemoveAt(index + 1);
			}
		}

		// Token: 0x06005323 RID: 21283 RVA: 0x0025B6B4 File Offset: 0x0025A6B4
		private void OnContentAdded(TextContainerChangeEventArgs e)
		{
			ITextPointer textPointer;
			if (e.ITextPosition.Offset > 0)
			{
				textPointer = e.ITextPosition.CreatePointer(-1);
			}
			else
			{
				textPointer = e.ITextPosition;
			}
			textPointer.Freeze();
			ITextPointer textPointer2;
			if (e.ITextPosition.Offset + e.Count < e.ITextPosition.TextContainer.SymbolCount - 1)
			{
				textPointer2 = e.ITextPosition.CreatePointer(e.Count + 1);
			}
			else
			{
				textPointer2 = e.ITextPosition.CreatePointer(e.Count);
			}
			textPointer2.Freeze();
			this.MarkRange(textPointer, textPointer2, SpellerStatusTable.RunType.Dirty);
		}

		// Token: 0x06005324 RID: 21284 RVA: 0x0025B748 File Offset: 0x0025A748
		private void OnContentRemoved(ITextPointer position)
		{
			int num = this.FindIndex(position.CreateStaticPointer(), LogicalDirection.Backward);
			if (num == -1)
			{
				num = 0;
			}
			SpellerStatusTable.Run run = this.GetRun(num);
			if (run.RunType != SpellerStatusTable.RunType.Dirty)
			{
				this.NotifyHighlightLayerBeforeRunChange(num);
				run.RunType = SpellerStatusTable.RunType.Dirty;
				if (num > 0 && this.GetRun(num - 1).RunType == SpellerStatusTable.RunType.Dirty)
				{
					this._runList.RemoveAt(num);
					num--;
				}
			}
			num++;
			int i;
			for (i = num; i < this._runList.Count; i++)
			{
				ITextPointer position2 = this.GetRun(i).Position;
				if (position2.CompareTo(position) > 0 && position2.CompareTo(this.GetRunEndPosition(i)) != 0)
				{
					break;
				}
			}
			this._runList.RemoveRange(num, i - num);
			if (num < this._runList.Count)
			{
				this.NotifyHighlightLayerBeforeRunChange(num);
				this._runList.RemoveAt(num);
				if (num < this._runList.Count && this.GetRun(num).RunType == SpellerStatusTable.RunType.Dirty)
				{
					this._runList.RemoveAt(num);
				}
			}
		}

		// Token: 0x06005325 RID: 21285 RVA: 0x0025B848 File Offset: 0x0025A848
		private void NotifyHighlightLayerBeforeRunChange(int index)
		{
			if (this.IsErrorRun(index))
			{
				ITextPointer position = this.GetRun(index).Position;
				ITextPointer runEndPositionDynamic = this.GetRunEndPositionDynamic(index);
				if (position.CompareTo(runEndPositionDynamic) != 0)
				{
					this._highlightLayer.FireChangedEvent(position, runEndPositionDynamic);
				}
			}
		}

		// Token: 0x06005326 RID: 21286 RVA: 0x0025B88C File Offset: 0x0025A88C
		private void DebugAssertRunList()
		{
			Invariant.Assert(this._runList.Count >= 1, "Run list should never be empty!");
			if (Invariant.Strict)
			{
				SpellerStatusTable.RunType runType = SpellerStatusTable.RunType.Clean;
				for (int i = 0; i < this._runList.Count; i++)
				{
					SpellerStatusTable.Run run = this.GetRun(i);
					if (this._runList.Count == 1)
					{
						Invariant.Assert(run.Position.CompareTo(run.Position.TextContainer.Start) == 0);
					}
					else
					{
						Invariant.Assert(run.Position.CompareTo(this.GetRunEndPosition(i)) <= 0, "Found negative width run!");
					}
					Invariant.Assert(i == 0 || this.GetRunEndPosition(i - 1).CompareTo(run.Position) <= 0, "Found overlapping runs!");
					if (!this.IsErrorRun(i))
					{
						Invariant.Assert(i == 0 || runType != run.RunType, "Found consecutive dirty/dirt or clean/clean runs!");
					}
					runType = run.RunType;
				}
			}
		}

		// Token: 0x06005327 RID: 21287 RVA: 0x0025B992 File Offset: 0x0025A992
		private SpellerStatusTable.Run GetRun(int index)
		{
			return (SpellerStatusTable.Run)this._runList[index];
		}

		// Token: 0x06005328 RID: 21288 RVA: 0x0025B9A8 File Offset: 0x0025A9A8
		private ITextPointer GetRunEndPositionDynamic(int index)
		{
			return this.GetRunEndPosition(index).CreateDynamicTextPointer(LogicalDirection.Forward);
		}

		// Token: 0x06005329 RID: 21289 RVA: 0x0025B9C8 File Offset: 0x0025A9C8
		private StaticTextPointer GetRunEndPosition(int index)
		{
			StaticTextPointer result;
			if (index + 1 < this._runList.Count)
			{
				result = this.GetRun(index + 1).Position.CreateStaticPointer();
			}
			else
			{
				ITextContainer textContainer = this.GetRun(index).Position.TextContainer;
				result = textContainer.CreateStaticPointerAtOffset(textContainer.SymbolCount);
			}
			return result;
		}

		// Token: 0x0600532A RID: 21290 RVA: 0x0025BA1C File Offset: 0x0025AA1C
		private bool IsErrorRun(int index)
		{
			SpellerStatusTable.Run run = this.GetRun(index);
			return run.RunType != SpellerStatusTable.RunType.Clean && run.RunType != SpellerStatusTable.RunType.Dirty;
		}

		// Token: 0x04002ED2 RID: 11986
		private readonly SpellerHighlightLayer _highlightLayer;

		// Token: 0x04002ED3 RID: 11987
		private readonly ArrayList _runList;

		// Token: 0x02000B5A RID: 2906
		internal enum RunType
		{
			// Token: 0x040048A3 RID: 18595
			Clean,
			// Token: 0x040048A4 RID: 18596
			Dirty,
			// Token: 0x040048A5 RID: 18597
			Error
		}

		// Token: 0x02000B5B RID: 2907
		private class Run
		{
			// Token: 0x06008DB5 RID: 36277 RVA: 0x0033ECBF File Offset: 0x0033DCBF
			internal Run(ITextPointer position, SpellerStatusTable.RunType runType)
			{
				this._position = position.GetFrozenPointer(LogicalDirection.Backward);
				this._runType = runType;
			}

			// Token: 0x17001EF7 RID: 7927
			// (get) Token: 0x06008DB6 RID: 36278 RVA: 0x0033ECDB File Offset: 0x0033DCDB
			// (set) Token: 0x06008DB7 RID: 36279 RVA: 0x0033ECE3 File Offset: 0x0033DCE3
			internal ITextPointer Position
			{
				get
				{
					return this._position;
				}
				set
				{
					this._position = value;
				}
			}

			// Token: 0x17001EF8 RID: 7928
			// (get) Token: 0x06008DB8 RID: 36280 RVA: 0x0033ECEC File Offset: 0x0033DCEC
			// (set) Token: 0x06008DB9 RID: 36281 RVA: 0x0033ECF4 File Offset: 0x0033DCF4
			internal SpellerStatusTable.RunType RunType
			{
				get
				{
					return this._runType;
				}
				set
				{
					this._runType = value;
				}
			}

			// Token: 0x040048A6 RID: 18598
			private ITextPointer _position;

			// Token: 0x040048A7 RID: 18599
			private SpellerStatusTable.RunType _runType;
		}
	}
}
