using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x0200052B RID: 1323
	internal abstract class ElementPseudoPropertyBase : ElementObjectPropertyBase
	{
		// Token: 0x060041BC RID: 16828 RVA: 0x00219491 File Offset: 0x00218491
		internal ElementPseudoPropertyBase(object value, Type type, ElementMarkupObject obj) : base(obj)
		{
			this._value = value;
			this._type = type;
		}

		// Token: 0x17000EA6 RID: 3750
		// (get) Token: 0x060041BD RID: 16829 RVA: 0x002194A8 File Offset: 0x002184A8
		public override Type PropertyType
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x060041BE RID: 16830 RVA: 0x002194B0 File Offset: 0x002184B0
		public override object Value
		{
			get
			{
				return ElementProperty.CheckForMarkupExtension(this.PropertyType, this._value, base.Context, true);
			}
		}

		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x060041BF RID: 16831 RVA: 0x00219452 File Offset: 0x00218452
		public override AttributeCollection Attributes
		{
			get
			{
				return AttributeCollection.Empty;
			}
		}

		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x060041C0 RID: 16832 RVA: 0x002194CA File Offset: 0x002184CA
		public override IEnumerable<Type> TypeReferences
		{
			get
			{
				return Array.Empty<Type>();
			}
		}

		// Token: 0x040024D9 RID: 9433
		private object _value;

		// Token: 0x040024DA RID: 9434
		private Type _type;
	}
}
