using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace System.Windows.Controls
{
	// Token: 0x020007E9 RID: 2025
	public class TextChangedEventArgs : RoutedEventArgs
	{
		// Token: 0x0600753D RID: 30013 RVA: 0x002EB030 File Offset: 0x002EA030
		public TextChangedEventArgs(RoutedEvent id, UndoAction action, ICollection<TextChange> changes)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			if (action < UndoAction.None || action > UndoAction.Create)
			{
				throw new InvalidEnumArgumentException("action", (int)action, typeof(UndoAction));
			}
			base.RoutedEvent = id;
			this._undoAction = action;
			this._changes = changes;
		}

		// Token: 0x0600753E RID: 30014 RVA: 0x002EB084 File Offset: 0x002EA084
		public TextChangedEventArgs(RoutedEvent id, UndoAction action) : this(id, action, new ReadOnlyCollection<TextChange>(new List<TextChange>()))
		{
		}

		// Token: 0x17001B3C RID: 6972
		// (get) Token: 0x0600753F RID: 30015 RVA: 0x002EB098 File Offset: 0x002EA098
		public UndoAction UndoAction
		{
			get
			{
				return this._undoAction;
			}
		}

		// Token: 0x17001B3D RID: 6973
		// (get) Token: 0x06007540 RID: 30016 RVA: 0x002EB0A0 File Offset: 0x002EA0A0
		public ICollection<TextChange> Changes
		{
			get
			{
				return this._changes;
			}
		}

		// Token: 0x06007541 RID: 30017 RVA: 0x002EB0A8 File Offset: 0x002EA0A8
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			((TextChangedEventHandler)genericHandler)(genericTarget, this);
		}

		// Token: 0x0400383B RID: 14395
		private UndoAction _undoAction;

		// Token: 0x0400383C RID: 14396
		private readonly ICollection<TextChange> _changes;
	}
}
