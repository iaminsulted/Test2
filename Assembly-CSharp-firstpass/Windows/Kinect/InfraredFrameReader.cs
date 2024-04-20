using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x02000070 RID: 112
	public sealed class InfraredFrameReader : IDisposable, INativeWrapper
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x000207E5 File Offset: 0x0001E9E5
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x000207ED File Offset: 0x0001E9ED
		internal InfraredFrameReader(IntPtr pNative)
		{
			this._pNative = pNative;
			InfraredFrameReader.Windows_Kinect_InfraredFrameReader_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00020808 File Offset: 0x0001EA08
		~InfraredFrameReader()
		{
			this.Dispose(false);
		}

		// Token: 0x06000579 RID: 1401
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameReader_ReleaseObject(ref IntPtr pNative);

		// Token: 0x0600057A RID: 1402
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameReader_AddRefObject(ref IntPtr pNative);

		// Token: 0x0600057B RID: 1403 RVA: 0x00020838 File Offset: 0x0001EA38
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<InfraredFrameReader>(this._pNative);
			if (disposing)
			{
				InfraredFrameReader.Windows_Kinect_InfraredFrameReader_Dispose(this._pNative);
			}
			InfraredFrameReader.Windows_Kinect_InfraredFrameReader_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x0600057C RID: 1404
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_InfraredFrameReader_get_InfraredFrameSource(IntPtr pNative);

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x00020890 File Offset: 0x0001EA90
		public InfraredFrameSource InfraredFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrameReader");
				}
				IntPtr intPtr = InfraredFrameReader.Windows_Kinect_InfraredFrameReader_get_InfraredFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<InfraredFrameSource>(intPtr, (IntPtr n) => new InfraredFrameSource(n));
			}
		}

		// Token: 0x0600057E RID: 1406
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_InfraredFrameReader_get_IsPaused(IntPtr pNative);

		// Token: 0x0600057F RID: 1407
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameReader_put_IsPaused(IntPtr pNative, bool isPaused);

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x000208FF File Offset: 0x0001EAFF
		// (set) Token: 0x06000581 RID: 1409 RVA: 0x00020929 File Offset: 0x0001EB29
		public bool IsPaused
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrameReader");
				}
				return InfraredFrameReader.Windows_Kinect_InfraredFrameReader_get_IsPaused(this._pNative);
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrameReader");
				}
				InfraredFrameReader.Windows_Kinect_InfraredFrameReader_put_IsPaused(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0002095C File Offset: 0x0001EB5C
		[MonoPInvokeCallback(typeof(InfraredFrameReader._Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate))]
		private static void Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<InfraredFrameArrivedEventArgs>> list = null;
			InfraredFrameReader.Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<InfraredFrameArrivedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				InfraredFrameReader objThis = NativeObjectCache.GetObject<InfraredFrameReader>(pNative);
				InfraredFrameArrivedEventArgs args = new InfraredFrameArrivedEventArgs(result);
				using (List<EventHandler<InfraredFrameArrivedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<InfraredFrameArrivedEventArgs> func = enumerator.Current;
						EventPump.Instance.Enqueue(delegate
						{
							try
							{
								func(objThis, args);
							}
							catch
							{
							}
						});
					}
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(obj);
				}
			}
		}

		// Token: 0x06000583 RID: 1411
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameReader_add_FrameArrived(IntPtr pNative, InfraredFrameReader._Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06000584 RID: 1412 RVA: 0x00020A20 File Offset: 0x0001EC20
		// (remove) Token: 0x06000585 RID: 1413 RVA: 0x00020AB4 File Offset: 0x0001ECB4
		public event EventHandler<InfraredFrameArrivedEventArgs> FrameArrived
		{
			add
			{
				EventPump.EnsureInitialized();
				InfraredFrameReader.Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<InfraredFrameArrivedEventArgs>> list = InfraredFrameReader.Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<InfraredFrameArrivedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						InfraredFrameReader._Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate windows_Kinect_InfraredFrameArrivedEventArgs_Delegate = new InfraredFrameReader._Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate(InfraredFrameReader.Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_Handler);
						InfraredFrameReader._Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_InfraredFrameArrivedEventArgs_Delegate);
						InfraredFrameReader.Windows_Kinect_InfraredFrameReader_add_FrameArrived(this._pNative, windows_Kinect_InfraredFrameArrivedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				InfraredFrameReader.Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<InfraredFrameArrivedEventArgs>> list = InfraredFrameReader.Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<InfraredFrameArrivedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						InfraredFrameReader.Windows_Kinect_InfraredFrameReader_add_FrameArrived(this._pNative, new InfraredFrameReader._Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate(InfraredFrameReader.Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_Handler), true);
						InfraredFrameReader._Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00020B50 File Offset: 0x0001ED50
		[MonoPInvokeCallback(typeof(InfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			InfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<PropertyChangedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				InfraredFrameReader objThis = NativeObjectCache.GetObject<InfraredFrameReader>(pNative);
				PropertyChangedEventArgs args = new PropertyChangedEventArgs(result);
				using (List<EventHandler<PropertyChangedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<PropertyChangedEventArgs> func = enumerator.Current;
						EventPump.Instance.Enqueue(delegate
						{
							try
							{
								func(objThis, args);
							}
							catch
							{
							}
						});
					}
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(obj);
				}
			}
		}

		// Token: 0x06000587 RID: 1415
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameReader_add_PropertyChanged(IntPtr pNative, InfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06000588 RID: 1416 RVA: 0x00020C14 File Offset: 0x0001EE14
		// (remove) Token: 0x06000589 RID: 1417 RVA: 0x00020CA8 File Offset: 0x0001EEA8
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				InfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = InfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						InfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new InfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(InfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						InfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						InfraredFrameReader.Windows_Kinect_InfraredFrameReader_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				InfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = InfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						InfraredFrameReader.Windows_Kinect_InfraredFrameReader_add_PropertyChanged(this._pNative, new InfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(InfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
						InfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x0600058A RID: 1418
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_InfraredFrameReader_AcquireLatestFrame(IntPtr pNative);

		// Token: 0x0600058B RID: 1419 RVA: 0x00020D44 File Offset: 0x0001EF44
		public InfraredFrame AcquireLatestFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("InfraredFrameReader");
			}
			IntPtr intPtr = InfraredFrameReader.Windows_Kinect_InfraredFrameReader_AcquireLatestFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<InfraredFrame>(intPtr, (IntPtr n) => new InfraredFrame(n));
		}

		// Token: 0x0600058C RID: 1420
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameReader_Dispose(IntPtr pNative);

		// Token: 0x0600058D RID: 1421 RVA: 0x00020DB3 File Offset: 0x0001EFB3
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00020DD8 File Offset: 0x0001EFD8
		private void __EventCleanup()
		{
			InfraredFrameReader.Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<InfraredFrameArrivedEventArgs>> list = InfraredFrameReader.Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<InfraredFrameArrivedEventArgs>> obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						InfraredFrameReader.Windows_Kinect_InfraredFrameReader_add_FrameArrived(this._pNative, new InfraredFrameReader._Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate(InfraredFrameReader.Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_Handler), true);
					}
					InfraredFrameReader._Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_Handle.Free();
				}
			}
			InfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = InfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<PropertyChangedEventArgs>> obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						InfraredFrameReader.Windows_Kinect_InfraredFrameReader_add_PropertyChanged(this._pNative, new InfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(InfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
					}
					InfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x04000292 RID: 658
		internal IntPtr _pNative;

		// Token: 0x04000293 RID: 659
		private static GCHandle _Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_Handle;

		// Token: 0x04000294 RID: 660
		private static CollectionMap<IntPtr, List<EventHandler<InfraredFrameArrivedEventArgs>>> Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<InfraredFrameArrivedEventArgs>>>();

		// Token: 0x04000295 RID: 661
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x04000296 RID: 662
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x0200028F RID: 655
		// (Invoke) Token: 0x06001216 RID: 4630
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_InfraredFrameArrivedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x02000290 RID: 656
		// (Invoke) Token: 0x0600121A RID: 4634
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
