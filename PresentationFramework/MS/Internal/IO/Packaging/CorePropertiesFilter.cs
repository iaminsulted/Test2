using System;
using System.Globalization;
using System.IO.Packaging;
using System.Runtime.InteropServices;
using System.Windows;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200015D RID: 349
	internal class CorePropertiesFilter : IManagedFilter
	{
		// Token: 0x06000B9C RID: 2972 RVA: 0x0012CD23 File Offset: 0x0012BD23
		internal CorePropertiesFilter(PackageProperties coreProperties)
		{
			if (coreProperties == null)
			{
				throw new ArgumentNullException("coreProperties");
			}
			this._coreProperties = coreProperties;
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x0012CD40 File Offset: 0x0012BD40
		public IFILTER_FLAGS Init(IFILTER_INIT grfFlags, ManagedFullPropSpec[] aAttributes)
		{
			this._grfFlags = grfFlags;
			this._aAttributes = aAttributes;
			this._corePropertyEnumerator = new CorePropertyEnumerator(this._coreProperties, this._grfFlags, this._aAttributes);
			return IFILTER_FLAGS.IFILTER_FLAGS_NONE;
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x0012CD6E File Offset: 0x0012BD6E
		public ManagedChunk GetChunk()
		{
			this._pendingGetValue = false;
			if (!this.CorePropertyEnumerator.MoveNext())
			{
				return null;
			}
			ManagedChunk result = new CorePropertiesFilter.PropertyChunk(this.AllocateChunkID(), this.CorePropertyEnumerator.CurrentGuid, this.CorePropertyEnumerator.CurrentPropId);
			this._pendingGetValue = true;
			return result;
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x0012CDAE File Offset: 0x0012BDAE
		public string GetText(int bufferCharacterCount)
		{
			throw new COMException(SR.Get("FilterGetTextNotSupported"), -2147215611);
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x0012CDC4 File Offset: 0x0012BDC4
		public object GetValue()
		{
			if (!this._pendingGetValue)
			{
				throw new COMException(SR.Get("FilterGetValueAlreadyCalledOnCurrentChunk"), -2147215614);
			}
			this._pendingGetValue = false;
			return this.CorePropertyEnumerator.CurrentValue;
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x0012CDF5 File Offset: 0x0012BDF5
		private uint AllocateChunkID()
		{
			if (this._chunkID == 4294967295U)
			{
				this._chunkID = 1U;
			}
			else
			{
				this._chunkID += 1U;
			}
			return this._chunkID;
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x0012CE1D File Offset: 0x0012BE1D
		private CorePropertyEnumerator CorePropertyEnumerator
		{
			get
			{
				if (this._corePropertyEnumerator == null)
				{
					this._corePropertyEnumerator = new CorePropertyEnumerator(this._coreProperties, this._grfFlags, this._aAttributes);
				}
				return this._corePropertyEnumerator;
			}
		}

		// Token: 0x040008D6 RID: 2262
		private IFILTER_INIT _grfFlags;

		// Token: 0x040008D7 RID: 2263
		private ManagedFullPropSpec[] _aAttributes;

		// Token: 0x040008D8 RID: 2264
		private uint _chunkID;

		// Token: 0x040008D9 RID: 2265
		private bool _pendingGetValue;

		// Token: 0x040008DA RID: 2266
		private CorePropertyEnumerator _corePropertyEnumerator;

		// Token: 0x040008DB RID: 2267
		private PackageProperties _coreProperties;

		// Token: 0x020009BF RID: 2495
		private class PropertyChunk : ManagedChunk
		{
			// Token: 0x060083CF RID: 33743 RVA: 0x003244BA File Offset: 0x003234BA
			internal PropertyChunk(uint chunkId, Guid guid, uint propId) : base(chunkId, CHUNK_BREAKTYPE.CHUNK_EOS, new ManagedFullPropSpec(guid, propId), (uint)CultureInfo.InvariantCulture.LCID, CHUNKSTATE.CHUNK_VALUE)
			{
			}
		}
	}
}
