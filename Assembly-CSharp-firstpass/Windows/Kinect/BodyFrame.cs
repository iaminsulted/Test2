using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200003E RID: 62
	public sealed class BodyFrame : IDisposable, INativeWrapper
	{
		// Token: 0x0600025E RID: 606
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrame_GetAndRefreshBodyData(IntPtr pNative, [Out] IntPtr[] bodies, int bodiesSize);

		// Token: 0x0600025F RID: 607 RVA: 0x000177C8 File Offset: 0x000159C8
		public void GetAndRefreshBodyData(IList<Body> bodies)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyFrame");
			}
			int num = 0;
			IntPtr[] array = new IntPtr[bodies.Count];
			for (int i = 0; i < bodies.Count; i++)
			{
				if (bodies[i] == null)
				{
					bodies[i] = new Body();
				}
				array[num] = bodies[i].GetIntPtr();
				num++;
			}
			BodyFrame.Windows_Kinect_BodyFrame_GetAndRefreshBodyData(this._pNative, array, bodies.Count);
			ExceptionHelper.CheckLastError();
			for (int j = 0; j < bodies.Count; j++)
			{
				bodies[j].SetIntPtr(array[j]);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000260 RID: 608 RVA: 0x00017870 File Offset: 0x00015A70
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00017878 File Offset: 0x00015A78
		internal BodyFrame(IntPtr pNative)
		{
			this._pNative = pNative;
			BodyFrame.Windows_Kinect_BodyFrame_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00017894 File Offset: 0x00015A94
		~BodyFrame()
		{
			this.Dispose(false);
		}

		// Token: 0x06000263 RID: 611
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrame_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06000264 RID: 612
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrame_AddRefObject(ref IntPtr pNative);

		// Token: 0x06000265 RID: 613 RVA: 0x000178C4 File Offset: 0x00015AC4
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<BodyFrame>(this._pNative);
			if (disposing)
			{
				BodyFrame.Windows_Kinect_BodyFrame_Dispose(this._pNative);
			}
			BodyFrame.Windows_Kinect_BodyFrame_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06000266 RID: 614
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_BodyFrame_get_BodyCount(IntPtr pNative);

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000267 RID: 615 RVA: 0x00017919 File Offset: 0x00015B19
		public int BodyCount
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrame");
				}
				return BodyFrame.Windows_Kinect_BodyFrame_get_BodyCount(this._pNative);
			}
		}

		// Token: 0x06000268 RID: 616
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyFrame_get_BodyFrameSource(IntPtr pNative);

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000269 RID: 617 RVA: 0x00017944 File Offset: 0x00015B44
		public BodyFrameSource BodyFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrame");
				}
				IntPtr intPtr = BodyFrame.Windows_Kinect_BodyFrame_get_BodyFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<BodyFrameSource>(intPtr, (IntPtr n) => new BodyFrameSource(n));
			}
		}

		// Token: 0x0600026A RID: 618
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyFrame_get_FloorClipPlane(IntPtr pNative);

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600026B RID: 619 RVA: 0x000179B4 File Offset: 0x00015BB4
		public Vector4 FloorClipPlane
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrame");
				}
				IntPtr intPtr = BodyFrame.Windows_Kinect_BodyFrame_get_FloorClipPlane(this._pNative);
				ExceptionHelper.CheckLastError();
				Vector4 result = (Vector4)Marshal.PtrToStructure(intPtr, typeof(Vector4));
				KinectUnityAddinUtils.FreeMemory(intPtr);
				return result;
			}
		}

		// Token: 0x0600026C RID: 620
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_BodyFrame_get_RelativeTime(IntPtr pNative);

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00017A0A File Offset: 0x00015C0A
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrame");
				}
				return TimeSpan.FromMilliseconds((double)BodyFrame.Windows_Kinect_BodyFrame_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x0600026E RID: 622
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrame_Dispose(IntPtr pNative);

		// Token: 0x0600026F RID: 623 RVA: 0x00017A3A File Offset: 0x00015C3A
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00017A5C File Offset: 0x00015C5C
		private void __EventCleanup()
		{
		}

		// Token: 0x04000202 RID: 514
		internal IntPtr _pNative;
	}
}
