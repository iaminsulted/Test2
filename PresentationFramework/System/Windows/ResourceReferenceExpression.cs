using System;
using System.ComponentModel;
using System.Windows.Markup;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x02000391 RID: 913
	[TypeConverter(typeof(ResourceReferenceExpressionConverter))]
	internal class ResourceReferenceExpression : Expression
	{
		// Token: 0x060024FC RID: 9468 RVA: 0x00185486 File Offset: 0x00184486
		public ResourceReferenceExpression(object resourceKey)
		{
			this._resourceKey = resourceKey;
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x00109403 File Offset: 0x00108403
		internal override DependencySource[] GetSources()
		{
			return null;
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x00185498 File Offset: 0x00184498
		internal override object GetValue(DependencyObject d, DependencyProperty dp)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			if (dp == null)
			{
				throw new ArgumentNullException("dp");
			}
			if (this.ReadInternalState(ResourceReferenceExpression.InternalState.HasCachedResourceValue))
			{
				return this._cachedResourceValue;
			}
			object obj;
			return this.GetRawValue(d, out obj, dp);
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x001854DB File Offset: 0x001844DB
		internal override Expression Copy(DependencyObject targetObject, DependencyProperty targetDP)
		{
			return new ResourceReferenceExpression(this.ResourceKey);
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x001854E8 File Offset: 0x001844E8
		internal object GetRawValue(DependencyObject d, out object source, DependencyProperty dp)
		{
			if (!this.ReadInternalState(ResourceReferenceExpression.InternalState.IsMentorCacheValid))
			{
				this._mentorCache = Helper.FindMentor(d);
				this.WriteInternalState(ResourceReferenceExpression.InternalState.IsMentorCacheValid, true);
				if (this._mentorCache != null && this._mentorCache != this._targetObject)
				{
					FrameworkElement frameworkElement;
					FrameworkContentElement frameworkContentElement;
					Helper.DowncastToFEorFCE(this._mentorCache, out frameworkElement, out frameworkContentElement, true);
					if (frameworkElement != null)
					{
						frameworkElement.ResourcesChanged += this.InvalidateExpressionValue;
					}
					else
					{
						frameworkContentElement.ResourcesChanged += this.InvalidateExpressionValue;
					}
				}
			}
			object obj;
			if (this._mentorCache != null)
			{
				FrameworkElement fe;
				FrameworkContentElement fce;
				Helper.DowncastToFEorFCE(this._mentorCache, out fe, out fce, true);
				obj = FrameworkElement.FindResourceInternal(fe, fce, dp, this._resourceKey, null, true, false, null, false, out source);
			}
			else
			{
				obj = FrameworkElement.FindResourceFromAppOrSystem(this._resourceKey, out source, false, true, false);
			}
			if (obj == null)
			{
				obj = DependencyProperty.UnsetValue;
			}
			this._cachedResourceValue = obj;
			this.WriteInternalState(ResourceReferenceExpression.InternalState.HasCachedResourceValue, true);
			object resource = obj;
			DeferredResourceReference deferredResourceReference = obj as DeferredResourceReference;
			if (deferredResourceReference != null)
			{
				if (deferredResourceReference.IsInflated)
				{
					resource = (deferredResourceReference.Value as Freezable);
				}
				else if (!this.ReadInternalState(ResourceReferenceExpression.InternalState.IsListeningForInflated))
				{
					deferredResourceReference.AddInflatedListener(this);
					this.WriteInternalState(ResourceReferenceExpression.InternalState.IsListeningForInflated, true);
				}
			}
			this.ListenForFreezableChanges(resource);
			return obj;
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x00105F35 File Offset: 0x00104F35
		internal override bool SetValue(DependencyObject d, DependencyProperty dp, object value)
		{
			return false;
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x00185600 File Offset: 0x00184600
		internal override void OnAttach(DependencyObject d, DependencyProperty dp)
		{
			this._targetObject = d;
			this._targetProperty = dp;
			FrameworkObject frameworkObject = new FrameworkObject(this._targetObject);
			frameworkObject.HasResourceReference = true;
			if (!frameworkObject.IsValid)
			{
				this._targetObject.InheritanceContextChanged += this.InvalidateExpressionValue;
			}
		}

		// Token: 0x06002503 RID: 9475 RVA: 0x00185650 File Offset: 0x00184650
		internal override void OnDetach(DependencyObject d, DependencyProperty dp)
		{
			this.InvalidateMentorCache();
			if (!(this._targetObject is FrameworkElement) && !(this._targetObject is FrameworkContentElement))
			{
				this._targetObject.InheritanceContextChanged -= this.InvalidateExpressionValue;
			}
			this._targetObject = null;
			this._targetProperty = null;
			this._weakContainerRRE = null;
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06002504 RID: 9476 RVA: 0x001856A9 File Offset: 0x001846A9
		public object ResourceKey
		{
			get
			{
				return this._resourceKey;
			}
		}

		// Token: 0x06002505 RID: 9477 RVA: 0x001856B4 File Offset: 0x001846B4
		private void InvalidateCacheValue()
		{
			object resource = this._cachedResourceValue;
			DeferredResourceReference deferredResourceReference = this._cachedResourceValue as DeferredResourceReference;
			if (deferredResourceReference != null)
			{
				if (deferredResourceReference.IsInflated)
				{
					resource = deferredResourceReference.Value;
				}
				else if (this.ReadInternalState(ResourceReferenceExpression.InternalState.IsListeningForInflated))
				{
					deferredResourceReference.RemoveInflatedListener(this);
					this.WriteInternalState(ResourceReferenceExpression.InternalState.IsListeningForInflated, false);
				}
				deferredResourceReference.RemoveFromDictionary();
			}
			this.StopListeningForFreezableChanges(resource);
			this._cachedResourceValue = null;
			this.WriteInternalState(ResourceReferenceExpression.InternalState.HasCachedResourceValue, false);
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x00185720 File Offset: 0x00184720
		private void InvalidateMentorCache()
		{
			if (this.ReadInternalState(ResourceReferenceExpression.InternalState.IsMentorCacheValid))
			{
				if (this._mentorCache != null)
				{
					if (this._mentorCache != this._targetObject)
					{
						FrameworkElement frameworkElement;
						FrameworkContentElement frameworkContentElement;
						Helper.DowncastToFEorFCE(this._mentorCache, out frameworkElement, out frameworkContentElement, true);
						if (frameworkElement != null)
						{
							frameworkElement.ResourcesChanged -= this.InvalidateExpressionValue;
						}
						else
						{
							frameworkContentElement.ResourcesChanged -= this.InvalidateExpressionValue;
						}
					}
					this._mentorCache = null;
				}
				this.WriteInternalState(ResourceReferenceExpression.InternalState.IsMentorCacheValid, false);
			}
			this.InvalidateCacheValue();
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x0018579C File Offset: 0x0018479C
		internal void InvalidateExpressionValue(object sender, EventArgs e)
		{
			if (this._targetObject == null)
			{
				return;
			}
			ResourcesChangedEventArgs resourcesChangedEventArgs = e as ResourcesChangedEventArgs;
			if (resourcesChangedEventArgs != null)
			{
				if (!resourcesChangedEventArgs.Info.IsTreeChange)
				{
					this.InvalidateCacheValue();
				}
				else
				{
					this.InvalidateMentorCache();
				}
			}
			else
			{
				this.InvalidateMentorCache();
			}
			this.InvalidateTargetProperty(sender, e);
		}

		// Token: 0x06002508 RID: 9480 RVA: 0x001857EA File Offset: 0x001847EA
		private void InvalidateTargetProperty(object sender, EventArgs e)
		{
			this._targetObject.InvalidateProperty(this._targetProperty);
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x001857FD File Offset: 0x001847FD
		private void InvalidateTargetSubProperty(object sender, EventArgs e)
		{
			this._targetObject.NotifySubPropertyChange(this._targetProperty);
		}

		// Token: 0x0600250A RID: 9482 RVA: 0x00185810 File Offset: 0x00184810
		private void ListenForFreezableChanges(object resource)
		{
			if (!this.ReadInternalState(ResourceReferenceExpression.InternalState.IsListeningForFreezableChanges))
			{
				Freezable freezable = resource as Freezable;
				if (freezable != null && !freezable.IsFrozen)
				{
					if (this._weakContainerRRE == null)
					{
						this._weakContainerRRE = new ResourceReferenceExpression.ResourceReferenceExpressionWeakContainer(this);
					}
					this._weakContainerRRE.AddChangedHandler(freezable);
					this.WriteInternalState(ResourceReferenceExpression.InternalState.IsListeningForFreezableChanges, true);
				}
			}
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x00185860 File Offset: 0x00184860
		private void StopListeningForFreezableChanges(object resource)
		{
			if (this.ReadInternalState(ResourceReferenceExpression.InternalState.IsListeningForFreezableChanges))
			{
				Freezable freezable = resource as Freezable;
				if (freezable != null && this._weakContainerRRE != null)
				{
					if (!freezable.IsFrozen)
					{
						this._weakContainerRRE.RemoveChangedHandler();
					}
					else
					{
						this._weakContainerRRE = null;
					}
				}
				this.WriteInternalState(ResourceReferenceExpression.InternalState.IsListeningForFreezableChanges, false);
			}
		}

		// Token: 0x0600250C RID: 9484 RVA: 0x001858AC File Offset: 0x001848AC
		internal void OnDeferredResourceInflated(DeferredResourceReference deferredResourceReference)
		{
			if (this.ReadInternalState(ResourceReferenceExpression.InternalState.IsListeningForInflated))
			{
				deferredResourceReference.RemoveInflatedListener(this);
				this.WriteInternalState(ResourceReferenceExpression.InternalState.IsListeningForInflated, false);
			}
			this.ListenForFreezableChanges(deferredResourceReference.Value);
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x001858D4 File Offset: 0x001848D4
		private bool ReadInternalState(ResourceReferenceExpression.InternalState reqFlag)
		{
			return (this._state & reqFlag) > ResourceReferenceExpression.InternalState.Default;
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x001858E1 File Offset: 0x001848E1
		private void WriteInternalState(ResourceReferenceExpression.InternalState reqFlag, bool set)
		{
			if (set)
			{
				this._state |= reqFlag;
				return;
			}
			this._state &= ~reqFlag;
		}

		// Token: 0x0400116C RID: 4460
		private object _resourceKey;

		// Token: 0x0400116D RID: 4461
		private object _cachedResourceValue;

		// Token: 0x0400116E RID: 4462
		private DependencyObject _mentorCache;

		// Token: 0x0400116F RID: 4463
		private DependencyObject _targetObject;

		// Token: 0x04001170 RID: 4464
		private DependencyProperty _targetProperty;

		// Token: 0x04001171 RID: 4465
		private ResourceReferenceExpression.InternalState _state;

		// Token: 0x04001172 RID: 4466
		private ResourceReferenceExpression.ResourceReferenceExpressionWeakContainer _weakContainerRRE;

		// Token: 0x02000A8B RID: 2699
		[Flags]
		private enum InternalState : byte
		{
			// Token: 0x040041B0 RID: 16816
			Default = 0,
			// Token: 0x040041B1 RID: 16817
			HasCachedResourceValue = 1,
			// Token: 0x040041B2 RID: 16818
			IsMentorCacheValid = 2,
			// Token: 0x040041B3 RID: 16819
			DisableThrowOnResourceFailure = 4,
			// Token: 0x040041B4 RID: 16820
			IsListeningForFreezableChanges = 8,
			// Token: 0x040041B5 RID: 16821
			IsListeningForInflated = 16
		}

		// Token: 0x02000A8C RID: 2700
		private class ResourceReferenceExpressionWeakContainer : WeakReference
		{
			// Token: 0x06008686 RID: 34438 RVA: 0x0032A9B4 File Offset: 0x003299B4
			public ResourceReferenceExpressionWeakContainer(ResourceReferenceExpression target) : base(target)
			{
			}

			// Token: 0x06008687 RID: 34439 RVA: 0x0032A9C0 File Offset: 0x003299C0
			private void InvalidateTargetSubProperty(object sender, EventArgs args)
			{
				ResourceReferenceExpression resourceReferenceExpression = (ResourceReferenceExpression)this.Target;
				if (resourceReferenceExpression != null)
				{
					resourceReferenceExpression.InvalidateTargetSubProperty(sender, args);
					return;
				}
				this.RemoveChangedHandler();
			}

			// Token: 0x06008688 RID: 34440 RVA: 0x0032A9EB File Offset: 0x003299EB
			public void AddChangedHandler(Freezable resource)
			{
				if (this._resource != null)
				{
					this.RemoveChangedHandler();
				}
				this._resource = resource;
				this._resource.Changed += this.InvalidateTargetSubProperty;
			}

			// Token: 0x06008689 RID: 34441 RVA: 0x0032AA19 File Offset: 0x00329A19
			public void RemoveChangedHandler()
			{
				if (!this._resource.IsFrozen)
				{
					this._resource.Changed -= this.InvalidateTargetSubProperty;
					this._resource = null;
				}
			}

			// Token: 0x040041B6 RID: 16822
			private Freezable _resource;
		}
	}
}
