using System;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000166 RID: 358
	internal class ManagedChunk
	{
		// Token: 0x06000BF0 RID: 3056 RVA: 0x0012EF0C File Offset: 0x0012DF0C
		internal ManagedChunk(uint index, CHUNK_BREAKTYPE breakType, ManagedFullPropSpec attribute, uint lcid, CHUNKSTATE flags)
		{
			Invariant.Assert(breakType >= CHUNK_BREAKTYPE.CHUNK_NO_BREAK && breakType <= CHUNK_BREAKTYPE.CHUNK_EOC);
			Invariant.Assert(attribute != null);
			this._index = index;
			this._breakType = breakType;
			this._lcid = lcid;
			this._attribute = attribute;
			this._flags = flags;
			this._idChunkSource = this._index;
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000BF1 RID: 3057 RVA: 0x0012EF6C File Offset: 0x0012DF6C
		// (set) Token: 0x06000BF2 RID: 3058 RVA: 0x0012EF74 File Offset: 0x0012DF74
		internal uint ID
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000BF3 RID: 3059 RVA: 0x0012EF7D File Offset: 0x0012DF7D
		internal CHUNKSTATE Flags
		{
			get
			{
				return this._flags;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x0012EF85 File Offset: 0x0012DF85
		// (set) Token: 0x06000BF5 RID: 3061 RVA: 0x0012EF8D File Offset: 0x0012DF8D
		internal CHUNK_BREAKTYPE BreakType
		{
			get
			{
				return this._breakType;
			}
			set
			{
				Invariant.Assert(value >= CHUNK_BREAKTYPE.CHUNK_NO_BREAK && value <= CHUNK_BREAKTYPE.CHUNK_EOC);
				this._breakType = value;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x0012EFA9 File Offset: 0x0012DFA9
		// (set) Token: 0x06000BF7 RID: 3063 RVA: 0x0012EFB1 File Offset: 0x0012DFB1
		internal uint Locale
		{
			get
			{
				return this._lcid;
			}
			set
			{
				this._lcid = value;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x0012EFBA File Offset: 0x0012DFBA
		internal uint ChunkSource
		{
			get
			{
				return this._idChunkSource;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000BF9 RID: 3065 RVA: 0x0012EFC2 File Offset: 0x0012DFC2
		internal uint StartSource
		{
			get
			{
				return this._startSource;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000BFA RID: 3066 RVA: 0x0012EFCA File Offset: 0x0012DFCA
		internal uint LenSource
		{
			get
			{
				return this._lenSource;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000BFB RID: 3067 RVA: 0x0012EFD2 File Offset: 0x0012DFD2
		// (set) Token: 0x06000BFC RID: 3068 RVA: 0x0012EFDA File Offset: 0x0012DFDA
		internal ManagedFullPropSpec Attribute
		{
			get
			{
				return this._attribute;
			}
			set
			{
				this._attribute = value;
			}
		}

		// Token: 0x04000908 RID: 2312
		private uint _index;

		// Token: 0x04000909 RID: 2313
		private CHUNK_BREAKTYPE _breakType;

		// Token: 0x0400090A RID: 2314
		private CHUNKSTATE _flags;

		// Token: 0x0400090B RID: 2315
		private uint _lcid;

		// Token: 0x0400090C RID: 2316
		private ManagedFullPropSpec _attribute;

		// Token: 0x0400090D RID: 2317
		private uint _idChunkSource;

		// Token: 0x0400090E RID: 2318
		private uint _startSource;

		// Token: 0x0400090F RID: 2319
		private uint _lenSource;
	}
}
