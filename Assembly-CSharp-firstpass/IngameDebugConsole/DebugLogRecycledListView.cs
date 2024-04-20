using System;
using System.Collections.Generic;
using UnityEngine;

namespace IngameDebugConsole
{
	// Token: 0x020001F1 RID: 497
	public class DebugLogRecycledListView : MonoBehaviour
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000F9D RID: 3997 RVA: 0x0002F2DE File Offset: 0x0002D4DE
		public float ItemHeight
		{
			get
			{
				return this.logItemHeight;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000F9E RID: 3998 RVA: 0x0002F2E6 File Offset: 0x0002D4E6
		public float SelectedItemHeight
		{
			get
			{
				return this.heightOfSelectedLogEntry;
			}
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0002F2F0 File Offset: 0x0002D4F0
		private void Awake()
		{
			this.viewportHeight = this.viewportTransform.rect.height;
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x0002F316 File Offset: 0x0002D516
		public void Initialize(DebugLogManager manager, List<DebugLogEntry> collapsedLogEntries, DebugLogIndexList indicesOfEntriesToShow, float logItemHeight)
		{
			this.manager = manager;
			this.collapsedLogEntries = collapsedLogEntries;
			this.indicesOfEntriesToShow = indicesOfEntriesToShow;
			this.logItemHeight = logItemHeight;
			this._1OverLogItemHeight = 1f / logItemHeight;
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x0002F343 File Offset: 0x0002D543
		public void SetCollapseMode(bool collapse)
		{
			this.isCollapseOn = collapse;
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0002F34C File Offset: 0x0002D54C
		public void OnLogItemClicked(DebugLogItem item)
		{
			if (this.indexOfSelectedLogEntry != item.Index)
			{
				this.DeselectSelectedLogItem();
				this.indexOfSelectedLogEntry = item.Index;
				this.positionOfSelectedLogEntry = (float)item.Index * this.logItemHeight;
				this.heightOfSelectedLogEntry = item.CalculateExpandedHeight(item.ToString());
				this.deltaHeightOfSelectedLogEntry = this.heightOfSelectedLogEntry - this.logItemHeight;
				this.manager.SetSnapToBottom(false);
			}
			else
			{
				this.DeselectSelectedLogItem();
			}
			if (this.indexOfSelectedLogEntry >= this.currentTopIndex && this.indexOfSelectedLogEntry <= this.currentBottomIndex)
			{
				this.ColorLogItem(this.logItemsAtIndices[this.indexOfSelectedLogEntry], this.indexOfSelectedLogEntry);
			}
			this.CalculateContentHeight();
			this.HardResetItems();
			this.UpdateItemsInTheList(true);
			this.manager.ValidateScrollPosition();
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0002F420 File Offset: 0x0002D620
		public void DeselectSelectedLogItem()
		{
			int num = this.indexOfSelectedLogEntry;
			this.indexOfSelectedLogEntry = int.MaxValue;
			this.positionOfSelectedLogEntry = float.MaxValue;
			this.heightOfSelectedLogEntry = (this.deltaHeightOfSelectedLogEntry = 0f);
			if (num >= this.currentTopIndex && num <= this.currentBottomIndex)
			{
				this.ColorLogItem(this.logItemsAtIndices[num], num);
			}
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x0002F484 File Offset: 0x0002D684
		public void OnLogEntriesUpdated(bool updateAllVisibleItemContents)
		{
			this.CalculateContentHeight();
			this.viewportHeight = this.viewportTransform.rect.height;
			if (updateAllVisibleItemContents)
			{
				this.HardResetItems();
			}
			this.UpdateItemsInTheList(updateAllVisibleItemContents);
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0002F4C0 File Offset: 0x0002D6C0
		public void OnCollapsedLogEntryAtIndexUpdated(int index)
		{
			DebugLogItem debugLogItem;
			if (this.logItemsAtIndices.TryGetValue(index, out debugLogItem))
			{
				debugLogItem.ShowCount();
			}
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x0002F4E4 File Offset: 0x0002D6E4
		public void OnViewportDimensionsChanged()
		{
			this.viewportHeight = this.viewportTransform.rect.height;
			this.UpdateItemsInTheList(false);
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0002F511 File Offset: 0x0002D711
		private void HardResetItems()
		{
			if (this.currentTopIndex != -1)
			{
				this.DestroyLogItemsBetweenIndices(this.currentTopIndex, this.currentBottomIndex);
				this.currentTopIndex = -1;
			}
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0002F538 File Offset: 0x0002D738
		private void CalculateContentHeight()
		{
			float y = Mathf.Max(1f, (float)this.indicesOfEntriesToShow.Count * this.logItemHeight + this.deltaHeightOfSelectedLogEntry);
			this.transformComponent.sizeDelta = new Vector2(0f, y);
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0002F580 File Offset: 0x0002D780
		public void UpdateItemsInTheList(bool updateAllVisibleItemContents)
		{
			if (this.indicesOfEntriesToShow.Count > 0)
			{
				float num = this.transformComponent.anchoredPosition.y - 1f;
				float num2 = num + this.viewportHeight + 2f;
				if (this.positionOfSelectedLogEntry <= num2)
				{
					if (this.positionOfSelectedLogEntry <= num)
					{
						num -= this.deltaHeightOfSelectedLogEntry;
						num2 -= this.deltaHeightOfSelectedLogEntry;
						if (num < this.positionOfSelectedLogEntry - 1f)
						{
							num = this.positionOfSelectedLogEntry - 1f;
						}
						if (num2 < num + 2f)
						{
							num2 = num + 2f;
						}
					}
					else
					{
						num2 -= this.deltaHeightOfSelectedLogEntry;
						if (num2 < this.positionOfSelectedLogEntry + 1f)
						{
							num2 = this.positionOfSelectedLogEntry + 1f;
						}
					}
				}
				int num3 = (int)(num * this._1OverLogItemHeight);
				int num4 = (int)(num2 * this._1OverLogItemHeight);
				if (num3 < 0)
				{
					num3 = 0;
				}
				if (num4 > this.indicesOfEntriesToShow.Count - 1)
				{
					num4 = this.indicesOfEntriesToShow.Count - 1;
				}
				if (this.currentTopIndex == -1)
				{
					updateAllVisibleItemContents = true;
					this.currentTopIndex = num3;
					this.currentBottomIndex = num4;
					this.CreateLogItemsBetweenIndices(num3, num4);
				}
				else
				{
					if (num4 < this.currentTopIndex || num3 > this.currentBottomIndex)
					{
						updateAllVisibleItemContents = true;
						this.DestroyLogItemsBetweenIndices(this.currentTopIndex, this.currentBottomIndex);
						this.CreateLogItemsBetweenIndices(num3, num4);
					}
					else
					{
						if (num3 > this.currentTopIndex)
						{
							this.DestroyLogItemsBetweenIndices(this.currentTopIndex, num3 - 1);
						}
						if (num4 < this.currentBottomIndex)
						{
							this.DestroyLogItemsBetweenIndices(num4 + 1, this.currentBottomIndex);
						}
						if (num3 < this.currentTopIndex)
						{
							this.CreateLogItemsBetweenIndices(num3, this.currentTopIndex - 1);
							if (!updateAllVisibleItemContents)
							{
								this.UpdateLogItemContentsBetweenIndices(num3, this.currentTopIndex - 1);
							}
						}
						if (num4 > this.currentBottomIndex)
						{
							this.CreateLogItemsBetweenIndices(this.currentBottomIndex + 1, num4);
							if (!updateAllVisibleItemContents)
							{
								this.UpdateLogItemContentsBetweenIndices(this.currentBottomIndex + 1, num4);
							}
						}
					}
					this.currentTopIndex = num3;
					this.currentBottomIndex = num4;
				}
				if (updateAllVisibleItemContents)
				{
					this.UpdateLogItemContentsBetweenIndices(this.currentTopIndex, this.currentBottomIndex);
					return;
				}
			}
			else
			{
				this.HardResetItems();
			}
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x0002F780 File Offset: 0x0002D980
		private void CreateLogItemsBetweenIndices(int topIndex, int bottomIndex)
		{
			for (int i = topIndex; i <= bottomIndex; i++)
			{
				this.CreateLogItemAtIndex(i);
			}
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x0002F7A0 File Offset: 0x0002D9A0
		private void CreateLogItemAtIndex(int index)
		{
			DebugLogItem debugLogItem = this.debugManager.PopLogItem();
			Vector2 anchoredPosition = new Vector2(1f, (float)(-(float)index) * this.logItemHeight);
			if (index > this.indexOfSelectedLogEntry)
			{
				anchoredPosition.y -= this.deltaHeightOfSelectedLogEntry;
			}
			debugLogItem.Transform.anchoredPosition = anchoredPosition;
			this.ColorLogItem(debugLogItem, index);
			this.logItemsAtIndices[index] = debugLogItem;
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x0002F80C File Offset: 0x0002DA0C
		private void DestroyLogItemsBetweenIndices(int topIndex, int bottomIndex)
		{
			for (int i = topIndex; i <= bottomIndex; i++)
			{
				this.debugManager.PoolLogItem(this.logItemsAtIndices[i]);
			}
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x0002F83C File Offset: 0x0002DA3C
		private void UpdateLogItemContentsBetweenIndices(int topIndex, int bottomIndex)
		{
			for (int i = topIndex; i <= bottomIndex; i++)
			{
				DebugLogItem debugLogItem = this.logItemsAtIndices[i];
				debugLogItem.SetContent(this.collapsedLogEntries[this.indicesOfEntriesToShow[i]], i, i == this.indexOfSelectedLogEntry);
				if (this.isCollapseOn)
				{
					debugLogItem.ShowCount();
				}
				else
				{
					debugLogItem.HideCount();
				}
			}
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x0002F8A0 File Offset: 0x0002DAA0
		private void ColorLogItem(DebugLogItem logItem, int index)
		{
			if (index == this.indexOfSelectedLogEntry)
			{
				logItem.Image.color = this.logItemSelectedColor;
				return;
			}
			if (index % 2 == 0)
			{
				logItem.Image.color = this.logItemNormalColor1;
				return;
			}
			logItem.Image.color = this.logItemNormalColor2;
		}

		// Token: 0x04000B1F RID: 2847
		[SerializeField]
		private RectTransform transformComponent;

		// Token: 0x04000B20 RID: 2848
		[SerializeField]
		private RectTransform viewportTransform;

		// Token: 0x04000B21 RID: 2849
		[SerializeField]
		private DebugLogManager debugManager;

		// Token: 0x04000B22 RID: 2850
		[SerializeField]
		private Color logItemNormalColor1;

		// Token: 0x04000B23 RID: 2851
		[SerializeField]
		private Color logItemNormalColor2;

		// Token: 0x04000B24 RID: 2852
		[SerializeField]
		private Color logItemSelectedColor;

		// Token: 0x04000B25 RID: 2853
		private DebugLogManager manager;

		// Token: 0x04000B26 RID: 2854
		private float logItemHeight;

		// Token: 0x04000B27 RID: 2855
		private float _1OverLogItemHeight;

		// Token: 0x04000B28 RID: 2856
		private float viewportHeight;

		// Token: 0x04000B29 RID: 2857
		private List<DebugLogEntry> collapsedLogEntries;

		// Token: 0x04000B2A RID: 2858
		private DebugLogIndexList indicesOfEntriesToShow;

		// Token: 0x04000B2B RID: 2859
		private int indexOfSelectedLogEntry = int.MaxValue;

		// Token: 0x04000B2C RID: 2860
		private float positionOfSelectedLogEntry = float.MaxValue;

		// Token: 0x04000B2D RID: 2861
		private float heightOfSelectedLogEntry;

		// Token: 0x04000B2E RID: 2862
		private float deltaHeightOfSelectedLogEntry;

		// Token: 0x04000B2F RID: 2863
		private Dictionary<int, DebugLogItem> logItemsAtIndices = new Dictionary<int, DebugLogItem>();

		// Token: 0x04000B30 RID: 2864
		private bool isCollapseOn;

		// Token: 0x04000B31 RID: 2865
		private int currentTopIndex = -1;

		// Token: 0x04000B32 RID: 2866
		private int currentBottomIndex = -1;
	}
}
