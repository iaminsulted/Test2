using System;
using System.Text;
using CodeStage.AdvancedFPSCounter.Labels;
using UnityEngine;

namespace CodeStage.AdvancedFPSCounter.CountersData
{
	// Token: 0x020001FE RID: 510
	[AddComponentMenu("")]
	[Serializable]
	public class DeviceInfoCounterData : BaseCounterData
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06001043 RID: 4163 RVA: 0x00034BF9 File Offset: 0x00032DF9
		// (set) Token: 0x06001044 RID: 4164 RVA: 0x00034C01 File Offset: 0x00032E01
		public bool Platform
		{
			get
			{
				return this.platform;
			}
			set
			{
				if (this.platform == value || !Application.isPlaying)
				{
					return;
				}
				this.platform = value;
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06001045 RID: 4165 RVA: 0x00034C2A File Offset: 0x00032E2A
		// (set) Token: 0x06001046 RID: 4166 RVA: 0x00034C32 File Offset: 0x00032E32
		public bool CpuModel
		{
			get
			{
				return this.cpuModel;
			}
			set
			{
				if (this.cpuModel == value || !Application.isPlaying)
				{
					return;
				}
				this.cpuModel = value;
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06001047 RID: 4167 RVA: 0x00034C5B File Offset: 0x00032E5B
		// (set) Token: 0x06001048 RID: 4168 RVA: 0x00034C63 File Offset: 0x00032E63
		public bool CpuModelNewLine
		{
			get
			{
				return this.cpuModelNewLine;
			}
			set
			{
				if (this.cpuModelNewLine == value || !Application.isPlaying)
				{
					return;
				}
				this.cpuModelNewLine = value;
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06001049 RID: 4169 RVA: 0x00034C8C File Offset: 0x00032E8C
		// (set) Token: 0x0600104A RID: 4170 RVA: 0x00034C94 File Offset: 0x00032E94
		public bool GpuModel
		{
			get
			{
				return this.gpuModel;
			}
			set
			{
				if (this.gpuModel == value || !Application.isPlaying)
				{
					return;
				}
				this.gpuModel = value;
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600104B RID: 4171 RVA: 0x00034CBD File Offset: 0x00032EBD
		// (set) Token: 0x0600104C RID: 4172 RVA: 0x00034CC5 File Offset: 0x00032EC5
		public bool GpuModelNewLine
		{
			get
			{
				return this.gpuModelNewLine;
			}
			set
			{
				if (this.gpuModelNewLine == value || !Application.isPlaying)
				{
					return;
				}
				this.gpuModelNewLine = value;
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600104D RID: 4173 RVA: 0x00034CEE File Offset: 0x00032EEE
		// (set) Token: 0x0600104E RID: 4174 RVA: 0x00034CF6 File Offset: 0x00032EF6
		public bool GpuApi
		{
			get
			{
				return this.gpuApi;
			}
			set
			{
				if (this.gpuApi == value || !Application.isPlaying)
				{
					return;
				}
				this.gpuApi = value;
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600104F RID: 4175 RVA: 0x00034D1F File Offset: 0x00032F1F
		// (set) Token: 0x06001050 RID: 4176 RVA: 0x00034D27 File Offset: 0x00032F27
		public bool GpuApiNewLine
		{
			get
			{
				return this.gpuApiNewLine;
			}
			set
			{
				if (this.gpuApiNewLine == value || !Application.isPlaying)
				{
					return;
				}
				this.gpuApiNewLine = value;
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06001051 RID: 4177 RVA: 0x00034D50 File Offset: 0x00032F50
		// (set) Token: 0x06001052 RID: 4178 RVA: 0x00034D58 File Offset: 0x00032F58
		public bool GpuSpec
		{
			get
			{
				return this.gpuSpec;
			}
			set
			{
				if (this.gpuSpec == value || !Application.isPlaying)
				{
					return;
				}
				this.gpuSpec = value;
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06001053 RID: 4179 RVA: 0x00034D81 File Offset: 0x00032F81
		// (set) Token: 0x06001054 RID: 4180 RVA: 0x00034D89 File Offset: 0x00032F89
		public bool GpuSpecNewLine
		{
			get
			{
				return this.gpuSpecNewLine;
			}
			set
			{
				if (this.gpuSpecNewLine == value || !Application.isPlaying)
				{
					return;
				}
				this.gpuSpecNewLine = value;
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06001055 RID: 4181 RVA: 0x00034DB2 File Offset: 0x00032FB2
		// (set) Token: 0x06001056 RID: 4182 RVA: 0x00034DBA File Offset: 0x00032FBA
		public bool RamSize
		{
			get
			{
				return this.ramSize;
			}
			set
			{
				if (this.ramSize == value || !Application.isPlaying)
				{
					return;
				}
				this.ramSize = value;
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06001057 RID: 4183 RVA: 0x00034DE3 File Offset: 0x00032FE3
		// (set) Token: 0x06001058 RID: 4184 RVA: 0x00034DEB File Offset: 0x00032FEB
		public bool RamSizeNewLine
		{
			get
			{
				return this.ramSizeNewLine;
			}
			set
			{
				if (this.ramSizeNewLine == value || !Application.isPlaying)
				{
					return;
				}
				this.ramSizeNewLine = value;
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06001059 RID: 4185 RVA: 0x00034E14 File Offset: 0x00033014
		// (set) Token: 0x0600105A RID: 4186 RVA: 0x00034E1C File Offset: 0x0003301C
		public bool ScreenData
		{
			get
			{
				return this.screenData;
			}
			set
			{
				if (this.screenData == value || !Application.isPlaying)
				{
					return;
				}
				this.screenData = value;
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600105B RID: 4187 RVA: 0x00034E45 File Offset: 0x00033045
		// (set) Token: 0x0600105C RID: 4188 RVA: 0x00034E4D File Offset: 0x0003304D
		public bool ScreenDataNewLine
		{
			get
			{
				return this.screenDataNewLine;
			}
			set
			{
				if (this.screenDataNewLine == value || !Application.isPlaying)
				{
					return;
				}
				this.screenDataNewLine = value;
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600105D RID: 4189 RVA: 0x00034E76 File Offset: 0x00033076
		// (set) Token: 0x0600105E RID: 4190 RVA: 0x00034E7E File Offset: 0x0003307E
		public bool DeviceModel
		{
			get
			{
				return this.deviceModel;
			}
			set
			{
				if (this.deviceModel == value || !Application.isPlaying)
				{
					return;
				}
				this.deviceModel = value;
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600105F RID: 4191 RVA: 0x00034EA7 File Offset: 0x000330A7
		// (set) Token: 0x06001060 RID: 4192 RVA: 0x00034EAF File Offset: 0x000330AF
		public bool DeviceModelNewLine
		{
			get
			{
				return this.deviceModelNewLine;
			}
			set
			{
				if (this.deviceModelNewLine == value || !Application.isPlaying)
				{
					return;
				}
				this.deviceModelNewLine = value;
				if (!this.enabled)
				{
					return;
				}
				base.Refresh();
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06001061 RID: 4193 RVA: 0x00034ED8 File Offset: 0x000330D8
		// (set) Token: 0x06001062 RID: 4194 RVA: 0x00034EE0 File Offset: 0x000330E0
		public string LastValue { get; private set; }

		// Token: 0x06001063 RID: 4195 RVA: 0x00034EEC File Offset: 0x000330EC
		internal DeviceInfoCounterData()
		{
			this.color = new Color32(172, 172, 172, byte.MaxValue);
			this.anchor = LabelAnchor.LowerLeft;
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x00034F8C File Offset: 0x0003318C
		internal override void UpdateValue(bool force)
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
			if (!this.enabled)
			{
				return;
			}
			bool flag = false;
			if (this.text == null)
			{
				this.text = new StringBuilder(500);
			}
			else
			{
				this.text.Length = 0;
			}
			if (this.platform)
			{
				this.text.Append("OS: ").Append(SystemInfo.operatingSystem).Append(" [").Append(Application.platform).Append("]");
				flag = true;
			}
			if (this.cpuModel)
			{
				if (flag)
				{
					this.text.Append(this.cpuModelNewLine ? '\n' : ' ');
				}
				this.text.Append("CPU: ").Append(SystemInfo.processorType).Append(" [").Append(SystemInfo.processorCount).Append(" cores]");
				flag = true;
			}
			if (this.gpuModel)
			{
				if (flag)
				{
					this.text.Append(this.gpuModelNewLine ? '\n' : ' ');
				}
				this.text.Append("GPU: ").Append(SystemInfo.graphicsDeviceName);
				flag = true;
			}
			if (this.gpuApi)
			{
				if (flag)
				{
					this.text.Append(this.gpuApiNewLine ? '\n' : ' ');
				}
				if (this.gpuApiNewLine || !this.gpuModel)
				{
					this.text.Append("GPU: ");
				}
				this.text.Append(SystemInfo.graphicsDeviceVersion);
				this.text.Append(" [").Append(SystemInfo.graphicsDeviceType).Append("]");
				flag = true;
			}
			if (this.gpuSpec)
			{
				if (flag)
				{
					this.text.Append(this.gpuSpecNewLine ? '\n' : ' ');
				}
				if (this.gpuSpecNewLine || (!this.gpuModel && !this.gpuApi))
				{
					this.text.Append("GPU: SM: ");
				}
				else
				{
					this.text.Append("SM: ");
				}
				int num = SystemInfo.graphicsShaderLevel;
				if (num >= 10 && num <= 99)
				{
					this.text.Append(num /= 10).Append('.').Append(num / 10);
				}
				else
				{
					this.text.Append("N/A");
				}
				this.text.Append(", VRAM: ");
				int graphicsMemorySize = SystemInfo.graphicsMemorySize;
				if (graphicsMemorySize > 0)
				{
					this.text.Append(graphicsMemorySize).Append(" MB");
				}
				else
				{
					this.text.Append("N/A");
				}
				flag = true;
			}
			if (this.ramSize)
			{
				if (flag)
				{
					this.text.Append(this.ramSizeNewLine ? '\n' : ' ');
				}
				int systemMemorySize = SystemInfo.systemMemorySize;
				if (systemMemorySize > 0)
				{
					this.text.Append("RAM: ").Append(systemMemorySize).Append(" MB");
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			if (this.screenData)
			{
				if (flag)
				{
					this.text.Append(this.screenDataNewLine ? '\n' : ' ');
				}
				Resolution currentResolution = Screen.currentResolution;
				this.text.Append("SCR: ").Append(currentResolution.width).Append("x").Append(currentResolution.height).Append("@").Append(currentResolution.refreshRate).Append("Hz [window size: ").Append(Screen.width).Append("x").Append(Screen.height);
				float dpi = Screen.dpi;
				if (dpi > 0f)
				{
					this.text.Append(", DPI: ").Append(dpi).Append("]");
				}
				else
				{
					this.text.Append("]");
				}
				flag = true;
			}
			if (this.deviceModel)
			{
				if (flag)
				{
					this.text.Append(this.deviceModelNewLine ? '\n' : ' ');
				}
				this.text.Append("Model: ").Append(SystemInfo.deviceModel);
			}
			this.LastValue = this.text.ToString();
			if (this.main.OperationMode == OperationMode.Normal)
			{
				this.text.Insert(0, this.colorCached);
				this.text.Append("</color>");
				base.ApplyTextStyles();
			}
			else
			{
				this.text.Length = 0;
			}
			this.dirty = true;
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x00035430 File Offset: 0x00033630
		protected override bool HasData()
		{
			return this.cpuModel || this.gpuModel || this.ramSize || this.screenData;
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x00035452 File Offset: 0x00033652
		protected override void CacheCurrentColor()
		{
			this.colorCached = "<color=#" + AFPSCounter.Color32ToHex(this.color) + ">";
		}

		// Token: 0x04000B9F RID: 2975
		[Tooltip("Shows operating system & platform info.")]
		[SerializeField]
		private bool platform = true;

		// Token: 0x04000BA0 RID: 2976
		[Tooltip("CPU model and cores (including virtual cores from Intel's Hyper Threading) count.")]
		[SerializeField]
		private bool cpuModel = true;

		// Token: 0x04000BA1 RID: 2977
		[Tooltip("Check to show CPU model on new line.")]
		[SerializeField]
		private bool cpuModelNewLine = true;

		// Token: 0x04000BA2 RID: 2978
		[Tooltip("Shows GPU model name.")]
		[SerializeField]
		private bool gpuModel = true;

		// Token: 0x04000BA3 RID: 2979
		[Tooltip("Check to show GPU model on new line.")]
		[SerializeField]
		private bool gpuModelNewLine = true;

		// Token: 0x04000BA4 RID: 2980
		[Tooltip("Shows graphics API version and type (if possible).")]
		[SerializeField]
		private bool gpuApi = true;

		// Token: 0x04000BA5 RID: 2981
		[Tooltip("Check to show graphics API version on new line.")]
		[SerializeField]
		private bool gpuApiNewLine = true;

		// Token: 0x04000BA6 RID: 2982
		[Tooltip("Shows graphics supported shader model (if possible), approximate pixel fill-rate (if possible) and total Video RAM size (if possible).")]
		[SerializeField]
		private bool gpuSpec = true;

		// Token: 0x04000BA7 RID: 2983
		[Tooltip("Check to show graphics specs on new line.")]
		[SerializeField]
		private bool gpuSpecNewLine = true;

		// Token: 0x04000BA8 RID: 2984
		[Tooltip("Shows total RAM size.")]
		[SerializeField]
		private bool ramSize = true;

		// Token: 0x04000BA9 RID: 2985
		[Tooltip("Check to show RAM size on new line.")]
		[SerializeField]
		private bool ramSizeNewLine = true;

		// Token: 0x04000BAA RID: 2986
		[Tooltip("Shows screen resolution, size and DPI (if possible).")]
		[SerializeField]
		private bool screenData = true;

		// Token: 0x04000BAB RID: 2987
		[Tooltip("Check to show screen data on new line.")]
		[SerializeField]
		private bool screenDataNewLine = true;

		// Token: 0x04000BAC RID: 2988
		[Tooltip("Shows device model. Actual for mobile devices.")]
		[SerializeField]
		private bool deviceModel;

		// Token: 0x04000BAD RID: 2989
		[Tooltip("Check to show device model on new line.")]
		[SerializeField]
		private bool deviceModelNewLine = true;
	}
}
