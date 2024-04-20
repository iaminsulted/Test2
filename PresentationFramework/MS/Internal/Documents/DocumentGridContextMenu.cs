using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MS.Internal.Documents
{
	// Token: 0x020001BB RID: 443
	internal static class DocumentGridContextMenu
	{
		// Token: 0x06000EFA RID: 3834 RVA: 0x0013BC14 File Offset: 0x0013AC14
		internal static void RegisterClassHandler()
		{
			EventManager.RegisterClassHandler(typeof(DocumentGrid), FrameworkElement.ContextMenuOpeningEvent, new ContextMenuEventHandler(DocumentGridContextMenu.OnContextMenuOpening));
			EventManager.RegisterClassHandler(typeof(DocumentApplicationDocumentViewer), FrameworkElement.ContextMenuOpeningEvent, new ContextMenuEventHandler(DocumentGridContextMenu.OnDocumentViewerContextMenuOpening));
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x0013BC64 File Offset: 0x0013AC64
		private static void OnDocumentViewerContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			if (e.CursorLeft == -1.0)
			{
				DocumentViewer documentViewer = sender as DocumentViewer;
				if (documentViewer != null && documentViewer.ScrollViewer != null)
				{
					DocumentGridContextMenu.OnContextMenuOpening(documentViewer.ScrollViewer.Content, e);
				}
			}
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x0013BCA8 File Offset: 0x0013ACA8
		private static void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			DocumentGrid documentGrid = sender as DocumentGrid;
			if (documentGrid == null)
			{
				return;
			}
			if (!(documentGrid.DocumentViewerOwner is DocumentApplicationDocumentViewer))
			{
				return;
			}
			if (documentGrid.DocumentViewerOwner.ContextMenu != null || documentGrid.DocumentViewerOwner.ScrollViewer.ContextMenu != null)
			{
				return;
			}
			ContextMenu contextMenu = documentGrid.ContextMenu;
			if (documentGrid.ReadLocalValue(FrameworkElement.ContextMenuProperty) == null)
			{
				return;
			}
			if (contextMenu != null)
			{
				return;
			}
			contextMenu = new DocumentGridContextMenu.ViewerContextMenu();
			contextMenu.Placement = PlacementMode.RelativePoint;
			contextMenu.PlacementTarget = documentGrid;
			((DocumentGridContextMenu.ViewerContextMenu)contextMenu).AddMenuItems(documentGrid, e.UserInitiated);
			Point position;
			if (e.CursorLeft == -1.0)
			{
				position = new Point(0.5 * documentGrid.ViewportWidth, 0.5 * documentGrid.ViewportHeight);
			}
			else
			{
				position = Mouse.GetPosition(documentGrid);
			}
			contextMenu.HorizontalOffset = position.X;
			contextMenu.VerticalOffset = position.Y;
			contextMenu.IsOpen = true;
			e.Handled = true;
		}

		// Token: 0x04000A7C RID: 2684
		private const double KeyboardInvokedSentinel = -1.0;

		// Token: 0x020009D6 RID: 2518
		private class ViewerContextMenu : ContextMenu
		{
			// Token: 0x0600840C RID: 33804 RVA: 0x00324E08 File Offset: 0x00323E08
			internal void AddMenuItems(DocumentGrid dg, bool userInitiated)
			{
				base.Name = "ViewerContextMenu";
				this.SetMenuProperties(new DocumentGridContextMenu.EditorMenuItem(), dg, ApplicationCommands.Copy);
				this.SetMenuProperties(new MenuItem(), dg, ApplicationCommands.SelectAll);
				this.AddSeparator();
				this.SetMenuProperties(new MenuItem(), dg, NavigationCommands.PreviousPage, SR.Get("DocumentApplicationContextMenuPreviousPageHeader"), SR.Get("DocumentApplicationContextMenuPreviousPageInputGesture"));
				this.SetMenuProperties(new MenuItem(), dg, NavigationCommands.NextPage, SR.Get("DocumentApplicationContextMenuNextPageHeader"), SR.Get("DocumentApplicationContextMenuNextPageInputGesture"));
				this.SetMenuProperties(new MenuItem(), dg, NavigationCommands.FirstPage, null, SR.Get("DocumentApplicationContextMenuFirstPageInputGesture"));
				this.SetMenuProperties(new MenuItem(), dg, NavigationCommands.LastPage, null, SR.Get("DocumentApplicationContextMenuLastPageInputGesture"));
				this.AddSeparator();
				this.SetMenuProperties(new MenuItem(), dg, ApplicationCommands.Print);
			}

			// Token: 0x0600840D RID: 33805 RVA: 0x00324EE1 File Offset: 0x00323EE1
			private void AddSeparator()
			{
				base.Items.Add(new Separator());
			}

			// Token: 0x0600840E RID: 33806 RVA: 0x00324EF4 File Offset: 0x00323EF4
			private void SetMenuProperties(MenuItem menuItem, DocumentGrid dg, RoutedUICommand command)
			{
				this.SetMenuProperties(menuItem, dg, command, null, null);
			}

			// Token: 0x0600840F RID: 33807 RVA: 0x00324F04 File Offset: 0x00323F04
			private void SetMenuProperties(MenuItem menuItem, DocumentGrid dg, RoutedUICommand command, string header, string inputGestureText)
			{
				menuItem.Command = command;
				menuItem.CommandTarget = dg.DocumentViewerOwner;
				if (header == null)
				{
					menuItem.Header = command.Text;
				}
				else
				{
					menuItem.Header = header;
				}
				if (inputGestureText != null)
				{
					menuItem.InputGestureText = inputGestureText;
				}
				menuItem.Name = "ViewerContextMenu_" + command.Name;
				base.Items.Add(menuItem);
			}
		}

		// Token: 0x020009D7 RID: 2519
		private class EditorMenuItem : MenuItem
		{
			// Token: 0x06008411 RID: 33809 RVA: 0x00324F75 File Offset: 0x00323F75
			internal EditorMenuItem()
			{
			}

			// Token: 0x06008412 RID: 33810 RVA: 0x00324F7D File Offset: 0x00323F7D
			internal override void OnClickCore(bool userInitiated)
			{
				base.OnClickImpl(userInitiated);
			}
		}
	}
}
