using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Threading;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x020006C4 RID: 1732
	internal class TextServicesHost : DispatcherObject
	{
		// Token: 0x060059EF RID: 23023 RVA: 0x0027EC25 File Offset: 0x0027DC25
		internal TextServicesHost()
		{
		}

		// Token: 0x060059F0 RID: 23024 RVA: 0x0027EC2D File Offset: 0x0027DC2D
		internal void RegisterTextStore(TextStore textstore)
		{
			this._RegisterTextStore(textstore);
			this._thread = Thread.CurrentThread;
		}

		// Token: 0x060059F1 RID: 23025 RVA: 0x0027EC41 File Offset: 0x0027DC41
		internal void UnregisterTextStore(TextStore textstore, bool finalizer)
		{
			if (!finalizer)
			{
				this.OnUnregisterTextStore(textstore);
				return;
			}
			if (!this._isDispatcherShutdownFinished)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.OnUnregisterTextStore), textstore);
			}
		}

		// Token: 0x060059F2 RID: 23026 RVA: 0x0027EC72 File Offset: 0x0027DC72
		internal void RegisterWinEventSink(TextStore textstore)
		{
			if (this._winEvent == null)
			{
				this._winEvent = new MoveSizeWinEventHandler();
				this._winEvent.Start();
			}
			this._winEvent.RegisterTextStore(textstore);
		}

		// Token: 0x060059F3 RID: 23027 RVA: 0x0027EC9E File Offset: 0x0027DC9E
		internal void UnregisterWinEventSink(TextStore textstore)
		{
			this._winEvent.UnregisterTextStore(textstore);
			if (this._winEvent.TextStoreCount == 0)
			{
				this._winEvent.Stop();
				this._winEvent.Clear();
				this._winEvent = null;
			}
		}

		// Token: 0x060059F4 RID: 23028 RVA: 0x0027ECD8 File Offset: 0x0027DCD8
		internal static void StartTransitoryExtension(TextStore textstore)
		{
			UnsafeNativeMethods.ITfCompartmentMgr tfCompartmentMgr = textstore.DocumentManager as UnsafeNativeMethods.ITfCompartmentMgr;
			Guid guid = UnsafeNativeMethods.GUID_COMPARTMENT_TRANSITORYEXTENSION;
			UnsafeNativeMethods.ITfCompartment tfCompartment;
			tfCompartmentMgr.GetCompartment(ref guid, out tfCompartment);
			object obj = 2;
			tfCompartment.SetValue(0, ref obj);
			guid = UnsafeNativeMethods.IID_ITfTransitoryExtensionSink;
			UnsafeNativeMethods.ITfSource tfSource = textstore.DocumentManager as UnsafeNativeMethods.ITfSource;
			if (tfSource != null)
			{
				int transitoryExtensionSinkCookie;
				tfSource.AdviseSink(ref guid, textstore, out transitoryExtensionSinkCookie);
				textstore.TransitoryExtensionSinkCookie = transitoryExtensionSinkCookie;
			}
			Marshal.ReleaseComObject(tfCompartment);
		}

		// Token: 0x060059F5 RID: 23029 RVA: 0x0027ED40 File Offset: 0x0027DD40
		internal static void StopTransitoryExtension(TextStore textstore)
		{
			UnsafeNativeMethods.ITfCompartmentMgr tfCompartmentMgr = textstore.DocumentManager as UnsafeNativeMethods.ITfCompartmentMgr;
			if (textstore.TransitoryExtensionSinkCookie != -1)
			{
				UnsafeNativeMethods.ITfSource tfSource = textstore.DocumentManager as UnsafeNativeMethods.ITfSource;
				if (tfSource != null)
				{
					tfSource.UnadviseSink(textstore.TransitoryExtensionSinkCookie);
				}
				textstore.TransitoryExtensionSinkCookie = -1;
			}
			Guid guid_COMPARTMENT_TRANSITORYEXTENSION = UnsafeNativeMethods.GUID_COMPARTMENT_TRANSITORYEXTENSION;
			UnsafeNativeMethods.ITfCompartment tfCompartment;
			tfCompartmentMgr.GetCompartment(ref guid_COMPARTMENT_TRANSITORYEXTENSION, out tfCompartment);
			object obj = 0;
			tfCompartment.SetValue(0, ref obj);
			Marshal.ReleaseComObject(tfCompartment);
		}

		// Token: 0x170014D4 RID: 5332
		// (get) Token: 0x060059F6 RID: 23030 RVA: 0x0027EDAC File Offset: 0x0027DDAC
		internal static TextServicesHost Current
		{
			get
			{
				TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
				if (threadLocalStore.TextServicesHost == null)
				{
					threadLocalStore.TextServicesHost = new TextServicesHost();
				}
				return threadLocalStore.TextServicesHost;
			}
		}

		// Token: 0x170014D5 RID: 5333
		// (get) Token: 0x060059F7 RID: 23031 RVA: 0x0027EDD8 File Offset: 0x0027DDD8
		internal UnsafeNativeMethods.ITfThreadMgr ThreadManager
		{
			get
			{
				if (this._threadManager == null)
				{
					return null;
				}
				return this._threadManager.Value;
			}
		}

		// Token: 0x060059F8 RID: 23032 RVA: 0x0027EDF0 File Offset: 0x0027DDF0
		private object OnUnregisterTextStore(object arg)
		{
			if (this._threadManager == null || this._threadManager.Value == null)
			{
				return null;
			}
			TextStore textStore = (TextStore)arg;
			if (textStore.ThreadFocusCookie != -1)
			{
				(this._threadManager.Value as UnsafeNativeMethods.ITfSource).UnadviseSink(textStore.ThreadFocusCookie);
				textStore.ThreadFocusCookie = -1;
			}
			UnsafeNativeMethods.ITfContext tfContext;
			textStore.DocumentManager.GetBase(out tfContext);
			if (tfContext != null)
			{
				if (textStore.EditSinkCookie != -1)
				{
					(tfContext as UnsafeNativeMethods.ITfSource).UnadviseSink(textStore.EditSinkCookie);
					textStore.EditSinkCookie = -1;
				}
				Marshal.ReleaseComObject(tfContext);
			}
			textStore.DocumentManager.Pop(UnsafeNativeMethods.PopFlags.TF_POPF_ALL);
			Marshal.ReleaseComObject(textStore.DocumentManager);
			textStore.DocumentManager = null;
			this._registeredtextstorecount--;
			if (this._isDispatcherShutdownFinished && this._registeredtextstorecount == 0)
			{
				this.DeactivateThreadManager();
			}
			return null;
		}

		// Token: 0x060059F9 RID: 23033 RVA: 0x0027EEC2 File Offset: 0x0027DEC2
		private void OnDispatcherShutdownFinished(object sender, EventArgs args)
		{
			base.Dispatcher.ShutdownFinished -= this.OnDispatcherShutdownFinished;
			if (this._registeredtextstorecount == 0)
			{
				this.DeactivateThreadManager();
			}
			this._isDispatcherShutdownFinished = true;
		}

		// Token: 0x060059FA RID: 23034 RVA: 0x0027EEF0 File Offset: 0x0027DEF0
		private void _RegisterTextStore(TextStore textstore)
		{
			int editCookie = -1;
			int threadFocusCookie = -1;
			int editSinkCookie = -1;
			if (this._threadManager == null)
			{
				this._threadManager = new SecurityCriticalDataClass<UnsafeNativeMethods.ITfThreadMgr>(TextServicesLoader.Load());
				if (this._threadManager.Value == null)
				{
					this._threadManager = null;
					return;
				}
				int value;
				this._threadManager.Value.Activate(out value);
				this._clientId = new SecurityCriticalData<int>(value);
				base.Dispatcher.ShutdownFinished += this.OnDispatcherShutdownFinished;
			}
			UnsafeNativeMethods.ITfDocumentMgr tfDocumentMgr;
			this._threadManager.Value.CreateDocumentMgr(out tfDocumentMgr);
			UnsafeNativeMethods.ITfContext tfContext;
			tfDocumentMgr.CreateContext(this._clientId.Value, (UnsafeNativeMethods.CreateContextFlags)0, textstore, out tfContext, out editCookie);
			tfDocumentMgr.Push(tfContext);
			if (textstore != null)
			{
				Guid guid = UnsafeNativeMethods.IID_ITfThreadFocusSink;
				(this._threadManager.Value as UnsafeNativeMethods.ITfSource).AdviseSink(ref guid, textstore, out threadFocusCookie);
			}
			if (textstore != null)
			{
				Guid guid = UnsafeNativeMethods.IID_ITfTextEditSink;
				(tfContext as UnsafeNativeMethods.ITfSource).AdviseSink(ref guid, textstore, out editSinkCookie);
			}
			Marshal.ReleaseComObject(tfContext);
			textstore.DocumentManager = tfDocumentMgr;
			textstore.ThreadFocusCookie = threadFocusCookie;
			textstore.EditSinkCookie = editSinkCookie;
			textstore.EditCookie = editCookie;
			if (textstore.UiScope.IsKeyboardFocused)
			{
				textstore.OnGotFocus();
			}
			this._registeredtextstorecount++;
		}

		// Token: 0x060059FB RID: 23035 RVA: 0x0027F01C File Offset: 0x0027E01C
		private void DeactivateThreadManager()
		{
			if (this._threadManager != null)
			{
				if (this._threadManager.Value != null)
				{
					if (this._thread == Thread.CurrentThread || Environment.OSVersion.Version.Major >= 6)
					{
						this._threadManager.Value.Deactivate();
					}
					Marshal.ReleaseComObject(this._threadManager.Value);
				}
				this._threadManager = null;
			}
			TextEditor._ThreadLocalStore.TextServicesHost = null;
		}

		// Token: 0x04003025 RID: 12325
		private int _registeredtextstorecount;

		// Token: 0x04003026 RID: 12326
		private SecurityCriticalData<int> _clientId;

		// Token: 0x04003027 RID: 12327
		private SecurityCriticalDataClass<UnsafeNativeMethods.ITfThreadMgr> _threadManager;

		// Token: 0x04003028 RID: 12328
		private bool _isDispatcherShutdownFinished;

		// Token: 0x04003029 RID: 12329
		private MoveSizeWinEventHandler _winEvent;

		// Token: 0x0400302A RID: 12330
		private Thread _thread;
	}
}
