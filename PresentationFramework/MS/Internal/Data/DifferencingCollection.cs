using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Controls;

namespace MS.Internal.Data
{
	// Token: 0x02000220 RID: 544
	internal sealed class DifferencingCollection : ObservableCollection<object>
	{
		// Token: 0x06001474 RID: 5236 RVA: 0x00152022 File Offset: 0x00151022
		internal DifferencingCollection(IEnumerable enumerable)
		{
			this.LoadItems(enumerable);
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x00152034 File Offset: 0x00151034
		internal void Update(IEnumerable enumerable)
		{
			IList<object> items = base.Items;
			int num = -1;
			int newIndex = -1;
			int count = items.Count;
			DifferencingCollection.Change change = DifferencingCollection.Change.None;
			int num2 = 0;
			object obj = DifferencingCollection.Unset;
			foreach (object obj2 in enumerable)
			{
				if (num2 < count && ItemsControl.EqualsEx(obj2, items[num2]))
				{
					num2++;
				}
				else
				{
					switch (change)
					{
					case DifferencingCollection.Change.None:
						if (num2 + 1 < count && ItemsControl.EqualsEx(obj2, items[num2 + 1]))
						{
							change = DifferencingCollection.Change.Remove;
							num = num2;
							obj = items[num2];
							num2 += 2;
						}
						else
						{
							change = DifferencingCollection.Change.Add;
							num = num2;
							obj = obj2;
						}
						break;
					case DifferencingCollection.Change.Add:
						if (num2 + 1 < count && ItemsControl.EqualsEx(obj2, items[num2 + 1]))
						{
							if (ItemsControl.EqualsEx(obj, items[num2]))
							{
								change = DifferencingCollection.Change.Move;
								newIndex = num;
								num = num2;
							}
							else if (num2 < count && num2 == num)
							{
								change = DifferencingCollection.Change.Replace;
							}
							else
							{
								change = DifferencingCollection.Change.Reset;
							}
							num2 += 2;
						}
						else
						{
							change = DifferencingCollection.Change.Reset;
						}
						break;
					case DifferencingCollection.Change.Remove:
						if (ItemsControl.EqualsEx(obj2, obj))
						{
							change = DifferencingCollection.Change.Move;
							newIndex = num2 - 1;
						}
						else
						{
							change = DifferencingCollection.Change.Reset;
						}
						break;
					default:
						change = DifferencingCollection.Change.Reset;
						break;
					}
					if (change == DifferencingCollection.Change.Reset)
					{
						break;
					}
				}
			}
			if (num2 == count - 1)
			{
				if (change != DifferencingCollection.Change.None)
				{
					if (change != DifferencingCollection.Change.Add)
					{
						change = DifferencingCollection.Change.Reset;
					}
					else if (ItemsControl.EqualsEx(obj, items[num2]))
					{
						change = DifferencingCollection.Change.Move;
						newIndex = num;
						num = num2;
					}
					else if (num == count - 1)
					{
						change = DifferencingCollection.Change.Replace;
					}
					else
					{
						change = DifferencingCollection.Change.Reset;
					}
				}
				else
				{
					change = DifferencingCollection.Change.Remove;
					num = num2;
				}
			}
			else if (num2 != count)
			{
				change = DifferencingCollection.Change.Reset;
			}
			switch (change)
			{
			case DifferencingCollection.Change.None:
				break;
			case DifferencingCollection.Change.Add:
				Invariant.Assert(obj != DifferencingCollection.Unset);
				base.Insert(num, obj);
				return;
			case DifferencingCollection.Change.Remove:
				base.RemoveAt(num);
				return;
			case DifferencingCollection.Change.Move:
				base.Move(num, newIndex);
				return;
			case DifferencingCollection.Change.Replace:
				Invariant.Assert(obj != DifferencingCollection.Unset);
				base[num] = obj;
				return;
			case DifferencingCollection.Change.Reset:
				this.Reload(enumerable);
				break;
			default:
				return;
			}
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x00152264 File Offset: 0x00151264
		private void LoadItems(IEnumerable enumerable)
		{
			foreach (object item in enumerable)
			{
				base.Items.Add(item);
			}
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x001522B8 File Offset: 0x001512B8
		private void Reload(IEnumerable enumerable)
		{
			base.Items.Clear();
			this.LoadItems(enumerable);
			this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
			this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		// Token: 0x04000BCE RID: 3022
		private static object Unset = new object();

		// Token: 0x020009F0 RID: 2544
		private enum Change
		{
			// Token: 0x04004016 RID: 16406
			None,
			// Token: 0x04004017 RID: 16407
			Add,
			// Token: 0x04004018 RID: 16408
			Remove,
			// Token: 0x04004019 RID: 16409
			Move,
			// Token: 0x0400401A RID: 16410
			Replace,
			// Token: 0x0400401B RID: 16411
			Reset
		}
	}
}
