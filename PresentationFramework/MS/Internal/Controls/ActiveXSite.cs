using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using MS.Win32;

namespace MS.Internal.Controls
{
	// Token: 0x02000251 RID: 593
	internal class ActiveXSite : UnsafeNativeMethods.IOleControlSite, UnsafeNativeMethods.IOleClientSite, UnsafeNativeMethods.IOleInPlaceSite, UnsafeNativeMethods.IPropertyNotifySink
	{
		// Token: 0x060016E3 RID: 5859 RVA: 0x0015C4F9 File Offset: 0x0015B4F9
		internal ActiveXSite(ActiveXHost host)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			this._host = host;
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x00105F35 File Offset: 0x00104F35
		int UnsafeNativeMethods.IOleControlSite.OnControlInfoChanged()
		{
			return 0;
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IOleControlSite.LockInPlaceActive(int fLock)
		{
			return -2147467263;
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x0015C516 File Offset: 0x0015B516
		int UnsafeNativeMethods.IOleControlSite.GetExtendedControl(out object ppDisp)
		{
			ppDisp = null;
			return -2147467263;
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x0015C520 File Offset: 0x0015B520
		int UnsafeNativeMethods.IOleControlSite.TransformCoords(NativeMethods.POINT pPtlHimetric, NativeMethods.POINTF pPtfContainer, int dwFlags)
		{
			if ((dwFlags & 4) != 0)
			{
				if ((dwFlags & 2) != 0)
				{
					pPtfContainer.x = (float)ActiveXHelper.HM2Pix(pPtlHimetric.x, ActiveXHelper.LogPixelsX);
					pPtfContainer.y = (float)ActiveXHelper.HM2Pix(pPtlHimetric.y, ActiveXHelper.LogPixelsY);
				}
				else
				{
					if ((dwFlags & 1) == 0)
					{
						return -2147024809;
					}
					pPtfContainer.x = (float)ActiveXHelper.HM2Pix(pPtlHimetric.x, ActiveXHelper.LogPixelsX);
					pPtfContainer.y = (float)ActiveXHelper.HM2Pix(pPtlHimetric.y, ActiveXHelper.LogPixelsY);
				}
			}
			else
			{
				if ((dwFlags & 8) == 0)
				{
					return -2147024809;
				}
				if ((dwFlags & 2) != 0)
				{
					pPtlHimetric.x = ActiveXHelper.Pix2HM((int)pPtfContainer.x, ActiveXHelper.LogPixelsX);
					pPtlHimetric.y = ActiveXHelper.Pix2HM((int)pPtfContainer.y, ActiveXHelper.LogPixelsY);
				}
				else
				{
					if ((dwFlags & 1) == 0)
					{
						return -2147024809;
					}
					pPtlHimetric.x = ActiveXHelper.Pix2HM((int)pPtfContainer.x, ActiveXHelper.LogPixelsX);
					pPtlHimetric.y = ActiveXHelper.Pix2HM((int)pPtfContainer.y, ActiveXHelper.LogPixelsY);
				}
			}
			return 0;
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		int UnsafeNativeMethods.IOleControlSite.TranslateAccelerator(ref MSG pMsg, int grfModifiers)
		{
			return 1;
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x00105F35 File Offset: 0x00104F35
		int UnsafeNativeMethods.IOleControlSite.OnFocus(int fGotFocus)
		{
			return 0;
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IOleControlSite.ShowPropertyFrame()
		{
			return -2147467263;
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IOleClientSite.SaveObject()
		{
			return -2147467263;
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x0015C624 File Offset: 0x0015B624
		int UnsafeNativeMethods.IOleClientSite.GetMoniker(int dwAssign, int dwWhichMoniker, out object moniker)
		{
			moniker = null;
			return -2147467263;
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x0015C62E File Offset: 0x0015B62E
		int UnsafeNativeMethods.IOleClientSite.GetContainer(out UnsafeNativeMethods.IOleContainer container)
		{
			container = this.Host.Container;
			return 0;
		}

		// Token: 0x060016EE RID: 5870 RVA: 0x0015C640 File Offset: 0x0015B640
		int UnsafeNativeMethods.IOleClientSite.ShowObject()
		{
			if (this.HostState >= ActiveXHelper.ActiveXState.InPlaceActive)
			{
				IntPtr intPtr;
				if (NativeMethods.Succeeded(this.Host.ActiveXInPlaceObject.GetWindow(out intPtr)))
				{
					if (this.Host.ControlHandle.Handle != intPtr && intPtr != IntPtr.Zero)
					{
						this.Host.AttachWindow(intPtr);
						this.OnActiveXRectChange(this.Host.Bounds);
					}
				}
				else if (this.Host.ActiveXInPlaceObject is UnsafeNativeMethods.IOleInPlaceObjectWindowless)
				{
					throw new InvalidOperationException(SR.Get("AxWindowlessControl"));
				}
			}
			return 0;
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x00105F35 File Offset: 0x00104F35
		int UnsafeNativeMethods.IOleClientSite.OnShowWindow(int fShow)
		{
			return 0;
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IOleClientSite.RequestNewObjectLayout()
		{
			return -2147467263;
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x0015C6E0 File Offset: 0x0015B6E0
		IntPtr UnsafeNativeMethods.IOleInPlaceSite.GetWindow()
		{
			IntPtr handle;
			try
			{
				handle = this.Host.ParentHandle.Handle;
			}
			catch (Exception)
			{
				throw;
			}
			return handle;
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IOleInPlaceSite.ContextSensitiveHelp(int fEnterMode)
		{
			return -2147467263;
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x00105F35 File Offset: 0x00104F35
		int UnsafeNativeMethods.IOleInPlaceSite.CanInPlaceActivate()
		{
			return 0;
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x0015C718 File Offset: 0x0015B718
		int UnsafeNativeMethods.IOleInPlaceSite.OnInPlaceActivate()
		{
			this.HostState = ActiveXHelper.ActiveXState.InPlaceActive;
			if (!this.HostBounds.IsEmpty)
			{
				this.OnActiveXRectChange(this.HostBounds);
			}
			return 0;
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x0015C73C File Offset: 0x0015B73C
		int UnsafeNativeMethods.IOleInPlaceSite.OnUIActivate()
		{
			this.HostState = ActiveXHelper.ActiveXState.UIActive;
			this.Host.Container.OnUIActivate(this.Host);
			return 0;
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x0015C75C File Offset: 0x0015B75C
		int UnsafeNativeMethods.IOleInPlaceSite.GetWindowContext(out UnsafeNativeMethods.IOleInPlaceFrame ppFrame, out UnsafeNativeMethods.IOleInPlaceUIWindow ppDoc, NativeMethods.COMRECT lprcPosRect, NativeMethods.COMRECT lprcClipRect, NativeMethods.OLEINPLACEFRAMEINFO lpFrameInfo)
		{
			ppDoc = null;
			ppFrame = this.Host.Container;
			lprcPosRect.left = this.Host.Bounds.left;
			lprcPosRect.top = this.Host.Bounds.top;
			lprcPosRect.right = this.Host.Bounds.right;
			lprcPosRect.bottom = this.Host.Bounds.bottom;
			lprcClipRect = this.Host.Bounds;
			if (lpFrameInfo != null)
			{
				lpFrameInfo.cb = (uint)Marshal.SizeOf(typeof(NativeMethods.OLEINPLACEFRAMEINFO));
				lpFrameInfo.fMDIApp = false;
				lpFrameInfo.hAccel = IntPtr.Zero;
				lpFrameInfo.cAccelEntries = 0U;
				lpFrameInfo.hwndFrame = this.Host.ParentHandle.Handle;
			}
			return 0;
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		int UnsafeNativeMethods.IOleInPlaceSite.Scroll(NativeMethods.SIZE scrollExtant)
		{
			return 1;
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x0015C82F File Offset: 0x0015B82F
		int UnsafeNativeMethods.IOleInPlaceSite.OnUIDeactivate(int fUndoable)
		{
			this.Host.Container.OnUIDeactivate(this.Host);
			if (this.HostState > ActiveXHelper.ActiveXState.InPlaceActive)
			{
				this.HostState = ActiveXHelper.ActiveXState.InPlaceActive;
			}
			return 0;
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x0015C858 File Offset: 0x0015B858
		int UnsafeNativeMethods.IOleInPlaceSite.OnInPlaceDeactivate()
		{
			if (this.HostState == ActiveXHelper.ActiveXState.UIActive)
			{
				((UnsafeNativeMethods.IOleInPlaceSite)this).OnUIDeactivate(0);
			}
			this.Host.Container.OnInPlaceDeactivate(this.Host);
			this.HostState = ActiveXHelper.ActiveXState.Running;
			return 0;
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x00105F35 File Offset: 0x00104F35
		int UnsafeNativeMethods.IOleInPlaceSite.DiscardUndoState()
		{
			return 0;
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x0015C889 File Offset: 0x0015B889
		int UnsafeNativeMethods.IOleInPlaceSite.DeactivateAndUndo()
		{
			return this.Host.ActiveXInPlaceObject.UIDeactivate();
		}

		// Token: 0x060016FC RID: 5884 RVA: 0x0015C89B File Offset: 0x0015B89B
		int UnsafeNativeMethods.IOleInPlaceSite.OnPosRectChange(NativeMethods.COMRECT lprcPosRect)
		{
			return this.OnActiveXRectChange(lprcPosRect);
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x060016FD RID: 5885 RVA: 0x0015C8A4 File Offset: 0x0015B8A4
		// (set) Token: 0x060016FE RID: 5886 RVA: 0x0015C8B1 File Offset: 0x0015B8B1
		private ActiveXHelper.ActiveXState HostState
		{
			get
			{
				return this.Host.ActiveXState;
			}
			set
			{
				this.Host.ActiveXState = value;
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x060016FF RID: 5887 RVA: 0x0015C8BF File Offset: 0x0015B8BF
		internal NativeMethods.COMRECT HostBounds
		{
			get
			{
				return this.Host.Bounds;
			}
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x0015C8CC File Offset: 0x0015B8CC
		void UnsafeNativeMethods.IPropertyNotifySink.OnChanged(int dispid)
		{
			try
			{
				this.OnPropertyChanged(dispid);
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x00105F35 File Offset: 0x00104F35
		int UnsafeNativeMethods.IPropertyNotifySink.OnRequestEdit(int dispid)
		{
			return 0;
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void OnPropertyChanged(int dispid)
		{
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001703 RID: 5891 RVA: 0x0015C8F8 File Offset: 0x0015B8F8
		internal ActiveXHost Host
		{
			get
			{
				return this._host;
			}
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x0015C900 File Offset: 0x0015B900
		internal void StartEvents()
		{
			if (this._connectionPoint != null)
			{
				return;
			}
			object activeXInstance = this.Host.ActiveXInstance;
			if (activeXInstance != null)
			{
				try
				{
					this._connectionPoint = new ConnectionPointCookie(activeXInstance, this, typeof(UnsafeNativeMethods.IPropertyNotifySink));
				}
				catch (Exception ex)
				{
					if (CriticalExceptions.IsCriticalException(ex))
					{
						throw;
					}
				}
			}
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x0015C95C File Offset: 0x0015B95C
		internal void StopEvents()
		{
			if (this._connectionPoint != null)
			{
				this._connectionPoint.Disconnect();
				this._connectionPoint = null;
			}
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x0015C978 File Offset: 0x0015B978
		internal int OnActiveXRectChange(NativeMethods.COMRECT lprcPosRect)
		{
			if (this.Host.ActiveXInPlaceObject != null)
			{
				this.Host.ActiveXInPlaceObject.SetObjectRects(lprcPosRect, lprcPosRect);
				this.Host.Bounds = lprcPosRect;
			}
			return 0;
		}

		// Token: 0x04000C83 RID: 3203
		private ActiveXHost _host;

		// Token: 0x04000C84 RID: 3204
		private ConnectionPointCookie _connectionPoint;
	}
}
