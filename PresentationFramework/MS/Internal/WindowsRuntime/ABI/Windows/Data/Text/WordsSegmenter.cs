using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MS.Internal.WindowsRuntime.Windows.Data.Text;
using WinRT;

namespace MS.Internal.WindowsRuntime.ABI.Windows.Data.Text
{
	// Token: 0x02000303 RID: 771
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal struct WordsSegmenter
	{
		// Token: 0x06001CE3 RID: 7395 RVA: 0x0016C70C File Offset: 0x0016B70C
		public static IObjectReference CreateMarshaler(WordsSegmenter obj)
		{
			if (obj != null)
			{
				return MarshalInspectable.CreateMarshaler(obj, true).As<IWordsSegmenter.Vftbl>();
			}
			return null;
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x000FC48C File Offset: 0x000FB48C
		public static IntPtr GetAbi(IObjectReference value)
		{
			if (value != null)
			{
				return MarshalInterfaceHelper<object>.GetAbi(value);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06001CE5 RID: 7397 RVA: 0x0016C71F File Offset: 0x0016B71F
		public static WordsSegmenter FromAbi(IntPtr thisPtr)
		{
			return WordsSegmenter.FromAbi(thisPtr);
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x0016C727 File Offset: 0x0016B727
		public static IntPtr FromManaged(WordsSegmenter obj)
		{
			if (obj != null)
			{
				return WordsSegmenter.CreateMarshaler(obj).GetRef();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06001CE7 RID: 7399 RVA: 0x0016C73D File Offset: 0x0016B73D
		public static MarshalInterfaceHelper<WordsSegmenter>.MarshalerArray CreateMarshalerArray(WordsSegmenter[] array)
		{
			return MarshalInterfaceHelper<WordsSegmenter>.CreateMarshalerArray(array, (WordsSegmenter o) => WordsSegmenter.CreateMarshaler(o));
		}

		// Token: 0x06001CE8 RID: 7400 RVA: 0x0016C764 File Offset: 0x0016B764
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			return MarshalInterfaceHelper<WordsSegmenter>.GetAbiArray(box);
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x0016C76C File Offset: 0x0016B76C
		public static WordsSegmenter[] FromAbiArray(object box)
		{
			return MarshalInterfaceHelper<WordsSegmenter>.FromAbiArray(box, new Func<IntPtr, WordsSegmenter>(WordsSegmenter.FromAbi));
		}

		// Token: 0x06001CEA RID: 7402 RVA: 0x0016C780 File Offset: 0x0016B780
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(WordsSegmenter[] array)
		{
			return MarshalInterfaceHelper<WordsSegmenter>.FromManagedArray(array, (WordsSegmenter o) => WordsSegmenter.FromManaged(o));
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x0016BAE3 File Offset: 0x0016AAE3
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInspectable.DisposeMarshaler(value);
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x0016C7A7 File Offset: 0x0016B7A7
		public static void DisposeMarshalerArray(MarshalInterfaceHelper<WordsSegmenter>.MarshalerArray array)
		{
			MarshalInterfaceHelper<WordsSegmenter>.DisposeMarshalerArray(array);
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x0016BAF8 File Offset: 0x0016AAF8
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInspectable.DisposeAbi(abi);
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x0016BB00 File Offset: 0x0016AB00
		public static void DisposeAbiArray(object box)
		{
			MarshalInspectable.DisposeAbiArray(box);
		}
	}
}
