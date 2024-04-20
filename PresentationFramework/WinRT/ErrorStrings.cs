using System;

namespace WinRT
{
	// Token: 0x020000A6 RID: 166
	internal class ErrorStrings
	{
		// Token: 0x0600026B RID: 619 RVA: 0x000FA5E7 File Offset: 0x000F95E7
		internal static string Format(string format, params object[] args)
		{
			return string.Format(format, args);
		}

		// Token: 0x040005A3 RID: 1443
		internal static readonly string Arg_IndexOutOfRangeException = "Index was outside the bounds of the array.";

		// Token: 0x040005A4 RID: 1444
		internal static readonly string Arg_KeyNotFound = "The given key was not present in the dictionary.";

		// Token: 0x040005A5 RID: 1445
		internal static readonly string Arg_KeyNotFoundWithKey = "The given key '{0}' was not present in the dictionary.";

		// Token: 0x040005A6 RID: 1446
		internal static readonly string Arg_RankMultiDimNotSupported = "Only single dimensional arrays are supported for the requested action.";

		// Token: 0x040005A7 RID: 1447
		internal static readonly string Argument_AddingDuplicate = "An item with the same key has already been added.";

		// Token: 0x040005A8 RID: 1448
		internal static readonly string Argument_AddingDuplicateWithKey = "An item with the same key has already been added. Key: {0}";

		// Token: 0x040005A9 RID: 1449
		internal static readonly string Argument_IndexOutOfArrayBounds = "The specified index is out of bounds of the specified array.";

		// Token: 0x040005AA RID: 1450
		internal static readonly string Argument_InsufficientSpaceToCopyCollection = "The specified space is not sufficient to copy the elements from this Collection.";

		// Token: 0x040005AB RID: 1451
		internal static readonly string ArgumentOutOfRange_Index = "Index was out of range. Must be non-negative and less than the size of the collection.";

		// Token: 0x040005AC RID: 1452
		internal static readonly string ArgumentOutOfRange_IndexLargerThanMaxValue = "This collection cannot work with indices larger than Int32.MaxValue - 1 (0x7FFFFFFF - 1).";

		// Token: 0x040005AD RID: 1453
		internal static readonly string InvalidOperation_CannotRemoveLastFromEmptyCollection = "Cannot remove the last element from an empty collection.";

		// Token: 0x040005AE RID: 1454
		internal static readonly string InvalidOperation_CollectionBackingDictionaryTooLarge = "The collection backing this Dictionary contains too many elements.";

		// Token: 0x040005AF RID: 1455
		internal static readonly string InvalidOperation_CollectionBackingListTooLarge = "The collection backing this List contains too many elements.";

		// Token: 0x040005B0 RID: 1456
		internal static readonly string InvalidOperation_EnumEnded = "Enumeration already finished.";

		// Token: 0x040005B1 RID: 1457
		internal static readonly string InvalidOperation_EnumFailedVersion = "Collection was modified; enumeration operation may not execute.";

		// Token: 0x040005B2 RID: 1458
		internal static readonly string InvalidOperation_EnumNotStarted = "Enumeration has not started. Call MoveNext.";

		// Token: 0x040005B3 RID: 1459
		internal static readonly string NotSupported_KeyCollectionSet = "Mutating a key collection derived from a dictionary is not allowed.";

		// Token: 0x040005B4 RID: 1460
		internal static readonly string NotSupported_ValueCollectionSet = "Mutating a value collection derived from a dictionary is not allowed.";
	}
}
