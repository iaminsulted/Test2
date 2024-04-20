using System;
using System.Collections;
using System.Reflection;

namespace System.Windows.Markup
{
	// Token: 0x02000476 RID: 1142
	internal class BamlCollectionHolder
	{
		// Token: 0x06003A89 RID: 14985 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		internal BamlCollectionHolder()
		{
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x001F0E84 File Offset: 0x001EFE84
		internal BamlCollectionHolder(BamlRecordReader reader, object parent, short attributeId) : this(reader, parent, attributeId, true)
		{
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x001F0E90 File Offset: 0x001EFE90
		internal BamlCollectionHolder(BamlRecordReader reader, object parent, short attributeId, bool needDefault)
		{
			this._reader = reader;
			this._parent = parent;
			this._propDef = new WpfPropertyDefinition(reader, attributeId, parent is DependencyObject);
			this._attributeId = attributeId;
			if (needDefault)
			{
				this.InitDefaultValue();
			}
			this.CheckReadOnly();
		}

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06003A8C RID: 14988 RVA: 0x001F0EDE File Offset: 0x001EFEDE
		// (set) Token: 0x06003A8D RID: 14989 RVA: 0x001F0EE6 File Offset: 0x001EFEE6
		internal object Collection
		{
			get
			{
				return this._collection;
			}
			set
			{
				this._collection = value;
			}
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06003A8E RID: 14990 RVA: 0x001F0EEF File Offset: 0x001EFEEF
		internal IList List
		{
			get
			{
				return this._collection as IList;
			}
		}

		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x06003A8F RID: 14991 RVA: 0x001F0EFC File Offset: 0x001EFEFC
		internal IDictionary Dictionary
		{
			get
			{
				return this._collection as IDictionary;
			}
		}

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x06003A90 RID: 14992 RVA: 0x001F0F09 File Offset: 0x001EFF09
		internal ArrayExtension ArrayExt
		{
			get
			{
				return this._collection as ArrayExtension;
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x06003A91 RID: 14993 RVA: 0x001F0F16 File Offset: 0x001EFF16
		internal object DefaultCollection
		{
			get
			{
				return this._defaultCollection;
			}
		}

		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x06003A92 RID: 14994 RVA: 0x001F0F1E File Offset: 0x001EFF1E
		internal WpfPropertyDefinition PropertyDefinition
		{
			get
			{
				return this._propDef;
			}
		}

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x06003A93 RID: 14995 RVA: 0x001F0F28 File Offset: 0x001EFF28
		internal Type PropertyType
		{
			get
			{
				if (this._resourcesParent == null)
				{
					return this.PropertyDefinition.PropertyType;
				}
				return typeof(ResourceDictionary);
			}
		}

		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x06003A94 RID: 14996 RVA: 0x001F0F56 File Offset: 0x001EFF56
		internal object Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x06003A95 RID: 14997 RVA: 0x001F0F5E File Offset: 0x001EFF5E
		// (set) Token: 0x06003A96 RID: 14998 RVA: 0x001F0F66 File Offset: 0x001EFF66
		internal bool ReadOnly
		{
			get
			{
				return this._readonly;
			}
			set
			{
				this._readonly = value;
			}
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x06003A97 RID: 14999 RVA: 0x001F0F6F File Offset: 0x001EFF6F
		// (set) Token: 0x06003A98 RID: 15000 RVA: 0x001F0F77 File Offset: 0x001EFF77
		internal bool IsClosed
		{
			get
			{
				return this._isClosed;
			}
			set
			{
				this._isClosed = value;
			}
		}

		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x06003A99 RID: 15001 RVA: 0x001F0F80 File Offset: 0x001EFF80
		internal string AttributeName
		{
			get
			{
				return this._reader.GetPropertyNameFromAttributeId(this._attributeId);
			}
		}

		// Token: 0x06003A9A RID: 15002 RVA: 0x001F0F94 File Offset: 0x001EFF94
		internal void SetPropertyValue()
		{
			if (!this._isPropertyValueSet)
			{
				this._isPropertyValueSet = true;
				if (this._resourcesParent != null)
				{
					this._resourcesParent.Resources = (ResourceDictionary)this.Collection;
					return;
				}
				if (this.PropertyDefinition.DependencyProperty != null)
				{
					DependencyObject dependencyObject = this.Parent as DependencyObject;
					if (dependencyObject == null)
					{
						this._reader.ThrowException("ParserParentDO", this.Parent.ToString());
					}
					this._reader.SetDependencyValue(dependencyObject, this.PropertyDefinition.DependencyProperty, this.Collection);
					return;
				}
				if (this.PropertyDefinition.AttachedPropertySetter != null)
				{
					this.PropertyDefinition.AttachedPropertySetter.Invoke(null, new object[]
					{
						this.Parent,
						this.Collection
					});
					return;
				}
				if (this.PropertyDefinition.PropertyInfo != null)
				{
					this.PropertyDefinition.PropertyInfo.SetValue(this.Parent, this.Collection, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy, null, null, TypeConverterHelper.InvariantEnglishUS);
					return;
				}
				this._reader.ThrowException("ParserCantGetDPOrPi", this.AttributeName);
			}
		}

		// Token: 0x06003A9B RID: 15003 RVA: 0x001F10C8 File Offset: 0x001F00C8
		internal void InitDefaultValue()
		{
			if (this.AttributeName == "Resources" && this.Parent is IHaveResources)
			{
				this._resourcesParent = (IHaveResources)this.Parent;
				this._defaultCollection = this._resourcesParent.Resources;
				return;
			}
			if (this.PropertyDefinition.DependencyProperty != null)
			{
				this._defaultCollection = ((DependencyObject)this.Parent).GetValue(this.PropertyDefinition.DependencyProperty);
				return;
			}
			if (this.PropertyDefinition.AttachedPropertyGetter != null)
			{
				this._defaultCollection = this.PropertyDefinition.AttachedPropertyGetter.Invoke(null, new object[]
				{
					this.Parent
				});
				return;
			}
			if (this.PropertyDefinition.PropertyInfo != null)
			{
				if (!this.PropertyDefinition.IsInternal)
				{
					this._defaultCollection = this.PropertyDefinition.PropertyInfo.GetValue(this.Parent, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy, null, null, TypeConverterHelper.InvariantEnglishUS);
					return;
				}
				this._defaultCollection = XamlTypeMapper.GetInternalPropertyValue(this._reader.ParserContext, this._reader.ParserContext.RootElement, this.PropertyDefinition.PropertyInfo, this.Parent);
				if (this._defaultCollection == null)
				{
					this._reader.ThrowException("ParserCantGetProperty", this.PropertyDefinition.Name);
					return;
				}
			}
			else
			{
				this._reader.ThrowException("ParserCantGetDPOrPi", this.AttributeName);
			}
		}

		// Token: 0x06003A9C RID: 15004 RVA: 0x001F1258 File Offset: 0x001F0258
		private void CheckReadOnly()
		{
			if (this._resourcesParent == null && (this.PropertyDefinition.DependencyProperty == null || this.PropertyDefinition.DependencyProperty.ReadOnly) && (this.PropertyDefinition.PropertyInfo == null || !this.PropertyDefinition.PropertyInfo.CanWrite) && this.PropertyDefinition.AttachedPropertySetter == null)
			{
				if (this.DefaultCollection == null)
				{
					this._reader.ThrowException("ParserReadOnlyNullProperty", this.PropertyDefinition.Name);
				}
				this.ReadOnly = true;
				this.Collection = this.DefaultCollection;
			}
		}

		// Token: 0x04001DC3 RID: 7619
		private object _collection;

		// Token: 0x04001DC4 RID: 7620
		private object _defaultCollection;

		// Token: 0x04001DC5 RID: 7621
		private short _attributeId;

		// Token: 0x04001DC6 RID: 7622
		private WpfPropertyDefinition _propDef;

		// Token: 0x04001DC7 RID: 7623
		private object _parent;

		// Token: 0x04001DC8 RID: 7624
		private BamlRecordReader _reader;

		// Token: 0x04001DC9 RID: 7625
		private IHaveResources _resourcesParent;

		// Token: 0x04001DCA RID: 7626
		private bool _readonly;

		// Token: 0x04001DCB RID: 7627
		private bool _isClosed;

		// Token: 0x04001DCC RID: 7628
		private bool _isPropertyValueSet;
	}
}
