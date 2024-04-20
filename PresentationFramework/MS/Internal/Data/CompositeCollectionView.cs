using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MS.Internal.Hashing.PresentationFramework;
using MS.Internal.Utility;

namespace MS.Internal.Data
{
	// Token: 0x02000210 RID: 528
	internal sealed class CompositeCollectionView : CollectionView
	{
		// Token: 0x060013E8 RID: 5096 RVA: 0x0014F254 File Offset: 0x0014E254
		internal CompositeCollectionView(CompositeCollection collection) : base(collection, -1)
		{
			this._collection = collection;
			this._collection.ContainedCollectionChanged += this.OnContainedCollectionChanged;
			int num = this.PrivateIsEmpty ? -1 : 0;
			int count = this.PrivateIsEmpty ? 0 : 1;
			base.SetCurrent(this.GetItem(num, out this._currentPositionX, out this._currentPositionY), num, count);
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x060013E9 RID: 5097 RVA: 0x0014F2CA File Offset: 0x0014E2CA
		public override int Count
		{
			get
			{
				if (this._count == -1)
				{
					this._count = this.CountDeep(this._collection.Count);
				}
				return this._count;
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x060013EA RID: 5098 RVA: 0x0014F2F2 File Offset: 0x0014E2F2
		public override bool IsEmpty
		{
			get
			{
				return this.PrivateIsEmpty;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x060013EB RID: 5099 RVA: 0x0014F2FC File Offset: 0x0014E2FC
		private bool PrivateIsEmpty
		{
			get
			{
				if (this._count < 0)
				{
					for (int i = 0; i < this._collection.Count; i++)
					{
						CollectionContainer collectionContainer = this._collection[i] as CollectionContainer;
						if (collectionContainer == null || collectionContainer.ViewCount != 0)
						{
							return false;
						}
					}
					this.CacheCount(0);
				}
				return this._count == 0;
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x060013EC RID: 5100 RVA: 0x0014F357 File Offset: 0x0014E357
		public override bool IsCurrentAfterLast
		{
			get
			{
				return this.IsEmpty || this._currentPositionX >= this._collection.Count;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x060013ED RID: 5101 RVA: 0x0014F379 File Offset: 0x0014E379
		public override bool IsCurrentBeforeFirst
		{
			get
			{
				return this.IsEmpty || this._currentPositionX < 0;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x060013EE RID: 5102 RVA: 0x00105F35 File Offset: 0x00104F35
		public override bool CanFilter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x0014F38E File Offset: 0x0014E38E
		public override bool Contains(object item)
		{
			return this.FindItem(item, false) >= 0;
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x0014F39E File Offset: 0x0014E39E
		public override int IndexOf(object item)
		{
			return this.FindItem(item, false);
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x0014F3A8 File Offset: 0x0014E3A8
		public override object GetItemAt(int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int num;
			int num2;
			object item = this.GetItem(index, out num, out num2);
			if (item == CompositeCollectionView.s_afterLast)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return item;
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x0014F3E6 File Offset: 0x0014E3E6
		public override bool MoveCurrentTo(object item)
		{
			if (ItemsControl.EqualsEx(this.CurrentItem, item) && (item != null || this.IsCurrentInView))
			{
				return this.IsCurrentInView;
			}
			if (!this.IsEmpty)
			{
				this.FindItem(item, true);
			}
			return this.IsCurrentInView;
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x0014F41F File Offset: 0x0014E41F
		public override bool MoveCurrentToFirst()
		{
			return !this.IsEmpty && this._MoveTo(0);
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x0014F434 File Offset: 0x0014E434
		public override bool MoveCurrentToLast()
		{
			bool isCurrentAfterLast = this.IsCurrentAfterLast;
			bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
			int num = this.Count - 1;
			int currentPositionX;
			int currentPositionY;
			object lastItem = this.GetLastItem(out currentPositionX, out currentPositionY);
			if ((this.CurrentPosition != num || this.CurrentItem != lastItem) && base.OKToChangeCurrent())
			{
				this._currentPositionX = currentPositionX;
				this._currentPositionY = currentPositionY;
				base.SetCurrent(lastItem, num);
				this.OnCurrentChanged();
				if (this.IsCurrentAfterLast != isCurrentAfterLast)
				{
					this.OnPropertyChanged("IsCurrentAfterLast");
				}
				if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
				{
					this.OnPropertyChanged("IsCurrentBeforeFirst");
				}
				this.OnPropertyChanged("CurrentPosition");
				this.OnPropertyChanged("CurrentItem");
			}
			return this.IsCurrentInView;
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x0014F4E3 File Offset: 0x0014E4E3
		public override bool MoveCurrentToNext()
		{
			return !this.IsCurrentAfterLast && this._MoveTo(this.CurrentPosition + 1);
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x0014F4FD File Offset: 0x0014E4FD
		public override bool MoveCurrentToPrevious()
		{
			return !this.IsCurrentBeforeFirst && this._MoveTo(this.CurrentPosition - 1);
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x0014F518 File Offset: 0x0014E518
		public override bool MoveCurrentToPosition(int position)
		{
			if (position < -1)
			{
				throw new ArgumentOutOfRangeException("position");
			}
			int currentPositionX;
			int currentPositionY;
			object obj = this.GetItem(position, out currentPositionX, out currentPositionY);
			if (position != this.CurrentPosition || obj != this.CurrentItem)
			{
				if (obj == CompositeCollectionView.s_afterLast)
				{
					obj = null;
					if (position > this.Count)
					{
						throw new ArgumentOutOfRangeException("position");
					}
				}
				if (base.OKToChangeCurrent())
				{
					bool isCurrentAfterLast = this.IsCurrentAfterLast;
					bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
					this._currentPositionX = currentPositionX;
					this._currentPositionY = currentPositionY;
					base.SetCurrent(obj, position);
					this.OnCurrentChanged();
					if (this.IsCurrentAfterLast != isCurrentAfterLast)
					{
						this.OnPropertyChanged("IsCurrentAfterLast");
					}
					if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
					{
						this.OnPropertyChanged("IsCurrentBeforeFirst");
					}
					this.OnPropertyChanged("CurrentPosition");
					this.OnPropertyChanged("CurrentItem");
				}
			}
			return this.IsCurrentInView;
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x0014F5EB File Offset: 0x0014E5EB
		protected override void RefreshOverride()
		{
			this._version++;
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x0014F607 File Offset: 0x0014E607
		protected override IEnumerator GetEnumerator()
		{
			return new CompositeCollectionView.FlatteningEnumerator(this._collection, this);
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x0014F618 File Offset: 0x0014E618
		protected override void ProcessCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			this.ValidateCollectionChangedEventArgs(args);
			bool flag = false;
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Add:
			case NotifyCollectionChangedAction.Remove:
			{
				object obj;
				int num;
				if (args.Action == NotifyCollectionChangedAction.Add)
				{
					obj = args.NewItems[0];
					num = args.NewStartingIndex;
				}
				else
				{
					obj = args.OldItems[0];
					num = args.OldStartingIndex;
				}
				int num2 = num;
				if (this._traceLog != null)
				{
					this._traceLog.Add("ProcessCollectionChanged  action = {0}  item = {1}", new object[]
					{
						args.Action,
						TraceLog.IdFor(obj)
					});
				}
				CollectionContainer collectionContainer = obj as CollectionContainer;
				if (collectionContainer == null)
				{
					for (int i = num2 - 1; i >= 0; i--)
					{
						collectionContainer = (this._collection[i] as CollectionContainer);
						if (collectionContainer != null)
						{
							num2 += collectionContainer.ViewCount - 1;
						}
					}
					if (args.Action == NotifyCollectionChangedAction.Add)
					{
						if (this._count >= 0)
						{
							this._count++;
						}
						this.UpdateCurrencyAfterAdd(num2, args.NewStartingIndex, true);
					}
					else if (args.Action == NotifyCollectionChangedAction.Remove)
					{
						if (this._count >= 0)
						{
							this._count--;
						}
						this.UpdateCurrencyAfterRemove(num2, args.OldStartingIndex, true);
					}
					args = new NotifyCollectionChangedEventArgs(args.Action, obj, num2);
				}
				else
				{
					if (args.Action == NotifyCollectionChangedAction.Add)
					{
						if (this._count >= 0)
						{
							this._count += collectionContainer.ViewCount;
						}
					}
					else if (this._count >= 0)
					{
						this._count -= collectionContainer.ViewCount;
					}
					if (num <= this._currentPositionX)
					{
						if (args.Action == NotifyCollectionChangedAction.Add)
						{
							this._currentPositionX++;
							this.SetCurrentPositionFromXY(this._currentPositionX, this._currentPositionY);
						}
						else
						{
							Invariant.Assert(args.Action == NotifyCollectionChangedAction.Remove);
							if (num == this._currentPositionX)
							{
								flag = true;
							}
							else
							{
								this._currentPositionX--;
								this.SetCurrentPositionFromXY(this._currentPositionX, this._currentPositionY);
							}
						}
					}
					args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
				}
				break;
			}
			case NotifyCollectionChangedAction.Replace:
			{
				CollectionContainer collectionContainer2 = args.NewItems[0] as CollectionContainer;
				CollectionContainer collectionContainer3 = args.OldItems[0] as CollectionContainer;
				int num3 = args.OldStartingIndex;
				if (collectionContainer2 == null && collectionContainer3 == null)
				{
					for (int j = num3 - 1; j >= 0; j--)
					{
						CollectionContainer collectionContainer4 = this._collection[j] as CollectionContainer;
						if (collectionContainer4 != null)
						{
							num3 += collectionContainer4.ViewCount - 1;
						}
					}
					if (num3 == this.CurrentPosition)
					{
						flag = true;
					}
					args = new NotifyCollectionChangedEventArgs(args.Action, args.NewItems, args.OldItems, num3);
				}
				else
				{
					if (this._count >= 0)
					{
						this._count -= ((collectionContainer3 == null) ? 1 : collectionContainer3.ViewCount);
						this._count += ((collectionContainer2 == null) ? 1 : collectionContainer2.ViewCount);
					}
					if (num3 < this._currentPositionX)
					{
						this.SetCurrentPositionFromXY(this._currentPositionX, this._currentPositionY);
					}
					else if (num3 == this._currentPositionX)
					{
						flag = true;
					}
					args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
				}
				break;
			}
			case NotifyCollectionChangedAction.Move:
			{
				bool flag2 = args.OldItems[0] is CollectionContainer;
				int num4 = args.OldStartingIndex;
				int num5 = args.NewStartingIndex;
				if (!flag2)
				{
					for (int k = num4 - 1; k >= 0; k--)
					{
						CollectionContainer collectionContainer5 = this._collection[k] as CollectionContainer;
						if (collectionContainer5 != null)
						{
							num4 += collectionContainer5.ViewCount - 1;
						}
					}
					for (int l = num5 - 1; l >= 0; l--)
					{
						CollectionContainer collectionContainer6 = this._collection[l] as CollectionContainer;
						if (collectionContainer6 != null)
						{
							num5 += collectionContainer6.ViewCount - 1;
						}
					}
					if (num4 == this.CurrentPosition)
					{
						flag = true;
					}
					else if (num5 <= this.CurrentPosition && num4 > this.CurrentPosition)
					{
						this.UpdateCurrencyAfterAdd(num5, args.NewStartingIndex, true);
					}
					else if (num4 < this.CurrentPosition && num5 >= this.CurrentPosition)
					{
						this.UpdateCurrencyAfterRemove(num4, args.OldStartingIndex, true);
					}
					args = new NotifyCollectionChangedEventArgs(args.Action, args.OldItems, num5, num4);
				}
				else
				{
					if (num4 == this._currentPositionX)
					{
						flag = true;
					}
					else if (num5 <= this._currentPositionX && num4 > this._currentPositionX)
					{
						this._currentPositionX++;
						this.SetCurrentPositionFromXY(this._currentPositionX, this._currentPositionY);
					}
					else if (num4 < this._currentPositionX && num5 >= this._currentPositionX)
					{
						this._currentPositionX--;
						this.SetCurrentPositionFromXY(this._currentPositionX, this._currentPositionY);
					}
					args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
				}
				break;
			}
			case NotifyCollectionChangedAction.Reset:
				if (this._traceLog != null)
				{
					this._traceLog.Add("ProcessCollectionChanged  action = {0}", new object[]
					{
						args.Action
					});
				}
				if (this._collection.Count != 0)
				{
					throw new InvalidOperationException(SR.Get("CompositeCollectionResetOnlyOnClear"));
				}
				this._count = 0;
				if (this._currentPositionX >= 0)
				{
					base.OnCurrentChanging();
					this.SetCurrentBeforeFirst();
					this.OnCurrentChanged();
					this.OnPropertyChanged("IsCurrentBeforeFirst");
					this.OnPropertyChanged("CurrentPosition");
					this.OnPropertyChanged("CurrentItem");
				}
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					args.Action
				}));
			}
			this._version++;
			this.OnCollectionChanged(args);
			if (flag)
			{
				this._currentPositionY = 0;
				this.MoveCurrencyOffDeletedElement();
			}
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x0014FBB4 File Offset: 0x0014EBB4
		internal void OnContainedCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			this.ValidateCollectionChangedEventArgs(args);
			this._count = -1;
			int num = args.OldStartingIndex;
			int num2 = args.NewStartingIndex;
			int num3 = 0;
			int i;
			for (i = 0; i < this._collection.Count; i++)
			{
				CollectionContainer collectionContainer = this._collection[i] as CollectionContainer;
				if (collectionContainer != null)
				{
					if (sender == collectionContainer)
					{
						break;
					}
					num3 += collectionContainer.ViewCount;
				}
				else
				{
					num3++;
				}
			}
			if (args.OldStartingIndex >= 0)
			{
				num += num3;
			}
			if (args.NewStartingIndex >= 0)
			{
				num2 += num3;
			}
			if (i >= this._collection.Count)
			{
				if (this._traceLog != null)
				{
					this._traceLog.Add("Received ContainerCollectionChange from unknown sender {0}  action = {1} old item = {2}, new item = {3}", new object[]
					{
						TraceLog.IdFor(sender),
						args.Action,
						TraceLog.IdFor(args.OldItems[0]),
						TraceLog.IdFor(args.NewItems[0])
					});
					this._traceLog.Add("Unhook CollectionChanged event handler from unknown sender.", Array.Empty<object>());
				}
				this.CacheCount(num3);
				return;
			}
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Add:
				this.TraceContainerCollectionChange(sender, args.Action, null, args.NewItems[0]);
				if (num2 < 0)
				{
					num2 = this.DeduceFlatIndexForAdd((CollectionContainer)sender, i);
				}
				this.UpdateCurrencyAfterAdd(num2, i, false);
				args = new NotifyCollectionChangedEventArgs(args.Action, args.NewItems[0], num2);
				break;
			case NotifyCollectionChangedAction.Remove:
				this.TraceContainerCollectionChange(sender, args.Action, args.OldItems[0], null);
				if (num < 0)
				{
					num = this.DeduceFlatIndexForRemove((CollectionContainer)sender, i, args.OldItems[0]);
				}
				this.UpdateCurrencyAfterRemove(num, i, false);
				args = new NotifyCollectionChangedEventArgs(args.Action, args.OldItems[0], num);
				break;
			case NotifyCollectionChangedAction.Replace:
				this.TraceContainerCollectionChange(sender, args.Action, args.OldItems[0], args.NewItems[0]);
				if (num == this.CurrentPosition)
				{
					this.MoveCurrencyOffDeletedElement();
				}
				args = new NotifyCollectionChangedEventArgs(args.Action, args.NewItems[0], args.OldItems[0], num);
				break;
			case NotifyCollectionChangedAction.Move:
				this.TraceContainerCollectionChange(sender, args.Action, args.OldItems[0], args.NewItems[0]);
				if (num < 0)
				{
					num = this.DeduceFlatIndexForRemove((CollectionContainer)sender, i, args.NewItems[0]);
				}
				if (num2 < 0)
				{
					num2 = this.DeduceFlatIndexForAdd((CollectionContainer)sender, i);
				}
				this.UpdateCurrencyAfterMove(num, num2, i, false);
				args = new NotifyCollectionChangedEventArgs(args.Action, args.OldItems[0], num2, num);
				break;
			case NotifyCollectionChangedAction.Reset:
				if (this._traceLog != null)
				{
					this._traceLog.Add("ContainerCollectionChange from {0}  action = {1}", new object[]
					{
						TraceLog.IdFor(sender),
						args.Action
					});
				}
				this.UpdateCurrencyAfterRefresh(sender);
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					args.Action
				}));
			}
			this._version++;
			this.OnCollectionChanged(args);
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x0014FEF4 File Offset: 0x0014EEF4
		internal override bool HasReliableHashCodes()
		{
			int i = 0;
			int count = this._collection.Count;
			while (i < count)
			{
				CollectionContainer collectionContainer = this._collection[i] as CollectionContainer;
				if (collectionContainer != null)
				{
					CollectionView collectionView = collectionContainer.View as CollectionView;
					if (collectionView != null && !collectionView.HasReliableHashCodes())
					{
						return false;
					}
				}
				else if (!HashHelper.HasReliableHashCode(this._collection[i]))
				{
					return false;
				}
				i++;
			}
			return true;
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x0014FF5D File Offset: 0x0014EF5D
		internal override void GetCollectionChangedSources(int level, Action<int, object, bool?, List<string>> format, List<string> sources)
		{
			format(level, this, new bool?(false), sources);
			if (this._collection != null)
			{
				this._collection.GetCollectionChangedSources(level + 1, format, sources);
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x060013FE RID: 5118 RVA: 0x0014FF86 File Offset: 0x0014EF86
		private bool IsCurrentInView
		{
			get
			{
				return 0 <= this._currentPositionX && this._currentPositionX < this._collection.Count;
			}
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x0014FFA8 File Offset: 0x0014EFA8
		private int FindItem(object item, bool changeCurrent)
		{
			int i = 0;
			int num = 0;
			int num2 = 0;
			while (i < this._collection.Count)
			{
				CollectionContainer collectionContainer = this._collection[i] as CollectionContainer;
				if (collectionContainer == null)
				{
					if (ItemsControl.EqualsEx(this._collection[i], item))
					{
						break;
					}
					num2++;
				}
				else
				{
					num = collectionContainer.ViewIndexOf(item);
					if (num >= 0)
					{
						num2 += num;
						break;
					}
					num = 0;
					num2 += collectionContainer.ViewCount;
				}
				i++;
			}
			if (i >= this._collection.Count)
			{
				this.CacheCount(num2);
				num2 = -1;
				item = null;
				i = -1;
				num = 0;
			}
			if (changeCurrent && this.CurrentPosition != num2 && base.OKToChangeCurrent())
			{
				object currentItem = this.CurrentItem;
				int currentPosition = this.CurrentPosition;
				bool isCurrentAfterLast = this.IsCurrentAfterLast;
				bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
				base.SetCurrent(item, num2);
				this._currentPositionX = i;
				this._currentPositionY = num;
				this.OnCurrentChanged();
				if (this.IsCurrentAfterLast != isCurrentAfterLast)
				{
					this.OnPropertyChanged("IsCurrentAfterLast");
				}
				if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
				{
					this.OnPropertyChanged("IsCurrentBeforeFirst");
				}
				if (currentPosition != this.CurrentPosition)
				{
					this.OnPropertyChanged("CurrentPosition");
				}
				if (currentItem != this.CurrentItem)
				{
					this.OnPropertyChanged("CurrentItem");
				}
			}
			return num2;
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x001500E0 File Offset: 0x0014F0E0
		private object GetItem(int flatIndex, out int positionX, out int positionY)
		{
			positionY = 0;
			if (flatIndex == -1)
			{
				positionX = -1;
				return null;
			}
			if (this._count >= 0 && flatIndex >= this._count)
			{
				positionX = this._collection.Count;
				return CompositeCollectionView.s_afterLast;
			}
			int num = 0;
			for (int i = 0; i < this._collection.Count; i++)
			{
				CollectionContainer collectionContainer = this._collection[i] as CollectionContainer;
				if (collectionContainer == null)
				{
					if (num == flatIndex)
					{
						positionX = i;
						return this._collection[i];
					}
					num++;
				}
				else if (collectionContainer.Collection != null)
				{
					int num2 = flatIndex - num;
					int viewCount = collectionContainer.ViewCount;
					if (num2 < viewCount)
					{
						positionX = i;
						positionY = num2;
						return collectionContainer.ViewItem(num2);
					}
					num += viewCount;
				}
			}
			this.CacheCount(num);
			positionX = this._collection.Count;
			return CompositeCollectionView.s_afterLast;
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x001501AC File Offset: 0x0014F1AC
		private object GetNextItemFromXY(int positionX, int positionY)
		{
			Invariant.Assert(positionY >= 0);
			object result = null;
			while (positionX < this._collection.Count)
			{
				CollectionContainer collectionContainer = this._collection[positionX] as CollectionContainer;
				if (collectionContainer == null)
				{
					result = this._collection[positionX];
					positionY = 0;
					break;
				}
				if (positionY < collectionContainer.ViewCount)
				{
					result = collectionContainer.ViewItem(positionY);
					break;
				}
				positionY = 0;
				positionX++;
			}
			if (positionX < this._collection.Count)
			{
				this._currentPositionX = positionX;
				this._currentPositionY = positionY;
			}
			else
			{
				this._currentPositionX = this._collection.Count;
				this._currentPositionY = 0;
			}
			return result;
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x00150250 File Offset: 0x0014F250
		private int CountDeep(int end)
		{
			if (Invariant.Strict)
			{
				Invariant.Assert(end <= this._collection.Count);
			}
			int num = 0;
			for (int i = 0; i < end; i++)
			{
				CollectionContainer collectionContainer = this._collection[i] as CollectionContainer;
				if (collectionContainer == null)
				{
					num++;
				}
				else
				{
					num += collectionContainer.ViewCount;
				}
			}
			return num;
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x001502AD File Offset: 0x0014F2AD
		private void CacheCount(int count)
		{
			bool flag = this._count != count && this._count >= 0;
			this._count = count;
			if (flag)
			{
				this.OnPropertyChanged("Count");
			}
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x001502DC File Offset: 0x0014F2DC
		private bool _MoveTo(int proposed)
		{
			int currentPositionX;
			int currentPositionY;
			object item = this.GetItem(proposed, out currentPositionX, out currentPositionY);
			if (proposed != this.CurrentPosition || item != this.CurrentItem)
			{
				Invariant.Assert(this._count < 0 || proposed <= this._count);
				if (base.OKToChangeCurrent())
				{
					object currentItem = this.CurrentItem;
					int currentPosition = this.CurrentPosition;
					bool isCurrentAfterLast = this.IsCurrentAfterLast;
					bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
					this._currentPositionX = currentPositionX;
					this._currentPositionY = currentPositionY;
					if (item == CompositeCollectionView.s_afterLast)
					{
						base.SetCurrent(null, this.Count);
					}
					else
					{
						base.SetCurrent(item, proposed);
					}
					this.OnCurrentChanged();
					if (this.IsCurrentAfterLast != isCurrentAfterLast)
					{
						this.OnPropertyChanged("IsCurrentAfterLast");
					}
					if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
					{
						this.OnPropertyChanged("IsCurrentBeforeFirst");
					}
					if (currentPosition != this.CurrentPosition)
					{
						this.OnPropertyChanged("CurrentPosition");
					}
					if (currentItem != this.CurrentItem)
					{
						this.OnPropertyChanged("CurrentItem");
					}
				}
			}
			return this.IsCurrentInView;
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x001503D8 File Offset: 0x0014F3D8
		private int DeduceFlatIndexForAdd(CollectionContainer sender, int x)
		{
			int result;
			if (this._currentPositionX > x)
			{
				result = 0;
			}
			else if (this._currentPositionX < x)
			{
				result = this.CurrentPosition + 1;
			}
			else
			{
				object o = sender.ViewItem(this._currentPositionY);
				if (ItemsControl.EqualsEx(this.CurrentItem, o))
				{
					result = this.CurrentPosition + 1;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x00150430 File Offset: 0x0014F430
		private int DeduceFlatIndexForRemove(CollectionContainer sender, int x, object item)
		{
			int result;
			if (this._currentPositionX > x)
			{
				result = 0;
			}
			else if (this._currentPositionX < x)
			{
				result = this.CurrentPosition + 1;
			}
			else if (ItemsControl.EqualsEx(item, this.CurrentItem))
			{
				result = this.CurrentPosition;
			}
			else
			{
				object o = sender.ViewItem(this._currentPositionY);
				if (ItemsControl.EqualsEx(item, o))
				{
					result = this.CurrentPosition + 1;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x0015049C File Offset: 0x0014F49C
		private void UpdateCurrencyAfterAdd(int flatIndex, int positionX, bool isCompositeItem)
		{
			if (flatIndex < 0)
			{
				return;
			}
			if (flatIndex <= this.CurrentPosition)
			{
				int newPosition = this.CurrentPosition + 1;
				if (isCompositeItem)
				{
					this._currentPositionX++;
				}
				else if (positionX == this._currentPositionX)
				{
					this._currentPositionY++;
				}
				base.SetCurrent(this.GetNextItemFromXY(this._currentPositionX, this._currentPositionY), newPosition);
			}
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x00150504 File Offset: 0x0014F504
		private void UpdateCurrencyAfterRemove(int flatIndex, int positionX, bool isCompositeItem)
		{
			if (flatIndex < 0)
			{
				return;
			}
			if (flatIndex < this.CurrentPosition)
			{
				base.SetCurrent(this.CurrentItem, this.CurrentPosition - 1);
				if (isCompositeItem)
				{
					this._currentPositionX--;
					return;
				}
				if (positionX == this._currentPositionX)
				{
					this._currentPositionY--;
					return;
				}
			}
			else if (flatIndex == this.CurrentPosition)
			{
				this.MoveCurrencyOffDeletedElement();
			}
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0015056C File Offset: 0x0014F56C
		private void UpdateCurrencyAfterMove(int oldIndex, int newIndex, int positionX, bool isCompositeItem)
		{
			if ((oldIndex < this.CurrentPosition && newIndex < this.CurrentPosition) || (oldIndex > this.CurrentPosition && newIndex > this.CurrentPosition))
			{
				return;
			}
			if (newIndex <= this.CurrentPosition)
			{
				this.UpdateCurrencyAfterAdd(newIndex, positionX, isCompositeItem);
			}
			if (oldIndex <= this.CurrentPosition)
			{
				this.UpdateCurrencyAfterRemove(oldIndex, positionX, isCompositeItem);
			}
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x001505C4 File Offset: 0x0014F5C4
		private void UpdateCurrencyAfterRefresh(object refreshedObject)
		{
			Invariant.Assert(refreshedObject is CollectionContainer);
			object currentItem = this.CurrentItem;
			int currentPosition = this.CurrentPosition;
			bool isCurrentAfterLast = this.IsCurrentAfterLast;
			bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
			if (this.IsCurrentInView && refreshedObject == this._collection[this._currentPositionX])
			{
				CollectionContainer collectionContainer = refreshedObject as CollectionContainer;
				if (collectionContainer.ViewCount == 0)
				{
					this._currentPositionY = 0;
					this.MoveCurrencyOffDeletedElement();
				}
				else
				{
					int num = collectionContainer.ViewIndexOf(this.CurrentItem);
					if (num >= 0)
					{
						this._currentPositionY = num;
						this.SetCurrentPositionFromXY(this._currentPositionX, this._currentPositionY);
					}
					else
					{
						base.OnCurrentChanging();
						this.SetCurrentBeforeFirst();
						this.OnCurrentChanged();
					}
				}
			}
			else
			{
				for (int i = 0; i < this._currentPositionX; i++)
				{
					if (this._collection[i] == refreshedObject)
					{
						this.SetCurrentPositionFromXY(this._currentPositionX, this._currentPositionY);
						break;
					}
				}
			}
			if (this.IsCurrentAfterLast != isCurrentAfterLast)
			{
				this.OnPropertyChanged("IsCurrentAfterLast");
			}
			if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
			{
				this.OnPropertyChanged("IsCurrentBeforeFirst");
			}
			if (currentPosition != this.CurrentPosition)
			{
				this.OnPropertyChanged("CurrentPosition");
			}
			if (currentItem != this.CurrentItem)
			{
				this.OnPropertyChanged("CurrentItem");
			}
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x00150704 File Offset: 0x0014F704
		private void MoveCurrencyOffDeletedElement()
		{
			int currentPosition = this.CurrentPosition;
			base.OnCurrentChanging();
			object newItem = this.GetNextItemFromXY(this._currentPositionX, this._currentPositionY);
			if (this._currentPositionX >= this._collection.Count)
			{
				newItem = this.GetLastItem(out this._currentPositionX, out this._currentPositionY);
				base.SetCurrent(newItem, this.Count - 1);
			}
			else
			{
				this.SetCurrentPositionFromXY(this._currentPositionX, this._currentPositionY);
				base.SetCurrent(newItem, this.CurrentPosition);
			}
			this.OnCurrentChanged();
			this.OnPropertyChanged("Count");
			this.OnPropertyChanged("CurrentItem");
			if (this.IsCurrentAfterLast)
			{
				this.OnPropertyChanged("IsCurrentAfterLast");
			}
			if (this.IsCurrentBeforeFirst)
			{
				this.OnPropertyChanged("IsCurrentBeforeFirst");
			}
			if (this.CurrentPosition != currentPosition)
			{
				this.OnPropertyChanged("CurrentPosition");
			}
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x001507E0 File Offset: 0x0014F7E0
		private object GetLastItem(out int positionX, out int positionY)
		{
			object result = null;
			positionX = -1;
			positionY = 0;
			if (this._count != 0)
			{
				for (positionX = this._collection.Count - 1; positionX >= 0; positionX--)
				{
					CollectionContainer collectionContainer = this._collection[positionX] as CollectionContainer;
					if (collectionContainer == null)
					{
						result = this._collection[positionX];
						break;
					}
					if (collectionContainer.ViewCount > 0)
					{
						positionY = collectionContainer.ViewCount - 1;
						result = collectionContainer.ViewItem(positionY);
						break;
					}
				}
				if (positionX < 0)
				{
					this.CacheCount(0);
				}
			}
			return result;
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x0015086A File Offset: 0x0014F86A
		private void SetCurrentBeforeFirst()
		{
			this._currentPositionX = -1;
			this._currentPositionY = 0;
			base.SetCurrent(null, -1);
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x00150882 File Offset: 0x0014F882
		private void SetCurrentPositionFromXY(int x, int y)
		{
			if (this.IsCurrentBeforeFirst)
			{
				base.SetCurrent(null, -1);
				return;
			}
			if (this.IsCurrentAfterLast)
			{
				base.SetCurrent(null, this.Count);
				return;
			}
			base.SetCurrent(this.CurrentItem, this.CountDeep(x) + y);
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x001508C0 File Offset: 0x0014F8C0
		private void InitializeTraceLog()
		{
			this._traceLog = new TraceLog(20);
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x001508D0 File Offset: 0x0014F8D0
		private void TraceContainerCollectionChange(object sender, NotifyCollectionChangedAction action, object oldItem, object newItem)
		{
			if (this._traceLog != null)
			{
				this._traceLog.Add("ContainerCollectionChange from {0}  action = {1} oldItem = {2} newItem = {3}", new object[]
				{
					TraceLog.IdFor(sender),
					action,
					TraceLog.IdFor(oldItem),
					TraceLog.IdFor(newItem)
				});
			}
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x00150920 File Offset: 0x0014F920
		private void ValidateCollectionChangedEventArgs(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (e.NewItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				if (e.OldItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				if (e.NewItems.Count != 1 || e.OldItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				break;
			case NotifyCollectionChangedAction.Move:
				if (e.NewItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				if (e.NewStartingIndex < 0)
				{
					throw new InvalidOperationException(SR.Get("CannotMoveToUnknownPosition"));
				}
				break;
			case NotifyCollectionChangedAction.Reset:
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					e.Action
				}));
			}
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x00150A1C File Offset: 0x0014FA1C
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x04000B96 RID: 2966
		private TraceLog _traceLog;

		// Token: 0x04000B97 RID: 2967
		private CompositeCollection _collection;

		// Token: 0x04000B98 RID: 2968
		private int _count = -1;

		// Token: 0x04000B99 RID: 2969
		private int _version;

		// Token: 0x04000B9A RID: 2970
		private int _currentPositionX = -1;

		// Token: 0x04000B9B RID: 2971
		private int _currentPositionY;

		// Token: 0x04000B9C RID: 2972
		private static readonly object s_afterLast = new object();

		// Token: 0x020009EC RID: 2540
		private class FlatteningEnumerator : IEnumerator, IDisposable
		{
			// Token: 0x06008447 RID: 33863 RVA: 0x00325509 File Offset: 0x00324509
			internal FlatteningEnumerator(CompositeCollection collection, CompositeCollectionView view)
			{
				Invariant.Assert(collection != null && view != null);
				this._collection = collection;
				this._view = view;
				this._version = view._version;
				this.Reset();
			}

			// Token: 0x06008448 RID: 33864 RVA: 0x00325540 File Offset: 0x00324540
			public bool MoveNext()
			{
				this.CheckVersion();
				bool result = true;
				object obj;
				for (;;)
				{
					if (this._containerEnumerator != null)
					{
						if (this._containerEnumerator.MoveNext())
						{
							break;
						}
						this.DisposeContainerEnumerator();
					}
					int num = this._index + 1;
					this._index = num;
					if (num >= this._collection.Count)
					{
						goto IL_9A;
					}
					obj = this._collection[this._index];
					CollectionContainer collectionContainer = obj as CollectionContainer;
					if (collectionContainer == null)
					{
						goto IL_91;
					}
					IEnumerable view = collectionContainer.View;
					this._containerEnumerator = ((view != null) ? view.GetEnumerator() : null);
				}
				this._current = this._containerEnumerator.Current;
				return result;
				IL_91:
				this._current = obj;
				return result;
				IL_9A:
				this._current = null;
				this._done = true;
				result = false;
				return result;
			}

			// Token: 0x17001DB9 RID: 7609
			// (get) Token: 0x06008449 RID: 33865 RVA: 0x003255F8 File Offset: 0x003245F8
			public object Current
			{
				get
				{
					if (this._index < 0)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
					}
					if (this._done)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
					}
					return this._current;
				}
			}

			// Token: 0x0600844A RID: 33866 RVA: 0x00325631 File Offset: 0x00324631
			public void Reset()
			{
				this.CheckVersion();
				this._index = -1;
				this._current = null;
				this.DisposeContainerEnumerator();
				this._done = false;
			}

			// Token: 0x0600844B RID: 33867 RVA: 0x00325654 File Offset: 0x00324654
			public void Dispose()
			{
				this.DisposeContainerEnumerator();
			}

			// Token: 0x0600844C RID: 33868 RVA: 0x0032565C File Offset: 0x0032465C
			private void DisposeContainerEnumerator()
			{
				IDisposable disposable = this._containerEnumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				this._containerEnumerator = null;
			}

			// Token: 0x0600844D RID: 33869 RVA: 0x00325688 File Offset: 0x00324688
			private void CheckVersion()
			{
				if (this._isInvalidated || (this._isInvalidated = (this._version != this._view._version)))
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
			}

			// Token: 0x04004008 RID: 16392
			private CompositeCollection _collection;

			// Token: 0x04004009 RID: 16393
			private CompositeCollectionView _view;

			// Token: 0x0400400A RID: 16394
			private int _index;

			// Token: 0x0400400B RID: 16395
			private object _current;

			// Token: 0x0400400C RID: 16396
			private IEnumerator _containerEnumerator;

			// Token: 0x0400400D RID: 16397
			private bool _done;

			// Token: 0x0400400E RID: 16398
			private bool _isInvalidated;

			// Token: 0x0400400F RID: 16399
			private int _version;
		}
	}
}
