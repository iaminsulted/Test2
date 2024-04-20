using System;
using System.Runtime.InteropServices;
using System.Windows;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000162 RID: 354
	internal class IndexingFilterMarshaler : IFilter
	{
		// Token: 0x06000BB6 RID: 2998 RVA: 0x0012D35F File Offset: 0x0012C35F
		internal IndexingFilterMarshaler(IManagedFilter managedFilter)
		{
			if (managedFilter == null)
			{
				throw new ArgumentNullException("managedFilter");
			}
			this._implementation = managedFilter;
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x0012D384 File Offset: 0x0012C384
		internal static ManagedFullPropSpec[] MarshalFullPropSpecArray(uint cAttributes, FULLPROPSPEC[] aAttributes)
		{
			if (cAttributes > 0U)
			{
				Invariant.Assert(aAttributes != null);
				ManagedFullPropSpec[] array = new ManagedFullPropSpec[checked((int)cAttributes)];
				int num = 0;
				while ((long)num < (long)((ulong)cAttributes))
				{
					array[num] = new ManagedFullPropSpec(aAttributes[num]);
					num++;
				}
				return array;
			}
			return null;
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x0012D3C8 File Offset: 0x0012C3C8
		internal static void MarshalStringToPtr(string s, ref uint bufCharacterCount, IntPtr p)
		{
			Invariant.Assert(bufCharacterCount > 0U);
			if (s.Length > (int)(bufCharacterCount - 1U))
			{
				throw new InvalidOperationException(SR.Get("FilterGetTextBufferOverflow"));
			}
			bufCharacterCount = (uint)(s.Length + 1);
			Marshal.Copy(s.ToCharArray(), 0, p, s.Length);
			Marshal.WriteInt16(p, s.Length * 2, 0);
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0012D428 File Offset: 0x0012C428
		internal static void MarshalPropSpec(ManagedPropSpec propSpec, ref PROPSPEC native)
		{
			native.propType = (uint)propSpec.PropType;
			PropSpecType propType = propSpec.PropType;
			if (propType == PropSpecType.Name)
			{
				native.union.name = Marshal.StringToCoTaskMemUni(propSpec.PropName);
				return;
			}
			if (propType == PropSpecType.Id)
			{
				native.union.propId = propSpec.PropId;
				return;
			}
			Invariant.Assert(false);
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0012D47E File Offset: 0x0012C47E
		internal static void MarshalFullPropSpec(ManagedFullPropSpec fullPropSpec, ref FULLPROPSPEC native)
		{
			native.guid = fullPropSpec.Guid;
			IndexingFilterMarshaler.MarshalPropSpec(fullPropSpec.Property, ref native.property);
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x0012D4A0 File Offset: 0x0012C4A0
		internal static STAT_CHUNK MarshalChunk(ManagedChunk chunk)
		{
			STAT_CHUNK result = default(STAT_CHUNK);
			result.idChunk = chunk.ID;
			Invariant.Assert(chunk.BreakType >= CHUNK_BREAKTYPE.CHUNK_NO_BREAK && chunk.BreakType <= CHUNK_BREAKTYPE.CHUNK_EOC);
			result.breakType = chunk.BreakType;
			Invariant.Assert(chunk.Flags >= (CHUNKSTATE)0 && chunk.Flags <= (CHUNKSTATE.CHUNK_TEXT | CHUNKSTATE.CHUNK_VALUE | CHUNKSTATE.CHUNK_FILTER_OWNED_VALUE));
			result.flags = chunk.Flags;
			result.locale = chunk.Locale;
			result.idChunkSource = chunk.ChunkSource;
			result.cwcStartSource = chunk.StartSource;
			result.cwcLenSource = chunk.LenSource;
			IndexingFilterMarshaler.MarshalFullPropSpec(chunk.Attribute, ref result.attribute);
			return result;
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x0012D560 File Offset: 0x0012C560
		internal static IntPtr MarshalPropVariant(object obj)
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			try
			{
				PROPVARIANT structure;
				if (obj is string)
				{
					intPtr = Marshal.StringToCoTaskMemAnsi((string)obj);
					structure = default(PROPVARIANT);
					structure.vt = VARTYPE.VT_LPSTR;
					structure.union.pszVal = intPtr;
				}
				else
				{
					if (!(obj is DateTime))
					{
						throw new InvalidOperationException(SR.Get("FilterGetValueMustBeStringOrDateTime"));
					}
					structure = default(PROPVARIANT);
					structure.vt = VARTYPE.VT_FILETIME;
					long num = ((DateTime)obj).ToFileTime();
					structure.union.filetime.dwLowDateTime = (int)num;
					structure.union.filetime.dwHighDateTime = (int)(num >> 32 & (long)((ulong)-1));
				}
				intPtr2 = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
				Invariant.Assert(intPtr2 != IntPtr.Zero);
				Marshal.StructureToPtr<PROPVARIANT>(structure, intPtr2, false);
			}
			catch
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(intPtr2);
				}
				throw;
			}
			return intPtr2;
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0012D680 File Offset: 0x0012C680
		public IFILTER_FLAGS Init(IFILTER_INIT grfFlags, uint cAttributes, FULLPROPSPEC[] aAttributes)
		{
			ManagedFullPropSpec[] aAttributes2 = IndexingFilterMarshaler.MarshalFullPropSpecArray(cAttributes, aAttributes);
			return this._implementation.Init(grfFlags, aAttributes2);
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x0012D6A4 File Offset: 0x0012C6A4
		public STAT_CHUNK GetChunk()
		{
			ManagedChunk chunk = this._implementation.GetChunk();
			if (chunk != null)
			{
				return IndexingFilterMarshaler.MarshalChunk(chunk);
			}
			if (this.ThrowOnEndOfChunks)
			{
				throw new COMException(SR.Get("FilterEndOfChunks"), -2147215616);
			}
			return new STAT_CHUNK
			{
				idChunk = 0U
			};
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x0012D6F5 File Offset: 0x0012C6F5
		public void GetText(ref uint bufCharacterCount, IntPtr pBuffer)
		{
			IndexingFilterMarshaler.MarshalStringToPtr(this._implementation.GetText((int)(bufCharacterCount - 1U)), ref bufCharacterCount, pBuffer);
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x0012D70D File Offset: 0x0012C70D
		public IntPtr GetValue()
		{
			return IndexingFilterMarshaler.MarshalPropVariant(this._implementation.GetValue());
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x0012D27E File Offset: 0x0012C27E
		public IntPtr BindRegion(FILTERREGION origPos, ref Guid riid)
		{
			throw new NotImplementedException(SR.Get("FilterBindRegionNotImplemented"));
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x0012D71F File Offset: 0x0012C71F
		// (set) Token: 0x06000BC3 RID: 3011 RVA: 0x0012D727 File Offset: 0x0012C727
		internal bool ThrowOnEndOfChunks
		{
			get
			{
				return this._throwOnEndOfChunks;
			}
			set
			{
				this._throwOnEndOfChunks = value;
			}
		}

		// Token: 0x040008E3 RID: 2275
		internal static Guid PSGUID_STORAGE = new Guid(3072717104U, 18415, 4122, 165, 241, 2, 96, 140, 158, 235, 172);

		// Token: 0x040008E4 RID: 2276
		internal const int _int16Size = 2;

		// Token: 0x040008E5 RID: 2277
		private IManagedFilter _implementation;

		// Token: 0x040008E6 RID: 2278
		private bool _throwOnEndOfChunks = true;
	}
}
