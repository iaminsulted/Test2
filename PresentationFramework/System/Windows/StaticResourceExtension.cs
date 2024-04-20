using System;
using System.Collections.Generic;
using System.Windows.Diagnostics;
using System.Windows.Markup;
using System.Xaml;

namespace System.Windows
{
	// Token: 0x0200039F RID: 927
	[MarkupExtensionReturnType(typeof(object))]
	[Localizability(LocalizationCategory.NeverLocalize)]
	public class StaticResourceExtension : MarkupExtension
	{
		// Token: 0x0600256B RID: 9579 RVA: 0x00173A19 File Offset: 0x00172A19
		public StaticResourceExtension()
		{
		}

		// Token: 0x0600256C RID: 9580 RVA: 0x00186492 File Offset: 0x00185492
		public StaticResourceExtension(object resourceKey)
		{
			if (resourceKey == null)
			{
				throw new ArgumentNullException("resourceKey");
			}
			this._resourceKey = resourceKey;
		}

		// Token: 0x0600256D RID: 9581 RVA: 0x001864AF File Offset: 0x001854AF
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (this.ResourceKey is SystemResourceKey)
			{
				return (this.ResourceKey as SystemResourceKey).Resource;
			}
			return this.ProvideValueInternal(serviceProvider, false);
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x0600256E RID: 9582 RVA: 0x001864D7 File Offset: 0x001854D7
		// (set) Token: 0x0600256F RID: 9583 RVA: 0x001864DF File Offset: 0x001854DF
		[ConstructorArgument("resourceKey")]
		public object ResourceKey
		{
			get
			{
				return this._resourceKey;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._resourceKey = value;
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06002570 RID: 9584 RVA: 0x00109403 File Offset: 0x00108403
		internal virtual DeferredResourceReference PrefetchedValue
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x001864F8 File Offset: 0x001854F8
		internal object ProvideValueInternal(IServiceProvider serviceProvider, bool allowDeferredReference)
		{
			object obj = this.TryProvideValueInternal(serviceProvider, allowDeferredReference, false);
			if (obj == DependencyProperty.UnsetValue)
			{
				throw new Exception(SR.Get("ParserNoResource", new object[]
				{
					this.ResourceKey.ToString()
				}));
			}
			return obj;
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x0018653C File Offset: 0x0018553C
		internal object TryProvideValueInternal(IServiceProvider serviceProvider, bool allowDeferredReference, bool mustReturnDeferredResourceReference)
		{
			if (!ResourceDictionaryDiagnostics.HasStaticResourceResolvedListeners)
			{
				return this.TryProvideValueImpl(serviceProvider, allowDeferredReference, mustReturnDeferredResourceReference);
			}
			return this.TryProvideValueWithDiagnosticEvent(serviceProvider, allowDeferredReference, mustReturnDeferredResourceReference);
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x00186558 File Offset: 0x00185558
		private object TryProvideValueWithDiagnosticEvent(IServiceProvider serviceProvider, bool allowDeferredReference, bool mustReturnDeferredResourceReference)
		{
			IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
			if (provideValueTarget == null || provideValueTarget.TargetObject == null || provideValueTarget.TargetProperty == null || ResourceDictionaryDiagnostics.ShouldIgnoreProperty(provideValueTarget.TargetProperty))
			{
				return this.TryProvideValueImpl(serviceProvider, allowDeferredReference, mustReturnDeferredResourceReference);
			}
			bool flag = false;
			ResourceDictionaryDiagnostics.LookupResult result;
			object obj;
			try
			{
				result = ResourceDictionaryDiagnostics.RequestLookupResult(this);
				obj = this.TryProvideValueImpl(serviceProvider, allowDeferredReference, mustReturnDeferredResourceReference);
				DeferredResourceReference deferredResourceReference = obj as DeferredResourceReference;
				if (deferredResourceReference != null)
				{
					flag = true;
					ResourceDictionary dictionary = deferredResourceReference.Dictionary;
					if (dictionary != null)
					{
						ResourceDictionaryDiagnostics.RecordLookupResult(this.ResourceKey, dictionary);
					}
				}
				else
				{
					flag = (obj != DependencyProperty.UnsetValue);
				}
			}
			finally
			{
				ResourceDictionaryDiagnostics.RevertRequest(this, flag);
			}
			if (flag)
			{
				ResourceDictionaryDiagnostics.OnStaticResourceResolved(provideValueTarget.TargetObject, provideValueTarget.TargetProperty, result);
			}
			return obj;
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x00186620 File Offset: 0x00185620
		private object TryProvideValueImpl(IServiceProvider serviceProvider, bool allowDeferredReference, bool mustReturnDeferredResourceReference)
		{
			DeferredResourceReference prefetchedValue = this.PrefetchedValue;
			object obj;
			if (prefetchedValue == null)
			{
				obj = this.FindResourceInEnviroment(serviceProvider, allowDeferredReference, mustReturnDeferredResourceReference);
			}
			else
			{
				obj = this.FindResourceInDeferredContent(serviceProvider, allowDeferredReference, mustReturnDeferredResourceReference);
				if (obj == DependencyProperty.UnsetValue)
				{
					obj = (allowDeferredReference ? prefetchedValue : prefetchedValue.GetValue(BaseValueSourceInternal.Unknown));
				}
			}
			return obj;
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x00186664 File Offset: 0x00185664
		private ResourceDictionary FindTheResourceDictionary(IServiceProvider serviceProvider, bool isDeferredContentSearch)
		{
			IXamlSchemaContextProvider xamlSchemaContextProvider = serviceProvider.GetService(typeof(IXamlSchemaContextProvider)) as IXamlSchemaContextProvider;
			if (xamlSchemaContextProvider == null)
			{
				throw new InvalidOperationException(SR.Get("MarkupExtensionNoContext", new object[]
				{
					base.GetType().Name,
					"IXamlSchemaContextProvider"
				}));
			}
			IAmbientProvider ambientProvider = serviceProvider.GetService(typeof(IAmbientProvider)) as IAmbientProvider;
			if (ambientProvider == null)
			{
				throw new InvalidOperationException(SR.Get("MarkupExtensionNoContext", new object[]
				{
					base.GetType().Name,
					"IAmbientProvider"
				}));
			}
			XamlSchemaContext schemaContext = xamlSchemaContextProvider.SchemaContext;
			XamlType xamlType = schemaContext.GetXamlType(typeof(FrameworkElement));
			XamlType xamlType2 = schemaContext.GetXamlType(typeof(Style));
			XamlType xamlType3 = schemaContext.GetXamlType(typeof(FrameworkTemplate));
			XamlType xamlType4 = schemaContext.GetXamlType(typeof(Application));
			XamlMember member = schemaContext.GetXamlType(typeof(FrameworkContentElement)).GetMember("Resources");
			XamlMember member2 = xamlType.GetMember("Resources");
			XamlMember member3 = xamlType2.GetMember("Resources");
			XamlMember member4 = xamlType2.GetMember("BasedOn");
			XamlMember member5 = xamlType3.GetMember("Resources");
			XamlMember member6 = xamlType4.GetMember("Resources");
			XamlType[] types = new XamlType[]
			{
				schemaContext.GetXamlType(typeof(ResourceDictionary))
			};
			List<AmbientPropertyValue> list = ambientProvider.GetAllAmbientValues(null, isDeferredContentSearch, types, new XamlMember[]
			{
				member,
				member2,
				member3,
				member4,
				member5,
				member6
			}) as List<AmbientPropertyValue>;
			for (int i = 0; i < list.Count; i++)
			{
				AmbientPropertyValue ambientPropertyValue = list[i];
				if (ambientPropertyValue.Value is ResourceDictionary)
				{
					ResourceDictionary resourceDictionary = (ResourceDictionary)ambientPropertyValue.Value;
					if (resourceDictionary.Contains(this.ResourceKey))
					{
						return resourceDictionary;
					}
				}
				if (ambientPropertyValue.Value is Style)
				{
					ResourceDictionary resourceDictionary2 = ((Style)ambientPropertyValue.Value).FindResourceDictionary(this.ResourceKey);
					if (resourceDictionary2 != null)
					{
						return resourceDictionary2;
					}
				}
			}
			return null;
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x00186874 File Offset: 0x00185874
		internal object FindResourceInDeferredContent(IServiceProvider serviceProvider, bool allowDeferredReference, bool mustReturnDeferredResourceReference)
		{
			ResourceDictionary resourceDictionary = this.FindTheResourceDictionary(serviceProvider, true);
			object obj = DependencyProperty.UnsetValue;
			if (resourceDictionary != null)
			{
				obj = resourceDictionary.Lookup(this.ResourceKey, allowDeferredReference, mustReturnDeferredResourceReference, false);
			}
			if (mustReturnDeferredResourceReference && obj == DependencyProperty.UnsetValue)
			{
				obj = new DeferredResourceReferenceHolder(this.ResourceKey, obj);
			}
			return obj;
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x001868BC File Offset: 0x001858BC
		private object FindResourceInAppOrSystem(IServiceProvider serviceProvider, bool allowDeferredReference, bool mustReturnDeferredResourceReference)
		{
			object result;
			if (!SystemResources.IsSystemResourcesParsing)
			{
				object obj;
				result = FrameworkElement.FindResourceFromAppOrSystem(this.ResourceKey, out obj, false, allowDeferredReference, mustReturnDeferredResourceReference);
			}
			else
			{
				result = SystemResources.FindResourceInternal(this.ResourceKey, allowDeferredReference, mustReturnDeferredResourceReference);
			}
			return result;
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x001868F4 File Offset: 0x001858F4
		private object FindResourceInEnviroment(IServiceProvider serviceProvider, bool allowDeferredReference, bool mustReturnDeferredResourceReference)
		{
			ResourceDictionary resourceDictionary = this.FindTheResourceDictionary(serviceProvider, false);
			if (resourceDictionary != null)
			{
				return resourceDictionary.Lookup(this.ResourceKey, allowDeferredReference, mustReturnDeferredResourceReference, false);
			}
			object obj = this.FindResourceInAppOrSystem(serviceProvider, allowDeferredReference, mustReturnDeferredResourceReference);
			if (obj == null)
			{
				obj = DependencyProperty.UnsetValue;
			}
			if (mustReturnDeferredResourceReference && !(obj is DeferredResourceReference))
			{
				obj = new DeferredResourceReferenceHolder(this.ResourceKey, obj);
			}
			return obj;
		}

		// Token: 0x04001190 RID: 4496
		private object _resourceKey;
	}
}
