using System;
using System.ComponentModel;

namespace System.Windows.Markup
{
	// Token: 0x020004D9 RID: 1241
	internal class TypeConvertContext : ITypeDescriptorContext, IServiceProvider
	{
		// Token: 0x06003F5F RID: 16223 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void OnComponentChanged()
		{
		}

		// Token: 0x06003F60 RID: 16224 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool OnComponentChanging()
		{
			return false;
		}

		// Token: 0x06003F61 RID: 16225 RVA: 0x00211028 File Offset: 0x00210028
		public virtual object GetService(Type serviceType)
		{
			if (serviceType == typeof(IUriContext))
			{
				return this._parserContext;
			}
			if (serviceType == typeof(string))
			{
				return this._attribStringValue;
			}
			return this._parserContext.ProvideValueProvider.GetService(serviceType);
		}

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x06003F62 RID: 16226 RVA: 0x00109403 File Offset: 0x00108403
		public IContainer Container
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x06003F63 RID: 16227 RVA: 0x00109403 File Offset: 0x00108403
		public object Instance
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06003F64 RID: 16228 RVA: 0x00109403 File Offset: 0x00108403
		public PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06003F65 RID: 16229 RVA: 0x00211078 File Offset: 0x00210078
		public ParserContext ParserContext
		{
			get
			{
				return this._parserContext;
			}
		}

		// Token: 0x06003F66 RID: 16230 RVA: 0x00211080 File Offset: 0x00210080
		public TypeConvertContext(ParserContext parserContext)
		{
			this._parserContext = parserContext;
		}

		// Token: 0x04002377 RID: 9079
		private ParserContext _parserContext;

		// Token: 0x04002378 RID: 9080
		private string _attribStringValue;
	}
}
