using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Controls;
using MS.Utility;

namespace System.Windows.Controls
{
	// Token: 0x0200079C RID: 1948
	public sealed class ItemContainerGenerator : IRecyclingItemContainerGenerator, IItemContainerGenerator, IWeakEventListener
	{
		// Token: 0x06006D42 RID: 27970 RVA: 0x002CBBA0 File Offset: 0x002CABA0
		internal ItemContainerGenerator(IGeneratorHost host) : this(null, host, host as DependencyObject, 0)
		{
			CollectionChangedEventManager.AddHandler(host.View, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnCollectionChanged));
		}

		// Token: 0x06006D43 RID: 27971 RVA: 0x002CBBC8 File Offset: 0x002CABC8
		private ItemContainerGenerator(ItemContainerGenerator parent, GroupItem groupItem) : this(parent, parent.Host, groupItem, parent.Level + 1)
		{
		}

		// Token: 0x06006D44 RID: 27972 RVA: 0x002CBBE0 File Offset: 0x002CABE0
		private ItemContainerGenerator(ItemContainerGenerator parent, IGeneratorHost host, DependencyObject peer, int level)
		{
			this._parent = parent;
			this._host = host;
			this._peer = peer;
			this._level = level;
			this.OnRefresh();
		}

		// Token: 0x17001954 RID: 6484
		// (get) Token: 0x06006D45 RID: 27973 RVA: 0x002CBC16 File Offset: 0x002CAC16
		public GeneratorStatus Status
		{
			get
			{
				return this._status;
			}
		}

