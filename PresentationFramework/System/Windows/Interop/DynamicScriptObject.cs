using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using MS.Internal.Interop;
using MS.Win32;

namespace System.Windows.Interop
{
	// Token: 0x02000421 RID: 1057
	public sealed class DynamicScriptObject : DynamicObject
	{
		// Token: 0x060032F3 RID: 13043 RVA: 0x001D341B File Offset: 0x001D241B
		internal DynamicScriptObject(UnsafeNativeMethods.IDispatch scriptObject)
		{
			if (scriptObject == null)
			{
				throw new ArgumentNullException("scriptObject");
			}
			this._scriptObject = scriptObject;
			this._scriptObjectEx = (this._scriptObject as UnsafeNativeMethods.IDispatchEx);
		}

		// Token: 0x060032F4 RID: 13044 RVA: 0x001D3454 File Offset: 0x001D2454
		public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
		{
			if (binder == null)
			{
				throw new ArgumentNullException("binder");
			}
			result = this.InvokeAndReturn(binder.Name, 1, args);
			return true;
		}

		// Token: 0x060032F5 RID: 13045 RVA: 0x001D3475 File Offset: 0x001D2475
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			if (binder == null)
			{
				throw new ArgumentNullException("binder");
			}
			result = this.InvokeAndReturn(binder.Name, 2, null);
			return true;
		}

