using System;
using UnityEngine;

namespace IngameDebugConsole
{
	// Token: 0x020001EA RID: 490
	public class DebugLogEntry : IEquatable<DebugLogEntry>
	{
		// Token: 0x06000F60 RID: 3936 RVA: 0x0002DB4E File Offset: 0x0002BD4E
		public DebugLogEntry(string logString, string stackTrace, Sprite sprite)
		{
			this.logString = logString;
			this.stackTrace = stackTrace;
			this.logTypeSpriteRepresentation = sprite;
			this.count = 1;
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x0002DB7D File Offset: 0x0002BD7D
		public bool Equals(DebugLogEntry other)
		{
			return this.logString == other.logString && this.stackTrace == other.stackTrace;
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x0002DBA5 File Offset: 0x0002BDA5
		public override string ToString()
		{
			if (this.completeLog == null)
			{
				this.completeLog = this.logString + "\n" + this.stackTrace;
			}
			return this.completeLog;
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x0002DBD4 File Offset: 0x0002BDD4
		public override int GetHashCode()
		{
			if (this.hashValue == -623218)
			{
				this.hashValue = 17;
				this.hashValue = (((this.hashValue * 23).ToString() + this.logString == null) ? 0 : this.logString.GetHashCode());
				this.hashValue = (((this.hashValue * 23).ToString() + this.stackTrace == null) ? 0 : this.stackTrace.GetHashCode());
			}
			return this.hashValue;
		}

		// Token: 0x04000ABE RID: 2750
		private const int HASH_NOT_CALCULATED = -623218;

		// Token: 0x04000ABF RID: 2751
		public string logString;

		// Token: 0x04000AC0 RID: 2752
		public string stackTrace;

		// Token: 0x04000AC1 RID: 2753
		private string completeLog;

		// Token: 0x04000AC2 RID: 2754
		public Sprite logTypeSpriteRepresentation;

		// Token: 0x04000AC3 RID: 2755
		public int count;

		// Token: 0x04000AC4 RID: 2756
		private int hashValue = -623218;
	}
}
