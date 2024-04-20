using System;

namespace System.Windows.Markup
{
	// Token: 0x020004F5 RID: 1269
	internal class XamlRoutedEventNode : XamlAttributeNode
	{
		// Token: 0x06003FDD RID: 16349 RVA: 0x0021279E File Offset: 0x0021179E
		internal XamlRoutedEventNode(int lineNumber, int linePosition, int depth, RoutedEvent routedEvent, string assemblyName, string typeFullName, string routedEventName, string value) : base(XamlNodeType.RoutedEvent, lineNumber, linePosition, depth, value)
		{
			this._routedEvent = routedEvent;
			this._assemblyName = assemblyName;
			this._typeFullName = typeFullName;
			this._routedEventName = routedEventName;
		}

		// Token: 0x17000E38 RID: 3640
		// (get) Token: 0x06003FDE RID: 16350 RVA: 0x002127CD File Offset: 0x002117CD
		internal RoutedEvent Event
		{
			get
			{
				return this._routedEvent;
			}
		}

		// Token: 0x17000E39 RID: 3641
		// (get) Token: 0x06003FDF RID: 16351 RVA: 0x002127D5 File Offset: 0x002117D5
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x17000E3A RID: 3642
		// (get) Token: 0x06003FE0 RID: 16352 RVA: 0x002127DD File Offset: 0x002117DD
		internal string TypeFullName
		{
			get
			{
				return this._typeFullName;
			}
		}

		// Token: 0x17000E3B RID: 3643
		// (get) Token: 0x06003FE1 RID: 16353 RVA: 0x002127E5 File Offset: 0x002117E5
		internal string EventName
		{
			get
			{
				return this._routedEventName;
			}
		}

		// Token: 0x040023DE RID: 9182
		private RoutedEvent _routedEvent;

		// Token: 0x040023DF RID: 9183
		private string _assemblyName;

		// Token: 0x040023E0 RID: 9184
		private string _typeFullName;

		// Token: 0x040023E1 RID: 9185
		private string _routedEventName;
	}
}
