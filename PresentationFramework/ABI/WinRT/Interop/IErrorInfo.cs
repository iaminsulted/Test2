using System;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.WinRT.Interop
{
	// Token: 0x0200008F RID: 143
	[Guid("1CF2B120-547D-101B-8E65-08002B2BD119")]
	internal class IErrorInfo : IErrorInfo
	{
		// Token: 0x060001DA RID: 474 RVA: 0x000F8DFF File Offset: 0x000F7DFF
		public static ObjectReference<IErrorInfo.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<IErrorInfo.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x000F8E07 File Offset: 0x000F7E07
		public static implicit operator IErrorInfo(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IErrorInfo(obj);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x000F8E14 File Offset: 0x000F7E14
		public static implicit operator IErrorInfo(ObjectReference<IErrorInfo.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IErrorInfo(obj);
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060001DD RID: 477 RVA: 0x000F8E21 File Offset: 0x000F7E21
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000F8E2E File Offset: 0x000F7E2E
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x060001DF RID: 479 RVA: 0x000F8E3B File Offset: 0x000F7E3B
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000F8E48 File Offset: 0x000F7E48
		public IErrorInfo(IObjectReference obj) : this(obj.As<IErrorInfo.Vftbl>())
		{
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x000F8E56 File Offset: 0x000F7E56
		public IErrorInfo(ObjectReference<IErrorInfo.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x000F8E68 File Offset: 0x000F7E68
		public Guid GetGuid()
		{
			Guid result;
			Marshal.ThrowExceptionForHR(this._obj.Vftbl.GetGuid_0(this.ThisPtr, out result));
			return result;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x000F8E98 File Offset: 0x000F7E98
		public string GetSource()
		{
			IntPtr intPtr = 0;
			string result;
			try
			{
				Marshal.ThrowExceptionForHR(this._obj.Vftbl.GetSource_1(this.ThisPtr, out intPtr));
				result = ((intPtr != IntPtr.Zero) ? Marshal.PtrToStringBSTR(intPtr) : string.Empty);
			}
			finally
			{
				Marshal.FreeBSTR(intPtr);
			}
			return result;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x000F8F04 File Offset: 0x000F7F04
		public string GetDescription()
		{
			IntPtr intPtr = 0;
			string result;
			try
			{
				Marshal.ThrowExceptionForHR(this._obj.Vftbl.GetDescription_2(this.ThisPtr, out intPtr));
				result = ((intPtr != IntPtr.Zero) ? Marshal.PtrToStringBSTR(intPtr) : string.Empty);
			}
			finally
			{
				Marshal.FreeBSTR(intPtr);
			}
			return result;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000F8F70 File Offset: 0x000F7F70
		public string GetHelpFile()
		{
			IntPtr intPtr = 0;
			string result;
			try
			{
				Marshal.ThrowExceptionForHR(this._obj.Vftbl.GetHelpFile_3(this.ThisPtr, out intPtr));
				result = ((intPtr != IntPtr.Zero) ? Marshal.PtrToStringBSTR(intPtr) : string.Empty);
			}
			finally
			{
				Marshal.FreeBSTR(intPtr);
			}
			return result;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000F8FDC File Offset: 0x000F7FDC
		public string GetHelpFileContent()
		{
			IntPtr intPtr = 0;
			string result;
			try
			{
				Marshal.ThrowExceptionForHR(this._obj.Vftbl.GetHelpFileContent_4(this.ThisPtr, out intPtr));
				result = ((intPtr != IntPtr.Zero) ? Marshal.PtrToStringBSTR(intPtr) : string.Empty);
			}
			finally
			{
				Marshal.FreeBSTR(intPtr);
			}
			return result;
		}

		// Token: 0x04000577 RID: 1399
		protected readonly ObjectReference<IErrorInfo.Vftbl> _obj;

		// Token: 0x0200087D RID: 2173
		[Guid("1CF2B120-547D-101B-8E65-08002B2BD119")]
		internal struct Vftbl
		{
			// Token: 0x0600800A RID: 32778 RVA: 0x003219FC File Offset: 0x003209FC
			unsafe static Vftbl()
			{
				IntPtr* value = (IntPtr*)((void*)Marshal.AllocCoTaskMem(Marshal.SizeOf<IErrorInfo.Vftbl>()));
				Marshal.StructureToPtr<IErrorInfo.Vftbl>(IErrorInfo.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)value), false);
				IErrorInfo.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)value);
			}

			// Token: 0x0600800B RID: 32779 RVA: 0x00321AB0 File Offset: 0x00320AB0
			private static int Do_Abi_GetGuid_0(IntPtr thisPtr, out Guid guid)
			{
				guid = default(Guid);
				try
				{
					guid = ComWrappersSupport.FindObject<IErrorInfo>(thisPtr).GetGuid();
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0600800C RID: 32780 RVA: 0x00321AF8 File Offset: 0x00320AF8
			private static int Do_Abi_GetSource_1(IntPtr thisPtr, out IntPtr source)
			{
				source = IntPtr.Zero;
				try
				{
					string source2 = ComWrappersSupport.FindObject<IErrorInfo>(thisPtr).GetSource();
					source = Marshal.StringToBSTR(source2);
				}
				catch (Exception ex)
				{
					Marshal.FreeBSTR(source);
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0600800D RID: 32781 RVA: 0x00321B4C File Offset: 0x00320B4C
			private static int Do_Abi_GetDescription_2(IntPtr thisPtr, out IntPtr description)
			{
				description = IntPtr.Zero;
				try
				{
					string description2 = ComWrappersSupport.FindObject<IErrorInfo>(thisPtr).GetDescription();
					description = Marshal.StringToBSTR(description2);
				}
				catch (Exception ex)
				{
					Marshal.FreeBSTR(description);
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0600800E RID: 32782 RVA: 0x00321BA0 File Offset: 0x00320BA0
			private static int Do_Abi_GetHelpFile_3(IntPtr thisPtr, out IntPtr helpFile)
			{
				helpFile = IntPtr.Zero;
				try
				{
					string helpFile2 = ComWrappersSupport.FindObject<IErrorInfo>(thisPtr).GetHelpFile();
					helpFile = Marshal.StringToBSTR(helpFile2);
				}
				catch (Exception ex)
				{
					Marshal.FreeBSTR(helpFile);
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0600800F RID: 32783 RVA: 0x00321BF4 File Offset: 0x00320BF4
			private static int Do_Abi_GetHelpFileContent_4(IntPtr thisPtr, out IntPtr helpFileContent)
			{
				helpFileContent = IntPtr.Zero;
				try
				{
					string helpFileContent2 = ComWrappersSupport.FindObject<IErrorInfo>(thisPtr).GetHelpFileContent();
					helpFileContent = Marshal.StringToBSTR(helpFileContent2);
				}
				catch (Exception ex)
				{
					Marshal.FreeBSTR(helpFileContent);
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04003BBD RID: 15293
			public IUnknownVftbl IUnknownVftbl;

			// Token: 0x04003BBE RID: 15294
			public IErrorInfo.Vftbl._GetGuid GetGuid_0;

			// Token: 0x04003BBF RID: 15295
			public IErrorInfo.Vftbl._GetBstr GetSource_1;

			// Token: 0x04003BC0 RID: 15296
			public IErrorInfo.Vftbl._GetBstr GetDescription_2;

			// Token: 0x04003BC1 RID: 15297
			public IErrorInfo.Vftbl._GetBstr GetHelpFile_3;

			// Token: 0x04003BC2 RID: 15298
			public IErrorInfo.Vftbl._GetBstr GetHelpFileContent_4;

			// Token: 0x04003BC3 RID: 15299
			private static readonly IErrorInfo.Vftbl AbiToProjectionVftable = new IErrorInfo.Vftbl
			{
				IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
				GetGuid_0 = new IErrorInfo.Vftbl._GetGuid(IErrorInfo.Vftbl.Do_Abi_GetGuid_0),
				GetSource_1 = new IErrorInfo.Vftbl._GetBstr(IErrorInfo.Vftbl.Do_Abi_GetSource_1),
				GetDescription_2 = new IErrorInfo.Vftbl._GetBstr(IErrorInfo.Vftbl.Do_Abi_GetDescription_2),
				GetHelpFile_3 = new IErrorInfo.Vftbl._GetBstr(IErrorInfo.Vftbl.Do_Abi_GetHelpFile_3),
				GetHelpFileContent_4 = new IErrorInfo.Vftbl._GetBstr(IErrorInfo.Vftbl.Do_Abi_GetHelpFileContent_4)
			};

			// Token: 0x04003BC4 RID: 15300
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x02000C68 RID: 3176
			// (Invoke) Token: 0x060091DD RID: 37341
			internal delegate int _GetGuid(IntPtr thisPtr, out Guid guid);

			// Token: 0x02000C69 RID: 3177
			// (Invoke) Token: 0x060091E1 RID: 37345
			internal delegate int _GetBstr(IntPtr thisPtr, out IntPtr bstr);
		}
	}
}
