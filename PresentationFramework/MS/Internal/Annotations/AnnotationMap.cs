using System;
using System.Collections.Generic;

namespace MS.Internal.Annotations
{
	// Token: 0x020002B7 RID: 695
	internal class AnnotationMap
	{
		// Token: 0x06001A0E RID: 6670 RVA: 0x00162E54 File Offset: 0x00161E54
		internal void AddAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			List<IAttachedAnnotation> list = null;
			if (!this._annotationIdToAttachedAnnotations.TryGetValue(attachedAnnotation.Annotation.Id, out list))
			{
				list = new List<IAttachedAnnotation>(1);
				this._annotationIdToAttachedAnnotations.Add(attachedAnnotation.Annotation.Id, list);
			}
			list.Add(attachedAnnotation);
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x00162EA4 File Offset: 0x00161EA4
		internal void RemoveAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			List<IAttachedAnnotation> list = null;
			if (this._annotationIdToAttachedAnnotations.TryGetValue(attachedAnnotation.Annotation.Id, out list))
			{
				list.Remove(attachedAnnotation);
				if (list.Count == 0)
				{
					this._annotationIdToAttachedAnnotations.Remove(attachedAnnotation.Annotation.Id);
				}
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001A10 RID: 6672 RVA: 0x00162EF4 File Offset: 0x00161EF4
		internal bool IsEmpty
		{
			get
			{
				return this._annotationIdToAttachedAnnotations.Count == 0;
			}
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x00162F04 File Offset: 0x00161F04
		internal List<IAttachedAnnotation> GetAttachedAnnotations(Guid annotationId)
		{
			List<IAttachedAnnotation> result = null;
			if (!this._annotationIdToAttachedAnnotations.TryGetValue(annotationId, out result))
			{
				return AnnotationMap._emptyList;
			}
			return result;
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x00162F2C File Offset: 0x00161F2C
		internal List<IAttachedAnnotation> GetAllAttachedAnnotations()
		{
			List<IAttachedAnnotation> list = new List<IAttachedAnnotation>(this._annotationIdToAttachedAnnotations.Keys.Count);
			foreach (Guid key in this._annotationIdToAttachedAnnotations.Keys)
			{
				List<IAttachedAnnotation> collection = this._annotationIdToAttachedAnnotations[key];
				list.AddRange(collection);
			}
			if (list.Count == 0)
			{
				return AnnotationMap._emptyList;
			}
			return list;
		}

		// Token: 0x04000D98 RID: 3480
		private Dictionary<Guid, List<IAttachedAnnotation>> _annotationIdToAttachedAnnotations = new Dictionary<Guid, List<IAttachedAnnotation>>();

		// Token: 0x04000D99 RID: 3481
		private static readonly List<IAttachedAnnotation> _emptyList = new List<IAttachedAnnotation>(0);
	}
}
