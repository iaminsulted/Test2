using System;
using System.Collections;
using System.Windows;

namespace MS.Internal.Documents
{
	// Token: 0x020001E6 RID: 486
	internal class ParentUndoUnit : IParentUndoUnit, IUndoUnit
	{
		// Token: 0x06001118 RID: 4376 RVA: 0x00142761 File Offset: 0x00141761
		public ParentUndoUnit(string description)
		{
			this.Init(description);
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x00142770 File Offset: 0x00141770
		public virtual void Open(IParentUndoUnit newUnit)
		{
			if (newUnit == null)
			{
				throw new ArgumentNullException("newUnit");
			}
			IParentUndoUnit deepestOpenUnit = this.DeepestOpenUnit;
			if (deepestOpenUnit == null)
			{
				if (this.IsInParentUnitChain(newUnit))
				{
					throw new InvalidOperationException(SR.Get("UndoUnitCantBeOpenedTwice"));
				}
				this._openedUnit = newUnit;
				if (newUnit != null)
				{
					newUnit.Container = this;
					return;
				}
			}
			else
			{
				if (newUnit != null)
				{
					newUnit.Container = deepestOpenUnit;
				}
				deepestOpenUnit.Open(newUnit);
			}
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x001427D1 File Offset: 0x001417D1
		public virtual void Close(UndoCloseAction closeAction)
		{
			this.Close(this.OpenedUnit, closeAction);
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x001427E0 File Offset: 0x001417E0
		public virtual void Close(IParentUndoUnit unit, UndoCloseAction closeAction)
		{
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
				IParentUndoUnit parentUndoUnit = this;
				while (parentUndoUnit.OpenedUnit != null && parentUndoUnit.OpenedUnit != unit)
				{
					parentUndoUnit = parentUndoUnit.OpenedUnit;
				}
				if (parentUndoUnit.OpenedUnit == null)
				{
					throw new ArgumentException(SR.Get("UndoUnitNotFound"), "unit");
				}
				if (parentUndoUnit != this)
				{
					parentUndoUnit.Close(closeAction);
					return;
				}
			}
			UndoManager undoManager = this.TopContainer as UndoManager;
			if (closeAction != UndoCloseAction.Commit)
			{
				if (undoManager != null)
				{
					undoManager.IsEnabled = false;
				}
				if (this.OpenedUnit.OpenedUnit != null)
				{
					this.OpenedUnit.Close(closeAction);
				}
				if (closeAction == UndoCloseAction.Rollback)
				{
					this.OpenedUnit.Do();
				}
				this._openedUnit = null;
				if (this.TopContainer is UndoManager)
				{
					((UndoManager)this.TopContainer).OnNextDiscard();
				}
				else
				{
					((IParentUndoUnit)this.TopContainer).OnNextDiscard();
				}
				if (undoManager != null)
				{
					undoManager.IsEnabled = true;
					return;
				}
			}
			else
			{
				if (this.OpenedUnit.OpenedUnit != null)
				{
					this.OpenedUnit.Close(UndoCloseAction.Commit);
				}
				IParentUndoUnit openedUnit = this.OpenedUnit;
				this._openedUnit = null;
				this.Add(openedUnit);
				this.SetLastUnit(openedUnit);
			}
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x00142918 File Offset: 0x00141918
		public virtual void Add(IUndoUnit unit)
		{
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
			if (this.IsInParentUnitChain(unit))
			{
				throw new InvalidOperationException(SR.Get("UndoUnitCantBeAddedTwice"));
			}
			if (this.Locked)
			{
				throw new InvalidOperationException(SR.Get("UndoUnitLocked"));
			}
			if (!this.Merge(unit))
			{
				this._units.Push(unit);
				if (this.LastUnit is IParentUndoUnit)
				{
					((IParentUndoUnit)this.LastUnit).OnNextAdd();
				}
				this.SetLastUnit(unit);
			}
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x001429AF File Offset: 0x001419AF
		public virtual void Clear()
		{
			if (this.Locked)
			{
				throw new InvalidOperationException(SR.Get("UndoUnitLocked"));
			}
			this._units.Clear();
			this.SetOpenedUnit(null);
			this.SetLastUnit(null);
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x001429E4 File Offset: 0x001419E4
		public virtual void OnNextAdd()
		{
			this._locked = true;
			foreach (object obj in this._units)
			{
				IUndoUnit undoUnit = (IUndoUnit)obj;
				if (undoUnit is IParentUndoUnit)
				{
					((IParentUndoUnit)undoUnit).OnNextAdd();
				}
			}
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x00142A50 File Offset: 0x00141A50
		public virtual void OnNextDiscard()
		{
			this._locked = false;
			IParentUndoUnit parentUndoUnit = this;
			foreach (object obj in this._units)
			{
				IUndoUnit undoUnit = (IUndoUnit)obj;
				if (undoUnit is IParentUndoUnit)
				{
					parentUndoUnit = (undoUnit as IParentUndoUnit);
				}
			}
			if (parentUndoUnit != this)
			{
				parentUndoUnit.OnNextDiscard();
			}
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x00142AC4 File Offset: 0x00141AC4
		public virtual void Do()
		{
			IParentUndoUnit unit = this.CreateParentUndoUnitForSelf();
			UndoManager undoManager = this.TopContainer as UndoManager;
			if (undoManager != null && undoManager.IsEnabled)
			{
				undoManager.Open(unit);
			}
			while (this._units.Count > 0)
			{
				(this._units.Pop() as IUndoUnit).Do();
			}
			if (undoManager != null && undoManager.IsEnabled)
			{
				undoManager.Close(unit, UndoCloseAction.Commit);
			}
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x00142B2E File Offset: 0x00141B2E
		public virtual bool Merge(IUndoUnit unit)
		{
			Invariant.Assert(unit != null);
			return false;
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06001122 RID: 4386 RVA: 0x00142B3A File Offset: 0x00141B3A
		// (set) Token: 0x06001123 RID: 4387 RVA: 0x00142B42 File Offset: 0x00141B42
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				this._description = value;
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06001124 RID: 4388 RVA: 0x00142B55 File Offset: 0x00141B55
		public IParentUndoUnit OpenedUnit
		{
			get
			{
				return this._openedUnit;
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06001125 RID: 4389 RVA: 0x00142B5D File Offset: 0x00141B5D
		public IUndoUnit LastUnit
		{
			get
			{
				return this._lastUnit;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06001126 RID: 4390 RVA: 0x00142B65 File Offset: 0x00141B65
		// (set) Token: 0x06001127 RID: 4391 RVA: 0x00142B6D File Offset: 0x00141B6D
		public virtual bool Locked
		{
			get
			{
				return this._locked;
			}
			protected set
			{
				this._locked = value;
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06001128 RID: 4392 RVA: 0x00142B76 File Offset: 0x00141B76
		// (set) Token: 0x06001129 RID: 4393 RVA: 0x00142B7E File Offset: 0x00141B7E
		public object Container
		{
			get
			{
				return this._container;
			}
			set
			{
				if (!(value is IParentUndoUnit) && !(value is UndoManager))
				{
					throw new Exception(SR.Get("UndoContainerTypeMismatch"));
				}
				this._container = value;
			}
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x00142BA7 File Offset: 0x00141BA7
		protected void Init(string description)
		{
			if (description == null)
			{
				description = string.Empty;
			}
			this._description = description;
			this._locked = false;
			this._openedUnit = null;
			this._units = new Stack(2);
			this._container = null;
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x00142BDB File Offset: 0x00141BDB
		protected void SetOpenedUnit(IParentUndoUnit value)
		{
			this._openedUnit = value;
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x00142BE4 File Offset: 0x00141BE4
		protected void SetLastUnit(IUndoUnit value)
		{
			this._lastUnit = value;
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x00142BED File Offset: 0x00141BED
		protected virtual IParentUndoUnit CreateParentUndoUnitForSelf()
		{
			return new ParentUndoUnit(this.Description);
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x0600112E RID: 4398 RVA: 0x00142BFC File Offset: 0x00141BFC
		protected IParentUndoUnit DeepestOpenUnit
		{
			get
			{
				IParentUndoUnit openedUnit = this._openedUnit;
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

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x0600112F RID: 4399 RVA: 0x00142C28 File Offset: 0x00141C28
		protected object TopContainer
		{
			get
			{
				object obj = this;
				while (obj is IParentUndoUnit && ((IParentUndoUnit)obj).Container != null)
				{
					obj = ((IParentUndoUnit)obj).Container;
				}
				return obj;
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06001130 RID: 4400 RVA: 0x00142C5B File Offset: 0x00141C5B
		protected Stack Units
		{
			get
			{
				return this._units;
			}
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x00142C64 File Offset: 0x00141C64
		private bool IsInParentUnitChain(IUndoUnit unit)
		{
			if (unit is IParentUndoUnit)
			{
				IParentUndoUnit parentUndoUnit = this;
				while (parentUndoUnit != unit)
				{
					parentUndoUnit = (parentUndoUnit.Container as IParentUndoUnit);
					if (parentUndoUnit == null)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x04000AF2 RID: 2802
		private string _description;

		// Token: 0x04000AF3 RID: 2803
		private bool _locked;

		// Token: 0x04000AF4 RID: 2804
		private IParentUndoUnit _openedUnit;

		// Token: 0x04000AF5 RID: 2805
		private IUndoUnit _lastUnit;

		// Token: 0x04000AF6 RID: 2806
		private Stack _units;

		// Token: 0x04000AF7 RID: 2807
		private object _container;
	}
}
