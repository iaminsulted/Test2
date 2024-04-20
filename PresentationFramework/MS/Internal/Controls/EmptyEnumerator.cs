using System;
using System.Collections;

namespace MS.Internal.Controls
{
	// Token: 0x02000254 RID: 596
	internal class EmptyEnumerator : IEnumerator
	{
		// Token: 0x0600170D RID: 5901 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		private EmptyEnumerator()
		{
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x0600170E RID: 5902 RVA: 0x0015CC0B File Offset: 0x0015BC0B
		public static IEnumerator Instance
		{
			get
			{
				if (EmptyEnumerator._instance == null)
				{
					EmptyEnumerator._instance = new EmptyEnumerator();
				}
				return EmptyEnumerator._instance;
			}
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void Reset()
		{
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool MoveNext()
		{
			return false;
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001711 RID: 5905 RVA: 0x00108C4F File Offset: 0x00107C4F
		public object Current
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x04000C88 RID: 3208
		private static IEnumerator _instance;
	}
}
