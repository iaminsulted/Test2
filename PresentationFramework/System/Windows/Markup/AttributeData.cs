using System;

namespace System.Windows.Markup
{
	// Token: 0x02000472 RID: 1138
	internal class AttributeData : DefAttributeData
	{
		// Token: 0x06003A7F RID: 14975 RVA: 0x001F0D50 File Offset: 0x001EFD50
		internal AttributeData(string targetAssemblyName, string targetFullName, Type targetType, string args, Type declaringType, string propertyName, object info, Type serializerType, int lineNumber, int linePosition, int depth, string targetNamespaceUri, short extensionTypeId, bool isValueNestedExtension, bool isValueTypeExtension, bool isSimple) : base(targetAssemblyName, targetFullName, targetType, args, declaringType, targetNamespaceUri, lineNumber, linePosition, depth, isSimple)
		{
			this.PropertyName = propertyName;
			this.SerializerType = serializerType;
			this.ExtensionTypeId = extensionTypeId;
			this.IsValueNestedExtension = isValueNestedExtension;
			this.IsValueTypeExtension = isValueTypeExtension;
			this.Info = info;
		}

		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x06003A80 RID: 14976 RVA: 0x001F0DA4 File Offset: 0x001EFDA4
		internal bool IsTypeExtension
		{
			get
			{
				return this.ExtensionTypeId == 691;
			}
		}

		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x06003A81 RID: 14977 RVA: 0x001F0DB3 File Offset: 0x001EFDB3
		internal bool IsStaticExtension
		{
			get
			{
				return this.ExtensionTypeId == 602;
			}
		}

		// Token: 0x04001DB3 RID: 7603
		internal string PropertyName;

		// Token: 0x04001DB4 RID: 7604
		internal Type SerializerType;

		// Token: 0x04001DB5 RID: 7605
		internal short ExtensionTypeId;

		// Token: 0x04001DB6 RID: 7606
		internal bool IsValueNestedExtension;

		// Token: 0x04001DB7 RID: 7607
		internal bool IsValueTypeExtension;

		// Token: 0x04001DB8 RID: 7608
		internal object Info;
	}
}
