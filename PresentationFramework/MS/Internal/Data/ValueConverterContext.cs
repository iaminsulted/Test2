using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace MS.Internal.Data
{
	// Token: 0x0200021F RID: 543
	internal class ValueConverterContext : ITypeDescriptorContext, IServiceProvider, IUriContext
	{
		// Token: 0x06001469 RID: 5225 RVA: 0x00151F67 File Offset: 0x00150F67
		public virtual object GetService(Type serviceType)
		{
			if (serviceType == typeof(IUriContext))
			{
				return this;
			}
			return null;
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x0600146A RID: 5226 RVA: 0x00151F7E File Offset: 0x00150F7E
		// (set) Token: 0x0600146B RID: 5227 RVA: 0x0012F160 File Offset: 0x0012E160
		public Uri BaseUri
		{
			get
			{
				if (this._cachedBaseUri == null)
				{
					if (this._targetElement != null)
					{
						this._cachedBaseUri = BaseUriHelper.GetBaseUri(this._targetElement);
					}
					else
					{
						this._cachedBaseUri = BaseUriHelper.BaseUri;
					}
				}
				return this._cachedBaseUri;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x00151FBC File Offset: 0x00150FBC
		internal void SetTargetElement(DependencyObject target)
		{
			if (target != null)
			{
				this._nestingLevel++;
			}
			else if (this._nestingLevel > 0)
			{
				this._nestingLevel--;
			}
			Invariant.Assert(this._nestingLevel <= 1, "illegal to recurse/reenter ValueConverterContext.SetTargetElement()");
			this._targetElement = target;
			this._cachedBaseUri = null;
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x0600146D RID: 5229 RVA: 0x00152017 File Offset: 0x00151017
		internal bool IsInUse
		{
			get
			{
				return this._nestingLevel > 0;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x0600146E RID: 5230 RVA: 0x00109403 File Offset: 0x00108403
		public IContainer Container
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x0600146F RID: 5231 RVA: 0x00109403 File Offset: 0x00108403
		public object Instance
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06001470 RID: 5232 RVA: 0x00109403 File Offset: 0x00108403
		public PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void OnComponentChanged()
		{
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool OnComponentChanging()
		{
			return false;
		}

		// Token: 0x04000BCB RID: 3019
		private DependencyObject _targetElement;

		// Token: 0x04000BCC RID: 3020
		private int _nestingLevel;

		// Token: 0x04000BCD RID: 3021
		private Uri _cachedBaseUri;
	}
}
