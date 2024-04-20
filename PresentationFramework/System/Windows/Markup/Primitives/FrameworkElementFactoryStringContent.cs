using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000538 RID: 1336
	internal class FrameworkElementFactoryStringContent : ElementPropertyBase
	{
		// Token: 0x06004218 RID: 16920 RVA: 0x00219D93 File Offset: 0x00218D93
		internal FrameworkElementFactoryStringContent(FrameworkElementFactory factory, FrameworkElementFactoryMarkupObject item) : base(item.Manager)
		{
			this._item = item;
			this._factory = factory;
		}

		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x06004219 RID: 16921 RVA: 0x00219D42 File Offset: 0x00218D42
		public override string Name
		{
			get
			{
				return "Content";
			}
		}

		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x0600421A RID: 16922 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool IsContent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x0600421B RID: 16923 RVA: 0x00105F35 File Offset: 0x00104F35
		public override bool IsComposite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000EE4 RID: 3812
		// (get) Token: 0x0600421C RID: 16924 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool IsValueAsString
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x0600421D RID: 16925 RVA: 0x00219DAF File Offset: 0x00218DAF
		public override IEnumerable<MarkupObject> Items
		{
			get
			{
				return Array.Empty<MarkupObject>();
			}
		}

		// Token: 0x0600421E RID: 16926 RVA: 0x00219DB6 File Offset: 0x00218DB6
		protected override IValueSerializerContext GetItemContext()
		{
			return this._item.Context;
		}

		// Token: 0x0600421F RID: 16927 RVA: 0x00219DC3 File Offset: 0x00218DC3
		protected override Type GetObjectType()
		{
			return this._item.ObjectType;
		}

		// Token: 0x17000EE6 RID: 3814
		// (get) Token: 0x06004220 RID: 16928 RVA: 0x00219D73 File Offset: 0x00218D73
		public override AttributeCollection Attributes
		{
			get
			{
				return new AttributeCollection(Array.Empty<Attribute>());
			}
		}

		// Token: 0x17000EE7 RID: 3815
		// (get) Token: 0x06004221 RID: 16929 RVA: 0x00219406 File Offset: 0x00218406
		public override Type PropertyType
		{
			get
			{
				return typeof(string);
			}
		}

		// Token: 0x17000EE8 RID: 3816
		// (get) Token: 0x06004222 RID: 16930 RVA: 0x00219DD0 File Offset: 0x00218DD0
		public override object Value
		{
			get
			{
				return this._factory.Text;
			}
		}

		// Token: 0x040024EC RID: 9452
		private FrameworkElementFactoryMarkupObject _item;

		// Token: 0x040024ED RID: 9453
		private FrameworkElementFactory _factory;
	}
}
