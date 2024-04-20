using System;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000161 RID: 353
	internal class IndexingContentUnit : ManagedChunk
	{
		// Token: 0x06000BB3 RID: 2995 RVA: 0x0012D31A File Offset: 0x0012C31A
		internal IndexingContentUnit(string contents, uint chunkID, CHUNK_BREAKTYPE breakType, ManagedFullPropSpec attribute, uint lcid) : base(chunkID, breakType, attribute, lcid, CHUNKSTATE.CHUNK_TEXT)
		{
			this._contents = contents;
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x0012D330 File Offset: 0x0012C330
		internal void InitIndexingContentUnit(string contents, uint chunkID, CHUNK_BREAKTYPE breakType, ManagedFullPropSpec attribute, uint lcid)
		{
			this._contents = contents;
			base.ID = chunkID;
			base.BreakType = breakType;
			base.Attribute = attribute;
			base.Locale = lcid;
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x0012D357 File Offset: 0x0012C357
		internal string Text
		{
			get
			{
				return this._contents;
			}
		}

		// Token: 0x040008E2 RID: 2274
		private string _contents;
	}
}
