using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Annotations.Storage;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Annotations;
using MS.Internal.Annotations.Anchoring;
using MS.Internal.Annotations.Component;
using MS.Utility;

namespace System.Windows.Annotations
{
	// Token: 0x0200086F RID: 2159
	public sealed class AnnotationService : DispatcherObject
	{
		// Token: 0x06007F54 RID: 32596 RVA: 0x0031D940 File Offset: 0x0031C940
		static AnnotationService()
		{
			CommandManager.RegisterClassCommandBinding(typeof(DocumentViewerBase), new CommandBinding(AnnotationService.CreateHighlightCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnCreateHighlightCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryCreateHighlightCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(DocumentViewerBase), new CommandBinding(AnnotationService.CreateTextStickyNoteCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnCreateTextStickyNoteCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryCreateTextStickyNoteCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(DocumentViewerBase), new CommandBinding(AnnotationService.CreateInkStickyNoteCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnCreateInkStickyNoteCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryCreateInkStickyNoteCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(DocumentViewerBase), new CommandBinding(AnnotationService.ClearHighlightsCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnClearHighlightsCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryClearHighlightsCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(DocumentViewerBase), new CommandBinding(AnnotationService.DeleteStickyNotesCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnDeleteStickyNotesCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryDeleteStickyNotesCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(DocumentViewerBase), new CommandBinding(AnnotationService.DeleteAnnotationsCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnDeleteAnnotationsCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryDeleteAnnotationsCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentScrollViewer), new CommandBinding(AnnotationService.CreateHighlightCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnCreateHighlightCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryCreateHighlightCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentScrollViewer), new CommandBinding(AnnotationService.CreateTextStickyNoteCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnCreateTextStickyNoteCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryCreateTextStickyNoteCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentScrollViewer), new CommandBinding(AnnotationService.CreateInkStickyNoteCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnCreateInkStickyNoteCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryCreateInkStickyNoteCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentScrollViewer), new CommandBinding(AnnotationService.ClearHighlightsCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnClearHighlightsCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryClearHighlightsCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentScrollViewer), new CommandBinding(AnnotationService.DeleteStickyNotesCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnDeleteStickyNotesCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryDeleteStickyNotesCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentScrollViewer), new CommandBinding(AnnotationService.DeleteAnnotationsCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnDeleteAnnotationsCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryDeleteAnnotationsCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentReader), new CommandBinding(AnnotationService.CreateHighlightCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnCreateHighlightCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryCreateHighlightCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentReader), new CommandBinding(AnnotationService.CreateTextStickyNoteCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnCreateTextStickyNoteCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryCreateTextStickyNoteCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentReader), new CommandBinding(AnnotationService.CreateInkStickyNoteCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnCreateInkStickyNoteCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryCreateInkStickyNoteCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentReader), new CommandBinding(AnnotationService.ClearHighlightsCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnClearHighlightsCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryClearHighlightsCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentReader), new CommandBinding(AnnotationService.DeleteStickyNotesCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnDeleteStickyNotesCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryDeleteStickyNotesCommand)));
			CommandManager.RegisterClassCommandBinding(typeof(FlowDocumentReader), new CommandBinding(AnnotationService.DeleteAnnotationsCommand, new ExecutedRoutedEventHandler(AnnotationHelper.OnDeleteAnnotationsCommand), new CanExecuteRoutedEventHandler(AnnotationHelper.OnQueryDeleteAnnotationsCommand)));
		}

		// Token: 0x06007F55 RID: 32597 RVA: 0x0031DE46 File Offset: 0x0031CE46
		public AnnotationService(DocumentViewerBase viewer)
		{
			if (viewer == null)
			{
				throw new ArgumentNullException("viewer");
			}
			this.Initialize(viewer);
		}

		// Token: 0x06007F56 RID: 32598 RVA: 0x0031DE46 File Offset: 0x0031CE46
		public AnnotationService(FlowDocumentScrollViewer viewer)
		{
			if (viewer == null)
			{
				throw new ArgumentNullException("viewer");
			}
			this.Initialize(viewer);
		}

		// Token: 0x06007F57 RID: 32599 RVA: 0x0031DE46 File Offset: 0x0031CE46
		public AnnotationService(FlowDocumentReader viewer)
		{
			if (viewer == null)
			{
				throw new ArgumentNullException("viewer");
			}
			this.Initialize(viewer);
		}

		// Token: 0x06007F58 RID: 32600 RVA: 0x0031DE7C File Offset: 0x0031CE7C
		internal AnnotationService(DependencyObject root)
		{
			if (root == null)
			{
				throw new ArgumentNullException("root");
			}
			if (!(root is FrameworkElement) && !(root is FrameworkContentElement))
			{
				throw new ArgumentException(SR.Get("ParameterMustBeLogicalNode"), "root");
			}
			this.Initialize(root);
		}

		// Token: 0x06007F59 RID: 32601 RVA: 0x0031DEE0 File Offset: 0x0031CEE0
		public void Enable(AnnotationStore annotationStore)
		{
			if (annotationStore == null)
			{
				throw new ArgumentNullException("annotationStore");
			}
			base.VerifyAccess();
			if (this._isEnabled)
			{
				throw new InvalidOperationException(SR.Get("AnnotationServiceIsAlreadyEnabled"));
			}
			AnnotationService.VerifyServiceConfiguration(this._root);
			this._asyncLoadOperation = this._root.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.LoadAnnotationsAsync), this);
			this._isEnabled = true;
			this._root.SetValue(AnnotationService.ServiceProperty, this);
			this._store = annotationStore;
			DocumentViewerBase documentViewerBase = this._root as DocumentViewerBase;
			if (documentViewerBase != null)
			{
				bool flag = documentViewerBase.Document is FixedDocument || documentViewerBase.Document is FixedDocumentSequence;
				bool flag2 = !flag && documentViewerBase.Document is FlowDocument;
				if (!flag && !flag2 && documentViewerBase.Document != null)
				{
					throw new InvalidOperationException(SR.Get("OnlyFlowFixedSupported"));
				}
				if (flag)
				{
					this._locatorManager.RegisterSelectionProcessor(new FixedTextSelectionProcessor(), typeof(TextRange));
					this._locatorManager.RegisterSelectionProcessor(new FixedTextSelectionProcessor(), typeof(TextAnchor));
				}
				else if (flag2)
				{
					this._locatorManager.RegisterSelectionProcessor(new TextSelectionProcessor(), typeof(TextRange));
					this._locatorManager.RegisterSelectionProcessor(new TextSelectionProcessor(), typeof(TextAnchor));
					this._locatorManager.RegisterSelectionProcessor(new TextViewSelectionProcessor(), typeof(DocumentViewerBase));
				}
			}
			annotationStore.StoreContentChanged += this.OnStoreContentChanged;
			annotationStore.AnchorChanged += this.OnAnchorChanged;
		}

		// Token: 0x06007F5A RID: 32602 RVA: 0x0031E080 File Offset: 0x0031D080
		public void Disable()
		{
			base.VerifyAccess();
			if (!this._isEnabled)
			{
				throw new InvalidOperationException(SR.Get("AnnotationServiceNotEnabled"));
			}
			if (this._asyncLoadOperation != null)
			{
				this._asyncLoadOperation.Abort();
				this._asyncLoadOperation = null;
			}
			try
			{
				this._store.StoreContentChanged -= this.OnStoreContentChanged;
				this._store.AnchorChanged -= this.OnAnchorChanged;
			}
			finally
			{
				DocumentViewerBase documentViewerBase;
				IDocumentPaginatorSource documentPaginatorSource;
				AnnotationService.GetViewerAndDocument(this._root, out documentViewerBase, out documentPaginatorSource);
				if (documentViewerBase != null)
				{
					this.UnregisterOnDocumentViewer(documentViewerBase);
				}
				else if (documentPaginatorSource != null)
				{
					ITextView textView = AnnotationService.GetTextView(documentPaginatorSource);
					if (textView != null)
					{
						textView.Updated -= this.OnContentChanged;
					}
				}
				this.UnloadAnnotations();
				this._isEnabled = false;
				this._root.ClearValue(AnnotationService.ServiceProperty);
			}
		}

		// Token: 0x06007F5B RID: 32603 RVA: 0x0031E160 File Offset: 0x0031D160
		public static AnnotationService GetService(DocumentViewerBase viewer)
		{
			if (viewer == null)
			{
				throw new ArgumentNullException("viewer");
			}
			return viewer.GetValue(AnnotationService.ServiceProperty) as AnnotationService;
		}

		// Token: 0x06007F5C RID: 32604 RVA: 0x0031E180 File Offset: 0x0031D180
		public static AnnotationService GetService(FlowDocumentReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			return reader.GetValue(AnnotationService.ServiceProperty) as AnnotationService;
		}

		// Token: 0x06007F5D RID: 32605 RVA: 0x0031E160 File Offset: 0x0031D160
		public static AnnotationService GetService(FlowDocumentScrollViewer viewer)
		{
			if (viewer == null)
			{
				throw new ArgumentNullException("viewer");
			}
			return viewer.GetValue(AnnotationService.ServiceProperty) as AnnotationService;
		}

		// Token: 0x06007F5E RID: 32606 RVA: 0x0031E1A0 File Offset: 0x0031D1A0
		internal void LoadAnnotations(DependencyObject element)
		{
			if (this._asyncLoadOperation != null)
			{
				return;
			}
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (!(element is FrameworkElement) && !(element is FrameworkContentElement))
			{
				throw new ArgumentException(SR.Get("ParameterMustBeLogicalNode"), "element");
			}
			base.VerifyAccess();
			if (!this._isEnabled)
			{
				throw new InvalidOperationException(SR.Get("AnnotationServiceNotEnabled"));
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.LoadAnnotationsBegin);
			IList<IAttachedAnnotation> attachedAnnotations = this.LocatorManager.ProcessSubTree(element);
			this.LoadAnnotationsFromList(attachedAnnotations);
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.LoadAnnotationsEnd);
		}

		// Token: 0x06007F5F RID: 32607 RVA: 0x0031E234 File Offset: 0x0031D234
		internal void UnloadAnnotations(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (!(element is FrameworkElement) && !(element is FrameworkContentElement))
			{
				throw new ArgumentException(SR.Get("ParameterMustBeLogicalNode"), "element");
			}
			base.VerifyAccess();
			if (!this._isEnabled)
			{
				throw new InvalidOperationException(SR.Get("AnnotationServiceNotEnabled"));
			}
			if (this._annotationMap.IsEmpty)
			{
				return;
			}
			IList allAttachedAnnotationsFor = this.GetAllAttachedAnnotationsFor(element);
			this.UnloadAnnotationsFromList(allAttachedAnnotationsFor);
		}

		// Token: 0x06007F60 RID: 32608 RVA: 0x0031E2B0 File Offset: 0x0031D2B0
		private void UnloadAnnotations()
		{
			IList allAttachedAnnotations = this._annotationMap.GetAllAttachedAnnotations();
			this.UnloadAnnotationsFromList(allAttachedAnnotations);
		}

		// Token: 0x06007F61 RID: 32609 RVA: 0x0031E2D0 File Offset: 0x0031D2D0
		internal IList<IAttachedAnnotation> GetAttachedAnnotations()
		{
			base.VerifyAccess();
			if (!this._isEnabled)
			{
				throw new InvalidOperationException(SR.Get("AnnotationServiceNotEnabled"));
			}
			return this._annotationMap.GetAllAttachedAnnotations();
		}

		// Token: 0x17001D66 RID: 7526
		// (get) Token: 0x06007F62 RID: 32610 RVA: 0x0031E2FB File Offset: 0x0031D2FB
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
		}

