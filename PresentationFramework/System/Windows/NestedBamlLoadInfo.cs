using System;
using System.IO;

namespace System.Windows
{
	// Token: 0x02000342 RID: 834
	internal class NestedBamlLoadInfo
	{
		// Token: 0x06001FC0 RID: 8128 RVA: 0x0017316A File Offset: 0x0017216A
		internal NestedBamlLoadInfo(Uri uri, Stream stream, bool bSkipJournalProperty)
		{
			this._BamlUri = uri;
			this._BamlStream = stream;
			this._SkipJournaledProperties = bSkipJournalProperty;
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001FC1 RID: 8129 RVA: 0x00173187 File Offset: 0x00172187
		// (set) Token: 0x06001FC2 RID: 8130 RVA: 0x0017318F File Offset: 0x0017218F
		internal Uri BamlUri
		{
			get
			{
				return this._BamlUri;
			}
			set
			{
				this._BamlUri = value;
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001FC3 RID: 8131 RVA: 0x00173198 File Offset: 0x00172198
		internal Stream BamlStream
		{
			get
			{
				return this._BamlStream;
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001FC4 RID: 8132 RVA: 0x001731A0 File Offset: 0x001721A0
		internal bool SkipJournaledProperties
		{
			get
			{
				return this._SkipJournaledProperties;
			}
		}

		// Token: 0x04000F99 RID: 3993
		private Uri _BamlUri;

		// Token: 0x04000F9A RID: 3994
		private Stream _BamlStream;

		// Token: 0x04000F9B RID: 3995
		private bool _SkipJournaledProperties;
	}
}
