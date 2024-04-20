using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x0200052A RID: 1322
	internal class ElementStringValueProperty : MarkupProperty
	{
		// Token: 0x060041B3 RID: 16819 RVA: 0x002193F0 File Offset: 0x002183F0
		internal ElementStringValueProperty(ElementMarkupObject obj)
		{
			this._object = obj;
		}

		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x060041B4 RID: 16820 RVA: 0x002193FF File Offset: 0x002183FF
		public override string Name
		{
			get
			{
				return "StringValue";
			}
		}

		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x060041B5 RID: 16821 RVA: 0x00219406 File Offset: 0x00218406
		public override Type PropertyType
		{
			get
			{
				return typeof(string);
			}
		}

		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x060041B6 RID: 16822 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool IsValueAsString
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x060041B7 RID: 16823 RVA: 0x00219412 File Offset: 0x00218412
		public override object Value
		{
			get
			{
				return this.StringValue;
			}
		}

		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x060041B8 RID: 16824 RVA: 0x0021941A File Offset: 0x0021841A
		public override string StringValue
		{
			get
			{
				return ValueSerializer.GetSerializerFor(this._object.ObjectType, this._object.Context).ConvertToString(this._object.Instance, this._object.Context);
			}
		}

		// Token: 0x17000EA3 RID: 3747
		// (get) Token: 0x060041B9 RID: 16825 RVA: 0x00109403 File Offset: 0x00108403
		public override IEnumerable<MarkupObject> Items
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x060041BA RID: 16826 RVA: 0x00219452 File Offset: 0x00218452
		public override AttributeCollection Attributes
		{
			get
			{
				return AttributeCollection.Empty;
			}
		}

		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x060041BB RID: 16827 RVA: 0x00219459 File Offset: 0x00218459
		public override IEnumerable<Type> TypeReferences
		{
			get
			{
				return ValueSerializer.GetSerializerFor(this._object.ObjectType, this._object.Context).TypeReferences(this._object.Instance, this._object.Context);
			}
		}

		// Token: 0x040024D8 RID: 9432
		private ElementMarkupObject _object;
	}
}
