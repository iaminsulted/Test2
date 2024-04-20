using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000532 RID: 1330
	internal class MarkupPropertyWrapper : MarkupProperty
	{
		// Token: 0x060041E0 RID: 16864 RVA: 0x00219656 File Offset: 0x00218656
		public MarkupPropertyWrapper(MarkupProperty baseProperty)
		{
			this._baseProperty = baseProperty;
		}

		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x060041E1 RID: 16865 RVA: 0x00219665 File Offset: 0x00218665
		public override AttributeCollection Attributes
		{
			get
			{
				return this._baseProperty.Attributes;
			}
		}

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x060041E2 RID: 16866 RVA: 0x00219672 File Offset: 0x00218672
		public override IEnumerable<MarkupObject> Items
		{
			get
			{
				return this._baseProperty.Items;
			}
		}

		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x060041E3 RID: 16867 RVA: 0x0021967F File Offset: 0x0021867F
		public override string Name
		{
			get
			{
				return this._baseProperty.Name;
			}
		}

		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x060041E4 RID: 16868 RVA: 0x0021968C File Offset: 0x0021868C
		public override Type PropertyType
		{
			get
			{
				return this._baseProperty.PropertyType;
			}
		}

		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x060041E5 RID: 16869 RVA: 0x00219699 File Offset: 0x00218699
		public override string StringValue
		{
			get
			{
				return this._baseProperty.StringValue;
			}
		}

		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x060041E6 RID: 16870 RVA: 0x002196A6 File Offset: 0x002186A6
		public override IEnumerable<Type> TypeReferences
		{
			get
			{
				return this._baseProperty.TypeReferences;
			}
		}

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x060041E7 RID: 16871 RVA: 0x002196B3 File Offset: 0x002186B3
		public override object Value
		{
			get
			{
				return this._baseProperty.Value;
			}
		}

		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x060041E8 RID: 16872 RVA: 0x002196C0 File Offset: 0x002186C0
		public override DependencyProperty DependencyProperty
		{
			get
			{
				return this._baseProperty.DependencyProperty;
			}
		}

		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x060041E9 RID: 16873 RVA: 0x002196CD File Offset: 0x002186CD
		public override bool IsAttached
		{
			get
			{
				return this._baseProperty.IsAttached;
			}
		}

		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x060041EA RID: 16874 RVA: 0x002196DA File Offset: 0x002186DA
		public override bool IsComposite
		{
			get
			{
				return this._baseProperty.IsComposite;
			}
		}

		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x060041EB RID: 16875 RVA: 0x002196E7 File Offset: 0x002186E7
		public override bool IsConstructorArgument
		{
			get
			{
				return this._baseProperty.IsConstructorArgument;
			}
		}

		// Token: 0x17000EC7 RID: 3783
		// (get) Token: 0x060041EC RID: 16876 RVA: 0x002196F4 File Offset: 0x002186F4
		public override bool IsKey
		{
			get
			{
				return this._baseProperty.IsKey;
			}
		}

		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x060041ED RID: 16877 RVA: 0x00219701 File Offset: 0x00218701
		public override bool IsValueAsString
		{
			get
			{
				return this._baseProperty.IsValueAsString;
			}
		}

		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x060041EE RID: 16878 RVA: 0x0021970E File Offset: 0x0021870E
		public override bool IsContent
		{
			get
			{
				return this._baseProperty.IsContent;
			}
		}

		// Token: 0x17000ECA RID: 3786
		// (get) Token: 0x060041EF RID: 16879 RVA: 0x0021971B File Offset: 0x0021871B
		public override PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return this._baseProperty.PropertyDescriptor;
			}
		}

		// Token: 0x060041F0 RID: 16880 RVA: 0x00219728 File Offset: 0x00218728
		internal override void VerifyOnlySerializableTypes()
		{
			this._baseProperty.VerifyOnlySerializableTypes();
		}

		// Token: 0x040024DF RID: 9439
		private MarkupProperty _baseProperty;
	}
}
