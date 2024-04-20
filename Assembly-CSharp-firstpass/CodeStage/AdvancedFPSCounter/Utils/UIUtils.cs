using System;
using UnityEngine;

namespace CodeStage.AdvancedFPSCounter.Utils
{
	// Token: 0x020001F8 RID: 504
	internal class UIUtils
	{
		// Token: 0x06001008 RID: 4104 RVA: 0x00033CE0 File Offset: 0x00031EE0
		internal static void ResetRectTransform(RectTransform rectTransform)
		{
			rectTransform.localRotation = Quaternion.identity;
			rectTransform.localScale = Vector3.one;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.anchoredPosition3D = Vector3.zero;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
		}
	}
}
