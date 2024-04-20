using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x0200005E RID: 94
	public sealed class ColorFrameSource : INativeWrapper
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060004CE RID: 1230 RVA: 0x0001E995 File Offset: 0x0001CB95
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0001E99D File Offset: 0x0001CB9D
		internal ColorFrameSource(IntPtr pNative)
		{
			this._pNative = pNative;
			ColorFrameSource.Windows_Kinect_ColorFrameSource_AddRefObject(ref this._pNative);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001E9B8 File Offset: 0x0001CBB8
		~ColorFrameSource()
		{
			this.Dispose(false);
		}

		// Token: 0x060004D1 RID: 1233
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameSource_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060004D2 RID: 1234
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameSource_AddRefObject(ref IntPtr pNative);

		// Token: 0x060004D3 RID: 1235 RVA: 0x0001E9E8 File Offset: 0x0001CBE8
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<ColorFrameSource>(this._pNative);
			ColorFrameSource.Windows_Kinect_ColorFrameSource_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060004D4 RID: 1236
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrameSource_get_FrameDescription(IntPtr pNative);

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0001EA24 File Offset: 0x0001CC24
		public FrameDescription FrameDescription
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrameSource");
				}
				IntPtr intPtr = ColorFrameSource.Windows_Kinect_ColorFrameSource_get_FrameDescription(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
			}
		}

		// Token: 0x060004D6 RID: 1238
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_ColorFrameSource_get_IsActive(IntPtr pNative);

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0001EA93 File Offset: 0x0001CC93
		public bool IsActive
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrameSource");
				}
				return ColorFrameSource.Windows_Kinect_ColorFrameSource_get_IsActive(this._pNative);
			}
		}

		// Token: 0x060004D8 RID: 1240
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrameSource_get_KinectSensor(IntPtr pNative);

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x0001EAC0 File Offset: 0x0001CCC0
		public KinectSensor KinectSensor
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrameSource");
				}
				IntPtr intPtr = ColorFrameSource.Windows_Kinect_ColorFrameSource_get_KinectSensor(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<KinectSensor>(intPtr, (IntPtr n) => new KinectSensor(n));
			}
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0001EB30 File Offset: 0x0001CD30
		[MonoPInvokeCallback(typeof(ColorFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate))]
		private static void Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<FrameCapturedEventArgs>> list = null;
			ColorFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<FrameCapturedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				ColorFrameSource objThis = NativeObjectCache.GetObject<ColorFrameSource>(pNative);
				FrameCapturedEventArgs args = new FrameCapturedEventArgs(result);
				using (List<EventHandler<FrameCapturedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<FrameCapturedEventArgs> func = enumerator.Current;
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

		// Token: 0x060004DB RID: 1243
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameSource_add_FrameCaptured(IntPtr pNative, ColorFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x060004DC RID: 1244 RVA: 0x0001EBF4 File Offset: 0x0001CDF4
		// (remove) Token: 0x060004DD RID: 1245 RVA: 0x0001EC88 File Offset: 0x0001CE88
		public event EventHandler<FrameCapturedEventArgs> FrameCaptured
		{
			add
			{
				EventPump.EnsureInitialized();
				ColorFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = ColorFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<FrameCapturedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						ColorFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate windows_Kinect_FrameCapturedEventArgs_Delegate = new ColorFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(ColorFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						ColorFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_FrameCapturedEventArgs_Delegate);
						ColorFrameSource.Windows_Kinect_ColorFrameSource_add_FrameCaptured(this._pNative, windows_Kinect_FrameCapturedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				ColorFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = ColorFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<FrameCapturedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						ColorFrameSource.Windows_Kinect_ColorFrameSource_add_FrameCaptured(this._pNative, new ColorFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(ColorFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler), true);
						ColorFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0001ED24 File Offset: 0x0001CF24
		[MonoPInvokeCallback(typeof(ColorFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			ColorFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<PropertyChangedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				ColorFrameSource objThis = NativeObjectCache.GetObject<ColorFrameSource>(pNative);
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

		// Token: 0x060004DF RID: 1247
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameSource_add_PropertyChanged(IntPtr pNative, ColorFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x060004E0 RID: 1248 RVA: 0x0001EDE8 File Offset: 0x0001CFE8
		// (remove) Token: 0x060004E1 RID: 1249 RVA: 0x0001EE7C File Offset: 0x0001D07C
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				ColorFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = ColorFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						ColorFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new ColorFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(ColorFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						ColorFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						ColorFrameSource.Windows_Kinect_ColorFrameSource_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				ColorFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = ColorFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						ColorFrameSource.Windows_Kinect_ColorFrameSource_add_PropertyChanged(this._pNative, new ColorFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(ColorFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
						ColorFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x060004E2 RID: 1250
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrameSource_OpenReader(IntPtr pNative);

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001EF18 File Offset: 0x0001D118
		public ColorFrameReader OpenReader()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ColorFrameSource");
			}
			IntPtr intPtr = ColorFrameSource.Windows_Kinect_ColorFrameSource_OpenReader(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<ColorFrameReader>(intPtr, (IntPtr n) => new ColorFrameReader(n));
		}

		// Token: 0x060004E4 RID: 1252
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrameSource_CreateFrameDescription(IntPtr pNative, ColorImageFormat format);

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001EF88 File Offset: 0x0001D188
		public FrameDescription CreateFrameDescription(ColorImageFormat format)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ColorFrameSource");
			}
			IntPtr intPtr = ColorFrameSource.Windows_Kinect_ColorFrameSource_CreateFrameDescription(this._pNative, format);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001EFF8 File Offset: 0x0001D1F8
		private void __EventCleanup()
		{
			ColorFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<FrameCapturedEventArgs>> list = ColorFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<FrameCapturedEventArgs>> obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						ColorFrameSource.Windows_Kinect_ColorFrameSource_add_FrameCaptured(this._pNative, new ColorFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(ColorFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler), true);
					}
					ColorFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
				}
			}
			ColorFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = ColorFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<PropertyChangedEventArgs>> obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						ColorFrameSource.Windows_Kinect_ColorFrameSource_add_PropertyChanged(this._pNative, new ColorFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(ColorFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
					}
					ColorFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x04000251 RID: 593
		internal IntPtr _pNative;

		// Token: 0x04000252 RID: 594
		private static GCHandle _Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle;

		// Token: 0x04000253 RID: 595
		private static CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>> Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>>();

		// Token: 0x04000254 RID: 596
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x04000255 RID: 597
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x02000277 RID: 631
		// (Invoke) Token: 0x060011D4 RID: 4564
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_FrameCapturedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x02000278 RID: 632
		// (Invoke) Token: 0x060011D8 RID: 4568
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
