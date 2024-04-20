using System;
using System.IO;
using System.Threading;
using System.Windows.Threading;

namespace System.Windows.Documents
{
	// Token: 0x0200064C RID: 1612
	internal sealed class PageContentAsyncResult : IAsyncResult
	{
		// Token: 0x06004FE4 RID: 20452 RVA: 0x00244DC0 File Offset: 0x00243DC0
		internal PageContentAsyncResult(AsyncCallback callback, object state, Dispatcher dispatcher, Uri baseUri, Uri source, FixedPage child)
		{
			this._dispatcher = dispatcher;
			this._isCompleted = false;
			this._completedSynchronously = false;
			this._callback = callback;
			this._asyncState = state;
			this._getpageStatus = PageContentAsyncResult.GetPageStatus.Loading;
			this._child = child;
			this._baseUri = baseUri;
			this._source = source;
		}

		// Token: 0x17001287 RID: 4743
		// (get) Token: 0x06004FE5 RID: 20453 RVA: 0x00244E15 File Offset: 0x00243E15
		public object AsyncState
		{
			get
			{
				return this._asyncState;
			}
		}

		// Token: 0x17001288 RID: 4744
		// (get) Token: 0x06004FE6 RID: 20454 RVA: 0x00109403 File Offset: 0x00108403
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001289 RID: 4745
		// (get) Token: 0x06004FE7 RID: 20455 RVA: 0x00244E1D File Offset: 0x00243E1D
		public bool CompletedSynchronously
		{
			get
			{
				return this._completedSynchronously;
			}
		}

		// Token: 0x1700128A RID: 4746
		// (get) Token: 0x06004FE8 RID: 20456 RVA: 0x00244E25 File Offset: 0x00243E25
		public bool IsCompleted
		{
			get
			{
				return this._isCompleted;
			}
		}

		// Token: 0x06004FE9 RID: 20457 RVA: 0x00244E30 File Offset: 0x00243E30
		internal object Dispatch(object arg)
		{
			if (this._exception != null)
			{
				this._getpageStatus = PageContentAsyncResult.GetPageStatus.Finished;
			}
			switch (this._getpageStatus)
			{
			case PageContentAsyncResult.GetPageStatus.Loading:
				try
				{
					if (this._child != null)
					{
						this._completedSynchronously = true;
						this._result = this._child;
						this._getpageStatus = PageContentAsyncResult.GetPageStatus.Finished;
					}
					else
					{
						Stream stream;
						PageContent._LoadPageImpl(this._baseUri, this._source, out this._result, out stream);
						if (this._result == null || this._result.IsInitialized)
						{
							stream.Close();
						}
						else
						{
							this._pendingStream = stream;
							this._result.Initialized += this._OnPaserFinished;
						}
						this._getpageStatus = PageContentAsyncResult.GetPageStatus.Finished;
					}
				}
				catch (ApplicationException exception)
				{
					this._exception = exception;
				}
				break;
			case PageContentAsyncResult.GetPageStatus.Cancelled:
			case PageContentAsyncResult.GetPageStatus.Finished:
				break;
			default:
				goto IL_D4;
			}
			this._isCompleted = true;
			if (this._callback != null)
			{
				this._callback(this);
			}
			IL_D4:
			return null;
		}

		// Token: 0x06004FEA RID: 20458 RVA: 0x00244F24 File Offset: 0x00243F24
		internal void Cancel()
		{
			this._getpageStatus = PageContentAsyncResult.GetPageStatus.Cancelled;
		}

		// Token: 0x06004FEB RID: 20459 RVA: 0x00244F2D File Offset: 0x00243F2D
		internal void Wait()
		{
			this._dispatcherOperation.Wait();
		}

		// Token: 0x1700128B RID: 4747
		// (get) Token: 0x06004FEC RID: 20460 RVA: 0x00244F3B File Offset: 0x00243F3B
		internal Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x1700128C RID: 4748
		// (get) Token: 0x06004FED RID: 20461 RVA: 0x00244F43 File Offset: 0x00243F43
		internal bool IsCancelled
		{
			get
			{
				return this._getpageStatus == PageContentAsyncResult.GetPageStatus.Cancelled;
			}
		}

		// Token: 0x1700128D RID: 4749
		// (set) Token: 0x06004FEE RID: 20462 RVA: 0x00244F4E File Offset: 0x00243F4E
		internal DispatcherOperation DispatcherOperation
		{
			set
			{
				this._dispatcherOperation = value;
			}
		}

		// Token: 0x1700128E RID: 4750
		// (get) Token: 0x06004FEF RID: 20463 RVA: 0x00244F57 File Offset: 0x00243F57
		internal FixedPage Result
		{
			get
			{
				return this._result;
			}
		}

		// Token: 0x06004FF0 RID: 20464 RVA: 0x00244F5F File Offset: 0x00243F5F
		private void _OnPaserFinished(object sender, EventArgs args)
		{
			if (this._pendingStream != null)
			{
				this._pendingStream.Close();
				this._pendingStream = null;
			}
		}

		// Token: 0x0400286A RID: 10346
		private object _asyncState;

		// Token: 0x0400286B RID: 10347
		private bool _isCompleted;

		// Token: 0x0400286C RID: 10348
		private bool _completedSynchronously;

		// Token: 0x0400286D RID: 10349
		private AsyncCallback _callback;

		// Token: 0x0400286E RID: 10350
		private Exception _exception;

		// Token: 0x0400286F RID: 10351
		private PageContentAsyncResult.GetPageStatus _getpageStatus;

		// Token: 0x04002870 RID: 10352
		private Uri _baseUri;

		// Token: 0x04002871 RID: 10353
		private Uri _source;

		// Token: 0x04002872 RID: 10354
		private FixedPage _child;

		// Token: 0x04002873 RID: 10355
		private Dispatcher _dispatcher;

		// Token: 0x04002874 RID: 10356
		private FixedPage _result;

		// Token: 0x04002875 RID: 10357
		private Stream _pendingStream;

		// Token: 0x04002876 RID: 10358
		private DispatcherOperation _dispatcherOperation;

		// Token: 0x02000B4A RID: 2890
		internal enum GetPageStatus
		{
			// Token: 0x04004879 RID: 18553
			Loading,
			// Token: 0x0400487A RID: 18554
			Cancelled,
			// Token: 0x0400487B RID: 18555
			Finished
		}
	}
}
