using System;
using System.Collections;
using System.Collections.Generic;

namespace MS.Internal
{
	// Token: 0x020000FA RID: 250
	internal class ListOfObject : IList<object>, ICollection<object>, IEnumerable<object>, IEnumerable
	{
		// Token: 0x060005EC RID: 1516 RVA: 0x001056B6 File Offset: 0x001046B6
		internal ListOfObject(IList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			this._list = list;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x001056D3 File Offset: 0x001046D3
		int IList<object>.IndexOf(object item)
		{
			return this._list.IndexOf(item);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x001056E1 File Offset: 0x001046E1
		void IList<object>.Insert(int index, object item)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x001056E1 File Offset: 0x001046E1
		void IList<object>.RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		// Token: 0x170000E5 RID: 229
		object IList<object>.this[int index]
		{
			get
			{
				return this._list[index];
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x001056E1 File Offset: 0x001046E1
		void ICollection<object>.Add(object item)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x001056E1 File Offset: 0x001046E1
		void ICollection<object>.Clear()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x001056F6 File Offset: 0x001046F6
		bool ICollection<object>.Contains(object item)
		{
			return this._list.Contains(item);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00105704 File Offset: 0x00104704
		void ICollection<object>.CopyTo(object[] array, int arrayIndex)
		{
			this._list.CopyTo(array, arrayIndex);
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060005F6 RID: 1526 RVA: 0x00105713 File Offset: 0x00104713
		int ICollection<object>.Count
		{
			get
			{
				return this._list.Count;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		bool ICollection<object>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x001056E1 File Offset: 0x001046E1
		bool ICollection<object>.Remove(object item)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00105720 File Offset: 0x00104720
		IEnumerator<object> IEnumerable<object>.GetEnumerator()
		{
			return new ListOfObject.ObjectEnumerator(this._list);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0010572D File Offset: 0x0010472D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<object>)this).GetEnumerator();
		}

		// Token: 0x040006DC RID: 1756
		private IList _list;

		// Token: 0x020008B3 RID: 2227
		private class ObjectEnumerator : IEnumerator<object>, IEnumerator, IDisposable
		{
			// Token: 0x060080D9 RID: 32985 RVA: 0x0032254F File Offset: 0x0032154F
			public ObjectEnumerator(IList list)
			{
				this._ie = list.GetEnumerator();
			}

			// Token: 0x17001D80 RID: 7552
			// (get) Token: 0x060080DA RID: 32986 RVA: 0x00322563 File Offset: 0x00321563
			object IEnumerator<object>.Current
			{
				get
				{
					return this._ie.Current;
				}
			}

			// Token: 0x060080DB RID: 32987 RVA: 0x00322570 File Offset: 0x00321570
			void IDisposable.Dispose()
			{
				this._ie = null;
			}

			// Token: 0x17001D81 RID: 7553
			// (get) Token: 0x060080DC RID: 32988 RVA: 0x00322563 File Offset: 0x00321563
			object IEnumerator.Current
			{
				get
				{
					return this._ie.Current;
				}
			}

			// Token: 0x060080DD RID: 32989 RVA: 0x00322579 File Offset: 0x00321579
			bool IEnumerator.MoveNext()
			{
				return this._ie.MoveNext();
			}

			// Token: 0x060080DE RID: 32990 RVA: 0x00322586 File Offset: 0x00321586
			void IEnumerator.Reset()
			{
				this._ie.Reset();
			}

			// Token: 0x04003C15 RID: 15381
			private IEnumerator _ie;
		}
	}
}
