using System;
using System.Collections;
using CodeStage.AdvancedFPSCounter.Utils;
using UnityEngine;
using UnityEngine.Profiling;

namespace CodeStage.AdvancedFPSCounter.CountersData
{
	// Token: 0x02000200 RID: 512
	[AddComponentMenu("")]
	[Serializable]
	public class MemoryCounterData : UpdatableCounterData
	{
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060010AD RID: 4269 RVA: 0x00036657 File Offset: 0x00034857
		// (set) Token: 0x060010AE RID: 4270 RVA: 0x0003665F File Offset: 0x0003485F
		public bool Precise
		{
			get
			{
				return this.precise;
			}
			set
			{
				if (this.precise == value || !Application.isPlaying)
				{
					return;
				}
				this.precise = value;
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x00036688 File Offset: 0x00034888
		// (set) Token: 0x060010B0 RID: 4272 RVA: 0x00036690 File Offset: 0x00034890
		public bool Total
		{
			get
			{
				return this.total;
			}
			set
			{
				if (this.total == value || !Application.isPlaying)
				{
					return;
				}
				this.total = value;
				if (!this.total)
				{
					this.LastTotalValue = 0L;
				}
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060010B1 RID: 4273 RVA: 0x000366C9 File Offset: 0x000348C9
		// (set) Token: 0x060010B2 RID: 4274 RVA: 0x000366D1 File Offset: 0x000348D1
		public bool Allocated
		{
			get
			{
				return this.allocated;
			}
			set
			{
				if (this.allocated == value || !Application.isPlaying)
				{
					return;
				}
				this.allocated = value;
				if (!this.allocated)
				{
					this.LastAllocatedValue = 0L;
				}
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060010B3 RID: 4275 RVA: 0x0003670A File Offset: 0x0003490A
		// (set) Token: 0x060010B4 RID: 4276 RVA: 0x00036712 File Offset: 0x00034912
		public bool MonoUsage
		{
			get
			{
				return this.monoUsage;
			}
			set
			{
				if (this.monoUsage == value || !Application.isPlaying)
				{
					return;
				}
				this.monoUsage = value;
				if (!this.monoUsage)
				{
					this.LastMonoValue = 0L;
				}
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060010B5 RID: 4277 RVA: 0x0003674B File Offset: 0x0003494B
		// (set) Token: 0x060010B6 RID: 4278 RVA: 0x00036753 File Offset: 0x00034953
		public bool Gfx
		{
			get
			{
				return this.gfx;
			}
			set
			{
				if (this.gfx == value || !Application.isPlaying)
				{
					return;
				}
				this.gfx = value;
				if (!this.gfx)
				{
					this.LastGfxValue = 0L;
				}
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060010B7 RID: 4279 RVA: 0x0003678C File Offset: 0x0003498C
		// (set) Token: 0x060010B8 RID: 4280 RVA: 0x00036794 File Offset: 0x00034994
		public long LastTotalValue { get; private set; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060010B9 RID: 4281 RVA: 0x0003679D File Offset: 0x0003499D
		// (set) Token: 0x060010BA RID: 4282 RVA: 0x000367A5 File Offset: 0x000349A5
		public long LastAllocatedValue { get; private set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060010BB RID: 4283 RVA: 0x000367AE File Offset: 0x000349AE
		// (set) Token: 0x060010BC RID: 4284 RVA: 0x000367B6 File Offset: 0x000349B6
		public long LastMonoValue { get; private set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060010BD RID: 4285 RVA: 0x000367BF File Offset: 0x000349BF
		// (set) Token: 0x060010BE RID: 4286 RVA: 0x000367C7 File Offset: 0x000349C7
		public long LastGfxValue { get; private set; }

		// Token: 0x060010BF RID: 4287 RVA: 0x000367D0 File Offset: 0x000349D0
		internal MemoryCounterData()
		{
			this.color = new Color32(234, 238, 101, byte.MaxValue);
			this.style = FontStyle.Bold;
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x00036828 File Offset: 0x00034A28
		internal override void UpdateValue(bool force)
		{
			if (!this.enabled)
			{
				return;
			}
			if (force)
			{
				if (!this.inited && this.HasData())
				{
					this.Activate();
					return;
				}
				if (this.inited && !this.HasData())
				{
					this.Deactivate();
					return;
				}
			}
			if (this.total)
			{
				long totalReservedMemoryLong = Profiler.GetTotalReservedMemoryLong();
				long num = 0L;
				bool flag;
				if (this.precise)
				{
					flag = (this.LastTotalValue != totalReservedMemoryLong);
				}
				else
				{
					num = totalReservedMemoryLong / 1048576L;
					flag = (this.LastTotalValue != num);
				}
				if (flag || force)
				{
					this.LastTotalValue = (this.precise ? totalReservedMemoryLong : num);
					this.dirty = true;
				}
			}
			if (this.allocated)
			{
				long totalAllocatedMemoryLong = Profiler.GetTotalAllocatedMemoryLong();
				long num2 = 0L;
				bool flag2;
				if (this.precise)
				{
					flag2 = (this.LastAllocatedValue != totalAllocatedMemoryLong);
				}
				else
				{
					num2 = totalAllocatedMemoryLong / 1048576L;
					flag2 = (this.LastAllocatedValue != num2);
				}
				if (flag2 || force)
				{
					this.LastAllocatedValue = (this.precise ? totalAllocatedMemoryLong : num2);
					this.dirty = true;
				}
			}
			if (this.monoUsage)
			{
				long totalMemory = GC.GetTotalMemory(false);
				long num3 = 0L;
				bool flag3;
				if (this.precise)
				{
					flag3 = (this.LastMonoValue != totalMemory);
				}
				else
				{
					num3 = totalMemory / 1048576L;
					flag3 = (this.LastMonoValue != num3);
				}
				if (flag3 || force)
				{
					this.LastMonoValue = (this.precise ? totalMemory : num3);
					this.dirty = true;
				}
			}
			if (!this.dirty || this.main.OperationMode != OperationMode.Normal)
			{
				return;
			}
			bool flag4 = false;
			this.text.Length = 0;
			this.text.Append(this.colorCached);
			if (this.total)
			{
				this.text.Append("MEM TOTAL: ");
				if (this.precise)
				{
					this.text.AppendLookup((float)this.LastTotalValue / 1048576f);
				}
				else
				{
					this.text.AppendLookup(this.LastTotalValue);
				}
				this.text.Append(" MB");
				flag4 = true;
			}
			if (this.allocated)
			{
				if (flag4)
				{
					this.text.Append('\n');
				}
				this.text.Append("MEM ALLOC: ");
				if (this.precise)
				{
					this.text.AppendLookup((float)this.LastAllocatedValue / 1048576f);
				}
				else
				{
					this.text.AppendLookup(this.LastAllocatedValue);
				}
				this.text.Append(" MB");
				flag4 = true;
			}
			if (this.monoUsage)
			{
				if (flag4)
				{
					this.text.Append('\n');
				}
				this.text.Append("MEM MONO: ");
				if (this.precise)
				{
					this.text.AppendLookup((float)this.LastMonoValue / 1048576f);
				}
				else
				{
					this.text.AppendLookup(this.LastMonoValue);
				}
				this.text.Append(" MB");
			}
			this.text.Append("</color>");
			base.ApplyTextStyles();
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x00036B2C File Offset: 0x00034D2C
		protected override void PerformActivationActions()
		{
			base.PerformActivationActions();
			if (!this.HasData())
			{
				return;
			}
			this.LastTotalValue = 0L;
			this.LastAllocatedValue = 0L;
			this.LastMonoValue = 0L;
			if (this.main.OperationMode == OperationMode.Normal)
			{
				if (this.colorCached == null)
				{
					this.colorCached = string.Format("<color=#{0}>", AFPSCounter.Color32ToHex(this.color));
				}
				this.text.Append(this.colorCached);
				if (this.total)
				{
					if (this.precise)
					{
						this.text.Append("MEM TOTAL: ").Append("0.00").Append(" MB");
					}
					else
					{
						this.text.Append("MEM TOTAL: ").Append(0).Append(" MB");
					}
				}
				if (this.allocated)
				{
					if (this.text.Length > 0)
					{
						this.text.Append('\n');
					}
					if (this.precise)
					{
						this.text.Append("MEM ALLOC: ").Append("0.00").Append(" MB");
					}
					else
					{
						this.text.Append("MEM ALLOC: ").Append(0).Append(" MB");
					}
				}
				if (this.monoUsage)
				{
					if (this.text.Length > 0)
					{
						this.text.Append('\n');
					}
					if (this.precise)
					{
						this.text.Append("MEM MONO: ").Append("0.00").Append(" MB");
					}
					else
					{
						this.text.Append("MEM MONO: ").Append(0).Append(" MB");
					}
				}
				this.text.Append("</color>");
				base.ApplyTextStyles();
				this.dirty = true;
			}
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x00036D09 File Offset: 0x00034F09
		protected override void PerformDeActivationActions()
		{
			base.PerformDeActivationActions();
			if (this.text != null)
			{
				this.text.Length = 0;
			}
			this.main.MakeDrawableLabelDirty(this.anchor);
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x00036D36 File Offset: 0x00034F36
		protected override IEnumerator UpdateCounter()
		{
			for (;;)
			{
				this.UpdateValue();
				this.main.UpdateTexts();
				float previousUpdateTime = Time.unscaledTime;
				while (Time.unscaledTime < previousUpdateTime + this.updateInterval)
				{
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x00036D45 File Offset: 0x00034F45
		protected override bool HasData()
		{
			return this.total || this.allocated || this.monoUsage || this.gfx;
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x00036D67 File Offset: 0x00034F67
		protected override void CacheCurrentColor()
		{
			this.colorCached = string.Format("<color=#{0}>", AFPSCounter.Color32ToHex(this.color));
		}

		// Token: 0x04000BEE RID: 3054
		public const long MemoryDivider = 1048576L;

		// Token: 0x04000BEF RID: 3055
		private const string TextStart = "<color=#{0}>";

		// Token: 0x04000BF0 RID: 3056
		private const string LineStartTotal = "MEM TOTAL: ";

		// Token: 0x04000BF1 RID: 3057
		private const string LineStartAllocated = "MEM ALLOC: ";

		// Token: 0x04000BF2 RID: 3058
		private const string LineStartMono = "MEM MONO: ";

		// Token: 0x04000BF3 RID: 3059
		private const string LineEnd = " MB";

		// Token: 0x04000BF4 RID: 3060
		private const string TextEnd = "</color>";

		// Token: 0x04000BF5 RID: 3061
		[Tooltip("Allows to output memory usage more precisely thus using a bit more system resources.")]
		[SerializeField]
		private bool precise = true;

		// Token: 0x04000BF6 RID: 3062
		[Tooltip("Allows to see private memory amount reserved for application. This memory can’t be used by other applications.")]
		[SerializeField]
		private bool total = true;

		// Token: 0x04000BF7 RID: 3063
		[Tooltip("Allows to see amount of memory, currently allocated by application.")]
		[SerializeField]
		private bool allocated = true;

		// Token: 0x04000BF8 RID: 3064
		[Tooltip("Allows to see amount of memory, allocated by managed Mono objects, such as UnityEngine.Object and everything derived from it for example.")]
		[SerializeField]
		private bool monoUsage;

		// Token: 0x04000BF9 RID: 3065
		[Tooltip("Allows to see amount of allocated memory for the graphics driver (dev builds only).")]
		[SerializeField]
		private bool gfx = true;
	}
}
