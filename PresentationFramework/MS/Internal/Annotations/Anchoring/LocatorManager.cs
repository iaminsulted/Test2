using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Annotations.Storage;
using System.Windows.Documents;
using System.Windows.Threading;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020002D3 RID: 723
	internal sealed class LocatorManager : DispatcherObject
	{
		// Token: 0x06001B45 RID: 6981 RVA: 0x00167E77 File Offset: 0x00166E77
		public LocatorManager() : this(null)
		{
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x00167E80 File Offset: 0x00166E80
		public LocatorManager(AnnotationStore store)
		{
			this._locatorPartHandlers = new Hashtable();
			this._subtreeProcessors = new Hashtable();
			this._selectionProcessors = new Hashtable();
			this.RegisterSubTreeProcessor(new DataIdProcessor(this), "Id");
			this.RegisterSubTreeProcessor(new FixedPageProcessor(this), FixedPageProcessor.Id);
			TreeNodeSelectionProcessor processor = new TreeNodeSelectionProcessor();
			this.RegisterSelectionProcessor(processor, typeof(FrameworkElement));
			this.RegisterSelectionProcessor(processor, typeof(FrameworkContentElement));
			TextSelectionProcessor processor2 = new TextSelectionProcessor();
			this.RegisterSelectionProcessor(processor2, typeof(TextRange));
			this.RegisterSelectionProcessor(processor2, typeof(TextAnchor));
			this._internalStore = store;
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x00167F30 File Offset: 0x00166F30
		public void RegisterSubTreeProcessor(SubTreeProcessor processor, string processorId)
		{
			base.VerifyAccess();
			if (processor == null)
			{
				throw new ArgumentNullException("processor");
			}
			if (processorId == null)
			{
				throw new ArgumentNullException("processorId");
			}
			XmlQualifiedName[] locatorPartTypes = processor.GetLocatorPartTypes();
			this._subtreeProcessors[processorId] = processor;
			if (locatorPartTypes != null)
			{
				foreach (XmlQualifiedName key in locatorPartTypes)
				{
					this._locatorPartHandlers[key] = processor;
				}
			}
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x00167F98 File Offset: 0x00166F98
		public SubTreeProcessor GetSubTreeProcessor(DependencyObject node)
		{
			base.VerifyAccess();
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			string text = node.GetValue(LocatorManager.SubTreeProcessorIdProperty) as string;
			if (string.IsNullOrEmpty(text))
			{
				return this._subtreeProcessors["Id"] as SubTreeProcessor;
			}
			SubTreeProcessor subTreeProcessor = (SubTreeProcessor)this._subtreeProcessors[text];
			if (subTreeProcessor != null)
			{
				return subTreeProcessor;
			}
			throw new ArgumentException(SR.Get("InvalidSubTreeProcessor", new object[]
			{
				text
			}));
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x00168018 File Offset: 0x00167018
		public SubTreeProcessor GetSubTreeProcessorForLocatorPart(ContentLocatorPart locatorPart)
		{
			base.VerifyAccess();
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			return this._locatorPartHandlers[locatorPart.PartType] as SubTreeProcessor;
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x00168044 File Offset: 0x00167044
		public void RegisterSelectionProcessor(SelectionProcessor processor, Type selectionType)
		{
			base.VerifyAccess();
			if (processor == null)
			{
				throw new ArgumentNullException("processor");
			}
			if (selectionType == null)
			{
				throw new ArgumentNullException("selectionType");
			}
			XmlQualifiedName[] locatorPartTypes = processor.GetLocatorPartTypes();
			this._selectionProcessors[selectionType] = processor;
			if (locatorPartTypes != null)
			{
				foreach (XmlQualifiedName key in locatorPartTypes)
				{
					this._locatorPartHandlers[key] = processor;
				}
			}
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x001680B4 File Offset: 0x001670B4
		public SelectionProcessor GetSelectionProcessor(Type selectionType)
		{
			base.VerifyAccess();
			if (selectionType == null)
			{
				throw new ArgumentNullException("selectionType");
			}
			SelectionProcessor selectionProcessor;
			do
			{
				selectionProcessor = (this._selectionProcessors[selectionType] as SelectionProcessor);
				selectionType = selectionType.BaseType;
			}
			while (selectionProcessor == null && selectionType != null);
			return selectionProcessor;
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x00168104 File Offset: 0x00167104
		public SelectionProcessor GetSelectionProcessorForLocatorPart(ContentLocatorPart locatorPart)
		{
			base.VerifyAccess();
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			return this._locatorPartHandlers[locatorPart.PartType] as SelectionProcessor;
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x00168130 File Offset: 0x00167130
		public IList<IAttachedAnnotation> ProcessAnnotations(DependencyObject node)
		{
			base.VerifyAccess();
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			IList<IAttachedAnnotation> list = new List<IAttachedAnnotation>();
			IList<ContentLocatorBase> list2 = this.GenerateLocators(node);
			if (list2.Count > 0)
			{
				AnnotationStore annotationStore;
				if (this._internalStore != null)
				{
					annotationStore = this._internalStore;
				}
				else
				{
					AnnotationService service = AnnotationService.GetService(node);
					if (service == null || !service.IsEnabled)
					{
						throw new InvalidOperationException(SR.Get("AnnotationServiceNotEnabled"));
					}
					annotationStore = service.Store;
				}
				ContentLocator[] array = new ContentLocator[list2.Count];
				ICollection<ContentLocatorBase> collection = list2;
				ContentLocatorBase[] array2 = array;
				collection.CopyTo(array2, 0);
				IList<Annotation> annotations = annotationStore.GetAnnotations(array[0]);
				foreach (ContentLocatorBase contentLocatorBase in list2)
				{
					ContentLocator contentLocator = (ContentLocator)contentLocatorBase;
					if (contentLocator.Parts[contentLocator.Parts.Count - 1].NameValuePairs.ContainsKey("IncludeOverlaps"))
					{
						contentLocator.Parts.RemoveAt(contentLocator.Parts.Count - 1);
					}
				}
				foreach (Annotation annotation in annotations)
				{
					foreach (AnnotationResource annotationResource in annotation.Anchors)
					{
						foreach (ContentLocatorBase locator in annotationResource.ContentLocators)
						{
							AttachmentLevel attachmentLevel;
							object attachedAnchor = this.FindAttachedAnchor(node, array, locator, out attachmentLevel);
							if (attachmentLevel != AttachmentLevel.Unresolved)
							{
								list.Add(new AttachedAnnotation(this, annotation, annotationResource, attachedAnchor, attachmentLevel));
								break;
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x00168330 File Offset: 0x00167330
		public IList<ContentLocatorBase> GenerateLocators(object selection)
		{
			base.VerifyAccess();
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			SelectionProcessor selectionProcessor = this.GetSelectionProcessor(selection.GetType());
			if (selectionProcessor != null)
			{
				ICollection nodes = (ICollection)selectionProcessor.GetSelectedNodes(selection);
				IList<ContentLocatorBase> list = null;
				PathNode pathNode = PathNode.BuildPathForElements(nodes);
				if (pathNode != null)
				{
					SubTreeProcessor subTreeProcessor = this.GetSubTreeProcessor(pathNode.Node);
					list = this.GenerateLocators(subTreeProcessor, pathNode, selection);
				}
				if (list == null)
				{
					list = new List<ContentLocatorBase>(0);
				}
				return list;
			}
			throw new ArgumentException("Unsupported Selection", "selection");
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x001683B4 File Offset: 0x001673B4
		public object ResolveLocator(ContentLocatorBase locator, int offset, DependencyObject startNode, out AttachmentLevel attachmentLevel)
		{
			base.VerifyAccess();
			if (locator == null)
			{
				throw new ArgumentNullException("locator");
			}
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			ContentLocator contentLocator = locator as ContentLocator;
			if (contentLocator != null && (offset < 0 || offset >= contentLocator.Parts.Count))
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			return this.InternalResolveLocator(locator, offset, startNode, false, out attachmentLevel);
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x00168416 File Offset: 0x00167416
		public static void SetSubTreeProcessorId(DependencyObject d, string id)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			d.SetValue(LocatorManager.SubTreeProcessorIdProperty, id);
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x00168432 File Offset: 0x00167432
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public static string GetSubTreeProcessorId(DependencyObject d)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			return d.GetValue(LocatorManager.SubTreeProcessorIdProperty) as string;
		}

		// Token: 0x06001B52 RID: 6994 RVA: 0x00168454 File Offset: 0x00167454
		internal IList<IAttachedAnnotation> ProcessSubTree(DependencyObject subTree)
		{
			if (subTree == null)
			{
				throw new ArgumentNullException("subTree");
			}
			LocatorManager.ProcessingTreeState processingTreeState = new LocatorManager.ProcessingTreeState();
			new PrePostDescendentsWalker<LocatorManager.ProcessingTreeState>(TreeWalkPriority.VisualTree, new VisitedCallback<LocatorManager.ProcessingTreeState>(this.PreVisit), new VisitedCallback<LocatorManager.ProcessingTreeState>(this.PostVisit), processingTreeState).StartWalk(subTree);
			return processingTreeState.AttachedAnnotations;
		}

		// Token: 0x06001B53 RID: 6995 RVA: 0x001684A0 File Offset: 0x001674A0
		internal object FindAttachedAnchor(DependencyObject startNode, ContentLocator[] prefixes, ContentLocatorBase locator, out AttachmentLevel attachmentLevel)
		{
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			if (locator == null)
			{
				throw new ArgumentNullException("locator");
			}
			attachmentLevel = AttachmentLevel.Unresolved;
			object result = null;
			bool flag = true;
			int num = this.FindMatchingPrefix(prefixes, locator, out flag);
			if (flag)
			{
				ContentLocator contentLocator = locator as ContentLocator;
				if (contentLocator == null || num < contentLocator.Parts.Count)
				{
					result = this.InternalResolveLocator(locator, num, startNode, num != 0, out attachmentLevel);
				}
				if (attachmentLevel == AttachmentLevel.Unresolved && num > 0)
				{
					if (num == 0)
					{
						attachmentLevel = AttachmentLevel.Unresolved;
					}
					else if (contentLocator != null && num < contentLocator.Parts.Count)
					{
						attachmentLevel = AttachmentLevel.Incomplete;
						result = startNode;
					}
					else
					{
						attachmentLevel = AttachmentLevel.Full;
						result = startNode;
					}
				}
			}
			return result;
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x00168540 File Offset: 0x00167540
		private int FindMatchingPrefix(ContentLocator[] prefixes, ContentLocatorBase locator, out bool matched)
		{
			matched = true;
			int result = 0;
			ContentLocator contentLocator = locator as ContentLocator;
			if (contentLocator != null && prefixes != null && prefixes.Length != 0)
			{
				matched = false;
				foreach (ContentLocator contentLocator2 in prefixes)
				{
					if (contentLocator.StartsWith(contentLocator2))
					{
						result = contentLocator2.Parts.Count;
						matched = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x00168598 File Offset: 0x00167598
		private IList<ContentLocatorBase> GenerateLocators(SubTreeProcessor processor, PathNode startNode, object selection)
		{
			List<ContentLocatorBase> list = new List<ContentLocatorBase>();
			bool flag = true;
			ContentLocator contentLocator = processor.GenerateLocator(startNode, out flag);
			bool flag2 = contentLocator != null;
			if (flag)
			{
				int count = startNode.Children.Count;
				if (count != 0)
				{
					if (count != 1)
					{
						ContentLocatorBase contentLocatorBase = this.GenerateLocatorGroup(startNode, selection);
						if (contentLocatorBase != null)
						{
							flag2 = false;
						}
						if (contentLocator != null)
						{
							list.Add(contentLocator.Merge(contentLocatorBase));
						}
						else if (contentLocatorBase != null)
						{
							list.Add(contentLocatorBase);
						}
					}
					else
					{
						SubTreeProcessor subTreeProcessor = this.GetSubTreeProcessor(startNode.Node);
						IList<ContentLocatorBase> list2 = this.GenerateLocators(subTreeProcessor, (PathNode)startNode.Children[0], selection);
						if (list2 != null && list2.Count > 0)
						{
							flag2 = false;
						}
						if (contentLocator != null)
						{
							list.AddRange(this.Merge(contentLocator, list2));
						}
						else
						{
							list.AddRange(list2);
						}
					}
				}
				else if (contentLocator != null)
				{
					list.Add(contentLocator);
				}
			}
			else if (contentLocator != null)
			{
				list.Add(contentLocator);
			}
			if (flag2 && selection != null)
			{
				SelectionProcessor selectionProcessor = this.GetSelectionProcessor(selection.GetType());
				if (selectionProcessor != null)
				{
					IList<ContentLocatorPart> list3 = selectionProcessor.GenerateLocatorParts(selection, startNode.Node);
					if (list3 != null && list3.Count > 0)
					{
						List<ContentLocatorBase> list4 = new List<ContentLocatorBase>(list.Count * list3.Count);
						foreach (ContentLocatorBase contentLocatorBase2 in list)
						{
							list4.AddRange(((ContentLocator)contentLocatorBase2).DotProduct(list3));
						}
						list = list4;
					}
				}
			}
			return list;
		}

		// Token: 0x06001B56 RID: 6998 RVA: 0x00168728 File Offset: 0x00167728
		private ContentLocatorBase GenerateLocatorGroup(PathNode node, object selection)
		{
			SubTreeProcessor subTreeProcessor = this.GetSubTreeProcessor(node.Node);
			ContentLocatorGroup contentLocatorGroup = new ContentLocatorGroup();
			foreach (object obj in node.Children)
			{
				PathNode startNode = (PathNode)obj;
				IList<ContentLocatorBase> list = this.GenerateLocators(subTreeProcessor, startNode, selection);
				if (list != null && list.Count > 0 && list[0] != null)
				{
					ContentLocator contentLocator = list[0] as ContentLocator;
					if (contentLocator != null && contentLocator.Parts.Count != 0)
					{
						contentLocatorGroup.Locators.Add(contentLocator);
					}
					else
					{
						ContentLocatorGroup contentLocatorGroup2 = list[0] as ContentLocatorGroup;
					}
				}
			}
			if (contentLocatorGroup.Locators.Count == 0)
			{
				return null;
			}
			if (contentLocatorGroup.Locators.Count == 1)
			{
				ContentLocator contentLocator2 = contentLocatorGroup.Locators[0];
				contentLocatorGroup.Locators.Remove(contentLocator2);
				return contentLocator2;
			}
			return contentLocatorGroup;
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x0016882C File Offset: 0x0016782C
		private bool PreVisit(DependencyObject dependencyObject, LocatorManager.ProcessingTreeState data, bool visitedViaVisualTree)
		{
			bool flag = false;
			IList<IAttachedAnnotation> list = this.GetSubTreeProcessor(dependencyObject).PreProcessNode(dependencyObject, out flag);
			if (list != null)
			{
				data.AttachedAnnotations.AddRange(list);
			}
			data.CalledProcessAnnotations = (data.CalledProcessAnnotations || flag);
			data.Push();
			return !flag;
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x00168874 File Offset: 0x00167874
		private bool PostVisit(DependencyObject dependencyObject, LocatorManager.ProcessingTreeState data, bool visitedViaVisualTree)
		{
			bool flag = data.Pop();
			SubTreeProcessor subTreeProcessor = this.GetSubTreeProcessor(dependencyObject);
			bool flag2 = false;
			IList<IAttachedAnnotation> list = subTreeProcessor.PostProcessNode(dependencyObject, flag, out flag2);
			if (list != null)
			{
				data.AttachedAnnotations.AddRange(list);
			}
			data.CalledProcessAnnotations = (data.CalledProcessAnnotations || flag2 || flag);
			return true;
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x001688BC File Offset: 0x001678BC
		private object InternalResolveLocator(ContentLocatorBase locator, int offset, DependencyObject startNode, bool skipStartNode, out AttachmentLevel attachmentLevel)
		{
			attachmentLevel = AttachmentLevel.Full;
			object result = null;
			ContentLocatorGroup contentLocatorGroup = locator as ContentLocatorGroup;
			ContentLocator contentLocator = locator as ContentLocator;
			AttachmentLevel attachmentLevel2 = AttachmentLevel.Unresolved;
			if (contentLocator != null && offset == contentLocator.Parts.Count - 1)
			{
				ContentLocatorPart locatorPart = contentLocator.Parts[offset];
				SelectionProcessor selectionProcessorForLocatorPart = this.GetSelectionProcessorForLocatorPart(locatorPart);
				if (selectionProcessorForLocatorPart != null)
				{
					result = selectionProcessorForLocatorPart.ResolveLocatorPart(locatorPart, startNode, out attachmentLevel2);
					attachmentLevel = attachmentLevel2;
					return result;
				}
			}
			IList<ContentLocator> list;
			if (contentLocatorGroup == null)
			{
				list = new List<ContentLocator>(1);
				list.Add(contentLocator);
			}
			else
			{
				AnnotationService service = AnnotationService.GetService(startNode);
				if (service != null)
				{
					startNode = service.Root;
				}
				list = contentLocatorGroup.Locators;
				offset = 0;
				skipStartNode = false;
			}
			bool flag = true;
			if (list.Count > 0)
			{
				LocatorManager.ResolvingLocatorState resolvingLocatorState = this.ResolveSingleLocator(ref result, ref attachmentLevel, AttachmentLevel.StartPortion, list[0], offset, startNode, skipStartNode);
				if (list.Count == 1)
				{
					result = resolvingLocatorState.AttachedAnchor;
					attachmentLevel = resolvingLocatorState.AttachmentLevel;
				}
				else
				{
					if (list.Count > 2)
					{
						AttachmentLevel attachmentLevel3 = AttachmentLevel.Unresolved;
						AttachmentLevel attachmentLevel4 = attachmentLevel;
						for (int i = 1; i < list.Count - 1; i++)
						{
							resolvingLocatorState = this.ResolveSingleLocator(ref result, ref attachmentLevel, AttachmentLevel.MiddlePortion, list[i], offset, startNode, skipStartNode);
							if (attachmentLevel3 == AttachmentLevel.Unresolved || (attachmentLevel & AttachmentLevel.MiddlePortion) != AttachmentLevel.Unresolved)
							{
								attachmentLevel3 = attachmentLevel;
							}
							attachmentLevel = attachmentLevel4;
						}
						attachmentLevel = attachmentLevel3;
					}
					else
					{
						flag = false;
					}
					resolvingLocatorState = this.ResolveSingleLocator(ref result, ref attachmentLevel, AttachmentLevel.EndPortion, list[list.Count - 1], offset, startNode, skipStartNode);
					if (!flag && attachmentLevel == AttachmentLevel.MiddlePortion)
					{
						attachmentLevel &= ~AttachmentLevel.MiddlePortion;
					}
					if (attachmentLevel == (AttachmentLevel.StartPortion | AttachmentLevel.EndPortion))
					{
						attachmentLevel = AttachmentLevel.Full;
					}
				}
			}
			else
			{
				attachmentLevel = AttachmentLevel.Unresolved;
			}
			return result;
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x00168A50 File Offset: 0x00167A50
		private LocatorManager.ResolvingLocatorState ResolveSingleLocator(ref object selection, ref AttachmentLevel attachmentLevel, AttachmentLevel attemptedLevel, ContentLocator locator, int offset, DependencyObject startNode, bool skipStartNode)
		{
			LocatorManager.ResolvingLocatorState resolvingLocatorState = new LocatorManager.ResolvingLocatorState();
			resolvingLocatorState.LocatorPartIndex = offset;
			resolvingLocatorState.ContentLocatorBase = locator;
			new PrePostDescendentsWalker<LocatorManager.ResolvingLocatorState>(TreeWalkPriority.VisualTree, new VisitedCallback<LocatorManager.ResolvingLocatorState>(this.ResolveLocatorPart), new VisitedCallback<LocatorManager.ResolvingLocatorState>(this.TerminateResolve), resolvingLocatorState).StartWalk(startNode, skipStartNode);
			if (resolvingLocatorState.AttachmentLevel == AttachmentLevel.Full && resolvingLocatorState.AttachedAnchor != null)
			{
				if (selection != null)
				{
					SelectionProcessor selectionProcessor = this.GetSelectionProcessor(selection.GetType());
					if (selectionProcessor != null)
					{
						object obj;
						if (selectionProcessor.MergeSelections(selection, resolvingLocatorState.AttachedAnchor, out obj))
						{
							selection = obj;
						}
						else
						{
							attachmentLevel &= ~attemptedLevel;
						}
					}
					else
					{
						attachmentLevel &= ~attemptedLevel;
					}
				}
				else
				{
					selection = resolvingLocatorState.AttachedAnchor;
				}
			}
			else
			{
				attachmentLevel &= ~attemptedLevel;
			}
			return resolvingLocatorState;
		}

		// Token: 0x06001B5B RID: 7003 RVA: 0x00168AFC File Offset: 0x00167AFC
		private bool ResolveLocatorPart(DependencyObject dependencyObject, LocatorManager.ResolvingLocatorState data, bool visitedViaVisualTree)
		{
			if (data.Finished)
			{
				return false;
			}
			ContentLocator contentLocatorBase = data.ContentLocatorBase;
			bool result = true;
			ContentLocatorPart contentLocatorPart = contentLocatorBase.Parts[data.LocatorPartIndex];
			if (contentLocatorPart == null)
			{
				result = false;
			}
			SubTreeProcessor subTreeProcessorForLocatorPart = this.GetSubTreeProcessorForLocatorPart(contentLocatorPart);
			if (subTreeProcessorForLocatorPart == null)
			{
				result = false;
			}
			if (contentLocatorPart != null && subTreeProcessorForLocatorPart != null)
			{
				DependencyObject dependencyObject2 = subTreeProcessorForLocatorPart.ResolveLocatorPart(contentLocatorPart, dependencyObject, out result);
				if (dependencyObject2 != null)
				{
					data.AttachmentLevel = AttachmentLevel.Incomplete;
					data.AttachedAnchor = dependencyObject2;
					result = true;
					data.LastNodeMatched = dependencyObject2;
					data.LocatorPartIndex++;
					if (data.LocatorPartIndex == contentLocatorBase.Parts.Count)
					{
						data.AttachmentLevel = AttachmentLevel.Full;
						data.AttachedAnchor = dependencyObject2;
						result = false;
					}
					else if (data.LocatorPartIndex == contentLocatorBase.Parts.Count - 1)
					{
						contentLocatorPart = contentLocatorBase.Parts[data.LocatorPartIndex];
						SelectionProcessor selectionProcessorForLocatorPart = this.GetSelectionProcessorForLocatorPart(contentLocatorPart);
						if (selectionProcessorForLocatorPart != null)
						{
							AttachmentLevel attachmentLevel;
							object obj = selectionProcessorForLocatorPart.ResolveLocatorPart(contentLocatorPart, dependencyObject2, out attachmentLevel);
							if (obj != null)
							{
								data.AttachmentLevel = attachmentLevel;
								data.AttachedAnchor = obj;
								result = false;
							}
							else
							{
								result = false;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x00168C13 File Offset: 0x00167C13
		private bool TerminateResolve(DependencyObject dependencyObject, LocatorManager.ResolvingLocatorState data, bool visitedViaVisualTree)
		{
			if (!data.Finished && data.LastNodeMatched == dependencyObject)
			{
				data.Finished = true;
			}
			return false;
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x00168C30 File Offset: 0x00167C30
		private IList<ContentLocatorBase> Merge(ContentLocatorBase initialLocator, IList<ContentLocatorBase> additionalLocators)
		{
			if (additionalLocators == null || additionalLocators.Count == 0)
			{
				return new List<ContentLocatorBase>(1)
				{
					initialLocator
				};
			}
			for (int i = 1; i < additionalLocators.Count; i++)
			{
				additionalLocators[i] = ((ContentLocatorBase)initialLocator.Clone()).Merge(additionalLocators[i]);
			}
			additionalLocators[0] = initialLocator.Merge(additionalLocators[0]);
			return additionalLocators;
		}

		// Token: 0x04000E05 RID: 3589
		public static readonly DependencyProperty SubTreeProcessorIdProperty = DependencyProperty.RegisterAttached("SubTreeProcessorId", typeof(string), typeof(LocatorManager), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior));

		// Token: 0x04000E06 RID: 3590
		private Hashtable _locatorPartHandlers;

		// Token: 0x04000E07 RID: 3591
		private Hashtable _subtreeProcessors;

		// Token: 0x04000E08 RID: 3592
		private Hashtable _selectionProcessors;

		// Token: 0x04000E09 RID: 3593
		private static readonly char[] Separators = new char[]
		{
			',',
			' ',
			';'
		};

		// Token: 0x04000E0A RID: 3594
		private AnnotationStore _internalStore;

		// Token: 0x02000A1C RID: 2588
		private class ProcessingTreeState
		{
			// Token: 0x060084F8 RID: 34040 RVA: 0x00327C10 File Offset: 0x00326C10
			public ProcessingTreeState()
			{
				this._calledProcessAnnotations.Push(false);
			}

			// Token: 0x17001DE8 RID: 7656
			// (get) Token: 0x060084F9 RID: 34041 RVA: 0x00327C3A File Offset: 0x00326C3A
			public List<IAttachedAnnotation> AttachedAnnotations
			{
				get
				{
					return this._attachedAnnotations;
				}
			}

			// Token: 0x17001DE9 RID: 7657
			// (get) Token: 0x060084FA RID: 34042 RVA: 0x00327C42 File Offset: 0x00326C42
			// (set) Token: 0x060084FB RID: 34043 RVA: 0x00327C4F File Offset: 0x00326C4F
			public bool CalledProcessAnnotations
			{
				get
				{
					return this._calledProcessAnnotations.Peek();
				}
				set
				{
					if (this._calledProcessAnnotations.Peek() != value)
					{
						this._calledProcessAnnotations.Pop();
						this._calledProcessAnnotations.Push(value);
					}
				}
			}

			// Token: 0x060084FC RID: 34044 RVA: 0x00327C77 File Offset: 0x00326C77
			public void Push()
			{
				this._calledProcessAnnotations.Push(false);
			}

			// Token: 0x060084FD RID: 34045 RVA: 0x00327C85 File Offset: 0x00326C85
			public bool Pop()
			{
				return this._calledProcessAnnotations.Pop();
			}

			// Token: 0x040040A6 RID: 16550
			private List<IAttachedAnnotation> _attachedAnnotations = new List<IAttachedAnnotation>();

			// Token: 0x040040A7 RID: 16551
			private Stack<bool> _calledProcessAnnotations = new Stack<bool>();
		}

		// Token: 0x02000A1D RID: 2589
		private class ResolvingLocatorState
		{
			// Token: 0x040040A8 RID: 16552
			public ContentLocator ContentLocatorBase;

			// Token: 0x040040A9 RID: 16553
			public int LocatorPartIndex;

			// Token: 0x040040AA RID: 16554
			public AttachmentLevel AttachmentLevel;

			// Token: 0x040040AB RID: 16555
			public object AttachedAnchor;

			// Token: 0x040040AC RID: 16556
			public bool Finished;

			// Token: 0x040040AD RID: 16557
			public object LastNodeMatched;
		}
	}
}
