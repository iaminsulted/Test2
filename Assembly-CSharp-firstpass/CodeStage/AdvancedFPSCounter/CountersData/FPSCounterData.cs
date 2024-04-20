using System;
using System.Collections;
using CodeStage.AdvancedFPSCounter.Utils;
using UnityEngine;

namespace CodeStage.AdvancedFPSCounter.CountersData
{
	// Token: 0x020001FF RID: 511
	[AddComponentMenu("")]
	[Serializable]
	public class FPSCounterData : UpdatableCounterData
	{
		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06001067 RID: 4199 RVA: 0x0003547C File Offset: 0x0003367C
		// (remove) Token: 0x06001068 RID: 4200 RVA: 0x000354B4 File Offset: 0x000336B4
		public event Action<FPSLevel> OnFPSLevelChange;

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06001069 RID: 4201 RVA: 0x000354E9 File Offset: 0x000336E9
		// (set) Token: 0x0600106A RID: 4202 RVA: 0x000354F1 File Offset: 0x000336F1
		public bool RealtimeFPS
		{
			get
			{
				return this.realtimeFPS;
			}
			set
			{
				if (this.realtimeFPS == value || !Application.isPlaying)
				{
					return;
				}
				this.realtimeFPS = value;
				if (!this.realtimeFPS)
				{
					this.LastValue = 0;
				}
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600106B RID: 4203 RVA: 0x00035529 File Offset: 0x00033729
		// (set) Token: 0x0600106C RID: 4204 RVA: 0x00035531 File Offset: 0x00033731
		public bool Milliseconds
		{
			get
			{
				return this.milliseconds;
			}
			set
			{
				if (this.milliseconds == value || !Application.isPlaying)
				{
					return;
				}
				this.milliseconds = value;
				if (!this.milliseconds)
				{
					this.LastMillisecondsValue = 0f;
				}
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600106D RID: 4205 RVA: 0x0003556D File Offset: 0x0003376D
		// (set) Token: 0x0600106E RID: 4206 RVA: 0x00035575 File Offset: 0x00033775
		public bool Average
		{
			get
			{
				return this.average;
			}
			set
			{
				if (this.average == value || !Application.isPlaying)
				{
					return;
				}
				this.average = value;
				if (!this.average)
				{
					this.ResetAverage();
				}
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600106F RID: 4207 RVA: 0x000355AC File Offset: 0x000337AC
		// (set) Token: 0x06001070 RID: 4208 RVA: 0x000355B4 File Offset: 0x000337B4
		public bool AverageMilliseconds
		{
			get
			{
				return this.averageMilliseconds;
			}
			set
			{
				if (this.averageMilliseconds == value || !Application.isPlaying)
				{
					return;
				}
				this.averageMilliseconds = value;
				if (!this.averageMilliseconds)
				{
					this.LastAverageMillisecondsValue = 0f;
				}
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06001071 RID: 4209 RVA: 0x000355F0 File Offset: 0x000337F0
		// (set) Token: 0x06001072 RID: 4210 RVA: 0x000355F8 File Offset: 0x000337F8
		public bool AverageNewLine
		{
			get
			{
				return this.averageNewLine;
			}
			set
			{
				if (this.averageNewLine == value || !Application.isPlaying)
				{
					return;
				}
				this.averageNewLine = value;
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06001073 RID: 4211 RVA: 0x00035621 File Offset: 0x00033821
		// (set) Token: 0x06001074 RID: 4212 RVA: 0x0003562C File Offset: 0x0003382C
		public int AverageSamples
		{
			get
			{
				return this.averageSamples;
			}
			set
			{
				if (this.averageSamples == value || !Application.isPlaying)
				{
					return;
				}
				this.averageSamples = value;
				if (!this.enabled)
				{
					return;
				}
				if (this.averageSamples > 0)
				{
					if (this.accumulatedAverageSamples == null)
					{
						this.accumulatedAverageSamples = new float[this.averageSamples];
					}
					else if (this.accumulatedAverageSamples.Length != this.averageSamples)
					{
						Array.Resize<float>(ref this.accumulatedAverageSamples, this.averageSamples);
					}
				}
				else
				{
					this.accumulatedAverageSamples = null;
				}
				this.ResetAverage();
				base.Refresh();
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06001075 RID: 4213 RVA: 0x000356B4 File Offset: 0x000338B4
		// (set) Token: 0x06001076 RID: 4214 RVA: 0x000356BC File Offset: 0x000338BC
		public bool MinMax
		{
			get
			{
				return this.minMax;
			}
			set
			{
				if (this.minMax == value || !Application.isPlaying)
				{
					return;
				}
				this.minMax = value;
				if (!this.minMax)
				{
					this.ResetMinMax(false);
				}
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06001077 RID: 4215 RVA: 0x000356F4 File Offset: 0x000338F4
		// (set) Token: 0x06001078 RID: 4216 RVA: 0x000356FC File Offset: 0x000338FC
		public bool MinMaxMilliseconds
		{
			get
			{
				return this.minMaxMilliseconds;
			}
			set
			{
				if (this.minMaxMilliseconds == value || !Application.isPlaying)
				{
					return;
				}
				this.minMaxMilliseconds = value;
				if (!this.minMaxMilliseconds)
				{
					this.LastMinMillisecondsValue = 0f;
					this.LastMaxMillisecondsValue = 0f;
				}
				else
				{
					this.LastMinMillisecondsValue = 1000f / (float)this.LastMinimumValue;
					this.LastMaxMillisecondsValue = 1000f / (float)this.LastMaximumValue;
				}
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06001079 RID: 4217 RVA: 0x00035776 File Offset: 0x00033976
		// (set) Token: 0x0600107A RID: 4218 RVA: 0x0003577E File Offset: 0x0003397E
		public bool MinMaxNewLine
		{
			get
			{
				return this.minMaxNewLine;
			}
			set
			{
				if (this.minMaxNewLine == value || !Application.isPlaying)
				{
					return;
				}
				this.minMaxNewLine = value;
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x0600107B RID: 4219 RVA: 0x000357A7 File Offset: 0x000339A7
		// (set) Token: 0x0600107C RID: 4220 RVA: 0x000357AF File Offset: 0x000339AF
		public bool MinMaxTwoLines
		{
			get
			{
				return this.minMaxTwoLines;
			}
			set
			{
				if (this.minMaxTwoLines == value || !Application.isPlaying)
				{
					return;
				}
				this.minMaxTwoLines = value;
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600107D RID: 4221 RVA: 0x000357D8 File Offset: 0x000339D8
		// (set) Token: 0x0600107E RID: 4222 RVA: 0x000357E0 File Offset: 0x000339E0
		public bool Render
		{
			get
			{
				return this.render;
			}
			set
			{
				if (this.render == value || !Application.isPlaying)
				{
					return;
				}
				this.render = value;
				if (!this.render)
				{
					if (this.renderAutoAdd)
					{
						FPSCounterData.TryToRemoveRenderRecorder();
					}
					return;
				}
				this.previousFrameCount = Time.frameCount;
				if (this.renderAutoAdd)
				{
					FPSCounterData.TryToAddRenderRecorder();
				}
				base.Refresh();
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600107F RID: 4223 RVA: 0x00035839 File Offset: 0x00033A39
		// (set) Token: 0x06001080 RID: 4224 RVA: 0x00035841 File Offset: 0x00033A41
		public bool RenderNewLine
		{
			get
			{
				return this.renderNewLine;
			}
			set
			{
				if (this.renderNewLine == value || !Application.isPlaying)
				{
					return;
				}
				this.renderNewLine = value;
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06001081 RID: 4225 RVA: 0x0003586A File Offset: 0x00033A6A
		// (set) Token: 0x06001082 RID: 4226 RVA: 0x00035872 File Offset: 0x00033A72
		public bool RenderAutoAdd
		{
			get
			{
				return this.renderAutoAdd;
			}
			set
			{
				if (this.renderAutoAdd == value || !Application.isPlaying)
				{
					return;
				}
				this.renderAutoAdd = value;
				if (!this.enabled)
				{
					return;
				}
				FPSCounterData.TryToAddRenderRecorder();
				base.Refresh();
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06001083 RID: 4227 RVA: 0x000358A0 File Offset: 0x00033AA0
		// (set) Token: 0x06001084 RID: 4228 RVA: 0x000358A8 File Offset: 0x00033AA8
		public Color ColorWarning
		{
			get
			{
				return this.colorWarning;
			}
			set
			{
				if (this.colorWarning == value || !Application.isPlaying)
				{
					return;
				}
				this.colorWarning = value;
				if (!this.enabled)
				{
					return;
				}
				this.CacheWarningColor();
				base.Refresh();
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06001085 RID: 4229 RVA: 0x000358DC File Offset: 0x00033ADC
		// (set) Token: 0x06001086 RID: 4230 RVA: 0x000358E4 File Offset: 0x00033AE4
		public Color ColorCritical
		{
			get
			{
				return this.colorCritical;
			}
			set
			{
				if (this.colorCritical == value || !Application.isPlaying)
				{
					return;
				}
				this.colorCritical = value;
				if (!this.enabled)
				{
					return;
				}
				this.CacheCriticalColor();
				base.Refresh();
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06001087 RID: 4231 RVA: 0x00035918 File Offset: 0x00033B18
		// (set) Token: 0x06001088 RID: 4232 RVA: 0x00035920 File Offset: 0x00033B20
		public Color ColorRender
		{
			get
			{
				return this.colorRender;
			}
			set
			{
				if (this.colorRender == value || !Application.isPlaying)
				{
					return;
				}
				this.colorRender = value;
				if (!this.enabled)
				{
					return;
				}
				this.CacheCurrentColor();
				base.Refresh();
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06001089 RID: 4233 RVA: 0x00035954 File Offset: 0x00033B54
		// (set) Token: 0x0600108A RID: 4234 RVA: 0x0003595C File Offset: 0x00033B5C
		public int LastValue { get; private set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600108B RID: 4235 RVA: 0x00035965 File Offset: 0x00033B65
		// (set) Token: 0x0600108C RID: 4236 RVA: 0x0003596D File Offset: 0x00033B6D
		public float LastMillisecondsValue { get; private set; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600108D RID: 4237 RVA: 0x00035976 File Offset: 0x00033B76
		// (set) Token: 0x0600108E RID: 4238 RVA: 0x0003597E File Offset: 0x00033B7E
		public float LastRenderValue { get; private set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600108F RID: 4239 RVA: 0x00035987 File Offset: 0x00033B87
		// (set) Token: 0x06001090 RID: 4240 RVA: 0x0003598F File Offset: 0x00033B8F
		public int LastAverageValue { get; private set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06001091 RID: 4241 RVA: 0x00035998 File Offset: 0x00033B98
		// (set) Token: 0x06001092 RID: 4242 RVA: 0x000359A0 File Offset: 0x00033BA0
		public float LastAverageMillisecondsValue { get; private set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06001093 RID: 4243 RVA: 0x000359A9 File Offset: 0x00033BA9
		// (set) Token: 0x06001094 RID: 4244 RVA: 0x000359B1 File Offset: 0x00033BB1
		public int LastMinimumValue { get; private set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06001095 RID: 4245 RVA: 0x000359BA File Offset: 0x00033BBA
		// (set) Token: 0x06001096 RID: 4246 RVA: 0x000359C2 File Offset: 0x00033BC2
		public int LastMaximumValue { get; private set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06001097 RID: 4247 RVA: 0x000359CB File Offset: 0x00033BCB
		// (set) Token: 0x06001098 RID: 4248 RVA: 0x000359D3 File Offset: 0x00033BD3
		public float LastMinMillisecondsValue { get; private set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06001099 RID: 4249 RVA: 0x000359DC File Offset: 0x00033BDC
		// (set) Token: 0x0600109A RID: 4250 RVA: 0x000359E4 File Offset: 0x00033BE4
		public float LastMaxMillisecondsValue { get; private set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600109B RID: 4251 RVA: 0x000359ED File Offset: 0x00033BED
		// (set) Token: 0x0600109C RID: 4252 RVA: 0x000359F5 File Offset: 0x00033BF5
		public FPSLevel CurrentFpsLevel { get; private set; }

		// Token: 0x0600109D RID: 4253 RVA: 0x00035A00 File Offset: 0x00033C00
		internal FPSCounterData()
		{
			this.color = new Color32(85, 218, 102, byte.MaxValue);
			this.colorRender = new Color32(167, 110, 209, byte.MaxValue);
			this.style = FontStyle.Bold;
			this.realtimeFPS = true;
			this.milliseconds = true;
			this.render = false;
			this.renderNewLine = true;
			this.average = true;
			this.averageMilliseconds = true;
			this.averageNewLine = true;
			this.resetAverageOnNewScene = true;
			this.minMax = true;
			this.minMaxNewLine = true;
			this.resetMinMaxOnNewScene = true;
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x00035B0C File Offset: 0x00033D0C
		public void ResetAverage()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.LastAverageValue = 0;
			this.currentAverageSamples = 0;
			this.currentAverageRaw = 0f;
			if (this.averageSamples > 0 && this.accumulatedAverageSamples != null)
			{
				Array.Clear(this.accumulatedAverageSamples, 0, this.accumulatedAverageSamples.Length);
			}
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x00035B5F File Offset: 0x00033D5F
		public void ResetMinMax(bool withoutUpdate = false)
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.LastMinimumValue = -1;
			this.LastMaximumValue = -1;
			this.minMaxIntervalsSkipped = 0;
			this.UpdateValue(true);
			this.dirty = true;
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x00035B8C File Offset: 0x00033D8C
		internal void OnLevelLoadedCallback()
		{
			if (this.minMax && this.resetMinMaxOnNewScene)
			{
				this.ResetMinMax(false);
			}
			if (this.average && this.resetAverageOnNewScene)
			{
				this.ResetAverage();
			}
			if (this.render && this.renderAutoAdd)
			{
				FPSCounterData.TryToAddRenderRecorder();
			}
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x00035BDB File Offset: 0x00033DDB
		internal void AddRenderTime(float time)
		{
			if (!this.enabled || !this.inited)
			{
				return;
			}
			this.renderTimeBank += time;
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x00035BFC File Offset: 0x00033DFC
		internal override void UpdateValue(bool force)
		{
			if (!this.enabled)
			{
				return;
			}
			if (!this.realtimeFPS && !this.average && !this.minMax && !this.render)
			{
				if (this.text.Length > 0)
				{
					this.text.Length = 0;
				}
				return;
			}
			int num = (int)this.newValue;
			if (this.LastValue != num || force)
			{
				this.LastValue = num;
				this.dirty = true;
			}
			if (this.LastValue <= this.criticalLevelValue)
			{
				if (this.LastValue != 0 && this.CurrentFpsLevel != FPSLevel.Critical)
				{
					this.CurrentFpsLevel = FPSLevel.Critical;
					if (this.OnFPSLevelChange != null)
					{
						this.OnFPSLevelChange(this.CurrentFpsLevel);
					}
				}
			}
			else if (this.LastValue < this.warningLevelValue)
			{
				if (this.LastValue != 0 && this.CurrentFpsLevel != FPSLevel.Warning)
				{
					this.CurrentFpsLevel = FPSLevel.Warning;
					if (this.OnFPSLevelChange != null)
					{
						this.OnFPSLevelChange(this.CurrentFpsLevel);
					}
				}
			}
			else if (this.LastValue != 0 && this.CurrentFpsLevel != FPSLevel.Normal)
			{
				this.CurrentFpsLevel = FPSLevel.Normal;
				if (this.OnFPSLevelChange != null)
				{
					this.OnFPSLevelChange(this.CurrentFpsLevel);
				}
			}
			if (this.dirty && this.milliseconds)
			{
				this.LastMillisecondsValue = 1000f / this.newValue;
			}
			if (this.render && this.renderTimeBank > 0f)
			{
				int frameCount = Time.frameCount;
				int num2 = frameCount - this.previousFrameCount;
				if (num2 == 0)
				{
					num2 = 1;
				}
				float num3 = this.renderTimeBank / (float)num2;
				if (Math.Abs(num3 - this.LastRenderValue) > 0.0001f || force)
				{
					this.LastRenderValue = num3;
					this.dirty = true;
				}
				this.previousFrameCount = frameCount;
				this.renderTimeBank = 0f;
			}
			int num4 = 0;
			if (this.average)
			{
				if (this.averageSamples == 0)
				{
					this.currentAverageSamples++;
					this.currentAverageRaw += ((float)this.LastValue - this.currentAverageRaw) / (float)this.currentAverageSamples;
				}
				else
				{
					if (this.accumulatedAverageSamples == null)
					{
						this.accumulatedAverageSamples = new float[this.averageSamples];
						this.ResetAverage();
					}
					this.accumulatedAverageSamples[this.currentAverageSamples % this.averageSamples] = (float)this.LastValue;
					this.currentAverageSamples++;
					this.currentAverageRaw = this.GetAverageFromAccumulatedSamples();
				}
				num4 = Mathf.RoundToInt(this.currentAverageRaw);
				if (this.LastAverageValue != num4 || force)
				{
					this.LastAverageValue = num4;
					this.dirty = true;
					if (this.averageMilliseconds)
					{
						this.LastAverageMillisecondsValue = 1000f / (float)this.LastAverageValue;
					}
				}
			}
			if (this.minMax)
			{
				if (this.minMaxIntervalsSkipped <= this.minMaxIntervalsToSkip)
				{
					if (!force)
					{
						this.minMaxIntervalsSkipped++;
					}
				}
				else if (this.LastMinimumValue == -1)
				{
					this.dirty = true;
				}
				if (this.minMaxIntervalsSkipped > this.minMaxIntervalsToSkip && this.dirty)
				{
					if (this.LastMinimumValue == -1)
					{
						this.LastMinimumValue = this.LastValue;
						if (this.minMaxMilliseconds)
						{
							this.LastMinMillisecondsValue = 1000f / (float)this.LastMinimumValue;
						}
					}
					else if (this.LastValue < this.LastMinimumValue)
					{
						this.LastMinimumValue = this.LastValue;
						if (this.minMaxMilliseconds)
						{
							this.LastMinMillisecondsValue = 1000f / (float)this.LastMinimumValue;
						}
					}
					if (this.LastMaximumValue == -1)
					{
						this.LastMaximumValue = this.LastValue;
						if (this.minMaxMilliseconds)
						{
							this.LastMaxMillisecondsValue = 1000f / (float)this.LastMaximumValue;
						}
					}
					else if (this.LastValue > this.LastMaximumValue)
					{
						this.LastMaximumValue = this.LastValue;
						if (this.minMaxMilliseconds)
						{
							this.LastMaxMillisecondsValue = 1000f / (float)this.LastMaximumValue;
						}
					}
				}
			}
			if (this.dirty && this.main.OperationMode == OperationMode.Normal)
			{
				this.text.Length = 0;
				if (this.realtimeFPS)
				{
					string colorCached;
					if (this.LastValue >= this.warningLevelValue)
					{
						colorCached = this.colorCached;
					}
					else if (this.LastValue <= this.criticalLevelValue)
					{
						colorCached = this.colorCriticalCached;
					}
					else
					{
						colorCached = this.colorWarningCached;
					}
					this.text.Append(colorCached).AppendLookup(this.LastValue).Append("</color>");
					if (this.milliseconds)
					{
						if (this.LastValue >= this.warningLevelValue)
						{
							colorCached = this.colorCachedMs;
						}
						else if (this.LastValue <= this.criticalLevelValue)
						{
							colorCached = this.colorCriticalCachedMs;
						}
						else
						{
							colorCached = this.colorWarningCachedMs;
						}
						this.text.Append(colorCached).AppendLookup(this.LastMillisecondsValue).Append(" MS]</color>");
					}
				}
				if (this.average)
				{
					if (this.realtimeFPS)
					{
						this.text.Append(this.averageNewLine ? '\n' : ' ');
					}
					string colorCached;
					if (num4 >= this.warningLevelValue)
					{
						colorCached = this.colorCachedAvg;
					}
					else if (num4 <= this.criticalLevelValue)
					{
						colorCached = this.colorCriticalCachedAvg;
					}
					else
					{
						colorCached = this.colorWarningCachedAvg;
					}
					this.text.Append(colorCached).AppendLookup(num4);
					if (this.averageMilliseconds)
					{
						this.text.Append(" [").AppendLookup(this.LastAverageMillisecondsValue).Append(" MS]");
					}
					this.text.Append("</color>");
				}
				if (this.minMax)
				{
					if (this.realtimeFPS || this.average)
					{
						this.text.Append(this.minMaxNewLine ? '\n' : ' ');
					}
					string colorCached;
					if (this.LastMinimumValue >= this.warningLevelValue)
					{
						colorCached = this.colorCachedMin;
					}
					else if (this.LastMinimumValue <= this.criticalLevelValue)
					{
						colorCached = this.colorCriticalCachedMin;
					}
					else
					{
						colorCached = this.colorWarningCachedMin;
					}
					this.text.Append(colorCached).AppendLookup(this.LastMinimumValue);
					if (this.minMaxMilliseconds)
					{
						this.text.Append(" [").AppendLookup(this.LastMinMillisecondsValue).Append(" MS]");
					}
					this.text.Append("</color>");
					this.text.Append(this.minMaxTwoLines ? '\n' : ' ');
					if (this.LastMaximumValue >= this.warningLevelValue)
					{
						colorCached = this.colorCachedMax;
					}
					else if (this.LastMaximumValue <= this.criticalLevelValue)
					{
						colorCached = this.colorCriticalCachedMax;
					}
					else
					{
						colorCached = this.colorWarningCachedMax;
					}
					this.text.Append(colorCached).AppendLookup(this.LastMaximumValue);
					if (this.minMaxMilliseconds)
					{
						this.text.Append(" [").AppendLookup(this.LastMaxMillisecondsValue).Append(" MS]");
					}
					this.text.Append("</color>");
				}
				if (this.render)
				{
					if (this.realtimeFPS || this.average || this.minMax)
					{
						this.text.Append(this.renderNewLine ? '\n' : ' ');
					}
					this.text.Append(this.colorCachedRender).AppendLookup(this.LastRenderValue).Append(" MS").Append("</color>");
				}
				base.ApplyTextStyles();
			}
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x00036344 File Offset: 0x00034544
		protected override void PerformActivationActions()
		{
			base.PerformActivationActions();
			this.LastValue = 0;
			this.LastMinimumValue = -1;
			if (this.render)
			{
				this.previousFrameCount = Time.frameCount;
				if (this.renderAutoAdd)
				{
					FPSCounterData.TryToAddRenderRecorder();
				}
			}
			if (this.main.OperationMode == OperationMode.Normal)
			{
				if (this.colorWarningCached == null)
				{
					this.CacheWarningColor();
				}
				if (this.colorCriticalCached == null)
				{
					this.CacheCriticalColor();
				}
				this.text.Append(this.colorCriticalCached).Append("0").Append("</color>");
				base.ApplyTextStyles();
				this.dirty = true;
			}
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x000363E2 File Offset: 0x000345E2
		protected override void PerformDeActivationActions()
		{
			base.PerformDeActivationActions();
			this.ResetMinMax(true);
			this.ResetAverage();
			this.LastValue = 0;
			this.CurrentFpsLevel = FPSLevel.Normal;
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x00036405 File Offset: 0x00034605
		protected override IEnumerator UpdateCounter()
		{
			for (;;)
			{
				float previousUpdateTime = Time.unscaledTime;
				int previousUpdateFrames = Time.frameCount;
				while (Time.unscaledTime < previousUpdateTime + this.updateInterval)
				{
					yield return null;
				}
				float num = Time.unscaledTime - previousUpdateTime;
				int num2 = Time.frameCount - previousUpdateFrames;
				this.newValue = (float)num2 / num;
				this.UpdateValue(false);
				this.main.UpdateTexts();
			}
			yield break;
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x00036414 File Offset: 0x00034614
		protected override bool HasData()
		{
			return true;
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x00036418 File Offset: 0x00034618
		protected override void CacheCurrentColor()
		{
			string arg = AFPSCounter.Color32ToHex(this.color);
			this.colorCached = string.Format("<color=#{0}>FPS: ", arg);
			this.colorCachedMs = string.Format(" <color=#{0}>[", arg);
			this.colorCachedMin = string.Format("<color=#{0}>MIN: ", arg);
			this.colorCachedMax = string.Format("<color=#{0}>MAX: ", arg);
			this.colorCachedAvg = string.Format("<color=#{0}>AVG: ", arg);
			string arg2 = AFPSCounter.Color32ToHex(this.colorRender);
			this.colorCachedRender = string.Format("<color=#{0}>REN: ", arg2);
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x000364B0 File Offset: 0x000346B0
		protected void CacheWarningColor()
		{
			string arg = AFPSCounter.Color32ToHex(this.colorWarning);
			this.colorWarningCached = string.Format("<color=#{0}>FPS: ", arg);
			this.colorWarningCachedMs = string.Format(" <color=#{0}>[", arg);
			this.colorWarningCachedMin = string.Format("<color=#{0}>MIN: ", arg);
			this.colorWarningCachedMax = string.Format("<color=#{0}>MAX: ", arg);
			this.colorWarningCachedAvg = string.Format("<color=#{0}>AVG: ", arg);
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x00036524 File Offset: 0x00034724
		protected void CacheCriticalColor()
		{
			string arg = AFPSCounter.Color32ToHex(this.colorCritical);
			this.colorCriticalCached = string.Format("<color=#{0}>FPS: ", arg);
			this.colorCriticalCachedMs = string.Format(" <color=#{0}>[", arg);
			this.colorCriticalCachedMin = string.Format("<color=#{0}>MIN: ", arg);
			this.colorCriticalCachedMax = string.Format("<color=#{0}>MAX: ", arg);
			this.colorCriticalCachedAvg = string.Format("<color=#{0}>AVG: ", arg);
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x00036598 File Offset: 0x00034798
		private float GetAverageFromAccumulatedSamples()
		{
			float num = 0f;
			for (int i = 0; i < this.averageSamples; i++)
			{
				num += this.accumulatedAverageSamples[i];
			}
			float result;
			if (this.currentAverageSamples < this.averageSamples)
			{
				result = num / (float)this.currentAverageSamples;
			}
			else
			{
				result = num / (float)this.averageSamples;
			}
			return result;
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x000365EC File Offset: 0x000347EC
		private static void TryToAddRenderRecorder()
		{
			Camera main = Camera.main;
			if (main == null)
			{
				return;
			}
			if (main.GetComponent<AFPSRenderRecorder>() == null)
			{
				main.gameObject.AddComponent<AFPSRenderRecorder>();
			}
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x00036624 File Offset: 0x00034824
		private static void TryToRemoveRenderRecorder()
		{
			Camera main = Camera.main;
			if (main == null)
			{
				return;
			}
			AFPSRenderRecorder component = main.GetComponent<AFPSRenderRecorder>();
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
		}

		// Token: 0x04000BAF RID: 2991
		private const string ColorTextStart = "<color=#{0}>";

		// Token: 0x04000BB0 RID: 2992
		private const string ColorTextEnd = "</color>";

		// Token: 0x04000BB1 RID: 2993
		private const string FPSTextStart = "<color=#{0}>FPS: ";

		// Token: 0x04000BB2 RID: 2994
		private const string MsTextStart = " <color=#{0}>[";

		// Token: 0x04000BB3 RID: 2995
		private const string MsTextEnd = " MS]</color>";

		// Token: 0x04000BB4 RID: 2996
		private const string MinTextStart = "<color=#{0}>MIN: ";

		// Token: 0x04000BB5 RID: 2997
		private const string MaxTextStart = "<color=#{0}>MAX: ";

		// Token: 0x04000BB6 RID: 2998
		private const string AvgTextStart = "<color=#{0}>AVG: ";

		// Token: 0x04000BB7 RID: 2999
		private const string RenderTextStart = "<color=#{0}>REN: ";

		// Token: 0x04000BB8 RID: 3000
		public int warningLevelValue = 50;

		// Token: 0x04000BB9 RID: 3001
		public int criticalLevelValue = 20;

		// Token: 0x04000BBA RID: 3002
		[Tooltip("Average FPS counter accumulative data will be reset on new scene load if enabled.")]
		public bool resetAverageOnNewScene;

		// Token: 0x04000BBB RID: 3003
		[Tooltip("Minimum and maximum FPS readouts will be reset on new scene load if enabled")]
		public bool resetMinMaxOnNewScene;

		// Token: 0x04000BBC RID: 3004
		[Tooltip("Amount of update intervals to skip before recording minimum and maximum FPS.\nUse it to skip initialization performance spikes and drops.")]
		[Range(0f, 10f)]
		public int minMaxIntervalsToSkip = 3;

		// Token: 0x04000BBE RID: 3006
		internal float newValue;

		// Token: 0x04000BBF RID: 3007
		private string colorCachedMs;

		// Token: 0x04000BC0 RID: 3008
		private string colorCachedMin;

		// Token: 0x04000BC1 RID: 3009
		private string colorCachedMax;

		// Token: 0x04000BC2 RID: 3010
		private string colorCachedAvg;

		// Token: 0x04000BC3 RID: 3011
		private string colorCachedRender;

		// Token: 0x04000BC4 RID: 3012
		private string colorWarningCached;

		// Token: 0x04000BC5 RID: 3013
		private string colorWarningCachedMs;

		// Token: 0x04000BC6 RID: 3014
		private string colorWarningCachedMin;

		// Token: 0x04000BC7 RID: 3015
		private string colorWarningCachedMax;

		// Token: 0x04000BC8 RID: 3016
		private string colorWarningCachedAvg;

		// Token: 0x04000BC9 RID: 3017
		private string colorCriticalCached;

		// Token: 0x04000BCA RID: 3018
		private string colorCriticalCachedMs;

		// Token: 0x04000BCB RID: 3019
		private string colorCriticalCachedMin;

		// Token: 0x04000BCC RID: 3020
		private string colorCriticalCachedMax;

		// Token: 0x04000BCD RID: 3021
		private string colorCriticalCachedAvg;

		// Token: 0x04000BCE RID: 3022
		private int currentAverageSamples;

		// Token: 0x04000BCF RID: 3023
		private float currentAverageRaw;

		// Token: 0x04000BD0 RID: 3024
		private float[] accumulatedAverageSamples;

		// Token: 0x04000BD1 RID: 3025
		private int minMaxIntervalsSkipped;

		// Token: 0x04000BD2 RID: 3026
		private float renderTimeBank;

		// Token: 0x04000BD3 RID: 3027
		private int previousFrameCount;

		// Token: 0x04000BD4 RID: 3028
		[Tooltip("Shows realtime FPS at the moment of the counter update scene start. Allows to hide FPS readout if necessary.")]
		[SerializeField]
		private bool realtimeFPS;

		// Token: 0x04000BD5 RID: 3029
		[Tooltip("Shows time in milliseconds spent to render 1 frame.")]
		[SerializeField]
		private bool milliseconds;

		// Token: 0x04000BD6 RID: 3030
		[Tooltip("Shows Average FPS calculated from specified Samples amount or since game / scene start, depending on Samples value and 'Reset On Load' toggle.")]
		[SerializeField]
		private bool average;

		// Token: 0x04000BD7 RID: 3031
		[Tooltip("Shows time in milliseconds for the average FPS.")]
		[SerializeField]
		private bool averageMilliseconds;

		// Token: 0x04000BD8 RID: 3032
		[Tooltip("Controls placing Average on the new line.")]
		[SerializeField]
		private bool averageNewLine;

		// Token: 0x04000BD9 RID: 3033
		[Tooltip("Amount of last samples to get average from. Set 0 to get average from all samples since startup or level load.\nOne Sample recorded per one Interval.")]
		[Range(0f, 100f)]
		[SerializeField]
		private int averageSamples = 50;

		// Token: 0x04000BDA RID: 3034
		[Tooltip("Shows minimum and maximum FPS readouts since game / scene start, depending on 'Reset On Load' toggle.")]
		[SerializeField]
		private bool minMax;

		// Token: 0x04000BDB RID: 3035
		[Tooltip("Shows time in milliseconds for the Min Max FPS.")]
		[SerializeField]
		private bool minMaxMilliseconds;

		// Token: 0x04000BDC RID: 3036
		[Tooltip("Controls placing Min Max on the new line.")]
		[SerializeField]
		private bool minMaxNewLine;

		// Token: 0x04000BDD RID: 3037
		[Tooltip("Check to place Min Max on two separate lines. Otherwise they will be placed on a single line.")]
		[SerializeField]
		private bool minMaxTwoLines;

		// Token: 0x04000BDE RID: 3038
		[Tooltip("Shows time spent on Camera.Render excluding Image Effects. Add AFPSRenderRecorder to the cameras you wish to count.")]
		[SerializeField]
		private bool render;

		// Token: 0x04000BDF RID: 3039
		[Tooltip("Controls placing Render on the new line.")]
		[SerializeField]
		private bool renderNewLine;

		// Token: 0x04000BE0 RID: 3040
		[Tooltip("Check to automatically add AFPSRenderRecorder to the Main Camera if present.")]
		[SerializeField]
		private bool renderAutoAdd = true;

		// Token: 0x04000BE1 RID: 3041
		[Tooltip("Color of the FPS counter while FPS is between Critical and Warning levels.")]
		[SerializeField]
		private Color colorWarning = new Color32(236, 224, 88, byte.MaxValue);

		// Token: 0x04000BE2 RID: 3042
		[Tooltip("Color of the FPS counter while FPS is below Critical level.")]
		[SerializeField]
		private Color colorCritical = new Color32(249, 91, 91, byte.MaxValue);

		// Token: 0x04000BE3 RID: 3043
		[Tooltip("Color of the Render Time output.")]
		[SerializeField]
		protected Color colorRender;
	}
}
