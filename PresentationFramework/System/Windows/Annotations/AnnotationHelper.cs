using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using MS.Internal;
using MS.Internal.Annotations;
using MS.Internal.Annotations.Anchoring;
using MS.Internal.Annotations.Component;
using MS.Utility;

namespace System.Windows.Annotations
{
	// Token: 0x0200086B RID: 2155
	public static class AnnotationHelper
	{
		// Token: 0x06007F0F RID: 32527 RVA: 0x0031C258 File Offset: 0x0031B258
		public static Annotation CreateHighlightForSelection(AnnotationService service, string author, Brush highlightBrush)
		{
			Annotation annotation = null;
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.CreateHighlightBegin);
			try
			{
				annotation = AnnotationHelper.Highlight(service, author, highlightBrush, true);
				Invariant.Assert(annotation != null, "Highlight not returned from create call.");
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.CreateHighlightEnd);
			}
			return annotation;
		}

		// Token: 0x06007F10 RID: 32528 RVA: 0x0031C2AC File Offset: 0x0031B2AC
		public static Annotation CreateTextStickyNoteForSelection(AnnotationService service, string author)
		{
			return AnnotationHelper.CreateStickyNoteForSelection(service, StickyNoteControl.TextSchemaName, author);
		}

		// Token: 0x06007F11 RID: 32529 RVA: 0x0031C2BA File Offset: 0x0031B2BA
		public static Annotation CreateInkStickyNoteForSelection(AnnotationService service, string author)
		{
			return AnnotationHelper.CreateStickyNoteForSelection(service, StickyNoteControl.InkSchemaName, author);
		}

		// Token: 0x06007F12 RID: 32530 RVA: 0x0031C2C8 File Offset: 0x0031B2C8
		public static void ClearHighlightsForSelection(AnnotationService service)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.ClearHighlightBegin);
			try
			{
				AnnotationHelper.Highlight(service, null, null, false);
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.ClearHighlightEnd);
			}
		}

		// Token: 0x06007F13 RID: 32531 RVA: 0x0031C30C File Offset: 0x0031B30C
		public static void DeleteTextStickyNotesForSelection(AnnotationService service)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.DeleteTextNoteBegin);
			try
			{
				AnnotationHelper.DeleteSpannedAnnotations(service, StickyNoteControl.TextSchemaName);
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.DeleteTextNoteEnd);
			}
		}

		// Token: 0x06007F14 RID: 32532 RVA: 0x0031C350 File Offset: 0x0031B350
		public static void DeleteInkStickyNotesForSelection(AnnotationService service)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.DeleteInkNoteBegin);
			try
			{
				AnnotationHelper.DeleteSpannedAnnotations(service, StickyNoteControl.InkSchemaName);
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.DeleteInkNoteEnd);
			}
		}

		// Token: 0x06007F15 RID: 32533 RVA: 0x0031C394 File Offset: 0x0031B394
		public static IAnchorInfo GetAnchorInfo(AnnotationService service, Annotation annotation)
		{
			AnnotationHelper.CheckInputs(service);
			if (annotation == null)
			{
				throw new ArgumentNullException("annotation");
			}
			bool flag = true;
			DocumentViewerBase documentViewerBase = service.Root as DocumentViewerBase;
			if (documentViewerBase == null)
			{
				FlowDocumentReader flowDocumentReader = service.Root as FlowDocumentReader;
				if (flowDocumentReader != null)
				{
					documentViewerBase = (AnnotationHelper.GetFdrHost(flowDocumentReader) as DocumentViewerBase);
				}
			}
			else
			{
				flag = (documentViewerBase.Document is FlowDocument);
			}
			IList<IAttachedAnnotation> list = null;
			if (flag)
			{
				TextSelectionProcessor textSelectionProcessor = service.LocatorManager.GetSelectionProcessor(typeof(TextRange)) as TextSelectionProcessor;
				TextSelectionProcessor textSelectionProcessor2 = service.LocatorManager.GetSelectionProcessor(typeof(TextAnchor)) as TextSelectionProcessor;
				Invariant.Assert(textSelectionProcessor != null, "TextSelectionProcessor should be available for TextRange if we are processing flow content.");
				Invariant.Assert(textSelectionProcessor2 != null, "TextSelectionProcessor should be available for TextAnchor if we are processing flow content.");
				try
				{
					textSelectionProcessor.Clamping = false;
					textSelectionProcessor2.Clamping = false;
					list = AnnotationHelper.ResolveAnnotations(service, new Annotation[]
					{
						annotation
					});
					goto IL_12E;
				}
				finally
				{
					textSelectionProcessor.Clamping = true;
					textSelectionProcessor2.Clamping = true;
				}
			}
			FixedPageProcessor fixedPageProcessor = service.LocatorManager.GetSubTreeProcessorForLocatorPart(FixedPageProcessor.CreateLocatorPart(0)) as FixedPageProcessor;
			Invariant.Assert(fixedPageProcessor != null, "FixedPageProcessor should be available if we are processing fixed content.");
			try
			{
				fixedPageProcessor.UseLogicalTree = true;
				list = AnnotationHelper.ResolveAnnotations(service, new Annotation[]
				{
					annotation
				});
			}
			finally
			{
				fixedPageProcessor.UseLogicalTree = false;
			}
			IL_12E:
			Invariant.Assert(list != null);
			if (list.Count > 0)
			{
				return list[0];
			}
			return null;
		}

		// Token: 0x06007F16 RID: 32534 RVA: 0x0031C508 File Offset: 0x0031B508
		internal static void OnCreateHighlightCommand(object sender, ExecutedRoutedEventArgs e)
		{
			DependencyObject dependencyObject = sender as DependencyObject;
			if (dependencyObject != null)
			{
				AnnotationHelper.CreateHighlightForSelection(AnnotationService.GetService(dependencyObject), null, (e.Parameter != null) ? (e.Parameter as Brush) : null);
			}
		}

		// Token: 0x06007F17 RID: 32535 RVA: 0x0031C544 File Offset: 0x0031B544
		internal static void OnCreateTextStickyNoteCommand(object sender, ExecutedRoutedEventArgs e)
		{
			DependencyObject dependencyObject = sender as DependencyObject;
			if (dependencyObject != null)
			{
				AnnotationHelper.CreateTextStickyNoteForSelection(AnnotationService.GetService(dependencyObject), e.Parameter as string);
			}
		}

		// Token: 0x06007F18 RID: 32536 RVA: 0x0031C574 File Offset: 0x0031B574
		internal static void OnCreateInkStickyNoteCommand(object sender, ExecutedRoutedEventArgs e)
		{
			DependencyObject dependencyObject = sender as DependencyObject;
			if (dependencyObject != null)
			{
				AnnotationHelper.CreateInkStickyNoteForSelection(AnnotationService.GetService(dependencyObject), e.Parameter as string);
			}
		}

		// Token: 0x06007F19 RID: 32537 RVA: 0x0031C5A4 File Offset: 0x0031B5A4
		internal static void OnClearHighlightsCommand(object sender, ExecutedRoutedEventArgs e)
		{
			DependencyObject dependencyObject = sender as DependencyObject;
			if (dependencyObject != null)
			{
				AnnotationHelper.ClearHighlightsForSelection(AnnotationService.GetService(dependencyObject));
			}
		}

		// Token: 0x06007F1A RID: 32538 RVA: 0x0031C5C8 File Offset: 0x0031B5C8
		internal static void OnDeleteStickyNotesCommand(object sender, ExecutedRoutedEventArgs e)
		{
			DependencyObject dependencyObject = sender as DependencyObject;
			if (dependencyObject != null)
			{
				AnnotationHelper.DeleteTextStickyNotesForSelection(AnnotationService.GetService(dependencyObject));
				AnnotationHelper.DeleteInkStickyNotesForSelection(AnnotationService.GetService(dependencyObject));
			}
		}

		// Token: 0x06007F1B RID: 32539 RVA: 0x0031C5F8 File Offset: 0x0031B5F8
		internal static void OnDeleteAnnotationsCommand(object sender, ExecutedRoutedEventArgs e)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement != null)
			{
				ITextSelection textSelection = AnnotationHelper.GetTextSelection(frameworkElement);
				if (textSelection != null)
				{
					AnnotationService service = AnnotationService.GetService(frameworkElement);
					AnnotationHelper.DeleteTextStickyNotesForSelection(service);
					AnnotationHelper.DeleteInkStickyNotesForSelection(service);
					if (!textSelection.IsEmpty)
					{
						AnnotationHelper.ClearHighlightsForSelection(service);
					}
				}
			}
		}

		// Token: 0x06007F1C RID: 32540 RVA: 0x0031C63A File Offset: 0x0031B63A
		internal static void OnQueryCreateHighlightCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = AnnotationHelper.IsCommandEnabled(sender, true);
			e.Handled = true;
		}

		// Token: 0x06007F1D RID: 32541 RVA: 0x0031C63A File Offset: 0x0031B63A
		internal static void OnQueryCreateTextStickyNoteCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = AnnotationHelper.IsCommandEnabled(sender, true);
			e.Handled = true;
		}

		// Token: 0x06007F1E RID: 32542 RVA: 0x0031C63A File Offset: 0x0031B63A
		internal static void OnQueryCreateInkStickyNoteCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = AnnotationHelper.IsCommandEnabled(sender, true);
			e.Handled = true;
		}

		// Token: 0x06007F1F RID: 32543 RVA: 0x0031C63A File Offset: 0x0031B63A
		internal static void OnQueryClearHighlightsCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = AnnotationHelper.IsCommandEnabled(sender, true);
			e.Handled = true;
		}

		// Token: 0x06007F20 RID: 32544 RVA: 0x0031C650 File Offset: 0x0031B650
		internal static void OnQueryDeleteStickyNotesCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = AnnotationHelper.IsCommandEnabled(sender, false);
			e.Handled = true;
		}

		// Token: 0x06007F21 RID: 32545 RVA: 0x0031C650 File Offset: 0x0031B650
		internal static void OnQueryDeleteAnnotationsCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = AnnotationHelper.IsCommandEnabled(sender, false);
			e.Handled = true;
		}

		// Token: 0x06007F22 RID: 32546 RVA: 0x0031C668 File Offset: 0x0031B668
		internal static DocumentPageView FindView(DocumentViewerBase viewer, int pageNb)
		{
			Invariant.Assert(viewer != null, "viewer is null");
			Invariant.Assert(pageNb >= 0, "negative pageNb");
			foreach (DocumentPageView documentPageView in viewer.PageViews)
			{
				if (documentPageView.PageNumber == pageNb)
				{
					return documentPageView;
				}
			}
			return null;
		}

		// Token: 0x06007F23 RID: 32547 RVA: 0x0031C6E0 File Offset: 0x0031B6E0
		private static Annotation CreateStickyNoteForSelection(AnnotationService service, XmlQualifiedName noteType, string author)
		{
			AnnotationHelper.CheckInputs(service);
			ITextSelection textSelection = AnnotationHelper.GetTextSelection((FrameworkElement)service.Root);
			Invariant.Assert(textSelection != null, "TextSelection is null");
			if (textSelection.IsEmpty)
			{
				throw new InvalidOperationException(SR.Get("EmptySelectionNotSupported"));
			}
			Annotation annotation = null;
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.CreateStickyNoteBegin);
			try
			{
				annotation = AnnotationHelper.CreateAnnotationForSelection(service, textSelection, noteType, author);
				Invariant.Assert(annotation != null, "CreateAnnotationForSelection returned null.");
				service.Store.AddAnnotation(annotation);
				textSelection.SetCaretToPosition(textSelection.MovingPosition, textSelection.MovingPosition.LogicalDirection, true, true);
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.CreateStickyNoteEnd);
			}
			return annotation;
		}

		// Token: 0x06007F24 RID: 32548 RVA: 0x0031C798 File Offset: 0x0031B798
		private static bool AreAllPagesVisible(DocumentViewerBase viewer, int startPage, int endPage)
		{
			Invariant.Assert(viewer != null, "viewer is null.");
			Invariant.Assert(endPage >= startPage, "EndPage is less than StartPage");
			bool result = true;
			if (viewer.PageViews.Count <= endPage - startPage)
			{
				return false;
			}
			for (int i = startPage; i <= endPage; i++)
			{
				if (AnnotationHelper.FindView(viewer, i) == null)
				{
					result = false;
					break;
				}
			}
			return result;
		}

		// Token: 0x06007F25 RID: 32549 RVA: 0x0031C7F4 File Offset: 0x0031B7F4
		private static IList<IAttachedAnnotation> GetSpannedAnnotations(AnnotationService service)
		{
			AnnotationHelper.CheckInputs(service);
			bool flag = true;
			DocumentViewerBase documentViewerBase = service.Root as DocumentViewerBase;
			if (documentViewerBase == null)
			{
				FlowDocumentReader flowDocumentReader = service.Root as FlowDocumentReader;
				if (flowDocumentReader != null)
				{
					documentViewerBase = (AnnotationHelper.GetFdrHost(flowDocumentReader) as DocumentViewerBase);
				}
			}
			else
			{
				flag = (documentViewerBase.Document is FlowDocument);
			}
			bool flag2 = true;
			ITextSelection textSelection = AnnotationHelper.GetTextSelection((FrameworkElement)service.Root);
			Invariant.Assert(textSelection != null, "TextSelection is null");
			int num = 0;
			int num2 = 0;
			if (documentViewerBase != null)
			{
				TextSelectionHelper.GetPointerPage(textSelection.Start, out num);
				TextSelectionHelper.GetPointerPage(textSelection.End, out num2);
				if (num == -1 || num2 == -1)
				{
					throw new ArgumentException(SR.Get("InvalidSelectionPages"));
				}
				flag2 = AnnotationHelper.AreAllPagesVisible(documentViewerBase, num, num2);
			}
			IList<IAttachedAnnotation> list;
			if (flag2)
			{
				list = service.GetAttachedAnnotations();
			}
			else if (flag)
			{
				list = AnnotationHelper.GetSpannedAnnotationsForFlow(service, textSelection);
			}
			else
			{
				list = AnnotationHelper.GetSpannedAnnotationsForFixed(service, num, num2);
			}
			IList<TextSegment> textSegments = textSelection.TextSegments;
			if (list != null && list.Count > 0 && (flag2 || !flag))
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					TextAnchor textAnchor = list[i].AttachedAnchor as TextAnchor;
					if (textAnchor == null || !textAnchor.IsOverlapping(textSegments))
					{
						list.RemoveAt(i);
					}
				}
			}
			return list;
		}

		// Token: 0x06007F26 RID: 32550 RVA: 0x0031C940 File Offset: 0x0031B940
		internal static object GetFdrHost(FlowDocumentReader fdr)
		{
			Invariant.Assert(fdr != null, "Null FDR");
			Decorator decorator = null;
			if (fdr.TemplateInternal != null)
			{
				decorator = (StyleHelper.FindNameInTemplateContent(fdr, "PART_ContentHost", fdr.TemplateInternal) as Decorator);
			}
			if (decorator == null)
			{
				return null;
			}
			return decorator.Child;
		}

		// Token: 0x06007F27 RID: 32551 RVA: 0x0031C988 File Offset: 0x0031B988
		private static IList<IAttachedAnnotation> GetSpannedAnnotationsForFlow(AnnotationService service, ITextSelection selection)
		{
			Invariant.Assert(service != null);
			ITextPointer textPointer = selection.Start.CreatePointer();
			ITextPointer textPointer2 = selection.End.CreatePointer();
			textPointer.MoveToNextInsertionPosition(LogicalDirection.Backward);
			textPointer2.MoveToNextInsertionPosition(LogicalDirection.Forward);
			ITextRange selection2 = new TextRange(textPointer, textPointer2);
			IList<ContentLocatorBase> list = service.LocatorManager.GenerateLocators(selection2);
			Invariant.Assert(list != null && list.Count > 0);
			TextSelectionProcessor textSelectionProcessor = service.LocatorManager.GetSelectionProcessor(typeof(TextRange)) as TextSelectionProcessor;
			TextSelectionProcessor textSelectionProcessor2 = service.LocatorManager.GetSelectionProcessor(typeof(TextAnchor)) as TextSelectionProcessor;
			Invariant.Assert(textSelectionProcessor != null, "TextSelectionProcessor should be available for TextRange if we are processing flow content.");
			Invariant.Assert(textSelectionProcessor2 != null, "TextSelectionProcessor should be available for TextAnchor if we are processing flow content.");
			IList<IAttachedAnnotation> result = null;
			try
			{
				textSelectionProcessor.Clamping = false;
				textSelectionProcessor2.Clamping = false;
				ContentLocator contentLocator = list[0] as ContentLocator;
				Invariant.Assert(contentLocator != null, "Locators for selection in Flow should always be ContentLocators.  ContentLocatorSets not supported.");
				contentLocator.Parts[contentLocator.Parts.Count - 1].NameValuePairs.Add("IncludeOverlaps", bool.TrueString);
				IList<Annotation> annotations = service.Store.GetAnnotations(contentLocator);
				result = AnnotationHelper.ResolveAnnotations(service, annotations);
			}
			finally
			{
				textSelectionProcessor.Clamping = true;
				textSelectionProcessor2.Clamping = true;
			}
			return result;
		}

		// Token: 0x06007F28 RID: 32552 RVA: 0x0031CAE0 File Offset: 0x0031BAE0
		private static IList<IAttachedAnnotation> GetSpannedAnnotationsForFixed(AnnotationService service, int startPage, int endPage)
		{
			Invariant.Assert(service != null, "Need non-null service to get spanned annotations for fixed content.");
			FixedPageProcessor fixedPageProcessor = service.LocatorManager.GetSubTreeProcessorForLocatorPart(FixedPageProcessor.CreateLocatorPart(0)) as FixedPageProcessor;
			Invariant.Assert(fixedPageProcessor != null, "FixedPageProcessor should be available if we are processing fixed content.");
			List<IAttachedAnnotation> result = null;
			List<Annotation> annotations = new List<Annotation>();
			try
			{
				fixedPageProcessor.UseLogicalTree = true;
				for (int i = startPage; i <= endPage; i++)
				{
					ContentLocator contentLocator = new ContentLocator();
					contentLocator.Parts.Add(FixedPageProcessor.CreateLocatorPart(i));
					AnnotationHelper.AddRange(annotations, service.Store.GetAnnotations(contentLocator));
				}
				result = AnnotationHelper.ResolveAnnotations(service, annotations);
			}
			finally
			{
				fixedPageProcessor.UseLogicalTree = false;
			}
			return result;
		}

		// Token: 0x06007F29 RID: 32553 RVA: 0x0031CB8C File Offset: 0x0031BB8C
		private static void AddRange(List<Annotation> annotations, IList<Annotation> newAnnotations)
		{
			Invariant.Assert(annotations != null && newAnnotations != null, "annotations or newAnnotations array is null");
			foreach (Annotation annotation in newAnnotations)
			{
				bool flag = true;
				using (List<Annotation>.Enumerator enumerator2 = annotations.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Id.Equals(annotation.Id))
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					annotations.Add(annotation);
				}
			}
		}

		// Token: 0x06007F2A RID: 32554 RVA: 0x0031CC3C File Offset: 0x0031BC3C
		private static List<IAttachedAnnotation> ResolveAnnotations(AnnotationService service, IList<Annotation> annotations)
		{
			Invariant.Assert(annotations != null);
			List<IAttachedAnnotation> list = new List<IAttachedAnnotation>(annotations.Count);
			foreach (Annotation annotation in annotations)
			{
				AttachmentLevel attachmentLevel;
				object obj = service.LocatorManager.ResolveLocator(annotation.Anchors[0].ContentLocators[0], 0, service.Root, out attachmentLevel);
				if (attachmentLevel != AttachmentLevel.Incomplete && attachmentLevel != AttachmentLevel.Unresolved && obj != null)
				{
					list.Add(new AttachedAnnotation(service.LocatorManager, annotation, annotation.Anchors[0], obj, attachmentLevel));
				}
			}
			return list;
		}

		// Token: 0x06007F2B RID: 32555 RVA: 0x0031CCF0 File Offset: 0x0031BCF0
		private static void DeleteSpannedAnnotations(AnnotationService service, XmlQualifiedName annotationType)
		{
			AnnotationHelper.CheckInputs(service);
			Invariant.Assert(annotationType != null && (annotationType == HighlightComponent.TypeName || annotationType == StickyNoteControl.TextSchemaName || annotationType == StickyNoteControl.InkSchemaName), "Invalid Annotation Type");
			ITextSelection textSelection = AnnotationHelper.GetTextSelection((FrameworkElement)service.Root);
			Invariant.Assert(textSelection != null, "TextSelection is null");
			foreach (IAttachedAnnotation attachedAnnotation in AnnotationHelper.GetSpannedAnnotations(service))
			{
				if (annotationType.Equals(attachedAnnotation.Annotation.AnnotationType))
				{
					TextAnchor textAnchor = attachedAnnotation.AttachedAnchor as TextAnchor;
					if (textAnchor != null && ((textSelection.Start.CompareTo(textAnchor.Start) > 0 && textSelection.Start.CompareTo(textAnchor.End) < 0) || (textSelection.End.CompareTo(textAnchor.Start) > 0 && textSelection.End.CompareTo(textAnchor.End) < 0) || (textSelection.Start.CompareTo(textAnchor.Start) <= 0 && textSelection.End.CompareTo(textAnchor.End) >= 0) || AnnotationHelper.CheckCaret(textSelection, textAnchor, annotationType)))
					{
						service.Store.DeleteAnnotation(attachedAnnotation.Annotation.Id);
					}
				}
			}
		}

		// Token: 0x06007F2C RID: 32556 RVA: 0x0031CE60 File Offset: 0x0031BE60
		private static bool CheckCaret(ITextSelection selection, TextAnchor anchor, XmlQualifiedName type)
		{
			return selection.IsEmpty && ((anchor.Start.CompareTo(selection.Start) == 0 && selection.Start.LogicalDirection == LogicalDirection.Forward) || (anchor.End.CompareTo(selection.End) == 0 && selection.End.LogicalDirection == LogicalDirection.Backward));
		}

		// Token: 0x06007F2D RID: 32557 RVA: 0x0031CEBC File Offset: 0x0031BEBC
		private static Annotation CreateAnnotationForSelection(AnnotationService service, ITextRange textSelection, XmlQualifiedName annotationType, string author)
		{
			Invariant.Assert(service != null && textSelection != null, "Parameter 'service' or 'textSelection' is null.");
			Invariant.Assert(annotationType != null && (annotationType == HighlightComponent.TypeName || annotationType == StickyNoteControl.TextSchemaName || annotationType == StickyNoteControl.InkSchemaName), "Invalid Annotation Type");
			Annotation annotation = new Annotation(annotationType);
			AnnotationHelper.SetAnchor(service, annotation, textSelection);
			if (author != null)
			{
				annotation.Authors.Add(author);
			}
			return annotation;
		}

		// Token: 0x06007F2E RID: 32558 RVA: 0x0031CF3C File Offset: 0x0031BF3C
		private static Annotation Highlight(AnnotationService service, string author, Brush highlightBrush, bool create)
		{
			AnnotationHelper.CheckInputs(service);
			ITextSelection textSelection = AnnotationHelper.GetTextSelection((FrameworkElement)service.Root);
			Invariant.Assert(textSelection != null, "TextSelection is null");
			if (textSelection.IsEmpty)
			{
				throw new InvalidOperationException(SR.Get("EmptySelectionNotSupported"));
			}
			Color? color = null;
			if (highlightBrush != null)
			{
				SolidColorBrush solidColorBrush = highlightBrush as SolidColorBrush;
				if (solidColorBrush == null)
				{
					throw new ArgumentException(SR.Get("InvalidHighlightColor"), "highlightBrush");
				}
				byte a;
				if (solidColorBrush.Opacity <= 0.0)
				{
					a = 0;
				}
				else if (solidColorBrush.Opacity >= 1.0)
				{
					a = solidColorBrush.Color.A;
				}
				else
				{
					a = (byte)(solidColorBrush.Opacity * (double)solidColorBrush.Color.A);
				}
				color = new Color?(Color.FromArgb(a, solidColorBrush.Color.R, solidColorBrush.Color.G, solidColorBrush.Color.B));
			}
			ITextRange textRange = new TextRange(textSelection.Start, textSelection.End);
			Annotation result = AnnotationHelper.ProcessHighlights(service, textRange, author, color, create);
			textSelection.SetCaretToPosition(textSelection.MovingPosition, textSelection.MovingPosition.LogicalDirection, true, true);
			return result;
		}

		// Token: 0x06007F2F RID: 32559 RVA: 0x0031D078 File Offset: 0x0031C078
		private static Annotation ProcessHighlights(AnnotationService service, ITextRange textRange, string author, Color? color, bool create)
		{
			Invariant.Assert(textRange != null, "Parameter 'textRange' is null.");
			foreach (IAttachedAnnotation attachedAnnotation in AnnotationHelper.GetSpannedAnnotations(service))
			{
				if (HighlightComponent.TypeName.Equals(attachedAnnotation.Annotation.AnnotationType))
				{
					TextAnchor textAnchor = attachedAnnotation.FullyAttachedAnchor as TextAnchor;
					Invariant.Assert(textAnchor != null, "FullyAttachedAnchor must not be null.");
					TextAnchor textAnchor2 = new TextAnchor(textAnchor);
					textAnchor2 = TextAnchor.TrimToRelativeComplement(textAnchor2, textRange.TextSegments);
					if (textAnchor2 == null || textAnchor2.IsEmpty)
					{
						service.Store.DeleteAnnotation(attachedAnnotation.Annotation.Id);
					}
					else
					{
						AnnotationHelper.SetAnchor(service, attachedAnnotation.Annotation, textAnchor2);
					}
				}
			}
			if (create)
			{
				Annotation annotation = AnnotationHelper.CreateHighlight(service, textRange, author, color);
				service.Store.AddAnnotation(annotation);
				return annotation;
			}
			return null;
		}

		// Token: 0x06007F30 RID: 32560 RVA: 0x0031D160 File Offset: 0x0031C160
		private static Annotation CreateHighlight(AnnotationService service, ITextRange textRange, string author, Color? color)
		{
			Invariant.Assert(textRange != null, "textRange is null");
			Annotation annotation = AnnotationHelper.CreateAnnotationForSelection(service, textRange, HighlightComponent.TypeName, author);
			if (color != null)
			{
				ColorConverter colorConverter = new ColorConverter();
				XmlElement xmlElement = new XmlDocument().CreateElement("Colors", "http://schemas.microsoft.com/windows/annotations/2003/11/base");
				xmlElement.SetAttribute("Background", colorConverter.ConvertToInvariantString(color.Value));
				AnnotationResource annotationResource = new AnnotationResource("Highlight");
				annotationResource.Contents.Add(xmlElement);
				annotation.Cargos.Add(annotationResource);
			}
			return annotation;
		}

		// Token: 0x06007F31 RID: 32561 RVA: 0x0031D1F0 File Offset: 0x0031C1F0
		private static ITextSelection GetTextSelection(FrameworkElement viewer)
		{
			FlowDocumentReader flowDocumentReader = viewer as FlowDocumentReader;
			if (flowDocumentReader != null)
			{
				viewer = (AnnotationHelper.GetFdrHost(flowDocumentReader) as FrameworkElement);
			}
			if (viewer != null)
			{
				return TextEditor.GetTextSelection(viewer);
			}
			return null;
		}

		// Token: 0x06007F32 RID: 32562 RVA: 0x0031D220 File Offset: 0x0031C220
		private static void SetAnchor(AnnotationService service, Annotation annot, object selection)
		{
			Invariant.Assert(annot != null && selection != null, "null input parameter");
			IList<ContentLocatorBase> list = service.LocatorManager.GenerateLocators(selection);
			Invariant.Assert(list != null && list.Count > 0, "No locators generated for selection.");
			AnnotationResource annotationResource = new AnnotationResource();
			foreach (ContentLocatorBase item in list)
			{
				annotationResource.ContentLocators.Add(item);
			}
			annot.Anchors.Clear();
			annot.Anchors.Add(annotationResource);
		}

		// Token: 0x06007F33 RID: 32563 RVA: 0x0031D2C4 File Offset: 0x0031C2C4
		private static void CheckInputs(AnnotationService service)
		{
			if (service == null)
			{
				throw new ArgumentNullException("service");
			}
			if (!service.IsEnabled)
			{
				throw new ArgumentException(SR.Get("AnnotationServiceNotEnabled"), "service");
			}
			DocumentViewerBase documentViewerBase = service.Root as DocumentViewerBase;
			if (documentViewerBase == null)
			{
				bool flag = service.Root is FlowDocumentScrollViewer;
				FlowDocumentReader flowDocumentReader = service.Root as FlowDocumentReader;
				Invariant.Assert(flag || flowDocumentReader != null, "Service's Root must be either a FlowDocumentReader, DocumentViewerBase or a FlowDocumentScrollViewer.");
				return;
			}
			if (documentViewerBase.Document == null)
			{
				throw new InvalidOperationException(SR.Get("OnlyFlowFixedSupported"));
			}
		}

		// Token: 0x06007F34 RID: 32564 RVA: 0x0031D350 File Offset: 0x0031C350
		private static bool IsCommandEnabled(object sender, bool checkForEmpty)
		{
			Invariant.Assert(sender != null, "Parameter 'sender' is null.");
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement != null)
			{
				FrameworkElement frameworkElement2 = frameworkElement.Parent as FrameworkElement;
				AnnotationService service = AnnotationService.GetService(frameworkElement);
				if (service != null && service.IsEnabled && (service.Root == frameworkElement || (frameworkElement2 != null && service.Root == frameworkElement2.TemplatedParent)))
				{
					ITextSelection textSelection = AnnotationHelper.GetTextSelection(frameworkElement);
					if (textSelection != null)
					{
						return !checkForEmpty || !textSelection.IsEmpty;
					}
				}
			}
			return false;
		}
	}
}
