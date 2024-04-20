using System;

namespace System.Windows.Documents
{
	// Token: 0x020006AF RID: 1711
	internal static class TextElementCollectionHelper
	{
		// Token: 0x06005711 RID: 22289 RVA: 0x0026CB18 File Offset: 0x0026BB18
		internal static void MarkDirty(DependencyObject parent)
		{
			if (parent == null)
			{
				return;
			}
			WeakReference[] cleanParentList = TextElementCollectionHelper._cleanParentList;
			lock (cleanParentList)
			{
				for (int i = 0; i < TextElementCollectionHelper._cleanParentList.Length; i++)
				{
					if (TextElementCollectionHelper._cleanParentList[i] != null)
					{
						TextElementCollectionHelper.ParentCollectionPair parentCollectionPair = (TextElementCollectionHelper.ParentCollectionPair)TextElementCollectionHelper._cleanParentList[i].Target;
						if (parentCollectionPair == null || parentCollectionPair.Parent == parent)
						{
							TextElementCollectionHelper._cleanParentList[i] = null;
						}
					}
				}
			}
		}

		// Token: 0x06005712 RID: 22290 RVA: 0x0026CB98 File Offset: 0x0026BB98
		internal static void MarkClean(DependencyObject parent, object collection)
		{
			WeakReference[] cleanParentList = TextElementCollectionHelper._cleanParentList;
			lock (cleanParentList)
			{
				int num2;
				int num = TextElementCollectionHelper.GetCleanParentIndex(parent, collection, out num2);
				if (num == -1)
				{
					num = ((num2 >= 0) ? num2 : (TextElementCollectionHelper._cleanParentList.Length - 1));
					TextElementCollectionHelper._cleanParentList[num] = new WeakReference(new TextElementCollectionHelper.ParentCollectionPair(parent, collection));
				}
				TextElementCollectionHelper.TouchCleanParent(num);
			}
		}

		// Token: 0x06005713 RID: 22291 RVA: 0x0026CC0C File Offset: 0x0026BC0C
		internal static bool IsCleanParent(DependencyObject parent, object collection)
		{
			int num = -1;
			WeakReference[] cleanParentList = TextElementCollectionHelper._cleanParentList;
			lock (cleanParentList)
			{
				int num2;
				num = TextElementCollectionHelper.GetCleanParentIndex(parent, collection, out num2);
				if (num >= 0)
				{
					TextElementCollectionHelper.TouchCleanParent(num);
				}
			}
			return num >= 0;
		}

		// Token: 0x06005714 RID: 22292 RVA: 0x0026CC64 File Offset: 0x0026BC64
		private static void TouchCleanParent(int index)
		{
			WeakReference weakReference = TextElementCollectionHelper._cleanParentList[index];
			Array.Copy(TextElementCollectionHelper._cleanParentList, 0, TextElementCollectionHelper._cleanParentList, 1, index);
			TextElementCollectionHelper._cleanParentList[0] = weakReference;
		}

		// Token: 0x06005715 RID: 22293 RVA: 0x0026CC94 File Offset: 0x0026BC94
		private static int GetCleanParentIndex(DependencyObject parent, object collection, out int firstFreeIndex)
		{
			int result = -1;
			firstFreeIndex = -1;
			for (int i = 0; i < TextElementCollectionHelper._cleanParentList.Length; i++)
			{
				if (TextElementCollectionHelper._cleanParentList[i] == null)
				{
					if (firstFreeIndex == -1)
					{
						firstFreeIndex = i;
					}
				}
				else
				{
					TextElementCollectionHelper.ParentCollectionPair parentCollectionPair = (TextElementCollectionHelper.ParentCollectionPair)TextElementCollectionHelper._cleanParentList[i].Target;
					if (parentCollectionPair == null)
					{
						TextElementCollectionHelper._cleanParentList[i] = null;
						if (firstFreeIndex == -1)
						{
							firstFreeIndex = i;
						}
					}
					else if (parentCollectionPair.Parent == parent && parentCollectionPair.Collection == collection)
					{
						result = i;
					}
				}
			}
			return result;
		}

		// Token: 0x04002FB1 RID: 12209
		private static readonly WeakReference[] _cleanParentList = new WeakReference[10];

		// Token: 0x02000B6A RID: 2922
		private class ParentCollectionPair
		{
			// Token: 0x06008DF1 RID: 36337 RVA: 0x0033FE7A File Offset: 0x0033EE7A
			internal ParentCollectionPair(DependencyObject parent, object collection)
			{
				this._parent = parent;
				this._collection = collection;
			}

			// Token: 0x17001F01 RID: 7937
			// (get) Token: 0x06008DF2 RID: 36338 RVA: 0x0033FE90 File Offset: 0x0033EE90
			internal DependencyObject Parent
			{
				get
				{
					return this._parent;
				}
			}

			// Token: 0x17001F02 RID: 7938
			// (get) Token: 0x06008DF3 RID: 36339 RVA: 0x0033FE98 File Offset: 0x0033EE98
			internal object Collection
			{
				get
				{
					return this._collection;
				}
			}

			// Token: 0x040048CE RID: 18638
			private readonly DependencyObject _parent;

			// Token: 0x040048CF RID: 18639
			private readonly object _collection;
		}
	}
}
