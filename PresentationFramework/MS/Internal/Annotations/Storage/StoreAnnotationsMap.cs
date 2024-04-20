using System;
using System.Collections.Generic;
using System.Windows.Annotations;

namespace MS.Internal.Annotations.Storage
{
	// Token: 0x020002C5 RID: 709
	internal class StoreAnnotationsMap
	{
		// Token: 0x06001A61 RID: 6753 RVA: 0x00163A3E File Offset: 0x00162A3E
		internal StoreAnnotationsMap(AnnotationAuthorChangedEventHandler authorChanged, AnnotationResourceChangedEventHandler anchorChanged, AnnotationResourceChangedEventHandler cargoChanged)
		{
			this._authorChanged = authorChanged;
			this._anchorChanged = anchorChanged;
			this._cargoChanged = cargoChanged;
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x00163A68 File Offset: 0x00162A68
		public void AddAnnotation(Annotation annotation, bool dirty)
		{
			annotation.AuthorChanged += this.OnAuthorChanged;
			annotation.AnchorChanged += this.OnAnchorChanged;
			annotation.CargoChanged += this.OnCargoChanged;
			this._currentAnnotations.Add(annotation.Id, new StoreAnnotationsMap.CachedAnnotation(annotation, dirty));
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x00163AC4 File Offset: 0x00162AC4
		public void RemoveAnnotation(Guid id)
		{
			StoreAnnotationsMap.CachedAnnotation cachedAnnotation = null;
			if (this._currentAnnotations.TryGetValue(id, out cachedAnnotation))
			{
				cachedAnnotation.Annotation.AuthorChanged -= this.OnAuthorChanged;
				cachedAnnotation.Annotation.AnchorChanged -= this.OnAnchorChanged;
				cachedAnnotation.Annotation.CargoChanged -= this.OnCargoChanged;
				this._currentAnnotations.Remove(id);
			}
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x00163B38 File Offset: 0x00162B38
		public Dictionary<Guid, Annotation> FindAnnotations(ContentLocator anchorLocator)
		{
			if (anchorLocator == null)
			{
				throw new ArgumentNullException("locator");
			}
			Dictionary<Guid, Annotation> dictionary = new Dictionary<Guid, Annotation>();
			foreach (StoreAnnotationsMap.CachedAnnotation cachedAnnotation in this._currentAnnotations.Values)
			{
				Annotation annotation = cachedAnnotation.Annotation;
				bool flag = false;
				foreach (AnnotationResource annotationResource in annotation.Anchors)
				{
					foreach (ContentLocatorBase contentLocatorBase in annotationResource.ContentLocators)
					{
						ContentLocator contentLocator = contentLocatorBase as ContentLocator;
						if (contentLocator != null)
						{
							if (contentLocator.StartsWith(anchorLocator))
							{
								flag = true;
							}
						}
						else
						{
							ContentLocatorGroup contentLocatorGroup = contentLocatorBase as ContentLocatorGroup;
							if (contentLocatorGroup != null)
							{
								using (IEnumerator<ContentLocator> enumerator4 = contentLocatorGroup.Locators.GetEnumerator())
								{
									while (enumerator4.MoveNext())
									{
										if (enumerator4.Current.StartsWith(anchorLocator))
										{
											flag = true;
											break;
										}
									}
								}
							}
						}
						if (flag)
						{
							dictionary.Add(annotation.Id, annotation);
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x00163C90 File Offset: 0x00162C90
		public Dictionary<Guid, Annotation> FindAnnotations()
		{
			Dictionary<Guid, Annotation> dictionary = new Dictionary<Guid, Annotation>();
			foreach (KeyValuePair<Guid, StoreAnnotationsMap.CachedAnnotation> keyValuePair in this._currentAnnotations)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value.Annotation);
			}
			return dictionary;
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x00163CFC File Offset: 0x00162CFC
		public Annotation FindAnnotation(Guid id)
		{
			StoreAnnotationsMap.CachedAnnotation cachedAnnotation = null;
			if (this._currentAnnotations.TryGetValue(id, out cachedAnnotation))
			{
				return cachedAnnotation.Annotation;
			}
			return null;
		}

		// Token: 0x06001A67 RID: 6759 RVA: 0x00163D24 File Offset: 0x00162D24
		public List<Annotation> FindDirtyAnnotations()
		{
			List<Annotation> list = new List<Annotation>();
			foreach (KeyValuePair<Guid, StoreAnnotationsMap.CachedAnnotation> keyValuePair in this._currentAnnotations)
			{
				if (keyValuePair.Value.Dirty)
				{
					list.Add(keyValuePair.Value.Annotation);
				}
			}
			return list;
		}

		// Token: 0x06001A68 RID: 6760 RVA: 0x00163D98 File Offset: 0x00162D98
		public void ValidateDirtyAnnotations()
		{
			foreach (KeyValuePair<Guid, StoreAnnotationsMap.CachedAnnotation> keyValuePair in this._currentAnnotations)
			{
				if (keyValuePair.Value.Dirty)
				{
					keyValuePair.Value.Dirty = false;
				}
			}
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x00163E00 File Offset: 0x00162E00
		private void OnAnchorChanged(object sender, AnnotationResourceChangedEventArgs args)
		{
			this._currentAnnotations[args.Annotation.Id].Dirty = true;
			this._anchorChanged(sender, args);
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x00163E2B File Offset: 0x00162E2B
		private void OnCargoChanged(object sender, AnnotationResourceChangedEventArgs args)
		{
			this._currentAnnotations[args.Annotation.Id].Dirty = true;
			this._cargoChanged(sender, args);
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x00163E56 File Offset: 0x00162E56
		private void OnAuthorChanged(object sender, AnnotationAuthorChangedEventArgs args)
		{
			this._currentAnnotations[args.Annotation.Id].Dirty = true;
			this._authorChanged(sender, args);
		}

		// Token: 0x04000DB4 RID: 3508
		private Dictionary<Guid, StoreAnnotationsMap.CachedAnnotation> _currentAnnotations = new Dictionary<Guid, StoreAnnotationsMap.CachedAnnotation>();

		// Token: 0x04000DB5 RID: 3509
		private AnnotationAuthorChangedEventHandler _authorChanged;

		// Token: 0x04000DB6 RID: 3510
		private AnnotationResourceChangedEventHandler _anchorChanged;

		// Token: 0x04000DB7 RID: 3511
		private AnnotationResourceChangedEventHandler _cargoChanged;

		// Token: 0x02000A15 RID: 2581
		private class CachedAnnotation
		{
			// Token: 0x060084C9 RID: 33993 RVA: 0x00326EFC File Offset: 0x00325EFC
			public CachedAnnotation(Annotation annotation, bool dirty)
			{
				this.Annotation = annotation;
				this.Dirty = dirty;
			}

			// Token: 0x17001DD7 RID: 7639
			// (get) Token: 0x060084CA RID: 33994 RVA: 0x00326F12 File Offset: 0x00325F12
			// (set) Token: 0x060084CB RID: 33995 RVA: 0x00326F1A File Offset: 0x00325F1A
			public Annotation Annotation
			{
				get
				{
					return this._annotation;
				}
				set
				{
					this._annotation = value;
				}
			}

			// Token: 0x17001DD8 RID: 7640
			// (get) Token: 0x060084CC RID: 33996 RVA: 0x00326F23 File Offset: 0x00325F23
			// (set) Token: 0x060084CD RID: 33997 RVA: 0x00326F2B File Offset: 0x00325F2B
			public bool Dirty
			{
				get
				{
					return this._dirty;
				}
				set
				{
					this._dirty = value;
				}
			}

			// Token: 0x04004093 RID: 16531
			private Annotation _annotation;

			// Token: 0x04004094 RID: 16532
			private bool _dirty;
		}
	}
}
