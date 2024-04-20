using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000226 RID: 550
	internal class EnumerableCollectionView : CollectionView, IItemProperties
	{
		// Token: 0x0600148D RID: 5261 RVA: 0x001525B0 File Offset: 0x001515B0
		internal EnumerableCollectionView(IEnumerable source) : base(source, -1)
		{
			this._snapshot = new ObservableCollection<object>();
			this._pollForChanges = !(source is INotifyCollectionChanged);
			this.LoadSnapshotCore(source);
			if (this._snapshot.Count > 0)
			{
				base.SetCurrent(this._snapshot[0], 0, 1);
			}
			else
			{
				base.SetCurrent(null, -1, 0);
			}
			this._view = new ListCollectionView(this._snapshot);
			((INotifyCollectionChanged)this._view).CollectionChanged += this._OnViewChanged;
			((INotifyPropertyChanged)this._view).PropertyChanged += this._OnPropertyChanged;
			this._view.CurrentChanging += this._OnCurrentChanging;
			this._view.CurrentChanged += this._OnCurrentChanged;
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x0600148E RID: 5262 RVA: 0x00152683 File Offset: 0x00151683
		// (set) Token: 0x0600148F RID: 5263 RVA: 0x00152690 File Offset: 0x00151690
		public override CultureInfo Culture
		{
			get
			{
				return this._view.Culture;
			}
			set
			{
				this._view.Culture = value;
			}
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x0015269E File Offset: 0x0015169E
		public override bool Contains(object item)
		{
			this.EnsureSnapshot();
			return this._view.Contains(item);
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001491 RID: 5265 RVA: 0x001526B2 File Offset: 0x001516B2
		// (set) Token: 0x06001492 RID: 5266 RVA: 0x001526BF File Offset: 0x001516BF
		public override Predicate<object> Filter
		{
			get
			{
				return this._view.Filter;
			}
			set
			{
				this._view.Filter = value;
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001493 RID: 5267 RVA: 0x001526CD File Offset: 0x001516CD
		public override bool CanFilter
		{
			get
			{
				return this._view.CanFilter;
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001494 RID: 5268 RVA: 0x001526DA File Offset: 0x001516DA
		public override SortDescriptionCollection SortDescriptions
		{
			get
			{
				return this._view.SortDescriptions;
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06001495 RID: 5269 RVA: 0x001526E7 File Offset: 0x001516E7
		public override bool CanSort
		{
			get
			{
				return this._view.CanSort;
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06001496 RID: 5270 RVA: 0x001526F4 File Offset: 0x001516F4
		public override bool CanGroup
		{
			get
			{
				return this._view.CanGroup;
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06001497 RID: 5271 RVA: 0x00152701 File Offset: 0x00151701
		public override ObservableCollection<GroupDescription> GroupDescriptions
		{
			get
			{
				return this._view.GroupDescriptions;
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06001498 RID: 5272 RVA: 0x0015270E File Offset: 0x0015170E
		public override ReadOnlyObservableCollection<object> Groups
		{
			get
			{
				return this._view.Groups;
			}
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x0015271B File Offset: 0x0015171B
		public override IDisposable DeferRefresh()
		{
			return this._view.DeferRefresh();
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x0600149A RID: 5274 RVA: 0x00152728 File Offset: 0x00151728
		public override object CurrentItem
		{
			get
			{
				return this._view.CurrentItem;
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x0600149B RID: 5275 RVA: 0x00152735 File Offset: 0x00151735
		public override int CurrentPosition
		{
			get
			{
				return this._view.CurrentPosition;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x0600149C RID: 5276 RVA: 0x00152742 File Offset: 0x00151742
		public override bool IsCurrentAfterLast
		{
			get
			{
				return this._view.IsCurrentAfterLast;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x0600149D RID: 5277 RVA: 0x0015274F File Offset: 0x0015174F
		public override bool IsCurrentBeforeFirst
		{
			get
			{
				return this._view.IsCurrentBeforeFirst;
			}
		}

		// Token: 0x0600149E RID: 5278 RVA: 0x0015275C File Offset: 0x0015175C
		public override bool MoveCurrentToFirst()
		{
			return this._view.MoveCurrentToFirst();
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x00152769 File Offset: 0x00151769
		public override bool MoveCurrentToPrevious()
		{
			return this._view.MoveCurrentToPrevious();
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x00152776 File Offset: 0x00151776
		public override bool MoveCurrentToNext()
		{
			return this._view.MoveCurrentToNext();
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x00152783 File Offset: 0x00151783
		public override bool MoveCurrentToLast()
		{
			return this._view.MoveCurrentToLast();
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x00152790 File Offset: 0x00151790
		public override bool MoveCurrentTo(object item)
		{
			return this._view.MoveCurrentTo(item);
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x0015279E File Offset: 0x0015179E
		public override bool MoveCurrentToPosition(int position)
		{
			return this._view.MoveCurrentToPosition(position);
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x060014A4 RID: 5284 RVA: 0x001527AC File Offset: 0x001517AC
		public ReadOnlyCollection<ItemPropertyInfo> ItemProperties
		{
			get
			{
				return ((IItemProperties)this._view).ItemProperties;
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x060014A5 RID: 5285 RVA: 0x001527B9 File Offset: 0x001517B9
		public override int Count
		{
			get
			{
				this.EnsureSnapshot();
				return this._view.Count;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x060014A6 RID: 5286 RVA: 0x001527CC File Offset: 0x001517CC
		public override bool IsEmpty
		{
			get
			{
				this.EnsureSnapshot();
				return this._view == null || this._view.IsEmpty;
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x060014A7 RID: 5287 RVA: 0x001527E9 File Offset: 0x001517E9
		public override bool NeedsRefresh
		{
			get
			{
				return this._view.NeedsRefresh;
			}
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x001527F6 File Offset: 0x001517F6
		public override int IndexOf(object item)
		{
			this.EnsureSnapshot();
			return this._view.IndexOf(item);
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x0015280A File Offset: 0x0015180A
		public override bool PassesFilter(object item)
		{
			return !this._view.CanFilter || this._view.Filter == null || this._view.Filter(item);
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x00152839 File Offset: 0x00151839
		public override object GetItemAt(int index)
		{
			this.EnsureSnapshot();
			return this._view.GetItemAt(index);
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x0015284D File Offset: 0x0015184D
		protected override IEnumerator GetEnumerator()
		{
			this.EnsureSnapshot();
			return ((IEnumerable)this._view).GetEnumerator();
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x00152860 File Offset: 0x00151860
		protected override void RefreshOverride()
		{
			this.LoadSnapshot(this.SourceCollection);
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x00152870 File Offset: 0x00151870
		protected override void ProcessCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (this._view == null)
			{
				return;
			}
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (args.NewStartingIndex < 0 || this._snapshot.Count <= args.NewStartingIndex)
				{
					for (int i = 0; i < args.NewItems.Count; i++)
					{
						this._snapshot.Add(args.NewItems[i]);
					}
					return;
				}
				for (int j = args.NewItems.Count - 1; j >= 0; j--)
				{
					this._snapshot.Insert(args.NewStartingIndex, args.NewItems[j]);
				}
				return;
			case NotifyCollectionChangedAction.Remove:
			{
				if (args.OldStartingIndex < 0)
				{
					throw new InvalidOperationException(SR.Get("RemovedItemNotFound"));
				}
				int k = args.OldItems.Count - 1;
				int num = args.OldStartingIndex + k;
				while (k >= 0)
				{
					if (!ItemsControl.EqualsEx(args.OldItems[k], this._snapshot[num]))
					{
						throw new InvalidOperationException(SR.Get("AddedItemNotAtIndex", new object[]
						{
							num
						}));
					}
					this._snapshot.RemoveAt(num);
					k--;
					num--;
				}
				return;
			}
			case NotifyCollectionChangedAction.Replace:
			{
				int l = args.NewItems.Count - 1;
				int num2 = args.NewStartingIndex + l;
				while (l >= 0)
				{
					if (!ItemsControl.EqualsEx(args.OldItems[l], this._snapshot[num2]))
					{
						throw new InvalidOperationException(SR.Get("AddedItemNotAtIndex", new object[]
						{
							num2
						}));
					}
					this._snapshot[num2] = args.NewItems[l];
					l--;
					num2--;
				}
				return;
			}
			case NotifyCollectionChangedAction.Move:
			{
				if (args.NewStartingIndex < 0)
				{
					throw new InvalidOperationException(SR.Get("CannotMoveToUnknownPosition"));
				}
				if (args.OldStartingIndex < args.NewStartingIndex)
				{
					int m = args.OldItems.Count - 1;
					int num3 = args.OldStartingIndex + m;
					int num4 = args.NewStartingIndex + m;
					while (m >= 0)
					{
						if (!ItemsControl.EqualsEx(args.OldItems[m], this._snapshot[num3]))
						{
							throw new InvalidOperationException(SR.Get("AddedItemNotAtIndex", new object[]
							{
								num3
							}));
						}
						this._snapshot.Move(num3, num4);
						m--;
						num3--;
						num4--;
					}
					return;
				}
				int n = 0;
				int num5 = args.OldStartingIndex + n;
				int num6 = args.NewStartingIndex + n;
				while (n < args.OldItems.Count)
				{
					if (!ItemsControl.EqualsEx(args.OldItems[n], this._snapshot[num5]))
					{
						throw new InvalidOperationException(SR.Get("AddedItemNotAtIndex", new object[]
						{
							num5
						}));
					}
					this._snapshot.Move(num5, num6);
					n++;
					num5++;
					num6++;
				}
				return;
			}
			case NotifyCollectionChangedAction.Reset:
				this.LoadSnapshot(this.SourceCollection);
				return;
			default:
				return;
			}
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x00152B94 File Offset: 0x00151B94
		private void LoadSnapshot(IEnumerable source)
		{
			base.OnCurrentChanging();
			object currentItem = this.CurrentItem;
			int currentPosition = this.CurrentPosition;
			bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
			bool isCurrentAfterLast = this.IsCurrentAfterLast;
			this.LoadSnapshotCore(source);
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			this.OnCurrentChanged();
			if (this.IsCurrentAfterLast != isCurrentAfterLast)
			{
				this.OnPropertyChanged(new PropertyChangedEventArgs("IsCurrentAfterLast"));
			}
			if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
			{
				this.OnPropertyChanged(new PropertyChangedEventArgs("IsCurrentBeforeFirst"));
			}
			if (currentPosition != this.CurrentPosition)
			{
				this.OnPropertyChanged(new PropertyChangedEventArgs("CurrentPosition"));
			}
			if (currentItem != this.CurrentItem)
			{
				this.OnPropertyChanged(new PropertyChangedEventArgs("CurrentItem"));
			}
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x00152C3C File Offset: 0x00151C3C
		private void LoadSnapshotCore(IEnumerable source)
		{
			IEnumerator enumerator = source.GetEnumerator();
			using (this.IgnoreViewEvents())
			{
				this._snapshot.Clear();
				while (enumerator.MoveNext())
				{
					object item = enumerator.Current;
					this._snapshot.Add(item);
				}
			}
			if (this._pollForChanges)
			{
				IEnumerator trackingEnumerator = this._trackingEnumerator;
				this._trackingEnumerator = enumerator;
				enumerator = trackingEnumerator;
			}
			IDisposable disposable2 = enumerator as IDisposable;
			if (disposable2 != null)
			{
				disposable2.Dispose();
			}
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x00152CC0 File Offset: 0x00151CC0
		private void EnsureSnapshot()
		{
			if (this._pollForChanges)
			{
				try
				{
					this._trackingEnumerator.MoveNext();
				}
				catch (InvalidOperationException)
				{
					if (TraceData.IsEnabled && !this._warningHasBeenRaised)
					{
						this._warningHasBeenRaised = true;
						TraceData.TraceAndNotify(TraceEventType.Warning, TraceData.CollectionChangedWithoutNotification(new object[]
						{
							this.SourceCollection.GetType().FullName
						}), null);
					}
					this.LoadSnapshotCore(this.SourceCollection);
				}
			}
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x00152D40 File Offset: 0x00151D40
		private IDisposable IgnoreViewEvents()
		{
			return new EnumerableCollectionView.IgnoreViewEventsHelper(this);
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x00152D48 File Offset: 0x00151D48
		private void BeginIgnoreEvents()
		{
			this._ignoreEventsLevel++;
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x00152D58 File Offset: 0x00151D58
		private void EndIgnoreEvents()
		{
			this._ignoreEventsLevel--;
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x00152D68 File Offset: 0x00151D68
		private void _OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (this._ignoreEventsLevel != 0)
			{
				return;
			}
			this.OnPropertyChanged(args);
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x00152D7A File Offset: 0x00151D7A
		private void _OnViewChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if (this._ignoreEventsLevel != 0)
			{
				return;
			}
			this.OnCollectionChanged(args);
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x00152D8C File Offset: 0x00151D8C
		private void _OnCurrentChanging(object sender, CurrentChangingEventArgs args)
		{
			if (this._ignoreEventsLevel != 0)
			{
				return;
			}
			base.OnCurrentChanging();
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x00152D9D File Offset: 0x00151D9D
		private void _OnCurrentChanged(object sender, EventArgs args)
		{
			if (this._ignoreEventsLevel != 0)
			{
				return;
			}
			this.OnCurrentChanged();
		}

		// Token: 0x04000BDA RID: 3034
		private ListCollectionView _view;

		// Token: 0x04000BDB RID: 3035
		private ObservableCollection<object> _snapshot;

		// Token: 0x04000BDC RID: 3036
		private IEnumerator _trackingEnumerator;

		// Token: 0x04000BDD RID: 3037
		private int _ignoreEventsLevel;

		// Token: 0x04000BDE RID: 3038
		private bool _pollForChanges;

		// Token: 0x04000BDF RID: 3039
		private bool _warningHasBeenRaised;

		// Token: 0x020009F1 RID: 2545
		private class IgnoreViewEventsHelper : IDisposable
		{
			// Token: 0x06008455 RID: 33877 RVA: 0x003257D0 File Offset: 0x003247D0
			public IgnoreViewEventsHelper(EnumerableCollectionView parent)
			{
				this._parent = parent;
				this._parent.BeginIgnoreEvents();
			}

			// Token: 0x06008456 RID: 33878 RVA: 0x003257EA File Offset: 0x003247EA
			public void Dispose()
			{
				if (this._parent != null)
				{
					this._parent.EndIgnoreEvents();
					this._parent = null;
				}
				GC.SuppressFinalize(this);
			}

			// Token: 0x0400401C RID: 16412
			private EnumerableCollectionView _parent;
		}
	}
}
