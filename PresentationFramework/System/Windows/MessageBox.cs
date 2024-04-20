using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using MS.Win32;

namespace System.Windows
{
	// Token: 0x0200037F RID: 895
	public sealed class MessageBox
	{
		// Token: 0x06002435 RID: 9269 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		private MessageBox()
		{
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x00181CED File Offset: 0x00180CED
		private static MessageBoxResult Win32ToMessageBoxResult(int value)
		{
			switch (value)
			{
			case 1:
				return MessageBoxResult.OK;
			case 2:
				return MessageBoxResult.Cancel;
			case 6:
				return MessageBoxResult.Yes;
			case 7:
				return MessageBoxResult.No;
			}
			return MessageBoxResult.No;
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x00181D1E File Offset: 0x00180D1E
		public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, MessageBoxOptions options)
		{
			return MessageBox.ShowCore(IntPtr.Zero, messageBoxText, caption, button, icon, defaultResult, options);
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x00181D32 File Offset: 0x00180D32
		public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult)
		{
			return MessageBox.ShowCore(IntPtr.Zero, messageBoxText, caption, button, icon, defaultResult, MessageBoxOptions.None);
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x00181D45 File Offset: 0x00180D45
		public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
		{
			return MessageBox.ShowCore(IntPtr.Zero, messageBoxText, caption, button, icon, MessageBoxResult.None, MessageBoxOptions.None);
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x00181D57 File Offset: 0x00180D57
		public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button)
		{
			return MessageBox.ShowCore(IntPtr.Zero, messageBoxText, caption, button, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x00181D69 File Offset: 0x00180D69
		public static MessageBoxResult Show(string messageBoxText, string caption)
		{
			return MessageBox.ShowCore(IntPtr.Zero, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x00181D7B File Offset: 0x00180D7B
		public static MessageBoxResult Show(string messageBoxText)
		{
			return MessageBox.ShowCore(IntPtr.Zero, messageBoxText, string.Empty, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x00181D91 File Offset: 0x00180D91
		public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, MessageBoxOptions options)
		{
			return MessageBox.ShowCore(new WindowInteropHelper(owner).CriticalHandle, messageBoxText, caption, button, icon, defaultResult, options);
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x00181DAC File Offset: 0x00180DAC
		public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult)
		{
			return MessageBox.ShowCore(new WindowInteropHelper(owner).CriticalHandle, messageBoxText, caption, button, icon, defaultResult, MessageBoxOptions.None);
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x00181DC6 File Offset: 0x00180DC6
		public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
		{
			return MessageBox.ShowCore(new WindowInteropHelper(owner).CriticalHandle, messageBoxText, caption, button, icon, MessageBoxResult.None, MessageBoxOptions.None);
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x00181DDF File Offset: 0x00180DDF
		public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button)
		{
			return MessageBox.ShowCore(new WindowInteropHelper(owner).CriticalHandle, messageBoxText, caption, button, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x00181DF7 File Offset: 0x00180DF7
		public static MessageBoxResult Show(Window owner, string messageBoxText, string caption)
		{
			return MessageBox.ShowCore(new WindowInteropHelper(owner).CriticalHandle, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x00181E0F File Offset: 0x00180E0F
		public static MessageBoxResult Show(Window owner, string messageBoxText)
		{
			return MessageBox.ShowCore(new WindowInteropHelper(owner).CriticalHandle, messageBoxText, string.Empty, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x00181E2C File Offset: 0x00180E2C
		private static int DefaultResultToButtonNumber(MessageBoxResult result, MessageBoxButton button)
		{
			if (result == MessageBoxResult.None)
			{
				return 0;
			}
			switch (button)
			{
			case MessageBoxButton.OK:
				return 0;
			case MessageBoxButton.OKCancel:
				if (result == MessageBoxResult.Cancel)
				{
					return 256;
				}
				return 0;
			case MessageBoxButton.YesNoCancel:
				if (result == MessageBoxResult.No)
				{
					return 256;
				}
				if (result == MessageBoxResult.Cancel)
				{
					return 512;
				}
				return 0;
			case MessageBoxButton.YesNo:
				if (result == MessageBoxResult.No)
				{
					return 256;
				}
				return 0;
			}
			return 0;
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x00181E8C File Offset: 0x00180E8C
		internal static MessageBoxResult ShowCore(IntPtr owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, MessageBoxOptions options)
		{
			if (!MessageBox.IsValidMessageBoxButton(button))
			{
				throw new InvalidEnumArgumentException("button", (int)button, typeof(MessageBoxButton));
			}
			if (!MessageBox.IsValidMessageBoxImage(icon))
			{
				throw new InvalidEnumArgumentException("icon", (int)icon, typeof(MessageBoxImage));
			}
			if (!MessageBox.IsValidMessageBoxResult(defaultResult))
			{
				throw new InvalidEnumArgumentException("defaultResult", (int)defaultResult, typeof(MessageBoxResult));
			}
			if (!MessageBox.IsValidMessageBoxOptions(options))
			{
				throw new InvalidEnumArgumentException("options", (int)options, typeof(MessageBoxOptions));
			}
			if ((options & (MessageBoxOptions.ServiceNotification | MessageBoxOptions.DefaultDesktopOnly)) != MessageBoxOptions.None)
			{
				if (owner != IntPtr.Zero)
				{
					throw new ArgumentException(SR.Get("CantShowMBServiceWithOwner"));
				}
			}
			else if (owner == IntPtr.Zero)
			{
				owner = UnsafeNativeMethods.GetActiveWindow();
			}
			int type = (int)(button | (MessageBoxButton)icon | (MessageBoxButton)MessageBox.DefaultResultToButtonNumber(defaultResult, button) | (MessageBoxButton)options);
			return MessageBox.Win32ToMessageBoxResult(UnsafeNativeMethods.MessageBox(new HandleRef(null, owner), messageBoxText, caption, type));
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x00181F77 File Offset: 0x00180F77
		private static bool IsValidMessageBoxButton(MessageBoxButton value)
		{
			return value == MessageBoxButton.OK || value == MessageBoxButton.OKCancel || value == MessageBoxButton.YesNo || value == MessageBoxButton.YesNoCancel;
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x00181F8A File Offset: 0x00180F8A
		private static bool IsValidMessageBoxImage(MessageBoxImage value)
		{
			return value == MessageBoxImage.Asterisk || value == MessageBoxImage.Hand || value == MessageBoxImage.Exclamation || value == MessageBoxImage.Hand || value == MessageBoxImage.Asterisk || value == MessageBoxImage.None || value == MessageBoxImage.Question || value == MessageBoxImage.Hand || value == MessageBoxImage.Exclamation;
		}

		// Token: 0x06002447 RID: 9287 RVA: 0x00181FB9 File Offset: 0x00180FB9
		private static bool IsValidMessageBoxResult(MessageBoxResult value)
		{
			return value == MessageBoxResult.Cancel || value == MessageBoxResult.No || value == MessageBoxResult.None || value == MessageBoxResult.OK || value == MessageBoxResult.Yes;
		}

		// Token: 0x06002448 RID: 9288 RVA: 0x00181FD0 File Offset: 0x00180FD0
		private static bool IsValidMessageBoxOptions(MessageBoxOptions value)
		{
			int num = -3801089;
			return (value & (MessageBoxOptions)num) == MessageBoxOptions.None;
		}

		// Token: 0x04001114 RID: 4372
		private const int IDOK = 1;

		// Token: 0x04001115 RID: 4373
		private const int IDCANCEL = 2;

		// Token: 0x04001116 RID: 4374
		private const int IDABORT = 3;

		// Token: 0x04001117 RID: 4375
		private const int IDRETRY = 4;

		// Token: 0x04001118 RID: 4376
		private const int IDIGNORE = 5;

		// Token: 0x04001119 RID: 4377
		private const int IDYES = 6;

		// Token: 0x0400111A RID: 4378
		private const int IDNO = 7;

		// Token: 0x0400111B RID: 4379
		private const int DEFAULT_BUTTON1 = 0;

		// Token: 0x0400111C RID: 4380
		private const int DEFAULT_BUTTON2 = 256;

		// Token: 0x0400111D RID: 4381
		private const int DEFAULT_BUTTON3 = 512;
	}
}
