using System;
using System.Windows;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020001A2 RID: 418
	internal sealed class BamlStartElementNode : BamlTreeNode, ILocalizabilityInheritable
	{
		// Token: 0x06000DD2 RID: 3538 RVA: 0x00136DCA File Offset: 0x00135DCA
		internal BamlStartElementNode(string assemblyName, string typeFullName, bool isInjected, bool useTypeConverter) : base(BamlNodeType.StartElement)
		{
			this._assemblyName = assemblyName;
			this._typeFullName = typeFullName;
			this._isInjected = isInjected;
			this._useTypeConverter = useTypeConverter;
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x00136DF0 File Offset: 0x00135DF0
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteStartElement(this._assemblyName, this._typeFullName, this._isInjected, this._useTypeConverter);
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x00136E10 File Offset: 0x00135E10
		internal override BamlTreeNode Copy()
		{
			return new BamlStartElementNode(this._assemblyName, this._typeFullName, this._isInjected, this._useTypeConverter)
			{
				_content = this._content,
				_uid = this._uid,
				_inheritableAttribute = this._inheritableAttribute
			};
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x00136E60 File Offset: 0x00135E60
		internal void InsertProperty(BamlTreeNode child)
		{
			if (this._children == null)
			{
				base.AddChild(child);
				return;
			}
			int index = 0;
			for (int i = 0; i < this._children.Count; i++)
			{
				if (this._children[i].NodeType == BamlNodeType.Property)
				{
					index = i;
				}
			}
			this._children.Insert(index, child);
			child.Parent = this;
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x00136EBF File Offset: 0x00135EBF
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000DD7 RID: 3543 RVA: 0x00136EC7 File Offset: 0x00135EC7
		internal string TypeFullName
		{
			get
			{
				return this._typeFullName;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x00136ECF File Offset: 0x00135ECF
		// (set) Token: 0x06000DD9 RID: 3545 RVA: 0x00136ED7 File Offset: 0x00135ED7
		internal string Content
		{
			get
			{
				return this._content;
			}
			set
			{
				this._content = value;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000DDA RID: 3546 RVA: 0x00136EE0 File Offset: 0x00135EE0
		// (set) Token: 0x06000DDB RID: 3547 RVA: 0x00136EE8 File Offset: 0x00135EE8
		internal string Uid
		{
			get
			{
				return this._uid;
			}
			set
			{
				this._uid = value;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000DDC RID: 3548 RVA: 0x00136EF4 File Offset: 0x00135EF4
		public ILocalizabilityInheritable LocalizabilityAncestor
		{
			get
			{
				if (this._localizabilityAncestor == null)
				{
					BamlTreeNode parent = base.Parent;
					while (this._localizabilityAncestor == null && parent != null)
					{
						this._localizabilityAncestor = (parent as ILocalizabilityInheritable);
						parent = parent.Parent;
					}
				}
				return this._localizabilityAncestor;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000DDD RID: 3549 RVA: 0x00136F36 File Offset: 0x00135F36
		// (set) Token: 0x06000DDE RID: 3550 RVA: 0x00136F3E File Offset: 0x00135F3E
		public LocalizabilityAttribute InheritableAttribute
		{
			get
			{
				return this._inheritableAttribute;
			}
			set
			{
				this._inheritableAttribute = value;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000DDF RID: 3551 RVA: 0x00136F47 File Offset: 0x00135F47
		// (set) Token: 0x06000DE0 RID: 3552 RVA: 0x00136F4F File Offset: 0x00135F4F
		public bool IsIgnored
		{
			get
			{
				return this._isIgnored;
			}
			set
			{
				this._isIgnored = value;
			}
		}

		// Token: 0x04000A09 RID: 2569
		private string _assemblyName;

		// Token: 0x04000A0A RID: 2570
		private string _typeFullName;

		// Token: 0x04000A0B RID: 2571
		private string _content;

		// Token: 0x04000A0C RID: 2572
		private string _uid;

		// Token: 0x04000A0D RID: 2573
		private LocalizabilityAttribute _inheritableAttribute;

		// Token: 0x04000A0E RID: 2574
		private ILocalizabilityInheritable _localizabilityAncestor;

		// Token: 0x04000A0F RID: 2575
		private bool _isIgnored;

		// Token: 0x04000A10 RID: 2576
		private bool _isInjected;

		// Token: 0x04000A11 RID: 2577
		private bool _useTypeConverter;
	}
}
