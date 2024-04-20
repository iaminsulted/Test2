using System;
using System.Xaml;

namespace System.Windows.Markup
{
	// Token: 0x0200050F RID: 1295
	internal class XamlReaderHelper
	{
		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x06004071 RID: 16497 RVA: 0x002140AF File Offset: 0x002130AF
		internal static XamlDirective Freeze
		{
			get
			{
				if (XamlReaderHelper._freezeDirective == null)
				{
					XamlReaderHelper._freezeDirective = new XamlDirective("http://schemas.microsoft.com/winfx/2006/xaml/presentation/options", "Freeze");
				}
				return XamlReaderHelper._freezeDirective;
			}
		}

		// Token: 0x04002416 RID: 9238
		internal const string DefinitionNamespaceURI = "http://schemas.microsoft.com/winfx/2006/xaml";

		// Token: 0x04002417 RID: 9239
		internal const string DefinitionUid = "Uid";

		// Token: 0x04002418 RID: 9240
		internal const string DefinitionType = "Type";

		// Token: 0x04002419 RID: 9241
		internal const string DefinitionTypeArgs = "TypeArguments";

		// Token: 0x0400241A RID: 9242
		internal const string DefinitionName = "Key";

		// Token: 0x0400241B RID: 9243
		internal const string DefinitionRuntimeName = "Name";

		// Token: 0x0400241C RID: 9244
		internal const string DefinitionShared = "Shared";

		// Token: 0x0400241D RID: 9245
		internal const string DefinitionSynchronousMode = "SynchronousMode";

		// Token: 0x0400241E RID: 9246
		internal const string DefinitionAsyncRecords = "AsyncRecords";

		// Token: 0x0400241F RID: 9247
		internal const string DefinitionContent = "Content";

		// Token: 0x04002420 RID: 9248
		internal const string DefinitionClass = "Class";

		// Token: 0x04002421 RID: 9249
		internal const string DefinitionSubclass = "Subclass";

		// Token: 0x04002422 RID: 9250
		internal const string DefinitionClassModifier = "ClassModifier";

		// Token: 0x04002423 RID: 9251
		internal const string DefinitionFieldModifier = "FieldModifier";

		// Token: 0x04002424 RID: 9252
		internal const string DefinitionCodeTag = "Code";

		// Token: 0x04002425 RID: 9253
		internal const string DefinitionXDataTag = "XData";

		// Token: 0x04002426 RID: 9254
		internal const string MappingProtocol = "clr-namespace:";

		// Token: 0x04002427 RID: 9255
		internal const string MappingAssembly = ";assembly=";

		// Token: 0x04002428 RID: 9256
		internal const string PresentationOptionsFreeze = "Freeze";

		// Token: 0x04002429 RID: 9257
		internal const string DefaultNamespaceURI = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

		// Token: 0x0400242A RID: 9258
		internal const string DefinitionMetroNamespaceURI = "http://schemas.microsoft.com/xps/2005/06/resourcedictionary-key";

		// Token: 0x0400242B RID: 9259
		internal const string PresentationOptionsNamespaceURI = "http://schemas.microsoft.com/winfx/2006/xaml/presentation/options";

		// Token: 0x0400242C RID: 9260
		private static XamlDirective _freezeDirective;
	}
}
