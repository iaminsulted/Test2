using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x0200005C RID: 92
	public sealed class ColorFrameReader : IDisposable, INativeWrapper
	{
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x0001E135 File Offset: 0x0001C335
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0001E13D File Offset: 0x0001C33D
		internal ColorFrameReader(IntPtr pNative)
		{
			this._pNative = pNative;
			ColorFrameReader.Windows_Kinect_ColorFrameReader_AddRefObject(ref this._pNative);
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0001E158 File Offset: 0x0001C358
		~ColorFrameReader()
		{
			this.Dispose(false);
		}

		// Token: 0x060004AC RID: 1196
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameReader_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060004AD RID: 1197
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameReader_AddRefObject(ref IntPtr pNative);

		// Token: 0x060004AE RID: 1198 RVA: 0x0001E188 File Offset: 0x0001C388
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<ColorFrameReader>(this._pNative);
			if (disposing)
			{
				ColorFrameReader.Windows_Kinect_ColorFrameReader_Dispose(this._pNative);
			}
			ColorFrameReader.Windows_Kinect_ColorFrameReader_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060004AF RID: 1199
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrameReader_get_ColorFrameSource(IntPtr pNative);

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x0001E1E0 File Offset: 0x0001C3E0
		public ColorFrameSource ColorFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrameReader");
				}
				IntPtr intPtr = ColorFrameReader.Windows_Kinect_ColorFrameReader_get_ColorFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<ColorFrameSource>(intPtr, (IntPtr n) => new ColorFrameSource(n));
			}
		}

		// Token: 0x060004B1 RID: 1201
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_ColorFrameReader_get_IsPaused(IntPtr pNative);

		// Token: 0x060004B2 RID: 1202
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameReader_put_IsPaused(IntPtr pNative, bool isPaused);

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x0001E24F File Offset: 0x0001C44F
		// (set) Token: 0x060004B4 RID: 1204 RVA: 0x0001E279 File Offset: 0x0001C479
		public bool IsPaused
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrameReader");
				}
				return ColorFrameReader.Windows_Kinect_ColorFrameReader_get_IsPaused(this._pNative);
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrameReader");
				}
				ColorFrameReader.Windows_Kinect_ColorFrameReader_put_IsPaused(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0001E2AC File Offset: 0x0001C4AC
		[MonoPInvokeCallback(typeof(ColorFrameReader._Windows_Kinect_ColorFrameArrivedEventArgs_Delegate))]
		private static void Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<ColorFrameArrivedEventArgs>> list = null;
			ColorFrameReader.Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<ColorFrameArrivedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				ColorFrameReader objThis = NativeObjectCache.GetObject<ColorFrameReader>(pNative);
				ColorFrameArrivedEventArgs args = new ColorFrameArrivedEventArgs(result);
				using (List<EventHandler<ColorFrameArrivedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<ColorFrameArrivedEventArgs> func = enumerator.Current;
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

		// Token: 0x060004B6 RID: 1206
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameReader_add_FrameArrived(IntPtr pNative, ColorFrameReader._Windows_Kinect_ColorFrameArrivedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060004B7 RID: 1207 RVA: 0x0001E370 File Offset: 0x0001C570
		// (remove) Token: 0x060004B8 RID: 1208 RVA: 0x0001E404 File Offset: 0x0001C604
		public event EventHandler<ColorFrameArrivedEventArgs> FrameArrived
		{
			add
			{
				EventPump.EnsureInitialized();
				ColorFrameReader.Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<ColorFrameArrivedEventArgs>> list = ColorFrameReader.Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<ColorFrameArrivedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						ColorFrameReader._Windows_Kinect_ColorFrameArrivedEventArgs_Delegate windows_Kinect_ColorFrameArrivedEventArgs_Delegate = new ColorFrameReader._Windows_Kinect_ColorFrameArrivedEventArgs_Delegate(ColorFrameReader.Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_Handler);
						ColorFrameReader._Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_ColorFrameArrivedEventArgs_Delegate);
						ColorFrameReader.Windows_Kinect_ColorFrameReader_add_FrameArrived(this._pNative, windows_Kinect_ColorFrameArrivedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				ColorFrameReader.Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<ColorFrameArrivedEventArgs>> list = ColorFrameReader.Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<ColorFrameArrivedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						ColorFrameReader.Windows_Kinect_ColorFrameReader_add_FrameArrived(this._pNative, new ColorFrameReader._Windows_Kinect_ColorFrameArrivedEventArgs_Delegate(ColorFrameReader.Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_Handler), true);
						ColorFrameReader._Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0001E4A0 File Offset: 0x0001C6A0
		[MonoPInvokeCallback(typeof(ColorFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			ColorFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<PropertyChangedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				ColorFrameReader objThis = NativeObjectCache.GetObject<ColorFrameReader>(pNative);
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

		// Token: 0x060004BA RID: 1210
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameReader_add_PropertyChanged(IntPtr pNative, ColorFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060004BB RID: 1211 RVA: 0x0001E564 File Offset: 0x0001C764
		// (remove) Token: 0x060004BC RID: 1212 RVA: 0x0001E5F8 File Offset: 0x0001C7F8
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				ColorFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = ColorFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						ColorFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new ColorFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(ColorFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						ColorFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						ColorFrameReader.Windows_Kinect_ColorFrameReader_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				ColorFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = ColorFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						ColorFrameReader.Windows_Kinect_ColorFrameReader_add_PropertyChanged(this._pNative, new ColorFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(ColorFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
						ColorFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x060004BD RID: 1213
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrameReader_AcquireLatestFrame(IntPtr pNative);

		// Token: 0x060004BE RID: 1214 RVA: 0x0001E694 File Offset: 0x0001C894
		public ColorFrame AcquireLatestFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ColorFrameReader");
			}
			IntPtr intPtr = ColorFrameReader.Windows_Kinect_ColorFrameReader_AcquireLatestFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<ColorFrame>(intPtr, (IntPtr n) => new ColorFrame(n));
		}

		// Token: 0x060004BF RID: 1215
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameReader_Dispose(IntPtr pNative);

		// Token: 0x060004C0 RID: 1216 RVA: 0x0001E703 File Offset: 0x0001C903
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0001E728 File Offset: 0x0001C928
		private void __EventCleanup()
		{
			ColorFrameReader.Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<ColorFrameArrivedEventArgs>> list = ColorFrameReader.Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<ColorFrameArrivedEventArgs>> obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						ColorFrameReader.Windows_Kinect_ColorFrameReader_add_FrameArrived(this._pNative, new ColorFrameReader._Windows_Kinect_ColorFrameArrivedEventArgs_Delegate(ColorFrameReader.Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_Handler), true);
					}
					ColorFrameReader._Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_Handle.Free();
				}
			}
			ColorFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = ColorFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<PropertyChangedEventArgs>> obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						ColorFrameReader.Windows_Kinect_ColorFrameReader_add_PropertyChanged(this._pNative, new ColorFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(ColorFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
					}
					ColorFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x0400024B RID: 587
		internal IntPtr _pNative;

		// Token: 0x0400024C RID: 588
		private static GCHandle _Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_Handle;

		// Token: 0x0400024D RID: 589
		private static CollectionMap<IntPtr, List<EventHandler<ColorFrameArrivedEventArgs>>> Windows_Kinect_ColorFrameArrivedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<ColorFrameArrivedEventArgs>>>();

		// Token: 0x0400024E RID: 590
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x0400024F RID: 591
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x0200026F RID: 623
		// (Invoke) Token: 0x060011BF RID: 4543
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_ColorFrameArrivedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x02000270 RID: 624
		// (Invoke) Token: 0x060011C3 RID: 4547
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
