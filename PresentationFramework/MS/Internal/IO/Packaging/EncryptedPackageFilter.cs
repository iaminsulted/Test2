using System;
using System.IO.Packaging;
using System.Runtime.InteropServices;
using System.Windows;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200015F RID: 351
	internal class EncryptedPackageFilter : IFilter
	{
		// Token: 0x06000BAA RID: 2986 RVA: 0x0012D228 File Offset: 0x0012C228
		internal EncryptedPackageFilter(EncryptedPackageEnvelope encryptedPackage)
		{
			if (encryptedPackage == null)
			{
				throw new ArgumentNullException("encryptedPackage");
			}
			this._filter = new IndexingFilterMarshaler(new CorePropertiesFilter(encryptedPackage.PackageProperties));
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x0012D254 File Offset: 0x0012C254
		public IFILTER_FLAGS Init([In] IFILTER_INIT grfFlags, [In] uint cAttributes, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] FULLPROPSPEC[] aAttributes)
		{
			return this._filter.Init(grfFlags, cAttributes, aAttributes);
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x0012D264 File Offset: 0x0012C264
		public STAT_CHUNK GetChunk()
		{
			return this._filter.GetChunk();
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x0012CDAE File Offset: 0x0012BDAE
		public void GetText(ref uint bufCharacterCount, IntPtr pBuffer)
		{
			throw new COMException(SR.Get("FilterGetTextNotSupported"), -2147215611);
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x0012D271 File Offset: 0x0012C271
		public IntPtr GetValue()
		{
			return this._filter.GetValue();
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x0012D27E File Offset: 0x0012C27E
		public IntPtr BindRegion([In] FILTERREGION origPos, [In] ref Guid riid)
		{
			throw new NotImplementedException(SR.Get("FilterBindRegionNotImplemented"));
		}

		// Token: 0x040008DF RID: 2271
		private IFilter _filter;
	}
}
