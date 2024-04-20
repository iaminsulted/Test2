using System;
using System.Collections;

namespace MS.Internal.Controls
{
	// Token: 0x02000253 RID: 595
	internal class EmptyEnumerable : IEnumerable
	{
		// Token: 0x0600170A RID: 5898 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		private EmptyEnumerable()
		{
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x0015CBEC File Offset: 0x0015BBEC
		IEnumerator IEnumerable.GetEnumerator()
		{
			return EmptyEnumerator.Instance;
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x0600170C RID: 5900 RVA: 0x0015CBF3 File Offset: 0x0015BBF3
		public static IEnumerable Instance
		{
			get
			{
				if (EmptyEnumerable._instance == null)
				{
					EmptyEnumerable._instance = new EmptyEnumerable();
				}
				return EmptyEnumerable._instance;
			}
		}

		// Token: 0x04000C87 RID: 3207
		private static IEnumerable _instance;
	}
}
