using System;

namespace System.Windows
{
	// Token: 0x02000345 RID: 837
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class AttachedPropertyBrowsableForChildrenAttribute : AttachedPropertyBrowsableAttribute
	{
		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001FC6 RID: 8134 RVA: 0x001731B0 File Offset: 0x001721B0
		// (set) Token: 0x06001FC7 RID: 8135 RVA: 0x001731B8 File Offset: 0x001721B8
		public bool IncludeDescendants
		{
			get
			{
				return this._includeDescendants;
			}
			set
			{
				this._includeDescendants = value;
			}
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x001731C4 File Offset: 0x001721C4
		public override bool Equals(object obj)
		{
			AttachedPropertyBrowsableForChildrenAttribute attachedPropertyBrowsableForChildrenAttribute = obj as AttachedPropertyBrowsableForChildrenAttribute;
			return attachedPropertyBrowsableForChildrenAttribute != null && this._includeDescendants == attachedPropertyBrowsableForChildrenAttribute._includeDescendants;
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x001731EB File Offset: 0x001721EB
		public override int GetHashCode()
		{
			return this._includeDescendants.GetHashCode();
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x001731F8 File Offset: 0x001721F8
		internal override bool IsBrowsable(DependencyObject d, DependencyProperty dp)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			if (dp == null)
			{
				throw new ArgumentNullException("dp");
			}
			DependencyObject dependencyObject = d;
			Type ownerType = dp.OwnerType;
			for (;;)
			{
				dependencyObject = FrameworkElement.GetFrameworkParent(dependencyObject);
				if (dependencyObject != null && ownerType.IsInstanceOfType(dependencyObject))
				{
					break;
				}
				if (!this._includeDescendants || dependencyObject == null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000FA3 RID: 4003
		private bool _includeDescendants;
	}
}
