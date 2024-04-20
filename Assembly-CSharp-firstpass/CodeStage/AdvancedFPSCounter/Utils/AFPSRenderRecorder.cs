using System;
using UnityEngine;

namespace CodeStage.AdvancedFPSCounter.Utils
{
	// Token: 0x020001F6 RID: 502
	[DisallowMultipleComponent]
	public class AFPSRenderRecorder : MonoBehaviour
	{
		// Token: 0x06001000 RID: 4096 RVA: 0x00030DAF File Offset: 0x0002EFAF
		private void OnPreCull()
		{
			if (this.recording || AFPSCounter.Instance == null)
			{
				return;
			}
			this.recording = true;
			this.renderTime = Time.realtimeSinceStartup;
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x00030DDC File Offset: 0x0002EFDC
		private void OnPostRender()
		{
			if (!this.recording || AFPSCounter.Instance == null)
			{
				return;
			}
			this.recording = false;
			this.renderTime = Time.realtimeSinceStartup - this.renderTime;
			AFPSCounter.Instance.fpsCounter.AddRenderTime(this.renderTime * 1000f);
		}

		// Token: 0x04000B6B RID: 2923
		private bool recording;

		// Token: 0x04000B6C RID: 2924
		private float renderTime;
	}
}
