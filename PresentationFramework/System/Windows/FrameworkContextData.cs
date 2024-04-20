using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace System.Windows
{
	// Token: 0x02000367 RID: 871
	internal class FrameworkContextData
	{
		// Token: 0x060021B5 RID: 8629 RVA: 0x00178AF8 File Offset: 0x00177AF8
		public static FrameworkContextData From(Dispatcher context)
		{
			FrameworkContextData frameworkContextData = (FrameworkContextData)context.Reserved2;
			if (frameworkContextData == null)
			{
				frameworkContextData = new FrameworkContextData();
				context.Reserved2 = frameworkContextData;
			}
			return frameworkContextData;
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x00178B22 File Offset: 0x00177B22
		private FrameworkContextData()
		{
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x00178B38 File Offset: 0x00177B38
		public void AddWalker(object data, DescendentsWalkerBase walker)
		{
			FrameworkContextData.WalkerEntry item = default(FrameworkContextData.WalkerEntry);
			item.Data = data;
			item.Walker = walker;
			this._currentWalkers.Add(item);
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x00178B6C File Offset: 0x00177B6C
		public void RemoveWalker(object data, DescendentsWalkerBase walker)
		{
			int index = this._currentWalkers.Count - 1;
			this._currentWalkers.RemoveAt(index);
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x00178B94 File Offset: 0x00177B94
		public bool WasNodeVisited(DependencyObject d, object data)
		{
			if (this._currentWalkers.Count > 0)
			{
				int index = this._currentWalkers.Count - 1;
				FrameworkContextData.WalkerEntry walkerEntry = this._currentWalkers[index];
				if (walkerEntry.Data == data)
				{
					return walkerEntry.Walker.WasVisited(d);
				}
			}
			return false;
		}

		// Token: 0x04001024 RID: 4132
		private List<FrameworkContextData.WalkerEntry> _currentWalkers = new List<FrameworkContextData.WalkerEntry>(4);

		// Token: 0x02000A7A RID: 2682
		private struct WalkerEntry
		{
			// Token: 0x0400417B RID: 16763
			public object Data;

			// Token: 0x0400417C RID: 16764
			public DescendentsWalkerBase Walker;
		}
	}
}