		// Token: 0x06006D46 RID: 27974 RVA: 0x002CBC20 File Offset: 0x002CAC20
		private void SetStatus(GeneratorStatus value)
		{
			if (value != this._status)
			{
				this._status = value;
				GeneratorStatus status = this._status;
				if (status != GeneratorStatus.GeneratingContainers)
				{
					if (status == GeneratorStatus.ContainersGenerated)
					{
						string text = null;
						if (this._itemsGenerated >= 0)
						{
							DependencyObject dependencyObject = this.Host as DependencyObject;
							if (dependencyObject != null)
							{
								text = (string)dependencyObject.GetValue(FrameworkElement.NameProperty);
							}
							if (text == null || text.Length == 0)
							{
								text = this.Host.GetHashCode().ToString(CultureInfo.InvariantCulture);
							}
							EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringEnd, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, string.Format(CultureInfo.InvariantCulture, "ItemContainerGenerator for {0} {1} - {2} items", this.Host.GetType().Name, text, this._itemsGenerated));
						}
					}
				}
				else if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info))
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringBegin, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "ItemsControl.Generator");
					this._itemsGenerated = 0;
				}
				else
				{
					this._itemsGenerated = int.MinValue;
				}
				if (this.StatusChanged != null)
				{
					this.StatusChanged(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x17001955 RID: 6485
		// (get) Token: 0x06006D47 RID: 27975 RVA: 0x002CBD2D File Offset: 0x002CAD2D
		public ReadOnlyCollection<object> Items
		{
			get
			{
				if (this._itemsReadOnly == null && this._items != null)
				{
					this._itemsReadOnly = new ReadOnlyCollection<object>(new ListOfObject(this._items));
				}
				return this._itemsReadOnly;
			}
		}

		// Token: 0x06006D48 RID: 27976 RVA: 0x002CBD5C File Offset: 0x002CAD5C
		ItemContainerGenerator IItemContainerGenerator.GetItemContainerGeneratorForPanel(Panel panel)
		{
			if (!panel.IsItemsHost)
			{
				throw new ArgumentException(SR.Get("PanelIsNotItemsHost"), "panel");
			}
			ItemsPresenter itemsPresenter = ItemsPresenter.FromPanel(panel);
			if (itemsPresenter != null)
			{
				return itemsPresenter.Generator;
			}
			if (panel.TemplatedParent != null)
			{
				return this;
			}
			return null;
		}

		// Token: 0x06006D49 RID: 27977 RVA: 0x002CBDA2 File Offset: 0x002CADA2
		IDisposable IItemContainerGenerator.StartAt(GeneratorPosition position, GeneratorDirection direction)
		{
			return ((IItemContainerGenerator)this).StartAt(position, direction, false);
		}

		// Token: 0x06006D4A RID: 27978 RVA: 0x002CBDAD File Offset: 0x002CADAD
		IDisposable IItemContainerGenerator.StartAt(GeneratorPosition position, GeneratorDirection direction, bool allowStartAtRealizedItem)
		{
			if (this._generator != null)
			{
				throw new InvalidOperationException(SR.Get("GenerationInProgress"));
			}
			this._generator = new ItemContainerGenerator.Generator(this, position, direction, allowStartAtRealizedItem);
			return this._generator;
		}

		// Token: 0x06006D4B RID: 27979 RVA: 0x002CBDDC File Offset: 0x002CADDC
		public IDisposable GenerateBatches()
		{
			if (this._isGeneratingBatches)
			{
				throw new InvalidOperationException(SR.Get("GenerationInProgress"));
			}
			return new ItemContainerGenerator.BatchGenerator(this);
		}

		// Token: 0x06006D4C RID: 27980 RVA: 0x002CBDFC File Offset: 0x002CADFC
		DependencyObject IItemContainerGenerator.GenerateNext()
		{
			if (this._generator == null)
			{
				throw new InvalidOperationException(SR.Get("GenerationNotInProgress"));
			}
			bool flag;
			return this._generator.GenerateNext(true, out flag);
		}

		// Token: 0x06006D4D RID: 27981 RVA: 0x002CBE2F File Offset: 0x002CAE2F
		DependencyObject IItemContainerGenerator.GenerateNext(out bool isNewlyRealized)
		{
			if (this._generator == null)
			{
				throw new InvalidOperationException(SR.Get("GenerationNotInProgress"));
			}
			return this._generator.GenerateNext(false, out isNewlyRealized);
		}

		// Token: 0x06006D4E RID: 27982 RVA: 0x002CBE58 File Offset: 0x002CAE58
		void IItemContainerGenerator.PrepareItemContainer(DependencyObject container)
		{
			object item = container.ReadLocalValue(ItemContainerGenerator.ItemForItemContainerProperty);
			this.Host.PrepareItemContainer(container, item);
		}

		// Token: 0x06006D4F RID: 27983 RVA: 0x002CBE7E File Offset: 0x002CAE7E
		void IItemContainerGenerator.Remove(GeneratorPosition position, int count)
		{
			this.Remove(position, count, false);
		}

		// Token: 0x06006D50 RID: 27984 RVA: 0x002CBE8C File Offset: 0x002CAE8C
		private void Remove(GeneratorPosition position, int count, bool isRecycling)
		{
			if (position.Offset != 0)
			{
				throw new ArgumentException(SR.Get("RemoveRequiresOffsetZero", new object[]
				{
					position.Index,
					position.Offset
				}), "position");
			}
			if (count <= 0)
			{
				throw new ArgumentException(SR.Get("RemoveRequiresPositiveCount", new object[]
				{
					count
				}), "count");
			}
			if (this._itemMap == null)
			{
				return;
			}
			int index = position.Index;
			int num = index;
			ItemContainerGenerator.ItemBlock itemBlock = this._itemMap.Next;
			while (itemBlock != this._itemMap && num >= itemBlock.ContainerCount)
			{
				num -= itemBlock.ContainerCount;
				itemBlock = itemBlock.Next;
			}
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock = itemBlock as ItemContainerGenerator.RealizedItemBlock;
			int num2 = num + count - 1;
			while (itemBlock != this._itemMap)
			{
				if (!(itemBlock is ItemContainerGenerator.RealizedItemBlock))
				{
					throw new InvalidOperationException(SR.Get("CannotRemoveUnrealizedItems", new object[]
					{
						index,
						count
					}));
				}
				if (num2 < itemBlock.ContainerCount)
				{
					break;
				}
				num2 -= itemBlock.ContainerCount;
				itemBlock = itemBlock.Next;
			}
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock2 = itemBlock as ItemContainerGenerator.RealizedItemBlock;
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock3 = realizedItemBlock;
			int num3 = num;
			while (realizedItemBlock3 != realizedItemBlock2 || num3 <= num2)
			{
				DependencyObject dependencyObject = realizedItemBlock3.ContainerAt(num3);
				this.UnlinkContainerFromItem(dependencyObject, realizedItemBlock3.ItemAt(num3));
				bool flag = this._generatesGroupItems && !(dependencyObject is GroupItem);
				if (isRecycling && !flag)
				{
					if (this._containerType == null)
					{
						this._containerType = dependencyObject.GetType();
					}
					else if (this._containerType != dependencyObject.GetType())
					{
						throw new InvalidOperationException(SR.Get("CannotRecyleHeterogeneousTypes"));
					}
					this._recyclableContainers.Enqueue(dependencyObject);
				}
				if (++num3 >= realizedItemBlock3.ContainerCount && realizedItemBlock3 != realizedItemBlock2)
				{
					realizedItemBlock3 = (realizedItemBlock3.Next as ItemContainerGenerator.RealizedItemBlock);
					num3 = 0;
				}
			}
			bool flag2 = num == 0;
			bool flag3 = num2 == realizedItemBlock2.ItemCount - 1;
			bool flag4 = flag2 && realizedItemBlock.Prev is ItemContainerGenerator.UnrealizedItemBlock;
			bool flag5 = flag3 && realizedItemBlock2.Next is ItemContainerGenerator.UnrealizedItemBlock;
			ItemContainerGenerator.ItemBlock itemBlock2 = null;
			ItemContainerGenerator.UnrealizedItemBlock unrealizedItemBlock;
			int num4;
			int num5;
			if (flag4)
			{
				unrealizedItemBlock = (ItemContainerGenerator.UnrealizedItemBlock)realizedItemBlock.Prev;
				num4 = unrealizedItemBlock.ItemCount;
				num5 = -unrealizedItemBlock.ItemCount;
			}
			else if (flag5)
			{
				unrealizedItemBlock = (ItemContainerGenerator.UnrealizedItemBlock)realizedItemBlock2.Next;
				num4 = 0;
				num5 = num;
			}
			else
			{
				unrealizedItemBlock = new ItemContainerGenerator.UnrealizedItemBlock();
				num4 = 0;
				num5 = num;
				itemBlock2 = (flag2 ? realizedItemBlock.Prev : realizedItemBlock);
			}
			for (itemBlock = realizedItemBlock; itemBlock != realizedItemBlock2; itemBlock = itemBlock.Next)
			{
				int itemCount = itemBlock.ItemCount;
				this.MoveItems(itemBlock, num, itemCount - num, unrealizedItemBlock, num4, num5);
				num4 += itemCount - num;
				num = 0;
				num5 -= itemCount;
				if (itemBlock.ItemCount == 0)
				{
					itemBlock.Remove();
				}
			}
			int count2 = itemBlock.ItemCount - 1 - num2;
			this.MoveItems(itemBlock, num, num2 - num + 1, unrealizedItemBlock, num4, num5);
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock4 = realizedItemBlock2;
			if (!flag3)
			{
				if (realizedItemBlock == realizedItemBlock2 && !flag2)
				{
					realizedItemBlock4 = new ItemContainerGenerator.RealizedItemBlock();
				}
				this.MoveItems(itemBlock, num2 + 1, count2, realizedItemBlock4, 0, num2 + 1);
			}
			if (itemBlock2 != null)
			{
				unrealizedItemBlock.InsertAfter(itemBlock2);
			}
			if (realizedItemBlock4 != realizedItemBlock2)
			{
				realizedItemBlock4.InsertAfter(unrealizedItemBlock);
			}
			this.RemoveAndCoalesceBlocksIfNeeded(itemBlock);
		}

		// Token: 0x06006D51 RID: 27985 RVA: 0x002CC1DF File Offset: 0x002CB1DF
		void IItemContainerGenerator.RemoveAll()
		{
			this.RemoveAllInternal(false);
		}

		// Token: 0x06006D52 RID: 27986 RVA: 0x002CC1E8 File Offset: 0x002CB1E8
		internal void RemoveAllInternal(bool saveRecycleQueue)
		{
			ItemContainerGenerator.ItemBlock itemMap = this._itemMap;
			this._itemMap = null;
			try
			{
				if (itemMap != null)
				{
					for (ItemContainerGenerator.ItemBlock next = itemMap.Next; next != itemMap; next = next.Next)
					{
						ItemContainerGenerator.RealizedItemBlock realizedItemBlock = next as ItemContainerGenerator.RealizedItemBlock;
						if (realizedItemBlock != null)
						{
							for (int i = 0; i < realizedItemBlock.ContainerCount; i++)
							{
								this.UnlinkContainerFromItem(realizedItemBlock.ContainerAt(i), realizedItemBlock.ItemAt(i));
							}
						}
					}
				}
			}
			finally
			{
				this.PrepareGrouping();
				this._itemMap = new ItemContainerGenerator.ItemBlock();
				this._itemMap.Prev = (this._itemMap.Next = this._itemMap);
				ItemContainerGenerator.UnrealizedItemBlock unrealizedItemBlock = new ItemContainerGenerator.UnrealizedItemBlock();
				unrealizedItemBlock.InsertAfter(this._itemMap);
				unrealizedItemBlock.ItemCount = this.ItemsInternal.Count;
				if (!saveRecycleQueue)
				{
					this.ResetRecyclableContainers();
				}
				this.SetAlternationCount();
				if (this.MapChanged != null)
				{
					this.MapChanged(null, -1, 0, unrealizedItemBlock, 0, 0);
				}
			}
		}

		// Token: 0x06006D53 RID: 27987 RVA: 0x002CC2E0 File Offset: 0x002CB2E0
		private void ResetRecyclableContainers()
		{
			this._recyclableContainers = new Queue<DependencyObject>();
			this._containerType = null;
			this._generatesGroupItems = false;
		}

		// Token: 0x06006D54 RID: 27988 RVA: 0x002CC2FB File Offset: 0x002CB2FB
		void IRecyclingItemContainerGenerator.Recycle(GeneratorPosition position, int count)
		{
			this.Remove(position, count, true);
		}

		// Token: 0x06006D55 RID: 27989 RVA: 0x002CC308 File Offset: 0x002CB308
		GeneratorPosition IItemContainerGenerator.GeneratorPositionFromIndex(int itemIndex)
		{
			GeneratorPosition result;
			ItemContainerGenerator.ItemBlock itemBlock;
			int num;
			this.GetBlockAndPosition(itemIndex, out result, out itemBlock, out num);
			if (itemBlock == this._itemMap && result.Index == -1)
			{
				int offset = result.Offset + 1;
				result.Offset = offset;
			}
			return result;
		}

		// Token: 0x06006D56 RID: 27990 RVA: 0x002CC348 File Offset: 0x002CB348
		int IItemContainerGenerator.IndexFromGeneratorPosition(GeneratorPosition position)
		{
			int num = position.Index;
			if (num != -1)
			{
				if (this._itemMap != null)
				{
					int num2 = 0;
					for (ItemContainerGenerator.ItemBlock next = this._itemMap.Next; next != this._itemMap; next = next.Next)
					{
						if (num < next.ContainerCount)
						{
							return num2 + num + position.Offset;
						}
						num2 += next.ItemCount;
						num -= next.ContainerCount;
					}
				}
				return -1;
			}
			if (position.Offset >= 0)
			{
				return position.Offset - 1;
			}
			return this.ItemsInternal.Count + position.Offset;
		}

		// Token: 0x06006D57 RID: 27991 RVA: 0x002CC3DC File Offset: 0x002CB3DC
		public object ItemFromContainer(DependencyObject container)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			object obj = container.ReadLocalValue(ItemContainerGenerator.ItemForItemContainerProperty);
			if (obj != DependencyProperty.UnsetValue && !this.Host.IsHostForItemContainer(container))
			{
				obj = DependencyProperty.UnsetValue;
			}
			return obj;
		}

		// Token: 0x06006D58 RID: 27992 RVA: 0x002CC420 File Offset: 0x002CB420
		public DependencyObject ContainerFromItem(object item)
		{
			object obj;
			DependencyObject result;
			int num;
			this.DoLinearSearch((object o, DependencyObject d) => ItemsControl.EqualsEx(o, item), out obj, out result, out num, false);
			return result;
		}

		// Token: 0x06006D59 RID: 27993 RVA: 0x002CC455 File Offset: 0x002CB455
		public int IndexFromContainer(DependencyObject container)
		{
			return this.IndexFromContainer(container, false);
		}

		// Token: 0x06006D5A RID: 27994 RVA: 0x002CC460 File Offset: 0x002CB460
		public int IndexFromContainer(DependencyObject container, bool returnLocalIndex)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			object obj;
			DependencyObject dependencyObject;
			int result;
			this.DoLinearSearch((object o, DependencyObject d) => d == container, out obj, out dependencyObject, out result, returnLocalIndex);
			return result;
		}

		// Token: 0x06006D5B RID: 27995 RVA: 0x002CC4A8 File Offset: 0x002CB4A8
		internal bool FindItem(Func<object, DependencyObject, bool> match, out DependencyObject container, out int itemIndex)
		{
			object obj;
			return this.DoLinearSearch(match, out obj, out container, out itemIndex, false);
		}

		// Token: 0x06006D5C RID: 27996 RVA: 0x002CC4C4 File Offset: 0x002CB4C4
		private bool DoLinearSearch(Func<object, DependencyObject, bool> match, out object item, out DependencyObject container, out int itemIndex, bool returnLocalIndex)
		{
			item = null;
			container = null;
			itemIndex = 0;
			if (this._itemMap != null)
			{
				ItemContainerGenerator.ItemBlock itemBlock = this._itemMap.Next;
				int num = 0;
				while (num <= this._startIndexForUIFromItem && itemBlock != this._itemMap)
				{
					num += itemBlock.ItemCount;
					itemBlock = itemBlock.Next;
				}
				itemBlock = itemBlock.Prev;
				num -= itemBlock.ItemCount;
				ItemContainerGenerator.RealizedItemBlock realizedItemBlock = itemBlock as ItemContainerGenerator.RealizedItemBlock;
				int num2;
				if (realizedItemBlock != null)
				{
					num2 = this._startIndexForUIFromItem - num;
					if (num2 >= realizedItemBlock.ItemCount)
					{
						num2 = 0;
					}
				}
				else
				{
					num2 = 0;
				}
				ItemContainerGenerator.ItemBlock itemBlock2 = itemBlock;
				int i = num2;
				int num3 = itemBlock.ItemCount;
				for (;;)
				{
					if (realizedItemBlock != null)
					{
						while (i < num3)
						{
							bool flag = match(realizedItemBlock.ItemAt(i), realizedItemBlock.ContainerAt(i));
							if (flag)
							{
								item = realizedItemBlock.ItemAt(i);
								container = realizedItemBlock.ContainerAt(i);
							}
							else if (!returnLocalIndex && this.IsGrouping && realizedItemBlock.ItemAt(i) is CollectionViewGroup)
							{
								int num4;
								flag = ((GroupItem)realizedItemBlock.ContainerAt(i)).Generator.DoLinearSearch(match, out item, out container, out num4, false);
								if (flag)
								{
									itemIndex = num4;
								}
							}
							if (flag)
							{
								goto Block_11;
							}
							i++;
						}
						if (itemBlock2 == itemBlock && i == num2)
						{
							goto IL_19C;
						}
					}
					num += itemBlock2.ItemCount;
					i = 0;
					itemBlock2 = itemBlock2.Next;
					if (itemBlock2 == this._itemMap)
					{
						itemBlock2 = itemBlock2.Next;
						num = 0;
					}
					num3 = itemBlock2.ItemCount;
					realizedItemBlock = (itemBlock2 as ItemContainerGenerator.RealizedItemBlock);
					if (itemBlock2 == itemBlock)
					{
						if (realizedItemBlock == null)
						{
							goto IL_19C;
						}
						num3 = num2;
					}
				}
				Block_11:
				this._startIndexForUIFromItem = num + i;
				itemIndex += this.GetRealizedItemBlockCount(realizedItemBlock, i, returnLocalIndex) + this.GetCount(itemBlock2, returnLocalIndex);
				return true;
			}
			IL_19C:
			itemIndex = -1;
			item = null;
			container = null;
			return false;
		}

		// Token: 0x06006D5D RID: 27997 RVA: 0x002CC678 File Offset: 0x002CB678
		private int GetCount()
		{
			return this.GetCount(this._itemMap);
		}

		// Token: 0x06006D5E RID: 27998 RVA: 0x002CC686 File Offset: 0x002CB686
		private int GetCount(ItemContainerGenerator.ItemBlock stop)
		{
			return this.GetCount(stop, false);
		}

		// Token: 0x06006D5F RID: 27999 RVA: 0x002CC690 File Offset: 0x002CB690
		private int GetCount(ItemContainerGenerator.ItemBlock stop, bool returnLocalIndex)
		{
			if (this._itemMap == null)
			{
				return 0;
			}
			int num = 0;
			for (ItemContainerGenerator.ItemBlock next = this._itemMap.Next; next != stop; next = next.Next)
			{
				num += next.ItemCount;
			}
			if (!returnLocalIndex && this.IsGrouping)
			{
				int num2 = num;
				num = 0;
				for (int i = 0; i < num2; i++)
				{
					CollectionViewGroup collectionViewGroup = this.Items[i] as CollectionViewGroup;
					num += ((collectionViewGroup == null) ? 1 : collectionViewGroup.ItemCount);
				}
			}
			return num;
		}

		// Token: 0x06006D60 RID: 28000 RVA: 0x002CC70C File Offset: 0x002CB70C
		private int GetRealizedItemBlockCount(ItemContainerGenerator.RealizedItemBlock rib, int end, bool returnLocalIndex)
		{
			if (!this.IsGrouping || returnLocalIndex)
			{
				return end;
			}
			int num = 0;
			for (int i = 0; i < end; i++)
			{
				CollectionViewGroup collectionViewGroup;
				if ((collectionViewGroup = (rib.ItemAt(i) as CollectionViewGroup)) != null)
				{
					num += collectionViewGroup.ItemCount;
				}
				else
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06006D61 RID: 28001 RVA: 0x002CC758 File Offset: 0x002CB758
		public DependencyObject ContainerFromIndex(int index)
		{
			if (this._itemMap == null)
			{
				return null;
			}
			int num = 0;
			if (this.IsGrouping)
			{
				num = index;
				index = 0;
				int count = this.ItemsInternal.Count;
				while (index < count)
				{
					CollectionViewGroup collectionViewGroup = this.ItemsInternal[index] as CollectionViewGroup;
					int num2 = (collectionViewGroup == null) ? 1 : collectionViewGroup.ItemCount;
					if (num < num2)
					{
						break;
					}
					num -= num2;
					index++;
				}
			}
			for (ItemContainerGenerator.ItemBlock next = this._itemMap.Next; next != this._itemMap; next = next.Next)
			{
				if (index < next.ItemCount)
				{
					DependencyObject dependencyObject = next.ContainerAt(index);
					GroupItem groupItem = dependencyObject as GroupItem;
					if (groupItem != null)
					{
						dependencyObject = groupItem.Generator.ContainerFromIndex(num);
					}
					return dependencyObject;
				}
				index -= next.ItemCount;
			}
			return null;
		}

		// Token: 0x1400012F RID: 303
		// (add) Token: 0x06006D62 RID: 28002 RVA: 0x002CC820 File Offset: 0x002CB820
		// (remove) Token: 0x06006D63 RID: 28003 RVA: 0x002CC858 File Offset: 0x002CB858
		public event ItemsChangedEventHandler ItemsChanged;

		// Token: 0x14000130 RID: 304
		// (add) Token: 0x06006D64 RID: 28004 RVA: 0x002CC890 File Offset: 0x002CB890
		// (remove) Token: 0x06006D65 RID: 28005 RVA: 0x002CC8C8 File Offset: 0x002CB8C8
		public event EventHandler StatusChanged;

		// Token: 0x17001956 RID: 6486
		// (get) Token: 0x06006D66 RID: 28006 RVA: 0x002CC8FD File Offset: 0x002CB8FD
		internal IEnumerable RecyclableContainers
		{
			get
			{
				return this._recyclableContainers;
			}
		}

		// Token: 0x06006D67 RID: 28007 RVA: 0x002CC905 File Offset: 0x002CB905
		internal void Refresh()
		{
			this.OnRefresh();
		}

		// Token: 0x06006D68 RID: 28008 RVA: 0x002CC90D File Offset: 0x002CB90D
		internal void Release()
		{
			((IItemContainerGenerator)this).RemoveAll();
		}

		// Token: 0x06006D69 RID: 28009 RVA: 0x002CC918 File Offset: 0x002CB918
		internal void Verify()
		{
			if (this._itemMap == null)
			{
				return;
			}
			List<string> list = new List<string>();
			int num = 0;
			for (ItemContainerGenerator.ItemBlock next = this._itemMap.Next; next != this._itemMap; next = next.Next)
			{
				num += next.ItemCount;
			}
			if (num != this._items.Count)
			{
				list.Add(SR.Get("Generator_CountIsWrong", new object[]
				{
					num,
					this._items.Count
				}));
			}
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			for (ItemContainerGenerator.ItemBlock next2 = this._itemMap.Next; next2 != this._itemMap; next2 = next2.Next)
			{
				ItemContainerGenerator.RealizedItemBlock realizedItemBlock = next2 as ItemContainerGenerator.RealizedItemBlock;
				if (realizedItemBlock != null)
				{
					for (int i = 0; i < realizedItemBlock.ItemCount; i++)
					{
						int num5 = num4 + i;
						object obj = realizedItemBlock.ItemAt(i);
						object obj2 = (num5 < this._items.Count) ? this._items[num5] : null;
						if (!ItemsControl.EqualsEx(obj, obj2))
						{
							if (num3 < 3)
							{
								list.Add(SR.Get("Generator_ItemIsWrong", new object[]
								{
									num5,
									obj,
									obj2
								}));
								num3++;
							}
							num2++;
						}
					}
				}
				num4 += next2.ItemCount;
			}
			if (num2 > num3)
			{
				list.Add(SR.Get("Generator_MoreErrors", new object[]
				{
					num2 - num3
				}));
			}
			if (list.Count > 0)
			{
				CultureInfo invariantEnglishUS = TypeConverterHelper.InvariantEnglishUS;
				DependencyObject peer = this.Peer;
				string text = (string)peer.GetValue(FrameworkElement.NameProperty);
				if (string.IsNullOrWhiteSpace(text))
				{
					text = SR.Get("Generator_Unnamed");
				}
				List<string> list2 = new List<string>();
				this.GetCollectionChangedSources(0, new Action<int, object, bool?, List<string>>(this.FormatCollectionChangedSource), list2);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(SR.Get("Generator_Readme0"));
				stringBuilder.Append(SR.Get("Generator_Readme1", new object[]
				{
					peer,
					text
				}));
				stringBuilder.Append("  ");
				stringBuilder.AppendLine(SR.Get("Generator_Readme2"));
				foreach (string arg in list)
				{
					stringBuilder.AppendFormat(invariantEnglishUS, "  {0}", arg);
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(SR.Get("Generator_Readme3"));
				foreach (string arg2 in list2)
				{
					stringBuilder.AppendFormat(invariantEnglishUS, "  {0}", arg2);
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendLine(SR.Get("Generator_Readme4"));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(SR.Get("Generator_Readme5"));
				stringBuilder.AppendLine();
				stringBuilder.Append(SR.Get("Generator_Readme6"));
				stringBuilder.Append("  ");
				stringBuilder.Append(SR.Get("Generator_Readme7", new object[]
				{
					"PresentationTraceSources.TraceLevel",
					"High"
				}));
				stringBuilder.Append("  ");
				stringBuilder.AppendLine(SR.Get("Generator_Readme8", new object[]
				{
					"System.Diagnostics.PresentationTraceSources.SetTraceLevel(myItemsControl.ItemContainerGenerator, System.Diagnostics.PresentationTraceLevel.High)"
				}));
				stringBuilder.AppendLine(SR.Get("Generator_Readme9"));
				Exception innerException = new Exception(stringBuilder.ToString());
				throw new InvalidOperationException(SR.Get("Generator_Inconsistent"), innerException);
			}
		}

		// Token: 0x06006D6A RID: 28010 RVA: 0x002CCCF4 File Offset: 0x002CBCF4
		private void FormatCollectionChangedSource(int level, object source, bool? isLikely, List<string> sources)
		{
			Type type = source.GetType();
			if (isLikely == null)
			{
				isLikely = new bool?(true);
				string assemblyQualifiedName = type.AssemblyQualifiedName;
				int num = assemblyQualifiedName.LastIndexOf("PublicKeyToken=");
				if (num >= 0)
				{
					string strA = assemblyQualifiedName.Substring(num + "PublicKeyToken=".Length);
					if (string.Compare(strA, "31bf3856ad364e35", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(strA, "b77a5c561934e089", StringComparison.OrdinalIgnoreCase) == 0)
					{
						isLikely = new bool?(false);
					}
				}
			}
			bool? flag = isLikely;
			bool flag2 = true;
			char c = (flag.GetValueOrDefault() == flag2 & flag != null) ? '*' : ' ';
			string arg = new string(' ', level);
			sources.Add(string.Format(TypeConverterHelper.InvariantEnglishUS, "{0} {1} {2}", c, arg, type.FullName));
		}

		// Token: 0x06006D6B RID: 28011 RVA: 0x002CCDBB File Offset: 0x002CBDBB
		private void GetCollectionChangedSources(int level, Action<int, object, bool?, List<string>> format, List<string> sources)
		{
			format(level, this, new bool?(false), sources);
			this.Host.View.GetCollectionChangedSources(level + 1, format, sources);
		}

		// Token: 0x06006D6C RID: 28012 RVA: 0x002CCDE4 File Offset: 0x002CBDE4
		internal void ChangeAlternationCount()
		{
			if (this._itemMap == null)
			{
				return;
			}
			this.SetAlternationCount();
			if (this.IsGrouping && this.GroupStyle != null)
			{
				for (ItemContainerGenerator.ItemBlock next = this._itemMap.Next; next != this._itemMap; next = next.Next)
				{
					for (int i = 0; i < next.ContainerCount; i++)
					{
						GroupItem groupItem = ((ItemContainerGenerator.RealizedItemBlock)next).ContainerAt(i) as GroupItem;
						if (groupItem != null)
						{
							groupItem.Generator.ChangeAlternationCount();
						}
					}
				}
			}
		}

		// Token: 0x06006D6D RID: 28013 RVA: 0x002CCE60 File Offset: 0x002CBE60
		private void ChangeAlternationCount(int newAlternationCount)
		{
			if (this._alternationCount == newAlternationCount)
			{
				return;
			}
			ItemContainerGenerator.ItemBlock next = this._itemMap.Next;
			int i = 0;
			while (i == next.ContainerCount)
			{
				next = next.Next;
			}
			if (next != this._itemMap)
			{
				if (newAlternationCount > 0)
				{
					this._alternationCount = newAlternationCount;
					this.SetAlternationIndex((ItemContainerGenerator.RealizedItemBlock)next, i, GeneratorDirection.Forward);
				}
				else if (this._alternationCount > 0)
				{
					while (next != this._itemMap)
					{
						for (i = 0; i < next.ContainerCount; i++)
						{
							ItemsControl.ClearAlternationIndex(((ItemContainerGenerator.RealizedItemBlock)next).ContainerAt(i));
						}
						next = next.Next;
					}
				}
			}
			this._alternationCount = newAlternationCount;
		}

		// Token: 0x17001957 RID: 6487
		// (get) Token: 0x06006D6E RID: 28014 RVA: 0x002CCEFF File Offset: 0x002CBEFF
		internal ItemContainerGenerator Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x17001958 RID: 6488
		// (get) Token: 0x06006D6F RID: 28015 RVA: 0x002CCF07 File Offset: 0x002CBF07
		internal int Level
		{
			get
			{
				return this._level;
			}
		}

		// Token: 0x17001959 RID: 6489
		// (get) Token: 0x06006D70 RID: 28016 RVA: 0x002CCF0F File Offset: 0x002CBF0F
		// (set) Token: 0x06006D71 RID: 28017 RVA: 0x002CCF18 File Offset: 0x002CBF18
		internal GroupStyle GroupStyle
		{
			get
			{
				return this._groupStyle;
			}
			set
			{
				if (this._groupStyle != value)
				{
					if (this._groupStyle != null)
					{
						PropertyChangedEventManager.RemoveHandler(this._groupStyle, new EventHandler<PropertyChangedEventArgs>(this.OnGroupStylePropertyChanged), string.Empty);
					}
					this._groupStyle = value;
					if (this._groupStyle != null)
					{
						PropertyChangedEventManager.AddHandler(this._groupStyle, new EventHandler<PropertyChangedEventArgs>(this.OnGroupStylePropertyChanged), string.Empty);
					}
				}
			}
		}

		// Token: 0x1700195A RID: 6490
		// (get) Token: 0x06006D72 RID: 28018 RVA: 0x002CCF7D File Offset: 0x002CBF7D
		// (set) Token: 0x06006D73 RID: 28019 RVA: 0x002CCF88 File Offset: 0x002CBF88
		internal IList ItemsInternal
		{
			get
			{
				return this._items;
			}
			set
			{
				if (this._items != value)
				{
					INotifyCollectionChanged notifyCollectionChanged = this._items as INotifyCollectionChanged;
					if (this._items != this.Host.View && notifyCollectionChanged != null)
					{
						CollectionChangedEventManager.RemoveHandler(notifyCollectionChanged, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnCollectionChanged));
					}
					this._items = value;
					this._itemsReadOnly = null;
					notifyCollectionChanged = (this._items as INotifyCollectionChanged);
					if (this._items != this.Host.View && notifyCollectionChanged != null)
					{
						CollectionChangedEventManager.AddHandler(notifyCollectionChanged, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnCollectionChanged));
					}
				}
			}
		}

		// Token: 0x14000131 RID: 305
		// (add) Token: 0x06006D74 RID: 28020 RVA: 0x002CD014 File Offset: 0x002CC014
		// (remove) Token: 0x06006D75 RID: 28021 RVA: 0x002CD04C File Offset: 0x002CC04C
		internal event EventHandler PanelChanged;

		// Token: 0x06006D76 RID: 28022 RVA: 0x002CD081 File Offset: 0x002CC081
		internal void OnPanelChanged()
		{
			if (this.PanelChanged != null)
			{
				this.PanelChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x1700195B RID: 6491
		// (get) Token: 0x06006D77 RID: 28023 RVA: 0x002CD09C File Offset: 0x002CC09C
		private IGeneratorHost Host
		{
			get
			{
				return this._host;
			}
		}

		// Token: 0x1700195C RID: 6492
		// (get) Token: 0x06006D78 RID: 28024 RVA: 0x002CD0A4 File Offset: 0x002CC0A4
		private DependencyObject Peer
		{
			get
			{
				return this._peer;
			}
		}

		// Token: 0x1700195D RID: 6493
		// (get) Token: 0x06006D79 RID: 28025 RVA: 0x002CD0AC File Offset: 0x002CC0AC
		private bool IsGrouping
		{
			get
			{
				return this.ItemsInternal != this.Host.View;
			}
		}

		// Token: 0x06006D7A RID: 28026 RVA: 0x002CD0C4 File Offset: 0x002CC0C4
		private void MoveToPosition(GeneratorPosition position, GeneratorDirection direction, bool allowStartAtRealizedItem, ref ItemContainerGenerator.GeneratorState state)
		{
			ItemContainerGenerator.ItemBlock itemBlock = this._itemMap;
			if (itemBlock == null)
			{
				return;
			}
			int num = 0;
			if (position.Index != -1)
			{
				int num2 = 0;
				int i = position.Index;
				itemBlock = itemBlock.Next;
				while (i >= itemBlock.ContainerCount)
				{
					num2 += itemBlock.ItemCount;
					i -= itemBlock.ContainerCount;
					num += itemBlock.ItemCount;
					itemBlock = itemBlock.Next;
				}
				state.Block = itemBlock;
				state.Offset = i;
				state.Count = num2;
				state.ItemIndex = num + i;
			}
			else
			{
				state.Block = itemBlock;
				state.Offset = 0;
				state.Count = 0;
				state.ItemIndex = num - 1;
			}
			int j = position.Offset;
			if (j == 0 && (!allowStartAtRealizedItem || state.Block == this._itemMap))
			{
				j = ((direction == GeneratorDirection.Forward) ? 1 : -1);
			}
			if (j > 0)
			{
				state.Block.MoveForward(ref state, true);
				for (j--; j > 0; j -= state.Block.MoveForward(ref state, allowStartAtRealizedItem, j))
				{
				}
				return;
			}
			if (j < 0)
			{
				if (state.Block == this._itemMap)
				{
					state.ItemIndex = (state.Count = this.ItemsInternal.Count);
				}
				state.Block.MoveBackward(ref state, true);
				for (j++; j < 0; j += state.Block.MoveBackward(ref state, allowStartAtRealizedItem, -j))
				{
				}
			}
		}

		// Token: 0x06006D7B RID: 28027 RVA: 0x002CD228 File Offset: 0x002CC228
		private void Realize(ItemContainerGenerator.UnrealizedItemBlock block, int offset, object item, DependencyObject container)
		{
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock;
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock2;
			int num;
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock3;
			if (offset == 0 && (realizedItemBlock = (block.Prev as ItemContainerGenerator.RealizedItemBlock)) != null && realizedItemBlock.ItemCount < 16)
			{
				realizedItemBlock2 = realizedItemBlock;
				num = realizedItemBlock.ItemCount;
				this.MoveItems(block, offset, 1, realizedItemBlock2, num, -realizedItemBlock.ItemCount);
				this.MoveItems(block, 1, block.ItemCount, block, 0, 1);
			}
			else if (offset == block.ItemCount - 1 && (realizedItemBlock3 = (block.Next as ItemContainerGenerator.RealizedItemBlock)) != null && realizedItemBlock3.ItemCount < 16)
			{
				realizedItemBlock2 = realizedItemBlock3;
				num = 0;
				this.MoveItems(realizedItemBlock2, 0, realizedItemBlock2.ItemCount, realizedItemBlock2, 1, -1);
				this.MoveItems(block, offset, 1, realizedItemBlock2, num, offset);
			}
			else
			{
				realizedItemBlock2 = new ItemContainerGenerator.RealizedItemBlock();
				num = 0;
				if (offset == 0)
				{
					realizedItemBlock2.InsertBefore(block);
					this.MoveItems(block, offset, 1, realizedItemBlock2, num, 0);
					this.MoveItems(block, 1, block.ItemCount, block, 0, 1);
				}
				else if (offset == block.ItemCount - 1)
				{
					realizedItemBlock2.InsertAfter(block);
					this.MoveItems(block, offset, 1, realizedItemBlock2, num, offset);
				}
				else
				{
					ItemContainerGenerator.UnrealizedItemBlock unrealizedItemBlock = new ItemContainerGenerator.UnrealizedItemBlock();
					unrealizedItemBlock.InsertAfter(block);
					realizedItemBlock2.InsertAfter(block);
					this.MoveItems(block, offset + 1, block.ItemCount - offset - 1, unrealizedItemBlock, 0, offset + 1);
					this.MoveItems(block, offset, 1, realizedItemBlock2, 0, offset);
				}
			}
			this.RemoveAndCoalesceBlocksIfNeeded(block);
			realizedItemBlock2.RealizeItem(num, item, container);
		}

		// Token: 0x06006D7C RID: 28028 RVA: 0x002CD36C File Offset: 0x002CC36C
		private void RemoveAndCoalesceBlocksIfNeeded(ItemContainerGenerator.ItemBlock block)
		{
			if (block != null && block != this._itemMap && block.ItemCount == 0)
			{
				block.Remove();
				if (block.Prev is ItemContainerGenerator.UnrealizedItemBlock && block.Next is ItemContainerGenerator.UnrealizedItemBlock)
				{
					this.MoveItems(block.Next, 0, block.Next.ItemCount, block.Prev, block.Prev.ItemCount, -block.Prev.ItemCount - 1);
					block.Next.Remove();
				}
			}
		}

		// Token: 0x06006D7D RID: 28029 RVA: 0x002CD3F0 File Offset: 0x002CC3F0
		private void MoveItems(ItemContainerGenerator.ItemBlock block, int offset, int count, ItemContainerGenerator.ItemBlock newBlock, int newOffset, int deltaCount)
		{
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock = block as ItemContainerGenerator.RealizedItemBlock;
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock2 = newBlock as ItemContainerGenerator.RealizedItemBlock;
			if (realizedItemBlock != null && realizedItemBlock2 != null)
			{
				realizedItemBlock2.CopyEntries(realizedItemBlock, offset, count, newOffset);
			}
			else if (realizedItemBlock != null && realizedItemBlock.ItemCount > count)
			{
				realizedItemBlock.ClearEntries(offset, count);
			}
			block.ItemCount -= count;
			newBlock.ItemCount += count;
			if (this.MapChanged != null)
			{
				this.MapChanged(block, offset, count, newBlock, newOffset, deltaCount);
			}
		}

		// Token: 0x06006D7E RID: 28030 RVA: 0x002CD46C File Offset: 0x002CC46C
		private void SetAlternationIndex(ItemContainerGenerator.ItemBlock block, int offset, GeneratorDirection direction)
		{
			if (this._alternationCount <= 0)
			{
				return;
			}
			if (direction != GeneratorDirection.Backward)
			{
				offset--;
				while (offset < 0 || block is ItemContainerGenerator.UnrealizedItemBlock)
				{
					block = block.Prev;
					offset = block.ContainerCount - 1;
				}
				ItemContainerGenerator.RealizedItemBlock realizedItemBlock = block as ItemContainerGenerator.RealizedItemBlock;
				int num = (block == this._itemMap) ? -1 : ItemsControl.GetAlternationIndex(realizedItemBlock.ContainerAt(offset));
				for (;;)
				{
					for (offset++; offset == block.ContainerCount; offset = 0)
					{
						block = block.Next;
					}
					if (block == this._itemMap)
					{
						break;
					}
					num = (num + 1) % this._alternationCount;
					realizedItemBlock = (block as ItemContainerGenerator.RealizedItemBlock);
					ItemsControl.SetAlternationIndex(realizedItemBlock.ContainerAt(offset), num);
				}
			}
			else
			{
				offset++;
				while (offset >= block.ContainerCount || block is ItemContainerGenerator.UnrealizedItemBlock)
				{
					block = block.Next;
					offset = 0;
				}
				ItemContainerGenerator.RealizedItemBlock realizedItemBlock = block as ItemContainerGenerator.RealizedItemBlock;
				int num = (block == this._itemMap) ? 1 : ItemsControl.GetAlternationIndex(realizedItemBlock.ContainerAt(offset));
				for (;;)
				{
					for (offset--; offset < 0; offset = block.ContainerCount - 1)
					{
						block = block.Prev;
					}
					if (block == this._itemMap)
					{
						break;
					}
					num = (this._alternationCount + num - 1) % this._alternationCount;
					realizedItemBlock = (block as ItemContainerGenerator.RealizedItemBlock);
					ItemsControl.SetAlternationIndex(realizedItemBlock.ContainerAt(offset), num);
				}
			}
		}

		// Token: 0x06006D7F RID: 28031 RVA: 0x002CD5A8 File Offset: 0x002CC5A8
		private DependencyObject ContainerForGroup(CollectionViewGroup group)
		{
			this._generatesGroupItems = true;
			if (!this.ShouldHide(group))
			{
				GroupItem groupItem = new GroupItem();
				ItemContainerGenerator.LinkContainerToItem(groupItem, group);
				groupItem.Generator = new ItemContainerGenerator(this, groupItem);
				return groupItem;
			}
			this.AddEmptyGroupItem(group);
			return null;
		}

		// Token: 0x06006D80 RID: 28032 RVA: 0x002CD5EC File Offset: 0x002CC5EC
		private void PrepareGrouping()
		{
			GroupStyle groupStyle;
			IList list;
			if (this.Level == 0)
			{
				groupStyle = this.Host.GetGroupStyle(null, 0);
				if (groupStyle == null)
				{
					list = this.Host.View;
				}
				else
				{
					CollectionView collectionView = this.Host.View.CollectionView;
					list = ((collectionView == null) ? null : collectionView.Groups);
					if (list == null)
					{
						list = this.Host.View;
						if (list.Count > 0)
						{
							groupStyle = null;
						}
					}
				}
			}
			else
			{
				CollectionViewGroup collectionViewGroup = ((GroupItem)this.Peer).ReadLocalValue(ItemContainerGenerator.ItemForItemContainerProperty) as CollectionViewGroup;
				if (collectionViewGroup != null)
				{
					if (collectionViewGroup.IsBottomLevel)
					{
						groupStyle = null;
					}
					else
					{
						groupStyle = this.Host.GetGroupStyle(collectionViewGroup, this.Level);
					}
					list = collectionViewGroup.Items;
				}
				else
				{
					groupStyle = null;
					list = this.Host.View;
				}
			}
			this.GroupStyle = groupStyle;
			this.ItemsInternal = list;
			if (this.Level == 0 && this.Host != null)
			{
				this.Host.SetIsGrouping(this.IsGrouping);
			}
		}

		// Token: 0x06006D81 RID: 28033 RVA: 0x002CD6E0 File Offset: 0x002CC6E0
		private void SetAlternationCount()
		{
			int alternationCount;
			if (this.IsGrouping && this.GroupStyle != null)
			{
				if (this.GroupStyle.IsAlternationCountSet)
				{
					alternationCount = this.GroupStyle.AlternationCount;
				}
				else if (this._parent != null)
				{
					alternationCount = this._parent._alternationCount;
				}
				else
				{
					alternationCount = this.Host.AlternationCount;
				}
			}
			else
			{
				alternationCount = this.Host.AlternationCount;
			}
			this.ChangeAlternationCount(alternationCount);
		}

		// Token: 0x06006D82 RID: 28034 RVA: 0x002CD74F File Offset: 0x002CC74F
		private bool ShouldHide(CollectionViewGroup group)
		{
			return this.GroupStyle.HidesIfEmpty && group.ItemCount == 0;
		}

		// Token: 0x06006D83 RID: 28035 RVA: 0x002CD76C File Offset: 0x002CC76C
		private void AddEmptyGroupItem(CollectionViewGroup group)
		{
			ItemContainerGenerator.EmptyGroupItem emptyGroupItem = new ItemContainerGenerator.EmptyGroupItem();
			ItemContainerGenerator.LinkContainerToItem(emptyGroupItem, group);
			emptyGroupItem.SetGenerator(new ItemContainerGenerator(this, emptyGroupItem));
			if (this._emptyGroupItems == null)
			{
				this._emptyGroupItems = new ArrayList();
			}
			this._emptyGroupItems.Add(emptyGroupItem);
		}

		// Token: 0x06006D84 RID: 28036 RVA: 0x002CD7B4 File Offset: 0x002CC7B4
		private void OnSubgroupBecameNonEmpty(ItemContainerGenerator.EmptyGroupItem groupItem, CollectionViewGroup group)
		{
			this.UnlinkContainerFromItem(groupItem, group);
			if (this._emptyGroupItems != null)
			{
				this._emptyGroupItems.Remove(groupItem);
			}
			if (this.ItemsChanged != null)
			{
				GeneratorPosition position = this.PositionFromIndex(this.ItemsInternal.IndexOf(group));
				this.ItemsChanged(this, new ItemsChangedEventArgs(NotifyCollectionChangedAction.Add, position, 1, 0));
			}
		}

		// Token: 0x06006D85 RID: 28037 RVA: 0x002CD810 File Offset: 0x002CC810
		private void OnSubgroupBecameEmpty(CollectionViewGroup group)
		{
			if (this.ShouldHide(group))
			{
				GeneratorPosition position = this.PositionFromIndex(this.ItemsInternal.IndexOf(group));
				if (position.Offset == 0 && position.Index >= 0)
				{
					((IItemContainerGenerator)this).Remove(position, 1);
					if (this.ItemsChanged != null)
					{
						this.ItemsChanged(this, new ItemsChangedEventArgs(NotifyCollectionChangedAction.Remove, position, 1, 1));
					}
					this.AddEmptyGroupItem(group);
				}
			}
		}

		// Token: 0x06006D86 RID: 28038 RVA: 0x002CD878 File Offset: 0x002CC878
		private GeneratorPosition PositionFromIndex(int itemIndex)
		{
			GeneratorPosition result;
			ItemContainerGenerator.ItemBlock itemBlock;
			int num;
			this.GetBlockAndPosition(itemIndex, out result, out itemBlock, out num);
			return result;
		}

		// Token: 0x06006D87 RID: 28039 RVA: 0x002CD893 File Offset: 0x002CC893
		private void GetBlockAndPosition(object item, int itemIndex, bool deletedFromItems, out GeneratorPosition position, out ItemContainerGenerator.ItemBlock block, out int offsetFromBlockStart, out int correctIndex)
		{
			if (itemIndex >= 0)
			{
				this.GetBlockAndPosition(itemIndex, out position, out block, out offsetFromBlockStart);
				correctIndex = itemIndex;
				return;
			}
			this.GetBlockAndPosition(item, deletedFromItems, out position, out block, out offsetFromBlockStart, out correctIndex);
		}

		// Token: 0x06006D88 RID: 28040 RVA: 0x002CD8BC File Offset: 0x002CC8BC
		private void GetBlockAndPosition(int itemIndex, out GeneratorPosition position, out ItemContainerGenerator.ItemBlock block, out int offsetFromBlockStart)
		{
			position = new GeneratorPosition(-1, 0);
			block = null;
			offsetFromBlockStart = itemIndex;
			if (this._itemMap == null || itemIndex < 0)
			{
				return;
			}
			int num = 0;
			block = this._itemMap.Next;
			while (block != this._itemMap)
			{
				if (offsetFromBlockStart >= block.ItemCount)
				{
					num += block.ContainerCount;
					offsetFromBlockStart -= block.ItemCount;
					block = block.Next;
				}
				else
				{
					if (block.ContainerCount > 0)
					{
						position = new GeneratorPosition(num + offsetFromBlockStart, 0);
						return;
					}
					position = new GeneratorPosition(num - 1, offsetFromBlockStart + 1);
					return;
				}
			}
		}

		// Token: 0x06006D89 RID: 28041 RVA: 0x002CD964 File Offset: 0x002CC964
		private void GetBlockAndPosition(object item, bool deletedFromItems, out GeneratorPosition position, out ItemContainerGenerator.ItemBlock block, out int offsetFromBlockStart, out int correctIndex)
		{
			correctIndex = 0;
			int num = 0;
			offsetFromBlockStart = 0;
			int num2 = deletedFromItems ? 1 : 0;
			position = new GeneratorPosition(-1, 0);
			if (this._itemMap == null)
			{
				block = null;
				return;
			}
			for (block = this._itemMap.Next; block != this._itemMap; block = block.Next)
			{
				ItemContainerGenerator.RealizedItemBlock realizedItemBlock = block as ItemContainerGenerator.RealizedItemBlock;
				if (realizedItemBlock != null)
				{
					offsetFromBlockStart = realizedItemBlock.OffsetOfItem(item);
					if (offsetFromBlockStart >= 0)
					{
						position = new GeneratorPosition(num + offsetFromBlockStart, 0);
						correctIndex += offsetFromBlockStart;
						break;
					}
				}
				else if (block is ItemContainerGenerator.UnrealizedItemBlock)
				{
					bool flag = false;
					realizedItemBlock = (block.Next as ItemContainerGenerator.RealizedItemBlock);
					if (realizedItemBlock != null && realizedItemBlock.ContainerCount > 0)
					{
						flag = ItemsControl.EqualsEx(realizedItemBlock.ItemAt(0), this.ItemsInternal[correctIndex + block.ItemCount - num2]);
					}
					else if (block.Next == this._itemMap)
					{
						flag = (block.Prev == this._itemMap || this.ItemsInternal.Count == correctIndex + block.ItemCount - num2);
					}
					if (flag)
					{
						offsetFromBlockStart = 0;
						position = new GeneratorPosition(num - 1, 1);
						break;
					}
				}
				correctIndex += block.ItemCount;
				num += block.ContainerCount;
			}
			if (block == this._itemMap)
			{
				throw new InvalidOperationException(SR.Get("CannotFindRemovedItem"));
			}
		}

		// Token: 0x06006D8A RID: 28042 RVA: 0x002CDAE5 File Offset: 0x002CCAE5
		internal static void LinkContainerToItem(DependencyObject container, object item)
		{
			container.ClearValue(ItemContainerGenerator.ItemForItemContainerProperty);
			container.SetValue(ItemContainerGenerator.ItemForItemContainerProperty, item);
			if (container != item)
			{
				container.SetValue(FrameworkElement.DataContextProperty, item);
			}
		}

		// Token: 0x06006D8B RID: 28043 RVA: 0x002CDB0E File Offset: 0x002CCB0E
		private void UnlinkContainerFromItem(DependencyObject container, object item)
		{
			ItemContainerGenerator.UnlinkContainerFromItem(container, item, this._host);
		}

		// Token: 0x06006D8C RID: 28044 RVA: 0x002CDB20 File Offset: 0x002CCB20
		internal static void UnlinkContainerFromItem(DependencyObject container, object item, IGeneratorHost host)
		{
			container.ClearValue(ItemContainerGenerator.ItemForItemContainerProperty);
			host.ClearContainerForItem(container, item);
			if (container != item)
			{
				DependencyProperty dataContextProperty = FrameworkElement.DataContextProperty;
				container.SetValue(dataContextProperty, BindingExpressionBase.DisconnectedItem);
			}
		}

		// Token: 0x06006D8D RID: 28045 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x06006D8E RID: 28046 RVA: 0x002CDB56 File Offset: 0x002CCB56
		private void OnGroupStylePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Panel")
			{
				this.OnPanelChanged();
				return;
			}
			this.OnRefresh();
		}

		// Token: 0x06006D8F RID: 28047 RVA: 0x002CDB77 File Offset: 0x002CCB77
		private void ValidateAndCorrectIndex(object item, ref int index)
		{
			if (index < 0)
			{
				index = this.ItemsInternal.IndexOf(item);
				if (index < 0)
				{
					throw new InvalidOperationException(SR.Get("CollectionAddEventMissingItem", new object[]
					{
						item
					}));
				}
			}
		}

		// Token: 0x06006D90 RID: 28048 RVA: 0x002CDBAC File Offset: 0x002CCBAC
		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if (sender != this.ItemsInternal && args.Action != NotifyCollectionChangedAction.Reset)
			{
				return;
			}
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (args.NewItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				this.OnItemAdded(args.NewItems[0], args.NewStartingIndex);
				break;
			case NotifyCollectionChangedAction.Remove:
				if (args.OldItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				this.OnItemRemoved(args.OldItems[0], args.OldStartingIndex);
				break;
			case NotifyCollectionChangedAction.Replace:
				if (!FrameworkCompatibilityPreferences.TargetsDesktop_V4_0 && args.OldItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				this.OnItemReplaced(args.OldItems[0], args.NewItems[0], args.NewStartingIndex);
				break;
			case NotifyCollectionChangedAction.Move:
				if (!FrameworkCompatibilityPreferences.TargetsDesktop_V4_0 && args.OldItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				this.OnItemMoved(args.OldItems[0], args.OldStartingIndex, args.NewStartingIndex);
				break;
			case NotifyCollectionChangedAction.Reset:
				this.OnRefresh();
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					args.Action
				}));
			}
			if (PresentationTraceSources.GetTraceLevel(this) >= PresentationTraceLevel.High)
			{
				this.Verify();
			}
		}

		// Token: 0x06006D91 RID: 28049 RVA: 0x002CDD34 File Offset: 0x002CCD34
		private void OnItemAdded(object item, int index)
		{
			if (this._itemMap == null)
			{
				return;
			}
			this.ValidateAndCorrectIndex(item, ref index);
			GeneratorPosition position = new GeneratorPosition(-1, 0);
			ItemContainerGenerator.ItemBlock itemBlock = this._itemMap.Next;
			int num = index;
			int num2 = 0;
			while (itemBlock != this._itemMap && num >= itemBlock.ItemCount)
			{
				num -= itemBlock.ItemCount;
				position.Index += itemBlock.ContainerCount;
				num2 = ((itemBlock.ContainerCount > 0) ? 0 : (num2 + itemBlock.ItemCount));
				itemBlock = itemBlock.Next;
			}
			position.Offset = num2 + num + 1;
			ItemContainerGenerator.UnrealizedItemBlock unrealizedItemBlock = itemBlock as ItemContainerGenerator.UnrealizedItemBlock;
			if (unrealizedItemBlock != null)
			{
				this.MoveItems(unrealizedItemBlock, num, 1, unrealizedItemBlock, num + 1, 0);
				ItemContainerGenerator.UnrealizedItemBlock unrealizedItemBlock2 = unrealizedItemBlock;
				int itemCount = unrealizedItemBlock2.ItemCount + 1;
				unrealizedItemBlock2.ItemCount = itemCount;
			}
			else if ((num == 0 || itemBlock == this._itemMap) && (unrealizedItemBlock = (itemBlock.Prev as ItemContainerGenerator.UnrealizedItemBlock)) != null)
			{
				ItemContainerGenerator.UnrealizedItemBlock unrealizedItemBlock3 = unrealizedItemBlock;
				int itemCount = unrealizedItemBlock3.ItemCount + 1;
				unrealizedItemBlock3.ItemCount = itemCount;
			}
			else
			{
				unrealizedItemBlock = new ItemContainerGenerator.UnrealizedItemBlock();
				unrealizedItemBlock.ItemCount = 1;
				ItemContainerGenerator.RealizedItemBlock realizedItemBlock;
				if (num > 0 && (realizedItemBlock = (itemBlock as ItemContainerGenerator.RealizedItemBlock)) != null)
				{
					ItemContainerGenerator.RealizedItemBlock realizedItemBlock2 = new ItemContainerGenerator.RealizedItemBlock();
					this.MoveItems(realizedItemBlock, num, realizedItemBlock.ItemCount - num, realizedItemBlock2, 0, num);
					realizedItemBlock2.InsertAfter(realizedItemBlock);
					position.Index += itemBlock.ContainerCount;
					position.Offset = 1;
					itemBlock = realizedItemBlock2;
				}
				unrealizedItemBlock.InsertBefore(itemBlock);
			}
			if (this.MapChanged != null)
			{
				this.MapChanged(null, index, 1, unrealizedItemBlock, 0, 0);
			}
			if (this.ItemsChanged != null)
			{
				this.ItemsChanged(this, new ItemsChangedEventArgs(NotifyCollectionChangedAction.Add, position, 1, 0));
			}
		}

		// Token: 0x06006D92 RID: 28050 RVA: 0x002CDECC File Offset: 0x002CCECC
		private void OnItemRemoved(object item, int itemIndex)
		{
			DependencyObject dependencyObject = null;
			int itemUICount = 0;
			GeneratorPosition position;
			ItemContainerGenerator.ItemBlock itemBlock;
			int num;
			int num2;
			this.GetBlockAndPosition(item, itemIndex, true, out position, out itemBlock, out num, out num2);
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock = itemBlock as ItemContainerGenerator.RealizedItemBlock;
			if (realizedItemBlock != null)
			{
				itemUICount = 1;
				dependencyObject = realizedItemBlock.ContainerAt(num);
			}
			this.MoveItems(itemBlock, num + 1, itemBlock.ItemCount - num - 1, itemBlock, num, 0);
			ItemContainerGenerator.ItemBlock itemBlock2 = itemBlock;
			int itemCount = itemBlock2.ItemCount - 1;
			itemBlock2.ItemCount = itemCount;
			if (realizedItemBlock != null)
			{
				this.SetAlternationIndex(itemBlock, num, GeneratorDirection.Forward);
			}
			this.RemoveAndCoalesceBlocksIfNeeded(itemBlock);
			if (this.MapChanged != null)
			{
				this.MapChanged(null, itemIndex, -1, null, 0, 0);
			}
			if (this.ItemsChanged != null)
			{
				this.ItemsChanged(this, new ItemsChangedEventArgs(NotifyCollectionChangedAction.Remove, position, 1, itemUICount));
			}
			if (dependencyObject != null)
			{
				this.UnlinkContainerFromItem(dependencyObject, item);
			}
			if (this.Level > 0 && this.ItemsInternal.Count == 0)
			{
				CollectionViewGroup collectionViewGroup = ((GroupItem)this.Peer).ReadLocalValue(ItemContainerGenerator.ItemForItemContainerProperty) as CollectionViewGroup;
				if (collectionViewGroup != null)
				{
					this.Parent.OnSubgroupBecameEmpty(collectionViewGroup);
				}
			}
		}

		// Token: 0x06006D93 RID: 28051 RVA: 0x002CDFCC File Offset: 0x002CCFCC
		private void OnItemReplaced(object oldItem, object newItem, int index)
		{
			GeneratorPosition position;
			ItemContainerGenerator.ItemBlock itemBlock;
			int index2;
			int num;
			this.GetBlockAndPosition(oldItem, index, false, out position, out itemBlock, out index2, out num);
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock = itemBlock as ItemContainerGenerator.RealizedItemBlock;
			if (realizedItemBlock != null)
			{
				DependencyObject dependencyObject = realizedItemBlock.ContainerAt(index2);
				if (oldItem != dependencyObject && !this._host.IsItemItsOwnContainer(newItem))
				{
					realizedItemBlock.RealizeItem(index2, newItem, dependencyObject);
					ItemContainerGenerator.LinkContainerToItem(dependencyObject, newItem);
					this._host.PrepareItemContainer(dependencyObject, newItem);
					return;
				}
				DependencyObject containerForItem = this._host.GetContainerForItem(newItem);
				realizedItemBlock.RealizeItem(index2, newItem, containerForItem);
				ItemContainerGenerator.LinkContainerToItem(containerForItem, newItem);
				if (this.ItemsChanged != null)
				{
					this.ItemsChanged(this, new ItemsChangedEventArgs(NotifyCollectionChangedAction.Replace, position, 1, 1));
				}
				this.UnlinkContainerFromItem(dependencyObject, oldItem);
			}
		}

		// Token: 0x06006D94 RID: 28052 RVA: 0x002CE080 File Offset: 0x002CD080
		private void OnItemMoved(object item, int oldIndex, int newIndex)
		{
			if (this._itemMap == null)
			{
				return;
			}
			DependencyObject dependencyObject = null;
			int itemUICount = 0;
			GeneratorPosition generatorPosition;
			ItemContainerGenerator.ItemBlock itemBlock;
			int num;
			int num2;
			this.GetBlockAndPosition(item, oldIndex, true, out generatorPosition, out itemBlock, out num, out num2);
			GeneratorPosition oldPosition = generatorPosition;
			ItemContainerGenerator.RealizedItemBlock realizedItemBlock = itemBlock as ItemContainerGenerator.RealizedItemBlock;
			if (realizedItemBlock != null)
			{
				itemUICount = 1;
				dependencyObject = realizedItemBlock.ContainerAt(num);
			}
			this.MoveItems(itemBlock, num + 1, itemBlock.ItemCount - num - 1, itemBlock, num, 0);
			ItemContainerGenerator.ItemBlock itemBlock2 = itemBlock;
			int itemCount = itemBlock2.ItemCount - 1;
			itemBlock2.ItemCount = itemCount;
			this.RemoveAndCoalesceBlocksIfNeeded(itemBlock);
			generatorPosition = new GeneratorPosition(-1, 0);
			itemBlock = this._itemMap.Next;
			num = newIndex;
			while (itemBlock != this._itemMap && num >= itemBlock.ItemCount)
			{
				num -= itemBlock.ItemCount;
				if (itemBlock.ContainerCount > 0)
				{
					generatorPosition.Index += itemBlock.ContainerCount;
					generatorPosition.Offset = 0;
				}
				else
				{
					generatorPosition.Offset += itemBlock.ItemCount;
				}
				itemBlock = itemBlock.Next;
			}
			generatorPosition.Offset += num + 1;
			ItemContainerGenerator.UnrealizedItemBlock unrealizedItemBlock = itemBlock as ItemContainerGenerator.UnrealizedItemBlock;
			if (unrealizedItemBlock != null)
			{
				this.MoveItems(unrealizedItemBlock, num, 1, unrealizedItemBlock, num + 1, 0);
				ItemContainerGenerator.UnrealizedItemBlock unrealizedItemBlock2 = unrealizedItemBlock;
				itemCount = unrealizedItemBlock2.ItemCount + 1;
				unrealizedItemBlock2.ItemCount = itemCount;
			}
			else if ((num == 0 || itemBlock == this._itemMap) && (unrealizedItemBlock = (itemBlock.Prev as ItemContainerGenerator.UnrealizedItemBlock)) != null)
			{
				ItemContainerGenerator.UnrealizedItemBlock unrealizedItemBlock3 = unrealizedItemBlock;
				itemCount = unrealizedItemBlock3.ItemCount + 1;
				unrealizedItemBlock3.ItemCount = itemCount;
			}
			else
			{
				unrealizedItemBlock = new ItemContainerGenerator.UnrealizedItemBlock();
				unrealizedItemBlock.ItemCount = 1;
				if (num > 0 && (realizedItemBlock = (itemBlock as ItemContainerGenerator.RealizedItemBlock)) != null)
				{
					ItemContainerGenerator.RealizedItemBlock realizedItemBlock2 = new ItemContainerGenerator.RealizedItemBlock();
					this.MoveItems(realizedItemBlock, num, realizedItemBlock.ItemCount - num, realizedItemBlock2, 0, num);
					realizedItemBlock2.InsertAfter(realizedItemBlock);
					generatorPosition.Index += itemBlock.ContainerCount;
					generatorPosition.Offset = 1;
					num = 0;
					itemBlock = realizedItemBlock2;
				}
				unrealizedItemBlock.InsertBefore(itemBlock);
			}
			DependencyObject parentInternal = VisualTreeHelper.GetParentInternal(dependencyObject);
			if (this.ItemsChanged != null)
			{
				this.ItemsChanged(this, new ItemsChangedEventArgs(NotifyCollectionChangedAction.Move, generatorPosition, oldPosition, 1, itemUICount));
			}
			if (dependencyObject != null)
			{
				if (parentInternal == null || VisualTreeHelper.GetParentInternal(dependencyObject) != parentInternal)
				{
					this.UnlinkContainerFromItem(dependencyObject, item);
				}
				else
				{
					this.Realize(unrealizedItemBlock, num, item, dependencyObject);
				}
			}
			if (this._alternationCount > 0)
			{
				int itemIndex = Math.Min(oldIndex, newIndex);
				this.GetBlockAndPosition(itemIndex, out generatorPosition, out itemBlock, out num);
				this.SetAlternationIndex(itemBlock, num, GeneratorDirection.Forward);
			}
		}

		// Token: 0x06006D95 RID: 28053 RVA: 0x002CE2E4 File Offset: 0x002CD2E4
		private void OnRefresh()
		{
			((IItemContainerGenerator)this).RemoveAll();
			if (this.ItemsChanged != null)
			{
				GeneratorPosition position = new GeneratorPosition(0, 0);
				this.ItemsChanged(this, new ItemsChangedEventArgs(NotifyCollectionChangedAction.Reset, position, 0, 0));
			}
		}

		// Token: 0x14000132 RID: 306
		// (add) Token: 0x06006D96 RID: 28054 RVA: 0x002CE320 File Offset: 0x002CD320
		// (remove) Token: 0x06006D97 RID: 28055 RVA: 0x002CE358 File Offset: 0x002CD358
		private event ItemContainerGenerator.MapChangedHandler MapChanged;

		// Token: 0x0400361D RID: 13853
		internal static readonly DependencyProperty ItemForItemContainerProperty = DependencyProperty.RegisterAttached("ItemForItemContainer", typeof(object), typeof(ItemContainerGenerator), new FrameworkPropertyMetadata(null));

		// Token: 0x0400361F RID: 13855
		private ItemContainerGenerator.Generator _generator;

		// Token: 0x04003620 RID: 13856
		private IGeneratorHost _host;

		// Token: 0x04003621 RID: 13857
		private ItemContainerGenerator.ItemBlock _itemMap;

		// Token: 0x04003622 RID: 13858
		private GeneratorStatus _status;

		// Token: 0x04003623 RID: 13859
		private int _itemsGenerated;

		// Token: 0x04003624 RID: 13860
		private int _startIndexForUIFromItem;

		// Token: 0x04003625 RID: 13861
		private DependencyObject _peer;

		// Token: 0x04003626 RID: 13862
		private int _level;

		// Token: 0x04003627 RID: 13863
		private IList _items;

		// Token: 0x04003628 RID: 13864
		private ReadOnlyCollection<object> _itemsReadOnly;

		// Token: 0x04003629 RID: 13865
		private GroupStyle _groupStyle;

		// Token: 0x0400362A RID: 13866
		private ItemContainerGenerator _parent;

		// Token: 0x0400362B RID: 13867
		private ArrayList _emptyGroupItems;

		// Token: 0x0400362C RID: 13868
		private int _alternationCount;

		// Token: 0x0400362D RID: 13869
		private Type _containerType;

		// Token: 0x0400362E RID: 13870
		private Queue<DependencyObject> _recyclableContainers = new Queue<DependencyObject>();

		// Token: 0x0400362F RID: 13871
		private bool _generatesGroupItems;

		// Token: 0x04003630 RID: 13872
		private bool _isGeneratingBatches;

		// Token: 0x02000BFC RID: 3068
		private class Generator : IDisposable
		{
			// Token: 0x06008FFD RID: 36861 RVA: 0x0034594C File Offset: 0x0034494C
			internal Generator(ItemContainerGenerator factory, GeneratorPosition position, GeneratorDirection direction, bool allowStartAtRealizedItem)
			{
				this._factory = factory;
				this._direction = direction;
				this._factory.MapChanged += this.OnMapChanged;
				this._factory.MoveToPosition(position, direction, allowStartAtRealizedItem, ref this._cachedState);
				this._done = (this._factory.ItemsInternal.Count == 0);
				this._factory.SetStatus(GeneratorStatus.GeneratingContainers);
			}

			// Token: 0x06008FFE RID: 36862 RVA: 0x003459C0 File Offset: 0x003449C0
			public DependencyObject GenerateNext(bool stopAtRealized, out bool isNewlyRealized)
			{
				DependencyObject dependencyObject = null;
				isNewlyRealized = false;
				while (dependencyObject == null)
				{
					ItemContainerGenerator.UnrealizedItemBlock unrealizedItemBlock = this._cachedState.Block as ItemContainerGenerator.UnrealizedItemBlock;
					IList itemsInternal = this._factory.ItemsInternal;
					int itemIndex = this._cachedState.ItemIndex;
					GeneratorDirection direction = this._direction;
					if (this._cachedState.Block == this._factory._itemMap)
					{
						this._done = true;
					}
					if (unrealizedItemBlock == null && stopAtRealized)
					{
						this._done = true;
					}
					if (0 > itemIndex || itemIndex >= itemsInternal.Count)
					{
						this._done = true;
					}
					if (this._done)
					{
						isNewlyRealized = false;
						return null;
					}
					object obj = itemsInternal[itemIndex];
					if (unrealizedItemBlock != null)
					{
						isNewlyRealized = true;
						CollectionViewGroup collectionViewGroup = obj as CollectionViewGroup;
						bool flag = this._factory._generatesGroupItems && collectionViewGroup == null;
						if (this._factory._recyclableContainers.Count > 0 && !this._factory.Host.IsItemItsOwnContainer(obj) && !flag)
						{
							dependencyObject = this._factory._recyclableContainers.Dequeue();
							isNewlyRealized = false;
						}
						else if (collectionViewGroup == null || !this._factory.IsGrouping)
						{
							dependencyObject = this._factory.Host.GetContainerForItem(obj);
						}
						else
						{
							dependencyObject = this._factory.ContainerForGroup(collectionViewGroup);
						}
						if (dependencyObject != null)
						{
							ItemContainerGenerator.LinkContainerToItem(dependencyObject, obj);
							this._factory.Realize(unrealizedItemBlock, this._cachedState.Offset, obj, dependencyObject);
							this._factory.SetAlternationIndex(this._cachedState.Block, this._cachedState.Offset, this._direction);
						}
					}
					else
					{
						isNewlyRealized = false;
						dependencyObject = ((ItemContainerGenerator.RealizedItemBlock)this._cachedState.Block).ContainerAt(this._cachedState.Offset);
					}
					this._cachedState.ItemIndex = itemIndex;
					if (this._direction == GeneratorDirection.Forward)
					{
						this._cachedState.Block.MoveForward(ref this._cachedState, true);
					}
					else
					{
						this._cachedState.Block.MoveBackward(ref this._cachedState, true);
					}
				}
				return dependencyObject;
			}

			// Token: 0x06008FFF RID: 36863 RVA: 0x00345BBC File Offset: 0x00344BBC
			void IDisposable.Dispose()
			{
				if (this._factory != null)
				{
					this._factory.MapChanged -= this.OnMapChanged;
					this._done = true;
					if (!this._factory._isGeneratingBatches)
					{
						this._factory.SetStatus(GeneratorStatus.ContainersGenerated);
					}
					this._factory._generator = null;
					this._factory = null;
				}
				GC.SuppressFinalize(this);
			}

			// Token: 0x06009000 RID: 36864 RVA: 0x00345C24 File Offset: 0x00344C24
			private void OnMapChanged(ItemContainerGenerator.ItemBlock block, int offset, int count, ItemContainerGenerator.ItemBlock newBlock, int newOffset, int deltaCount)
			{
				if (block != null)
				{
					if (block == this._cachedState.Block && offset <= this._cachedState.Offset && this._cachedState.Offset < offset + count)
					{
						this._cachedState.Block = newBlock;
						this._cachedState.Offset = this._cachedState.Offset + (newOffset - offset);
						this._cachedState.Count = this._cachedState.Count + deltaCount;
						return;
					}
				}
				else if (offset >= 0)
				{
					if (offset < this._cachedState.Count || (offset == this._cachedState.Count && newBlock != null && newBlock != this._cachedState.Block))
					{
						this._cachedState.Count = this._cachedState.Count + count;
						this._cachedState.ItemIndex = this._cachedState.ItemIndex + count;
						return;
					}
					if (offset < this._cachedState.Count + this._cachedState.Offset)
					{
						this._cachedState.Offset = this._cachedState.Offset + count;
						this._cachedState.ItemIndex = this._cachedState.ItemIndex + count;
						return;
					}
					if (offset == this._cachedState.Count + this._cachedState.Offset)
					{
						if (count > 0)
						{
							this._cachedState.Offset = this._cachedState.Offset + count;
							this._cachedState.ItemIndex = this._cachedState.ItemIndex + count;
							return;
						}
						if (this._cachedState.Offset == this._cachedState.Block.ItemCount)
						{
							this._cachedState.Block = this._cachedState.Block.Next;
							this._cachedState.Offset = 0;
							return;
						}
					}
				}
				else
				{
					this._cachedState.Block = newBlock;
					this._cachedState.Offset = this._cachedState.Offset + this._cachedState.Count;
					this._cachedState.Count = 0;
				}
			}

			// Token: 0x04004AAA RID: 19114
			private ItemContainerGenerator _factory;

			// Token: 0x04004AAB RID: 19115
			private GeneratorDirection _direction;

			// Token: 0x04004AAC RID: 19116
			private bool _done;

			// Token: 0x04004AAD RID: 19117
			private ItemContainerGenerator.GeneratorState _cachedState;
		}

		// Token: 0x02000BFD RID: 3069
		private class BatchGenerator : IDisposable
		{
			// Token: 0x06009001 RID: 36865 RVA: 0x00345E02 File Offset: 0x00344E02
			public BatchGenerator(ItemContainerGenerator factory)
			{
				this._factory = factory;
				this._factory._isGeneratingBatches = true;
				this._factory.SetStatus(GeneratorStatus.GeneratingContainers);
			}

			// Token: 0x06009002 RID: 36866 RVA: 0x00345E29 File Offset: 0x00344E29
			void IDisposable.Dispose()
			{
				if (this._factory != null)
				{
					this._factory._isGeneratingBatches = false;
					this._factory.SetStatus(GeneratorStatus.ContainersGenerated);
					this._factory = null;
				}
				GC.SuppressFinalize(this);
			}

			// Token: 0x04004AAE RID: 19118
			private ItemContainerGenerator _factory;
		}

		// Token: 0x02000BFE RID: 3070
		// (Invoke) Token: 0x06009004 RID: 36868
		private delegate void MapChangedHandler(ItemContainerGenerator.ItemBlock block, int offset, int count, ItemContainerGenerator.ItemBlock newBlock, int newOffset, int deltaCount);

		// Token: 0x02000BFF RID: 3071
		private class ItemBlock
		{
			// Token: 0x17001F75 RID: 8053
			// (get) Token: 0x06009007 RID: 36871 RVA: 0x00345E58 File Offset: 0x00344E58
			// (set) Token: 0x06009008 RID: 36872 RVA: 0x00345E60 File Offset: 0x00344E60
			public int ItemCount
			{
				get
				{
					return this._count;
				}
				set
				{
					this._count = value;
				}
			}

			// Token: 0x17001F76 RID: 8054
			// (get) Token: 0x06009009 RID: 36873 RVA: 0x00345E69 File Offset: 0x00344E69
			// (set) Token: 0x0600900A RID: 36874 RVA: 0x00345E71 File Offset: 0x00344E71
			public ItemContainerGenerator.ItemBlock Prev
			{
				get
				{
					return this._prev;
				}
				set
				{
					this._prev = value;
				}
			}

			// Token: 0x17001F77 RID: 8055
			// (get) Token: 0x0600900B RID: 36875 RVA: 0x00345E7A File Offset: 0x00344E7A
			// (set) Token: 0x0600900C RID: 36876 RVA: 0x00345E82 File Offset: 0x00344E82
			public ItemContainerGenerator.ItemBlock Next
			{
				get
				{
					return this._next;
				}
				set
				{
					this._next = value;
				}
			}

			// Token: 0x17001F78 RID: 8056
			// (get) Token: 0x0600900D RID: 36877 RVA: 0x00345E8B File Offset: 0x00344E8B
			public virtual int ContainerCount
			{
				get
				{
					return int.MaxValue;
				}
			}

			// Token: 0x0600900E RID: 36878 RVA: 0x00109403 File Offset: 0x00108403
			public virtual DependencyObject ContainerAt(int index)
			{
				return null;
			}

			// Token: 0x0600900F RID: 36879 RVA: 0x00109403 File Offset: 0x00108403
			public virtual object ItemAt(int index)
			{
				return null;
			}

			// Token: 0x06009010 RID: 36880 RVA: 0x00345E92 File Offset: 0x00344E92
			public void InsertAfter(ItemContainerGenerator.ItemBlock prev)
			{
				this.Next = prev.Next;
				this.Prev = prev;
				this.Prev.Next = this;
				this.Next.Prev = this;
			}

			// Token: 0x06009011 RID: 36881 RVA: 0x00345EBF File Offset: 0x00344EBF
			public void InsertBefore(ItemContainerGenerator.ItemBlock next)
			{
				this.InsertAfter(next.Prev);
			}

			// Token: 0x06009012 RID: 36882 RVA: 0x00345ECD File Offset: 0x00344ECD
			public void Remove()
			{
				this.Prev.Next = this.Next;
				this.Next.Prev = this.Prev;
			}

			// Token: 0x06009013 RID: 36883 RVA: 0x00345EF4 File Offset: 0x00344EF4
			public void MoveForward(ref ItemContainerGenerator.GeneratorState state, bool allowMovePastRealizedItem)
			{
				if (this.IsMoveAllowed(allowMovePastRealizedItem))
				{
					state.ItemIndex++;
					int num = state.Offset + 1;
					state.Offset = num;
					if (num >= this.ItemCount)
					{
						state.Block = this.Next;
						state.Offset = 0;
						state.Count += this.ItemCount;
					}
				}
			}

			// Token: 0x06009014 RID: 36884 RVA: 0x00345F58 File Offset: 0x00344F58
			public void MoveBackward(ref ItemContainerGenerator.GeneratorState state, bool allowMovePastRealizedItem)
			{
				if (this.IsMoveAllowed(allowMovePastRealizedItem))
				{
					int num = state.Offset - 1;
					state.Offset = num;
					if (num < 0)
					{
						state.Block = this.Prev;
						state.Offset = state.Block.ItemCount - 1;
						state.Count -= state.Block.ItemCount;
					}
					state.ItemIndex--;
				}
			}

			// Token: 0x06009015 RID: 36885 RVA: 0x00345FC8 File Offset: 0x00344FC8
			public int MoveForward(ref ItemContainerGenerator.GeneratorState state, bool allowMovePastRealizedItem, int count)
			{
				if (this.IsMoveAllowed(allowMovePastRealizedItem))
				{
					if (count < this.ItemCount - state.Offset)
					{
						state.Offset += count;
					}
					else
					{
						count = this.ItemCount - state.Offset;
						state.Block = this.Next;
						state.Offset = 0;
						state.Count += this.ItemCount;
					}
					state.ItemIndex += count;
				}
				return count;
			}

			// Token: 0x06009016 RID: 36886 RVA: 0x00346044 File Offset: 0x00345044
			public int MoveBackward(ref ItemContainerGenerator.GeneratorState state, bool allowMovePastRealizedItem, int count)
			{
				if (this.IsMoveAllowed(allowMovePastRealizedItem))
				{
					if (count <= state.Offset)
					{
						state.Offset -= count;
					}
					else
					{
						count = state.Offset + 1;
						state.Block = this.Prev;
						state.Offset = state.Block.ItemCount - 1;
						state.Count -= state.Block.ItemCount;
					}
					state.ItemIndex -= count;
				}
				return count;
			}

			// Token: 0x06009017 RID: 36887 RVA: 0x001136C4 File Offset: 0x001126C4
			protected virtual bool IsMoveAllowed(bool allowMovePastRealizedItem)
			{
				return allowMovePastRealizedItem;
			}

			// Token: 0x04004AAF RID: 19119
			public const int BlockSize = 16;

			// Token: 0x04004AB0 RID: 19120
			private int _count;

			// Token: 0x04004AB1 RID: 19121
			private ItemContainerGenerator.ItemBlock _prev;

			// Token: 0x04004AB2 RID: 19122
			private ItemContainerGenerator.ItemBlock _next;
		}

		// Token: 0x02000C00 RID: 3072
		private class UnrealizedItemBlock : ItemContainerGenerator.ItemBlock
		{
			// Token: 0x17001F79 RID: 8057
			// (get) Token: 0x06009019 RID: 36889 RVA: 0x00105F35 File Offset: 0x00104F35
			public override int ContainerCount
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x0600901A RID: 36890 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
			protected override bool IsMoveAllowed(bool allowMovePastRealizedItem)
			{
				return true;
			}
		}

		// Token: 0x02000C01 RID: 3073
		private class RealizedItemBlock : ItemContainerGenerator.ItemBlock
		{
			// Token: 0x17001F7A RID: 8058
			// (get) Token: 0x0600901C RID: 36892 RVA: 0x003460CB File Offset: 0x003450CB
			public override int ContainerCount
			{
				get
				{
					return base.ItemCount;
				}
			}

			// Token: 0x0600901D RID: 36893 RVA: 0x003460D3 File Offset: 0x003450D3
			public override DependencyObject ContainerAt(int index)
			{
				return this._entry[index].Container;
			}

			// Token: 0x0600901E RID: 36894 RVA: 0x003460E6 File Offset: 0x003450E6
			public override object ItemAt(int index)
			{
				return this._entry[index].Item;
			}

			// Token: 0x0600901F RID: 36895 RVA: 0x003460FC File Offset: 0x003450FC
			public void CopyEntries(ItemContainerGenerator.RealizedItemBlock src, int offset, int count, int newOffset)
			{
				if (offset < newOffset)
				{
					for (int i = count - 1; i >= 0; i--)
					{
						this._entry[newOffset + i] = src._entry[offset + i];
					}
					if (src != this)
					{
						src.ClearEntries(offset, count);
						return;
					}
					src.ClearEntries(offset, newOffset - offset);
					return;
				}
				else
				{
					for (int i = 0; i < count; i++)
					{
						this._entry[newOffset + i] = src._entry[offset + i];
					}
					if (src != this)
					{
						src.ClearEntries(offset, count);
						return;
					}
					src.ClearEntries(newOffset + count, offset - newOffset);
					return;
				}
			}

			// Token: 0x06009020 RID: 36896 RVA: 0x00346198 File Offset: 0x00345198
			public void ClearEntries(int offset, int count)
			{
				for (int i = 0; i < count; i++)
				{
					this._entry[offset + i].Item = null;
					this._entry[offset + i].Container = null;
				}
			}

			// Token: 0x06009021 RID: 36897 RVA: 0x003461D9 File Offset: 0x003451D9
			public void RealizeItem(int index, object item, DependencyObject container)
			{
				this._entry[index].Item = item;
				this._entry[index].Container = container;
			}

			// Token: 0x06009022 RID: 36898 RVA: 0x00346200 File Offset: 0x00345200
			public int OffsetOfItem(object item)
			{
				for (int i = 0; i < base.ItemCount; i++)
				{
					if (ItemsControl.EqualsEx(this._entry[i].Item, item))
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x04004AB3 RID: 19123
			private ItemContainerGenerator.BlockEntry[] _entry = new ItemContainerGenerator.BlockEntry[16];
		}

		// Token: 0x02000C02 RID: 3074
		private struct BlockEntry
		{
			// Token: 0x17001F7B RID: 8059
			// (get) Token: 0x06009024 RID: 36900 RVA: 0x0034624F File Offset: 0x0034524F
			// (set) Token: 0x06009025 RID: 36901 RVA: 0x00346257 File Offset: 0x00345257
			public object Item
			{
				get
				{
					return this._item;
				}
				set
				{
					this._item = value;
				}
			}

			// Token: 0x17001F7C RID: 8060
			// (get) Token: 0x06009026 RID: 36902 RVA: 0x00346260 File Offset: 0x00345260
			// (set) Token: 0x06009027 RID: 36903 RVA: 0x00346268 File Offset: 0x00345268
			public DependencyObject Container
			{
				get
				{
					return this._container;
				}
				set
				{
					this._container = value;
				}
			}

			// Token: 0x04004AB4 RID: 19124
			private object _item;

			// Token: 0x04004AB5 RID: 19125
			private DependencyObject _container;
		}

		// Token: 0x02000C03 RID: 3075
		private struct GeneratorState
		{
			// Token: 0x17001F7D RID: 8061
			// (get) Token: 0x06009028 RID: 36904 RVA: 0x00346271 File Offset: 0x00345271
			// (set) Token: 0x06009029 RID: 36905 RVA: 0x00346279 File Offset: 0x00345279
			public ItemContainerGenerator.ItemBlock Block
			{
				get
				{
					return this._block;
				}
				set
				{
					this._block = value;
				}
			}

			// Token: 0x17001F7E RID: 8062
			// (get) Token: 0x0600902A RID: 36906 RVA: 0x00346282 File Offset: 0x00345282
			// (set) Token: 0x0600902B RID: 36907 RVA: 0x0034628A File Offset: 0x0034528A
			public int Offset
			{
				get
				{
					return this._offset;
				}
				set
				{
					this._offset = value;
				}
			}

			// Token: 0x17001F7F RID: 8063
			// (get) Token: 0x0600902C RID: 36908 RVA: 0x00346293 File Offset: 0x00345293
			// (set) Token: 0x0600902D RID: 36909 RVA: 0x0034629B File Offset: 0x0034529B
			public int Count
			{
				get
				{
					return this._count;
				}
				set
				{
					this._count = value;
				}
			}

			// Token: 0x17001F80 RID: 8064
			// (get) Token: 0x0600902E RID: 36910 RVA: 0x003462A4 File Offset: 0x003452A4
			// (set) Token: 0x0600902F RID: 36911 RVA: 0x003462AC File Offset: 0x003452AC
			public int ItemIndex
			{
				get
				{
					return this._itemIndex;
				}
				set
				{
					this._itemIndex = value;
				}
			}

			// Token: 0x04004AB6 RID: 19126
			private ItemContainerGenerator.ItemBlock _block;

			// Token: 0x04004AB7 RID: 19127
			private int _offset;

			// Token: 0x04004AB8 RID: 19128
			private int _count;

			// Token: 0x04004AB9 RID: 19129
			private int _itemIndex;
		}

		// Token: 0x02000C04 RID: 3076
		private class EmptyGroupItem : GroupItem
		{
			// Token: 0x06009030 RID: 36912 RVA: 0x003462B5 File Offset: 0x003452B5
			public void SetGenerator(ItemContainerGenerator generator)
			{
				base.Generator = generator;
				generator.ItemsChanged += this.OnItemsChanged;
			}

			// Token: 0x06009031 RID: 36913 RVA: 0x003462D0 File Offset: 0x003452D0
			private void OnItemsChanged(object sender, ItemsChangedEventArgs e)
			{
				CollectionViewGroup collectionViewGroup = (CollectionViewGroup)base.GetValue(ItemContainerGenerator.ItemForItemContainerProperty);
				if (collectionViewGroup.ItemCount > 0)
				{
					ItemContainerGenerator generator = base.Generator;
					generator.ItemsChanged -= this.OnItemsChanged;
					generator.Parent.OnSubgroupBecameNonEmpty(this, collectionViewGroup);
				}
			}
		}
	}
}
