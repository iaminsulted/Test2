using System;
using System.Collections;
using System.Runtime.InteropServices;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x02000644 RID: 1604
	internal class MoveSizeWinEventHandler : WinEventHandler
	{
		// Token: 0x06004F4A RID: 20298 RVA: 0x00243C28 File Offset: 0x00242C28
		internal MoveSizeWinEventHandler() : base(11, 11)
		{
		}

		// Token: 0x06004F4B RID: 20299 RVA: 0x00243C34 File Offset: 0x00242C34
		internal void RegisterTextStore(TextStore textstore)
		{
			if (this._arTextStore == null)
			{
				this._arTextStore = new ArrayList(1);
			}
			this._arTextStore.Add(textstore);
		}

		// Token: 0x06004F4C RID: 20300 RVA: 0x00243C57 File Offset: 0x00242C57
		internal void UnregisterTextStore(TextStore textstore)
		{
			this._arTextStore.Remove(textstore);
		}

		// Token: 0x06004F4D RID: 20301 RVA: 0x00243C68 File Offset: 0x00242C68
		internal override void WinEventProc(int eventId, IntPtr hwnd)
		{
			Invariant.Assert(eventId == 11);
			if (this._arTextStore != null)
			{
				for (int i = 0; i < this._arTextStore.Count; i++)
				{
					bool flag = false;
					TextStore textStore = (TextStore)this._arTextStore[i];
					IntPtr intPtr = textStore.CriticalSourceWnd;
					while (intPtr != IntPtr.Zero)
					{
						if (hwnd == intPtr)
						{
							textStore.OnLayoutUpdated();
							flag = true;
							break;
						}
						intPtr = UnsafeNativeMethods.GetParent(new HandleRef(this, intPtr));
					}
					if (!flag)
					{
						textStore.MakeLayoutChangeOnGotFocus();
					}
				}
			}
		}

		// Token: 0x17001269 RID: 4713
		// (get) Token: 0x06004F4E RID: 20302 RVA: 0x00243CF0 File Offset: 0x00242CF0
		internal int TextStoreCount
		{
			get
			{
				return this._arTextStore.Count;
			}
		}

		// Token: 0x04002850 RID: 10320
		private ArrayList _arTextStore;
	}
}
