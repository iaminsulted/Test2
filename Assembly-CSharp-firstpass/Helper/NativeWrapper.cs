using System;

namespace Helper
{
	// Token: 0x02000035 RID: 53
	public static class NativeWrapper
	{
		// Token: 0x060001E9 RID: 489 RVA: 0x00016A28 File Offset: 0x00014C28
		public static IntPtr GetNativePtr(object obj)
		{
			if (obj == null)
			{
				return IntPtr.Zero;
			}
			INativeWrapper nativeWrapper = obj as INativeWrapper;
			if (nativeWrapper != null)
			{
				return nativeWrapper.nativePtr;
			}
			throw new ArgumentException("Object must wrap native type");
		}
	}
}
