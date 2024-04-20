using System;

namespace System.Windows.Markup
{
	// Token: 0x02000473 RID: 1139
	internal class DefAttributeData
	{
		// Token: 0x06003A82 RID: 14978 RVA: 0x001F0DC4 File Offset: 0x001EFDC4
		internal DefAttributeData(string targetAssemblyName, string targetFullName, Type targetType, string args, Type declaringType, string targetNamespaceUri, int lineNumber, int linePosition, int depth, bool isSimple)
		{
			this.TargetType = targetType;
			this.DeclaringType = declaringType;
			this.TargetFullName = targetFullName;
			this.TargetAssemblyName = targetAssemblyName;
			this.Args = args;
			this.TargetNamespaceUri = targetNamespaceUri;
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
			this.Depth = depth;
			this.IsSimple = isSimple;
		}

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x06003A83 RID: 14979 RVA: 0x001F0E24 File Offset: 0x001EFE24
		internal bool IsUnknownExtension
		{
			get
			{
				return this.TargetType == typeof(MarkupExtensionParser.UnknownMarkupExtension);
			}
		}

		// Token: 0x04001DB9 RID: 7609
		internal Type TargetType;

		// Token: 0x04001DBA RID: 7610
		internal Type DeclaringType;

		// Token: 0x04001DBB RID: 7611
		internal string TargetFullName;

		// Token: 0x04001DBC RID: 7612
		internal string TargetAssemblyName;

		// Token: 0x04001DBD RID: 7613
		internal string Args;

		// Token: 0x04001DBE RID: 7614
		internal string TargetNamespaceUri;

		// Token: 0x04001DBF RID: 7615
		internal int LineNumber;

		// Token: 0x04001DC0 RID: 7616
		internal int LinePosition;

		// Token: 0x04001DC1 RID: 7617
		internal int Depth;

		// Token: 0x04001DC2 RID: 7618
		internal bool IsSimple;
	}
}
