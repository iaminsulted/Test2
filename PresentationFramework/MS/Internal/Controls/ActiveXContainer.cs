using System;
using System.Windows.Interop;
using MS.Win32;

namespace MS.Internal.Controls
{
	// Token: 0x0200024F RID: 591
	internal class ActiveXContainer : UnsafeNativeMethods.IOleContainer, UnsafeNativeMethods.IOleInPlaceFrame
	{
		// Token: 0x060016C6 RID: 5830 RVA: 0x0015C2EF File Offset: 0x0015B2EF
		internal ActiveXContainer(ActiveXHost host)
		{
			this._host = host;
			Invariant.Assert(this._host != null);
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x0015C30C File Offset: 0x0015B30C
		int UnsafeNativeMethods.IOleContainer.ParseDisplayName(object pbc, string pszDisplayName, int[] pchEaten, object[] ppmkOut)
		{
			if (ppmkOut != null)
			{
				ppmkOut[0] = null;
			}
			return -2147467263;
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x0015C31C File Offset: 0x0015B31C
		int UnsafeNativeMethods.IOleContainer.EnumObjects(int grfFlags, out UnsafeNativeMethods.IEnumUnknown ppenum)
		{
			ppenum = null;
			object activeXInstance = this._host.ActiveXInstance;
			if (activeXInstance != null && ((grfFlags & 1) != 0 || ((grfFlags & 16) != 0 && this._host.ActiveXState == ActiveXHelper.ActiveXState.Running)))
			{
				ppenum = new EnumUnknown(new object[]
				{
					activeXInstance
				});
				return 0;
			}
			ppenum = new EnumUnknown(null);
			return 0;
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IOleContainer.LockContainer(bool fLock)
		{
			return -2147467263;
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x0015C37C File Offset: 0x0015B37C
		IntPtr UnsafeNativeMethods.IOleInPlaceFrame.GetWindow()
		{
			return this._host.ParentHandle.Handle;
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x00105F35 File Offset: 0x00104F35
		int UnsafeNativeMethods.IOleInPlaceFrame.ContextSensitiveHelp(int fEnterMode)
		{
			return 0;
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IOleInPlaceFrame.GetBorder(NativeMethods.COMRECT lprectBorder)
		{
			return -2147467263;
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IOleInPlaceFrame.RequestBorderSpace(NativeMethods.COMRECT pborderwidths)
		{
			return -2147467263;
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IOleInPlaceFrame.SetBorderSpace(NativeMethods.COMRECT pborderwidths)
		{
			return -2147467263;
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x00105F35 File Offset: 0x00104F35
		int UnsafeNativeMethods.IOleInPlaceFrame.SetActiveObject(UnsafeNativeMethods.IOleInPlaceActiveObject pActiveObject, string pszObjName)
		{
			return 0;
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x00105F35 File Offset: 0x00104F35
		int UnsafeNativeMethods.IOleInPlaceFrame.InsertMenus(IntPtr hmenuShared, NativeMethods.tagOleMenuGroupWidths lpMenuWidths)
		{
			return 0;
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IOleInPlaceFrame.SetMenu(IntPtr hmenuShared, IntPtr holemenu, IntPtr hwndActiveObject)
		{
			return -2147467263;
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IOleInPlaceFrame.RemoveMenus(IntPtr hmenuShared)
		{
			return -2147467263;
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IOleInPlaceFrame.SetStatusText(string pszStatusText)
		{
			return -2147467263;
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IOleInPlaceFrame.EnableModeless(bool fEnable)
		{
			return -2147467263;
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		int UnsafeNativeMethods.IOleInPlaceFrame.TranslateAccelerator(ref MSG lpmsg, short wID)
		{
			return 1;
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x0015C39C File Offset: 0x0015B39C
		internal void OnUIActivate(ActiveXHost site)
		{
			if (this._siteUIActive == site)
			{
				return;
			}
			if (this._siteUIActive != null)
			{
				this._siteUIActive.ActiveXInPlaceObject.UIDeactivate();
			}
			this._siteUIActive = site;
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x0015C3C8 File Offset: 0x0015B3C8
		internal void OnUIDeactivate(ActiveXHost site)
		{
			this._siteUIActive = null;
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x0015C3D1 File Offset: 0x0015B3D1
		internal void OnInPlaceDeactivate(ActiveXHost site)
		{
			ActiveXHost activeXHost = this.ActiveXHost;
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x060016D9 RID: 5849 RVA: 0x0015C3DC File Offset: 0x0015B3DC
		internal ActiveXHost ActiveXHost
		{
			get
			{
				return this._host;
			}
		}

		// Token: 0x04000C7B RID: 3195
		private ActiveXHost _host;

		// Token: 0x04000C7C RID: 3196
		private ActiveXHost _siteUIActive;
	}
}
