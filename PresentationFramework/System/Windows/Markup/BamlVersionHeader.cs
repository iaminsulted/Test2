using System;
using System.Globalization;
using System.IO;
using MS.Internal.IO.Packaging.CompoundFile;

namespace System.Windows.Markup
{
	// Token: 0x020004BD RID: 1213
	internal class BamlVersionHeader
	{
		// Token: 0x06003E4D RID: 15949 RVA: 0x001FFC2C File Offset: 0x001FEC2C
		public BamlVersionHeader()
		{
			this._bamlVersion = new FormatVersion("MSBAML", BamlVersionHeader.BamlWriterVersion);
		}

		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x06003E4E RID: 15950 RVA: 0x001FFC49 File Offset: 0x001FEC49
		// (set) Token: 0x06003E4F RID: 15951 RVA: 0x001FFC51 File Offset: 0x001FEC51
		public FormatVersion BamlVersion
		{
			get
			{
				return this._bamlVersion;
			}
			set
			{
				this._bamlVersion = value;
			}
		}

		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x06003E50 RID: 15952 RVA: 0x001FD464 File Offset: 0x001FC464
		public static int BinarySerializationSize
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x06003E51 RID: 15953 RVA: 0x001FFC5C File Offset: 0x001FEC5C
		internal void LoadVersion(BinaryReader bamlBinaryReader)
		{
			this.BamlVersion = FormatVersion.LoadFromStream(bamlBinaryReader.BaseStream);
			if (this.BamlVersion.ReaderVersion != BamlVersionHeader.BamlWriterVersion)
			{
				throw new InvalidOperationException(SR.Get("ParserBamlVersion", new object[]
				{
					this.BamlVersion.ReaderVersion.Major.ToString(CultureInfo.CurrentCulture) + "." + this.BamlVersion.ReaderVersion.Minor.ToString(CultureInfo.CurrentCulture),
					BamlVersionHeader.BamlWriterVersion.Major.ToString(CultureInfo.CurrentCulture) + "." + BamlVersionHeader.BamlWriterVersion.Minor.ToString(CultureInfo.CurrentCulture)
				}));
			}
		}

		// Token: 0x06003E52 RID: 15954 RVA: 0x001FFD2C File Offset: 0x001FED2C
		internal void WriteVersion(BinaryWriter bamlBinaryWriter)
		{
			this.BamlVersion.SaveToStream(bamlBinaryWriter.BaseStream);
		}

		// Token: 0x04001F1A RID: 7962
		internal static readonly VersionPair BamlWriterVersion = new VersionPair(0, 96);

		// Token: 0x04001F1B RID: 7963
		private FormatVersion _bamlVersion;
	}
}
