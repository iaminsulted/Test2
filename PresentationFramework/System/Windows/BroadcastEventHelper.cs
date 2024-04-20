using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.PresentationFramework;

namespace System.Windows
{
	// Token: 0x02000346 RID: 838
	internal static class BroadcastEventHelper
	{
		// Token: 0x06001FCB RID: 8139 RVA: 0x0017324C File Offset: 0x0017224C
		internal static void AddLoadedCallback(DependencyObject d, DependencyObject logicalParent)
		{
			DispatcherOperationCallback dispatcherOperationCallback = new DispatcherOperationCallback(BroadcastEventHelper.BroadcastLoadedEvent);
			LoadedOrUnloadedOperation loadedOrUnloadedOperation = MediaContext.From(d.Dispatcher).AddLoadedOrUnloadedCallback(dispatcherOperationCallback, d);
			DispatcherOperation dispatcherOperation = d.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, dispatcherOperationCallback, d);
			d.SetValue(FrameworkElement.LoadedPendingPropertyKey, new object[]
			{
				loadedOrUnloadedOperation,
				dispatcherOperation,
				logicalParent
			});
		}

		// Token: 0x06001FCC RID: 8140 RVA: 0x001732A8 File Offset: 0x001722A8
		internal static void RemoveLoadedCallback(DependencyObject d, object[] loadedPending)
		{
			if (loadedPending != null)
			{
				d.ClearValue(FrameworkElement.LoadedPendingPropertyKey);
				DispatcherOperation dispatcherOperation = (DispatcherOperation)loadedPending[1];
				if (dispatcherOperation.Status == DispatcherOperationStatus.Pending)
				{
					dispatcherOperation.Abort();
				}
				MediaContext.From(d.Dispatcher).RemoveLoadedOrUnloadedCallback((LoadedOrUnloadedOperation)loadedPending[0]);
			}
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x001732F4 File Offset: 0x001722F4
		internal static void AddUnloadedCallback(DependencyObject d, DependencyObject logicalParent)
		{
			DispatcherOperationCallback dispatcherOperationCallback = new DispatcherOperationCallback(BroadcastEventHelper.BroadcastUnloadedEvent);
			LoadedOrUnloadedOperation loadedOrUnloadedOperation = MediaContext.From(d.Dispatcher).AddLoadedOrUnloadedCallback(dispatcherOperationCallback, d);
			DispatcherOperation dispatcherOperation = d.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, dispatcherOperationCallback, d);
			d.SetValue(FrameworkElement.UnloadedPendingPropertyKey, new object[]
			{
				loadedOrUnloadedOperation,
				dispatcherOperation,
				logicalParent
			});
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x00173350 File Offset: 0x00172350
		internal static void RemoveUnloadedCallback(DependencyObject d, object[] unloadedPending)
		{
			if (unloadedPending != null)
			{
				d.ClearValue(FrameworkElement.UnloadedPendingPropertyKey);
				DispatcherOperation dispatcherOperation = (DispatcherOperation)unloadedPending[1];
				if (dispatcherOperation.Status == DispatcherOperationStatus.Pending)
				{
					dispatcherOperation.Abort();
				}
				MediaContext.From(d.Dispatcher).RemoveLoadedOrUnloadedCallback((LoadedOrUnloadedOperation)unloadedPending[0]);
			}
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x0017339B File Offset: 0x0017239B
		internal static void BroadcastLoadedOrUnloadedEvent(DependencyObject d, DependencyObject oldParent, DependencyObject newParent)
		{
			if (oldParent == null && newParent != null)
			{
				if (BroadcastEventHelper.IsLoadedHelper(newParent))
				{
					BroadcastEventHelper.FireLoadedOnDescendentsHelper(d);
					return;
				}
			}
			else if (oldParent != null && newParent == null && BroadcastEventHelper.IsLoadedHelper(oldParent))
			{
				BroadcastEventHelper.FireUnloadedOnDescendentsHelper(d);
			}
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x001733C8 File Offset: 0x001723C8
		internal static object BroadcastLoadedEvent(object root)
		{
			DependencyObject dependencyObject = (DependencyObject)root;
			object[] loadedPending = (object[])dependencyObject.GetValue(FrameworkElement.LoadedPendingProperty);
			bool isLoaded = BroadcastEventHelper.IsLoadedHelper(dependencyObject);
			BroadcastEventHelper.RemoveLoadedCallback(dependencyObject, loadedPending);
			BroadcastEventHelper.BroadcastLoadedSynchronously(dependencyObject, isLoaded);
			return null;
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x00173401 File Offset: 0x00172401
		internal static void BroadcastLoadedSynchronously(DependencyObject rootDO, bool isLoaded)
		{
			if (!isLoaded)
			{
				BroadcastEventHelper.BroadcastEvent(rootDO, FrameworkElement.LoadedEvent);
			}
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x00173414 File Offset: 0x00172414
		internal static object BroadcastUnloadedEvent(object root)
		{
			DependencyObject dependencyObject = (DependencyObject)root;
			object[] unloadedPending = (object[])dependencyObject.GetValue(FrameworkElement.UnloadedPendingProperty);
			bool isLoaded = BroadcastEventHelper.IsLoadedHelper(dependencyObject);
			BroadcastEventHelper.RemoveUnloadedCallback(dependencyObject, unloadedPending);
			BroadcastEventHelper.BroadcastUnloadedSynchronously(dependencyObject, isLoaded);
			return null;
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x0017344D File Offset: 0x0017244D
		internal static void BroadcastUnloadedSynchronously(DependencyObject rootDO, bool isLoaded)
		{
			if (isLoaded)
			{
				BroadcastEventHelper.BroadcastEvent(rootDO, FrameworkElement.UnloadedEvent);
			}
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x00173460 File Offset: 0x00172460
		private static void BroadcastEvent(DependencyObject root, RoutedEvent routedEvent)
		{
			List<DependencyObject> list = new List<DependencyObject>();
			new DescendentsWalker<BroadcastEventHelper.BroadcastEventData>(TreeWalkPriority.VisualTree, BroadcastEventHelper.BroadcastDelegate, new BroadcastEventHelper.BroadcastEventData(root, routedEvent, list)).StartWalk(root);
			for (int i = 0; i < list.Count; i++)
			{
				DependencyObject dependencyObject = list[i];
				RoutedEventArgs args = new RoutedEventArgs(routedEvent, dependencyObject);
				FrameworkObject frameworkObject = new FrameworkObject(dependencyObject, true);
				if (routedEvent == FrameworkElement.LoadedEvent)
				{
					frameworkObject.OnLoaded(args);
				}
				else
				{
					frameworkObject.OnUnloaded(args);
				}
			}
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x001734D0 File Offset: 0x001724D0
		private static bool OnBroadcastCallback(DependencyObject d, BroadcastEventHelper.BroadcastEventData data, bool visitedViaVisualTree)
		{
			DependencyObject root = data.Root;
			RoutedEvent routedEvent = data.RoutedEvent;
			List<DependencyObject> eventRoute = data.EventRoute;
			if (FrameworkElement.DType.IsInstanceOfType(d))
			{
				FrameworkElement frameworkElement = (FrameworkElement)d;
				if (frameworkElement != root && routedEvent == FrameworkElement.LoadedEvent && frameworkElement.UnloadedPending != null)
				{
					frameworkElement.FireLoadedOnDescendentsInternal();
				}
				else if (frameworkElement != root && routedEvent == FrameworkElement.UnloadedEvent && frameworkElement.LoadedPending != null)
				{
					BroadcastEventHelper.RemoveLoadedCallback(frameworkElement, frameworkElement.LoadedPending);
				}
				else
				{
					if (frameworkElement != root)
					{
						if (routedEvent == FrameworkElement.LoadedEvent && frameworkElement.LoadedPending != null)
						{
							BroadcastEventHelper.RemoveLoadedCallback(frameworkElement, frameworkElement.LoadedPending);
						}
						else if (routedEvent == FrameworkElement.UnloadedEvent && frameworkElement.UnloadedPending != null)
						{
							BroadcastEventHelper.RemoveUnloadedCallback(frameworkElement, frameworkElement.UnloadedPending);
						}
					}
					if (frameworkElement.SubtreeHasLoadedChangeHandler)
					{
						frameworkElement.IsLoadedCache = (routedEvent == FrameworkElement.LoadedEvent);
						eventRoute.Add(frameworkElement);
						return true;
					}
				}
			}
			else
			{
				FrameworkContentElement frameworkContentElement = (FrameworkContentElement)d;
				if (frameworkContentElement != root && routedEvent == FrameworkElement.LoadedEvent && frameworkContentElement.UnloadedPending != null)
				{
					frameworkContentElement.FireLoadedOnDescendentsInternal();
				}
				else if (frameworkContentElement != root && routedEvent == FrameworkElement.UnloadedEvent && frameworkContentElement.LoadedPending != null)
				{
					BroadcastEventHelper.RemoveLoadedCallback(frameworkContentElement, frameworkContentElement.LoadedPending);
				}
				else
				{
					if (frameworkContentElement != root)
					{
						if (routedEvent == FrameworkElement.LoadedEvent && frameworkContentElement.LoadedPending != null)
						{
							BroadcastEventHelper.RemoveLoadedCallback(frameworkContentElement, frameworkContentElement.LoadedPending);
						}
						else if (routedEvent == FrameworkElement.UnloadedEvent && frameworkContentElement.UnloadedPending != null)
						{
							BroadcastEventHelper.RemoveUnloadedCallback(frameworkContentElement, frameworkContentElement.UnloadedPending);
						}
					}
					if (frameworkContentElement.SubtreeHasLoadedChangeHandler)
					{
						frameworkContentElement.IsLoadedCache = (routedEvent == FrameworkElement.LoadedEvent);
						eventRoute.Add(frameworkContentElement);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x00173665 File Offset: 0x00172665
		private static bool SubtreeHasLoadedChangeHandlerHelper(DependencyObject d)
		{
			if (FrameworkElement.DType.IsInstanceOfType(d))
			{
				return ((FrameworkElement)d).SubtreeHasLoadedChangeHandler;
			}
			return FrameworkContentElement.DType.IsInstanceOfType(d) && ((FrameworkContentElement)d).SubtreeHasLoadedChangeHandler;
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x0017369A File Offset: 0x0017269A
		private static void FireLoadedOnDescendentsHelper(DependencyObject d)
		{
			if (FrameworkElement.DType.IsInstanceOfType(d))
			{
				((FrameworkElement)d).FireLoadedOnDescendentsInternal();
				return;
			}
			((FrameworkContentElement)d).FireLoadedOnDescendentsInternal();
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x001736C0 File Offset: 0x001726C0
		private static void FireUnloadedOnDescendentsHelper(DependencyObject d)
		{
			if (FrameworkElement.DType.IsInstanceOfType(d))
			{
				((FrameworkElement)d).FireUnloadedOnDescendentsInternal();
				return;
			}
			((FrameworkContentElement)d).FireUnloadedOnDescendentsInternal();
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x001736E8 File Offset: 0x001726E8
		private static bool IsLoadedHelper(DependencyObject d)
		{
			FrameworkObject frameworkObject = new FrameworkObject(d);
			return frameworkObject.IsLoaded;
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x00173704 File Offset: 0x00172704
		internal static bool IsParentLoaded(DependencyObject d)
		{
			FrameworkObject frameworkObject = new FrameworkObject(d);
			DependencyObject effectiveParent = frameworkObject.EffectiveParent;
			if (effectiveParent != null)
			{
				return BroadcastEventHelper.IsLoadedHelper(effectiveParent);
			}
			Visual visual;
			if ((visual = (d as Visual)) != null)
			{
				return SafeSecurityHelper.IsConnectedToPresentationSource(visual);
			}
			Visual3D reference;
			if ((reference = (d as Visual3D)) != null)
			{
				visual = VisualTreeHelper.GetContainingVisual2D(reference);
				return visual != null && SafeSecurityHelper.IsConnectedToPresentationSource(visual);
			}
			return false;
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x0017375C File Offset: 0x0017275C
		internal static FrameworkElementFactory GetFEFTreeRoot(DependencyObject templatedParent)
		{
			FrameworkObject frameworkObject = new FrameworkObject(templatedParent, true);
			return frameworkObject.FE.TemplateInternal.VisualTree;
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x00173783 File Offset: 0x00172783
		internal static void AddOrRemoveHasLoadedChangeHandlerFlag(DependencyObject d, DependencyObject oldParent, DependencyObject newParent)
		{
			if (BroadcastEventHelper.SubtreeHasLoadedChangeHandlerHelper(d))
			{
				if (oldParent == null && newParent != null)
				{
					BroadcastEventHelper.AddHasLoadedChangeHandlerFlagInAncestry(newParent);
					return;
				}
				if (oldParent != null && newParent == null)
				{
					BroadcastEventHelper.RemoveHasLoadedChangeHandlerFlagInAncestry(oldParent);
				}
			}
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x001737A6 File Offset: 0x001727A6
		internal static void AddHasLoadedChangeHandlerFlagInAncestry(DependencyObject d)
		{
			BroadcastEventHelper.UpdateHasLoadedChangeHandlerFlagInAncestry(d, true);
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x001737AF File Offset: 0x001727AF
		internal static void RemoveHasLoadedChangeHandlerFlagInAncestry(DependencyObject d)
		{
			BroadcastEventHelper.UpdateHasLoadedChangeHandlerFlagInAncestry(d, false);
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x001737B8 File Offset: 0x001727B8
		private static bool AreThereLoadedChangeHandlersInSubtree(ref FrameworkObject fo)
		{
			if (!fo.IsValid)
			{
				return false;
			}
			if (fo.ThisHasLoadedChangeEventHandler)
			{
				return true;
			}
			if (fo.IsFE)
			{
				Visual fe = fo.FE;
				int childrenCount = VisualTreeHelper.GetChildrenCount(fe);
				for (int i = 0; i < childrenCount; i++)
				{
					FrameworkElement frameworkElement = VisualTreeHelper.GetChild(fe, i) as FrameworkElement;
					if (frameworkElement != null && frameworkElement.SubtreeHasLoadedChangeHandler)
					{
						return true;
					}
				}
			}
			foreach (object obj in LogicalTreeHelper.GetChildren(fo.DO))
			{
				DependencyObject dependencyObject = obj as DependencyObject;
				if (dependencyObject != null && BroadcastEventHelper.SubtreeHasLoadedChangeHandlerHelper(dependencyObject))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x00173880 File Offset: 0x00172880
		private static void UpdateHasLoadedChangeHandlerFlagInAncestry(DependencyObject d, bool addHandler)
		{
			FrameworkObject frameworkObject = new FrameworkObject(d);
			if (!addHandler && BroadcastEventHelper.AreThereLoadedChangeHandlersInSubtree(ref frameworkObject))
			{
				return;
			}
			if (frameworkObject.IsValid)
			{
				if (frameworkObject.SubtreeHasLoadedChangeHandler != addHandler)
				{
					DependencyObject dependencyObject = frameworkObject.IsFE ? VisualTreeHelper.GetParent(frameworkObject.FE) : null;
					DependencyObject parent = frameworkObject.Parent;
					DependencyObject dependencyObject2 = null;
					frameworkObject.SubtreeHasLoadedChangeHandler = addHandler;
					if (dependencyObject != null)
					{
						BroadcastEventHelper.UpdateHasLoadedChangeHandlerFlagInAncestry(dependencyObject, addHandler);
						dependencyObject2 = dependencyObject;
					}
					if (parent != null && parent != dependencyObject)
					{
						BroadcastEventHelper.UpdateHasLoadedChangeHandlerFlagInAncestry(parent, addHandler);
						if (frameworkObject.IsFCE)
						{
							dependencyObject2 = parent;
						}
					}
					if (parent == null && dependencyObject == null)
					{
						dependencyObject2 = Helper.FindMentor(frameworkObject.DO.InheritanceContext);
						if (dependencyObject2 != null)
						{
							frameworkObject.ChangeSubtreeHasLoadedChangedHandler(dependencyObject2);
						}
					}
					if (addHandler)
					{
						if (frameworkObject.IsFE)
						{
							BroadcastEventHelper.UpdateIsLoadedCache(frameworkObject.FE, dependencyObject2);
							return;
						}
						BroadcastEventHelper.UpdateIsLoadedCache(frameworkObject.FCE, dependencyObject2);
						return;
					}
				}
			}
			else
			{
				DependencyObject dependencyObject3 = null;
				Visual reference;
				ContentElement reference2;
				Visual3D reference3;
				if ((reference = (d as Visual)) != null)
				{
					dependencyObject3 = VisualTreeHelper.GetParent(reference);
				}
				else if ((reference2 = (d as ContentElement)) != null)
				{
					dependencyObject3 = ContentOperations.GetParent(reference2);
				}
				else if ((reference3 = (d as Visual3D)) != null)
				{
					dependencyObject3 = VisualTreeHelper.GetParent(reference3);
				}
				if (dependencyObject3 != null)
				{
					BroadcastEventHelper.UpdateHasLoadedChangeHandlerFlagInAncestry(dependencyObject3, addHandler);
				}
			}
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x001739A5 File Offset: 0x001729A5
		private static void UpdateIsLoadedCache(FrameworkElement fe, DependencyObject parent)
		{
			if (fe.GetValue(FrameworkElement.LoadedPendingProperty) != null)
			{
				fe.IsLoadedCache = false;
				return;
			}
			if (parent != null)
			{
				fe.IsLoadedCache = BroadcastEventHelper.IsLoadedHelper(parent);
				return;
			}
			if (SafeSecurityHelper.IsConnectedToPresentationSource(fe))
			{
				fe.IsLoadedCache = true;
				return;
			}
			fe.IsLoadedCache = false;
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x001739E3 File Offset: 0x001729E3
		private static void UpdateIsLoadedCache(FrameworkContentElement fce, DependencyObject parent)
		{
			if (fce.GetValue(FrameworkElement.LoadedPendingProperty) == null)
			{
				fce.IsLoadedCache = BroadcastEventHelper.IsLoadedHelper(parent);
				return;
			}
			fce.IsLoadedCache = false;
		}

		// Token: 0x04000FA4 RID: 4004
		private static VisitedCallback<BroadcastEventHelper.BroadcastEventData> BroadcastDelegate = new VisitedCallback<BroadcastEventHelper.BroadcastEventData>(BroadcastEventHelper.OnBroadcastCallback);

		// Token: 0x02000A78 RID: 2680
		private struct BroadcastEventData
		{
			// Token: 0x06008651 RID: 34385 RVA: 0x0032A469 File Offset: 0x00329469
			internal BroadcastEventData(DependencyObject root, RoutedEvent routedEvent, List<DependencyObject> eventRoute)
			{
				this.Root = root;
				this.RoutedEvent = routedEvent;
				this.EventRoute = eventRoute;
			}

			// Token: 0x04004176 RID: 16758
			internal DependencyObject Root;

			// Token: 0x04004177 RID: 16759
			internal RoutedEvent RoutedEvent;

			// Token: 0x04004178 RID: 16760
			internal List<DependencyObject> EventRoute;
		}
	}
}
