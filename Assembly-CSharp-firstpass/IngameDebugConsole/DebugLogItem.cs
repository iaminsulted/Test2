using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IngameDebugConsole
{
	// Token: 0x020001ED RID: 493
	public class DebugLogItem : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000F6A RID: 3946 RVA: 0x0002DD10 File Offset: 0x0002BF10
		public RectTransform Transform
		{
			get
			{
				return this.transformComponent;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000F6B RID: 3947 RVA: 0x0002DD18 File Offset: 0x0002BF18
		public Image Image
		{
			get
			{
				return this.imageComponent;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000F6C RID: 3948 RVA: 0x0002DD20 File Offset: 0x0002BF20
		public int Index
		{
			get
			{
				return this.entryIndex;
			}
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x0002DD28 File Offset: 0x0002BF28
		public void Initialize(DebugLogRecycledListView manager)
		{
			this.manager = manager;
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x0002DD34 File Offset: 0x0002BF34
		public void SetContent(DebugLogEntry logEntry, int entryIndex, bool isExpanded)
		{
			this.logEntry = logEntry;
			this.entryIndex = entryIndex;
			Vector2 sizeDelta = this.transformComponent.sizeDelta;
			if (isExpanded)
			{
				this.logText.horizontalOverflow = HorizontalWrapMode.Wrap;
				sizeDelta.y = this.manager.SelectedItemHeight;
			}
			else
			{
				this.logText.horizontalOverflow = HorizontalWrapMode.Overflow;
				sizeDelta.y = this.manager.ItemHeight;
			}
			this.transformComponent.sizeDelta = sizeDelta;
			this.logText.text = (isExpanded ? logEntry.ToString() : logEntry.logString);
			this.logTypeImage.sprite = logEntry.logTypeSpriteRepresentation;
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x0002DDD5 File Offset: 0x0002BFD5
		public void ShowCount()
		{
			this.logCountText.text = this.logEntry.count.ToString();
			this.logCountParent.SetActive(true);
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x0002DDFE File Offset: 0x0002BFFE
		public void HideCount()
		{
			this.logCountParent.SetActive(false);
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x0002DE0C File Offset: 0x0002C00C
		public void OnPointerClick(PointerEventData eventData)
		{
			this.manager.OnLogItemClicked(this);
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x0002DE1C File Offset: 0x0002C01C
		public float CalculateExpandedHeight(string content)
		{
			string text = this.logText.text;
			HorizontalWrapMode horizontalOverflow = this.logText.horizontalOverflow;
			this.logText.text = content;
			this.logText.horizontalOverflow = HorizontalWrapMode.Wrap;
			float preferredHeight = this.logText.preferredHeight;
			this.logText.text = text;
			this.logText.horizontalOverflow = horizontalOverflow;
			return Mathf.Max(this.manager.ItemHeight, preferredHeight);
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x0002DE8E File Offset: 0x0002C08E
		public override string ToString()
		{
			return this.logEntry.ToString();
		}

		// Token: 0x04000ACA RID: 2762
		[SerializeField]
		private RectTransform transformComponent;

		// Token: 0x04000ACB RID: 2763
		[SerializeField]
		private Image imageComponent;

		// Token: 0x04000ACC RID: 2764
		[SerializeField]
		private Text logText;

		// Token: 0x04000ACD RID: 2765
		[SerializeField]
		private Image logTypeImage;

		// Token: 0x04000ACE RID: 2766
		[SerializeField]
		private GameObject logCountParent;

		// Token: 0x04000ACF RID: 2767
		[SerializeField]
		private Text logCountText;

		// Token: 0x04000AD0 RID: 2768
		private DebugLogEntry logEntry;

		// Token: 0x04000AD1 RID: 2769
		private int entryIndex;

		// Token: 0x04000AD2 RID: 2770
		private DebugLogRecycledListView manager;
	}
}
