using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006AE RID: 1710
	public class TextElementCollection<TextElementType> : IList, ICollection, IEnumerable, ICollection<TextElementType>, IEnumerable<!0> where TextElementType : TextElement
	{
		// Token: 0x060056E7 RID: 22247 RVA: 0x0026BD90 File Offset: 0x0026AD90
		internal TextElementCollection(DependencyObject owner, bool isOwnerParent)
		{
			if (isOwnerParent)
			{
				Invariant.Assert(owner is TextElement || owner is FlowDocument || owner is TextBlock);
			}
			else
			{
				Invariant.Assert(owner is TextElement);
			}
			this._owner = owner;
			this._isOwnerParent = isOwnerParent;
			this._indexCache = new TextElementCollection<TextElementType>.ElementIndexCache(-1, default(TextElementType));
		}

		// Token: 0x060056E8 RID: 22248 RVA: 0x0026BDFC File Offset: 0x0026ADFC
		public void Add(TextElementType item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.ValidateChild(item);
			this.TextContainer.BeginChange();
			try
			{
				item.RepositionWithContent(this.ContentEnd);
			}
			finally
			{
				this.TextContainer.EndChange();
			}
		}

		// Token: 0x060056E9 RID: 22249 RVA: 0x0026BE60 File Offset: 0x0026AE60
		public void Clear()
		{
			TextContainer textContainer = this.TextContainer;
			textContainer.BeginChange();
			try
			{
				textContainer.DeleteContentInternal(this.ContentStart, this.ContentEnd);
			}
			finally
			{
				textContainer.EndChange();
			}
		}

		// Token: 0x060056EA RID: 22250 RVA: 0x0026BEA8 File Offset: 0x0026AEA8
		public bool Contains(TextElementType item)
		{
			if (item == null)
			{
				return false;
			}
			TextElementType textElementType = this.FirstChild;
			while (textElementType != null && textElementType != item)
			{
				textElementType = (TextElementType)((object)textElementType.NextElement);
			}
			return textElementType == item;
		}

		// Token: 0x060056EB RID: 22251 RVA: 0x00243550 File Offset: 0x00242550
		public void CopyTo(TextElementType[] array, int arrayIndex)
		{
			((ICollection)this).CopyTo(array, arrayIndex);
		}

		// Token: 0x1700146C RID: 5228
		// (get) Token: 0x060056EC RID: 22252 RVA: 0x0026BF00 File Offset: 0x0026AF00
		public int Count
		{
			get
			{
				int num = 0;
				TextElement textElement;
				if (this._indexCache.IsValid(this))
				{
					textElement = this._indexCache.Element;
					num += this._indexCache.Index;
				}
				else
				{
					textElement = this.FirstChild;
				}
				while (textElement != null)
				{
					num++;
					textElement = textElement.NextElement;
				}
				return num;
			}
		}

		// Token: 0x1700146D RID: 5229
		// (get) Token: 0x060056ED RID: 22253 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060056EE RID: 22254 RVA: 0x0026BF5C File Offset: 0x0026AF5C
		public bool Remove(TextElementType item)
		{
			if (item == null)
			{
				return false;
			}
			if (item.Parent != this.Parent)
			{
				return false;
			}
			TextContainer textContainer = this.TextContainer;
			textContainer.BeginChange();
			try
			{
				item.RepositionWithContent(null);
			}
			finally
			{
				textContainer.EndChange();
			}
			return true;
		}

		// Token: 0x060056EF RID: 22255 RVA: 0x0026BFBC File Offset: 0x0026AFBC
		public void InsertAfter(TextElementType previousSibling, TextElementType newItem)
		{
			if (previousSibling == null)
			{
				throw new ArgumentNullException("previousSibling");
			}
			if (newItem == null)
			{
				throw new ArgumentNullException("newItem");
			}
			if (previousSibling.Parent != this.Parent)
			{
				throw new InvalidOperationException(SR.Get("TextElementCollection_PreviousSiblingDoesNotBelongToThisCollection", new object[]
				{
					previousSibling.GetType().Name
				}));
			}
			if (newItem.Parent != null)
			{
				throw new ArgumentException(SR.Get("TextSchema_TheChildElementBelongsToAnotherTreeAlready", new object[]
				{
					base.GetType().Name
				}));
			}
			this.ValidateChild(newItem);
			this.TextContainer.BeginChange();
			try
			{
				newItem.RepositionWithContent(previousSibling.ElementEnd);
			}
			finally
			{
				this.TextContainer.EndChange();
			}
		}

		// Token: 0x060056F0 RID: 22256 RVA: 0x0026C0A4 File Offset: 0x0026B0A4
		public void InsertBefore(TextElementType nextSibling, TextElementType newItem)
		{
			if (nextSibling == null)
			{
				throw new ArgumentNullException("nextSibling");
			}
			if (newItem == null)
			{
				throw new ArgumentNullException("newItem");
			}
			if (nextSibling.Parent != this.Parent)
			{
				throw new InvalidOperationException(SR.Get("TextElementCollection_NextSiblingDoesNotBelongToThisCollection", new object[]
				{
					nextSibling.GetType().Name
				}));
			}
			if (newItem.Parent != null)
			{
				throw new ArgumentException(SR.Get("TextSchema_TheChildElementBelongsToAnotherTreeAlready", new object[]
				{
					base.GetType().Name
				}));
			}
			this.ValidateChild(newItem);
			this.TextContainer.BeginChange();
			try
			{
				newItem.RepositionWithContent(nextSibling.ElementStart);
			}
			finally
			{
				this.TextContainer.EndChange();
			}
		}

		// Token: 0x060056F1 RID: 22257 RVA: 0x0026C18C File Offset: 0x0026B18C
		public void AddRange(IEnumerable range)
		{
			if (range == null)
			{
				throw new ArgumentNullException("range");
			}
			IEnumerator enumerator = range.GetEnumerator();
			if (enumerator == null)
			{
				throw new ArgumentException(SR.Get("TextElementCollection_NoEnumerator"), "range");
			}
			this.TextContainer.BeginChange();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					TextElementType textElementType = obj as TextElementType;
					if (textElementType == null)
					{
						throw new ArgumentException(SR.Get("TextElementCollection_ItemHasUnexpectedType", new object[]
						{
							"range",
							typeof(TextElementType).Name,
							typeof(TextElementType).Name
						}), "value");
					}
					this.Add(textElementType);
				}
			}
			finally
			{
				this.TextContainer.EndChange();
			}
		}

		// Token: 0x060056F2 RID: 22258 RVA: 0x0026C260 File Offset: 0x0026B260
		public IEnumerator<TextElementType> GetEnumerator()
		{
			return new TextElementEnumerator<TextElementType>(this.ContentStart, this.ContentEnd);
		}

		// Token: 0x060056F3 RID: 22259 RVA: 0x0026C273 File Offset: 0x0026B273
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new RangeContentEnumerator(this.ContentStart, this.ContentEnd);
		}

		// Token: 0x060056F4 RID: 22260 RVA: 0x0026C288 File Offset: 0x0026B288
		internal virtual int OnAdd(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is TextElementType))
			{
				throw new ArgumentException(SR.Get("TextElementCollection_TextElementTypeExpected", new object[]
				{
					typeof(TextElementType).Name
				}), "value");
			}
			this.ValidateChild((TextElementType)((object)value));
			this.TextContainer.BeginChange();
			int result;
			try
			{
				bool isCacheSafePreviousIndex = this._indexCache.IsValid(this);
				this.Add((TextElementType)((object)value));
				result = this.IndexOfInternal(value, isCacheSafePreviousIndex);
			}
			finally
			{
				this.TextContainer.EndChange();
			}
			return result;
		}

		// Token: 0x060056F5 RID: 22261 RVA: 0x0026C330 File Offset: 0x0026B330
		int IList.Add(object value)
		{
			return this.OnAdd(value);
		}

		// Token: 0x060056F6 RID: 22262 RVA: 0x0026C339 File Offset: 0x0026B339
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x060056F7 RID: 22263 RVA: 0x0026C344 File Offset: 0x0026B344
		bool IList.Contains(object value)
		{
			TextElementType textElementType = value as TextElementType;
			return textElementType != null && this.Contains(textElementType);
		}

		// Token: 0x060056F8 RID: 22264 RVA: 0x0026C36E File Offset: 0x0026B36E
		int IList.IndexOf(object value)
		{
			return this.IndexOfInternal(value, false);
		}

		// Token: 0x060056F9 RID: 22265 RVA: 0x0026C378 File Offset: 0x0026B378
		void IList.Insert(int index, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			TextElementType textElementType = value as TextElementType;
			if (textElementType == null)
			{
				throw new ArgumentException(SR.Get("TextElementCollection_TextElementTypeExpected", new object[]
				{
					typeof(TextElementType).Name
				}), "value");
			}
			if (index < 0)
			{
				throw new IndexOutOfRangeException(SR.Get("TextElementCollection_IndexOutOfRange"));
			}
			if (textElementType.Parent != null)
			{
				throw new ArgumentException(SR.Get("TextSchema_TheChildElementBelongsToAnotherTreeAlready", new object[]
				{
					base.GetType().Name
				}));
			}
			this.ValidateChild(textElementType);
			this.TextContainer.BeginChange();
			try
			{
				TextPointer textPointer;
				if (this.FirstChild == null)
				{
					if (index != 0)
					{
						throw new IndexOutOfRangeException(SR.Get("TextElementCollection_IndexOutOfRange"));
					}
					textPointer = this.ContentStart;
				}
				else
				{
					bool flag;
					TextElementType elementAtIndex = this.GetElementAtIndex(index, out flag);
					if (!flag && elementAtIndex == null)
					{
						throw new IndexOutOfRangeException(SR.Get("TextElementCollection_IndexOutOfRange"));
					}
					textPointer = (flag ? this.ContentEnd : elementAtIndex.ElementStart);
				}
				textPointer.InsertTextElement(textElementType);
				this.SetCache(index, textElementType);
			}
			finally
			{
				this.TextContainer.EndChange();
			}
		}

		// Token: 0x1700146E RID: 5230
		// (get) Token: 0x060056FA RID: 22266 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700146F RID: 5231
		// (get) Token: 0x060056FB RID: 22267 RVA: 0x0026C4C4 File Offset: 0x0026B4C4
		bool IList.IsReadOnly
		{
			get
			{
				return this.IsReadOnly;
			}
		}

		// Token: 0x060056FC RID: 22268 RVA: 0x0026C4CC File Offset: 0x0026B4CC
		void IList.Remove(object value)
		{
			TextElementType textElementType = value as TextElementType;
			if (textElementType == null)
			{
				return;
			}
			this.Remove(textElementType);
		}

		// Token: 0x060056FD RID: 22269 RVA: 0x0026C4F6 File Offset: 0x0026B4F6
		void IList.RemoveAt(int index)
		{
			this.RemoveAtInternal(index);
		}

		// Token: 0x17001470 RID: 5232
		object IList.this[int index]
		{
			get
			{
				if (index < 0)
				{
					throw new IndexOutOfRangeException(SR.Get("TextElementCollection_IndexOutOfRange"));
				}
				TextElementType elementAtIndex = this.GetElementAtIndex(index);
				if (elementAtIndex == null)
				{
					throw new IndexOutOfRangeException(SR.Get("TextElementCollection_IndexOutOfRange"));
				}
				this.SetCache(index, elementAtIndex);
				return elementAtIndex;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!(value is TextElementType))
				{
					throw new ArgumentException(SR.Get("TextElementCollection_TextElementTypeExpected", new object[]
					{
						typeof(TextElementType).Name
					}), "value");
				}
				this.ValidateChild((TextElementType)((object)value));
				this.TextContainer.BeginChange();
				try
				{
					TextElementType textElementType = this.RemoveAtInternal(index);
					((textElementType == null) ? this.ContentEnd : textElementType.ElementStart).InsertTextElement((TextElementType)((object)value));
					this.SetCache(index, (TextElementType)((object)value));
				}
				finally
				{
					this.TextContainer.EndChange();
				}
			}
		}

		// Token: 0x06005700 RID: 22272 RVA: 0x0026C618 File Offset: 0x0026B618
		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			int count = this.Count;
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Type elementType = array.GetType().GetElementType();
			if (elementType == null || !elementType.IsAssignableFrom(typeof(TextElementType)))
			{
				throw new ArgumentException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			if (arrayIndex > array.Length)
			{
				throw new ArgumentException("arrayIndex");
			}
			if (array.Length < arrayIndex + count)
			{
				throw new ArgumentException(SR.Get("TextElementCollection_CannotCopyToArrayNotSufficientMemory", new object[]
				{
					count,
					arrayIndex,
					array.Length
				}));
			}
			for (TextElementType textElementType = this.FirstChild; textElementType != null; textElementType = (TextElementType)((object)textElementType.NextElement))
			{
				array.SetValue(textElementType, arrayIndex++);
			}
		}

		// Token: 0x17001471 RID: 5233
		// (get) Token: 0x06005701 RID: 22273 RVA: 0x0026C704 File Offset: 0x0026B704
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x17001472 RID: 5234
		// (get) Token: 0x06005702 RID: 22274 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		bool ICollection.IsSynchronized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001473 RID: 5235
		// (get) Token: 0x06005703 RID: 22275 RVA: 0x0026C70C File Offset: 0x0026B70C
		object ICollection.SyncRoot
		{
			get
			{
				return this.TextContainer;
			}
		}

		// Token: 0x17001474 RID: 5236
		// (get) Token: 0x06005704 RID: 22276 RVA: 0x0026C714 File Offset: 0x0026B714
		internal DependencyObject Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17001475 RID: 5237
		// (get) Token: 0x06005705 RID: 22277 RVA: 0x0026C71C File Offset: 0x0026B71C
		internal DependencyObject Parent
		{
			get
			{
				if (!this._isOwnerParent)
				{
					return ((TextElement)this._owner).Parent;
				}
				return this._owner;
			}
		}

		// Token: 0x17001476 RID: 5238
		// (get) Token: 0x06005706 RID: 22278 RVA: 0x0026C740 File Offset: 0x0026B740
		internal TextContainer TextContainer
		{
			get
			{
				TextContainer result;
				if (this._owner is TextBlock)
				{
					result = (TextContainer)((TextBlock)this._owner).TextContainer;
				}
				else if (this._owner is FlowDocument)
				{
					result = ((FlowDocument)this._owner).TextContainer;
				}
				else
				{
					result = ((TextElement)this._owner).TextContainer;
				}
				return result;
			}
		}

		// Token: 0x17001477 RID: 5239
		// (get) Token: 0x06005707 RID: 22279 RVA: 0x0026C7A4 File Offset: 0x0026B7A4
		internal TextElementType FirstChild
		{
			get
			{
				TextElementType result;
				if (this.Parent is TextElement)
				{
					result = (TextElementType)((object)((TextElement)this.Parent).FirstChildElement);
				}
				else
				{
					TextTreeTextElementNode textTreeTextElementNode = this.TextContainer.FirstContainedNode as TextTreeTextElementNode;
					result = (TextElementType)((object)((textTreeTextElementNode == null) ? null : textTreeTextElementNode.TextElement));
				}
				return result;
			}
		}

		// Token: 0x17001478 RID: 5240
		// (get) Token: 0x06005708 RID: 22280 RVA: 0x0026C7FC File Offset: 0x0026B7FC
		internal TextElementType LastChild
		{
			get
			{
				TextElementType result;
				if (this.Parent is TextElement)
				{
					result = (TextElementType)((object)((TextElement)this.Parent).LastChildElement);
				}
				else
				{
					TextTreeTextElementNode textTreeTextElementNode = this.TextContainer.LastContainedNode as TextTreeTextElementNode;
					result = (TextElementType)((object)((textTreeTextElementNode == null) ? null : textTreeTextElementNode.TextElement));
				}
				return result;
			}
		}

		// Token: 0x06005709 RID: 22281 RVA: 0x0026C854 File Offset: 0x0026B854
		private TextElementType RemoveAtInternal(int index)
		{
			if (index < 0)
			{
				throw new IndexOutOfRangeException(SR.Get("TextElementCollection_IndexOutOfRange"));
			}
			TextElementType elementAtIndex = this.GetElementAtIndex(index);
			if (elementAtIndex == null)
			{
				throw new IndexOutOfRangeException(SR.Get("TextElementCollection_IndexOutOfRange"));
			}
			TextElementType textElementType = (TextElementType)((object)elementAtIndex.NextElement);
			TextContainer textContainer = this.TextContainer;
			textContainer.BeginChange();
			try
			{
				TextElementType textElementType2 = textElementType;
				if (textElementType2 == null)
				{
					textElementType2 = (TextElementType)((object)elementAtIndex.PreviousElement);
					index--;
				}
				elementAtIndex.RepositionWithContent(null);
				if (textElementType2 != null)
				{
					this.SetCache(index, textElementType2);
				}
			}
			finally
			{
				textContainer.EndChange();
			}
			return textElementType;
		}

		// Token: 0x0600570A RID: 22282 RVA: 0x0026C90C File Offset: 0x0026B90C
		private TextElementType GetElementAtIndex(int index)
		{
			bool flag;
			return this.GetElementAtIndex(index, out flag);
		}

		// Token: 0x0600570B RID: 22283 RVA: 0x0026C924 File Offset: 0x0026B924
		private TextElementType GetElementAtIndex(int index, out bool atCollectionEnd)
		{
			bool flag = true;
			TextElementType textElementType;
			if (this._indexCache.IsValid(this))
			{
				if (this._indexCache.Index == index)
				{
					textElementType = this._indexCache.Element;
					index = 0;
				}
				else if (this._indexCache.Index < index)
				{
					textElementType = this._indexCache.Element;
					index -= this._indexCache.Index;
				}
				else
				{
					textElementType = this._indexCache.Element;
					index = this._indexCache.Index - index;
					flag = false;
				}
			}
			else
			{
				textElementType = this.FirstChild;
			}
			while (index > 0 && textElementType != null)
			{
				textElementType = (TextElementType)((object)(flag ? textElementType.NextElement : textElementType.PreviousElement));
				index--;
			}
			atCollectionEnd = (index == 0 && textElementType == null);
			return textElementType;
		}

		// Token: 0x0600570C RID: 22284 RVA: 0x0026C9F7 File Offset: 0x0026B9F7
		private void SetCache(int index, TextElementType item)
		{
			this._indexCache = new TextElementCollection<TextElementType>.ElementIndexCache(index, item);
			TextElementCollectionHelper.MarkClean(this.Parent, this);
		}

		// Token: 0x0600570D RID: 22285 RVA: 0x0026CA14 File Offset: 0x0026BA14
		private int IndexOfInternal(object value, bool isCacheSafePreviousIndex)
		{
			TextElementType textElementType = value as TextElementType;
			if (value == null)
			{
				return -1;
			}
			if (this._indexCache.IsValid(this) && textElementType == this._indexCache.Element)
			{
				return this._indexCache.Index;
			}
			int num;
			TextElementType textElementType2;
			if (isCacheSafePreviousIndex)
			{
				num = this._indexCache.Index;
				textElementType2 = this._indexCache.Element;
			}
			else
			{
				num = 0;
				textElementType2 = this.FirstChild;
			}
			while (textElementType2 != null)
			{
				if (textElementType2 == textElementType)
				{
					this.SetCache(num, textElementType);
					return num;
				}
				textElementType2 = (TextElementType)((object)textElementType2.NextElement);
				num++;
			}
			return -1;
		}

		// Token: 0x0600570E RID: 22286 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void ValidateChild(TextElementType child)
		{
		}

		// Token: 0x17001479 RID: 5241
		// (get) Token: 0x0600570F RID: 22287 RVA: 0x0026CAC2 File Offset: 0x0026BAC2
		private TextPointer ContentStart
		{
			get
			{
				if (!(this.Parent is TextElement))
				{
					return this.TextContainer.Start;
				}
				return ((TextElement)this.Parent).ContentStart;
			}
		}

		// Token: 0x1700147A RID: 5242
		// (get) Token: 0x06005710 RID: 22288 RVA: 0x0026CAED File Offset: 0x0026BAED
		private TextPointer ContentEnd
		{
			get
			{
				if (!(this.Parent is TextElement))
				{
					return this.TextContainer.End;
				}
				return ((TextElement)this.Parent).ContentEnd;
			}
		}

		// Token: 0x04002FAE RID: 12206
		private DependencyObject _owner;

		// Token: 0x04002FAF RID: 12207
		private bool _isOwnerParent;

		// Token: 0x04002FB0 RID: 12208
		private TextElementCollection<TextElementType>.ElementIndexCache _indexCache;

		// Token: 0x02000B69 RID: 2921
		private struct ElementIndexCache
		{
			// Token: 0x06008DED RID: 36333 RVA: 0x0033FE22 File Offset: 0x0033EE22
			internal ElementIndexCache(int index, TextElementType element)
			{
				Invariant.Assert(index == -1 || element != null);
				this._index = index;
				this._element = element;
			}

			// Token: 0x06008DEE RID: 36334 RVA: 0x0033FE47 File Offset: 0x0033EE47
			internal bool IsValid(TextElementCollection<TextElementType> collection)
			{
				return this._index >= 0 && TextElementCollectionHelper.IsCleanParent(this._element.Parent, collection);
			}

			// Token: 0x17001EFF RID: 7935
			// (get) Token: 0x06008DEF RID: 36335 RVA: 0x0033FE6A File Offset: 0x0033EE6A
			internal int Index
			{
				get
				{
					return this._index;
				}
			}

			// Token: 0x17001F00 RID: 7936
			// (get) Token: 0x06008DF0 RID: 36336 RVA: 0x0033FE72 File Offset: 0x0033EE72
			internal TextElementType Element
			{
				get
				{
					return this._element;
				}
			}

			// Token: 0x040048CC RID: 18636
			private readonly int _index;

			// Token: 0x040048CD RID: 18637
			private readonly TextElementType _element;
		}
	}
}
