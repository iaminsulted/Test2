using System;
using System.Text;
using CodeStage.AdvancedFPSCounter.Labels;
using UnityEngine;

namespace CodeStage.AdvancedFPSCounter.CountersData
{
	// Token: 0x020001FC RID: 508
	[AddComponentMenu("")]
	[Serializable]
	public abstract class BaseCounterData
	{
		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06001023 RID: 4131 RVA: 0x000347E9 File Offset: 0x000329E9
		// (set) Token: 0x06001024 RID: 4132 RVA: 0x000347F1 File Offset: 0x000329F1
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				if (this.enabled == value || !Application.isPlaying)
				{
					return;
				}
				this.enabled = value;
				if (this.enabled)
				{
					this.Activate();
				}
				else
				{
					this.Deactivate();
				}
				this.main.UpdateTexts();
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06001025 RID: 4133 RVA: 0x0003482C File Offset: 0x00032A2C
		// (set) Token: 0x06001026 RID: 4134 RVA: 0x00034834 File Offset: 0x00032A34
		public LabelAnchor Anchor
		{
			get
			{
				return this.anchor;
			}
			set
			{
				if (this.anchor == value || !Application.isPlaying)
				{
					return;
				}
				LabelAnchor labelAnchor = this.anchor;
				this.anchor = value;
				if (!this.enabled)
				{
					return;
				}
				this.dirty = true;
				this.main.MakeDrawableLabelDirty(labelAnchor);
				this.main.UpdateTexts();
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06001027 RID: 4135 RVA: 0x00034887 File Offset: 0x00032A87
		// (set) Token: 0x06001028 RID: 4136 RVA: 0x0003488F File Offset: 0x00032A8F
		public Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				if (this.color == value || !Application.isPlaying)
				{
					return;
				}
				this.color = value;
				if (!this.enabled)
				{
					return;
				}
				this.CacheCurrentColor();
				this.Refresh();
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06001029 RID: 4137 RVA: 0x000348C3 File Offset: 0x00032AC3
		// (set) Token: 0x0600102A RID: 4138 RVA: 0x000348CB File Offset: 0x00032ACB
		public FontStyle Style
		{
			get
			{
				return this.style;
			}
			set
			{
				if (this.style == value || !Application.isPlaying)
				{
					return;
				}
				this.style = value;
				if (!this.enabled)
				{
					return;
				}
				this.Refresh();
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600102B RID: 4139 RVA: 0x000348F4 File Offset: 0x00032AF4
		// (set) Token: 0x0600102C RID: 4140 RVA: 0x000348FC File Offset: 0x00032AFC
		public string ExtraText
		{
			get
			{
				return this.extraText;
			}
			set
			{
				if (this.extraText == value || !Application.isPlaying)
				{
					return;
				}
				this.extraText = value;
				if (!this.enabled)
				{
					return;
				}
				this.Refresh();
			}
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x0003492A File Offset: 0x00032B2A
		public void Refresh()
		{
			if (!this.enabled || !Application.isPlaying)
			{
				return;
			}
			this.UpdateValue(true);
			this.main.UpdateTexts();
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x0003494E File Offset: 0x00032B4E
		internal virtual void UpdateValue()
		{
			this.UpdateValue(false);
		}

		// Token: 0x0600102F RID: 4143
		internal abstract void UpdateValue(bool force);

		// Token: 0x06001030 RID: 4144 RVA: 0x00034957 File Offset: 0x00032B57
		internal void Init(AFPSCounter reference)
		{
			this.main = reference;
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x00034960 File Offset: 0x00032B60
		internal void Destroy()
		{
			this.main = null;
			if (this.text != null)
			{
				this.text.Remove(0, this.text.Length);
				this.text = null;
			}
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x00034990 File Offset: 0x00032B90
		internal virtual void Activate()
		{
			if (!this.enabled)
			{
				return;
			}
			if (this.main.OperationMode == OperationMode.Disabled)
			{
				return;
			}
			if (!this.HasData())
			{
				return;
			}
			if (this.text == null)
			{
				this.text = new StringBuilder(500);
			}
			else
			{
				this.text.Length = 0;
			}
			if (this.main.OperationMode == OperationMode.Normal && this.colorCached == null)
			{
				this.CacheCurrentColor();
			}
			this.PerformActivationActions();
			if (!this.inited)
			{
				this.PerformInitActions();
				this.inited = true;
			}
			this.UpdateValue();
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x00034A20 File Offset: 0x00032C20
		internal virtual void Deactivate()
		{
			if (!this.inited)
			{
				return;
			}
			if (this.text != null)
			{
				this.text.Length = 0;
			}
			this.main.MakeDrawableLabelDirty(this.anchor);
			this.PerformDeActivationActions();
			this.inited = false;
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x00034A5D File Offset: 0x00032C5D
		protected virtual void PerformInitActions()
		{
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x00034A5F File Offset: 0x00032C5F
		protected virtual void PerformActivationActions()
		{
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x00034A61 File Offset: 0x00032C61
		protected virtual void PerformDeActivationActions()
		{
		}

		// Token: 0x06001037 RID: 4151
		protected abstract bool HasData();

		// Token: 0x06001038 RID: 4152
		protected abstract void CacheCurrentColor();

		// Token: 0x06001039 RID: 4153 RVA: 0x00034A64 File Offset: 0x00032C64
		protected void ApplyTextStyles()
		{
			if (this.text.Length > 0)
			{
				switch (this.style)
				{
				case FontStyle.Normal:
					break;
				case FontStyle.Bold:
					this.text.Insert(0, "<b>");
					this.text.Append("</b>");
					break;
				case FontStyle.Italic:
					this.text.Insert(0, "<i>");
					this.text.Append("</i>");
					break;
				case FontStyle.BoldAndItalic:
					this.text.Insert(0, "<b>");
					this.text.Append("</b>");
					this.text.Insert(0, "<i>");
					this.text.Append("</i>");
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			if (!string.IsNullOrEmpty(this.extraText))
			{
				this.text.Append('\n').Append(this.extraText);
			}
		}

		// Token: 0x04000B8F RID: 2959
		protected const string BoldStart = "<b>";

		// Token: 0x04000B90 RID: 2960
		protected const string BoldEnd = "</b>";

		// Token: 0x04000B91 RID: 2961
		protected const string ItalicStart = "<i>";

		// Token: 0x04000B92 RID: 2962
		protected const string ItalicEnd = "</i>";

		// Token: 0x04000B93 RID: 2963
		internal StringBuilder text;

		// Token: 0x04000B94 RID: 2964
		internal bool dirty;

		// Token: 0x04000B95 RID: 2965
		[NonSerialized]
		protected AFPSCounter main;

		// Token: 0x04000B96 RID: 2966
		protected string colorCached;

		// Token: 0x04000B97 RID: 2967
		protected bool inited;

		// Token: 0x04000B98 RID: 2968
		[SerializeField]
		protected bool enabled = true;

		// Token: 0x04000B99 RID: 2969
		[Tooltip("Current anchoring label for the counter output.\nRefreshes both previous and specified label when switching anchor.")]
		[SerializeField]
		protected LabelAnchor anchor;

		// Token: 0x04000B9A RID: 2970
		[Tooltip("Regular color of the counter output.")]
		[SerializeField]
		protected Color color;

		// Token: 0x04000B9B RID: 2971
		[Tooltip("Controls text style.")]
		[SerializeField]
		protected FontStyle style;

		// Token: 0x04000B9C RID: 2972
		[Tooltip("Additional text to append to the end of the counter in normal Operation Mode.")]
		protected string extraText;
	}
}
