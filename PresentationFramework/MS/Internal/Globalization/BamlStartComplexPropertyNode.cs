using System;
using System.Windows;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020001A5 RID: 421
	internal class BamlStartComplexPropertyNode : BamlTreeNode, ILocalizabilityInheritable
	{
		// Token: 0x06000DE7 RID: 3559 RVA: 0x00136FAE File Offset: 0x00135FAE
		internal BamlStartComplexPropertyNode(string assemblyName, string ownerTypeFullName, string propertyName) : base(BamlNodeType.StartComplexProperty)
		{
			this._assemblyName = assemblyName;
			this._ownerTypeFullName = ownerTypeFullName;
			this._propertyName = propertyName;
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x00136FCD File Offset: 0x00135FCD
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteStartComplexProperty(this._assemblyName, this._ownerTypeFullName, this._propertyName);
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00136FE7 File Offset: 0x00135FE7
		internal override BamlTreeNode Copy()
		{
			return new BamlStartComplexPropertyNode(this._assemblyName, this._ownerTypeFullName, this._propertyName);
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000DEA RID: 3562 RVA: 0x00137000 File Offset: 0x00136000
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000DEB RID: 3563 RVA: 0x00137008 File Offset: 0x00136008
		internal string PropertyName
		{
			get
			{
				return this._propertyName;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000DEC RID: 3564 RVA: 0x00137010 File Offset: 0x00136010
		internal string OwnerTypeFullName
		{
			get
			{
				return this._ownerTypeFullName;
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000DED RID: 3565 RVA: 0x00137018 File Offset: 0x00136018
		// (set) Token: 0x06000DEE RID: 3566 RVA: 0x00137020 File Offset: 0x00136020
		public ILocalizabilityInheritable LocalizabilityAncestor
		{
			get
			{
				return this._localizabilityAncestor;
			}
			set
			{
				this._localizabilityAncestor = value;
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000DEF RID: 3567 RVA: 0x00137029 File Offset: 0x00136029
		// (set) Token: 0x06000DF0 RID: 3568 RVA: 0x00137031 File Offset: 0x00136031
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

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x0013703A File Offset: 0x0013603A
		// (set) Token: 0x06000DF2 RID: 3570 RVA: 0x00137042 File Offset: 0x00136042
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

		// Token: 0x04000A14 RID: 2580
		protected string _assemblyName;

		// Token: 0x04000A15 RID: 2581
		protected string _ownerTypeFullName;

		// Token: 0x04000A16 RID: 2582
		protected string _propertyName;

		// Token: 0x04000A17 RID: 2583
		private ILocalizabilityInheritable _localizabilityAncestor;

		// Token: 0x04000A18 RID: 2584
		private LocalizabilityAttribute _inheritableAttribute;

		// Token: 0x04000A19 RID: 2585
		private bool _isIgnored;
	}
}
