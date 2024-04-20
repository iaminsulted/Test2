using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IngameDebugConsole
{
	// Token: 0x020001F0 RID: 496
	public class DebugLogPopup : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		// Token: 0x06000F8E RID: 3982 RVA: 0x0002EE86 File Offset: 0x0002D086
		private void Awake()
		{
			this.popupTransform = (RectTransform)base.transform;
			this.backgroundImage = base.GetComponent<Image>();
			this.canvasGroup = base.GetComponent<CanvasGroup>();
			this.normalColor = this.backgroundImage.color;
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x0002EEC2 File Offset: 0x0002D0C2
		private void Start()
		{
			this.halfSize = this.popupTransform.sizeDelta * 0.5f * this.popupTransform.root.localScale.x;
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x0002EEFC File Offset: 0x0002D0FC
		public void OnViewportDimensionsChanged()
		{
			if (!base.gameObject.activeSelf)
			{
				return;
			}
			this.halfSize = this.popupTransform.sizeDelta * 0.5f * this.popupTransform.root.localScale.x;
			this.OnEndDrag(null);
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x0002EF54 File Offset: 0x0002D154
		public void NewInfoLogArrived()
		{
			this.newInfoCount++;
			this.newInfoCountText.text = this.newInfoCount.ToString();
			if (this.newWarningCount == 0 && this.newErrorCount == 0)
			{
				this.backgroundImage.color = this.alertColorInfo;
			}
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0002EFA6 File Offset: 0x0002D1A6
		public void NewWarningLogArrived()
		{
			this.newWarningCount++;
			this.newWarningCountText.text = this.newWarningCount.ToString();
			if (this.newErrorCount == 0)
			{
				this.backgroundImage.color = this.alertColorWarning;
			}
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0002EFE5 File Offset: 0x0002D1E5
		public void NewErrorLogArrived()
		{
			this.newErrorCount++;
			this.newErrorCountText.text = this.newErrorCount.ToString();
			this.backgroundImage.color = this.alertColorError;
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x0002F01C File Offset: 0x0002D21C
		private void Reset()
		{
			this.newInfoCount = 0;
			this.newWarningCount = 0;
			this.newErrorCount = 0;
			this.newInfoCountText.text = "0";
			this.newWarningCountText.text = "0";
			this.newErrorCountText.text = "0";
			this.backgroundImage.color = this.normalColor;
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x0002F07F File Offset: 0x0002D27F
		private IEnumerator MoveToPosAnimation(Vector3 targetPos)
		{
			float modifier = 0f;
			Vector3 initialPos = this.popupTransform.position;
			while (modifier < 1f)
			{
				modifier += 4f * Time.unscaledDeltaTime;
				this.popupTransform.position = Vector3.Lerp(initialPos, targetPos, modifier);
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0002F095 File Offset: 0x0002D295
		public void OnPointerClick(PointerEventData data)
		{
			if (!this.isPopupBeingDragged)
			{
				this.debugManager.ShowLogWindow();
			}
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0002F0AA File Offset: 0x0002D2AA
		public void Show()
		{
			this.canvasGroup.interactable = true;
			this.canvasGroup.blocksRaycasts = true;
			this.canvasGroup.alpha = 1f;
			this.Reset();
			this.OnViewportDimensionsChanged();
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0002F0E0 File Offset: 0x0002D2E0
		public void Hide()
		{
			this.canvasGroup.interactable = false;
			this.canvasGroup.blocksRaycasts = false;
			this.canvasGroup.alpha = 0f;
			this.isPopupBeingDragged = false;
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x0002F111 File Offset: 0x0002D311
		public void OnBeginDrag(PointerEventData data)
		{
			this.isPopupBeingDragged = true;
			if (this.moveToPosCoroutine != null)
			{
				base.StopCoroutine(this.moveToPosCoroutine);
				this.moveToPosCoroutine = null;
			}
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0002F135 File Offset: 0x0002D335
		public void OnDrag(PointerEventData data)
		{
			this.popupTransform.position = data.position;
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0002F150 File Offset: 0x0002D350
		public void OnEndDrag(PointerEventData data)
		{
			int width = Screen.width;
			int height = Screen.height;
			Vector3 position = this.popupTransform.position;
			float x = position.x;
			float num = Mathf.Abs(position.x - (float)width);
			float num2 = Mathf.Abs(position.y);
			float num3 = Mathf.Abs(position.y - (float)height);
			float num4 = Mathf.Min(x, num);
			float num5 = Mathf.Min(num2, num3);
			if (num4 < num5)
			{
				if (x < num)
				{
					position = new Vector3(this.halfSize.x, position.y, 0f);
				}
				else
				{
					position = new Vector3((float)width - this.halfSize.x, position.y, 0f);
				}
				position.y = Mathf.Clamp(position.y, this.halfSize.y, (float)height - this.halfSize.y);
			}
			else
			{
				if (num2 < num3)
				{
					position = new Vector3(position.x, this.halfSize.y, 0f);
				}
				else
				{
					position = new Vector3(position.x, (float)height - this.halfSize.y, 0f);
				}
				position.x = Mathf.Clamp(position.x, this.halfSize.x, (float)width - this.halfSize.x);
			}
			if (this.moveToPosCoroutine != null)
			{
				base.StopCoroutine(this.moveToPosCoroutine);
			}
			this.moveToPosCoroutine = this.MoveToPosAnimation(position);
			base.StartCoroutine(this.moveToPosCoroutine);
			this.isPopupBeingDragged = false;
		}

		// Token: 0x04000B0E RID: 2830
		private RectTransform popupTransform;

		// Token: 0x04000B0F RID: 2831
		private Vector2 halfSize;

		// Token: 0x04000B10 RID: 2832
		private Image backgroundImage;

		// Token: 0x04000B11 RID: 2833
		private CanvasGroup canvasGroup;

		// Token: 0x04000B12 RID: 2834
		[SerializeField]
		private DebugLogManager debugManager;

		// Token: 0x04000B13 RID: 2835
		[SerializeField]
		private Text newInfoCountText;

		// Token: 0x04000B14 RID: 2836
		[SerializeField]
		private Text newWarningCountText;

		// Token: 0x04000B15 RID: 2837
		[SerializeField]
		private Text newErrorCountText;

		// Token: 0x04000B16 RID: 2838
		[SerializeField]
		private Color alertColorInfo;

		// Token: 0x04000B17 RID: 2839
		[SerializeField]
		private Color alertColorWarning;

		// Token: 0x04000B18 RID: 2840
		[SerializeField]
		private Color alertColorError;

		// Token: 0x04000B19 RID: 2841
		private int newInfoCount;

		// Token: 0x04000B1A RID: 2842
		private int newWarningCount;

		// Token: 0x04000B1B RID: 2843
		private int newErrorCount;

		// Token: 0x04000B1C RID: 2844
		private Color normalColor;

		// Token: 0x04000B1D RID: 2845
		private bool isPopupBeingDragged;

		// Token: 0x04000B1E RID: 2846
		private IEnumerator moveToPosCoroutine;
	}
}
