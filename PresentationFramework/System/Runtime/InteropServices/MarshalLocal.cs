using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200033B RID: 827
	internal static class MarshalLocal
	{
		// Token: 0x06001F2A RID: 7978 RVA: 0x0017120C File Offset: 0x0017020C
		public static bool IsTypeVisibleFromCom(Type type)
		{
			try
			{
				Marshal.GetStartComSlot(type);
			}
			catch (ArgumentException)
			{
				if (type == null)
				{
					throw;
				}
				return false;
			}
			return true;
		}
	}
}
