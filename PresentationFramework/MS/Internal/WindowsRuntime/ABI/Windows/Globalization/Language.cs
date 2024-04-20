using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MS.Internal.WindowsRuntime.Windows.Globalization;
using WinRT;

namespace MS.Internal.WindowsRuntime.ABI.Windows.Globalization
{
	// Token: 0x020002F5 RID: 757
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal struct Language
	{
		// Token: 0x06001C72 RID: 7282 RVA: 0x0016BA48 File Offset: 0x0016AA48
		public static IObjectReference CreateMarshaler(Language obj)
		{
			if (obj != null)
			{
				return MarshalInspectable.CreateMarshaler(obj, true).As<ILanguage.Vftbl>();
			}
			return null;
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x000FC48C File Offset: 0x000FB48C
		public static IntPtr GetAbi(IObjectReference value)
		{
			if (value != null)
			{
				return MarshalInterfaceHelper<object>.GetAbi(value);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x0016BA5B File Offset: 0x0016AA5B
		public static Language FromAbi(IntPtr thisPtr)
		{
			return Language.FromAbi(thisPtr);
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x0016BA63 File Offset: 0x0016AA63
		public static IntPtr FromManaged(Language obj)
		{
			if (obj != null)
			{
				return Language.CreateMarshaler(obj).GetRef();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x0016BA79 File Offset: 0x0016AA79
		public static MarshalInterfaceHelper<Language>.MarshalerArray CreateMarshalerArray(Language[] array)
		{
			return MarshalInterfaceHelper<Language>.CreateMarshalerArray(array, (Language o) => Language.CreateMarshaler(o));
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x0016BAA0 File Offset: 0x0016AAA0
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			return MarshalInterfaceHelper<Language>.GetAbiArray(box);
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x0016BAA8 File Offset: 0x0016AAA8
		public static Language[] FromAbiArray(object box)
		{
			return MarshalInterfaceHelper<Language>.FromAbiArray(box, new Func<IntPtr, Language>(Language.FromAbi));
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x0016BABC File Offset: 0x0016AABC
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(Language[] array)
		{
			return MarshalInterfaceHelper<Language>.FromManagedArray(array, (Language o) => Language.FromManaged(o));
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x0016BAE3 File Offset: 0x0016AAE3
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInspectable.DisposeMarshaler(value);
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x0016BAEB File Offset: 0x0016AAEB
		public static void DisposeMarshalerArray(MarshalInterfaceHelper<Language>.MarshalerArray array)
		{
			MarshalInterfaceHelper<Language>.DisposeMarshalerArray(array);
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x0016BAF8 File Offset: 0x0016AAF8
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInspectable.DisposeAbi(abi);
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x0016BB00 File Offset: 0x0016AB00
		public static void DisposeAbiArray(object box)
		{
			MarshalInspectable.DisposeAbiArray(box);
		}
	}
}
