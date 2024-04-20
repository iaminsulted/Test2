using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace MS.Internal.Documents
{
	// Token: 0x020001FB RID: 507
	internal class UndoManager
	{
		// Token: 0x06001286 RID: 4742 RVA: 0x0014ACAC File Offset: 0x00149CAC
		internal UndoManager()
		{
			this._scope = null;
			this._state = UndoState.Normal;
			this._isEnabled = false;
			this._undoStack = new List<IUndoUnit>(4);
			this._redoStack = new Stack(2);
			this._topUndoIndex = -1;
			this._bottomUndoIndex = 0;
			this._undoLimit = 100;
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x0014AD04 File Offset: 0x00149D04
		internal static void AttachUndoManager(DependencyObject scope, UndoManager undoManager)
		{
			if (scope == null)
			{
				throw new ArgumentNullException("scope");
			}
			if (undoManager == null)
			{
				throw new ArgumentNullException("undoManager");
			}
			if (undoManager != null && undoManager._scope != null)
			{
				throw new InvalidOperationException(SR.Get("UndoManagerAlreadyAttached"));
			}
			UndoManager.DetachUndoManager(scope);
			scope.SetValue(UndoManager.UndoManagerInstanceProperty, undoManager);
			if (undoManager != null)
			{
				undoManager._scope = scope;
			}
			undoManager.IsEnabled = true;
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x0014AD6C File Offset: 0x00149D6C
		internal static void DetachUndoManager(DependencyObject scope)
		{
			if (scope == null)
			{
				throw new ArgumentNullException("scope");
			}
			UndoManager undoManager = scope.ReadLocalValue(UndoManager.UndoManagerInstanceProperty) as UndoManager;
			if (undoManager != null)
			{
				undoManager.IsEnabled = false;
				scope.ClearValue(UndoManager.UndoManagerInstanceProperty);
				if (undoManager != null)
				{
					undoManager._scope = null;
				}
			}
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x0014ADB7 File Offset: 0x00149DB7
		internal static UndoManager GetUndoManager(DependencyObject target)
		{
			if (target == null)
			{
				return null;
			}
			return target.GetValue(UndoManager.UndoManagerInstanceProperty) as UndoManager;
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x0014ADD0 File Offset: 0x00149DD0
		internal void Open(IParentUndoUnit unit)
		{
			if (!this.IsEnabled)
			{
				throw new InvalidOperationException(SR.Get("UndoServiceDisabled"));
			}
			if (unit == null)
			{
				throw new ArgumentNullException("unit");
			}
			IParentUndoUnit deepestOpenUnit = this.DeepestOpenUnit;
			if (deepestOpenUnit == unit)
			{
				throw new InvalidOperationException(SR.Get("UndoUnitCantBeOpenedTwice"));
			}
			if (deepestOpenUnit == null)
			{
				if (unit != this.LastUnit)
				{
					this.Add(unit);
					this.SetLastUnit(unit);
				}
				this.SetOpenedUnit(unit);
				unit.Container = this;
				return;
			}
			unit.Container = deepestOpenUnit;
			deepestOpenUnit.Open(unit);
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x0014AE58 File Offset: 0x00149E58
		internal void Reopen(IParentUndoUnit unit)
		{
			if (!this.IsEnabled)
			{
				throw new InvalidOperationException(SR.Get("UndoServiceDisabled"));
			}
			if (unit == null)
			{
				throw new ArgumentNullException("unit");
			}
			if (this.OpenedUnit != null)
			{
				throw new InvalidOperationException(SR.Get("UndoUnitAlreadyOpen"));
			}
			switch (this.State)
			{
			case UndoState.Normal:
			case UndoState.Redo:
				if (this.UndoCount == 0 || this.PeekUndoStack() != unit)
				{
					throw new InvalidOperationException(SR.Get("UndoUnitNotOnTopOfStack"));
				}
				break;
			case UndoState.Undo:
				if (this.RedoStack.Count == 0 || (IParentUndoUnit)this.RedoStack.Peek() != unit)
				{
					throw new InvalidOperationException(SR.Get("UndoUnitNotOnTopOfStack"));
				}
				break;
			}
			if (unit.Locked)
			{
				throw new InvalidOperationException(SR.Get("UndoUnitLocked"));
			}
			this.Open(unit);
			this._lastReopenedUnit = unit;
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x0014AF39 File Offset: 0x00149F39
		internal void Close(UndoCloseAction closeAction)
		{
			this.Close(this.OpenedUnit, closeAction);
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x0014AF48 File Offset: 0x00149F48
		internal void Close(IParentUndoUnit unit, UndoCloseAction closeAction)
		{
			if (!this.IsEnabled)
			{
				throw new InvalidOperationException(SR.Get("UndoServiceDisabled"));
			}
			if (unit == null)
			{
				throw new ArgumentNullException("unit");
			}
			if (this.OpenedUnit == null)
			{
				throw new InvalidOperationException(SR.Get("UndoNoOpenUnit"));
			}
			if (this.OpenedUnit != unit)
			{
				IParentUndoUnit openedUnit = this.OpenedUnit;
				while (openedUnit.OpenedUnit != null && openedUnit.OpenedUnit != unit)
				{
					openedUnit = openedUnit.OpenedUnit;
				}
				if (openedUnit.OpenedUnit == null)
				{
					throw new ArgumentException(SR.Get("UndoUnitNotFound"), "unit");
				}
				openedUnit.Close(closeAction);
				return;
			}
			else
			{
				if (closeAction != UndoCloseAction.Commit)
				{
					this.SetState(UndoState.Rollback);
					if (unit.OpenedUnit != null)
					{
						unit.Close(closeAction);
					}
					if (closeAction == UndoCloseAction.Rollback)
					{
						unit.Do();
					}
					this.PopUndoStack();
					this.SetOpenedUnit(null);
					this.OnNextDiscard();
					this.SetLastUnit((this._topUndoIndex == -1) ? null : this.PeekUndoStack());
					this.SetState(UndoState.Normal);
					return;
				}
				if (unit.OpenedUnit != null)
				{
					unit.Close(UndoCloseAction.Commit);
				}
				if (this.State != UndoState.Redo && this.State != UndoState.Undo && this.RedoStack.Count > 0)
				{
					this.RedoStack.Clear();
				}
				this.SetOpenedUnit(null);
				return;
			}
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x0014B07C File Offset: 0x0014A07C
		internal void Add(IUndoUnit unit)
		{
			if (!this.IsEnabled)
			{
				throw new InvalidOperationException(SR.Get("UndoServiceDisabled"));
			}
			if (unit == null)
			{
				throw new ArgumentNullException("unit");
			}
			IParentUndoUnit deepestOpenUnit = this.DeepestOpenUnit;
			if (deepestOpenUnit != null)
			{
				deepestOpenUnit.Add(unit);
				return;
			}
			if (!(unit is IParentUndoUnit))
			{
				throw new InvalidOperationException(SR.Get("UndoNoOpenParentUnit"));
			}
			((IParentUndoUnit)unit).Container = this;
			if (this.LastUnit is IParentUndoUnit)
			{
				((IParentUndoUnit)this.LastUnit).OnNextAdd();
			}
			this.SetLastUnit(unit);
			if (this.State == UndoState.Normal || this.State == UndoState.Redo)
			{
				int num = this._topUndoIndex + 1;
				this._topUndoIndex = num;
				if (num == this.UndoLimit)
				{
					this._topUndoIndex = 0;
				}
				if ((this._topUndoIndex >= this.UndoStack.Count || this.PeekUndoStack() != null) && (this.UndoLimit == -1 || this.UndoStack.Count < this.UndoLimit))
				{
					this.UndoStack.Add(unit);
					return;
				}
				if (this.PeekUndoStack() != null)
				{
					num = this._bottomUndoIndex + 1;
					this._bottomUndoIndex = num;
					if (num == this.UndoLimit)
					{
						this._bottomUndoIndex = 0;
					}
				}
				this.UndoStack[this._topUndoIndex] = unit;
				return;
			}
			else
			{
				if (this.State == UndoState.Undo)
				{
					this.RedoStack.Push(unit);
					return;
				}
				UndoState state = this.State;
				return;
			}
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x0014B1DE File Offset: 0x0014A1DE
		internal void Clear()
		{
			if (!this.IsEnabled)
			{
				throw new InvalidOperationException(SR.Get("UndoServiceDisabled"));
			}
			if (!this._imeSupportModeEnabled)
			{
				this.DoClear();
			}
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x0014B208 File Offset: 0x0014A208
		internal void Undo(int count)
		{
			if (!this.IsEnabled)
			{
				throw new InvalidOperationException(SR.Get("UndoServiceDisabled"));
			}
			if (count > this.UndoCount || count <= 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.State != UndoState.Normal)
			{
				throw new InvalidOperationException(SR.Get("UndoNotInNormalState"));
			}
			if (this.OpenedUnit != null)
			{
				throw new InvalidOperationException(SR.Get("UndoUnitOpen"));
			}
			Invariant.Assert(this.UndoCount > this._minUndoStackCount);
			this.SetState(UndoState.Undo);
			bool flag = true;
			try
			{
				while (count > 0)
				{
					this.PopUndoStack().Do();
					count--;
				}
				flag = false;
			}
			finally
			{
				if (flag)
				{
					this.Clear();
				}
			}
			this.SetState(UndoState.Normal);
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x0014B2CC File Offset: 0x0014A2CC
		internal void Redo(int count)
		{
			if (!this.IsEnabled)
			{
				throw new InvalidOperationException(SR.Get("UndoServiceDisabled"));
			}
			if (count > this.RedoStack.Count || count <= 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.State != UndoState.Normal)
			{
				throw new InvalidOperationException(SR.Get("UndoNotInNormalState"));
			}
			if (this.OpenedUnit != null)
			{
				throw new InvalidOperationException(SR.Get("UndoUnitOpen"));
			}
			this.SetState(UndoState.Redo);
			bool flag = true;
			try
			{
				while (count > 0)
				{
					((IUndoUnit)this.RedoStack.Pop()).Do();
					count--;
				}
				flag = false;
			}
			finally
			{
				if (flag)
				{
					this.Clear();
				}
			}
			this.SetState(UndoState.Normal);
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x0014B38C File Offset: 0x0014A38C
		internal virtual void OnNextDiscard()
		{
			if (this.UndoCount > 0)
			{
				((IParentUndoUnit)this.PeekUndoStack()).OnNextDiscard();
			}
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x0014B3A7 File Offset: 0x0014A3A7
		internal IUndoUnit PeekUndoStack()
		{
			if (this._topUndoIndex < 0 || this._topUndoIndex == this.UndoStack.Count)
			{
				return null;
			}
			return this.UndoStack[this._topUndoIndex];
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x0014B3D8 File Offset: 0x0014A3D8
		internal Stack SetRedoStack(Stack value)
		{
			Stack redoStack = this._redoStack;
			if (value == null)
			{
				value = new Stack(2);
			}
			this._redoStack = value;
			return redoStack;
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06001295 RID: 4757 RVA: 0x0014B3F2 File Offset: 0x0014A3F2
		// (set) Token: 0x06001296 RID: 4758 RVA: 0x0014B3FC File Offset: 0x0014A3FC
		internal bool IsImeSupportModeEnabled
		{
			get
			{
				return this._imeSupportModeEnabled;
			}
			set
			{
				if (value != this._imeSupportModeEnabled)
				{
					if (value)
					{
						if (this._bottomUndoIndex != 0 && this._topUndoIndex >= 0)
						{
							List<IUndoUnit> list = new List<IUndoUnit>(this.UndoCount);
							if (this._bottomUndoIndex > this._topUndoIndex)
							{
								for (int i = this._bottomUndoIndex; i < this.UndoLimit; i++)
								{
									list.Add(this._undoStack[i]);
								}
								this._bottomUndoIndex = 0;
							}
							for (int i = this._bottomUndoIndex; i <= this._topUndoIndex; i++)
							{
								list.Add(this._undoStack[i]);
							}
							this._undoStack = list;
							this._bottomUndoIndex = 0;
							this._topUndoIndex = list.Count - 1;
						}
						this._imeSupportModeEnabled = value;
						return;
					}
					this._imeSupportModeEnabled = value;
					if (!this.IsEnabled)
					{
						this.DoClear();
						return;
					}
					if (this.UndoLimit >= 0 && this._topUndoIndex >= this.UndoLimit)
					{
						List<IUndoUnit> list2 = new List<IUndoUnit>(this.UndoLimit);
						for (int j = this._topUndoIndex + 1 - this.UndoLimit; j <= this._topUndoIndex; j++)
						{
							list2.Add(this._undoStack[j]);
						}
						this._undoStack = list2;
						this._bottomUndoIndex = 0;
						this._topUndoIndex = this.UndoLimit - 1;
					}
				}
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06001297 RID: 4759 RVA: 0x0014B54D File Offset: 0x0014A54D
		// (set) Token: 0x06001298 RID: 4760 RVA: 0x0014B55F File Offset: 0x0014A55F
		internal int UndoLimit
		{
			get
			{
				if (!this._imeSupportModeEnabled)
				{
					return this._undoLimit;
				}
				return -1;
			}
			set
			{
				this._undoLimit = value;
				if (!this._imeSupportModeEnabled)
				{
					this.DoClear();
				}
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06001299 RID: 4761 RVA: 0x0014B576 File Offset: 0x0014A576
		internal UndoState State
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x0600129A RID: 4762 RVA: 0x0014B57E File Offset: 0x0014A57E
		// (set) Token: 0x0600129B RID: 4763 RVA: 0x0014B59D File Offset: 0x0014A59D
		internal bool IsEnabled
		{
			get
			{
				return this._imeSupportModeEnabled || (this._isEnabled && this._undoLimit != 0);
			}
			set
			{
				this._isEnabled = value;
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x0600129C RID: 4764 RVA: 0x0014B5A6 File Offset: 0x0014A5A6
		internal IParentUndoUnit OpenedUnit
		{
			get
			{
				return this._openedUnit;
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x0600129D RID: 4765 RVA: 0x0014B5AE File Offset: 0x0014A5AE
		internal IUndoUnit LastUnit
		{
			get
			{
				return this._lastUnit;
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x0600129E RID: 4766 RVA: 0x0014B5B6 File Offset: 0x0014A5B6
		internal IParentUndoUnit LastReopenedUnit
		{
			get
			{
				return this._lastReopenedUnit;
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x0600129F RID: 4767 RVA: 0x0014B5C0 File Offset: 0x0014A5C0
		internal int UndoCount
		{
			get
			{
				int result;
				if (this.UndoStack.Count == 0 || this._topUndoIndex < 0)
				{
					result = 0;
				}
				else if (this._topUndoIndex == this._bottomUndoIndex - 1 && this.PeekUndoStack() == null)
				{
					result = 0;
				}
				else if (this._topUndoIndex >= this._bottomUndoIndex)
				{
					result = this._topUndoIndex - this._bottomUndoIndex + 1;
				}
				else
				{
					result = this._topUndoIndex + (this.UndoLimit - this._bottomUndoIndex) + 1;
				}
				return result;
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x060012A0 RID: 4768 RVA: 0x0014B63B File Offset: 0x0014A63B
		internal int RedoCount
		{
			get
			{
				return this.RedoStack.Count;
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x060012A1 RID: 4769 RVA: 0x0014B648 File Offset: 0x0014A648
		internal static int UndoLimitDefaultValue
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x0014B64C File Offset: 0x0014A64C
		internal IUndoUnit GetUndoUnit(int index)
		{
			Invariant.Assert(index < this.UndoCount);
			Invariant.Assert(index >= 0);
			Invariant.Assert(this._bottomUndoIndex == 0);
			Invariant.Assert(this._imeSupportModeEnabled);
			return this._undoStack[index];
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x0014B698 File Offset: 0x0014A698
		internal void RemoveUndoRange(int index, int count)
		{
			Invariant.Assert(index >= 0);
			Invariant.Assert(count >= 0);
			Invariant.Assert(count + index <= this.UndoCount);
			Invariant.Assert(this._bottomUndoIndex == 0);
			Invariant.Assert(this._imeSupportModeEnabled);
			for (int i = index + count; i <= this._topUndoIndex; i++)
			{
				this._undoStack[i - count] = this._undoStack[i];
			}
			for (int i = this._topUndoIndex - (count - 1); i <= this._topUndoIndex; i++)
			{
				this._undoStack[i] = null;
			}
			this._topUndoIndex -= count;
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x060012A4 RID: 4772 RVA: 0x0014B74B File Offset: 0x0014A74B
		// (set) Token: 0x060012A5 RID: 4773 RVA: 0x0014B753 File Offset: 0x0014A753
		internal int MinUndoStackCount
		{
			get
			{
				return this._minUndoStackCount;
			}
			set
			{
				this._minUndoStackCount = value;
			}
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x0014B75C File Offset: 0x0014A75C
		protected void SetState(UndoState value)
		{
			this._state = value;
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x0014B765 File Offset: 0x0014A765
		protected void SetOpenedUnit(IParentUndoUnit value)
		{
			this._openedUnit = value;
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x0014B76E File Offset: 0x0014A76E
		protected void SetLastUnit(IUndoUnit value)
		{
			this._lastUnit = value;
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x060012A9 RID: 4777 RVA: 0x0014B778 File Offset: 0x0014A778
		protected IParentUndoUnit DeepestOpenUnit
		{
			get
			{
				IParentUndoUnit openedUnit = this.OpenedUnit;
				if (openedUnit != null)
				{
					while (openedUnit.OpenedUnit != null)
					{
						openedUnit = openedUnit.OpenedUnit;
					}
				}
				return openedUnit;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x060012AA RID: 4778 RVA: 0x0014B7A1 File Offset: 0x0014A7A1
		protected List<IUndoUnit> UndoStack
		{
			get
			{
				return this._undoStack;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x060012AB RID: 4779 RVA: 0x0014B7A9 File Offset: 0x0014A7A9
		protected Stack RedoStack
		{
			get
			{
				return this._redoStack;
			}
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x0014B7B4 File Offset: 0x0014A7B4
		private void DoClear()
		{
			Invariant.Assert(!this._imeSupportModeEnabled);
			if (this.UndoStack.Count > 0)
			{
				this.UndoStack.Clear();
				this.UndoStack.TrimExcess();
			}
			if (this.RedoStack.Count > 0)
			{
				this.RedoStack.Clear();
			}
			this.SetLastUnit(null);
			this.SetOpenedUnit(null);
			this._topUndoIndex = -1;
			this._bottomUndoIndex = 0;
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x0014B828 File Offset: 0x0014A828
		private IUndoUnit PopUndoStack()
		{
			int num = this.UndoCount - 1;
			IUndoUnit result = this.UndoStack[this._topUndoIndex];
			List<IUndoUnit> undoStack = this.UndoStack;
			int topUndoIndex = this._topUndoIndex;
			this._topUndoIndex = topUndoIndex - 1;
			undoStack[topUndoIndex] = null;
			if (this._topUndoIndex < 0 && num > 0)
			{
				Invariant.Assert(this.UndoLimit > 0);
				this._topUndoIndex = this.UndoLimit - 1;
			}
			return result;
		}

		// Token: 0x04000B3A RID: 2874
		private static readonly DependencyProperty UndoManagerInstanceProperty = DependencyProperty.RegisterAttached("UndoManagerInstance", typeof(UndoManager), typeof(UndoManager), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x04000B3B RID: 2875
		private DependencyObject _scope;

		// Token: 0x04000B3C RID: 2876
		private IParentUndoUnit _openedUnit;

		// Token: 0x04000B3D RID: 2877
		private IUndoUnit _lastUnit;

		// Token: 0x04000B3E RID: 2878
		private List<IUndoUnit> _undoStack;

		// Token: 0x04000B3F RID: 2879
		private Stack _redoStack;

		// Token: 0x04000B40 RID: 2880
		private UndoState _state;

		// Token: 0x04000B41 RID: 2881
		private bool _isEnabled;

		// Token: 0x04000B42 RID: 2882
		private IParentUndoUnit _lastReopenedUnit;

		// Token: 0x04000B43 RID: 2883
		private int _topUndoIndex;

		// Token: 0x04000B44 RID: 2884
		private int _bottomUndoIndex;

		// Token: 0x04000B45 RID: 2885
		private int _undoLimit;

		// Token: 0x04000B46 RID: 2886
		private int _minUndoStackCount;

		// Token: 0x04000B47 RID: 2887
		private bool _imeSupportModeEnabled;

		// Token: 0x04000B48 RID: 2888
		private const int _undoLimitDefaultValue = 100;
	}
}
