using System;
using System.Windows.Controls;
using MS.Internal.AppModel;

namespace System.Windows.Navigation
{
	// Token: 0x020005CC RID: 1484
	public abstract class PageFunctionBase : Page
	{
		// Token: 0x060047CE RID: 18382 RVA: 0x0022A99A File Offset: 0x0022999A
		protected PageFunctionBase()
		{
			this.PageFunctionId = Guid.NewGuid();
			this.ParentPageFunctionId = Guid.Empty;
		}

		// Token: 0x17001015 RID: 4117
		// (get) Token: 0x060047CF RID: 18383 RVA: 0x0022A9BF File Offset: 0x002299BF
		// (set) Token: 0x060047D0 RID: 18384 RVA: 0x0022A9C7 File Offset: 0x002299C7
		public bool RemoveFromJournal
		{
			get
			{
				return this._fRemoveFromJournal;
			}
			set
			{
				this._fRemoveFromJournal = value;
			}
		}

		// Token: 0x060047D1 RID: 18385 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void Start()
		{
		}

		// Token: 0x060047D2 RID: 18386 RVA: 0x0022A9D0 File Offset: 0x002299D0
		internal void CallStart()
		{
			this.Start();
		}

		// Token: 0x060047D3 RID: 18387 RVA: 0x0022A9D8 File Offset: 0x002299D8
		internal void _OnReturnUnTyped(object o)
		{
			if (this._finish != null)
			{
				this._finish(this, o);
			}
		}

		// Token: 0x060047D4 RID: 18388 RVA: 0x0022A9F0 File Offset: 0x002299F0
		internal void _AddEventHandler(Delegate d)
		{
			PageFunctionBase pageFunctionBase = d.Target as PageFunctionBase;
			if (pageFunctionBase != null)
			{
				this.ParentPageFunctionId = pageFunctionBase.PageFunctionId;
			}
			this._returnHandler = Delegate.Combine(this._returnHandler, d);
		}

		// Token: 0x060047D5 RID: 18389 RVA: 0x0022AA2A File Offset: 0x00229A2A
		internal void _RemoveEventHandler(Delegate d)
		{
			this._returnHandler = Delegate.Remove(this._returnHandler, d);
		}

		// Token: 0x060047D6 RID: 18390 RVA: 0x0022AA3E File Offset: 0x00229A3E
		internal void _DetachEvents()
		{
			this._returnHandler = null;
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x0022AA48 File Offset: 0x00229A48
		internal void _OnFinish(object returnEventArgs)
		{
			RaiseTypedEventArgs args = new RaiseTypedEventArgs(this._returnHandler, returnEventArgs);
			this.RaiseTypedEvent(this, args);
		}

		// Token: 0x17001016 RID: 4118
		// (get) Token: 0x060047D8 RID: 18392 RVA: 0x0022AA6F File Offset: 0x00229A6F
		// (set) Token: 0x060047D9 RID: 18393 RVA: 0x0022AA77 File Offset: 0x00229A77
		internal Guid PageFunctionId
		{
			get
			{
				return this._pageFunctionId;
			}
			set
			{
				this._pageFunctionId = value;
			}
		}

		// Token: 0x17001017 RID: 4119
		// (get) Token: 0x060047DA RID: 18394 RVA: 0x0022AA80 File Offset: 0x00229A80
		// (set) Token: 0x060047DB RID: 18395 RVA: 0x0022AA88 File Offset: 0x00229A88
		internal Guid ParentPageFunctionId
		{
			get
			{
				return this._parentPageFunctionId;
			}
			set
			{
				this._parentPageFunctionId = value;
			}
		}

		// Token: 0x17001018 RID: 4120
		// (get) Token: 0x060047DC RID: 18396 RVA: 0x0022AA91 File Offset: 0x00229A91
		internal Delegate _Return
		{
			get
			{
				return this._returnHandler;
			}
		}

		// Token: 0x17001019 RID: 4121
		// (get) Token: 0x060047DD RID: 18397 RVA: 0x0022AA99 File Offset: 0x00229A99
		// (set) Token: 0x060047DE RID: 18398 RVA: 0x0022AAA1 File Offset: 0x00229AA1
		internal bool _Resume
		{
			get
			{
				return this._resume;
			}
			set
			{
				this._resume = value;
			}
		}

		// Token: 0x1700101A RID: 4122
		// (get) Token: 0x060047DF RID: 18399 RVA: 0x0022AAAA File Offset: 0x00229AAA
		// (set) Token: 0x060047E0 RID: 18400 RVA: 0x0022AAB2 File Offset: 0x00229AB2
		internal ReturnEventSaver _Saver
		{
			get
			{
				return this._saverInfo;
			}
			set
			{
				this._saverInfo = value;
			}
		}

		// Token: 0x1700101B RID: 4123
		// (get) Token: 0x060047E1 RID: 18401 RVA: 0x0022AABB File Offset: 0x00229ABB
		// (set) Token: 0x060047E2 RID: 18402 RVA: 0x0022AAC3 File Offset: 0x00229AC3
		internal FinishEventHandler FinishHandler
		{
			get
			{
				return this._finish;
			}
			set
			{
				this._finish = value;
			}
		}

		// Token: 0x140000AB RID: 171
		// (add) Token: 0x060047E3 RID: 18403 RVA: 0x0022AACC File Offset: 0x00229ACC
		// (remove) Token: 0x060047E4 RID: 18404 RVA: 0x0022AB04 File Offset: 0x00229B04
		internal event EventToRaiseTypedEvent RaiseTypedEvent;

		// Token: 0x040025E6 RID: 9702
		private Guid _pageFunctionId;

		// Token: 0x040025E7 RID: 9703
		private Guid _parentPageFunctionId;

		// Token: 0x040025E8 RID: 9704
		private bool _fRemoveFromJournal = true;

		// Token: 0x040025E9 RID: 9705
		private bool _resume;

		// Token: 0x040025EA RID: 9706
		private ReturnEventSaver _saverInfo;

		// Token: 0x040025EB RID: 9707
		private FinishEventHandler _finish;

		// Token: 0x040025EC RID: 9708
		private Delegate _returnHandler;
	}
}
