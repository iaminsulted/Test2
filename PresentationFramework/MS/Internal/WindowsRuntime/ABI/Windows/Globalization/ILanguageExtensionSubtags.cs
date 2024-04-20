using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MS.Internal.WindowsRuntime.ABI.System.Collections.Generic;
using MS.Internal.WindowsRuntime.Windows.Globalization;
using WinRT;

namespace MS.Internal.WindowsRuntime.ABI.Windows.Globalization
{
	// Token: 0x020002ED RID: 749
	[ObjectReferenceWrapper("_obj")]
	[Guid("7D7DAF45-368D-4364-852B-DEC927037B85")]
	internal class ILanguageExtensionSubtags : ILanguageExtensionSubtags
	{
		// Token: 0x06001C4D RID: 7245 RVA: 0x0016B6C2 File Offset: 0x0016A6C2
		public static ObjectReference<ILanguageExtensionSubtags.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<ILanguageExtensionSubtags.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06001C4E RID: 7246 RVA: 0x0016B6CA File Offset: 0x0016A6CA
		public static implicit operator ILanguageExtensionSubtags(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new ILanguageExtensionSubtags(obj);
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001C4F RID: 7247 RVA: 0x0016B6D7 File Offset: 0x0016A6D7
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001C50 RID: 7248 RVA: 0x0016B6DF File Offset: 0x0016A6DF
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06001C51 RID: 7249 RVA: 0x0016B6EC File Offset: 0x0016A6EC
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06001C52 RID: 7250 RVA: 0x0016B6F9 File Offset: 0x0016A6F9
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06001C53 RID: 7251 RVA: 0x0016B706 File Offset: 0x0016A706
		public ILanguageExtensionSubtags(IObjectReference obj) : this(obj.As<ILanguageExtensionSubtags.Vftbl>())
		{
		}

		// Token: 0x06001C54 RID: 7252 RVA: 0x0016B714 File Offset: 0x0016A714
		public ILanguageExtensionSubtags(ObjectReference<ILanguageExtensionSubtags.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x0016B724 File Offset: 0x0016A724
		public IReadOnlyList<string> GetExtensionSubtags(string singleton)
		{
			MarshalString m = null;
			IntPtr intPtr = 0;
			IReadOnlyList<string> result;
			try
			{
				m = MarshalString.CreateMarshaler(singleton);
				ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.GetExtensionSubtags_0(this.ThisPtr, MarshalString.GetAbi(m), out intPtr));
				result = IReadOnlyList<string>.FromAbi(intPtr);
			}
			finally
			{
				MarshalString.DisposeMarshaler(m);
				IReadOnlyList<string>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x04000E76 RID: 3702
		protected readonly ObjectReference<ILanguageExtensionSubtags.Vftbl> _obj;

		// Token: 0x02000A31 RID: 2609
		[Guid("7D7DAF45-368D-4364-852B-DEC927037B85")]
		internal struct Vftbl
		{
			// Token: 0x06008551 RID: 34129 RVA: 0x00328E68 File Offset: 0x00327E68
			unsafe static Vftbl()
			{
				IntPtr* value = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(ILanguageExtensionSubtags.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<ILanguageExtensionSubtags.Vftbl>(ILanguageExtensionSubtags.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)value), false);
				ILanguageExtensionSubtags.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)value);
			}

			// Token: 0x06008552 RID: 34130 RVA: 0x00328EE0 File Offset: 0x00327EE0
			private static int Do_Abi_GetExtensionSubtags_0(IntPtr thisPtr, IntPtr singleton, out IntPtr value)
			{
				value = 0;
				try
				{
					IReadOnlyList<string> extensionSubtags = ComWrappersSupport.FindObject<ILanguageExtensionSubtags>(thisPtr).GetExtensionSubtags(MarshalString.FromAbi(singleton));
					value = IReadOnlyList<string>.FromManaged(extensionSubtags);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x040040E5 RID: 16613
			public IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040040E6 RID: 16614
			public ILanguageExtensionSubtags_Delegates.GetExtensionSubtags_0 GetExtensionSubtags_0;

			// Token: 0x040040E7 RID: 16615
			private static readonly ILanguageExtensionSubtags.Vftbl AbiToProjectionVftable = new ILanguageExtensionSubtags.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				GetExtensionSubtags_0 = new ILanguageExtensionSubtags_Delegates.GetExtensionSubtags_0(ILanguageExtensionSubtags.Vftbl.Do_Abi_GetExtensionSubtags_0)
			};

			// Token: 0x040040E8 RID: 16616
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
