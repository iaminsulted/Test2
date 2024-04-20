using System;
using System.Windows.Input;

namespace System.Windows.Controls
{
	// Token: 0x02000726 RID: 1830
	internal static class CalendarKeyboardHelper
	{
		// Token: 0x06006087 RID: 24711 RVA: 0x00299D94 File Offset: 0x00298D94
		public static void GetMetaKeyState(out bool ctrl, out bool shift)
		{
			ctrl = ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control);
			shift = ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift);
		}
	}
}
