using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000489 RID: 1161
	internal interface IBamlDictionaryKey
	{
		// Token: 0x06003C47 RID: 15431
		void UpdateValuePosition(int newPosition, BinaryWriter bamlBinaryWriter);

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x06003C48 RID: 15432
		// (set) Token: 0x06003C49 RID: 15433
		int ValuePosition { get; set; }

		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x06003C4A RID: 15434
		// (set) Token: 0x06003C4B RID: 15435
		object KeyObject { get; set; }

		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x06003C4C RID: 15436
		// (set) Token: 0x06003C4D RID: 15437
		long ValuePositionPosition { get; set; }

		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x06003C4E RID: 15438
		// (set) Token: 0x06003C4F RID: 15439
		bool Shared { get; set; }

		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x06003C50 RID: 15440
		// (set) Token: 0x06003C51 RID: 15441
		bool SharedSet { get; set; }

		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x06003C52 RID: 15442
		// (set) Token: 0x06003C53 RID: 15443
		object[] StaticResourceValues { get; set; }
	}
}
