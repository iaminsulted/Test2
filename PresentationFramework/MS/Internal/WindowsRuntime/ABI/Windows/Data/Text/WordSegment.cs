using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MS.Internal.WindowsRuntime.Windows.Data.Text;
using WinRT;

namespace MS.Internal.WindowsRuntime.ABI.Windows.Data.Text
{
	// Token: 0x02000301 RID: 769
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal struct WordSegment
	{
		// Token: 0x06001CCE RID: 7374 RVA: 0x0016C510 File Offset: 0x0016B510
		public static IObjectReference CreateMarshaler(WordSegment obj)
		{
			if (obj != null)
			{
				return MarshalInspectable.CreateMarshaler(obj, true).As<IWordSegment.Vftbl>();
			}
			return null;
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x000FC48C File Offset: 0x000FB48C
		public static IntPtr GetAbi(IObjectReference value)
		{
			if (value != null)
			{
				return MarshalInterfaceHelper<object>.GetAbi(value);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x0016C523 File Offset: 0x0016B523
		public static WordSegment FromAbi(IntPtr thisPtr)
		{
			return WordSegment.FromAbi(thisPtr);
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x0016C52B File Offset: 0x0016B52B
		public static IntPtr FromManaged(WordSegment obj)
		{
			if (obj != null)
			{
				return WordSegment.CreateMarshaler(obj).GetRef();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06001CD2 RID: 7378 RVA: 0x0016C541 File Offset: 0x0016B541
		public static MarshalInterfaceHelper<WordSegment>.MarshalerArray CreateMarshalerArray(WordSegment[] array)
		{
			return MarshalInterfaceHelper<WordSegment>.CreateMarshalerArray(array, (WordSegment o) => WordSegment.CreateMarshaler(o));
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x0016C568 File Offset: 0x0016B568
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			return MarshalInterfaceHelper<WordSegment>.GetAbiArray(box);
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x0016C570 File Offset: 0x0016B570
		public static WordSegment[] FromAbiArray(object box)
		{
			return MarshalInterfaceHelper<WordSegment>.FromAbiArray(box, new Func<IntPtr, WordSegment>(WordSegment.FromAbi));
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x0016C584 File Offset: 0x0016B584
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(WordSegment[] array)
		{
			return MarshalInterfaceHelper<WordSegment>.FromManagedArray(array, (WordSegment o) => WordSegment.FromManaged(o));
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x0016BAE3 File Offset: 0x0016AAE3
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInspectable.DisposeMarshaler(value);
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x0016C5AB File Offset: 0x0016B5AB
		public static void DisposeMarshalerArray(MarshalInterfaceHelper<WordSegment>.MarshalerArray array)
		{
			MarshalInterfaceHelper<WordSegment>.DisposeMarshalerArray(array);
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x0016BAF8 File Offset: 0x0016AAF8
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInspectable.DisposeAbi(abi);
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x0016BB00 File Offset: 0x0016AB00
		public static void DisposeAbiArray(object box)
		{
			MarshalInspectable.DisposeAbiArray(box);
		}
	}
}
