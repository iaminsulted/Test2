using System;
using System.Collections;
using System.Globalization;

namespace MS.Internal.Utility
{
	// Token: 0x020002E2 RID: 738
	internal class TraceLog
	{
		// Token: 0x06001BDC RID: 7132 RVA: 0x0016A879 File Offset: 0x00169879
		internal TraceLog() : this(int.MaxValue)
		{
		}

		// Token: 0x06001BDD RID: 7133 RVA: 0x0016A886 File Offset: 0x00169886
		internal TraceLog(int size)
		{
			this._size = size;
			this._log = new ArrayList();
		}

		// Token: 0x06001BDE RID: 7134 RVA: 0x0016A8A0 File Offset: 0x001698A0
		internal void Add(string message, params object[] args)
		{
			string value = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) + " " + string.Format(CultureInfo.InvariantCulture, message, args);
			if (this._log.Count == this._size)
			{
				this._log.RemoveAt(0);
			}
			this._log.Add(value);
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x0016A90C File Offset: 0x0016990C
		internal void WriteLog()
		{
			for (int i = 0; i < this._log.Count; i++)
			{
				Console.WriteLine(this._log[i]);
			}
		}

		// Token: 0x06001BE0 RID: 7136 RVA: 0x0016A940 File Offset: 0x00169940
		internal static string IdFor(object o)
		{
			if (o == null)
			{
				return "NULL";
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", o.GetType().Name, o.GetHashCode());
		}

		// Token: 0x04000E69 RID: 3689
		private ArrayList _log;

		// Token: 0x04000E6A RID: 3690
		private int _size;
	}
}
