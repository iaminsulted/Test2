using System;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000530 RID: 1328
	internal class ValueSerializerContextWrapper : IValueSerializerContext, ITypeDescriptorContext, IServiceProvider
	{
		// Token: 0x060041D1 RID: 16849 RVA: 0x0021953C File Offset: 0x0021853C
		public ValueSerializerContextWrapper(IValueSerializerContext baseContext)
		{
			this._baseContext = baseContext;
		}

		// Token: 0x060041D2 RID: 16850 RVA: 0x0021954B File Offset: 0x0021854B
		public ValueSerializer GetValueSerializerFor(PropertyDescriptor descriptor)
		{
			if (this._baseContext != null)
			{
				return this._baseContext.GetValueSerializerFor(descriptor);
			}
			return null;
		}

		// Token: 0x060041D3 RID: 16851 RVA: 0x00219563 File Offset: 0x00218563
		public ValueSerializer GetValueSerializerFor(Type type)
		{
			if (this._baseContext != null)
			{
				return this._baseContext.GetValueSerializerFor(type);
			}
			return null;
		}

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x060041D4 RID: 16852 RVA: 0x0021957B File Offset: 0x0021857B
		public IContainer Container
		{
			get
			{
				if (this._baseContext != null)
				{
					return this._baseContext.Container;
				}
				return null;
			}
		}

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x060041D5 RID: 16853 RVA: 0x00219592 File Offset: 0x00218592
		public object Instance
		{
			get
			{
				if (this._baseContext != null)
				{
					return this._baseContext.Instance;
				}
				return null;
			}
		}

		// Token: 0x060041D6 RID: 16854 RVA: 0x002195A9 File Offset: 0x002185A9
		public void OnComponentChanged()
		{
			if (this._baseContext != null)
			{
				this._baseContext.OnComponentChanged();
			}
		}

		// Token: 0x060041D7 RID: 16855 RVA: 0x002195BE File Offset: 0x002185BE
		public bool OnComponentChanging()
		{
			return this._baseContext == null || this._baseContext.OnComponentChanging();
		}

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x060041D8 RID: 16856 RVA: 0x002195D5 File Offset: 0x002185D5
		public PropertyDescriptor PropertyDescriptor
		{
			get
			{
				if (this._baseContext != null)
				{
					return this._baseContext.PropertyDescriptor;
				}
				return null;
			}
		}

		// Token: 0x060041D9 RID: 16857 RVA: 0x002195EC File Offset: 0x002185EC
		public object GetService(Type serviceType)
		{
			if (this._baseContext != null)
			{
				return this._baseContext.GetService(serviceType);
			}
			return null;
		}

		// Token: 0x040024DD RID: 9437
		private IValueSerializerContext _baseContext;
	}
}
