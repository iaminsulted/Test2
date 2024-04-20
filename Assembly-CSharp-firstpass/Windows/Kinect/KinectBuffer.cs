using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200003A RID: 58
	public class KinectBuffer : INativeWrapper, IDisposable
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00016F34 File Offset: 0x00015134
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00016F3C File Offset: 0x0001513C
		internal KinectBuffer(IntPtr pNative)
		{
			this._pNative = pNative;
			KinectBuffer.Windows_Storage_Streams_IBuffer_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00016F58 File Offset: 0x00015158
		~KinectBuffer()
		{
			this.Dispose(false);
		}

		// Token: 0x06000216 RID: 534
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Windows_Storage_Streams_IBuffer_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06000217 RID: 535
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Windows_Storage_Streams_IBuffer_AddRefObject(ref IntPtr pNative);

		// Token: 0x06000218 RID: 536 RVA: 0x00016F88 File Offset: 0x00015188
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			NativeObjectCache.RemoveObject<KinectBuffer>(this._pNative);
			if (disposing)
			{
				KinectBuffer.Windows_Storage_Streams_IBuffer_Dispose(this._pNative);
			}
			KinectBuffer.Windows_Storage_Streams_IBuffer_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06000219 RID: 537
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern uint Windows_Storage_Streams_IBuffer_get_Capacity(IntPtr pNative);

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600021A RID: 538 RVA: 0x00016FD7 File Offset: 0x000151D7
		public uint Capacity
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectBuffer");
				}
				uint result = KinectBuffer.Windows_Storage_Streams_IBuffer_get_Capacity(this._pNative);
				ExceptionHelper.CheckLastError();
				return result;
			}
		}

		// Token: 0x0600021B RID: 539
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern uint Windows_Storage_Streams_IBuffer_get_Length(IntPtr pNative);

		// Token: 0x0600021C RID: 540
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Storage_Streams_IBuffer_put_Length(IntPtr pNative, uint value);

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00017006 File Offset: 0x00015206
		// (set) Token: 0x0600021E RID: 542 RVA: 0x00017035 File Offset: 0x00015235
		public uint Length
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectBuffer");
				}
				uint result = KinectBuffer.Windows_Storage_Streams_IBuffer_get_Length(this._pNative);
				ExceptionHelper.CheckLastError();
				return result;
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectBuffer");
				}
				KinectBuffer.Windows_Storage_Streams_IBuffer_put_Length(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x0600021F RID: 543
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Windows_Storage_Streams_IBuffer_Dispose(IntPtr pNative);

		// Token: 0x06000220 RID: 544 RVA: 0x00017065 File Offset: 0x00015265
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("KinectBuffer");
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000221 RID: 545
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Storage_Streams_IBuffer_get_UnderlyingBuffer(IntPtr pNative);

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00017091 File Offset: 0x00015291
		public IntPtr UnderlyingBuffer
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectBuffer");
				}
				IntPtr result = KinectBuffer.Windows_Storage_Streams_IBuffer_get_UnderlyingBuffer(this._pNative);
				ExceptionHelper.CheckLastError();
				return result;
			}
		}

		// Token: 0x040001FC RID: 508
		internal IntPtr _pNative;
	}
}
