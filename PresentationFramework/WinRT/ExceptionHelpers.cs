using System;
using System.Collections;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using ABI.WinRT.Interop;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x020000A4 RID: 164
	internal static class ExceptionHelpers
	{
		// Token: 0x0600025E RID: 606
		[DllImport("oleaut32.dll")]
		private static extern int SetErrorInfo(uint dwReserved, IntPtr perrinfo);

		// Token: 0x0600025F RID: 607 RVA: 0x000FA044 File Offset: 0x000F9044
		static ExceptionHelpers()
		{
			IntPtr intPtr = Platform.LoadLibraryExW("api-ms-win-core-winrt-error-l1-1-1.dll", IntPtr.Zero, 2048U);
			if (intPtr != IntPtr.Zero)
			{
				ExceptionHelpers.getRestrictedErrorInfo = Platform.GetProcAddress<ExceptionHelpers.GetRestrictedErrorInfo>(intPtr);
				ExceptionHelpers.setRestrictedErrorInfo = Platform.GetProcAddress<ExceptionHelpers.SetRestrictedErrorInfo>(intPtr);
				ExceptionHelpers.roOriginateLanguageException = Platform.GetProcAddress<ExceptionHelpers.RoOriginateLanguageException>(intPtr);
				ExceptionHelpers.roReportUnhandledError = Platform.GetProcAddress<ExceptionHelpers.RoReportUnhandledError>(intPtr);
				return;
			}
			intPtr = Platform.LoadLibraryExW("api-ms-win-core-winrt-error-l1-1-0.dll", IntPtr.Zero, 2048U);
			if (intPtr != IntPtr.Zero)
			{
				ExceptionHelpers.getRestrictedErrorInfo = Platform.GetProcAddress<ExceptionHelpers.GetRestrictedErrorInfo>(intPtr);
				ExceptionHelpers.setRestrictedErrorInfo = Platform.GetProcAddress<ExceptionHelpers.SetRestrictedErrorInfo>(intPtr);
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x000FA0D8 File Offset: 0x000F90D8
		public static void ThrowExceptionForHR(int hr)
		{
			bool flag;
			Exception exceptionForHR = ExceptionHelpers.GetExceptionForHR(hr, true, out flag);
			if (flag)
			{
				ExceptionDispatchInfo.Capture(exceptionForHR).Throw();
				return;
			}
			if (exceptionForHR != null)
			{
				throw exceptionForHR;
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x000FA104 File Offset: 0x000F9104
		public static Exception GetExceptionForHR(int hr)
		{
			bool flag;
			return ExceptionHelpers.GetExceptionForHR(hr, false, out flag);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x000FA11C File Offset: 0x000F911C
		private static Exception GetExceptionForHR(int hr, bool useGlobalErrorState, out bool restoredExceptionFromGlobalState)
		{
			restoredExceptionFromGlobalState = false;
			if (hr >= 0)
			{
				return null;
			}
			ObjectReference<ABI.WinRT.Interop.IErrorInfo.Vftbl> objectReference = null;
			IObjectReference restrictedErrorObject = null;
			string text = null;
			string restrictedError = null;
			string restrictedErrorReference = null;
			string restrictedCapabilitySid = null;
			bool hasRestrictedLanguageErrorObject = false;
			Exception ex;
			if (useGlobalErrorState && ExceptionHelpers.getRestrictedErrorInfo != null)
			{
				IntPtr value;
				Marshal.ThrowExceptionForHR(ExceptionHelpers.getRestrictedErrorInfo(out value));
				if (value != IntPtr.Zero)
				{
					IObjectReference objectReference2 = ObjectReference<ABI.WinRT.Interop.IRestrictedErrorInfo.Vftbl>.Attach(ref value);
					restrictedErrorObject = objectReference2.As<ABI.WinRT.Interop.IRestrictedErrorInfo.Vftbl>();
					ABI.WinRT.Interop.IRestrictedErrorInfo restrictedErrorInfo = new ABI.WinRT.Interop.IRestrictedErrorInfo(objectReference2);
					int num;
					restrictedErrorInfo.GetErrorDetails(out text, out num, out restrictedError, out restrictedCapabilitySid);
					restrictedErrorReference = restrictedErrorInfo.GetReference();
					ObjectReference<ABI.WinRT.Interop.ILanguageExceptionErrorInfo.Vftbl> obj;
					if (objectReference2.TryAs<ABI.WinRT.Interop.ILanguageExceptionErrorInfo.Vftbl>(out obj) >= 0)
					{
						using (IObjectReference languageException = ((WinRT.Interop.ILanguageExceptionErrorInfo)new ABI.WinRT.Interop.ILanguageExceptionErrorInfo(obj)).GetLanguageException())
						{
							if (languageException != null)
							{
								if (languageException.IsReferenceToManagedObject)
								{
									ex = ComWrappersSupport.FindObject<Exception>(languageException.ThisPtr);
									if (ExceptionHelpers.GetHRForException(ex) == hr)
									{
										restoredExceptionFromGlobalState = true;
										return ex;
									}
								}
								else
								{
									hasRestrictedLanguageErrorObject = true;
								}
							}
							goto IL_DF;
						}
					}
					if (hr == num)
					{
						objectReference2.TryAs<ABI.WinRT.Interop.IErrorInfo.Vftbl>(out objectReference);
					}
				}
			}
			IL_DF:
			using (objectReference)
			{
				if (hr - -2147483635 <= 1 || hr == -2147483624 || hr == -2147009196)
				{
					ex = new InvalidOperationException(text);
				}
				else
				{
					ex = Marshal.GetExceptionForHR(hr, (objectReference != null) ? objectReference.ThisPtr : ((IntPtr)(-1)));
				}
			}
			ex.AddExceptionDataForRestrictedErrorInfo(text, restrictedError, restrictedErrorReference, restrictedCapabilitySid, restrictedErrorObject, hasRestrictedLanguageErrorObject);
			return ex;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x000FA284 File Offset: 0x000F9284
		public unsafe static void SetErrorInfo(Exception ex)
		{
			if (ExceptionHelpers.getRestrictedErrorInfo != null && ExceptionHelpers.setRestrictedErrorInfo != null && ExceptionHelpers.roOriginateLanguageException != null)
			{
				IObjectReference objectReference;
				if (ex.TryGetRestrictedLanguageErrorObject(out objectReference))
				{
					using (objectReference)
					{
						ExceptionHelpers.setRestrictedErrorInfo(objectReference.ThisPtr);
						return;
					}
				}
				string text = ex.Message;
				if (string.IsNullOrEmpty(text))
				{
					text = ex.GetType().FullName;
				}
				IntPtr zero;
				if (Platform.WindowsCreateString(text, text.Length, &zero) != 0)
				{
					zero = IntPtr.Zero;
				}
				using (IObjectReference objectReference3 = ComWrappersSupport.CreateCCWForObject(ex))
				{
					ExceptionHelpers.roOriginateLanguageException(ExceptionHelpers.GetHRForException(ex), zero, objectReference3.ThisPtr);
					return;
				}
			}
			using (IObjectReference objectReference4 = ComWrappersSupport.CreateCCWForObject(new ManagedExceptionErrorInfo(ex)))
			{
				ExceptionHelpers.SetErrorInfo(0U, objectReference4.ThisPtr);
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x000FA390 File Offset: 0x000F9390
		public static void ReportUnhandledError(Exception ex)
		{
			ExceptionHelpers.SetErrorInfo(ex);
			if (ExceptionHelpers.getRestrictedErrorInfo != null && ExceptionHelpers.roReportUnhandledError != null)
			{
				IntPtr intPtr;
				Marshal.ThrowExceptionForHR(ExceptionHelpers.getRestrictedErrorInfo(out intPtr));
				using (ObjectReference<IUnknownVftbl> objectReference = ObjectReference<IUnknownVftbl>.Attach(ref intPtr))
				{
					ExceptionHelpers.roReportUnhandledError(objectReference.ThisPtr);
				}
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x000FA3F8 File Offset: 0x000F93F8
		public static int GetHRForException(Exception ex)
		{
			int hresult = ex.HResult;
			IObjectReference objectReference;
			if (ex.TryGetRestrictedLanguageErrorObject(out objectReference))
			{
				string text;
				string text2;
				string text3;
				objectReference.AsType<ABI.WinRT.Interop.IRestrictedErrorInfo>().GetErrorDetails(out text, out hresult, out text2, out text3);
			}
			if (hresult == -2146232798)
			{
				return -2147483629;
			}
			return hresult;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x000FA438 File Offset: 0x000F9438
		internal static void AddExceptionDataForRestrictedErrorInfo(this Exception ex, string description, string restrictedError, string restrictedErrorReference, string restrictedCapabilitySid, IObjectReference restrictedErrorObject, bool hasRestrictedLanguageErrorObject = false)
		{
			IDictionary data = ex.Data;
			if (data != null)
			{
				data.Add("Description", description);
				data.Add("RestrictedDescription", restrictedError);
				data.Add("RestrictedErrorReference", restrictedErrorReference);
				data.Add("RestrictedCapabilitySid", restrictedCapabilitySid);
				data.Add("__RestrictedErrorObjectReference", (restrictedErrorObject == null) ? null : new ExceptionHelpers.__RestrictedErrorObject(restrictedErrorObject));
				data.Add("__HasRestrictedLanguageErrorObject", hasRestrictedLanguageErrorObject);
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x000FA4AC File Offset: 0x000F94AC
		internal static bool TryGetRestrictedLanguageErrorObject(this Exception ex, out IObjectReference restrictedErrorObject)
		{
			restrictedErrorObject = null;
			IDictionary data = ex.Data;
			if (data != null && data.Contains("__HasRestrictedLanguageErrorObject"))
			{
				if (data.Contains("__RestrictedErrorObjectReference"))
				{
					ExceptionHelpers.__RestrictedErrorObject _RestrictedErrorObject = data["__RestrictedErrorObjectReference"] as ExceptionHelpers.__RestrictedErrorObject;
					if (_RestrictedErrorObject != null)
					{
						restrictedErrorObject = _RestrictedErrorObject.RealErrorObject;
					}
				}
				return (bool)data["__HasRestrictedLanguageErrorObject"];
			}
			return false;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x000FA510 File Offset: 0x000F9510
		public static Exception AttachRestrictedErrorInfo(Exception e)
		{
			if (e != null)
			{
				try
				{
					IntPtr value;
					Marshal.ThrowExceptionForHR(ExceptionHelpers.getRestrictedErrorInfo(out value));
					if (value != IntPtr.Zero)
					{
						IObjectReference objectReference = ObjectReference<ABI.WinRT.Interop.IRestrictedErrorInfo.Vftbl>.Attach(ref value);
						ABI.WinRT.Interop.IRestrictedErrorInfo restrictedErrorInfo = new ABI.WinRT.Interop.IRestrictedErrorInfo(objectReference);
						string description;
						int num;
						string restrictedError;
						string restrictedCapabilitySid;
						restrictedErrorInfo.GetErrorDetails(out description, out num, out restrictedError, out restrictedCapabilitySid);
						if (e.HResult == num)
						{
							e.AddExceptionDataForRestrictedErrorInfo(description, restrictedError, restrictedErrorInfo.GetReference(), restrictedCapabilitySid, objectReference.As<ABI.WinRT.Interop.IRestrictedErrorInfo.Vftbl>(), false);
						}
					}
				}
				catch
				{
				}
			}
			return e;
		}

		// Token: 0x04000593 RID: 1427
		private const int COR_E_OBJECTDISPOSED = -2146232798;

		// Token: 0x04000594 RID: 1428
		private const int RO_E_CLOSED = -2147483629;

		// Token: 0x04000595 RID: 1429
		internal const int E_BOUNDS = -2147483637;

		// Token: 0x04000596 RID: 1430
		internal const int E_CHANGED_STATE = -2147483636;

		// Token: 0x04000597 RID: 1431
		private const int E_ILLEGAL_STATE_CHANGE = -2147483635;

		// Token: 0x04000598 RID: 1432
		private const int E_ILLEGAL_METHOD_CALL = -2147483634;

		// Token: 0x04000599 RID: 1433
		private const int E_ILLEGAL_DELEGATE_ASSIGNMENT = -2147483624;

		// Token: 0x0400059A RID: 1434
		private const int APPMODEL_ERROR_NO_PACKAGE = -2147009196;

		// Token: 0x0400059B RID: 1435
		internal const int E_XAMLPARSEFAILED = -2144665590;

		// Token: 0x0400059C RID: 1436
		internal const int E_LAYOUTCYCLE = -2144665580;

		// Token: 0x0400059D RID: 1437
		internal const int E_ELEMENTNOTENABLED = -2144665570;

		// Token: 0x0400059E RID: 1438
		internal const int E_ELEMENTNOTAVAILABLE = -2144665569;

		// Token: 0x0400059F RID: 1439
		private static ExceptionHelpers.GetRestrictedErrorInfo getRestrictedErrorInfo;

		// Token: 0x040005A0 RID: 1440
		private static ExceptionHelpers.SetRestrictedErrorInfo setRestrictedErrorInfo;

		// Token: 0x040005A1 RID: 1441
		private static ExceptionHelpers.RoOriginateLanguageException roOriginateLanguageException;

		// Token: 0x040005A2 RID: 1442
		private static ExceptionHelpers.RoReportUnhandledError roReportUnhandledError;

		// Token: 0x02000888 RID: 2184
		// (Invoke) Token: 0x06008023 RID: 32803
		internal delegate int GetRestrictedErrorInfo(out IntPtr ppRestrictedErrorInfo);

		// Token: 0x02000889 RID: 2185
		// (Invoke) Token: 0x06008027 RID: 32807
		internal delegate int SetRestrictedErrorInfo(IntPtr pRestrictedErrorInfo);

		// Token: 0x0200088A RID: 2186
		// (Invoke) Token: 0x0600802B RID: 32811
		internal delegate int RoOriginateLanguageException(int error, IntPtr message, IntPtr langaugeException);

		// Token: 0x0200088B RID: 2187
		// (Invoke) Token: 0x0600802F RID: 32815
		internal delegate int RoReportUnhandledError(IntPtr pRestrictedErrorInfo);

		// Token: 0x0200088C RID: 2188
		[Serializable]
		internal class __RestrictedErrorObject
		{
			// Token: 0x06008032 RID: 32818 RVA: 0x00321D70 File Offset: 0x00320D70
			internal __RestrictedErrorObject(IObjectReference errorObject)
			{
				this._realErrorObject = errorObject;
			}

			// Token: 0x17001D7A RID: 7546
			// (get) Token: 0x06008033 RID: 32819 RVA: 0x00321D7F File Offset: 0x00320D7F
			public IObjectReference RealErrorObject
			{
				get
				{
					return this._realErrorObject;
				}
			}

			// Token: 0x04003BD6 RID: 15318
			[NonSerialized]
			private readonly IObjectReference _realErrorObject;
		}
	}
}
