using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace CodeStage.AdvancedFPSCounter.Labels
{
	// Token: 0x020001F9 RID: 505
	internal class DrawableLabel
	{
		// Token: 0x0600100A RID: 4106 RVA: 0x00033D58 File Offset: 0x00031F58
		internal DrawableLabel(GameObject container, LabelAnchor anchor, LabelEffect background, LabelEffect shadow, LabelEffect outline, Font font, int fontSize, float lineSpacing, Vector2 pixelOffset)
		{
			this.container = container;
			this.anchor = anchor;
			this.background = background;
			this.shadow = shadow;
			this.outline = outline;
			this.font = font;
			this.fontSize = fontSize;
			this.lineSpacing = lineSpacing;
			this.pixelOffset = pixelOffset;
			this.NormalizeOffset();
			this.newText = new StringBuilder(1000);
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x00033DC8 File Offset: 0x00031FC8
		internal void CheckAndUpdate()
		{
			if (this.newText.Length > 0)
			{
				if (this.uiText == null)
				{
					this.labelGameObject = new GameObject(this.anchor.ToString(), new Type[]
					{
						typeof(RectTransform)
					});
					this.labelTransform = this.labelGameObject.GetComponent<RectTransform>();
					this.labelFitter = this.labelGameObject.AddComponent<ContentSizeFitter>();
					this.labelFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
					this.labelFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
					this.labelGroup = this.labelGameObject.AddComponent<HorizontalLayoutGroup>();
					this.labelGameObject.layer = this.container.layer;
					this.labelGameObject.tag = this.container.tag;
					this.labelGameObject.transform.SetParent(this.container.transform, false);
					this.uiTextGameObject = new GameObject("Text", new Type[]
					{
						typeof(Text)
					});
					this.uiTextGameObject.transform.SetParent(this.labelGameObject.transform, false);
					this.uiText = this.uiTextGameObject.GetComponent<Text>();
					this.uiText.horizontalOverflow = HorizontalWrapMode.Overflow;
					this.uiText.verticalOverflow = VerticalWrapMode.Overflow;
					this.ApplyShadow();
					this.ApplyOutline();
					this.ApplyFont();
					this.uiText.fontSize = this.fontSize;
					this.uiText.lineSpacing = this.lineSpacing;
					this.UpdateTextPosition();
					this.ApplyBackground();
				}
				if (this.dirty)
				{
					this.uiText.text = this.newText.ToString();
					this.ApplyBackground();
					this.dirty = false;
				}
				this.newText.Length = 0;
				return;
			}
			if (this.uiText != null)
			{
				this.Clear();
			}
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x00033FB0 File Offset: 0x000321B0
		internal void Clear()
		{
			this.newText.Length = 0;
			if (this.labelGameObject != null)
			{
				UnityEngine.Object.Destroy(this.labelGameObject);
				this.labelGameObject = null;
				this.labelTransform = null;
				this.uiText = null;
			}
			if (this.backgroundImage != null)
			{
				UnityEngine.Object.Destroy(this.backgroundImage);
				this.backgroundImage = null;
			}
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x00034017 File Offset: 0x00032217
		internal void Destroy()
		{
			this.Clear();
			this.newText = null;
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x00034026 File Offset: 0x00032226
		internal void ChangeFont(Font labelsFont)
		{
			this.font = labelsFont;
			this.ApplyFont();
			this.ApplyBackground();
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0003403B File Offset: 0x0003223B
		internal void ChangeFontSize(int newSize)
		{
			this.fontSize = newSize;
			if (this.uiText != null)
			{
				this.uiText.fontSize = this.fontSize;
			}
			this.ApplyBackground();
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x00034069 File Offset: 0x00032269
		internal void ChangeOffset(Vector2 newPixelOffset)
		{
			this.pixelOffset = newPixelOffset;
			this.NormalizeOffset();
			if (this.labelTransform != null)
			{
				this.labelTransform.anchoredPosition = this.pixelOffset;
			}
			this.ApplyBackground();
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0003409D File Offset: 0x0003229D
		internal void ChangeLineSpacing(float newValueLineSpacing)
		{
			this.lineSpacing = newValueLineSpacing;
			if (this.uiText != null)
			{
				this.uiText.lineSpacing = newValueLineSpacing;
			}
			this.ApplyBackground();
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x000340C6 File Offset: 0x000322C6
		internal void ChangeBackground(bool enabled)
		{
			this.background.enabled = enabled;
			this.ApplyBackground();
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x000340DA File Offset: 0x000322DA
		internal void ChangeBackgroundColor(Color color)
		{
			this.background.color = color;
			this.ApplyBackground();
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x000340EE File Offset: 0x000322EE
		public void ChangeBackgroundPadding(int backgroundPadding)
		{
			this.background.padding = backgroundPadding;
			this.ApplyBackground();
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x00034102 File Offset: 0x00032302
		internal void ChangeShadow(bool enabled)
		{
			this.shadow.enabled = enabled;
			this.ApplyShadow();
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00034116 File Offset: 0x00032316
		internal void ChangeShadowColor(Color color)
		{
			this.shadow.color = color;
			this.ApplyShadow();
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0003412A File Offset: 0x0003232A
		internal void ChangeShadowDistance(Vector2 distance)
		{
			this.shadow.distance = distance;
			this.ApplyShadow();
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x0003413E File Offset: 0x0003233E
		internal void ChangeOutline(bool enabled)
		{
			this.outline.enabled = enabled;
			this.ApplyOutline();
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x00034152 File Offset: 0x00032352
		internal void ChangeOutlineColor(Color color)
		{
			this.outline.color = color;
			this.ApplyOutline();
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x00034166 File Offset: 0x00032366
		internal void ChangeOutlineDistance(Vector2 distance)
		{
			this.outline.distance = distance;
			this.ApplyOutline();
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0003417C File Offset: 0x0003237C
		private void UpdateTextPosition()
		{
			this.labelTransform.localRotation = Quaternion.identity;
			this.labelTransform.sizeDelta = Vector2.zero;
			this.labelTransform.anchoredPosition = this.pixelOffset;
			switch (this.anchor)
			{
			case LabelAnchor.UpperLeft:
				this.uiText.alignment = TextAnchor.UpperLeft;
				this.labelTransform.anchorMin = Vector2.up;
				this.labelTransform.anchorMax = Vector2.up;
				this.labelTransform.pivot = new Vector2(0f, 1f);
				return;
			case LabelAnchor.UpperRight:
				this.uiText.alignment = TextAnchor.UpperRight;
				this.labelTransform.anchorMin = Vector2.one;
				this.labelTransform.anchorMax = Vector2.one;
				this.labelTransform.pivot = new Vector2(1f, 1f);
				return;
			case LabelAnchor.LowerLeft:
				this.uiText.alignment = TextAnchor.LowerLeft;
				this.labelTransform.anchorMin = Vector2.zero;
				this.labelTransform.anchorMax = Vector2.zero;
				this.labelTransform.pivot = new Vector2(0f, 0f);
				return;
			case LabelAnchor.LowerRight:
				this.uiText.alignment = TextAnchor.LowerRight;
				this.labelTransform.anchorMin = Vector2.right;
				this.labelTransform.anchorMax = Vector2.right;
				this.labelTransform.pivot = new Vector2(1f, 0f);
				return;
			case LabelAnchor.UpperCenter:
				this.uiText.alignment = TextAnchor.UpperCenter;
				this.labelTransform.anchorMin = new Vector2(0.5f, 1f);
				this.labelTransform.anchorMax = new Vector2(0.5f, 1f);
				this.labelTransform.pivot = new Vector2(0.5f, 1f);
				return;
			case LabelAnchor.LowerCenter:
				this.uiText.alignment = TextAnchor.LowerCenter;
				this.labelTransform.anchorMin = new Vector2(0.5f, 0f);
				this.labelTransform.anchorMax = new Vector2(0.5f, 0f);
				this.labelTransform.pivot = new Vector2(0.5f, 0f);
				return;
			default:
				Debug.LogWarning("[AFPSCounter]: Unknown label anchor!", this.uiText);
				this.uiText.alignment = TextAnchor.UpperLeft;
				this.labelTransform.anchorMin = Vector2.up;
				this.labelTransform.anchorMax = Vector2.up;
				return;
			}
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x000343F4 File Offset: 0x000325F4
		private void NormalizeOffset()
		{
			switch (this.anchor)
			{
			case LabelAnchor.UpperLeft:
				this.pixelOffset.y = -this.pixelOffset.y;
				return;
			case LabelAnchor.UpperRight:
				this.pixelOffset.x = -this.pixelOffset.x;
				this.pixelOffset.y = -this.pixelOffset.y;
				return;
			case LabelAnchor.LowerLeft:
				break;
			case LabelAnchor.LowerRight:
				this.pixelOffset.x = -this.pixelOffset.x;
				return;
			case LabelAnchor.UpperCenter:
				this.pixelOffset.y = -this.pixelOffset.y;
				this.pixelOffset.x = 0f;
				return;
			case LabelAnchor.LowerCenter:
				this.pixelOffset.x = 0f;
				return;
			default:
				this.pixelOffset.y = -this.pixelOffset.y;
				break;
			}
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x000344DC File Offset: 0x000326DC
		private void ApplyBackground()
		{
			if (this.uiText == null)
			{
				return;
			}
			if (this.background.enabled && !this.backgroundImage)
			{
				this.backgroundImage = this.labelGameObject.AddComponent<Image>();
			}
			if (!this.background.enabled && this.backgroundImage)
			{
				UnityEngine.Object.Destroy(this.backgroundImage);
				this.backgroundImage = null;
			}
			if (this.backgroundImage != null)
			{
				if (this.backgroundImage.color != this.background.color)
				{
					this.backgroundImage.color = this.background.color;
				}
				if (this.labelGroup.padding.bottom != this.background.padding)
				{
					this.labelGroup.padding.top = this.background.padding;
					this.labelGroup.padding.left = this.background.padding;
					this.labelGroup.padding.right = this.background.padding;
					this.labelGroup.padding.bottom = this.background.padding;
					this.labelGroup.SetLayoutHorizontal();
				}
			}
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x00034628 File Offset: 0x00032828
		private void ApplyShadow()
		{
			if (this.uiText == null)
			{
				return;
			}
			if (this.shadow.enabled && !this.shadowComponent)
			{
				this.shadowComponent = this.uiTextGameObject.AddComponent<Shadow>();
			}
			if (!this.shadow.enabled && this.shadowComponent)
			{
				UnityEngine.Object.Destroy(this.shadowComponent);
			}
			if (this.shadowComponent != null)
			{
				this.shadowComponent.effectColor = this.shadow.color;
				this.shadowComponent.effectDistance = this.shadow.distance;
			}
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x000346D0 File Offset: 0x000328D0
		private void ApplyOutline()
		{
			if (this.uiText == null)
			{
				return;
			}
			if (this.outline.enabled && !this.outlineComponent)
			{
				this.outlineComponent = this.uiTextGameObject.AddComponent<Outline>();
			}
			if (!this.outline.enabled && this.outlineComponent)
			{
				UnityEngine.Object.Destroy(this.outlineComponent);
			}
			if (this.outlineComponent != null)
			{
				this.outlineComponent.effectColor = this.outline.color;
				this.outlineComponent.effectDistance = this.outline.distance;
			}
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x00034776 File Offset: 0x00032976
		private void ApplyFont()
		{
			if (this.uiText == null)
			{
				return;
			}
			if (this.font == null)
			{
				this.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
			}
			this.uiText.font = this.font;
		}

		// Token: 0x04000B70 RID: 2928
		internal GameObject container;

		// Token: 0x04000B71 RID: 2929
		internal LabelAnchor anchor;

		// Token: 0x04000B72 RID: 2930
		internal StringBuilder newText;

		// Token: 0x04000B73 RID: 2931
		internal bool dirty;

		// Token: 0x04000B74 RID: 2932
		private GameObject labelGameObject;

		// Token: 0x04000B75 RID: 2933
		private RectTransform labelTransform;

		// Token: 0x04000B76 RID: 2934
		private ContentSizeFitter labelFitter;

		// Token: 0x04000B77 RID: 2935
		private HorizontalLayoutGroup labelGroup;

		// Token: 0x04000B78 RID: 2936
		private GameObject uiTextGameObject;

		// Token: 0x04000B79 RID: 2937
		private Text uiText;

		// Token: 0x04000B7A RID: 2938
		private Font font;

		// Token: 0x04000B7B RID: 2939
		private int fontSize;

		// Token: 0x04000B7C RID: 2940
		private float lineSpacing;

		// Token: 0x04000B7D RID: 2941
		private Vector2 pixelOffset;

		// Token: 0x04000B7E RID: 2942
		private readonly LabelEffect background;

		// Token: 0x04000B7F RID: 2943
		private Image backgroundImage;

		// Token: 0x04000B80 RID: 2944
		private readonly LabelEffect shadow;

		// Token: 0x04000B81 RID: 2945
		private Shadow shadowComponent;

		// Token: 0x04000B82 RID: 2946
		private readonly LabelEffect outline;

		// Token: 0x04000B83 RID: 2947
		private Outline outlineComponent;
	}
}
