using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200003F RID: 63
	public sealed class Body : INativeWrapper
	{
		// Token: 0x06000271 RID: 625 RVA: 0x00017A5E File Offset: 0x00015C5E
		internal void SetIntPtr(IntPtr value)
		{
			this._pNative = value;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00017A67 File Offset: 0x00015C67
		internal IntPtr GetIntPtr()
		{
			return this._pNative;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00017A6F File Offset: 0x00015C6F
		internal Body()
		{
		}

		// Token: 0x06000274 RID: 628
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_Body_get_Lean(IntPtr pNative);

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000275 RID: 629 RVA: 0x00017A78 File Offset: 0x00015C78
		public PointF Lean
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				IntPtr intPtr = Body.Windows_Kinect_Body_get_Lean(this._pNative);
				ExceptionHelper.CheckLastError();
				PointF result = (PointF)Marshal.PtrToStructure(intPtr, typeof(PointF));
				Marshal.FreeHGlobal(intPtr);
				return result;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000276 RID: 630 RVA: 0x00017ACE File Offset: 0x00015CCE
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00017AD6 File Offset: 0x00015CD6
		internal Body(IntPtr pNative)
		{
			this._pNative = pNative;
			Body.Windows_Kinect_Body_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00017AF0 File Offset: 0x00015CF0
		~Body()
		{
			this.Dispose(false);
		}

		// Token: 0x06000279 RID: 633
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_Body_ReleaseObject(ref IntPtr pNative);

		// Token: 0x0600027A RID: 634
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_Body_AddRefObject(ref IntPtr pNative);

		// Token: 0x0600027B RID: 635 RVA: 0x00017B20 File Offset: 0x00015D20
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<Body>(this._pNative);
			Body.Windows_Kinect_Body_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x0600027C RID: 636
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_Activities(IntPtr pNative, [Out] Activity[] outKeys, [Out] DetectionResult[] outValues, int outCollectionSize);

		// Token: 0x0600027D RID: 637
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_Activities_Length(IntPtr pNative);

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600027E RID: 638 RVA: 0x00017B5C File Offset: 0x00015D5C
		public Dictionary<Activity, DetectionResult> Activities
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				int num = Body.Windows_Kinect_Body_get_Activities_Length(this._pNative);
				Activity[] array = new Activity[num];
				DetectionResult[] array2 = new DetectionResult[num];
				Dictionary<Activity, DetectionResult> dictionary = new Dictionary<Activity, DetectionResult>();
				num = Body.Windows_Kinect_Body_get_Activities(this._pNative, array, array2, num);
				ExceptionHelper.CheckLastError();
				for (int i = 0; i < num; i++)
				{
					dictionary.Add(array[i], array2[i]);
				}
				return dictionary;
			}
		}

		// Token: 0x0600027F RID: 639
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_Appearance(IntPtr pNative, [Out] Appearance[] outKeys, [Out] DetectionResult[] outValues, int outCollectionSize);

		// Token: 0x06000280 RID: 640
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_Appearance_Length(IntPtr pNative);

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000281 RID: 641 RVA: 0x00017BDC File Offset: 0x00015DDC
		public Dictionary<Appearance, DetectionResult> Appearance
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				int num = Body.Windows_Kinect_Body_get_Appearance_Length(this._pNative);
				Appearance[] array = new Appearance[num];
				DetectionResult[] array2 = new DetectionResult[num];
				Dictionary<Appearance, DetectionResult> dictionary = new Dictionary<Appearance, DetectionResult>();
				num = Body.Windows_Kinect_Body_get_Appearance(this._pNative, array, array2, num);
				ExceptionHelper.CheckLastError();
				for (int i = 0; i < num; i++)
				{
					dictionary.Add(array[i], array2[i]);
				}
				return dictionary;
			}
		}

		// Token: 0x06000282 RID: 642
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern FrameEdges Windows_Kinect_Body_get_ClippedEdges(IntPtr pNative);

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000283 RID: 643 RVA: 0x00017C59 File Offset: 0x00015E59
		public FrameEdges ClippedEdges
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				return Body.Windows_Kinect_Body_get_ClippedEdges(this._pNative);
			}
		}

		// Token: 0x06000284 RID: 644
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern DetectionResult Windows_Kinect_Body_get_Engaged(IntPtr pNative);

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000285 RID: 645 RVA: 0x00017C83 File Offset: 0x00015E83
		public DetectionResult Engaged
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				return Body.Windows_Kinect_Body_get_Engaged(this._pNative);
			}
		}

		// Token: 0x06000286 RID: 646
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_Expressions(IntPtr pNative, [Out] Expression[] outKeys, [Out] DetectionResult[] outValues, int outCollectionSize);

		// Token: 0x06000287 RID: 647
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_Expressions_Length(IntPtr pNative);

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000288 RID: 648 RVA: 0x00017CB0 File Offset: 0x00015EB0
		public Dictionary<Expression, DetectionResult> Expressions
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				int num = Body.Windows_Kinect_Body_get_Expressions_Length(this._pNative);
				Expression[] array = new Expression[num];
				DetectionResult[] array2 = new DetectionResult[num];
				Dictionary<Expression, DetectionResult> dictionary = new Dictionary<Expression, DetectionResult>();
				num = Body.Windows_Kinect_Body_get_Expressions(this._pNative, array, array2, num);
				ExceptionHelper.CheckLastError();
				for (int i = 0; i < num; i++)
				{
					dictionary.Add(array[i], array2[i]);
				}
				return dictionary;
			}
		}

		// Token: 0x06000289 RID: 649
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern TrackingConfidence Windows_Kinect_Body_get_HandLeftConfidence(IntPtr pNative);

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600028A RID: 650 RVA: 0x00017D2D File Offset: 0x00015F2D
		public TrackingConfidence HandLeftConfidence
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				return Body.Windows_Kinect_Body_get_HandLeftConfidence(this._pNative);
			}
		}

		// Token: 0x0600028B RID: 651
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern HandState Windows_Kinect_Body_get_HandLeftState(IntPtr pNative);

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600028C RID: 652 RVA: 0x00017D57 File Offset: 0x00015F57
		public HandState HandLeftState
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				return Body.Windows_Kinect_Body_get_HandLeftState(this._pNative);
			}
		}

		// Token: 0x0600028D RID: 653
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern TrackingConfidence Windows_Kinect_Body_get_HandRightConfidence(IntPtr pNative);

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600028E RID: 654 RVA: 0x00017D81 File Offset: 0x00015F81
		public TrackingConfidence HandRightConfidence
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				return Body.Windows_Kinect_Body_get_HandRightConfidence(this._pNative);
			}
		}

		// Token: 0x0600028F RID: 655
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern HandState Windows_Kinect_Body_get_HandRightState(IntPtr pNative);

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000290 RID: 656 RVA: 0x00017DAB File Offset: 0x00015FAB
		public HandState HandRightState
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				return Body.Windows_Kinect_Body_get_HandRightState(this._pNative);
			}
		}

		// Token: 0x06000291 RID: 657
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_Body_get_IsRestricted(IntPtr pNative);

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000292 RID: 658 RVA: 0x00017DD5 File Offset: 0x00015FD5
		public bool IsRestricted
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				return Body.Windows_Kinect_Body_get_IsRestricted(this._pNative);
			}
		}

		// Token: 0x06000293 RID: 659
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_Body_get_IsTracked(IntPtr pNative);

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000294 RID: 660 RVA: 0x00017DFF File Offset: 0x00015FFF
		public bool IsTracked
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				return Body.Windows_Kinect_Body_get_IsTracked(this._pNative);
			}
		}

		// Token: 0x06000295 RID: 661
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_JointOrientations(IntPtr pNative, [Out] JointType[] outKeys, [Out] JointOrientation[] outValues, int outCollectionSize);

		// Token: 0x06000296 RID: 662
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_JointOrientations_Length(IntPtr pNative);

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000297 RID: 663 RVA: 0x00017E2C File Offset: 0x0001602C
		public Dictionary<JointType, JointOrientation> JointOrientations
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				int num = Body.Windows_Kinect_Body_get_JointOrientations_Length(this._pNative);
				JointType[] array = new JointType[num];
				JointOrientation[] array2 = new JointOrientation[num];
				Dictionary<JointType, JointOrientation> dictionary = new Dictionary<JointType, JointOrientation>();
				num = Body.Windows_Kinect_Body_get_JointOrientations(this._pNative, array, array2, num);
				ExceptionHelper.CheckLastError();
				for (int i = 0; i < num; i++)
				{
					dictionary.Add(array[i], array2[i]);
				}
				return dictionary;
			}
		}

		// Token: 0x06000298 RID: 664
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_Joints(IntPtr pNative, [Out] JointType[] outKeys, [Out] Joint[] outValues, int outCollectionSize);

		// Token: 0x06000299 RID: 665
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_Joints_Length(IntPtr pNative);

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600029A RID: 666 RVA: 0x00017EB0 File Offset: 0x000160B0
		public Dictionary<JointType, Joint> Joints
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				int num = Body.Windows_Kinect_Body_get_Joints_Length(this._pNative);
				JointType[] array = new JointType[num];
				Joint[] array2 = new Joint[num];
				Dictionary<JointType, Joint> dictionary = new Dictionary<JointType, Joint>();
				num = Body.Windows_Kinect_Body_get_Joints(this._pNative, array, array2, num);
				ExceptionHelper.CheckLastError();
				for (int i = 0; i < num; i++)
				{
					dictionary.Add(array[i], array2[i]);
				}
				return dictionary;
			}
		}

		// Token: 0x0600029B RID: 667
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern TrackingState Windows_Kinect_Body_get_LeanTrackingState(IntPtr pNative);

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600029C RID: 668 RVA: 0x00017F31 File Offset: 0x00016131
		public TrackingState LeanTrackingState
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				return Body.Windows_Kinect_Body_get_LeanTrackingState(this._pNative);
			}
		}

		// Token: 0x0600029D RID: 669
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern ulong Windows_Kinect_Body_get_TrackingId(IntPtr pNative);

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600029E RID: 670 RVA: 0x00017F5B File Offset: 0x0001615B
		public ulong TrackingId
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("Body");
				}
				return Body.Windows_Kinect_Body_get_TrackingId(this._pNative);
			}
		}

		// Token: 0x0600029F RID: 671
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_Body_get_JointCount();

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x00017F85 File Offset: 0x00016185
		public static int JointCount
		{
			get
			{
				return Body.Windows_Kinect_Body_get_JointCount();
			}
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00017F8C File Offset: 0x0001618C
		private void __EventCleanup()
		{
		}

		// Token: 0x04000203 RID: 515
		internal IntPtr _pNative;
	}
}
