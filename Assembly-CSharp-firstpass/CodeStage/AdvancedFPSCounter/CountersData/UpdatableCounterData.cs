using System;
using System.Collections;
using UnityEngine;

namespace CodeStage.AdvancedFPSCounter.CountersData
{
	// Token: 0x020001FD RID: 509
	public abstract class UpdatableCounterData : BaseCounterData
	{
		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600103B RID: 4155 RVA: 0x00034B71 File Offset: 0x00032D71
		// (set) Token: 0x0600103C RID: 4156 RVA: 0x00034B79 File Offset: 0x00032D79
		public float UpdateInterval
		{
			get
			{
				return this.updateInterval;
			}
			set
			{
				if (Math.Abs(this.updateInterval - value) < 0.001f || !Application.isPlaying)
				{
					return;
				}
				this.updateInterval = value;
			}
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x00034B9E File Offset: 0x00032D9E
		protected override void PerformInitActions()
		{
			base.PerformInitActions();
			this.StartUpdateCoroutine();
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x00034BAC File Offset: 0x00032DAC
		protected override void PerformDeActivationActions()
		{
			base.PerformDeActivationActions();
			this.StopUpdateCoroutine();
		}

		// Token: 0x0600103F RID: 4159
		protected abstract IEnumerator UpdateCounter();

		// Token: 0x06001040 RID: 4160 RVA: 0x00034BBA File Offset: 0x00032DBA
		private void StartUpdateCoroutine()
		{
			this.updateCoroutine = this.main.StartCoroutine(this.UpdateCounter());
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x00034BD3 File Offset: 0x00032DD3
		private void StopUpdateCoroutine()
		{
			this.main.StopCoroutine(this.updateCoroutine);
		}

		// Token: 0x04000B9D RID: 2973
		protected Coroutine updateCoroutine;

		// Token: 0x04000B9E RID: 2974
		[Tooltip("Update interval in seconds.")]
		[Range(0.1f, 10f)]
		[SerializeField]
		protected float updateInterval = 0.5f;
	}
}
