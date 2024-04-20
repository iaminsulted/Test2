using System;

namespace MS.Internal.Data
{
	// Token: 0x02000201 RID: 513
	internal class AsyncDataRequest
	{
		// Token: 0x060012C5 RID: 4805 RVA: 0x0014BB3E File Offset: 0x0014AB3E
		internal AsyncDataRequest(object bindingState, AsyncRequestCallback workCallback, AsyncRequestCallback completedCallback, params object[] args)
		{
			this._bindingState = bindingState;
			this._workCallback = workCallback;
			this._completedCallback = completedCallback;
			this._args = args;
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x060012C6 RID: 4806 RVA: 0x0014BB6E File Offset: 0x0014AB6E
		public object Result
		{
			get
			{
				return this._result;
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x060012C7 RID: 4807 RVA: 0x0014BB76 File Offset: 0x0014AB76
		public AsyncRequestStatus Status
		{
			get
			{
				return this._status;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x060012C8 RID: 4808 RVA: 0x0014BB7E File Offset: 0x0014AB7E
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x0014BB86 File Offset: 0x0014AB86
		public object DoWork()
		{
			if (this.DoBeginWork() && this._workCallback != null)
			{
				return this._workCallback(this);
			}
			return null;
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x0014BBA6 File Offset: 0x0014ABA6
		public bool DoBeginWork()
		{
			return this.ChangeStatus(AsyncRequestStatus.Working);
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x0014BBAF File Offset: 0x0014ABAF
		public void Complete(object result)
		{
			if (this.ChangeStatus(AsyncRequestStatus.Completed))
			{
				this._result = result;
				if (this._completedCallback != null)
				{
					this._completedCallback(this);
				}
			}
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x0014BBD6 File Offset: 0x0014ABD6
		public void Cancel()
		{
			this.ChangeStatus(AsyncRequestStatus.Cancelled);
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x0014BBE0 File Offset: 0x0014ABE0
		public void Fail(Exception exception)
		{
			if (this.ChangeStatus(AsyncRequestStatus.Failed))
			{
				this._exception = exception;
				if (this._completedCallback != null)
				{
					this._completedCallback(this);
				}
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x060012CE RID: 4814 RVA: 0x0014BC07 File Offset: 0x0014AC07
		internal object[] Args
		{
			get
			{
				return this._args;
			}
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x0014BC10 File Offset: 0x0014AC10
		private bool ChangeStatus(AsyncRequestStatus newStatus)
		{
			bool flag = false;
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				switch (newStatus)
				{
				case AsyncRequestStatus.Working:
					flag = (this._status == AsyncRequestStatus.Waiting);
					break;
				case AsyncRequestStatus.Completed:
					flag = (this._status == AsyncRequestStatus.Working);
					break;
				case AsyncRequestStatus.Cancelled:
					flag = (this._status == AsyncRequestStatus.Waiting || this._status == AsyncRequestStatus.Working);
					break;
				case AsyncRequestStatus.Failed:
					flag = (this._status == AsyncRequestStatus.Working);
					break;
				}
				if (flag)
				{
					this._status = newStatus;
				}
			}
			return flag;
		}

		// Token: 0x04000B5A RID: 2906
		private AsyncRequestStatus _status;

		// Token: 0x04000B5B RID: 2907
		private object _result;

		// Token: 0x04000B5C RID: 2908
		private object _bindingState;

		// Token: 0x04000B5D RID: 2909
		private object[] _args;

		// Token: 0x04000B5E RID: 2910
		private Exception _exception;

		// Token: 0x04000B5F RID: 2911
		private AsyncRequestCallback _workCallback;

		// Token: 0x04000B60 RID: 2912
		private AsyncRequestCallback _completedCallback;

		// Token: 0x04000B61 RID: 2913
		private object SyncRoot = new object();
	}
}
