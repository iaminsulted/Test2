using System;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x020000A9 RID: 169
	[ObjectReferenceWrapper("_obj")]
	[Guid("AF86E2E0-B12D-4c6a-9C5A-D7AA65101E90")]
	internal class IInspectable
	{
		// Token: 0x06000274 RID: 628 RVA: 0x000FACB9 File Offset: 0x000F9CB9
		public static IInspectable FromAbi(IntPtr thisPtr)
		{
			return new IInspectable(ObjectReference<IInspectable.Vftbl>.FromAbi(thisPtr));
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000275 RID: 629 RVA: 0x000FACC6 File Offset: 0x000F9CC6
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x000FACD3 File Offset: 0x000F9CD3
		public static implicit operator IInspectable(IObjectReference obj)
		{
			return obj.As<IInspectable.Vftbl>();
		}

		// Token: 0x06000277 RID: 631 RVA: 0x000FACE0 File Offset: 0x000F9CE0
		public static implicit operator IInspectable(ObjectReference<IInspectable.Vftbl> obj)
		{
			return new IInspectable(obj);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x000FACE8 File Offset: 0x000F9CE8
		public ObjectReference<I> As<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000279 RID: 633 RVA: 0x000FACF5 File Offset: 0x000F9CF5
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x000FACFD File Offset: 0x000F9CFD
		public IInspectable(IObjectReference obj) : this(obj.As<IInspectable.Vftbl>())
		{
		}

		// Token: 0x0600027B RID: 635 RVA: 0x000FAD0B File Offset: 0x000F9D0B
		public IInspectable(ObjectReference<IInspectable.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x000FAD1C File Offset: 0x000F9D1C
		public unsafe string GetRuntimeClassName(bool noThrow = false)
		{
			IntPtr hstring = 0;
			string result;
			try
			{
				int num = this._obj.Vftbl.GetRuntimeClassName(this.ThisPtr, out hstring);
				if (num != 0)
				{
					if (noThrow)
					{
						return null;
					}
					Marshal.ThrowExceptionForHR(num);
				}
				uint length;
				result = new string(Platform.WindowsGetStringRawBuffer(hstring, &length), 0, (int)length);
			}
			finally
			{
				Platform.WindowsDeleteString(hstring);
			}
			return result;
		}

		// Token: 0x040005BA RID: 1466
		private readonly ObjectReference<IInspectable.Vftbl> _obj;

		// Token: 0x0200088E RID: 2190
		[Guid("AF86E2E0-B12D-4c6a-9C5A-D7AA65101E90")]
		internal struct Vftbl
		{
			// Token: 0x06008039 RID: 32825 RVA: 0x00321DC0 File Offset: 0x00320DC0
			static Vftbl()
			{
				Marshal.StructureToPtr<IInspectable.Vftbl>(IInspectable.Vftbl.AbiToProjectionVftable, IInspectable.Vftbl.AbiToProjectionVftablePtr, false);
			}

			// Token: 0x0600803A RID: 32826 RVA: 0x00321E40 File Offset: 0x00320E40
			private static int Do_Abi_GetIids(IntPtr pThis, out uint iidCount, out Guid[] iids)
			{
				iidCount = 0U;
				iids = null;
				try
				{
					iids = ComWrappersSupport.GetInspectableInfo(pThis).IIDs;
					iidCount = (uint)iids.Length;
				}
				catch (Exception ex)
				{
					return ex.HResult;
				}
				return 0;
			}

			// Token: 0x0600803B RID: 32827 RVA: 0x00321E84 File Offset: 0x00320E84
			private static int Do_Abi_GetRuntimeClassName(IntPtr pThis, out IntPtr className)
			{
				className = 0;
				try
				{
					string runtimeClassName = ComWrappersSupport.GetInspectableInfo(pThis).RuntimeClassName;
					className = MarshalString.FromManaged(runtimeClassName);
				}
				catch (Exception ex)
				{
					return ex.HResult;
				}
				return 0;
			}

			// Token: 0x0600803C RID: 32828 RVA: 0x00321ECC File Offset: 0x00320ECC
			private static int Do_Abi_GetTrustLevel(IntPtr pThis, out TrustLevel trustLevel)
			{
				trustLevel = TrustLevel.BaseTrust;
				return 0;
			}

			// Token: 0x04003BDB RID: 15323
			public IUnknownVftbl IUnknownVftbl;

			// Token: 0x04003BDC RID: 15324
			public IInspectable.Vftbl._GetIids GetIids;

			// Token: 0x04003BDD RID: 15325
			public IInspectable.Vftbl._GetRuntimeClassName GetRuntimeClassName;

			// Token: 0x04003BDE RID: 15326
			public IInspectable.Vftbl._GetTrustLevel GetTrustLevel;

			// Token: 0x04003BDF RID: 15327
			public static readonly IInspectable.Vftbl AbiToProjectionVftable = new IInspectable.Vftbl
			{
				IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
				GetIids = new IInspectable.Vftbl._GetIids(IInspectable.Vftbl.Do_Abi_GetIids),
				GetRuntimeClassName = new IInspectable.Vftbl._GetRuntimeClassName(IInspectable.Vftbl.Do_Abi_GetRuntimeClassName),
				GetTrustLevel = new IInspectable.Vftbl._GetTrustLevel(IInspectable.Vftbl.Do_Abi_GetTrustLevel)
			};

			// Token: 0x04003BE0 RID: 15328
			public static readonly IntPtr AbiToProjectionVftablePtr = Marshal.AllocHGlobal(Marshal.SizeOf<IInspectable.Vftbl>());

			// Token: 0x02000C6F RID: 3183
			// (Invoke) Token: 0x060091F9 RID: 37369
			internal delegate int _GetIids(IntPtr pThis, out uint iidCount, out Guid[] iids);

			// Token: 0x02000C70 RID: 3184
			// (Invoke) Token: 0x060091FD RID: 37373
			internal delegate int _GetRuntimeClassName(IntPtr pThis, out IntPtr className);

			// Token: 0x02000C71 RID: 3185
			// (Invoke) Token: 0x06009201 RID: 37377
			internal delegate int _GetTrustLevel(IntPtr pThis, out TrustLevel trustLevel);
		}
	}
}
