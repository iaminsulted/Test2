using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;

namespace MS.Internal.AppModel
{
	// Token: 0x0200028F RID: 655
	internal sealed class OleCmdHelper : MarshalByRefObject
	{
		// Token: 0x060018D2 RID: 6354 RVA: 0x001617F7 File Offset: 0x001607F7
		internal OleCmdHelper()
		{
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x00161800 File Offset: 0x00160800
		internal void QueryStatus(Guid guidCmdGroup, uint cmdId, ref uint flags)
		{
			if (Application.Current == null || Application.IsShuttingDown)
			{
				Marshal.ThrowExceptionForHR(-2147467259);
			}
			IDictionary oleCmdMappingTable = this.GetOleCmdMappingTable(guidCmdGroup);
			if (oleCmdMappingTable == null)
			{
				Marshal.ThrowExceptionForHR(-2147221244);
			}
			CommandWithArgument commandWithArgument = oleCmdMappingTable[cmdId] as CommandWithArgument;
			if (commandWithArgument == null)
			{
				flags = 0U;
				return;
			}
			flags = (((bool)Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, new DispatcherOperationCallback(this.QueryEnabled), commandWithArgument)) ? 3U : 1U);
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x00161880 File Offset: 0x00160880
		private object QueryEnabled(object command)
		{
			if (Application.Current.MainWindow == null)
			{
				return false;
			}
			IInputElement inputElement = FocusManager.GetFocusedElement(Application.Current.MainWindow);
			if (inputElement == null)
			{
				inputElement = Application.Current.MainWindow;
			}
			return BooleanBoxes.Box(((CommandWithArgument)command).QueryEnabled(inputElement, null));
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x001618D0 File Offset: 0x001608D0
		internal void ExecCommand(Guid guidCmdGroup, uint commandId, object arg)
		{
			if (Application.Current == null || Application.IsShuttingDown)
			{
				Marshal.ThrowExceptionForHR(-2147467259);
			}
			int num = (int)Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, new DispatcherOperationCallback(this.ExecCommandCallback), new object[]
			{
				guidCmdGroup,
				commandId,
				arg
			});
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x00161940 File Offset: 0x00160940
		private object ExecCommandCallback(object arguments)
		{
			object[] array = (object[])arguments;
			Invariant.Assert(array.Length == 3);
			Guid guidCmdGroup = (Guid)array[0];
			uint num = (uint)array[1];
			object argument = array[2];
			IDictionary oleCmdMappingTable = this.GetOleCmdMappingTable(guidCmdGroup);
			if (oleCmdMappingTable == null)
			{
				return -2147221244;
			}
			CommandWithArgument commandWithArgument = oleCmdMappingTable[num] as CommandWithArgument;
			if (commandWithArgument == null)
			{
				return -2147221248;
			}
			if (Application.Current.MainWindow == null)
			{
				return -2147221247;
			}
			IInputElement inputElement = FocusManager.GetFocusedElement(Application.Current.MainWindow);
			if (inputElement == null)
			{
				inputElement = Application.Current.MainWindow;
			}
			return commandWithArgument.Execute(inputElement, argument) ? 0 : -2147221247;
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x001619FC File Offset: 0x001609FC
		private IDictionary GetOleCmdMappingTable(Guid guidCmdGroup)
		{
			IDictionary result = null;
			if (guidCmdGroup.Equals(OleCmdHelper.CGID_ApplicationCommands))
			{
				this.EnsureApplicationCommandsTable();
				result = this._applicationCommandsMappingTable.Value;
			}
			else if (guidCmdGroup.Equals(Guid.Empty))
			{
				this.EnsureOleCmdMappingTable();
				result = this._oleCmdMappingTable.Value;
			}
			else if (guidCmdGroup.Equals(OleCmdHelper.CGID_EditingCommands))
			{
				this.EnsureEditingCommandsTable();
				result = this._editingCommandsMappingTable.Value;
			}
			return result;
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x00161A70 File Offset: 0x00160A70
		private void EnsureOleCmdMappingTable()
		{
			if (this._oleCmdMappingTable.Value == null)
			{
				this._oleCmdMappingTable.Value = new SortedList(10);
				this._oleCmdMappingTable.Value.Add(3U, new CommandWithArgument(ApplicationCommands.Save));
				this._oleCmdMappingTable.Value.Add(4U, new CommandWithArgument(ApplicationCommands.SaveAs));
				this._oleCmdMappingTable.Value.Add(6U, new CommandWithArgument(ApplicationCommands.Print));
				this._oleCmdMappingTable.Value.Add(11U, new CommandWithArgument(ApplicationCommands.Cut));
				this._oleCmdMappingTable.Value.Add(12U, new CommandWithArgument(ApplicationCommands.Copy));
				this._oleCmdMappingTable.Value.Add(13U, new CommandWithArgument(ApplicationCommands.Paste));
				this._oleCmdMappingTable.Value.Add(10U, new CommandWithArgument(ApplicationCommands.Properties));
				this._oleCmdMappingTable.Value.Add(22U, new CommandWithArgument(NavigationCommands.Refresh));
				this._oleCmdMappingTable.Value.Add(23U, new CommandWithArgument(NavigationCommands.BrowseStop));
			}
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x00161BC8 File Offset: 0x00160BC8
		private void EnsureApplicationCommandsTable()
		{
			if (this._applicationCommandsMappingTable.Value == null)
			{
				this._applicationCommandsMappingTable.Value = new Hashtable(19);
				this._applicationCommandsMappingTable.Value.Add(8001U, new CommandWithArgument(ApplicationCommands.Cut));
				this._applicationCommandsMappingTable.Value.Add(8002U, new CommandWithArgument(ApplicationCommands.Copy));
				this._applicationCommandsMappingTable.Value.Add(8003U, new CommandWithArgument(ApplicationCommands.Paste));
				this._applicationCommandsMappingTable.Value.Add(8004U, new CommandWithArgument(ApplicationCommands.SelectAll));
				this._applicationCommandsMappingTable.Value.Add(8005U, new CommandWithArgument(ApplicationCommands.Find));
				this._applicationCommandsMappingTable.Value.Add(8016U, new CommandWithArgument(NavigationCommands.Refresh));
				this._applicationCommandsMappingTable.Value.Add(8015U, new CommandWithArgument(NavigationCommands.BrowseStop));
				this._applicationCommandsMappingTable.Value.Add(8007U, new CommandWithArgument(DocumentApplicationDocumentViewer.Sign));
				this._applicationCommandsMappingTable.Value.Add(8008U, new CommandWithArgument(DocumentApplicationDocumentViewer.RequestSigners));
				this._applicationCommandsMappingTable.Value.Add(8009U, new CommandWithArgument(DocumentApplicationDocumentViewer.ShowSignatureSummary));
				this._applicationCommandsMappingTable.Value.Add(8011U, new CommandWithArgument(DocumentApplicationDocumentViewer.ShowRMPublishingUI));
				this._applicationCommandsMappingTable.Value.Add(8012U, new CommandWithArgument(DocumentApplicationDocumentViewer.ShowRMPermissions));
				this._applicationCommandsMappingTable.Value.Add(8013U, new CommandWithArgument(DocumentApplicationDocumentViewer.ShowRMCredentialManager));
				this._applicationCommandsMappingTable.Value.Add(8019U, new CommandWithArgument(NavigationCommands.IncreaseZoom));
				this._applicationCommandsMappingTable.Value.Add(8020U, new CommandWithArgument(NavigationCommands.DecreaseZoom));
				this._applicationCommandsMappingTable.Value.Add(8021U, new CommandWithArgument(NavigationCommands.Zoom, 400));
				this._applicationCommandsMappingTable.Value.Add(8022U, new CommandWithArgument(NavigationCommands.Zoom, 250));
				this._applicationCommandsMappingTable.Value.Add(8023U, new CommandWithArgument(NavigationCommands.Zoom, 150));
				this._applicationCommandsMappingTable.Value.Add(8024U, new CommandWithArgument(NavigationCommands.Zoom, 100));
				this._applicationCommandsMappingTable.Value.Add(8025U, new CommandWithArgument(NavigationCommands.Zoom, 75));
				this._applicationCommandsMappingTable.Value.Add(8026U, new CommandWithArgument(NavigationCommands.Zoom, 50));
				this._applicationCommandsMappingTable.Value.Add(8027U, new CommandWithArgument(NavigationCommands.Zoom, 25));
				this._applicationCommandsMappingTable.Value.Add(8028U, new CommandWithArgument(DocumentViewer.FitToWidthCommand));
				this._applicationCommandsMappingTable.Value.Add(8029U, new CommandWithArgument(DocumentViewer.FitToHeightCommand));
				this._applicationCommandsMappingTable.Value.Add(8030U, new CommandWithArgument(DocumentViewer.FitToMaxPagesAcrossCommand, 2));
				this._applicationCommandsMappingTable.Value.Add(8031U, new CommandWithArgument(DocumentViewer.ViewThumbnailsCommand));
			}
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x00161FE0 File Offset: 0x00160FE0
		private void EnsureEditingCommandsTable()
		{
			if (this._editingCommandsMappingTable.Value == null)
			{
				this._editingCommandsMappingTable.Value = new SortedList(2);
				this._editingCommandsMappingTable.Value.Add(1U, new CommandWithArgument(EditingCommands.Backspace));
				this._editingCommandsMappingTable.Value.Add(2U, new CommandWithArgument(EditingCommands.Delete));
			}
		}

		// Token: 0x04000D64 RID: 3428
		internal const int OLECMDERR_E_NOTSUPPORTED = -2147221248;

		// Token: 0x04000D65 RID: 3429
		internal const int OLECMDERR_E_DISABLED = -2147221247;

		// Token: 0x04000D66 RID: 3430
		internal const int OLECMDERR_E_UNKNOWNGROUP = -2147221244;

		// Token: 0x04000D67 RID: 3431
		internal const uint CommandUnsupported = 0U;

		// Token: 0x04000D68 RID: 3432
		internal const uint CommandEnabled = 3U;

		// Token: 0x04000D69 RID: 3433
		internal const uint CommandDisabled = 1U;

		// Token: 0x04000D6A RID: 3434
		internal static readonly Guid CGID_ApplicationCommands = new Guid(3955001955U, 34137, 18578, 151, 168, 49, 233, 176, 233, 133, 145);

		// Token: 0x04000D6B RID: 3435
		internal static readonly Guid CGID_EditingCommands = new Guid(209178181, 3356, 20266, 178, 147, 237, 213, 226, 126, 186, 71);

		// Token: 0x04000D6C RID: 3436
		private SecurityCriticalDataForSet<SortedList> _oleCmdMappingTable;

		// Token: 0x04000D6D RID: 3437
		private SecurityCriticalDataForSet<Hashtable> _applicationCommandsMappingTable;

		// Token: 0x04000D6E RID: 3438
		private SecurityCriticalDataForSet<SortedList> _editingCommandsMappingTable;
	}
}
