using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x02000099 RID: 153
	internal class DllModule
	{
		// Token: 0x0600022B RID: 555 RVA: 0x000F94E8 File Offset: 0x000F84E8
		public static DllModule Load(string fileName)
		{
			Dictionary<string, DllModule> cache = DllModule._cache;
			DllModule result;
			lock (cache)
			{
				DllModule dllModule;
				if (!DllModule._cache.TryGetValue(fileName, out dllModule))
				{
					dllModule = new DllModule(fileName);
					DllModule._cache[fileName] = dllModule;
				}
				result = dllModule;
			}
			return result;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x000F9548 File Offset: 0x000F8548
		private DllModule(string fileName)
		{
			this._fileName = fileName;
			this._moduleHandle = Platform.LoadLibraryExW(Path.Combine(DllModule._currentModuleDirectory, fileName), IntPtr.Zero, 8U);
			if (this._moduleHandle == IntPtr.Zero)
			{
				try
				{
					this._moduleHandle = NativeLibrary.Load(fileName, Assembly.GetExecutingAssembly(), null);
				}
				catch (Exception)
				{
				}
			}
			if (this._moduleHandle == IntPtr.Zero)
			{
				Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
			}
			this._GetActivationFactory = Platform.GetProcAddress<DllModule.DllGetActivationFactory>(this._moduleHandle);
			IntPtr procAddress = Platform.GetProcAddress(this._moduleHandle, "DllCanUnloadNow");
			if (procAddress != IntPtr.Zero)
			{
				this._CanUnloadNow = Marshal.GetDelegateForFunctionPointer<DllModule.DllCanUnloadNow>(procAddress);
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x000F9618 File Offset: 0x000F8618
		[return: TupleElementNames(new string[]
		{
			"obj",
			"hr"
		})]
		public ValueTuple<ObjectReference<IActivationFactoryVftbl>, int> GetActivationFactory(string runtimeClassId)
		{
			MarshalString m = MarshalString.CreateMarshaler(runtimeClassId);
			IntPtr intPtr;
			int num = this._GetActivationFactory(MarshalString.GetAbi(m), out intPtr);
			return new ValueTuple<ObjectReference<IActivationFactoryVftbl>, int>((num == 0) ? ObjectReference<IActivationFactoryVftbl>.Attach(ref intPtr) : null, num);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x000F9654 File Offset: 0x000F8654
		protected override void Finalize()
		{
			try
			{
				Dictionary<string, DllModule> cache = DllModule._cache;
				lock (cache)
				{
					DllModule._cache.Remove(this._fileName);
				}
				if (this._moduleHandle != IntPtr.Zero && !Platform.FreeLibrary(this._moduleHandle))
				{
					Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x0400057F RID: 1407
		private readonly string _fileName;

		// Token: 0x04000580 RID: 1408
		private readonly IntPtr _moduleHandle;

		// Token: 0x04000581 RID: 1409
		private readonly DllModule.DllGetActivationFactory _GetActivationFactory;

		// Token: 0x04000582 RID: 1410
		private readonly DllModule.DllCanUnloadNow _CanUnloadNow;

		// Token: 0x04000583 RID: 1411
		private static readonly string _currentModuleDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		// Token: 0x04000584 RID: 1412
		private static Dictionary<string, DllModule> _cache = new Dictionary<string, DllModule>();

		// Token: 0x02000883 RID: 2179
		// (Invoke) Token: 0x06008013 RID: 32787
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate int DllGetActivationFactory(IntPtr activatableClassId, out IntPtr activationFactory);

		// Token: 0x02000884 RID: 2180
		// (Invoke) Token: 0x06008017 RID: 32791
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate int DllCanUnloadNow();
	}
}
