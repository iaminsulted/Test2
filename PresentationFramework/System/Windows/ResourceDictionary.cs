using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Baml2006;
using System.Windows.Diagnostics;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xaml;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Utility;

namespace System.Windows
{
	// Token: 0x0200038D RID: 909
	[UsableDuringInitialization(true)]
	[Localizability(LocalizationCategory.Ignore)]
	[Ambient]
	public class ResourceDictionary : IDictionary, ICollection, IEnumerable, ISupportInitialize, IUriContext, INameScope
	{
		// Token: 0x06002491 RID: 9361 RVA: 0x001836E1 File Offset: 0x001826E1
		public ResourceDictionary()
		{
			this._baseDictionary = new Hashtable();
			this.IsThemeDictionary = SystemResources.IsSystemResourcesParsing;
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x0018370A File Offset: 0x0018270A
		static ResourceDictionary()
		{
			ResourceDictionary.DummyInheritanceContext.DetachFromDispatcher();
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x00183720 File Offset: 0x00182720
		public void CopyTo(DictionaryEntry[] array, int arrayIndex)
		{
			if (this.CanBeAccessedAcrossThreads)
			{
				object syncRoot = ((ICollection)this).SyncRoot;
				lock (syncRoot)
				{
					this.CopyToWithoutLock(array, arrayIndex);
					return;
				}
			}
			this.CopyToWithoutLock(array, arrayIndex);
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x00183774 File Offset: 0x00182774
		private void CopyToWithoutLock(DictionaryEntry[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			this._baseDictionary.CopyTo(array, arrayIndex);
			int num = arrayIndex + this.Count;
			for (int i = arrayIndex; i < num; i++)
			{
				DictionaryEntry dictionaryEntry = array[i];
				object value = dictionaryEntry.Value;
				bool flag;
				this.OnGettingValuePrivate(dictionaryEntry.Key, ref value, out flag);
				dictionaryEntry.Value = value;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06002495 RID: 9365 RVA: 0x001837DA File Offset: 0x001827DA
		public Collection<ResourceDictionary> MergedDictionaries
		{
			get
			{
				if (this._mergedDictionaries == null)
				{
					this._mergedDictionaries = new ResourceDictionaryCollection(this);
					this._mergedDictionaries.CollectionChanged += this.OnMergedDictionariesChanged;
				}
				return this._mergedDictionaries;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06002496 RID: 9366 RVA: 0x0018380D File Offset: 0x0018280D
		// (set) Token: 0x06002497 RID: 9367 RVA: 0x00183818 File Offset: 0x00182818
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Uri Source
		{
			get
			{
				return this._source;
			}
			set
			{
				if (value == null || string.IsNullOrEmpty(value.OriginalString))
				{
					throw new ArgumentException(SR.Get("ResourceDictionaryLoadFromFailure", new object[]
					{
						(value == null) ? "''" : value.ToString()
					}));
				}
				ResourceDictionaryDiagnostics.RemoveResourceDictionaryForUri(this._source, this);
				ResourceDictionary.ResourceDictionarySourceUriWrapper resourceDictionarySourceUriWrapper = value as ResourceDictionary.ResourceDictionarySourceUriWrapper;
				Uri orgUri;
				if (resourceDictionarySourceUriWrapper == null)
				{
					this._source = value;
					orgUri = this._source;
				}
				else
				{
					this._source = resourceDictionarySourceUriWrapper.OriginalUri;
					orgUri = resourceDictionarySourceUriWrapper.VersionedUri;
				}
				this.Clear();
				Uri resolvedUri = BindUriHelper.GetResolvedUri(this._baseUri, orgUri);
				WebRequest request = WpfWebRequestHelper.CreateRequest(resolvedUri);
				WpfWebRequestHelper.ConfigCachePolicy(request, false);
				ContentType contentType = null;
				Stream s = null;
				try
				{
					s = WpfWebRequestHelper.GetResponseStream(request, out contentType);
				}
				catch (IOException)
				{
					if (this.IsSourcedFromThemeDictionary)
					{
						ResourceDictionary.FallbackState fallbackState = this._fallbackState;
						if (fallbackState != ResourceDictionary.FallbackState.Classic)
						{
							if (fallbackState == ResourceDictionary.FallbackState.Generic)
							{
								this._fallbackState = ResourceDictionary.FallbackState.None;
								Uri source = ThemeDictionaryExtension.GenerateFallbackUri(this, "themes/generic");
								this.Source = source;
							}
						}
						else
						{
							this._fallbackState = ResourceDictionary.FallbackState.Generic;
							Uri source2 = ThemeDictionaryExtension.GenerateFallbackUri(this, "themes/classic");
							this.Source = source2;
							this._fallbackState = ResourceDictionary.FallbackState.Classic;
						}
						return;
					}
					throw;
				}
				System.Windows.Markup.XamlReader xamlReader;
				ResourceDictionary resourceDictionary = MimeObjectFactory.GetObjectAndCloseStream(s, contentType, resolvedUri, false, false, false, false, out xamlReader) as ResourceDictionary;
				if (resourceDictionary == null)
				{
					throw new InvalidOperationException(SR.Get("ResourceDictionaryLoadFromFailure", new object[]
					{
						this._source.ToString()
					}));
				}
				this._baseDictionary = resourceDictionary._baseDictionary;
				this._mergedDictionaries = resourceDictionary._mergedDictionaries;
				this.CopyDeferredContentFrom(resourceDictionary);
				this.MoveDeferredResourceReferencesFrom(resourceDictionary);
				this.HasImplicitStyles = resourceDictionary.HasImplicitStyles;
				this.HasImplicitDataTemplates = resourceDictionary.HasImplicitDataTemplates;
				this.InvalidatesImplicitDataTemplateResources = resourceDictionary.InvalidatesImplicitDataTemplateResources;
				if (this.InheritanceContext != null)
				{
					this.AddInheritanceContextToValues();
				}
				if (this._mergedDictionaries != null)
				{
					for (int i = 0; i < this._mergedDictionaries.Count; i++)
					{
						this.PropagateParentOwners(this._mergedDictionaries[i]);
					}
				}
				ResourceDictionaryDiagnostics.AddResourceDictionaryForUri(resolvedUri, this);
				if (!this.IsInitializePending)
				{
					this.NotifyOwners(new ResourcesChangeInfo(null, this));
				}
			}
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x00183A40 File Offset: 0x00182A40
		public void RegisterName(string name, object scopedElement)
		{
			throw new NotSupportedException(SR.Get("NamesNotSupportedInsideResourceDictionary"));
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void UnregisterName(string name)
		{
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x00109403 File Offset: 0x00108403
		public object FindName(string name)
		{
			return null;
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x0600249B RID: 9371 RVA: 0x00183A51 File Offset: 0x00182A51
		// (set) Token: 0x0600249C RID: 9372 RVA: 0x00183A59 File Offset: 0x00182A59
		Uri IUriContext.BaseUri
		{
			get
			{
				return this._baseUri;
			}
			set
			{
				this._baseUri = value;
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x0600249D RID: 9373 RVA: 0x00183A62 File Offset: 0x00182A62
		public bool IsFixedSize
		{
			get
			{
				return this._baseDictionary.IsFixedSize;
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x0600249E RID: 9374 RVA: 0x00183A6F File Offset: 0x00182A6F
		// (set) Token: 0x0600249F RID: 9375 RVA: 0x00183A78 File Offset: 0x00182A78
		public bool IsReadOnly
		{
			get
			{
				return this.ReadPrivateFlag(ResourceDictionary.PrivateFlags.IsReadOnly);
			}
			internal set
			{
				this.WritePrivateFlag(ResourceDictionary.PrivateFlags.IsReadOnly, value);
				if (value)
				{
					this.SealValues();
				}
				if (this._mergedDictionaries != null)
				{
					for (int i = 0; i < this._mergedDictionaries.Count; i++)
					{
						this._mergedDictionaries[i].IsReadOnly = value;
					}
				}
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x060024A0 RID: 9376 RVA: 0x00183AC6 File Offset: 0x00182AC6
		// (set) Token: 0x060024A1 RID: 9377 RVA: 0x00183AD0 File Offset: 0x00182AD0
		[DefaultValue(false)]
		public bool InvalidatesImplicitDataTemplateResources
		{
			get
			{
				return this.ReadPrivateFlag(ResourceDictionary.PrivateFlags.InvalidatesImplicitDataTemplateResources);
			}
			set
			{
				this.WritePrivateFlag(ResourceDictionary.PrivateFlags.InvalidatesImplicitDataTemplateResources, value);
			}
		}

		// Token: 0x17000749 RID: 1865
		public object this[object key]
		{
			get
			{
				bool flag;
				return this.GetValue(key, out flag);
			}
			set
			{
				this.SealValue(value);
				if (this.CanBeAccessedAcrossThreads)
				{
					object syncRoot = ((ICollection)this).SyncRoot;
					lock (syncRoot)
					{
						this.SetValueWithoutLock(key, value);
						return;
					}
				}
				this.SetValueWithoutLock(key, value);
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x060024A4 RID: 9380 RVA: 0x00109403 File Offset: 0x00108403
		// (set) Token: 0x060024A5 RID: 9381 RVA: 0x00183B50 File Offset: 0x00182B50
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DeferrableContent DeferrableContent
		{
			get
			{
				return null;
			}
			set
			{
				this.SetDeferrableContent(value);
			}
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x00183B5C File Offset: 0x00182B5C
		private void SetValueWithoutLock(object key, object value)
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("ResourceDictionaryIsReadOnly"));
			}
			if (this._baseDictionary[key] != value)
			{
				this.ValidateDeferredResourceReferences(key);
				if (TraceResourceDictionary.IsEnabled)
				{
					TraceResourceDictionary.Trace(TraceEventType.Start, TraceResourceDictionary.AddResource, this, key, value);
				}
				this._baseDictionary[key] = value;
				this.UpdateHasImplicitStyles(key);
				this.UpdateHasImplicitDataTemplates(key);
				this.NotifyOwners(new ResourcesChangeInfo(key));
				if (TraceResourceDictionary.IsEnabled)
				{
					TraceResourceDictionary.Trace(TraceEventType.Stop, TraceResourceDictionary.AddResource, this, key, value);
				}
			}
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x00183BF0 File Offset: 0x00182BF0
		internal object GetValue(object key, out bool canCache)
		{
			if (this.CanBeAccessedAcrossThreads)
			{
				object syncRoot = ((ICollection)this).SyncRoot;
				lock (syncRoot)
				{
					return this.GetValueWithoutLock(key, out canCache);
				}
			}
			return this.GetValueWithoutLock(key, out canCache);
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x00183C48 File Offset: 0x00182C48
		private object GetValueWithoutLock(object key, out bool canCache)
		{
			object obj = this._baseDictionary[key];
			if (obj != null)
			{
				this.OnGettingValuePrivate(key, ref obj, out canCache);
			}
			else
			{
				canCache = true;
				if (this._mergedDictionaries != null)
				{
					for (int i = this.MergedDictionaries.Count - 1; i > -1; i--)
					{
						ResourceDictionary resourceDictionary = this.MergedDictionaries[i];
						if (resourceDictionary != null)
						{
							obj = resourceDictionary.GetValue(key, out canCache);
							if (obj != null)
							{
								break;
							}
						}
					}
				}
			}
			return obj;
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x00183CB4 File Offset: 0x00182CB4
		internal Type GetValueType(object key, out bool found)
		{
			found = false;
			Type result = null;
			object obj = this._baseDictionary[key];
			if (obj != null)
			{
				found = true;
				KeyRecord keyRecord = obj as KeyRecord;
				if (keyRecord != null)
				{
					result = this.GetTypeOfFirstObject(keyRecord);
				}
				else
				{
					result = obj.GetType();
				}
			}
			else if (this._mergedDictionaries != null)
			{
				for (int i = this.MergedDictionaries.Count - 1; i > -1; i--)
				{
					ResourceDictionary resourceDictionary = this.MergedDictionaries[i];
					if (resourceDictionary != null)
					{
						result = resourceDictionary.GetValueType(key, out found);
						if (found)
						{
							break;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x060024AA RID: 9386 RVA: 0x00183D38 File Offset: 0x00182D38
		public ICollection Keys
		{
			get
			{
				object[] array = new object[this.Count];
				this._baseDictionary.Keys.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x060024AB RID: 9387 RVA: 0x00183D64 File Offset: 0x00182D64
		public ICollection Values
		{
			get
			{
				return new ResourceDictionary.ResourceValuesCollection(this);
			}
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x00183D6C File Offset: 0x00182D6C
		public void Add(object key, object value)
		{
			this.SealValue(value);
			if (this.CanBeAccessedAcrossThreads)
			{
				object syncRoot = ((ICollection)this).SyncRoot;
				lock (syncRoot)
				{
					this.AddWithoutLock(key, value);
					return;
				}
			}
			this.AddWithoutLock(key, value);
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x00183DC8 File Offset: 0x00182DC8
		private void AddWithoutLock(object key, object value)
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("ResourceDictionaryIsReadOnly"));
			}
			VisualDiagnostics.VerifyVisualTreeChange(this.InheritanceContext);
			if (TraceResourceDictionary.IsEnabled)
			{
				TraceResourceDictionary.Trace(TraceEventType.Start, TraceResourceDictionary.AddResource, this, key, value);
			}
			this._baseDictionary.Add(key, value);
			this.UpdateHasImplicitStyles(key);
			this.UpdateHasImplicitDataTemplates(key);
			this.NotifyOwners(new ResourcesChangeInfo(key));
			if (TraceResourceDictionary.IsEnabled)
			{
				TraceResourceDictionary.Trace(TraceEventType.Stop, TraceResourceDictionary.AddResource, this, key, value);
			}
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x00183E54 File Offset: 0x00182E54
		public void Clear()
		{
			if (this.CanBeAccessedAcrossThreads)
			{
				object syncRoot = ((ICollection)this).SyncRoot;
				lock (syncRoot)
				{
					this.ClearWithoutLock();
					return;
				}
			}
			this.ClearWithoutLock();
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x00183EA4 File Offset: 0x00182EA4
		private void ClearWithoutLock()
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("ResourceDictionaryIsReadOnly"));
			}
			VisualDiagnostics.VerifyVisualTreeChange(this.InheritanceContext);
			if (this.Count > 0)
			{
				this.ValidateDeferredResourceReferences(null);
				this.RemoveInheritanceContextFromValues();
				this._baseDictionary.Clear();
				this.NotifyOwners(ResourcesChangeInfo.CatastrophicDictionaryChangeInfo);
			}
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x00183F00 File Offset: 0x00182F00
		public bool Contains(object key)
		{
			bool flag = this._baseDictionary.Contains(key);
			if (flag)
			{
				KeyRecord keyRecord = this._baseDictionary[key] as KeyRecord;
				if (keyRecord != null && this._deferredLocationList.Contains(keyRecord))
				{
					return false;
				}
			}
			if (this._mergedDictionaries != null)
			{
				int num = this.MergedDictionaries.Count - 1;
				while (num > -1 && !flag)
				{
					ResourceDictionary resourceDictionary = this.MergedDictionaries[num];
					if (resourceDictionary != null)
					{
						flag = resourceDictionary.Contains(key);
					}
					num--;
				}
			}
			return flag;
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x00183F7E File Offset: 0x00182F7E
		private bool ContainsBamlObjectFactory(object key)
		{
			return this.GetBamlObjectFactory(key) != null;
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x00183F8C File Offset: 0x00182F8C
		private KeyRecord GetBamlObjectFactory(object key)
		{
			if (this._baseDictionary.Contains(key))
			{
				return this._baseDictionary[key] as KeyRecord;
			}
			if (this._mergedDictionaries != null)
			{
				for (int i = this.MergedDictionaries.Count - 1; i > -1; i--)
				{
					ResourceDictionary resourceDictionary = this.MergedDictionaries[i];
					if (resourceDictionary != null)
					{
						KeyRecord bamlObjectFactory = resourceDictionary.GetBamlObjectFactory(key);
						if (bamlObjectFactory != null)
						{
							return bamlObjectFactory;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x00183FF7 File Offset: 0x00182FF7
		public IDictionaryEnumerator GetEnumerator()
		{
			return new ResourceDictionary.ResourceDictionaryEnumerator(this);
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x00184000 File Offset: 0x00183000
		public void Remove(object key)
		{
			if (this.CanBeAccessedAcrossThreads)
			{
				object syncRoot = ((ICollection)this).SyncRoot;
				lock (syncRoot)
				{
					this.RemoveWithoutLock(key);
					return;
				}
			}
			this.RemoveWithoutLock(key);
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x00184050 File Offset: 0x00183050
		private void RemoveWithoutLock(object key)
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("ResourceDictionaryIsReadOnly"));
			}
			VisualDiagnostics.VerifyVisualTreeChange(this.InheritanceContext);
			this.ValidateDeferredResourceReferences(key);
			this.RemoveInheritanceContext(this._baseDictionary[key]);
			this._baseDictionary.Remove(key);
			this.NotifyOwners(new ResourcesChangeInfo(key));
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x060024B6 RID: 9398 RVA: 0x001840B1 File Offset: 0x001830B1
		public int Count
		{
			get
			{
				return this._baseDictionary.Count;
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x060024B7 RID: 9399 RVA: 0x001840BE File Offset: 0x001830BE
		bool ICollection.IsSynchronized
		{
			get
			{
				return this._baseDictionary.IsSynchronized;
			}
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x060024B8 RID: 9400 RVA: 0x001840CB File Offset: 0x001830CB
		object ICollection.SyncRoot
		{
			get
			{
				if (this.CanBeAccessedAcrossThreads)
				{
					return SystemResources.ThemeDictionaryLock;
				}
				return this._baseDictionary.SyncRoot;
			}
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x001840E6 File Offset: 0x001830E6
		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			this.CopyTo(array as DictionaryEntry[], arrayIndex);
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x001840F5 File Offset: 0x001830F5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary)this).GetEnumerator();
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x001840FD File Offset: 0x001830FD
		public void BeginInit()
		{
			if (this.IsInitializePending)
			{
				throw new InvalidOperationException(SR.Get("NestedBeginInitNotSupported"));
			}
			this.IsInitializePending = true;
			this.IsInitialized = false;
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x00184125 File Offset: 0x00183125
		public void EndInit()
		{
			if (!this.IsInitializePending)
			{
				throw new InvalidOperationException(SR.Get("EndInitWithoutBeginInitNotSupported"));
			}
			this.IsInitializePending = false;
			this.IsInitialized = true;
			this.NotifyOwners(new ResourcesChangeInfo(null, this));
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x0018415A File Offset: 0x0018315A
		private bool CanCache(KeyRecord keyRecord, object value)
		{
			return !keyRecord.SharedSet || keyRecord.Shared;
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x0018416C File Offset: 0x0018316C
		private void OnGettingValuePrivate(object key, ref object value, out bool canCache)
		{
			ResourceDictionaryDiagnostics.RecordLookupResult(key, this);
			this.OnGettingValue(key, ref value, out canCache);
			if ((key != null & canCache) && !object.Equals(this._baseDictionary[key], value))
			{
				if (this.InheritanceContext != null)
				{
					this.AddInheritanceContext(this.InheritanceContext, value);
				}
				this._baseDictionary[key] = value;
			}
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x001841CC File Offset: 0x001831CC
		protected virtual void OnGettingValue(object key, ref object value, out bool canCache)
		{
			KeyRecord keyRecord = value as KeyRecord;
			if (keyRecord == null)
			{
				canCache = true;
				return;
			}
			if (this._deferredLocationList.Contains(keyRecord))
			{
				canCache = false;
				value = null;
				return;
			}
			this._deferredLocationList.Add(keyRecord);
			try
			{
				if (TraceResourceDictionary.IsEnabled)
				{
					TraceResourceDictionary.Trace(TraceEventType.Start, TraceResourceDictionary.RealizeDeferContent, this, key, value);
				}
				value = this.CreateObject(keyRecord);
			}
			finally
			{
				if (TraceResourceDictionary.IsEnabled)
				{
					TraceResourceDictionary.Trace(TraceEventType.Stop, TraceResourceDictionary.RealizeDeferContent, this, key, value);
				}
			}
			this._deferredLocationList.Remove(keyRecord);
			if (key != null)
			{
				canCache = this.CanCache(keyRecord, value);
				if (canCache)
				{
					this.SealValue(value);
					this._numDefer--;
					if (this._numDefer == 0)
					{
						this.CloseReader();
						return;
					}
				}
			}
			else
			{
				canCache = true;
			}
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x001842A0 File Offset: 0x001832A0
		private void SetDeferrableContent(DeferrableContent deferrableContent)
		{
			Baml2006ReaderSettings baml2006ReaderSettings = new Baml2006ReaderSettings(deferrableContent.SchemaContext.Settings);
			baml2006ReaderSettings.IsBamlFragment = true;
			baml2006ReaderSettings.OwnsStream = true;
			baml2006ReaderSettings.BaseUri = null;
			Baml2006Reader baml2006Reader = new Baml2006Reader(deferrableContent.Stream, deferrableContent.SchemaContext, baml2006ReaderSettings);
			this._objectWriterFactory = deferrableContent.ObjectWriterFactory;
			this._objectWriterSettings = deferrableContent.ObjectWriterParentSettings;
			this._deferredLocationList = new List<KeyRecord>();
			this._rootElement = deferrableContent.RootObject;
			IList<KeyRecord> list = baml2006Reader.ReadKeys();
			if (this._source == null)
			{
				if (this._reader == null)
				{
					this._reader = baml2006Reader;
					this.SetKeys(list, deferrableContent.ServiceProvider);
					return;
				}
				throw new InvalidOperationException(SR.Get("ResourceDictionaryDuplicateDeferredContent"));
			}
			else
			{
				if (list.Count > 0)
				{
					throw new InvalidOperationException(SR.Get("ResourceDictionaryDeferredContentFailure"));
				}
				return;
			}
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x00184370 File Offset: 0x00183370
		private object GetKeyValue(KeyRecord key, IServiceProvider serviceProvider)
		{
			if (key.KeyString != null)
			{
				return key.KeyString;
			}
			if (key.KeyType != null)
			{
				return key.KeyType;
			}
			System.Xaml.XamlReader reader = key.KeyNodeList.GetReader();
			return this.EvaluateMarkupExtensionNodeList(reader, serviceProvider);
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x001843B8 File Offset: 0x001833B8
		private object EvaluateMarkupExtensionNodeList(System.Xaml.XamlReader reader, IServiceProvider serviceProvider)
		{
			XamlObjectWriter xamlObjectWriter = this._objectWriterFactory.GetXamlObjectWriter(null);
			XamlServices.Transform(reader, xamlObjectWriter);
			object obj = xamlObjectWriter.Result;
			MarkupExtension markupExtension = obj as MarkupExtension;
			if (markupExtension != null)
			{
				obj = markupExtension.ProvideValue(serviceProvider);
			}
			return obj;
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x001843F4 File Offset: 0x001833F4
		private object GetStaticResourceKeyValue(StaticResource staticResource, IServiceProvider serviceProvider)
		{
			System.Xaml.XamlReader reader = staticResource.ResourceNodeList.GetReader();
			XamlType xamlType = reader.SchemaContext.GetXamlType(typeof(StaticResourceExtension));
			XamlMember member = xamlType.GetMember("ResourceKey");
			reader.Read();
			if (reader.NodeType == System.Xaml.XamlNodeType.StartObject && reader.Type == xamlType)
			{
				reader.Read();
				while (reader.NodeType == System.Xaml.XamlNodeType.StartMember && reader.Member != XamlLanguage.PositionalParameters && reader.Member != member)
				{
					reader.Skip();
				}
				if (reader.NodeType == System.Xaml.XamlNodeType.StartMember)
				{
					object result = null;
					reader.Read();
					if (reader.NodeType == System.Xaml.XamlNodeType.StartObject)
					{
						System.Xaml.XamlReader reader2 = reader.ReadSubtree();
						result = this.EvaluateMarkupExtensionNodeList(reader2, serviceProvider);
					}
					else if (reader.NodeType == System.Xaml.XamlNodeType.Value)
					{
						result = reader.Value;
					}
					return result;
				}
			}
			return null;
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x001844CC File Offset: 0x001834CC
		private void SetKeys(IList<KeyRecord> keyCollection, IServiceProvider serviceProvider)
		{
			this._numDefer = keyCollection.Count;
			StaticResourceExtension staticResourceWorker = new StaticResourceExtension();
			for (int i = 0; i < keyCollection.Count; i++)
			{
				KeyRecord keyRecord = keyCollection[i];
				if (keyRecord == null)
				{
					throw new ArgumentException(SR.Get("KeyCollectionHasInvalidKey"));
				}
				object keyValue = this.GetKeyValue(keyRecord, serviceProvider);
				this.UpdateHasImplicitStyles(keyValue);
				this.UpdateHasImplicitDataTemplates(keyValue);
				if (keyRecord != null && keyRecord.HasStaticResources)
				{
					this.SetOptimizedStaticResources(keyRecord.StaticResources, serviceProvider, staticResourceWorker);
				}
				this._baseDictionary.Add(keyValue, keyRecord);
				if (TraceResourceDictionary.IsEnabled)
				{
					TraceResourceDictionary.TraceActivityItem(TraceResourceDictionary.SetKey, this, keyValue);
				}
			}
			this.NotifyOwners(new ResourcesChangeInfo(null, this));
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x00184578 File Offset: 0x00183578
		private void SetOptimizedStaticResources(IList<object> staticResources, IServiceProvider serviceProvider, StaticResourceExtension staticResourceWorker)
		{
			int i = 0;
			while (i < staticResources.Count)
			{
				OptimizedStaticResource optimizedStaticResource = staticResources[i] as OptimizedStaticResource;
				object resourceKey;
				if (optimizedStaticResource != null)
				{
					resourceKey = optimizedStaticResource.KeyValue;
					goto IL_3B;
				}
				StaticResource staticResource = staticResources[i] as StaticResource;
				if (staticResource != null)
				{
					resourceKey = this.GetStaticResourceKeyValue(staticResource, serviceProvider);
					goto IL_3B;
				}
				IL_5F:
				i++;
				continue;
				IL_3B:
				staticResourceWorker.ResourceKey = resourceKey;
				object obj = staticResourceWorker.TryProvideValueInternal(serviceProvider, true, true);
				staticResources[i] = new StaticResourceHolder(resourceKey, obj as DeferredResourceReference);
				goto IL_5F;
			}
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x001845F1 File Offset: 0x001835F1
		private Type GetTypeOfFirstObject(KeyRecord keyRecord)
		{
			return this._reader.GetTypeOfFirstStartObject(keyRecord) ?? typeof(string);
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x00184610 File Offset: 0x00183610
		private object CreateObject(KeyRecord key)
		{
			System.Xaml.XamlReader xamlReader = this._reader.ReadObject(key);
			if (xamlReader == null)
			{
				return null;
			}
			Uri baseUri = (this._rootElement is IUriContext) ? ((IUriContext)this._rootElement).BaseUri : this._baseUri;
			return WpfXamlLoader.LoadDeferredContent(xamlReader, this._objectWriterFactory, false, this._rootElement, this._objectWriterSettings, baseUri);
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x00184670 File Offset: 0x00183670
		internal object Lookup(object key, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference, bool canCacheAsThemeResource)
		{
			if (allowDeferredResourceReference)
			{
				bool flag;
				return this.FetchResource(key, allowDeferredResourceReference, mustReturnDeferredResourceReference, canCacheAsThemeResource, out flag);
			}
			if (!mustReturnDeferredResourceReference)
			{
				return this[key];
			}
			return new DeferredResourceReferenceHolder(key, this[key]);
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x001846A8 File Offset: 0x001836A8
		internal void AddOwner(DispatcherObject owner)
		{
			if (this._inheritanceContext == null)
			{
				DependencyObject dependencyObject = owner as DependencyObject;
				if (dependencyObject != null)
				{
					this._inheritanceContext = new WeakReference(dependencyObject);
					this.AddInheritanceContextToValues();
				}
				else
				{
					this._inheritanceContext = new WeakReference(ResourceDictionary.DummyInheritanceContext);
				}
			}
			FrameworkElement frameworkElement = owner as FrameworkElement;
			if (frameworkElement != null)
			{
				if (this._ownerFEs == null)
				{
					this._ownerFEs = new WeakReferenceList(1);
				}
				else if (this._ownerFEs.Contains(frameworkElement) && this.ContainsCycle(this))
				{
					throw new InvalidOperationException(SR.Get("ResourceDictionaryInvalidMergedDictionary"));
				}
				if (this.HasImplicitStyles)
				{
					frameworkElement.ShouldLookupImplicitStyles = true;
				}
				this._ownerFEs.Add(frameworkElement);
			}
			else
			{
				FrameworkContentElement frameworkContentElement = owner as FrameworkContentElement;
				if (frameworkContentElement != null)
				{
					if (this._ownerFCEs == null)
					{
						this._ownerFCEs = new WeakReferenceList(1);
					}
					else if (this._ownerFCEs.Contains(frameworkContentElement) && this.ContainsCycle(this))
					{
						throw new InvalidOperationException(SR.Get("ResourceDictionaryInvalidMergedDictionary"));
					}
					if (this.HasImplicitStyles)
					{
						frameworkContentElement.ShouldLookupImplicitStyles = true;
					}
					this._ownerFCEs.Add(frameworkContentElement);
				}
				else
				{
					Application application = owner as Application;
					if (application != null)
					{
						if (this._ownerApps == null)
						{
							this._ownerApps = new WeakReferenceList(1);
						}
						else if (this._ownerApps.Contains(application) && this.ContainsCycle(this))
						{
							throw new InvalidOperationException(SR.Get("ResourceDictionaryInvalidMergedDictionary"));
						}
						if (this.HasImplicitStyles)
						{
							application.HasImplicitStylesInResources = true;
						}
						this._ownerApps.Add(application);
						this.CanBeAccessedAcrossThreads = true;
						this.SealValues();
					}
				}
			}
			this.AddOwnerToAllMergedDictionaries(owner);
			this.TryInitialize();
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x00184844 File Offset: 0x00183844
		internal void RemoveOwner(DispatcherObject owner)
		{
			FrameworkElement frameworkElement = owner as FrameworkElement;
			if (frameworkElement != null)
			{
				if (this._ownerFEs != null)
				{
					this._ownerFEs.Remove(frameworkElement);
					if (this._ownerFEs.Count == 0)
					{
						this._ownerFEs = null;
					}
				}
			}
			else
			{
				FrameworkContentElement frameworkContentElement = owner as FrameworkContentElement;
				if (frameworkContentElement != null)
				{
					if (this._ownerFCEs != null)
					{
						this._ownerFCEs.Remove(frameworkContentElement);
						if (this._ownerFCEs.Count == 0)
						{
							this._ownerFCEs = null;
						}
					}
				}
				else
				{
					Application application = owner as Application;
					if (application != null && this._ownerApps != null)
					{
						this._ownerApps.Remove(application);
						if (this._ownerApps.Count == 0)
						{
							this._ownerApps = null;
						}
					}
				}
			}
			if (owner == this.InheritanceContext)
			{
				this.RemoveInheritanceContextFromValues();
				this._inheritanceContext = null;
			}
			this.RemoveOwnerFromAllMergedDictionaries(owner);
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x00184910 File Offset: 0x00183910
		internal bool ContainsOwner(DispatcherObject owner)
		{
			FrameworkElement frameworkElement = owner as FrameworkElement;
			if (frameworkElement != null)
			{
				return this._ownerFEs != null && this._ownerFEs.Contains(frameworkElement);
			}
			FrameworkContentElement frameworkContentElement = owner as FrameworkContentElement;
			if (frameworkContentElement != null)
			{
				return this._ownerFCEs != null && this._ownerFCEs.Contains(frameworkContentElement);
			}
			Application application = owner as Application;
			return application != null && this._ownerApps != null && this._ownerApps.Contains(application);
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x00184981 File Offset: 0x00183981
		private void TryInitialize()
		{
			if (!this.IsInitializePending && !this.IsInitialized)
			{
				this.IsInitialized = true;
			}
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x0018499C File Offset: 0x0018399C
		private void NotifyOwners(ResourcesChangeInfo info)
		{
			bool isInitialized = this.IsInitialized;
			bool flag = info.IsResourceAddOperation && this.HasImplicitStyles;
			if (isInitialized && this.InvalidatesImplicitDataTemplateResources)
			{
				info.SetIsImplicitDataTemplateChange();
			}
			if (isInitialized || flag)
			{
				if (this._ownerFEs != null)
				{
					foreach (object obj in this._ownerFEs)
					{
						FrameworkElement frameworkElement = obj as FrameworkElement;
						if (frameworkElement != null)
						{
							if (flag)
							{
								frameworkElement.ShouldLookupImplicitStyles = true;
							}
							if (isInitialized)
							{
								TreeWalkHelper.InvalidateOnResourcesChange(frameworkElement, null, info);
							}
						}
					}
				}
				if (this._ownerFCEs != null)
				{
					foreach (object obj2 in this._ownerFCEs)
					{
						FrameworkContentElement frameworkContentElement = obj2 as FrameworkContentElement;
						if (frameworkContentElement != null)
						{
							if (flag)
							{
								frameworkContentElement.ShouldLookupImplicitStyles = true;
							}
							if (isInitialized)
							{
								TreeWalkHelper.InvalidateOnResourcesChange(null, frameworkContentElement, info);
							}
						}
					}
				}
				if (this._ownerApps != null)
				{
					foreach (object obj3 in this._ownerApps)
					{
						Application application = obj3 as Application;
						if (application != null)
						{
							if (flag)
							{
								application.HasImplicitStylesInResources = true;
							}
							if (isInitialized)
							{
								application.InvalidateResourceReferences(info);
							}
						}
					}
				}
			}
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x00184AB0 File Offset: 0x00183AB0
		internal object FetchResource(object resourceKey, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference, out bool canCache)
		{
			return this.FetchResource(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference, true, out canCache);
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x00184AC0 File Offset: 0x00183AC0
		private object FetchResource(object resourceKey, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference, bool canCacheAsThemeResource, out bool canCache)
		{
			if (allowDeferredResourceReference && (this.ContainsBamlObjectFactory(resourceKey) || (mustReturnDeferredResourceReference && this.Contains(resourceKey))))
			{
				canCache = false;
				DeferredResourceReference deferredResourceReference;
				if (!this.IsThemeDictionary)
				{
					if (this._ownerApps != null)
					{
						deferredResourceReference = new DeferredAppResourceReference(this, resourceKey);
					}
					else
					{
						deferredResourceReference = new DeferredResourceReference(this, resourceKey);
					}
					if (this._deferredResourceReferences == null)
					{
						this._deferredResourceReferences = new WeakReferenceList();
					}
					this._deferredResourceReferences.Add(deferredResourceReference, true);
				}
				else
				{
					deferredResourceReference = new DeferredThemeResourceReference(this, resourceKey, canCacheAsThemeResource);
				}
				ResourceDictionaryDiagnostics.RecordLookupResult(resourceKey, this);
				return deferredResourceReference;
			}
			return this.GetValue(resourceKey, out canCache);
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x00184B4C File Offset: 0x00183B4C
		private void ValidateDeferredResourceReferences(object resourceKey)
		{
			if (this._deferredResourceReferences != null)
			{
				foreach (object obj in this._deferredResourceReferences)
				{
					DeferredResourceReference deferredResourceReference = obj as DeferredResourceReference;
					if (deferredResourceReference != null && (resourceKey == null || object.Equals(resourceKey, deferredResourceReference.Key)))
					{
						deferredResourceReference.GetValue(BaseValueSourceInternal.Unknown);
					}
				}
			}
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x00184BA4 File Offset: 0x00183BA4
		private void OnMergedDictionariesChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			List<ResourceDictionary> list = null;
			List<ResourceDictionary> list2 = null;
			ResourcesChangeInfo catastrophicDictionaryChangeInfo;
			if (e.Action != NotifyCollectionChangedAction.Reset)
			{
				Invariant.Assert((e.NewItems != null && e.NewItems.Count > 0) || (e.OldItems != null && e.OldItems.Count > 0), "The NotifyCollectionChanged event fired when no dictionaries were added or removed");
				if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
				{
					list = new List<ResourceDictionary>(e.OldItems.Count);
					for (int i = 0; i < e.OldItems.Count; i++)
					{
						ResourceDictionary resourceDictionary = (ResourceDictionary)e.OldItems[i];
						list.Add(resourceDictionary);
						this.RemoveParentOwners(resourceDictionary);
					}
				}
				if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
				{
					list2 = new List<ResourceDictionary>(e.NewItems.Count);
					for (int j = 0; j < e.NewItems.Count; j++)
					{
						ResourceDictionary resourceDictionary = (ResourceDictionary)e.NewItems[j];
						list2.Add(resourceDictionary);
						if (!this.HasImplicitStyles && resourceDictionary.HasImplicitStyles)
						{
							this.HasImplicitStyles = true;
						}
						if (!this.HasImplicitDataTemplates && resourceDictionary.HasImplicitDataTemplates)
						{
							this.HasImplicitDataTemplates = true;
						}
						if (this.IsThemeDictionary)
						{
							resourceDictionary.IsThemeDictionary = true;
						}
						this.PropagateParentOwners(resourceDictionary);
					}
				}
				catastrophicDictionaryChangeInfo = new ResourcesChangeInfo(list, list2, false, false, null);
			}
			else
			{
				catastrophicDictionaryChangeInfo = ResourcesChangeInfo.CatastrophicDictionaryChangeInfo;
			}
			this.NotifyOwners(catastrophicDictionaryChangeInfo);
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x00184D14 File Offset: 0x00183D14
		private void AddOwnerToAllMergedDictionaries(DispatcherObject owner)
		{
			if (this._mergedDictionaries != null)
			{
				for (int i = 0; i < this._mergedDictionaries.Count; i++)
				{
					this._mergedDictionaries[i].AddOwner(owner);
				}
			}
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x00184D54 File Offset: 0x00183D54
		private void RemoveOwnerFromAllMergedDictionaries(DispatcherObject owner)
		{
			if (this._mergedDictionaries != null)
			{
				for (int i = 0; i < this._mergedDictionaries.Count; i++)
				{
					this._mergedDictionaries[i].RemoveOwner(owner);
				}
			}
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x00184D94 File Offset: 0x00183D94
		private void PropagateParentOwners(ResourceDictionary mergedDictionary)
		{
			if (this._ownerFEs != null)
			{
				Invariant.Assert(this._ownerFEs.Count > 0);
				if (mergedDictionary._ownerFEs == null)
				{
					mergedDictionary._ownerFEs = new WeakReferenceList(this._ownerFEs.Count);
				}
				foreach (object obj in this._ownerFEs)
				{
					FrameworkElement frameworkElement = obj as FrameworkElement;
					if (frameworkElement != null)
					{
						mergedDictionary.AddOwner(frameworkElement);
					}
				}
			}
			if (this._ownerFCEs != null)
			{
				Invariant.Assert(this._ownerFCEs.Count > 0);
				if (mergedDictionary._ownerFCEs == null)
				{
					mergedDictionary._ownerFCEs = new WeakReferenceList(this._ownerFCEs.Count);
				}
				foreach (object obj2 in this._ownerFCEs)
				{
					FrameworkContentElement frameworkContentElement = obj2 as FrameworkContentElement;
					if (frameworkContentElement != null)
					{
						mergedDictionary.AddOwner(frameworkContentElement);
					}
				}
			}
			if (this._ownerApps != null)
			{
				Invariant.Assert(this._ownerApps.Count > 0);
				if (mergedDictionary._ownerApps == null)
				{
					mergedDictionary._ownerApps = new WeakReferenceList(this._ownerApps.Count);
				}
				foreach (object obj3 in this._ownerApps)
				{
					Application application = obj3 as Application;
					if (application != null)
					{
						mergedDictionary.AddOwner(application);
					}
				}
			}
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x00184EE8 File Offset: 0x00183EE8
		internal void RemoveParentOwners(ResourceDictionary mergedDictionary)
		{
			if (this._ownerFEs != null)
			{
				foreach (object obj in this._ownerFEs)
				{
					FrameworkElement owner = obj as FrameworkElement;
					mergedDictionary.RemoveOwner(owner);
				}
			}
			if (this._ownerFCEs != null)
			{
				Invariant.Assert(this._ownerFCEs.Count > 0);
				foreach (object obj2 in this._ownerFCEs)
				{
					FrameworkContentElement owner2 = obj2 as FrameworkContentElement;
					mergedDictionary.RemoveOwner(owner2);
				}
			}
			if (this._ownerApps != null)
			{
				Invariant.Assert(this._ownerApps.Count > 0);
				foreach (object obj3 in this._ownerApps)
				{
					Application owner3 = obj3 as Application;
					mergedDictionary.RemoveOwner(owner3);
				}
			}
		}

		// Token: 0x060024D6 RID: 9430 RVA: 0x00184FB4 File Offset: 0x00183FB4
		private bool ContainsCycle(ResourceDictionary origin)
		{
			for (int i = 0; i < this.MergedDictionaries.Count; i++)
			{
				ResourceDictionary resourceDictionary = this.MergedDictionaries[i];
				if (resourceDictionary == origin || resourceDictionary.ContainsCycle(origin))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x060024D7 RID: 9431 RVA: 0x00184FF4 File Offset: 0x00183FF4
		internal WeakReferenceList FrameworkElementOwners
		{
			get
			{
				return this._ownerFEs;
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x060024D8 RID: 9432 RVA: 0x00184FFC File Offset: 0x00183FFC
		internal WeakReferenceList FrameworkContentElementOwners
		{
			get
			{
				return this._ownerFCEs;
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x060024D9 RID: 9433 RVA: 0x00185004 File Offset: 0x00184004
		internal WeakReferenceList ApplicationOwners
		{
			get
			{
				return this._ownerApps;
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x060024DA RID: 9434 RVA: 0x0018500C File Offset: 0x0018400C
		internal WeakReferenceList DeferredResourceReferences
		{
			get
			{
				return this._deferredResourceReferences;
			}
		}

		// Token: 0x060024DB RID: 9435 RVA: 0x00185014 File Offset: 0x00184014
		private void SealValues()
		{
			int count = this._baseDictionary.Count;
			if (count > 0)
			{
				object[] array = new object[count];
				this._baseDictionary.Values.CopyTo(array, 0);
				foreach (object value in array)
				{
					this.SealValue(value);
				}
			}
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x00185068 File Offset: 0x00184068
		private void SealValue(object value)
		{
			DependencyObject inheritanceContext = this.InheritanceContext;
			if (inheritanceContext != null)
			{
				this.AddInheritanceContext(inheritanceContext, value);
			}
			if (this.IsThemeDictionary || this._ownerApps != null || this.IsReadOnly)
			{
				StyleHelper.SealIfSealable(value);
			}
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x001850A8 File Offset: 0x001840A8
		private void AddInheritanceContext(DependencyObject inheritanceContext, object value)
		{
			if (inheritanceContext.ProvideSelfAsInheritanceContext(value, VisualBrush.VisualProperty))
			{
				DependencyObject dependencyObject = value as DependencyObject;
				if (dependencyObject != null)
				{
					dependencyObject.IsInheritanceContextSealed = true;
				}
			}
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x001850D4 File Offset: 0x001840D4
		private void AddInheritanceContextToValues()
		{
			DependencyObject inheritanceContext = this.InheritanceContext;
			int count = this._baseDictionary.Count;
			if (count > 0)
			{
				object[] array = new object[count];
				this._baseDictionary.Values.CopyTo(array, 0);
				foreach (object value in array)
				{
					this.AddInheritanceContext(inheritanceContext, value);
				}
			}
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x00185134 File Offset: 0x00184134
		private void RemoveInheritanceContext(object value)
		{
			DependencyObject dependencyObject = value as DependencyObject;
			DependencyObject inheritanceContext = this.InheritanceContext;
			if (dependencyObject != null && inheritanceContext != null && dependencyObject.IsInheritanceContextSealed && dependencyObject.InheritanceContext == inheritanceContext)
			{
				dependencyObject.IsInheritanceContextSealed = false;
				inheritanceContext.RemoveSelfAsInheritanceContext(dependencyObject, VisualBrush.VisualProperty);
			}
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x0018517C File Offset: 0x0018417C
		private void RemoveInheritanceContextFromValues()
		{
			foreach (object value in this._baseDictionary.Values)
			{
				this.RemoveInheritanceContext(value);
			}
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x001851D8 File Offset: 0x001841D8
		private void UpdateHasImplicitStyles(object key)
		{
			if (!this.HasImplicitStyles)
			{
				this.HasImplicitStyles = (key as Type != null);
			}
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x001851F4 File Offset: 0x001841F4
		private void UpdateHasImplicitDataTemplates(object key)
		{
			if (!this.HasImplicitDataTemplates)
			{
				this.HasImplicitDataTemplates = (key is DataTemplateKey);
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x060024E3 RID: 9443 RVA: 0x0018520D File Offset: 0x0018420D
		private DependencyObject InheritanceContext
		{
			get
			{
				if (this._inheritanceContext == null)
				{
					return null;
				}
				return (DependencyObject)this._inheritanceContext.Target;
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x060024E4 RID: 9444 RVA: 0x00185229 File Offset: 0x00184229
		// (set) Token: 0x060024E5 RID: 9445 RVA: 0x00185232 File Offset: 0x00184232
		private bool IsInitialized
		{
			get
			{
				return this.ReadPrivateFlag(ResourceDictionary.PrivateFlags.IsInitialized);
			}
			set
			{
				this.WritePrivateFlag(ResourceDictionary.PrivateFlags.IsInitialized, value);
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x060024E6 RID: 9446 RVA: 0x0018523C File Offset: 0x0018423C
		// (set) Token: 0x060024E7 RID: 9447 RVA: 0x00185245 File Offset: 0x00184245
		private bool IsInitializePending
		{
			get
			{
				return this.ReadPrivateFlag(ResourceDictionary.PrivateFlags.IsInitializePending);
			}
			set
			{
				this.WritePrivateFlag(ResourceDictionary.PrivateFlags.IsInitializePending, value);
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x060024E8 RID: 9448 RVA: 0x0018524F File Offset: 0x0018424F
		// (set) Token: 0x060024E9 RID: 9449 RVA: 0x00185258 File Offset: 0x00184258
		private bool IsThemeDictionary
		{
			get
			{
				return this.ReadPrivateFlag(ResourceDictionary.PrivateFlags.IsThemeDictionary);
			}
			set
			{
				if (this.IsThemeDictionary != value)
				{
					this.WritePrivateFlag(ResourceDictionary.PrivateFlags.IsThemeDictionary, value);
					if (value)
					{
						this.SealValues();
					}
					if (this._mergedDictionaries != null)
					{
						for (int i = 0; i < this._mergedDictionaries.Count; i++)
						{
							this._mergedDictionaries[i].IsThemeDictionary = value;
						}
					}
				}
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x060024EA RID: 9450 RVA: 0x001852AF File Offset: 0x001842AF
		// (set) Token: 0x060024EB RID: 9451 RVA: 0x001852B9 File Offset: 0x001842B9
		internal bool HasImplicitStyles
		{
			get
			{
				return this.ReadPrivateFlag(ResourceDictionary.PrivateFlags.HasImplicitStyles);
			}
			set
			{
				this.WritePrivateFlag(ResourceDictionary.PrivateFlags.HasImplicitStyles, value);
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x060024EC RID: 9452 RVA: 0x001852C4 File Offset: 0x001842C4
		// (set) Token: 0x060024ED RID: 9453 RVA: 0x001852D1 File Offset: 0x001842D1
		internal bool HasImplicitDataTemplates
		{
			get
			{
				return this.ReadPrivateFlag(ResourceDictionary.PrivateFlags.HasImplicitDataTemplates);
			}
			set
			{
				this.WritePrivateFlag(ResourceDictionary.PrivateFlags.HasImplicitDataTemplates, value);
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x060024EE RID: 9454 RVA: 0x001852DF File Offset: 0x001842DF
		// (set) Token: 0x060024EF RID: 9455 RVA: 0x001852E9 File Offset: 0x001842E9
		internal bool CanBeAccessedAcrossThreads
		{
			get
			{
				return this.ReadPrivateFlag(ResourceDictionary.PrivateFlags.CanBeAccessedAcrossThreads);
			}
			set
			{
				this.WritePrivateFlag(ResourceDictionary.PrivateFlags.CanBeAccessedAcrossThreads, value);
			}
		}

		// Token: 0x060024F0 RID: 9456 RVA: 0x001852F4 File Offset: 0x001842F4
		private void WritePrivateFlag(ResourceDictionary.PrivateFlags bit, bool value)
		{
			if (value)
			{
				this._flags |= bit;
				return;
			}
			this._flags &= ~bit;
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x00185318 File Offset: 0x00184318
		private bool ReadPrivateFlag(ResourceDictionary.PrivateFlags bit)
		{
			return (this._flags & bit) > (ResourceDictionary.PrivateFlags)0;
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x00185325 File Offset: 0x00184325
		private void CloseReader()
		{
			this._reader.Close();
			this._reader = null;
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x0018533C File Offset: 0x0018433C
		private void CopyDeferredContentFrom(ResourceDictionary loadedRD)
		{
			this._buffer = loadedRD._buffer;
			this._bamlStream = loadedRD._bamlStream;
			this._startPosition = loadedRD._startPosition;
			this._contentSize = loadedRD._contentSize;
			this._objectWriterFactory = loadedRD._objectWriterFactory;
			this._objectWriterSettings = loadedRD._objectWriterSettings;
			this._rootElement = loadedRD._rootElement;
			this._reader = loadedRD._reader;
			this._numDefer = loadedRD._numDefer;
			this._deferredLocationList = loadedRD._deferredLocationList;
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x001853C4 File Offset: 0x001843C4
		private void MoveDeferredResourceReferencesFrom(ResourceDictionary loadedRD)
		{
			this._deferredResourceReferences = loadedRD._deferredResourceReferences;
			if (this._deferredResourceReferences != null)
			{
				foreach (object obj in this._deferredResourceReferences)
				{
					((DeferredResourceReference)obj).Dictionary = this;
				}
			}
		}

		// Token: 0x0400114F RID: 4431
		internal bool IsSourcedFromThemeDictionary;

		// Token: 0x04001150 RID: 4432
		private ResourceDictionary.FallbackState _fallbackState;

		// Token: 0x04001151 RID: 4433
		private Hashtable _baseDictionary;

		// Token: 0x04001152 RID: 4434
		private WeakReferenceList _ownerFEs;

		// Token: 0x04001153 RID: 4435
		private WeakReferenceList _ownerFCEs;

		// Token: 0x04001154 RID: 4436
		private WeakReferenceList _ownerApps;

		// Token: 0x04001155 RID: 4437
		private WeakReferenceList _deferredResourceReferences;

		// Token: 0x04001156 RID: 4438
		private ObservableCollection<ResourceDictionary> _mergedDictionaries;

		// Token: 0x04001157 RID: 4439
		private Uri _source;

		// Token: 0x04001158 RID: 4440
		private Uri _baseUri;

		// Token: 0x04001159 RID: 4441
		private ResourceDictionary.PrivateFlags _flags;

		// Token: 0x0400115A RID: 4442
		private List<KeyRecord> _deferredLocationList;

		// Token: 0x0400115B RID: 4443
		private byte[] _buffer;

		// Token: 0x0400115C RID: 4444
		private Stream _bamlStream;

		// Token: 0x0400115D RID: 4445
		private long _startPosition;

		// Token: 0x0400115E RID: 4446
		private int _contentSize;

		// Token: 0x0400115F RID: 4447
		private object _rootElement;

		// Token: 0x04001160 RID: 4448
		private int _numDefer;

		// Token: 0x04001161 RID: 4449
		private WeakReference _inheritanceContext;

		// Token: 0x04001162 RID: 4450
		private static readonly DependencyObject DummyInheritanceContext = new DependencyObject();

		// Token: 0x04001163 RID: 4451
		private XamlObjectIds _contextXamlObjectIds = new XamlObjectIds();

		// Token: 0x04001164 RID: 4452
		private IXamlObjectWriterFactory _objectWriterFactory;

		// Token: 0x04001165 RID: 4453
		private XamlObjectWriterSettings _objectWriterSettings;

		// Token: 0x04001166 RID: 4454
		private Baml2006Reader _reader;

		// Token: 0x02000A85 RID: 2693
		private class ResourceDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x06008670 RID: 34416 RVA: 0x0032A7E5 File Offset: 0x003297E5
			internal ResourceDictionaryEnumerator(ResourceDictionary owner)
			{
				this._owner = owner;
				this._keysEnumerator = this._owner.Keys.GetEnumerator();
			}

			// Token: 0x17001E1C RID: 7708
			// (get) Token: 0x06008671 RID: 34417 RVA: 0x0032A80A File Offset: 0x0032980A
			object IEnumerator.Current
			{
				get
				{
					return ((IDictionaryEnumerator)this).Entry;
				}
			}

			// Token: 0x06008672 RID: 34418 RVA: 0x0032A817 File Offset: 0x00329817
			bool IEnumerator.MoveNext()
			{
				return this._keysEnumerator.MoveNext();
			}

			// Token: 0x06008673 RID: 34419 RVA: 0x0032A824 File Offset: 0x00329824
			void IEnumerator.Reset()
			{
				this._keysEnumerator.Reset();
			}

			// Token: 0x17001E1D RID: 7709
			// (get) Token: 0x06008674 RID: 34420 RVA: 0x0032A834 File Offset: 0x00329834
			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					object key = this._keysEnumerator.Current;
					object value = this._owner[key];
					return new DictionaryEntry(key, value);
				}
			}

			// Token: 0x17001E1E RID: 7710
			// (get) Token: 0x06008675 RID: 34421 RVA: 0x0032A861 File Offset: 0x00329861
			object IDictionaryEnumerator.Key
			{
				get
				{
					return this._keysEnumerator.Current;
				}
			}

			// Token: 0x17001E1F RID: 7711
			// (get) Token: 0x06008676 RID: 34422 RVA: 0x0032A86E File Offset: 0x0032986E
			object IDictionaryEnumerator.Value
			{
				get
				{
					return this._owner[this._keysEnumerator.Current];
				}
			}

			// Token: 0x0400419B RID: 16795
			private ResourceDictionary _owner;

			// Token: 0x0400419C RID: 16796
			private IEnumerator _keysEnumerator;
		}

		// Token: 0x02000A86 RID: 2694
		private class ResourceValuesEnumerator : IEnumerator
		{
			// Token: 0x06008677 RID: 34423 RVA: 0x0032A886 File Offset: 0x00329886
			internal ResourceValuesEnumerator(ResourceDictionary owner)
			{
				this._owner = owner;
				this._keysEnumerator = this._owner.Keys.GetEnumerator();
			}

			// Token: 0x17001E20 RID: 7712
			// (get) Token: 0x06008678 RID: 34424 RVA: 0x0032A8AB File Offset: 0x003298AB
			object IEnumerator.Current
			{
				get
				{
					return this._owner[this._keysEnumerator.Current];
				}
			}

			// Token: 0x06008679 RID: 34425 RVA: 0x0032A8C3 File Offset: 0x003298C3
			bool IEnumerator.MoveNext()
			{
				return this._keysEnumerator.MoveNext();
			}

			// Token: 0x0600867A RID: 34426 RVA: 0x0032A8D0 File Offset: 0x003298D0
			void IEnumerator.Reset()
			{
				this._keysEnumerator.Reset();
			}

			// Token: 0x0400419D RID: 16797
			private ResourceDictionary _owner;

			// Token: 0x0400419E RID: 16798
			private IEnumerator _keysEnumerator;
		}

		// Token: 0x02000A87 RID: 2695
		private class ResourceValuesCollection : ICollection, IEnumerable
		{
			// Token: 0x0600867B RID: 34427 RVA: 0x0032A8DD File Offset: 0x003298DD
			internal ResourceValuesCollection(ResourceDictionary owner)
			{
				this._owner = owner;
			}

			// Token: 0x17001E21 RID: 7713
			// (get) Token: 0x0600867C RID: 34428 RVA: 0x0032A8EC File Offset: 0x003298EC
			int ICollection.Count
			{
				get
				{
					return this._owner.Count;
				}
			}

			// Token: 0x17001E22 RID: 7714
			// (get) Token: 0x0600867D RID: 34429 RVA: 0x00105F35 File Offset: 0x00104F35
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001E23 RID: 7715
			// (get) Token: 0x0600867E RID: 34430 RVA: 0x000F93D3 File Offset: 0x000F83D3
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x0600867F RID: 34431 RVA: 0x0032A8FC File Offset: 0x003298FC
			void ICollection.CopyTo(Array array, int index)
			{
				foreach (object key in this._owner.Keys)
				{
					array.SetValue(this._owner[key], index++);
				}
			}

			// Token: 0x06008680 RID: 34432 RVA: 0x0032A968 File Offset: 0x00329968
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new ResourceDictionary.ResourceValuesEnumerator(this._owner);
			}

			// Token: 0x0400419F RID: 16799
			private ResourceDictionary _owner;
		}

		// Token: 0x02000A88 RID: 2696
		private enum PrivateFlags : byte
		{
			// Token: 0x040041A1 RID: 16801
			IsInitialized = 1,
			// Token: 0x040041A2 RID: 16802
			IsInitializePending,
			// Token: 0x040041A3 RID: 16803
			IsReadOnly = 4,
			// Token: 0x040041A4 RID: 16804
			IsThemeDictionary = 8,
			// Token: 0x040041A5 RID: 16805
			HasImplicitStyles = 16,
			// Token: 0x040041A6 RID: 16806
			CanBeAccessedAcrossThreads = 32,
			// Token: 0x040041A7 RID: 16807
			InvalidatesImplicitDataTemplateResources = 64,
			// Token: 0x040041A8 RID: 16808
			HasImplicitDataTemplates = 128
		}

		// Token: 0x02000A89 RID: 2697
		internal class ResourceDictionarySourceUriWrapper : Uri
		{
			// Token: 0x06008681 RID: 34433 RVA: 0x0032A975 File Offset: 0x00329975
			public ResourceDictionarySourceUriWrapper(Uri originalUri, Uri versionedUri) : base(originalUri.OriginalString, UriKind.RelativeOrAbsolute)
			{
				this.OriginalUri = originalUri;
				this.VersionedUri = versionedUri;
			}

			// Token: 0x17001E24 RID: 7716
			// (get) Token: 0x06008682 RID: 34434 RVA: 0x0032A992 File Offset: 0x00329992
			// (set) Token: 0x06008683 RID: 34435 RVA: 0x0032A99A File Offset: 0x0032999A
			internal Uri OriginalUri { get; set; }

			// Token: 0x17001E25 RID: 7717
			// (get) Token: 0x06008684 RID: 34436 RVA: 0x0032A9A3 File Offset: 0x003299A3
			// (set) Token: 0x06008685 RID: 34437 RVA: 0x0032A9AB File Offset: 0x003299AB
			internal Uri VersionedUri { get; set; }
		}

		// Token: 0x02000A8A RID: 2698
		private enum FallbackState
		{
			// Token: 0x040041AC RID: 16812
			Classic,
			// Token: 0x040041AD RID: 16813
			Generic,
			// Token: 0x040041AE RID: 16814
			None
		}
	}
}
