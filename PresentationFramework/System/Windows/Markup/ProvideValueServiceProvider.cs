using System;
using System.Windows.Media;

namespace System.Windows.Markup
{
	// Token: 0x020004CF RID: 1231
	internal class ProvideValueServiceProvider : IServiceProvider, IProvideValueTarget, IXamlTypeResolver, IUriContext, IFreezeFreezables
	{
		// Token: 0x06003F0F RID: 16143 RVA: 0x00210425 File Offset: 0x0020F425
		internal ProvideValueServiceProvider(ParserContext context)
		{
			this._context = context;
		}

		// Token: 0x06003F10 RID: 16144 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		internal ProvideValueServiceProvider()
		{
		}

		// Token: 0x06003F11 RID: 16145 RVA: 0x00210434 File Offset: 0x0020F434
		internal void SetData(object targetObject, object targetProperty)
		{
			this._targetObject = targetObject;
			this._targetProperty = targetProperty;
		}

		// Token: 0x06003F12 RID: 16146 RVA: 0x00210444 File Offset: 0x0020F444
		internal void ClearData()
		{
			this._targetObject = (this._targetProperty = null);
		}

		// Token: 0x06003F13 RID: 16147 RVA: 0x00210461 File Offset: 0x0020F461
		Type IXamlTypeResolver.Resolve(string qualifiedTypeName)
		{
			return this._context.XamlTypeMapper.GetTypeFromBaseString(qualifiedTypeName, this._context, true);
		}

		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x06003F14 RID: 16148 RVA: 0x0021047B File Offset: 0x0020F47B
		object IProvideValueTarget.TargetObject
		{
			get
			{
				return this._targetObject;
			}
		}

		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x06003F15 RID: 16149 RVA: 0x00210483 File Offset: 0x0020F483
		object IProvideValueTarget.TargetProperty
		{
			get
			{
				return this._targetProperty;
			}
		}

		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x06003F16 RID: 16150 RVA: 0x0021048B File Offset: 0x0020F48B
		// (set) Token: 0x06003F17 RID: 16151 RVA: 0x00210498 File Offset: 0x0020F498
		Uri IUriContext.BaseUri
		{
			get
			{
				return this._context.BaseUri;
			}
			set
			{
				throw new NotSupportedException(SR.Get("ParserProvideValueCantSetUri"));
			}
		}

		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x06003F18 RID: 16152 RVA: 0x002104A9 File Offset: 0x0020F4A9
		bool IFreezeFreezables.FreezeFreezables
		{
			get
			{
				return this._context.FreezeFreezables;
			}
		}

		// Token: 0x06003F19 RID: 16153 RVA: 0x002104B6 File Offset: 0x0020F4B6
		bool IFreezeFreezables.TryFreeze(string value, Freezable freezable)
		{
			return this._context.TryCacheFreezable(value, freezable);
		}

		// Token: 0x06003F1A RID: 16154 RVA: 0x002104C5 File Offset: 0x0020F4C5
		Freezable IFreezeFreezables.TryGetFreezable(string value)
		{
			return this._context.TryGetFreezable(value);
		}

		// Token: 0x06003F1B RID: 16155 RVA: 0x002104D4 File Offset: 0x0020F4D4
		public object GetService(Type service)
		{
			if (service == typeof(IProvideValueTarget))
			{
				return this;
			}
			if (this._context != null)
			{
				if (service == typeof(IXamlTypeResolver))
				{
					return this;
				}
				if (service == typeof(IUriContext))
				{
					return this;
				}
				if (service == typeof(IFreezeFreezables))
				{
					return this;
				}
			}
			return null;
		}

		// Token: 0x04002355 RID: 9045
		private ParserContext _context;

		// Token: 0x04002356 RID: 9046
		private object _targetObject;

		// Token: 0x04002357 RID: 9047
		private object _targetProperty;
	}
}
