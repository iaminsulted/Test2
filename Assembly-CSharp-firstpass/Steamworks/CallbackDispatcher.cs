using System;
using UnityEngine;

namespace Steamworks
{
	// Token: 0x020001AC RID: 428
	public static class CallbackDispatcher
	{
		// Token: 0x06000D0F RID: 3343 RVA: 0x00029928 File Offset: 0x00027B28
		public static void ExceptionHandler(Exception e)
		{
			Debug.LogException(e);
		}
	}
}