		// Token: 0x060032F6 RID: 13046 RVA: 0x001D3498 File Offset: 0x001D2498
		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			if (binder == null)
			{
				throw new ArgumentNullException("binder");
			}
			int propertyPutMethod = DynamicScriptObject.GetPropertyPutMethod(value);
			this.InvokeAndReturn(binder.Name, propertyPutMethod, new object[]
			{
				value
			});
			return true;
		}

		// Token: 0x060032F7 RID: 13047 RVA: 0x001D34D4 File Offset: 0x001D24D4
		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			if (binder == null)
			{
				throw new ArgumentNullException("binder");
			}
			if (indexes == null)
			{
				throw new ArgumentNullException("indexes");
			}
			if (BrowserInteropHelper.IsHostedInIEorWebOC && this.TryFindMemberAndInvoke(null, 1, false, indexes, out result))
			{
				return true;
			}
			if (indexes.Length != 1)
			{
				throw new ArgumentException("indexes", HRESULT.DISP_E_BADPARAMCOUNT.GetException());
			}
			object obj = indexes[0];
			if (obj == null)
			{
				throw new ArgumentOutOfRangeException("indexes");
			}
			result = this.InvokeAndReturn(obj.ToString(), 2, false, null);
			return true;
		}

		// Token: 0x060032F8 RID: 13048 RVA: 0x001D3558 File Offset: 0x001D2558
		public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
		{
			if (binder == null)
			{
				throw new ArgumentNullException("binder");
			}
			if (indexes == null)
			{
				throw new ArgumentNullException("indexes");
			}
			if (indexes.Length != 1)
			{
				throw new ArgumentException("indexes", HRESULT.DISP_E_BADPARAMCOUNT.GetException());
			}
			object obj = indexes[0];
			if (obj == null)
			{
				throw new ArgumentOutOfRangeException("indexes");
			}
			this.InvokeAndReturn(obj.ToString(), 4, false, new object[]
			{
				value
			});
			return true;
		}

		// Token: 0x060032F9 RID: 13049 RVA: 0x001D35CB File Offset: 0x001D25CB
		public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
		{
			if (binder == null)
			{
				throw new ArgumentNullException("binder");
			}
			result = this.InvokeAndReturn(null, 1, args);
			return true;
		}

		// Token: 0x060032FA RID: 13050 RVA: 0x001D35E8 File Offset: 0x001D25E8
		public override string ToString()
		{
			Guid empty = Guid.Empty;
			object obj = null;
			NativeMethods.DISPPARAMS dp = new NativeMethods.DISPPARAMS();
			int dispid;
			HRESULT hresult;
			if (this.TryGetDispIdForMember("toString", true, out dispid))
			{
				hresult = this.InvokeOnScriptObject(dispid, 1, dp, null, out obj);
			}
			else
			{
				dispid = 0;
				hresult = this.InvokeOnScriptObject(dispid, 2, dp, null, out obj);
				if (hresult.Failed)
				{
					hresult = this.InvokeOnScriptObject(dispid, 1, dp, null, out obj);
				}
			}
			if (hresult.Succeeded && obj != null)
			{
				return obj.ToString();
			}
			return base.ToString();
		}

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x060032FB RID: 13051 RVA: 0x001D3660 File Offset: 0x001D2660
		internal UnsafeNativeMethods.IDispatch ScriptObject
		{
			get
			{
				return this._scriptObject;
			}
		}

		// Token: 0x060032FC RID: 13052 RVA: 0x001D3668 File Offset: 0x001D2668
		internal unsafe bool TryFindMemberAndInvokeNonWrapped(string memberName, int flags, bool cacheDispId, object[] args, out object result)
		{
			result = null;
			int dispid;
			if (!this.TryGetDispIdForMember(memberName, cacheDispId, out dispid))
			{
				return false;
			}
			NativeMethods.DISPPARAMS dispparams = new NativeMethods.DISPPARAMS();
			int num = -3;
			if (flags == 4 || flags == 8)
			{
				dispparams.cNamedArgs = 1U;
				dispparams.rgdispidNamedArgs = new IntPtr((void*)(&num));
			}
			try
			{
				if (args != null)
				{
					args = (object[])args.Clone();
					Array.Reverse<object>(args);
					for (int i = 0; i < args.Length; i++)
					{
						DynamicScriptObject dynamicScriptObject = args[i] as DynamicScriptObject;
						if (dynamicScriptObject != null)
						{
							args[i] = dynamicScriptObject._scriptObject;
						}
						if (args[i] != null)
						{
							Type type = args[i].GetType();
							if (type.IsArray)
							{
								type = type.GetElementType();
							}
							if (!MarshalLocal.IsTypeVisibleFromCom(type) && !type.IsCOMObject && type != typeof(DateTime))
							{
								throw new ArgumentException(SR.Get("NeedToBeComVisible"));
							}
						}
					}
					dispparams.rgvarg = UnsafeNativeMethods.ArrayToVARIANTHelper.ArrayToVARIANTVector(args);
					dispparams.cArgs = (uint)args.Length;
				}
				NativeMethods.EXCEPINFO excepinfo = new NativeMethods.EXCEPINFO();
				HRESULT hrLeft = this.InvokeOnScriptObject(dispid, flags, dispparams, excepinfo, out result);
				if (hrLeft.Failed)
				{
					if (hrLeft == HRESULT.DISP_E_MEMBERNOTFOUND)
					{
						return false;
					}
					if (hrLeft == HRESULT.SCRIPT_E_REPORTED)
					{
						excepinfo.scode = hrLeft.Code;
						hrLeft = HRESULT.DISP_E_EXCEPTION;
					}
					string text = "[" + (memberName ?? "(default)") + "]";
					Exception exception = hrLeft.GetException();
					if (hrLeft == HRESULT.DISP_E_EXCEPTION)
					{
						int code = (excepinfo.scode != 0) ? excepinfo.scode : ((int)excepinfo.wCode);
						hrLeft = HRESULT.Make(true, Facility.Dispatch, code);
						throw new TargetInvocationException(text + " " + (excepinfo.bstrDescription ?? string.Empty), exception)
						{
							HelpLink = excepinfo.bstrHelpFile,
							Source = excepinfo.bstrSource
						};
					}
					if (hrLeft == HRESULT.DISP_E_BADPARAMCOUNT || hrLeft == HRESULT.DISP_E_PARAMNOTOPTIONAL)
					{
						throw new TargetParameterCountException(text, exception);
					}
					if (hrLeft == HRESULT.DISP_E_OVERFLOW || hrLeft == HRESULT.DISP_E_TYPEMISMATCH)
					{
						throw new ArgumentException(text, new InvalidCastException(exception.Message, hrLeft.Code));
					}
					throw exception;
				}
			}
			finally
			{
				if (dispparams.rgvarg != IntPtr.Zero)
				{
					UnsafeNativeMethods.ArrayToVARIANTHelper.FreeVARIANTVector(dispparams.rgvarg, args.Length);
				}
			}
			return true;
		}

		// Token: 0x060032FD RID: 13053 RVA: 0x001D38FC File Offset: 0x001D28FC
		private object InvokeAndReturn(string memberName, int flags, object[] args)
		{
			return this.InvokeAndReturn(memberName, flags, true, args);
		}

		// Token: 0x060032FE RID: 13054 RVA: 0x001D3908 File Offset: 0x001D2908
		private object InvokeAndReturn(string memberName, int flags, bool cacheDispId, object[] args)
		{
			object result;
			if (this.TryFindMemberAndInvoke(memberName, flags, cacheDispId, args, out result))
			{
				return result;
			}
			if (flags == 1)
			{
				throw new MissingMethodException(this.ToString(), memberName);
			}
			throw new MissingMemberException(this.ToString(), memberName);
		}

		// Token: 0x060032FF RID: 13055 RVA: 0x001D3943 File Offset: 0x001D2943
		private bool TryFindMemberAndInvoke(string memberName, int flags, bool cacheDispId, object[] args, out object result)
		{
			if (!this.TryFindMemberAndInvokeNonWrapped(memberName, flags, cacheDispId, args, out result))
			{
				return false;
			}
			if (result != null && Marshal.IsComObject(result))
			{
				result = new DynamicScriptObject((UnsafeNativeMethods.IDispatch)result);
			}
			return true;
		}

		// Token: 0x06003300 RID: 13056 RVA: 0x001D3978 File Offset: 0x001D2978
		private bool TryGetDispIdForMember(string memberName, bool cacheDispId, out int dispid)
		{
			dispid = 0;
			if (!string.IsNullOrEmpty(memberName) && (!cacheDispId || !this._dispIdCache.TryGetValue(memberName, out dispid)))
			{
				Guid empty = Guid.Empty;
				string[] rgszNames = new string[]
				{
					memberName
				};
				int[] array = new int[]
				{
					-1
				};
				HRESULT idsOfNames = this._scriptObject.GetIDsOfNames(ref empty, rgszNames, array.Length, Thread.CurrentThread.CurrentCulture.LCID, array);
				if (idsOfNames == HRESULT.DISP_E_UNKNOWNNAME)
				{
					return false;
				}
				idsOfNames.ThrowIfFailed();
				dispid = array[0];
				if (cacheDispId)
				{
					this._dispIdCache[memberName] = dispid;
				}
			}
			return true;
		}

		// Token: 0x06003301 RID: 13057 RVA: 0x001D3A10 File Offset: 0x001D2A10
		private HRESULT InvokeOnScriptObject(int dispid, int flags, NativeMethods.DISPPARAMS dp, NativeMethods.EXCEPINFO exInfo, out object result)
		{
			if (this._scriptObjectEx != null)
			{
				return this._scriptObjectEx.InvokeEx(dispid, Thread.CurrentThread.CurrentCulture.LCID, flags, dp, out result, exInfo, BrowserInteropHelper.HostHtmlDocumentServiceProvider);
			}
			Guid empty = Guid.Empty;
			return this._scriptObject.Invoke(dispid, ref empty, Thread.CurrentThread.CurrentCulture.LCID, flags, dp, out result, exInfo, null);
		}

		// Token: 0x06003302 RID: 13058 RVA: 0x001D3A78 File Offset: 0x001D2A78
		private static int GetPropertyPutMethod(object value)
		{
			if (value == null)
			{
				return 8;
			}
			Type type = value.GetType();
			if (type.IsValueType || type.IsArray || type == typeof(string) || type == typeof(CurrencyWrapper) || type == typeof(DBNull) || type == typeof(Missing))
			{
				return 4;
			}
			return 8;
		}

		// Token: 0x04001C11 RID: 7185
		private UnsafeNativeMethods.IDispatch _scriptObject;

		// Token: 0x04001C12 RID: 7186
		private UnsafeNativeMethods.IDispatchEx _scriptObjectEx;

		// Token: 0x04001C13 RID: 7187
		private Dictionary<string, int> _dispIdCache = new Dictionary<string, int>();
	}
}
