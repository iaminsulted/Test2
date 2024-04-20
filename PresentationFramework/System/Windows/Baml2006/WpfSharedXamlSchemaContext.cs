using System;
using System.Collections.Generic;
using System.Xaml;

namespace System.Windows.Baml2006
{
	// Token: 0x0200041B RID: 1051
	internal class WpfSharedXamlSchemaContext : WpfSharedBamlSchemaContext
	{
		// Token: 0x06003264 RID: 12900 RVA: 0x001D199D File Offset: 0x001D099D
		public WpfSharedXamlSchemaContext(XamlSchemaContextSettings settings, bool useV3Rules) : base(settings)
		{
			this._useV3Rules = useV3Rules;
		}

		// Token: 0x06003265 RID: 12901 RVA: 0x001D19C4 File Offset: 0x001D09C4
		public override XamlType GetXamlType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			object syncObject = this._syncObject;
			XamlType xamlType;
			lock (syncObject)
			{
				if (!this._masterTypeTable.TryGetValue(type, out xamlType))
				{
					WpfSharedXamlSchemaContext.RequireRuntimeType(type);
					xamlType = base.CreateKnownBamlType(type.Name, false, this._useV3Rules);
					if (xamlType == null || xamlType.UnderlyingType != type)
					{
						xamlType = new WpfXamlType(type, this, false, this._useV3Rules);
					}
					this._masterTypeTable.Add(type, xamlType);
				}
			}
			return xamlType;
		}

		// Token: 0x06003266 RID: 12902 RVA: 0x001D1A70 File Offset: 0x001D0A70
		internal static void RequireRuntimeType(Type type)
		{
			if (!typeof(object).GetType().IsAssignableFrom(type.GetType()))
			{
				throw new ArgumentException(SR.Get("RuntimeTypeRequired", new object[]
				{
					type
				}), "type");
			}
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x001D1AAD File Offset: 0x001D0AAD
		internal XamlType GetXamlTypeInternal(string xamlNamespace, string name, params XamlType[] typeArguments)
		{
			return base.GetXamlType(xamlNamespace, name, typeArguments);
		}

		// Token: 0x04001BED RID: 7149
		private Dictionary<Type, XamlType> _masterTypeTable = new Dictionary<Type, XamlType>();

		// Token: 0x04001BEE RID: 7150
		private object _syncObject = new object();

		// Token: 0x04001BEF RID: 7151
		private bool _useV3Rules;
	}
}
