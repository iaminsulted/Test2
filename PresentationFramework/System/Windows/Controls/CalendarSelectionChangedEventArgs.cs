using System;
using System.Collections;

namespace System.Windows.Controls
{
	// Token: 0x02000729 RID: 1833
	internal class CalendarSelectionChangedEventArgs : SelectionChangedEventArgs
	{
		// Token: 0x0600608D RID: 24717 RVA: 0x00299DE6 File Offset: 0x00298DE6
		public CalendarSelectionChangedEventArgs(RoutedEvent eventId, IList removedItems, IList addedItems) : base(eventId, removedItems, addedItems)
		{
		}

		// Token: 0x0600608E RID: 24718 RVA: 0x00299DF4 File Offset: 0x00298DF4
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			EventHandler<SelectionChangedEventArgs> eventHandler = genericHandler as EventHandler<SelectionChangedEventArgs>;
			if (eventHandler != null)
			{
				eventHandler(genericTarget, this);
				return;
			}
			base.InvokeEventHandler(genericHandler, genericTarget);
		}
	}
}
