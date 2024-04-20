using System;
using System.Reflection;

namespace System.Windows.Markup
{
	// Token: 0x02000480 RID: 1152
	internal struct WpfPropertyDefinition
	{
		// Token: 0x06003BF5 RID: 15349 RVA: 0x001FAC70 File Offset: 0x001F9C70
		public WpfPropertyDefinition(BamlRecordReader reader, short attributeId, bool targetIsDependencyObject)
		{
			this._reader = reader;
			this._attributeId = attributeId;
			this._dependencyProperty = null;
			this._attributeInfo = null;
			if (this._reader.MapTable != null && targetIsDependencyObject)
			{
				this._dependencyProperty = this._reader.MapTable.GetDependencyProperty((int)this._attributeId);
			}
		}

		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x06003BF6 RID: 15350 RVA: 0x001FACC7 File Offset: 0x001F9CC7
		public DependencyProperty DependencyProperty
		{
			get
			{
				return this._dependencyProperty;
			}
		}

		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x06003BF7 RID: 15351 RVA: 0x001FACD0 File Offset: 0x001F9CD0
		public BamlAttributeUsage AttributeUsage
		{
			get
			{
				if (this._attributeInfo != null)
				{
					return this._attributeInfo.AttributeUsage;
				}
				if (this._reader.MapTable != null)
				{
					short num;
					string text;
					BamlAttributeUsage result;
					this._reader.MapTable.GetAttributeInfoFromId(this._attributeId, out num, out text, out result);
					return result;
				}
				return BamlAttributeUsage.Default;
			}
		}

		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x06003BF8 RID: 15352 RVA: 0x001FAD1D File Offset: 0x001F9D1D
		public BamlAttributeInfoRecord AttributeInfo
		{
			get
			{
				if (this._attributeInfo == null && this._reader.MapTable != null)
				{
					this._attributeInfo = this._reader.MapTable.GetAttributeInfoFromIdWithOwnerType(this._attributeId);
				}
				return this._attributeInfo;
			}
		}

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x06003BF9 RID: 15353 RVA: 0x001FAD58 File Offset: 0x001F9D58
		public PropertyInfo PropertyInfo
		{
			get
			{
				if (this.AttributeInfo == null)
				{
					return null;
				}
				if (this._attributeInfo.PropInfo == null)
				{
					Type type = this._reader.GetCurrentObjectData().GetType();
					this._reader.XamlTypeMapper.UpdateClrPropertyInfo(type, this._attributeInfo);
				}
				return this._attributeInfo.PropInfo;
			}
		}

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x06003BFA RID: 15354 RVA: 0x001FADB5 File Offset: 0x001F9DB5
		public MethodInfo AttachedPropertyGetter
		{
			get
			{
				if (this.AttributeInfo == null)
				{
					return null;
				}
				if (this._attributeInfo.AttachedPropertyGetter == null)
				{
					this._reader.XamlTypeMapper.UpdateAttachedPropertyGetter(this._attributeInfo);
				}
				return this._attributeInfo.AttachedPropertyGetter;
			}
		}

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x06003BFB RID: 15355 RVA: 0x001FADF5 File Offset: 0x001F9DF5
		public MethodInfo AttachedPropertySetter
		{
			get
			{
				if (this.AttributeInfo == null)
				{
					return null;
				}
				if (this._attributeInfo.AttachedPropertySetter == null)
				{
					this._reader.XamlTypeMapper.UpdateAttachedPropertySetter(this._attributeInfo);
				}
				return this._attributeInfo.AttachedPropertySetter;
			}
		}

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06003BFC RID: 15356 RVA: 0x001FAE35 File Offset: 0x001F9E35
		public bool IsInternal
		{
			get
			{
				return this.AttributeInfo != null && this._attributeInfo.IsInternal;
			}
		}

		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x06003BFD RID: 15357 RVA: 0x001FAE4C File Offset: 0x001F9E4C
		public Type PropertyType
		{
			get
			{
				if (this.DependencyProperty != null)
				{
					return this.DependencyProperty.PropertyType;
				}
				if (this.PropertyInfo != null)
				{
					return this.PropertyInfo.PropertyType;
				}
				if (this.AttachedPropertySetter != null)
				{
					return XamlTypeMapper.GetPropertyType(this.AttachedPropertySetter);
				}
				return this.AttachedPropertyGetter.ReturnType;
			}
		}

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x06003BFE RID: 15358 RVA: 0x001FAEAC File Offset: 0x001F9EAC
		public string Name
		{
			get
			{
				if (this.DependencyProperty != null)
				{
					return this.DependencyProperty.Name;
				}
				if (this.PropertyInfo != null)
				{
					return this.PropertyInfo.Name;
				}
				if (this.AttachedPropertySetter != null)
				{
					return this.AttachedPropertySetter.Name.Substring("Set".Length);
				}
				if (this.AttachedPropertyGetter != null)
				{
					return this.AttachedPropertyGetter.Name.Substring("Get".Length);
				}
				if (this._attributeInfo != null)
				{
					return this._attributeInfo.Name;
				}
				return "<unknown>";
			}
		}

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06003BFF RID: 15359 RVA: 0x001FAF52 File Offset: 0x001F9F52
		internal object DpOrPiOrMi
		{
			get
			{
				if (this.DependencyProperty != null)
				{
					return this.DependencyProperty;
				}
				if (!(this.PropertyInfo != null))
				{
					return this.AttachedPropertySetter;
				}
				return this.PropertyInfo;
			}
		}

		// Token: 0x04001E46 RID: 7750
		private BamlRecordReader _reader;

		// Token: 0x04001E47 RID: 7751
		private short _attributeId;

		// Token: 0x04001E48 RID: 7752
		private BamlAttributeInfoRecord _attributeInfo;

		// Token: 0x04001E49 RID: 7753
		private DependencyProperty _dependencyProperty;
	}
}
