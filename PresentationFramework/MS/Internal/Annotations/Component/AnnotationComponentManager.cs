using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Documents;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020002C8 RID: 712
	internal class AnnotationComponentManager : DependencyObject
	{
		// Token: 0x06001A90 RID: 6800 RVA: 0x00164755 File Offset: 0x00163755
		internal AnnotationComponentManager(AnnotationService service)
		{
			if (service != null)
			{
				service.AttachedAnnotationChanged += this.AttachedAnnotationUpdateEventHandler;
			}
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x00164780 File Offset: 0x00163780
		internal void AddAttachedAnnotation(IAttachedAnnotation attachedAnnotation, bool reorder)
		{
			IAnnotationComponent annotationComponent = this.FindComponent(attachedAnnotation);
			if (annotationComponent == null)
			{
				return;
			}
			this.AddComponent(attachedAnnotation, annotationComponent, reorder);
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x001647A4 File Offset: 0x001637A4
		internal void RemoveAttachedAnnotation(IAttachedAnnotation attachedAnnotation, bool reorder)
		{
			if (!this._attachedAnnotations.ContainsKey(attachedAnnotation))
			{
				return;
			}
			IEnumerable<IAnnotationComponent> enumerable = this._attachedAnnotations[attachedAnnotation];
			this._attachedAnnotations.Remove(attachedAnnotation);
			foreach (IAnnotationComponent annotationComponent in enumerable)
			{
				annotationComponent.RemoveAttachedAnnotation(attachedAnnotation);
				if (annotationComponent.AttachedAnnotations.Count == 0 && annotationComponent.PresentationContext != null)
				{
					annotationComponent.PresentationContext.RemoveFromHost(annotationComponent, reorder);
				}
			}
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x00164838 File Offset: 0x00163838
		private void AttachedAnnotationUpdateEventHandler(object sender, AttachedAnnotationChangedEventArgs e)
		{
			switch (e.Action)
			{
			case AttachedAnnotationAction.Loaded:
				this.AddAttachedAnnotation(e.AttachedAnnotation, false);
				return;
			case AttachedAnnotationAction.Unloaded:
				this.RemoveAttachedAnnotation(e.AttachedAnnotation, false);
				return;
			case AttachedAnnotationAction.AnchorModified:
				this.ModifyAttachedAnnotation(e.AttachedAnnotation, e.PreviousAttachedAnchor, e.PreviousAttachmentLevel);
				return;
			case AttachedAnnotationAction.Added:
				this.AddAttachedAnnotation(e.AttachedAnnotation, true);
				return;
			case AttachedAnnotationAction.Deleted:
				this.RemoveAttachedAnnotation(e.AttachedAnnotation, true);
				return;
			default:
				return;
			}
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x001648B7 File Offset: 0x001638B7
		private IAnnotationComponent FindComponent(IAttachedAnnotation attachedAnnotation)
		{
			return AnnotationService.GetChooser(attachedAnnotation.Parent as UIElement).ChooseAnnotationComponent(attachedAnnotation);
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x001648D0 File Offset: 0x001638D0
		private void AddComponent(IAttachedAnnotation attachedAnnotation, IAnnotationComponent component, bool reorder)
		{
			UIElement uielement = attachedAnnotation.Parent as UIElement;
			if (component.PresentationContext != null)
			{
				return;
			}
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(uielement);
			if (adornerLayer != null)
			{
				this.AddToAttachedAnnotations(attachedAnnotation, component);
				component.AddAttachedAnnotation(attachedAnnotation);
				AdornerPresentationContext.HostComponent(adornerLayer, component, uielement, reorder);
				return;
			}
			if (PresentationSource.FromVisual(uielement) == null)
			{
				return;
			}
			throw new InvalidOperationException(SR.Get("NoPresentationContextForGivenElement", new object[]
			{
				uielement
			}));
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x00164938 File Offset: 0x00163938
		private void ModifyAttachedAnnotation(IAttachedAnnotation attachedAnnotation, object previousAttachedAnchor, AttachmentLevel previousAttachmentLevel)
		{
			if (!this._attachedAnnotations.ContainsKey(attachedAnnotation))
			{
				this.AddAttachedAnnotation(attachedAnnotation, true);
				return;
			}
			IAnnotationComponent annotationComponent = this.FindComponent(attachedAnnotation);
			if (annotationComponent == null)
			{
				this.RemoveAttachedAnnotation(attachedAnnotation, true);
				return;
			}
			IList<IAnnotationComponent> list = this._attachedAnnotations[attachedAnnotation];
			if (list.Contains(annotationComponent))
			{
				using (IEnumerator<IAnnotationComponent> enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IAnnotationComponent annotationComponent2 = enumerator.Current;
						annotationComponent2.ModifyAttachedAnnotation(attachedAnnotation, previousAttachedAnchor, previousAttachmentLevel);
						if (annotationComponent2.AttachedAnnotations.Count == 0)
						{
							annotationComponent2.PresentationContext.RemoveFromHost(annotationComponent2, true);
						}
					}
					return;
				}
			}
			this.RemoveAttachedAnnotation(attachedAnnotation, true);
			this.AddComponent(attachedAnnotation, annotationComponent, true);
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x001649F0 File Offset: 0x001639F0
		private void AddToAttachedAnnotations(IAttachedAnnotation attachedAnnotation, IAnnotationComponent component)
		{
			IList<IAnnotationComponent> list;
			if (!this._attachedAnnotations.TryGetValue(attachedAnnotation, out list))
			{
				list = new List<IAnnotationComponent>();
				this._attachedAnnotations[attachedAnnotation] = list;
			}
			list.Add(component);
		}

		// Token: 0x04000DBE RID: 3518
		private Dictionary<IAttachedAnnotation, IList<IAnnotationComponent>> _attachedAnnotations = new Dictionary<IAttachedAnnotation, IList<IAnnotationComponent>>();
	}
}
