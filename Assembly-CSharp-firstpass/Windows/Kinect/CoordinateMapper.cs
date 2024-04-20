using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x02000046 RID: 70
	public sealed class CoordinateMapper : INativeWrapper
	{
		// Token: 0x06000349 RID: 841
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_CoordinateMapper_GetDepthCameraIntrinsics(IntPtr pNative);

		// Token: 0x0600034A RID: 842 RVA: 0x00019ACC File Offset: 0x00017CCC
		public CameraIntrinsics GetDepthCameraIntrinsics()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			IntPtr intPtr = CoordinateMapper.Windows_Kinect_CoordinateMapper_GetDepthCameraIntrinsics(this._pNative);
			ExceptionHelper.CheckLastError();
			CameraIntrinsics result = (CameraIntrinsics)Marshal.PtrToStructure(intPtr, typeof(CameraIntrinsics));
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x0600034B RID: 843
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_CoordinateMapper_GetDepthFrameToCameraSpaceTable(IntPtr pNative, IntPtr outCollection, uint outCollectionSize);

		// Token: 0x0600034C RID: 844 RVA: 0x00019B24 File Offset: 0x00017D24
		public PointF[] GetDepthFrameToCameraSpaceTable()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			if (this._DepthFrameToCameraSpaceTable == null)
			{
				FrameDescription frameDescription = KinectSensor.GetDefault().DepthFrameSource.FrameDescription;
				this._DepthFrameToCameraSpaceTable = new PointF[frameDescription.Width * frameDescription.Height];
				IntPtr outCollection = new SmartGCHandle(GCHandle.Alloc(this._DepthFrameToCameraSpaceTable, GCHandleType.Pinned)).AddrOfPinnedObject();
				CoordinateMapper.Windows_Kinect_CoordinateMapper_GetDepthFrameToCameraSpaceTable(this._pNative, outCollection, (uint)this._DepthFrameToCameraSpaceTable.Length);
				ExceptionHelper.CheckLastError();
			}
			return this._DepthFrameToCameraSpaceTable;
		}

		// Token: 0x0600034D RID: 845
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapColorFrameToDepthSpace(IntPtr pNative, IntPtr depthFrameData, uint depthFrameDataSize, IntPtr depthSpacePoints, uint depthSpacePointsSize);

		// Token: 0x0600034E RID: 846 RVA: 0x00019BB8 File Offset: 0x00017DB8
		public void MapColorFrameToDepthSpaceUsingIntPtr(IntPtr depthFrameData, uint depthFrameSize, IntPtr depthSpacePoints, uint depthSpacePointsSize)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			uint depthFrameDataSize = depthFrameSize / 2U;
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapColorFrameToDepthSpace(this._pNative, depthFrameData, depthFrameDataSize, depthSpacePoints, depthSpacePointsSize);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x0600034F RID: 847
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapColorFrameToCameraSpace(IntPtr pNative, IntPtr depthFrameData, uint depthFrameDataSize, IntPtr cameraSpacePoints, uint cameraSpacePointsSize);

		// Token: 0x06000350 RID: 848 RVA: 0x00019BFC File Offset: 0x00017DFC
		public void MapColorFrameToCameraSpaceUsingIntPtr(IntPtr depthFrameData, int depthFrameSize, IntPtr cameraSpacePoints, uint cameraSpacePointsSize)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			uint depthFrameDataSize = (uint)(depthFrameSize / 2);
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapColorFrameToCameraSpace(this._pNative, depthFrameData, depthFrameDataSize, cameraSpacePoints, cameraSpacePointsSize);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06000351 RID: 849
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapDepthFrameToColorSpace(IntPtr pNative, IntPtr depthFrameData, uint depthFrameDataSize, IntPtr colorSpacePoints, uint colorSpacePointsSize);

		// Token: 0x06000352 RID: 850 RVA: 0x00019C40 File Offset: 0x00017E40
		public void MapDepthFrameToColorSpaceUsingIntPtr(IntPtr depthFrameData, int depthFrameSize, IntPtr colorSpacePoints, uint colorSpacePointsSize)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			uint depthFrameDataSize = (uint)(depthFrameSize / 2);
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapDepthFrameToColorSpace(this._pNative, depthFrameData, depthFrameDataSize, colorSpacePoints, colorSpacePointsSize);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06000353 RID: 851
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapDepthFrameToCameraSpace(IntPtr pNative, IntPtr depthFrameData, uint depthFrameDataSize, IntPtr cameraSpacePoints, uint cameraSpacePointsSize);

		// Token: 0x06000354 RID: 852 RVA: 0x00019C84 File Offset: 0x00017E84
		public void MapDepthFrameToCameraSpaceUsingIntPtr(IntPtr depthFrameData, int depthFrameSize, IntPtr cameraSpacePoints, uint cameraSpacePointsSize)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			uint depthFrameDataSize = (uint)(depthFrameSize / 2);
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapDepthFrameToCameraSpace(this._pNative, depthFrameData, depthFrameDataSize, cameraSpacePoints, cameraSpacePointsSize);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000355 RID: 853 RVA: 0x00019CC7 File Offset: 0x00017EC7
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00019CCF File Offset: 0x00017ECF
		internal CoordinateMapper(IntPtr pNative)
		{
			this._pNative = pNative;
			CoordinateMapper.Windows_Kinect_CoordinateMapper_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00019CEC File Offset: 0x00017EEC
		~CoordinateMapper()
		{
			this.Dispose(false);
		}

		// Token: 0x06000358 RID: 856
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06000359 RID: 857
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_AddRefObject(ref IntPtr pNative);

		// Token: 0x0600035A RID: 858 RVA: 0x00019D1C File Offset: 0x00017F1C
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<CoordinateMapper>(this._pNative);
			CoordinateMapper.Windows_Kinect_CoordinateMapper_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00019D58 File Offset: 0x00017F58
		[MonoPInvokeCallback(typeof(CoordinateMapper._Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate))]
		private static void Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<CoordinateMappingChangedEventArgs>> list = null;
			CoordinateMapper.Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<CoordinateMappingChangedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				CoordinateMapper objThis = NativeObjectCache.GetObject<CoordinateMapper>(pNative);
				CoordinateMappingChangedEventArgs args = new CoordinateMappingChangedEventArgs(result);
				using (List<EventHandler<CoordinateMappingChangedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<CoordinateMappingChangedEventArgs> func = enumerator.Current;
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

		// Token: 0x0600035C RID: 860
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_add_CoordinateMappingChanged(IntPtr pNative, CoordinateMapper._Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600035D RID: 861 RVA: 0x00019E1C File Offset: 0x0001801C
		// (remove) Token: 0x0600035E RID: 862 RVA: 0x00019EB0 File Offset: 0x000180B0
		public event EventHandler<CoordinateMappingChangedEventArgs> CoordinateMappingChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				CoordinateMapper.Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<CoordinateMappingChangedEventArgs>> list = CoordinateMapper.Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<CoordinateMappingChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						CoordinateMapper._Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate windows_Kinect_CoordinateMappingChangedEventArgs_Delegate = new CoordinateMapper._Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate(CoordinateMapper.Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_Handler);
						CoordinateMapper._Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_CoordinateMappingChangedEventArgs_Delegate);
						CoordinateMapper.Windows_Kinect_CoordinateMapper_add_CoordinateMappingChanged(this._pNative, windows_Kinect_CoordinateMappingChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				CoordinateMapper.Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<CoordinateMappingChangedEventArgs>> list = CoordinateMapper.Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<CoordinateMappingChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						CoordinateMapper.Windows_Kinect_CoordinateMapper_add_CoordinateMappingChanged(this._pNative, new CoordinateMapper._Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate(CoordinateMapper.Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_Handler), true);
						CoordinateMapper._Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x0600035F RID: 863
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_CoordinateMapper_MapCameraPointToDepthSpace(IntPtr pNative, CameraSpacePoint cameraPoint);

		// Token: 0x06000360 RID: 864 RVA: 0x00019F4C File Offset: 0x0001814C
		public DepthSpacePoint MapCameraPointToDepthSpace(CameraSpacePoint cameraPoint)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			IntPtr intPtr = CoordinateMapper.Windows_Kinect_CoordinateMapper_MapCameraPointToDepthSpace(this._pNative, cameraPoint);
			ExceptionHelper.CheckLastError();
			DepthSpacePoint result = (DepthSpacePoint)Marshal.PtrToStructure(intPtr, typeof(DepthSpacePoint));
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x06000361 RID: 865
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_CoordinateMapper_MapCameraPointToColorSpace(IntPtr pNative, CameraSpacePoint cameraPoint);

		// Token: 0x06000362 RID: 866 RVA: 0x00019FA4 File Offset: 0x000181A4
		public ColorSpacePoint MapCameraPointToColorSpace(CameraSpacePoint cameraPoint)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			IntPtr intPtr = CoordinateMapper.Windows_Kinect_CoordinateMapper_MapCameraPointToColorSpace(this._pNative, cameraPoint);
			ExceptionHelper.CheckLastError();
			ColorSpacePoint result = (ColorSpacePoint)Marshal.PtrToStructure(intPtr, typeof(ColorSpacePoint));
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x06000363 RID: 867
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_CoordinateMapper_MapDepthPointToCameraSpace(IntPtr pNative, DepthSpacePoint depthPoint, ushort depth);

		// Token: 0x06000364 RID: 868 RVA: 0x00019FFC File Offset: 0x000181FC
		public CameraSpacePoint MapDepthPointToCameraSpace(DepthSpacePoint depthPoint, ushort depth)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			IntPtr intPtr = CoordinateMapper.Windows_Kinect_CoordinateMapper_MapDepthPointToCameraSpace(this._pNative, depthPoint, depth);
			ExceptionHelper.CheckLastError();
			CameraSpacePoint result = (CameraSpacePoint)Marshal.PtrToStructure(intPtr, typeof(CameraSpacePoint));
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x06000365 RID: 869
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_CoordinateMapper_MapDepthPointToColorSpace(IntPtr pNative, DepthSpacePoint depthPoint, ushort depth);

		// Token: 0x06000366 RID: 870 RVA: 0x0001A054 File Offset: 0x00018254
		public ColorSpacePoint MapDepthPointToColorSpace(DepthSpacePoint depthPoint, ushort depth)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			IntPtr intPtr = CoordinateMapper.Windows_Kinect_CoordinateMapper_MapDepthPointToColorSpace(this._pNative, depthPoint, depth);
			ExceptionHelper.CheckLastError();
			ColorSpacePoint result = (ColorSpacePoint)Marshal.PtrToStructure(intPtr, typeof(ColorSpacePoint));
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x06000367 RID: 871
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapCameraPointsToDepthSpace(IntPtr pNative, IntPtr cameraPoints, int cameraPointsSize, IntPtr depthPoints, int depthPointsSize);

		// Token: 0x06000368 RID: 872 RVA: 0x0001A0AC File Offset: 0x000182AC
		public void MapCameraPointsToDepthSpace(CameraSpacePoint[] cameraPoints, DepthSpacePoint[] depthPoints)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			IntPtr cameraPoints2 = new SmartGCHandle(GCHandle.Alloc(cameraPoints, GCHandleType.Pinned)).AddrOfPinnedObject();
			IntPtr depthPoints2 = new SmartGCHandle(GCHandle.Alloc(depthPoints, GCHandleType.Pinned)).AddrOfPinnedObject();
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapCameraPointsToDepthSpace(this._pNative, cameraPoints2, cameraPoints.Length, depthPoints2, depthPoints.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06000369 RID: 873
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapCameraPointsToColorSpace(IntPtr pNative, IntPtr cameraPoints, int cameraPointsSize, IntPtr colorPoints, int colorPointsSize);

		// Token: 0x0600036A RID: 874 RVA: 0x0001A114 File Offset: 0x00018314
		public void MapCameraPointsToColorSpace(CameraSpacePoint[] cameraPoints, ColorSpacePoint[] colorPoints)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			IntPtr cameraPoints2 = new SmartGCHandle(GCHandle.Alloc(cameraPoints, GCHandleType.Pinned)).AddrOfPinnedObject();
			IntPtr colorPoints2 = new SmartGCHandle(GCHandle.Alloc(colorPoints, GCHandleType.Pinned)).AddrOfPinnedObject();
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapCameraPointsToColorSpace(this._pNative, cameraPoints2, cameraPoints.Length, colorPoints2, colorPoints.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x0600036B RID: 875
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapDepthPointsToCameraSpace(IntPtr pNative, IntPtr depthPoints, int depthPointsSize, IntPtr depths, int depthsSize, IntPtr cameraPoints, int cameraPointsSize);

		// Token: 0x0600036C RID: 876 RVA: 0x0001A17C File Offset: 0x0001837C
		public void MapDepthPointsToCameraSpace(DepthSpacePoint[] depthPoints, ushort[] depths, CameraSpacePoint[] cameraPoints)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			IntPtr depthPoints2 = new SmartGCHandle(GCHandle.Alloc(depthPoints, GCHandleType.Pinned)).AddrOfPinnedObject();
			IntPtr depths2 = new SmartGCHandle(GCHandle.Alloc(depths, GCHandleType.Pinned)).AddrOfPinnedObject();
			IntPtr cameraPoints2 = new SmartGCHandle(GCHandle.Alloc(cameraPoints, GCHandleType.Pinned)).AddrOfPinnedObject();
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapDepthPointsToCameraSpace(this._pNative, depthPoints2, depthPoints.Length, depths2, depths.Length, cameraPoints2, cameraPoints.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x0600036D RID: 877
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapDepthPointsToColorSpace(IntPtr pNative, IntPtr depthPoints, int depthPointsSize, IntPtr depths, int depthsSize, IntPtr colorPoints, int colorPointsSize);

		// Token: 0x0600036E RID: 878 RVA: 0x0001A1F8 File Offset: 0x000183F8
		public void MapDepthPointsToColorSpace(DepthSpacePoint[] depthPoints, ushort[] depths, ColorSpacePoint[] colorPoints)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			IntPtr depthPoints2 = new SmartGCHandle(GCHandle.Alloc(depthPoints, GCHandleType.Pinned)).AddrOfPinnedObject();
			IntPtr depths2 = new SmartGCHandle(GCHandle.Alloc(depths, GCHandleType.Pinned)).AddrOfPinnedObject();
			IntPtr colorPoints2 = new SmartGCHandle(GCHandle.Alloc(colorPoints, GCHandleType.Pinned)).AddrOfPinnedObject();
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapDepthPointsToColorSpace(this._pNative, depthPoints2, depthPoints.Length, depths2, depths.Length, colorPoints2, colorPoints.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x0600036F RID: 879
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapDepthFrameToCameraSpace(IntPtr pNative, IntPtr depthFrameData, int depthFrameDataSize, IntPtr cameraSpacePoints, int cameraSpacePointsSize);

		// Token: 0x06000370 RID: 880 RVA: 0x0001A274 File Offset: 0x00018474
		public void MapDepthFrameToCameraSpace(ushort[] depthFrameData, CameraSpacePoint[] cameraSpacePoints)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			IntPtr depthFrameData2 = new SmartGCHandle(GCHandle.Alloc(depthFrameData, GCHandleType.Pinned)).AddrOfPinnedObject();
			IntPtr cameraSpacePoints2 = new SmartGCHandle(GCHandle.Alloc(cameraSpacePoints, GCHandleType.Pinned)).AddrOfPinnedObject();
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapDepthFrameToCameraSpace(this._pNative, depthFrameData2, depthFrameData.Length, cameraSpacePoints2, cameraSpacePoints.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06000371 RID: 881
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapDepthFrameToColorSpace(IntPtr pNative, IntPtr depthFrameData, int depthFrameDataSize, IntPtr colorSpacePoints, int colorSpacePointsSize);

		// Token: 0x06000372 RID: 882 RVA: 0x0001A2DC File Offset: 0x000184DC
		public void MapDepthFrameToColorSpace(ushort[] depthFrameData, ColorSpacePoint[] colorSpacePoints)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			IntPtr depthFrameData2 = new SmartGCHandle(GCHandle.Alloc(depthFrameData, GCHandleType.Pinned)).AddrOfPinnedObject();
			IntPtr colorSpacePoints2 = new SmartGCHandle(GCHandle.Alloc(colorSpacePoints, GCHandleType.Pinned)).AddrOfPinnedObject();
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapDepthFrameToColorSpace(this._pNative, depthFrameData2, depthFrameData.Length, colorSpacePoints2, colorSpacePoints.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06000373 RID: 883
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapColorFrameToDepthSpace(IntPtr pNative, IntPtr depthFrameData, int depthFrameDataSize, IntPtr depthSpacePoints, int depthSpacePointsSize);

		// Token: 0x06000374 RID: 884 RVA: 0x0001A344 File Offset: 0x00018544
		public void MapColorFrameToDepthSpace(ushort[] depthFrameData, DepthSpacePoint[] depthSpacePoints)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			IntPtr depthFrameData2 = new SmartGCHandle(GCHandle.Alloc(depthFrameData, GCHandleType.Pinned)).AddrOfPinnedObject();
			IntPtr depthSpacePoints2 = new SmartGCHandle(GCHandle.Alloc(depthSpacePoints, GCHandleType.Pinned)).AddrOfPinnedObject();
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapColorFrameToDepthSpace(this._pNative, depthFrameData2, depthFrameData.Length, depthSpacePoints2, depthSpacePoints.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06000375 RID: 885
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMapper_MapColorFrameToCameraSpace(IntPtr pNative, IntPtr depthFrameData, int depthFrameDataSize, IntPtr cameraSpacePoints, int cameraSpacePointsSize);

		// Token: 0x06000376 RID: 886 RVA: 0x0001A3AC File Offset: 0x000185AC
		public void MapColorFrameToCameraSpace(ushort[] depthFrameData, CameraSpacePoint[] cameraSpacePoints)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("CoordinateMapper");
			}
			IntPtr depthFrameData2 = new SmartGCHandle(GCHandle.Alloc(depthFrameData, GCHandleType.Pinned)).AddrOfPinnedObject();
			IntPtr cameraSpacePoints2 = new SmartGCHandle(GCHandle.Alloc(cameraSpacePoints, GCHandleType.Pinned)).AddrOfPinnedObject();
			CoordinateMapper.Windows_Kinect_CoordinateMapper_MapColorFrameToCameraSpace(this._pNative, depthFrameData2, depthFrameData.Length, cameraSpacePoints2, cameraSpacePoints.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0001A414 File Offset: 0x00018614
		private void __EventCleanup()
		{
			CoordinateMapper.Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<CoordinateMappingChangedEventArgs>> list = CoordinateMapper.Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<CoordinateMappingChangedEventArgs>> obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						CoordinateMapper.Windows_Kinect_CoordinateMapper_add_CoordinateMappingChanged(this._pNative, new CoordinateMapper._Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate(CoordinateMapper.Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_Handler), true);
					}
					CoordinateMapper._Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x0400020E RID: 526
		private PointF[] _DepthFrameToCameraSpaceTable;

		// Token: 0x0400020F RID: 527
		internal IntPtr _pNative;

		// Token: 0x04000210 RID: 528
		private static GCHandle _Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_Handle;

		// Token: 0x04000211 RID: 529
		private static CollectionMap<IntPtr, List<EventHandler<CoordinateMappingChangedEventArgs>>> Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<CoordinateMappingChangedEventArgs>>>();

		// Token: 0x02000237 RID: 567
		// (Invoke) Token: 0x0600112B RID: 4395
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_CoordinateMappingChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
