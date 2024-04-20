using System;
using System.Windows.Controls;
using MS.Internal.Annotations;
using MS.Internal.Annotations.Component;

namespace System.Windows.Annotations
{
	// Token: 0x02000869 RID: 2153
	internal sealed class AnnotationComponentChooser
	{
		// Token: 0x06007EFB RID: 32507 RVA: 0x0031BC38 File Offset: 0x0031AC38
		public IAnnotationComponent ChooseAnnotationComponent(IAttachedAnnotation attachedAnnotation)
		{
			if (attachedAnnotation == null)
			{
				throw new ArgumentNullException("attachedAnnotation");
			}
			IAnnotationComponent result = null;
			if (attachedAnnotation.Annotation.AnnotationType == StickyNoteControl.TextSchemaName)
			{
				result = new StickyNoteControl(StickyNoteType.Text);
			}
			else if (attachedAnnotation.Annotation.AnnotationType == StickyNoteControl.InkSchemaName)
			{
				result = new StickyNoteControl(StickyNoteType.Ink);
			}
			else if (attachedAnnotation.Annotation.AnnotationType == HighlightComponent.TypeName)
			{
				result = new HighlightComponent();
			}
			return result;
		}
	}
}