		// Token: 0x17001D67 RID: 7527
		// (get) Token: 0x06007F63 RID: 32611 RVA: 0x0031E303 File Offset: 0x0031D303
		public AnnotationStore Store
		{
			get
			{
				return this._store;
			}
		}

		// Token: 0x06007F64 RID: 32612 RVA: 0x0031E30B File Offset: 0x0031D30B
		internal static AnnotationService GetService(DependencyObject d)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			return d.GetValue(AnnotationService.ServiceProperty) as AnnotationService;
		}

		// Token: 0x06007F65 RID: 32613 RVA: 0x0031E32B File Offset: 0x0031D32B
		internal static AnnotationComponentChooser GetChooser(DependencyObject d)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			return (AnnotationComponentChooser)d.GetValue(AnnotationService.ChooserProperty);
		}

		// Token: 0x06007F66 RID: 32614 RVA: 0x0031E34B File Offset: 0x0031D34B
		internal static void SetSubTreeProcessorId(DependencyObject d, string id)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			d.SetValue(AnnotationService.SubTreeProcessorIdProperty, id);
		}

		// Token: 0x06007F67 RID: 32615 RVA: 0x0031E375 File Offset: 0x0031D375
		internal static string GetSubTreeProcessorId(DependencyObject d)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			return d.GetValue(AnnotationService.SubTreeProcessorIdProperty) as string;
		}

		// Token: 0x06007F68 RID: 32616 RVA: 0x0031E395 File Offset: 0x0031D395
		internal static void SetDataId(DependencyObject d, string id)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			d.SetValue(AnnotationService.DataIdProperty, id);
		}

		// Token: 0x06007F69 RID: 32617 RVA: 0x0031E3BF File Offset: 0x0031D3BF
		internal static string GetDataId(DependencyObject d)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			return d.GetValue(AnnotationService.DataIdProperty) as string;
		}

		// Token: 0x14000168 RID: 360
		// (add) Token: 0x06007F6A RID: 32618 RVA: 0x0031E3E0 File Offset: 0x0031D3E0
		// (remove) Token: 0x06007F6B RID: 32619 RVA: 0x0031E418 File Offset: 0x0031D418
		internal event AttachedAnnotationChangedEventHandler AttachedAnnotationChanged;

		// Token: 0x17001D68 RID: 7528
		// (get) Token: 0x06007F6C RID: 32620 RVA: 0x0031E44D File Offset: 0x0031D44D
		internal LocatorManager LocatorManager
		{
			get
			{
				return this._locatorManager;
			}
		}

		// Token: 0x17001D69 RID: 7529
		// (get) Token: 0x06007F6D RID: 32621 RVA: 0x0031E455 File Offset: 0x0031D455
		internal DependencyObject Root
		{
			get
			{
				return this._root;
			}
		}

		// Token: 0x06007F6E RID: 32622 RVA: 0x0031E460 File Offset: 0x0031D460
		private void Initialize(DependencyObject root)
		{
			Invariant.Assert(root != null, "Parameter 'root' is null.");
			this._root = root;
			this._locatorManager = new LocatorManager();
			this._annotationComponentManager = new AnnotationComponentManager(this);
			AdornerPresentationContext.SetTypeZLevel(typeof(StickyNoteControl), 0);
			AdornerPresentationContext.SetTypeZLevel(typeof(MarkedHighlightComponent), 1);
			AdornerPresentationContext.SetTypeZLevel(typeof(HighlightComponent), 1);
			AdornerPresentationContext.SetZLevelRange(0, 1073741824, int.MaxValue);
			AdornerPresentationContext.SetZLevelRange(1, 0, 0);
		}

		// Token: 0x06007F6F RID: 32623 RVA: 0x0031E4E4 File Offset: 0x0031D4E4
		private object LoadAnnotationsAsync(object obj)
		{
			Invariant.Assert(this._isEnabled, "Service was disabled before attach operation executed.");
			this._asyncLoadOperation = null;
			IDocumentPaginatorSource documentPaginatorSource = null;
			DocumentViewerBase documentViewerBase;
			AnnotationService.GetViewerAndDocument(this.Root, out documentViewerBase, out documentPaginatorSource);
			if (documentViewerBase != null)
			{
				this.RegisterOnDocumentViewer(documentViewerBase);
			}
			else if (documentPaginatorSource != null)
			{
				ITextView textView = AnnotationService.GetTextView(documentPaginatorSource);
				if (textView != null)
				{
					textView.Updated += this.OnContentChanged;
				}
			}
			IList<IAttachedAnnotation> obj2 = this.LocatorManager.ProcessSubTree(this._root);
			this.LoadAnnotationsFromListAsync(obj2);
			return null;
		}

		// Token: 0x06007F70 RID: 32624 RVA: 0x0031E560 File Offset: 0x0031D560
		private object LoadAnnotationsFromListAsync(object obj)
		{
			this._asyncLoadFromListOperation = null;
			List<IAttachedAnnotation> list = obj as List<IAttachedAnnotation>;
			if (list == null)
			{
				return null;
			}
			if (list.Count > 10)
			{
				List<IAttachedAnnotation> arg = new List<IAttachedAnnotation>(list.Count);
				arg = list.GetRange(10, list.Count - 10);
				this._asyncLoadFromListOperation = this._root.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.LoadAnnotationsFromListAsync), arg);
				list.RemoveRange(10, list.Count - 10);
			}
			this.LoadAnnotationsFromList(list);
			return null;
		}

		// Token: 0x06007F71 RID: 32625 RVA: 0x0031E5E8 File Offset: 0x0031D5E8
		private bool AttachedAnchorsEqual(IAttachedAnnotation firstAttachedAnnotation, IAttachedAnnotation secondAttachedAnnotation)
		{
			object attachedAnchor = firstAttachedAnnotation.AttachedAnchor;
			if (firstAttachedAnnotation.AttachmentLevel == secondAttachedAnnotation.AttachmentLevel)
			{
				TextAnchor textAnchor = secondAttachedAnnotation.AttachedAnchor as TextAnchor;
				if (textAnchor != null && textAnchor.Equals(attachedAnchor))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007F72 RID: 32626 RVA: 0x0031E628 File Offset: 0x0031D628
		private void LoadAnnotationsFromList(IList<IAttachedAnnotation> attachedAnnotations)
		{
			Invariant.Assert(attachedAnnotations != null, "null attachedAnnotations list");
			List<AttachedAnnotationChangedEventArgs> list = new List<AttachedAnnotationChangedEventArgs>(attachedAnnotations.Count);
			foreach (IAttachedAnnotation attachedAnnotation in attachedAnnotations)
			{
				Invariant.Assert(attachedAnnotation != null && attachedAnnotation.Annotation != null, "invalid attached annotation");
				IAttachedAnnotation attachedAnnotation2 = this.FindAnnotationInList(attachedAnnotation, this._annotationMap.GetAttachedAnnotations(attachedAnnotation.Annotation.Id));
				if (attachedAnnotation2 != null)
				{
					object attachedAnchor = attachedAnnotation2.AttachedAnchor;
					AttachmentLevel attachmentLevel = attachedAnnotation2.AttachmentLevel;
					if (attachedAnnotation.AttachmentLevel != AttachmentLevel.Unresolved && attachedAnnotation.AttachmentLevel != AttachmentLevel.Incomplete)
					{
						if (!this.AttachedAnchorsEqual(attachedAnnotation2, attachedAnnotation))
						{
							((AttachedAnnotation)attachedAnnotation2).Update(attachedAnnotation.AttachedAnchor, attachedAnnotation.AttachmentLevel, null);
							this.FullyResolveAnchor(attachedAnnotation2);
							list.Add(AttachedAnnotationChangedEventArgs.Modified(attachedAnnotation2, attachedAnchor, attachmentLevel));
						}
					}
					else
					{
						this.DoRemoveAttachedAnnotation(attachedAnnotation);
						list.Add(AttachedAnnotationChangedEventArgs.Unloaded(attachedAnnotation));
					}
				}
				else if (attachedAnnotation.AttachmentLevel != AttachmentLevel.Unresolved && attachedAnnotation.AttachmentLevel != AttachmentLevel.Incomplete)
				{
					this.DoAddAttachedAnnotation(attachedAnnotation);
					list.Add(AttachedAnnotationChangedEventArgs.Loaded(attachedAnnotation));
				}
			}
			this.FireEvents(list);
		}

		// Token: 0x06007F73 RID: 32627 RVA: 0x0031E76C File Offset: 0x0031D76C
		private void UnloadAnnotationsFromList(IList attachedAnnotations)
		{
			Invariant.Assert(attachedAnnotations != null, "null attachedAnnotations list");
			List<AttachedAnnotationChangedEventArgs> list = new List<AttachedAnnotationChangedEventArgs>(attachedAnnotations.Count);
			foreach (object obj in attachedAnnotations)
			{
				IAttachedAnnotation attachedAnnotation = (IAttachedAnnotation)obj;
				this.DoRemoveAttachedAnnotation(attachedAnnotation);
				list.Add(AttachedAnnotationChangedEventArgs.Unloaded(attachedAnnotation));
			}
			this.FireEvents(list);
		}

		// Token: 0x06007F74 RID: 32628 RVA: 0x0031E7F0 File Offset: 0x0031D7F0
		private void OnLayoutUpdated(object sender, EventArgs args)
		{
			UIElement uielement = this._root as UIElement;
			if (uielement != null)
			{
				uielement.LayoutUpdated -= this.OnLayoutUpdated;
			}
			this.UpdateAnnotations();
		}

		// Token: 0x06007F75 RID: 32629 RVA: 0x0031E824 File Offset: 0x0031D824
		private void UpdateAnnotations()
		{
			if (this._asyncLoadOperation != null)
			{
				return;
			}
			if (!this._isEnabled)
			{
				return;
			}
			IList<IAttachedAnnotation> list = null;
			IList<IAttachedAnnotation> list2 = new List<IAttachedAnnotation>();
			list = this.LocatorManager.ProcessSubTree(this._root);
			List<IAttachedAnnotation> allAttachedAnnotations = this._annotationMap.GetAllAttachedAnnotations();
			for (int i = allAttachedAnnotations.Count - 1; i >= 0; i--)
			{
				IAttachedAnnotation attachedAnnotation = this.FindAnnotationInList(allAttachedAnnotations[i], list);
				if (attachedAnnotation != null && this.AttachedAnchorsEqual(attachedAnnotation, allAttachedAnnotations[i]))
				{
					list.Remove(attachedAnnotation);
					list2.Add(allAttachedAnnotations[i]);
					allAttachedAnnotations.RemoveAt(i);
				}
			}
			if (allAttachedAnnotations != null && allAttachedAnnotations.Count > 0)
			{
				this.UnloadAnnotationsFromList(allAttachedAnnotations);
			}
			IList<UIElement> list3 = new List<UIElement>();
			foreach (IAttachedAnnotation attachedAnnotation2 in list2)
			{
				UIElement uielement = attachedAnnotation2.Parent as UIElement;
				if (uielement != null && !list3.Contains(uielement))
				{
					list3.Add(uielement);
					AnnotationService.InvalidateAdorners(uielement);
				}
			}
			if (this._asyncLoadFromListOperation != null)
			{
				this._asyncLoadFromListOperation.Abort();
				this._asyncLoadFromListOperation = null;
			}
			if (list != null && list.Count > 0)
			{
				this.LoadAnnotationsFromListAsync(list);
			}
		}

		// Token: 0x06007F76 RID: 32630 RVA: 0x0031E970 File Offset: 0x0031D970
		private static void InvalidateAdorners(UIElement element)
		{
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(element);
			if (adornerLayer != null)
			{
				Adorner[] adorners = adornerLayer.GetAdorners(element);
				if (adorners != null)
				{
					for (int i = 0; i < adorners.Length; i++)
					{
						AnnotationAdorner annotationAdorner = adorners[i] as AnnotationAdorner;
						if (annotationAdorner != null)
						{
							Invariant.Assert(annotationAdorner.AnnotationComponent != null, "AnnotationAdorner component undefined");
							annotationAdorner.AnnotationComponent.IsDirty = true;
						}
					}
					adornerLayer.Update(element);
				}
			}
		}

		// Token: 0x06007F77 RID: 32631 RVA: 0x0031E9D4 File Offset: 0x0031D9D4
		private static void VerifyServiceConfiguration(DependencyObject root)
		{
			Invariant.Assert(root != null, "Parameter 'root' is null.");
			if (AnnotationService.GetService(root) != null)
			{
				throw new InvalidOperationException(SR.Get("AnnotationServiceAlreadyExists"));
			}
			new DescendentsWalker<object>(TreeWalkPriority.VisualTree, new VisitedCallback<object>(AnnotationService.VerifyNoServiceOnNode), null).StartWalk(root);
		}

		// Token: 0x06007F78 RID: 32632 RVA: 0x0031EA20 File Offset: 0x0031DA20
		private static void GetViewerAndDocument(DependencyObject root, out DocumentViewerBase documentViewerBase, out IDocumentPaginatorSource document)
		{
			documentViewerBase = (root as DocumentViewerBase);
			document = null;
			if (documentViewerBase != null)
			{
				document = documentViewerBase.Document;
				return;
			}
			FlowDocumentReader flowDocumentReader = root as FlowDocumentReader;
			if (flowDocumentReader != null)
			{
				documentViewerBase = (AnnotationHelper.GetFdrHost(flowDocumentReader) as DocumentViewerBase);
				document = flowDocumentReader.Document;
				return;
			}
			FlowDocumentScrollViewer flowDocumentScrollViewer = root as FlowDocumentScrollViewer;
			if (flowDocumentScrollViewer != null)
			{
				document = flowDocumentScrollViewer.Document;
			}
		}

		// Token: 0x06007F79 RID: 32633 RVA: 0x0031EA78 File Offset: 0x0031DA78
		private static ITextView GetTextView(IDocumentPaginatorSource document)
		{
			ITextView result = null;
			IServiceProvider serviceProvider = document as IServiceProvider;
			if (serviceProvider != null)
			{
				ITextContainer textContainer = serviceProvider.GetService(typeof(ITextContainer)) as ITextContainer;
				if (textContainer != null)
				{
					result = textContainer.TextView;
				}
			}
			return result;
		}

		// Token: 0x06007F7A RID: 32634 RVA: 0x0031EAB2 File Offset: 0x0031DAB2
		private static bool VerifyNoServiceOnNode(DependencyObject node, object data, bool visitedViaVisualTree)
		{
			Invariant.Assert(node != null, "Parameter 'node' is null.");
			if (node.ReadLocalValue(AnnotationService.ServiceProperty) is AnnotationService)
			{
				throw new InvalidOperationException(SR.Get("AnnotationServiceAlreadyExists"));
			}
			return true;
		}

		// Token: 0x06007F7B RID: 32635 RVA: 0x0031EAE8 File Offset: 0x0031DAE8
		private IAttachedAnnotation FindAnnotationInList(IAttachedAnnotation attachedAnnotation, IList<IAttachedAnnotation> list)
		{
			foreach (IAttachedAnnotation attachedAnnotation2 in list)
			{
				if (attachedAnnotation2.Annotation == attachedAnnotation.Annotation && attachedAnnotation2.Anchor == attachedAnnotation.Anchor && attachedAnnotation2.Parent == attachedAnnotation.Parent)
				{
					return attachedAnnotation2;
				}
			}
			return null;
		}

		// Token: 0x06007F7C RID: 32636 RVA: 0x0031EB5C File Offset: 0x0031DB5C
		private IList GetAllAttachedAnnotationsFor(DependencyObject element)
		{
			Invariant.Assert(element != null, "Parameter 'element' is null.");
			List<IAttachedAnnotation> list = new List<IAttachedAnnotation>();
			new DescendentsWalker<List<IAttachedAnnotation>>(TreeWalkPriority.VisualTree, new VisitedCallback<List<IAttachedAnnotation>>(this.GetAttachedAnnotationsFor), list).StartWalk(element);
			return list;
		}

		// Token: 0x06007F7D RID: 32637 RVA: 0x0031EB98 File Offset: 0x0031DB98
		private bool GetAttachedAnnotationsFor(DependencyObject node, List<IAttachedAnnotation> result, bool visitedViaVisualTree)
		{
			Invariant.Assert(node != null, "Parameter 'node' is null.");
			Invariant.Assert(result != null, "Incorrect data passed - should be a List<IAttachedAnnotation>.");
			List<IAttachedAnnotation> list = node.GetValue(AnnotationService.AttachedAnnotationsProperty) as List<IAttachedAnnotation>;
			if (list != null)
			{
				result.AddRange(list);
			}
			return true;
		}

		// Token: 0x06007F7E RID: 32638 RVA: 0x0031EBE0 File Offset: 0x0031DBE0
		private void OnStoreContentChanged(object node, StoreContentChangedEventArgs args)
		{
			base.VerifyAccess();
			StoreContentAction action = args.Action;
			if (action == StoreContentAction.Added)
			{
				this.AnnotationAdded(args.Annotation);
				return;
			}
			if (action != StoreContentAction.Deleted)
			{
				Invariant.Assert(false, "Unknown StoreContentAction.");
				return;
			}
			this.AnnotationDeleted(args.Annotation.Id);
		}

		// Token: 0x06007F7F RID: 32639 RVA: 0x0031EC30 File Offset: 0x0031DC30
		private void OnAnchorChanged(object sender, AnnotationResourceChangedEventArgs args)
		{
			base.VerifyAccess();
			if (args.Resource == null)
			{
				return;
			}
			AttachedAnnotationChangedEventArgs attachedAnnotationChangedEventArgs = null;
			switch (args.Action)
			{
			case AnnotationAction.Added:
				attachedAnnotationChangedEventArgs = this.AnchorAdded(args.Annotation, args.Resource);
				break;
			case AnnotationAction.Removed:
				attachedAnnotationChangedEventArgs = this.AnchorRemoved(args.Annotation, args.Resource);
				break;
			case AnnotationAction.Modified:
				attachedAnnotationChangedEventArgs = this.AnchorModified(args.Annotation, args.Resource);
				break;
			default:
				Invariant.Assert(false, "Unknown AnnotationAction.");
				break;
			}
			if (attachedAnnotationChangedEventArgs != null)
			{
				this.AttachedAnnotationChanged(this, attachedAnnotationChangedEventArgs);
			}
		}

		// Token: 0x06007F80 RID: 32640 RVA: 0x0031ECC4 File Offset: 0x0031DCC4
		private void AnnotationAdded(Annotation annotation)
		{
			Invariant.Assert(annotation != null, "Parameter 'annotation' is null.");
			if (this._annotationMap.GetAttachedAnnotations(annotation.Id).Count > 0)
			{
				throw new Exception(SR.Get("AnnotationAlreadyExistInService"));
			}
			List<AttachedAnnotationChangedEventArgs> list = new List<AttachedAnnotationChangedEventArgs>(annotation.Anchors.Count);
			foreach (AnnotationResource annotationResource in annotation.Anchors)
			{
				if (annotationResource.ContentLocators.Count != 0)
				{
					AttachedAnnotationChangedEventArgs attachedAnnotationChangedEventArgs = this.AnchorAdded(annotation, annotationResource);
					if (attachedAnnotationChangedEventArgs != null)
					{
						list.Add(attachedAnnotationChangedEventArgs);
					}
				}
			}
			this.FireEvents(list);
		}

		// Token: 0x06007F81 RID: 32641 RVA: 0x0031ED7C File Offset: 0x0031DD7C
		private void AnnotationDeleted(Guid annotationId)
		{
			IList<IAttachedAnnotation> attachedAnnotations = this._annotationMap.GetAttachedAnnotations(annotationId);
			if (attachedAnnotations.Count > 0)
			{
				IAttachedAnnotation[] array = new IAttachedAnnotation[attachedAnnotations.Count];
				attachedAnnotations.CopyTo(array, 0);
				List<AttachedAnnotationChangedEventArgs> list = new List<AttachedAnnotationChangedEventArgs>(array.Length);
				foreach (IAttachedAnnotation attachedAnnotation in array)
				{
					this.DoRemoveAttachedAnnotation(attachedAnnotation);
					list.Add(AttachedAnnotationChangedEventArgs.Deleted(attachedAnnotation));
				}
				this.FireEvents(list);
			}
		}

		// Token: 0x06007F82 RID: 32642 RVA: 0x0031EDF4 File Offset: 0x0031DDF4
		private AttachedAnnotationChangedEventArgs AnchorAdded(Annotation annotation, AnnotationResource anchor)
		{
			Invariant.Assert(annotation != null && anchor != null, "Parameter 'annotation' or 'anchor' is null.");
			AttachedAnnotationChangedEventArgs result = null;
			AttachmentLevel attachmentLevel;
			object obj = this.FindAttachedAnchor(anchor, out attachmentLevel);
			if (attachmentLevel != AttachmentLevel.Unresolved && attachmentLevel != AttachmentLevel.Incomplete)
			{
				Invariant.Assert(obj != null, "Must have a valid attached anchor.");
				AttachedAnnotation attachedAnnotation = new AttachedAnnotation(this.LocatorManager, annotation, anchor, obj, attachmentLevel);
				this.DoAddAttachedAnnotation(attachedAnnotation);
				result = AttachedAnnotationChangedEventArgs.Added(attachedAnnotation);
			}
			return result;
		}

		// Token: 0x06007F83 RID: 32643 RVA: 0x0031EE5C File Offset: 0x0031DE5C
		private AttachedAnnotationChangedEventArgs AnchorRemoved(Annotation annotation, AnnotationResource anchor)
		{
			Invariant.Assert(annotation != null && anchor != null, "Parameter 'annotation' or 'anchor' is null.");
			AttachedAnnotationChangedEventArgs result = null;
			IList<IAttachedAnnotation> attachedAnnotations = this._annotationMap.GetAttachedAnnotations(annotation.Id);
			if (attachedAnnotations.Count > 0)
			{
				IAttachedAnnotation[] array = new IAttachedAnnotation[attachedAnnotations.Count];
				attachedAnnotations.CopyTo(array, 0);
				foreach (IAttachedAnnotation attachedAnnotation in array)
				{
					if (attachedAnnotation.Anchor == anchor)
					{
						this.DoRemoveAttachedAnnotation(attachedAnnotation);
						result = AttachedAnnotationChangedEventArgs.Deleted(attachedAnnotation);
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06007F84 RID: 32644 RVA: 0x0031EEE8 File Offset: 0x0031DEE8
		private AttachedAnnotationChangedEventArgs AnchorModified(Annotation annotation, AnnotationResource anchor)
		{
			Invariant.Assert(annotation != null && anchor != null, "Parameter 'annotation' or 'anchor' is null.");
			AttachedAnnotationChangedEventArgs result = null;
			bool flag = false;
			AttachmentLevel attachmentLevel;
			object obj = this.FindAttachedAnchor(anchor, out attachmentLevel);
			List<IAttachedAnnotation> attachedAnnotations = this._annotationMap.GetAttachedAnnotations(annotation.Id);
			IAttachedAnnotation[] array = new IAttachedAnnotation[((ICollection<IAttachedAnnotation>)attachedAnnotations).Count];
			((ICollection<IAttachedAnnotation>)attachedAnnotations).CopyTo(array, 0);
			IAttachedAnnotation[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				IAttachedAnnotation attachedAnnotation = array2[i];
				if (attachedAnnotation.Anchor == anchor)
				{
					flag = true;
					if (attachmentLevel != AttachmentLevel.Unresolved)
					{
						Invariant.Assert(obj != null, "AttachedAnnotation with AttachmentLevel != Unresolved should have non-null AttachedAnchor.");
						object attachedAnchor = attachedAnnotation.AttachedAnchor;
						AttachmentLevel attachmentLevel2 = attachedAnnotation.AttachmentLevel;
						((AttachedAnnotation)attachedAnnotation).Update(obj, attachmentLevel, null);
						this.FullyResolveAnchor(attachedAnnotation);
						result = AttachedAnnotationChangedEventArgs.Modified(attachedAnnotation, attachedAnchor, attachmentLevel2);
						break;
					}
					this.DoRemoveAttachedAnnotation(attachedAnnotation);
					result = AttachedAnnotationChangedEventArgs.Deleted(attachedAnnotation);
					break;
				}
				else
				{
					i++;
				}
			}
			if (!flag && attachmentLevel != AttachmentLevel.Unresolved && attachmentLevel != AttachmentLevel.Incomplete)
			{
				Invariant.Assert(obj != null, "AttachedAnnotation with AttachmentLevel != Unresolved should have non-null AttachedAnchor.");
				AttachedAnnotation attachedAnnotation2 = new AttachedAnnotation(this.LocatorManager, annotation, anchor, obj, attachmentLevel);
				this.DoAddAttachedAnnotation(attachedAnnotation2);
				result = AttachedAnnotationChangedEventArgs.Added(attachedAnnotation2);
			}
			return result;
		}

		// Token: 0x06007F85 RID: 32645 RVA: 0x0031F000 File Offset: 0x0031E000
		private void DoAddAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			Invariant.Assert(attachedAnnotation != null, "Parameter 'attachedAnnotation' is null.");
			DependencyObject parent = attachedAnnotation.Parent;
			Invariant.Assert(parent != null, "AttachedAnnotation being added should have non-null Parent.");
			List<IAttachedAnnotation> list = parent.GetValue(AnnotationService.AttachedAnnotationsProperty) as List<IAttachedAnnotation>;
			if (list == null)
			{
				list = new List<IAttachedAnnotation>(1);
				parent.SetValue(AnnotationService.AttachedAnnotationsProperty, list);
			}
			list.Add(attachedAnnotation);
			this._annotationMap.AddAttachedAnnotation(attachedAnnotation);
			this.FullyResolveAnchor(attachedAnnotation);
		}

		// Token: 0x06007F86 RID: 32646 RVA: 0x0031F074 File Offset: 0x0031E074
		private void DoRemoveAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			Invariant.Assert(attachedAnnotation != null, "Parameter 'attachedAnnotation' is null.");
			DependencyObject parent = attachedAnnotation.Parent;
			Invariant.Assert(parent != null, "AttachedAnnotation being added should have non-null Parent.");
			this._annotationMap.RemoveAttachedAnnotation(attachedAnnotation);
			List<IAttachedAnnotation> list = parent.GetValue(AnnotationService.AttachedAnnotationsProperty) as List<IAttachedAnnotation>;
			if (list != null)
			{
				list.Remove(attachedAnnotation);
				if (list.Count == 0)
				{
					parent.ClearValue(AnnotationService.AttachedAnnotationsProperty);
				}
			}
		}

		// Token: 0x06007F87 RID: 32647 RVA: 0x0031F0E0 File Offset: 0x0031E0E0
		private void FullyResolveAnchor(IAttachedAnnotation attachedAnnotation)
		{
			Invariant.Assert(attachedAnnotation != null, "Attached annotation cannot be null.");
			if (attachedAnnotation.AttachmentLevel == AttachmentLevel.Full)
			{
				return;
			}
			FixedPageProcessor fixedPageProcessor = null;
			TextSelectionProcessor textSelectionProcessor = null;
			TextSelectionProcessor textSelectionProcessor2 = null;
			bool flag = false;
			FrameworkElement frameworkElement = this.Root as FrameworkElement;
			if (frameworkElement is DocumentViewerBase)
			{
				flag = (((DocumentViewerBase)frameworkElement).Document is FlowDocument);
			}
			else if (frameworkElement is FlowDocumentScrollViewer || frameworkElement is FlowDocumentReader)
			{
				flag = true;
			}
			else
			{
				frameworkElement = null;
			}
			if (frameworkElement != null)
			{
				try
				{
					if (flag)
					{
						textSelectionProcessor = (this.LocatorManager.GetSelectionProcessor(typeof(TextRange)) as TextSelectionProcessor);
						Invariant.Assert(textSelectionProcessor != null, "TextSelectionProcessor should be available if we are processing flow content.");
						textSelectionProcessor.Clamping = false;
						textSelectionProcessor2 = (this.LocatorManager.GetSelectionProcessor(typeof(TextAnchor)) as TextSelectionProcessor);
						Invariant.Assert(textSelectionProcessor2 != null, "TextSelectionProcessor should be available if we are processing flow content.");
						textSelectionProcessor2.Clamping = false;
					}
					else
					{
						fixedPageProcessor = (this.LocatorManager.GetSubTreeProcessorForLocatorPart(FixedPageProcessor.CreateLocatorPart(0)) as FixedPageProcessor);
						Invariant.Assert(fixedPageProcessor != null, "FixedPageProcessor should be available if we are processing fixed content.");
						fixedPageProcessor.UseLogicalTree = true;
					}
					AttachmentLevel attachmentLevel;
					object fullyAttachedAnchor = this.FindAttachedAnchor(attachedAnnotation.Anchor, out attachmentLevel);
					if (attachmentLevel == AttachmentLevel.Full)
					{
						((AttachedAnnotation)attachedAnnotation).SetFullyAttachedAnchor(fullyAttachedAnchor);
					}
				}
				finally
				{
					if (flag)
					{
						textSelectionProcessor.Clamping = true;
						textSelectionProcessor2.Clamping = true;
					}
					else
					{
						fixedPageProcessor.UseLogicalTree = false;
					}
				}
			}
		}

		// Token: 0x06007F88 RID: 32648 RVA: 0x0031F23C File Offset: 0x0031E23C
		private object FindAttachedAnchor(AnnotationResource anchor, out AttachmentLevel attachmentLevel)
		{
			Invariant.Assert(anchor != null, "Parameter 'anchor' is null.");
			attachmentLevel = AttachmentLevel.Unresolved;
			object obj = null;
			foreach (ContentLocatorBase locator in anchor.ContentLocators)
			{
				obj = this.LocatorManager.FindAttachedAnchor(this._root, null, locator, out attachmentLevel);
				if (obj != null)
				{
					break;
				}
			}
			return obj;
		}

		// Token: 0x06007F89 RID: 32649 RVA: 0x0031F2B0 File Offset: 0x0031E2B0
		private void FireEvents(List<AttachedAnnotationChangedEventArgs> eventsToFire)
		{
			Invariant.Assert(eventsToFire != null, "Parameter 'eventsToFire' is null.");
			if (this.AttachedAnnotationChanged != null)
			{
				foreach (AttachedAnnotationChangedEventArgs e in eventsToFire)
				{
					this.AttachedAnnotationChanged(this, e);
				}
			}
		}

		// Token: 0x06007F8A RID: 32650 RVA: 0x0031F31C File Offset: 0x0031E31C
		private void RegisterOnDocumentViewer(DocumentViewerBase viewer)
		{
			Invariant.Assert(viewer != null, "Parameter 'viewer' is null.");
			Invariant.Assert(this._views.Count == 0, "Failed to unregister on a viewer before registering on new viewer.");
			foreach (DocumentPageView documentPageView in viewer.PageViews)
			{
				documentPageView.PageConnected += this.OnContentChanged;
				this._views.Add(documentPageView);
			}
			viewer.PageViewsChanged += this.OnPageViewsChanged;
		}

		// Token: 0x06007F8B RID: 32651 RVA: 0x0031F3B8 File Offset: 0x0031E3B8
		private void UnregisterOnDocumentViewer(DocumentViewerBase viewer)
		{
			Invariant.Assert(viewer != null, "Parameter 'viewer' is null.");
			foreach (DocumentPageView documentPageView in this._views)
			{
				documentPageView.PageConnected -= this.OnContentChanged;
			}
			this._views.Clear();
			viewer.PageViewsChanged -= this.OnPageViewsChanged;
		}

		// Token: 0x06007F8C RID: 32652 RVA: 0x0031F43C File Offset: 0x0031E43C
		private void OnPageViewsChanged(object sender, EventArgs e)
		{
			DocumentViewerBase documentViewerBase = sender as DocumentViewerBase;
			Invariant.Assert(documentViewerBase != null, "Sender for PageViewsChanged event should be a DocumentViewerBase.");
			this.UnregisterOnDocumentViewer(documentViewerBase);
			try
			{
				this.UpdateAnnotations();
			}
			finally
			{
				this.RegisterOnDocumentViewer(documentViewerBase);
			}
		}

		// Token: 0x06007F8D RID: 32653 RVA: 0x0031F488 File Offset: 0x0031E488
		private void OnContentChanged(object sender, EventArgs e)
		{
			UIElement uielement = this._root as UIElement;
			if (uielement != null)
			{
				uielement.LayoutUpdated += this.OnLayoutUpdated;
			}
		}

		// Token: 0x04003B75 RID: 15221
		public static readonly RoutedUICommand CreateHighlightCommand = new RoutedUICommand(SR.Get("CreateHighlight"), "CreateHighlight", typeof(AnnotationService), null);

		// Token: 0x04003B76 RID: 15222
		public static readonly RoutedUICommand CreateTextStickyNoteCommand = new RoutedUICommand(SR.Get("CreateTextNote"), "CreateTextStickyNote", typeof(AnnotationService), null);

		// Token: 0x04003B77 RID: 15223
		public static readonly RoutedUICommand CreateInkStickyNoteCommand = new RoutedUICommand(SR.Get("CreateInkNote"), "CreateInkStickyNote", typeof(AnnotationService), null);

		// Token: 0x04003B78 RID: 15224
		public static readonly RoutedUICommand ClearHighlightsCommand = new RoutedUICommand(SR.Get("ClearHighlight"), "ClearHighlights", typeof(AnnotationService), null);

		// Token: 0x04003B79 RID: 15225
		public static readonly RoutedUICommand DeleteStickyNotesCommand = new RoutedUICommand(SR.Get("DeleteNotes"), "DeleteStickyNotes", typeof(AnnotationService), null);

		// Token: 0x04003B7A RID: 15226
		public static readonly RoutedUICommand DeleteAnnotationsCommand = new RoutedUICommand(SR.Get("DeleteAnnotations"), "DeleteAnnotations", typeof(AnnotationService), null);

		// Token: 0x04003B7B RID: 15227
		internal static readonly DependencyProperty ChooserProperty = DependencyProperty.RegisterAttached("Chooser", typeof(AnnotationComponentChooser), typeof(AnnotationService), new FrameworkPropertyMetadata(new AnnotationComponentChooser(), FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior));

		// Token: 0x04003B7C RID: 15228
		internal static readonly DependencyProperty SubTreeProcessorIdProperty = LocatorManager.SubTreeProcessorIdProperty.AddOwner(typeof(AnnotationService));

		// Token: 0x04003B7D RID: 15229
		internal static readonly DependencyProperty DataIdProperty = DataIdProcessor.DataIdProperty.AddOwner(typeof(AnnotationService));

		// Token: 0x04003B7F RID: 15231
		internal static readonly DependencyProperty ServiceProperty = DependencyProperty.RegisterAttached("Service", typeof(AnnotationService), typeof(AnnotationService), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior));

		// Token: 0x04003B80 RID: 15232
		private static readonly DependencyProperty AttachedAnnotationsProperty = DependencyProperty.RegisterAttached("AttachedAnnotations", typeof(IList<IAttachedAnnotation>), typeof(AnnotationService));

		// Token: 0x04003B81 RID: 15233
		private DependencyObject _root;

		// Token: 0x04003B82 RID: 15234
		private AnnotationMap _annotationMap = new AnnotationMap();

		// Token: 0x04003B83 RID: 15235
		private AnnotationComponentManager _annotationComponentManager;

		// Token: 0x04003B84 RID: 15236
		private LocatorManager _locatorManager;

		// Token: 0x04003B85 RID: 15237
		private bool _isEnabled;

		// Token: 0x04003B86 RID: 15238
		private AnnotationStore _store;

		// Token: 0x04003B87 RID: 15239
		private Collection<DocumentPageView> _views = new Collection<DocumentPageView>();

		// Token: 0x04003B88 RID: 15240
		private DispatcherOperation _asyncLoadOperation;

		// Token: 0x04003B89 RID: 15241
		private DispatcherOperation _asyncLoadFromListOperation;

		// Token: 0x04003B8A RID: 15242
		private const int _maxAnnotationsBatch = 10;
	}
}
