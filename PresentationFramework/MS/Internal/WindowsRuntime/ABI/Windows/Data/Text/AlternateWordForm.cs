using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MS.Internal.WindowsRuntime.Windows.Data.Text;
using WinRT;

namespace MS.Internal.WindowsRuntime.ABI.Windows.Data.Text
{
	// Token: 0x020002F6 RID: 758
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal struct AlternateWordForm
	{
		// Token: 0x06001C7E RID: 7294 RVA: 0x0016BB08 File Offset: 0x0016AB08
		public static IObjectReference CreateMarshaler(AlternateWordForm obj)
		{
			if (obj != null)
			{
				return MarshalInspectable.CreateMarshaler(obj, true).As<IAlternateWordForm.Vftbl>();
			}
			return null;
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x000FC48C File Offset: 0x000FB48C
		public static IntPtr GetAbi(IObjectReference value)
		{
			if (value != null)
			{
				return MarshalInterfaceHelper<object>.GetAbi(value);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x0016BB1B File Offset: 0x0016AB1B
		public static AlternateWordForm FromAbi(IntPtr thisPtr)
		{
			return AlternateWordForm.FromAbi(thisPtr);
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x0016BB23 File Offset: 0x0016AB23
		public static IntPtr FromManaged(AlternateWordForm obj)
		{
			if (obj != null)
			{
				return AlternateWordForm.CreateMarshaler(obj).GetRef();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x0016BB39 File Offset: 0x0016AB39
		public static MarshalInterfaceHelper<AlternateWordForm>.MarshalerArray CreateMarshalerArray(AlternateWordForm[] array)
		{
			return MarshalInterfaceHelper<AlternateWordForm>.CreateMarshalerArray(array, (AlternateWordForm o) => AlternateWordForm.CreateMarshaler(o));
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x0016BB60 File Offset: 0x0016AB60
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			return MarshalInterfaceHelper<AlternateWordForm>.GetAbiArray(box);
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x0016BB68 File Offset: 0x0016AB68
		public static AlternateWordForm[] FromAbiArray(object box)
		{
			return MarshalInterfaceHelper<AlternateWordForm>.FromAbiArray(box, new Func<IntPtr, AlternateWordForm>(AlternateWordForm.FromAbi));
		}

		// Token: 0x06001C85 RID: 7301 RVA: 0x0016BB7C File Offset: 0x0016AB7C
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(AlternateWordForm[] array)
		{
			return MarshalInterfaceHelper<AlternateWordForm>.FromManagedArray(array, (AlternateWordForm o) => AlternateWordForm.FromManaged(o));
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x0016BAE3 File Offset: 0x0016AAE3
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInspectable.DisposeMarshaler(value);
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x0016BBA3 File Offset: 0x0016ABA3
		public static void DisposeMarshalerArray(MarshalInterfaceHelper<AlternateWordForm>.MarshalerArray array)
		{
			MarshalInterfaceHelper<AlternateWordForm>.DisposeMarshalerArray(array);
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x0016BAF8 File Offset: 0x0016AAF8
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInspectable.DisposeAbi(abi);
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x0016BB00 File Offset: 0x0016AB00
		public static void DisposeAbiArray(object box)
		{
			MarshalInspectable.DisposeAbiArray(box);
		}
	}
}
