using System;

namespace System.Windows
{
	// Token: 0x020003DD RID: 989
	public sealed class VisualStateChangedEventArgs : EventArgs
	{
		// Token: 0x06002991 RID: 10641 RVA: 0x00199E90 File Offset: 0x00198E90
		internal VisualStateChangedEventArgs(VisualState oldState, VisualState newState, FrameworkElement control, FrameworkElement stateGroupsRoot)
		{
			this._oldState = oldState;
			this._newState = newState;
			this._control = control;
			this._stateGroupsRoot = stateGroupsRoot;
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06002992 RID: 10642 RVA: 0x00199EB5 File Offset: 0x00198EB5
		public VisualState OldState
		{
			get
			{
				return this._oldState;
			}
		}

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x06002993 RID: 10643 RVA: 0x00199EBD File Offset: 0x00198EBD
		public VisualState NewState
		{
			get
			{
				return this._newState;
			}
		}

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x06002994 RID: 10644 RVA: 0x00199EC5 File Offset: 0x00198EC5
		public FrameworkElement Control
		{
			get
			{
				return this._control;
			}
		}

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x06002995 RID: 10645 RVA: 0x00199ECD File Offset: 0x00198ECD
		public FrameworkElement StateGroupsRoot
		{
			get
			{
				return this._stateGroupsRoot;
			}
		}

		// Token: 0x040014F9 RID: 5369
		private VisualState _oldState;

		// Token: 0x040014FA RID: 5370
		private VisualState _newState;

		// Token: 0x040014FB RID: 5371
		private FrameworkElement _control;

		// Token: 0x040014FC RID: 5372
		private FrameworkElement _stateGroupsRoot;
	}
}
