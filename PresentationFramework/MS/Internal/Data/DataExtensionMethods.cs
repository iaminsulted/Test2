using System;
using System.Collections;

namespace MS.Internal.Data
{
	// Token: 0x02000215 RID: 533
	internal static class DataExtensionMethods
	{
		// Token: 0x0600143E RID: 5182 RVA: 0x00151338 File Offset: 0x00150338
		internal static int Search(this IList list, int index, int count, object value, IComparer comparer)
		{
			ArrayList arrayList;
			if ((arrayList = (list as ArrayList)) != null)
			{
				return arrayList.BinarySearch(index, count, value, comparer);
			}
			LiveShapingList liveShapingList;
			if ((liveShapingList = (list as LiveShapingList)) != null)
			{
				return liveShapingList.Search(index, count, value);
			}
			return 0;
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x00151370 File Offset: 0x00150370
		internal static int Search(this IList list, object value, IComparer comparer)
		{
			return list.Search(0, list.Count, value, comparer);
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x00151384 File Offset: 0x00150384
		internal static void Move(this IList list, int oldIndex, int newIndex)
		{
			ArrayList arrayList;
			if ((arrayList = (list as ArrayList)) != null)
			{
				object value = arrayList[oldIndex];
				arrayList.RemoveAt(oldIndex);
				arrayList.Insert(newIndex, value);
				return;
			}
			LiveShapingList liveShapingList;
			if ((liveShapingList = (list as LiveShapingList)) != null)
			{
				liveShapingList.Move(oldIndex, newIndex);
			}
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x001513C8 File Offset: 0x001503C8
		internal static void Sort(this IList list, IComparer comparer)
		{
			ArrayList al;
			if ((al = (list as ArrayList)) != null)
			{
				SortFieldComparer.SortHelper(al, comparer);
				return;
			}
			LiveShapingList liveShapingList;
			if ((liveShapingList = (list as LiveShapingList)) != null)
			{
				liveShapingList.Sort();
			}
		}
	}
}
