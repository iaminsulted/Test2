﻿using System;

namespace Standard
{
	// Token: 0x02000070 RID: 112
	internal enum GPS
	{
		// Token: 0x0400050D RID: 1293
		DEFAULT,
		// Token: 0x0400050E RID: 1294
		HANDLERPROPERTIESONLY,
		// Token: 0x0400050F RID: 1295
		READWRITE,
		// Token: 0x04000510 RID: 1296
		TEMPORARY = 4,
		// Token: 0x04000511 RID: 1297
		FASTPROPERTIESONLY = 8,
		// Token: 0x04000512 RID: 1298
		OPENSLOWITEM = 16,
		// Token: 0x04000513 RID: 1299
		DELAYCREATION = 32,
		// Token: 0x04000514 RID: 1300
		BESTEFFORT = 64,
		// Token: 0x04000515 RID: 1301
		NO_OPLOCK = 128,
		// Token: 0x04000516 RID: 1302
		MASK_VALID = 255
	}
}
