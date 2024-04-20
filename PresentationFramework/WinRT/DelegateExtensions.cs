using System;
using System.Runtime.InteropServices;

namespace WinRT
{
	// Token: 0x02000096 RID: 150
	internal static class DelegateExtensions
	{
		// Token: 0x0600021C RID: 540 RVA: 0x000F9494 File Offset: 0x000F8494
		public static void DynamicInvokeAbi(this Delegate del, object[] invoke_params)
		{
			Marshal.ThrowExceptionForHR((int)del.DynamicInvoke(invoke_params));
		}

		// Token: 0x0600021D RID: 541 RVA: 0x000F94A7 File Offset: 0x000F84A7
		public static T AsDelegate<T>(this MulticastDelegate del)
		{
			return Marshal.GetDelegateForFunctionPointer<T>(Marshal.GetFunctionPointerForDelegate<MulticastDelegate>(del));
		}
	}
}
