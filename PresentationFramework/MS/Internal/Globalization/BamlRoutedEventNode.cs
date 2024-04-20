using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020001AA RID: 426
	internal sealed class BamlRoutedEventNode : BamlTreeNode
	{
		// Token: 0x06000E07 RID: 3591 RVA: 0x001371C8 File Offset: 0x001361C8
		internal BamlRoutedEventNode(string assemblyName, string ownerTypeFullName, string eventIdName, string handlerName) : base(BamlNodeType.RoutedEvent)
		{
			this._assemblyName = assemblyName;
			this._ownerTypeFullName = ownerTypeFullName;
			this._eventIdName = eventIdName;
			this._handlerName = handlerName;
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x001371EF File Offset: 0x001361EF
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteRoutedEvent(this._assemblyName, this._ownerTypeFullName, this._eventIdName, this._handlerName);
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0013720F File Offset: 0x0013620F
		internal override BamlTreeNode Copy()
		{
			return new BamlRoutedEventNode(this._assemblyName, this._ownerTypeFullName, this._eventIdName, this._handlerName);
		}

		// Token: 0x04000A21 RID: 2593
		private string _assemblyName;

		// Token: 0x04000A22 RID: 2594
		private string _ownerTypeFullName;

		// Token: 0x04000A23 RID: 2595
		private string _eventIdName;

		// Token: 0x04000A24 RID: 2596
		private string _handlerName;
	}
}
