using System;
using System.IO;

namespace System.Windows.Documents.Serialization
{
	// Token: 0x020006E9 RID: 1769
	public interface ISerializerFactory
	{
		// Token: 0x06005CF5 RID: 23797
		SerializerWriter CreateSerializerWriter(Stream stream);

		// Token: 0x17001598 RID: 5528
		// (get) Token: 0x06005CF6 RID: 23798
		string DisplayName { get; }

		// Token: 0x17001599 RID: 5529
		// (get) Token: 0x06005CF7 RID: 23799
		string ManufacturerName { get; }

		// Token: 0x1700159A RID: 5530
		// (get) Token: 0x06005CF8 RID: 23800
		Uri ManufacturerWebsite { get; }

		// Token: 0x1700159B RID: 5531
		// (get) Token: 0x06005CF9 RID: 23801
		string DefaultFileExtension { get; }
	}
}
