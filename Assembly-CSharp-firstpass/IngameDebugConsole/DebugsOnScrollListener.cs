using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IngameDebugConsole
{
	// Token: 0x020001F2 RID: 498
	public class DebugsOnScrollListener : MonoBehaviour, IScrollHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler
	{
		// Token: 0x06000FB0 RID: 4016 RVA: 0x0002F927 File Offset: 0x0002DB27
		public void OnScroll(PointerEventData data)
		{
			if (this.IsScrollbarAtBottom())
			{
				this.debugLogManager.SetSnapToBottom(true);
				return;
			}
			this.debugLogManager.SetSnapToBottom(false);
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x0002F94A File Offset: 0x0002DB4A
		public void OnBeginDrag(PointerEventData data)
		{
			this.debugLogManager.SetSnapToBottom(false);
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0002F958 File Offset: 0x0002DB58
		public void OnEndDrag(PointerEventData data)
		{
			if (this.IsScrollbarAtBottom())
			{
				this.debugLogManager.SetSnapToBottom(true);
				return;
			}
			this.debugLogManager.SetSnapToBottom(false);
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0002F97B File Offset: 0x0002DB7B
		public void OnScrollbarDragStart(BaseEventData data)
		{
			this.debugLogManager.SetSnapToBottom(false);
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0002F989 File Offset: 0x0002DB89
		public void OnScrollbarDragEnd(BaseEventData data)
		{
			if (this.IsScrollbarAtBottom())
			{
				this.debugLogManager.SetSnapToBottom(true);
				return;
			}
			this.debugLogManager.SetSnapToBottom(false);
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0002F9AC File Offset: 0x0002DBAC
		private bool IsScrollbarAtBottom()
		{
			return this.debugsScrollRect.verticalNormalizedPosition <= 1E-06f;
		}

		// Token: 0x04000B33 RID: 2867
		public ScrollRect debugsScrollRect;

		// Token: 0x04000B34 RID: 2868
		public DebugLogManager debugLogManager;
	}
}
