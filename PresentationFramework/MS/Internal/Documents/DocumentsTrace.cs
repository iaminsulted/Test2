using System;
using System.Diagnostics;

namespace MS.Internal.Documents
{
	// Token: 0x020001BF RID: 447
	internal sealed class DocumentsTrace
	{
		// Token: 0x06000F36 RID: 3894 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		public DocumentsTrace(string switchName)
		{
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x0013CDEF File Offset: 0x0013BDEF
		public DocumentsTrace(string switchName, bool initialState) : this(switchName)
		{
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		[Conditional("DEBUG")]
		public void Trace(string message)
		{
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		[Conditional("DEBUG")]
		public void TraceCallers(int Depth)
		{
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		[Conditional("DEBUG")]
		public void Indent()
		{
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		[Conditional("DEBUG")]
		public void Unindent()
		{
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		[Conditional("DEBUG")]
		public void Enable()
		{
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		[Conditional("DEBUG")]
		public void Disable()
		{
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000F3E RID: 3902 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsEnabled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x020009D8 RID: 2520
		internal static class FixedFormat
		{
			// Token: 0x04003FCF RID: 16335
			public static DocumentsTrace FixedDocument;

			// Token: 0x04003FD0 RID: 16336
			public static DocumentsTrace PageContent;

			// Token: 0x04003FD1 RID: 16337
			public static DocumentsTrace IDF;
		}

		// Token: 0x020009D9 RID: 2521
		internal static class FixedTextOM
		{
			// Token: 0x04003FD2 RID: 16338
			public static DocumentsTrace TextView;

			// Token: 0x04003FD3 RID: 16339
			public static DocumentsTrace TextContainer;

			// Token: 0x04003FD4 RID: 16340
			public static DocumentsTrace Map;

			// Token: 0x04003FD5 RID: 16341
			public static DocumentsTrace Highlight;

			// Token: 0x04003FD6 RID: 16342
			public static DocumentsTrace Builder;

			// Token: 0x04003FD7 RID: 16343
			public static DocumentsTrace FlowPosition;
		}

		// Token: 0x020009DA RID: 2522
		internal static class FixedDocumentSequence
		{
			// Token: 0x04003FD8 RID: 16344
			public static DocumentsTrace Content;

			// Token: 0x04003FD9 RID: 16345
			public static DocumentsTrace IDF;

			// Token: 0x04003FDA RID: 16346
			public static DocumentsTrace TextOM;

			// Token: 0x04003FDB RID: 16347
			public static DocumentsTrace Highlights;
		}
	}
}
