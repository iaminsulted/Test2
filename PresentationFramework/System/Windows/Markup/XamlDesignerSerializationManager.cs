using System;
using System.ComponentModel;
using System.Xml;

namespace System.Windows.Markup
{
	// Token: 0x020004DC RID: 1244
	public class XamlDesignerSerializationManager : ServiceProviders
	{
		// Token: 0x06003F74 RID: 16244 RVA: 0x0021190F File Offset: 0x0021090F
		public XamlDesignerSerializationManager(XmlWriter xmlWriter)
		{
			this._xamlWriterMode = XamlWriterMode.Value;
			this._xmlWriter = xmlWriter;
		}

		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x06003F75 RID: 16245 RVA: 0x00211925 File Offset: 0x00210925
		// (set) Token: 0x06003F76 RID: 16246 RVA: 0x0021192D File Offset: 0x0021092D
		public XamlWriterMode XamlWriterMode
		{
			get
			{
				return this._xamlWriterMode;
			}
			set
			{
				if (!XamlDesignerSerializationManager.IsValidXamlWriterMode(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(XamlWriterMode));
				}
				this._xamlWriterMode = value;
			}
		}

		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x06003F77 RID: 16247 RVA: 0x00211954 File Offset: 0x00210954
		internal XmlWriter XmlWriter
		{
			get
			{
				return this._xmlWriter;
			}
		}

		// Token: 0x06003F78 RID: 16248 RVA: 0x0021195C File Offset: 0x0021095C
		internal void ClearXmlWriter()
		{
			this._xmlWriter = null;
		}

		// Token: 0x06003F79 RID: 16249 RVA: 0x00211965 File Offset: 0x00210965
		private static bool IsValidXamlWriterMode(XamlWriterMode value)
		{
			return value == XamlWriterMode.Value || value == XamlWriterMode.Expression;
		}

		// Token: 0x0400237A RID: 9082
		private XamlWriterMode _xamlWriterMode;

		// Token: 0x0400237B RID: 9083
		private XmlWriter _xmlWriter;
	}
}
