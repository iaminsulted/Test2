using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000537 RID: 1335
	internal class FrameworkElementFactoryContent : ElementPropertyBase
	{
		// Token: 0x0600420E RID: 16910 RVA: 0x00219D26 File Offset: 0x00218D26
		internal FrameworkElementFactoryContent(FrameworkElementFactory factory, FrameworkElementFactoryMarkupObject item) : base(item.Manager)
		{
			this._item = item;
			this._factory = factory;
		}

		// Token: 0x17000EDA RID: 3802
		// (get) Token: 0x0600420F RID: 16911 RVA: 0x00219D42 File Offset: 0x00218D42
		public override string Name
		{
			get
			{
				return "Content";
			}
		}

		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x06004210 RID: 16912 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool IsContent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000EDC RID: 3804
		// (get) Token: 0x06004211 RID: 16913 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool IsComposite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000EDD RID: 3805
		// (get) Token: 0x06004212 RID: 16914 RVA: 0x00219D49 File Offset: 0x00218D49
		public override IEnumerable<MarkupObject> Items
		{
			get
			{
				for (FrameworkElementFactory child = this._factory.FirstChild; child != null; child = child.NextSibling)
				{
					yield return new FrameworkElementFactoryMarkupObject(child, base.Manager);
				}
				yield break;
			}
		}

		// Token: 0x06004213 RID: 16915 RVA: 0x00219D59 File Offset: 0x00218D59
		protected override IValueSerializerContext GetItemContext()
		{
			return this._item.Context;
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x00219D66 File Offset: 0x00218D66
		protected override Type GetObjectType()
		{
			return this._item.ObjectType;
		}

		// Token: 0x17000EDE RID: 3806
		// (get) Token: 0x06004215 RID: 16917 RVA: 0x00219D73 File Offset: 0x00218D73
		public override AttributeCollection Attributes
		{
			get
			{
				return new AttributeCollection(Array.Empty<Attribute>());
			}
		}

		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x06004216 RID: 16918 RVA: 0x00219D7F File Offset: 0x00218D7F
		public override Type PropertyType
		{
			get
			{
				return typeof(IEnumerable);
			}
		}

		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x06004217 RID: 16919 RVA: 0x00219D8B File Offset: 0x00218D8B
		public override object Value
		{
			get
			{
				return this._factory;
			}
		}

		// Token: 0x040024EA RID: 9450
		private FrameworkElementFactoryMarkupObject _item;

		// Token: 0x040024EB RID: 9451
		private FrameworkElementFactory _factory;
	}
}
