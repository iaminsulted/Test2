using System;
using System.Collections.Generic;
using CodeStage.AdvancedFPSCounter.CountersData;
using CodeStage.AdvancedFPSCounter.Labels;
using CodeStage.AdvancedFPSCounter.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CodeStage.AdvancedFPSCounter
{
	// Token: 0x020001F3 RID: 499
	[AddComponentMenu("Code Stage/\ud83d\ude80 Advanced FPS Counter")]
	[DisallowMultipleComponent]
	[HelpURL("http://codestage.net/uas_files/afps/api/class_code_stage_1_1_advanced_f_p_s_counter_1_1_a_f_p_s_counter.html")]
	public class AFPSCounter : MonoBehaviour
	{
		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000FB7 RID: 4023 RVA: 0x0002F9CB File Offset: 0x0002DBCB
		public bool KeepAlive
		{
			get
			{
				return this.keepAlive;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000FB8 RID: 4024 RVA: 0x0002F9D3 File Offset: 0x0002DBD3
		// (set) Token: 0x06000FB9 RID: 4025 RVA: 0x0002F9DC File Offset: 0x0002DBDC
		public OperationMode OperationMode
		{
			get
			{
				return this.operationMode;
			}
			set
			{
				if (this.operationMode == value || !Application.isPlaying)
				{
					return;
				}
				this.operationMode = value;
				if (this.operationMode != OperationMode.Disabled)
				{
					if (this.operationMode == OperationMode.Background)
					{
						for (int i = 0; i < this.anchorsCount; i++)
						{
							this.labels[i].Clear();
						}
					}
					this.OnEnable();
					this.fpsCounter.UpdateValue();
					this.memoryCounter.UpdateValue();
					this.deviceInfoCounter.UpdateValue();
					this.UpdateTexts();
					return;
				}
				this.OnDisable();
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000FBA RID: 4026 RVA: 0x0002FA64 File Offset: 0x0002DC64
		// (set) Token: 0x06000FBB RID: 4027 RVA: 0x0002FA6C File Offset: 0x0002DC6C
		public bool ForceFrameRate
		{
			get
			{
				return this.forceFrameRate;
			}
			set
			{
				if (this.forceFrameRate == value || !Application.isPlaying)
				{
					return;
				}
				this.forceFrameRate = value;
				if (this.operationMode == OperationMode.Disabled)
				{
					return;
				}
				this.RefreshForcedFrameRate();
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000FBC RID: 4028 RVA: 0x0002FA95 File Offset: 0x0002DC95
		// (set) Token: 0x06000FBD RID: 4029 RVA: 0x0002FA9D File Offset: 0x0002DC9D
		public int ForcedFrameRate
		{
			get
			{
				return this.forcedFrameRate;
			}
			set
			{
				if (this.forcedFrameRate == value || !Application.isPlaying)
				{
					return;
				}
				this.forcedFrameRate = value;
				if (this.operationMode == OperationMode.Disabled)
				{
					return;
				}
				this.RefreshForcedFrameRate();
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000FBE RID: 4030 RVA: 0x0002FAC6 File Offset: 0x0002DCC6
		// (set) Token: 0x06000FBF RID: 4031 RVA: 0x0002FAD0 File Offset: 0x0002DCD0
		public bool Background
		{
			get
			{
				return this.background;
			}
			set
			{
				if (this.background == value || !Application.isPlaying)
				{
					return;
				}
				this.background = value;
				if (this.operationMode == OperationMode.Disabled || this.labels == null)
				{
					return;
				}
				for (int i = 0; i < this.anchorsCount; i++)
				{
					this.labels[i].ChangeBackground(this.background);
				}
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x0002FB2A File Offset: 0x0002DD2A
		// (set) Token: 0x06000FC1 RID: 4033 RVA: 0x0002FB34 File Offset: 0x0002DD34
		public Color BackgroundColor
		{
			get
			{
				return this.backgroundColor;
			}
			set
			{
				if (this.backgroundColor == value || !Application.isPlaying)
				{
					return;
				}
				this.backgroundColor = value;
				if (this.operationMode == OperationMode.Disabled || this.labels == null)
				{
					return;
				}
				for (int i = 0; i < this.anchorsCount; i++)
				{
					this.labels[i].ChangeBackgroundColor(this.backgroundColor);
				}
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x0002FB93 File Offset: 0x0002DD93
		// (set) Token: 0x06000FC3 RID: 4035 RVA: 0x0002FB9C File Offset: 0x0002DD9C
		public int BackgroundPadding
		{
			get
			{
				return this.backgroundPadding;
			}
			set
			{
				if (this.backgroundPadding == value || !Application.isPlaying)
				{
					return;
				}
				this.backgroundPadding = value;
				if (this.operationMode == OperationMode.Disabled || this.labels == null)
				{
					return;
				}
				for (int i = 0; i < this.anchorsCount; i++)
				{
					this.labels[i].ChangeBackgroundPadding(this.backgroundPadding);
				}
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x0002FBF6 File Offset: 0x0002DDF6
		// (set) Token: 0x06000FC5 RID: 4037 RVA: 0x0002FC00 File Offset: 0x0002DE00
		public bool Shadow
		{
			get
			{
				return this.shadow;
			}
			set
			{
				if (this.shadow == value || !Application.isPlaying)
				{
					return;
				}
				this.shadow = value;
				if (this.operationMode == OperationMode.Disabled || this.labels == null)
				{
					return;
				}
				for (int i = 0; i < this.anchorsCount; i++)
				{
					this.labels[i].ChangeShadow(this.shadow);
				}
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x0002FC5A File Offset: 0x0002DE5A
		// (set) Token: 0x06000FC7 RID: 4039 RVA: 0x0002FC64 File Offset: 0x0002DE64
		public Color ShadowColor
		{
			get
			{
				return this.shadowColor;
			}
			set
			{
				if (this.shadowColor == value || !Application.isPlaying)
				{
					return;
				}
				this.shadowColor = value;
				if (this.operationMode == OperationMode.Disabled || this.labels == null)
				{
					return;
				}
				for (int i = 0; i < this.anchorsCount; i++)
				{
					this.labels[i].ChangeShadowColor(this.shadowColor);
				}
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x0002FCC3 File Offset: 0x0002DEC3
		// (set) Token: 0x06000FC9 RID: 4041 RVA: 0x0002FCCC File Offset: 0x0002DECC
		public Vector2 ShadowDistance
		{
			get
			{
				return this.shadowDistance;
			}
			set
			{
				if (this.shadowDistance == value || !Application.isPlaying)
				{
					return;
				}
				this.shadowDistance = value;
				if (this.operationMode == OperationMode.Disabled || this.labels == null)
				{
					return;
				}
				for (int i = 0; i < this.anchorsCount; i++)
				{
					this.labels[i].ChangeShadowDistance(this.shadowDistance);
				}
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000FCA RID: 4042 RVA: 0x0002FD2B File Offset: 0x0002DF2B
		// (set) Token: 0x06000FCB RID: 4043 RVA: 0x0002FD34 File Offset: 0x0002DF34
		public bool Outline
		{
			get
			{
				return this.outline;
			}
			set
			{
				if (this.outline == value || !Application.isPlaying)
				{
					return;
				}
				this.outline = value;
				if (this.operationMode == OperationMode.Disabled || this.labels == null)
				{
					return;
				}
				for (int i = 0; i < this.anchorsCount; i++)
				{
					this.labels[i].ChangeOutline(this.outline);
				}
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000FCC RID: 4044 RVA: 0x0002FD8E File Offset: 0x0002DF8E
		// (set) Token: 0x06000FCD RID: 4045 RVA: 0x0002FD98 File Offset: 0x0002DF98
		public Color OutlineColor
		{
			get
			{
				return this.outlineColor;
			}
			set
			{
				if (this.outlineColor == value || !Application.isPlaying)
				{
					return;
				}
				this.outlineColor = value;
				if (this.operationMode == OperationMode.Disabled || this.labels == null)
				{
					return;
				}
				for (int i = 0; i < this.anchorsCount; i++)
				{
					this.labels[i].ChangeOutlineColor(this.outlineColor);
				}
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000FCE RID: 4046 RVA: 0x0002FDF7 File Offset: 0x0002DFF7
		// (set) Token: 0x06000FCF RID: 4047 RVA: 0x0002FE00 File Offset: 0x0002E000
		public Vector2 OutlineDistance
		{
			get
			{
				return this.outlineDistance;
			}
			set
			{
				if (this.outlineDistance == value || !Application.isPlaying)
				{
					return;
				}
				this.outlineDistance = value;
				if (this.operationMode == OperationMode.Disabled || this.labels == null)
				{
					return;
				}
				for (int i = 0; i < this.anchorsCount; i++)
				{
					this.labels[i].ChangeOutlineDistance(this.outlineDistance);
				}
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x0002FE5F File Offset: 0x0002E05F
		// (set) Token: 0x06000FD1 RID: 4049 RVA: 0x0002FE68 File Offset: 0x0002E068
		public bool AutoScale
		{
			get
			{
				return this.autoScale;
			}
			set
			{
				if (this.autoScale == value || !Application.isPlaying)
				{
					return;
				}
				this.autoScale = value;
				if (this.operationMode == OperationMode.Disabled || this.labels == null)
				{
					return;
				}
				if (this.canvasScaler == null)
				{
					return;
				}
				this.canvasScaler.uiScaleMode = (this.autoScale ? CanvasScaler.ScaleMode.ScaleWithScreenSize : CanvasScaler.ScaleMode.ConstantPixelSize);
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000FD2 RID: 4050 RVA: 0x0002FEC4 File Offset: 0x0002E0C4
		// (set) Token: 0x06000FD3 RID: 4051 RVA: 0x0002FECC File Offset: 0x0002E0CC
		public float ScaleFactor
		{
			get
			{
				return this.scaleFactor;
			}
			set
			{
				if (Math.Abs(this.scaleFactor - value) < 0.001f || !Application.isPlaying)
				{
					return;
				}
				this.scaleFactor = value;
				if (this.operationMode == OperationMode.Disabled || this.canvasScaler == null)
				{
					return;
				}
				this.canvasScaler.scaleFactor = this.scaleFactor;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000FD4 RID: 4052 RVA: 0x0002FF24 File Offset: 0x0002E124
		// (set) Token: 0x06000FD5 RID: 4053 RVA: 0x0002FF2C File Offset: 0x0002E12C
		public Font LabelsFont
		{
			get
			{
				return this.labelsFont;
			}
			set
			{
				if (this.labelsFont == value || !Application.isPlaying)
				{
					return;
				}
				this.labelsFont = value;
				if (this.operationMode == OperationMode.Disabled || this.labels == null)
				{
					return;
				}
				for (int i = 0; i < this.anchorsCount; i++)
				{
					this.labels[i].ChangeFont(this.labelsFont);
				}
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000FD6 RID: 4054 RVA: 0x0002FF8B File Offset: 0x0002E18B
		// (set) Token: 0x06000FD7 RID: 4055 RVA: 0x0002FF94 File Offset: 0x0002E194
		public int FontSize
		{
			get
			{
				return this.fontSize;
			}
			set
			{
				if (this.fontSize == value || !Application.isPlaying)
				{
					return;
				}
				this.fontSize = value;
				if (this.operationMode == OperationMode.Disabled || this.labels == null)
				{
					return;
				}
				for (int i = 0; i < this.anchorsCount; i++)
				{
					this.labels[i].ChangeFontSize(this.fontSize);
				}
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000FD8 RID: 4056 RVA: 0x0002FFEE File Offset: 0x0002E1EE
		// (set) Token: 0x06000FD9 RID: 4057 RVA: 0x0002FFF8 File Offset: 0x0002E1F8
		public float LineSpacing
		{
			get
			{
				return this.lineSpacing;
			}
			set
			{
				if (Math.Abs(this.lineSpacing - value) < 0.001f || !Application.isPlaying)
				{
					return;
				}
				this.lineSpacing = value;
				if (this.operationMode == OperationMode.Disabled || this.labels == null)
				{
					return;
				}
				for (int i = 0; i < this.anchorsCount; i++)
				{
					this.labels[i].ChangeLineSpacing(this.lineSpacing);
				}
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000FDA RID: 4058 RVA: 0x0003005D File Offset: 0x0002E25D
		// (set) Token: 0x06000FDB RID: 4059 RVA: 0x00030068 File Offset: 0x0002E268
		public int CountersSpacing
		{
			get
			{
				return this.countersSpacing;
			}
			set
			{
				if (this.countersSpacing == value || !Application.isPlaying)
				{
					return;
				}
				this.countersSpacing = value;
				if (this.operationMode == OperationMode.Disabled || this.labels == null)
				{
					return;
				}
				this.UpdateTexts();
				for (int i = 0; i < this.anchorsCount; i++)
				{
					this.labels[i].dirty = true;
				}
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000FDC RID: 4060 RVA: 0x000300C3 File Offset: 0x0002E2C3
		// (set) Token: 0x06000FDD RID: 4061 RVA: 0x000300CC File Offset: 0x0002E2CC
		public Vector2 PaddingOffset
		{
			get
			{
				return this.paddingOffset;
			}
			set
			{
				if (this.paddingOffset == value || !Application.isPlaying)
				{
					return;
				}
				this.paddingOffset = value;
				if (this.operationMode == OperationMode.Disabled || this.labels == null)
				{
					return;
				}
				for (int i = 0; i < this.anchorsCount; i++)
				{
					this.labels[i].ChangeOffset(this.paddingOffset);
				}
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000FDE RID: 4062 RVA: 0x0003012B File Offset: 0x0002E32B
		// (set) Token: 0x06000FDF RID: 4063 RVA: 0x00030133 File Offset: 0x0002E333
		public bool PixelPerfect
		{
			get
			{
				return this.pixelPerfect;
			}
			set
			{
				if (this.pixelPerfect == value || !Application.isPlaying)
				{
					return;
				}
				this.pixelPerfect = value;
				if (this.operationMode == OperationMode.Disabled || this.labels == null)
				{
					return;
				}
				this.canvas.pixelPerfect = this.pixelPerfect;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x0003016F File Offset: 0x0002E36F
		// (set) Token: 0x06000FE1 RID: 4065 RVA: 0x00030178 File Offset: 0x0002E378
		public int SortingOrder
		{
			get
			{
				return this.sortingOrder;
			}
			set
			{
				if (this.sortingOrder == value || !Application.isPlaying)
				{
					return;
				}
				this.sortingOrder = value;
				if (this.operationMode == OperationMode.Disabled || this.canvas == null)
				{
					return;
				}
				this.canvas.sortingOrder = this.sortingOrder;
			}
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x000301C8 File Offset: 0x0002E3C8
		private AFPSCounter()
		{
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000FE3 RID: 4067 RVA: 0x000302F7 File Offset: 0x0002E4F7
		// (set) Token: 0x06000FE4 RID: 4068 RVA: 0x000302FE File Offset: 0x0002E4FE
		public static AFPSCounter Instance { get; private set; }

		// Token: 0x06000FE5 RID: 4069 RVA: 0x00030308 File Offset: 0x0002E508
		private static AFPSCounter GetOrCreateInstance(bool keepAlive)
		{
			if (AFPSCounter.Instance != null)
			{
				return AFPSCounter.Instance;
			}
			AFPSCounter afpscounter = UnityEngine.Object.FindObjectOfType<AFPSCounter>();
			if (afpscounter != null)
			{
				AFPSCounter.Instance = afpscounter;
			}
			else
			{
				AFPSCounter.CreateInScene(false).keepAlive = keepAlive;
			}
			return AFPSCounter.Instance;
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x00030350 File Offset: 0x0002E550
		public static AFPSCounter AddToScene()
		{
			return AFPSCounter.AddToScene(true);
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x00030358 File Offset: 0x0002E558
		public static AFPSCounter AddToScene(bool keepAlive)
		{
			return AFPSCounter.GetOrCreateInstance(keepAlive);
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x00030360 File Offset: 0x0002E560
		[Obsolete("Please use SelfDestroy() instead. This method will be removed in future updates.")]
		public static void Dispose()
		{
			AFPSCounter.SelfDestroy();
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x00030367 File Offset: 0x0002E567
		public static void SelfDestroy()
		{
			if (AFPSCounter.Instance != null)
			{
				AFPSCounter.Instance.DisposeInternal();
			}
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x00030380 File Offset: 0x0002E580
		internal static string Color32ToHex(Color32 color)
		{
			return color.r.ToString("x2") + color.g.ToString("x2") + color.b.ToString("x2") + color.a.ToString("x2");
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x000303D8 File Offset: 0x0002E5D8
		private static AFPSCounter CreateInScene(bool lookForExistingContainer = true)
		{
			GameObject gameObject = lookForExistingContainer ? GameObject.Find("Advanced FPS Counter") : null;
			if (gameObject == null)
			{
				gameObject = new GameObject("Advanced FPS Counter")
				{
					layer = LayerMask.NameToLayer("UI")
				};
			}
			return gameObject.AddComponent<AFPSCounter>();
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x00030420 File Offset: 0x0002E620
		private void Awake()
		{
			if (AFPSCounter.Instance != null && AFPSCounter.Instance.keepAlive)
			{
				UnityEngine.Object.Destroy(this);
				return;
			}
			AFPSCounter.Instance = this;
			this.fpsCounter.Init(this);
			this.memoryCounter.Init(this);
			this.deviceInfoCounter.Init(this);
			this.ConfigureCanvas();
			this.ConfigureLabels();
			this.inited = true;
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0003048A File Offset: 0x0002E68A
		private void Start()
		{
			if (this.keepAlive)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.transform.root.gameObject);
				SceneManager.sceneLoaded += this.OnLevelWasLoadedNew;
			}
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x000304BA File Offset: 0x0002E6BA
		private void Update()
		{
			if (!this.inited)
			{
				return;
			}
			this.ProcessHotKey();
			if (this.circleGesture && this.CircleGestureMade())
			{
				this.SwitchCounter();
			}
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x000304E1 File Offset: 0x0002E6E1
		private void OnLevelWasLoadedNew(Scene scene, LoadSceneMode mode)
		{
			this.OnLevelLoadedCallback();
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x000304E9 File Offset: 0x0002E6E9
		private void OnLevelLoadedCallback()
		{
			if (!this.inited)
			{
				return;
			}
			if (!this.fpsCounter.Enabled)
			{
				return;
			}
			this.fpsCounter.OnLevelLoadedCallback();
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0003050D File Offset: 0x0002E70D
		private void OnEnable()
		{
			if (!this.inited)
			{
				return;
			}
			if (this.operationMode == OperationMode.Disabled)
			{
				return;
			}
			this.ActivateCounters();
			base.Invoke("RefreshForcedFrameRate", 0.5f);
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x00030538 File Offset: 0x0002E738
		private void OnDisable()
		{
			if (!this.inited)
			{
				return;
			}
			this.DeactivateCounters();
			if (base.IsInvoking("RefreshForcedFrameRate"))
			{
				base.CancelInvoke("RefreshForcedFrameRate");
			}
			this.RefreshForcedFrameRate(true);
			for (int i = 0; i < this.anchorsCount; i++)
			{
				this.labels[i].Clear();
			}
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x00030594 File Offset: 0x0002E794
		private void OnDestroy()
		{
			if (this.inited)
			{
				this.fpsCounter.Destroy();
				this.memoryCounter.Destroy();
				this.deviceInfoCounter.Destroy();
				if (this.labels != null)
				{
					for (int i = 0; i < this.anchorsCount; i++)
					{
						this.labels[i].Destroy();
					}
					Array.Clear(this.labels, 0, this.anchorsCount);
					this.labels = null;
				}
				this.inited = false;
			}
			if (this.canvas != null)
			{
				UnityEngine.Object.Destroy(this.canvas.gameObject);
			}
			if (base.transform.childCount <= 1)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			if (AFPSCounter.Instance == this)
			{
				AFPSCounter.Instance = null;
			}
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0003065A File Offset: 0x0002E85A
		internal void MakeDrawableLabelDirty(LabelAnchor anchor)
		{
			if (this.operationMode == OperationMode.Normal)
			{
				this.labels[(int)anchor].dirty = true;
			}
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x00030674 File Offset: 0x0002E874
		internal void UpdateTexts()
		{
			if (this.operationMode != OperationMode.Normal)
			{
				return;
			}
			bool flag = false;
			if (this.fpsCounter.Enabled)
			{
				DrawableLabel drawableLabel = this.labels[(int)this.fpsCounter.Anchor];
				if (drawableLabel.newText.Length > 0)
				{
					drawableLabel.newText.Append(new string('\n', this.countersSpacing + 1));
				}
				drawableLabel.newText.Append(this.fpsCounter.text);
				drawableLabel.dirty |= this.fpsCounter.dirty;
				this.fpsCounter.dirty = false;
				flag = true;
			}
			if (this.memoryCounter.Enabled)
			{
				DrawableLabel drawableLabel2 = this.labels[(int)this.memoryCounter.Anchor];
				if (drawableLabel2.newText.Length > 0)
				{
					drawableLabel2.newText.Append(new string('\n', this.countersSpacing + 1));
				}
				drawableLabel2.newText.Append(this.memoryCounter.text);
				drawableLabel2.dirty |= this.memoryCounter.dirty;
				this.memoryCounter.dirty = false;
				flag = true;
			}
			if (this.deviceInfoCounter.Enabled)
			{
				DrawableLabel drawableLabel3 = this.labels[(int)this.deviceInfoCounter.Anchor];
				if (drawableLabel3.newText.Length > 0)
				{
					drawableLabel3.newText.Append(new string('\n', this.countersSpacing + 1));
				}
				drawableLabel3.newText.Append(this.deviceInfoCounter.text);
				drawableLabel3.dirty |= this.deviceInfoCounter.dirty;
				this.deviceInfoCounter.dirty = false;
				flag = true;
			}
			if (flag)
			{
				for (int i = 0; i < this.anchorsCount; i++)
				{
					this.labels[i].CheckAndUpdate();
				}
				return;
			}
			for (int j = 0; j < this.anchorsCount; j++)
			{
				this.labels[j].Clear();
			}
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0003086C File Offset: 0x0002EA6C
		private void ConfigureCanvas()
		{
			if (base.GetComponentInParent<Canvas>() != null)
			{
				this.externalCanvas = true;
				RectTransform rectTransform = base.gameObject.GetComponent<RectTransform>();
				if (rectTransform == null)
				{
					rectTransform = base.gameObject.AddComponent<RectTransform>();
				}
				UIUtils.ResetRectTransform(rectTransform);
			}
			GameObject gameObject = new GameObject("CountersCanvas", new Type[]
			{
				typeof(Canvas)
			});
			gameObject.tag = base.gameObject.tag;
			gameObject.layer = base.gameObject.layer;
			gameObject.transform.SetParent(base.transform, false);
			this.canvas = gameObject.GetComponent<Canvas>();
			UIUtils.ResetRectTransform(gameObject.GetComponent<RectTransform>());
			if (!this.externalCanvas)
			{
				this.canvas.renderMode = RenderMode.ScreenSpaceOverlay;
				this.canvas.pixelPerfect = this.pixelPerfect;
				this.canvas.sortingOrder = this.sortingOrder;
				this.canvasScaler = gameObject.AddComponent<CanvasScaler>();
				if (this.autoScale)
				{
					this.canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
					return;
				}
				this.canvasScaler.scaleFactor = this.scaleFactor;
			}
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x00030988 File Offset: 0x0002EB88
		private void ConfigureLabels()
		{
			this.anchorsCount = Enum.GetNames(typeof(LabelAnchor)).Length;
			this.labels = new DrawableLabel[this.anchorsCount];
			for (int i = 0; i < this.anchorsCount; i++)
			{
				this.labels[i] = new DrawableLabel(this.canvas.gameObject, (LabelAnchor)i, new LabelEffect(this.background, this.backgroundColor, this.backgroundPadding), new LabelEffect(this.shadow, this.shadowColor, this.shadowDistance), new LabelEffect(this.outline, this.outlineColor, this.outlineDistance), this.labelsFont, this.fontSize, this.lineSpacing, this.paddingOffset);
			}
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x00030A48 File Offset: 0x0002EC48
		private void DisposeInternal()
		{
			UnityEngine.Object.Destroy(this);
			if (AFPSCounter.Instance == this)
			{
				AFPSCounter.Instance = null;
			}
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x00030A64 File Offset: 0x0002EC64
		private void ProcessHotKey()
		{
			if (this.hotKey == KeyCode.None)
			{
				return;
			}
			if (Input.GetKeyDown(this.hotKey))
			{
				bool flag = true;
				if (this.hotKeyCtrl)
				{
					flag &= (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftMeta) || Input.GetKey(KeyCode.RightMeta));
				}
				if (this.hotKeyAlt)
				{
					flag &= (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt));
				}
				if (this.hotKeyShift)
				{
					flag &= (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
				}
				if (flag)
				{
					this.SwitchCounter();
				}
			}
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x00030B1C File Offset: 0x0002ED1C
		private bool CircleGestureMade()
		{
			int num = this.gesturePoints.Count;
			if (Input.GetMouseButton(0))
			{
				Vector2 vector = Input.mousePosition;
				if (num == 0 || (vector - this.gesturePoints[num - 1]).magnitude > 10f)
				{
					this.gesturePoints.Add(vector);
					num++;
				}
			}
			else if (Input.GetMouseButtonUp(0))
			{
				num = 0;
				this.gestureCount = 0;
				this.gesturePoints.Clear();
			}
			if (num < 10)
			{
				return false;
			}
			float num2 = 0f;
			Vector2 a = Vector2.zero;
			Vector2 rhs = Vector2.zero;
			for (int i = 0; i < num - 2; i++)
			{
				Vector2 vector2 = this.gesturePoints[i + 1] - this.gesturePoints[i];
				a += vector2;
				float magnitude = vector2.magnitude;
				num2 += magnitude;
				if (Vector2.Dot(vector2, rhs) < 0f)
				{
					this.gesturePoints.Clear();
					this.gestureCount = 0;
					return false;
				}
				rhs = vector2;
			}
			bool result = false;
			int num3 = (Screen.width + Screen.height) / 4;
			if (num2 > (float)num3 && a.magnitude < (float)num3 / 2f)
			{
				this.gesturePoints.Clear();
				this.gestureCount++;
				if (this.gestureCount >= 2)
				{
					this.gestureCount = 0;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x00030C83 File Offset: 0x0002EE83
		private void SwitchCounter()
		{
			if (this.operationMode == OperationMode.Disabled)
			{
				this.OperationMode = OperationMode.Normal;
				return;
			}
			if (this.operationMode == OperationMode.Normal)
			{
				this.OperationMode = OperationMode.Disabled;
			}
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x00030CA8 File Offset: 0x0002EEA8
		private void ActivateCounters()
		{
			this.fpsCounter.Activate();
			this.memoryCounter.Activate();
			this.deviceInfoCounter.Activate();
			if (this.fpsCounter.Enabled || this.memoryCounter.Enabled || this.deviceInfoCounter.Enabled)
			{
				this.UpdateTexts();
			}
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x00030D03 File Offset: 0x0002EF03
		private void DeactivateCounters()
		{
			if (AFPSCounter.Instance == null)
			{
				return;
			}
			this.fpsCounter.Deactivate();
			this.memoryCounter.Deactivate();
			this.deviceInfoCounter.Deactivate();
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x00030D34 File Offset: 0x0002EF34
		private void RefreshForcedFrameRate()
		{
			this.RefreshForcedFrameRate(false);
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x00030D40 File Offset: 0x0002EF40
		private void RefreshForcedFrameRate(bool disabling)
		{
			if (this.forceFrameRate && !disabling)
			{
				if (this.cachedVSync == -1)
				{
					this.cachedVSync = QualitySettings.vSyncCount;
					this.cachedFrameRate = Application.targetFrameRate;
					QualitySettings.vSyncCount = 0;
				}
				Application.targetFrameRate = this.forcedFrameRate;
				return;
			}
			if (this.cachedVSync != -1)
			{
				QualitySettings.vSyncCount = this.cachedVSync;
				Application.targetFrameRate = this.cachedFrameRate;
				this.cachedVSync = -1;
			}
		}

		// Token: 0x04000B35 RID: 2869
		private const string MenuPath = "Code Stage/\ud83d\ude80 Advanced FPS Counter";

		// Token: 0x04000B36 RID: 2870
		private const string ComponentName = "Advanced FPS Counter";

		// Token: 0x04000B37 RID: 2871
		internal const string LogPrefix = "[AFPSCounter]: ";

		// Token: 0x04000B38 RID: 2872
		internal const char NewLine = '\n';

		// Token: 0x04000B39 RID: 2873
		internal const char Space = ' ';

		// Token: 0x04000B3A RID: 2874
		public FPSCounterData fpsCounter = new FPSCounterData();

		// Token: 0x04000B3B RID: 2875
		public MemoryCounterData memoryCounter = new MemoryCounterData();

		// Token: 0x04000B3C RID: 2876
		public DeviceInfoCounterData deviceInfoCounter = new DeviceInfoCounterData();

		// Token: 0x04000B3D RID: 2877
		[Tooltip("Used to enable / disable plugin at runtime.\nSet to None to disable.")]
		public KeyCode hotKey = KeyCode.BackQuote;

		// Token: 0x04000B3E RID: 2878
		[Tooltip("Used to enable / disable plugin at runtime.\nMake two circle gestures with your finger \\ mouse to switch plugin on and off.")]
		public bool circleGesture;

		// Token: 0x04000B3F RID: 2879
		[Tooltip("Hot key modifier: any Control on Windows or any Command on Mac.")]
		public bool hotKeyCtrl;

		// Token: 0x04000B40 RID: 2880
		[Tooltip("Hot key modifier: any Shift.")]
		public bool hotKeyShift;

		// Token: 0x04000B41 RID: 2881
		[Tooltip("Hot key modifier: any Alt.")]
		public bool hotKeyAlt;

		// Token: 0x04000B42 RID: 2882
		[Tooltip("Prevents current or other topmost Game Object from destroying on level (scene) load.\nApplied once, on Start phase.")]
		[SerializeField]
		private bool keepAlive = true;

		// Token: 0x04000B43 RID: 2883
		private Canvas canvas;

		// Token: 0x04000B44 RID: 2884
		private CanvasScaler canvasScaler;

		// Token: 0x04000B45 RID: 2885
		private bool externalCanvas;

		// Token: 0x04000B46 RID: 2886
		private DrawableLabel[] labels;

		// Token: 0x04000B47 RID: 2887
		private int anchorsCount;

		// Token: 0x04000B48 RID: 2888
		private int cachedVSync = -1;

		// Token: 0x04000B49 RID: 2889
		private int cachedFrameRate = -1;

		// Token: 0x04000B4A RID: 2890
		private bool inited;

		// Token: 0x04000B4B RID: 2891
		private readonly List<Vector2> gesturePoints = new List<Vector2>();

		// Token: 0x04000B4C RID: 2892
		private int gestureCount;

		// Token: 0x04000B4D RID: 2893
		[Tooltip("Disabled: removes labels and stops all internal processes except Hot Key listener.\n\nBackground: removes labels keeping counters alive; use for hidden performance monitoring.\n\nNormal: shows labels and runs all internal processes as usual.")]
		[SerializeField]
		private OperationMode operationMode = OperationMode.Normal;

		// Token: 0x04000B4E RID: 2894
		[Tooltip("Allows to see how your game performs on specified frame rate.\nDoes not guarantee selected frame rate. Set -1 to render as fast as possible in current conditions.\nIMPORTANT: this option disables VSync while enabled!")]
		[SerializeField]
		private bool forceFrameRate;

		// Token: 0x04000B4F RID: 2895
		[Range(-1f, 200f)]
		[SerializeField]
		private int forcedFrameRate = -1;

		// Token: 0x04000B50 RID: 2896
		[Tooltip("Background for all texts. Cheapest effect. Overhead: 1 Draw Call.")]
		[SerializeField]
		private bool background = true;

		// Token: 0x04000B51 RID: 2897
		[Tooltip("Color of the background.")]
		[SerializeField]
		private Color backgroundColor = new Color32(0, 0, 0, 155);

		// Token: 0x04000B52 RID: 2898
		[Tooltip("Padding of the background.")]
		[Range(0f, 30f)]
		[SerializeField]
		private int backgroundPadding = 5;

		// Token: 0x04000B53 RID: 2899
		[Tooltip("Shadow effect for all texts. This effect uses extra resources. Overhead: medium CPU and light GPU usage.")]
		[SerializeField]
		private bool shadow;

		// Token: 0x04000B54 RID: 2900
		[Tooltip("Color of the shadow effect.")]
		[SerializeField]
		private Color shadowColor = new Color32(0, 0, 0, 128);

		// Token: 0x04000B55 RID: 2901
		[Tooltip("Distance of the shadow effect.")]
		[SerializeField]
		private Vector2 shadowDistance = new Vector2(1f, -1f);

		// Token: 0x04000B56 RID: 2902
		[Tooltip("Outline effect for all texts. Resource-heaviest effect. Overhead: huge CPU and medium GPU usage. Not recommended for use unless really necessary.")]
		[SerializeField]
		private bool outline;

		// Token: 0x04000B57 RID: 2903
		[Tooltip("Color of the outline effect.")]
		[SerializeField]
		private Color outlineColor = new Color32(0, 0, 0, 128);

		// Token: 0x04000B58 RID: 2904
		[Tooltip("Distance of the outline effect.")]
		[SerializeField]
		private Vector2 outlineDistance = new Vector2(1f, -1f);

		// Token: 0x04000B59 RID: 2905
		[Tooltip("Controls own Canvas Scaler scale mode. Check to use ScaleWithScreenSize. Otherwise ConstantPixelSize will be used.")]
		[SerializeField]
		private bool autoScale;

		// Token: 0x04000B5A RID: 2906
		[Tooltip("Controls global scale of all texts.")]
		[Range(0f, 30f)]
		[SerializeField]
		private float scaleFactor = 1f;

		// Token: 0x04000B5B RID: 2907
		[Tooltip("Leave blank to use default font.")]
		[SerializeField]
		private Font labelsFont;

		// Token: 0x04000B5C RID: 2908
		[Tooltip("Set to 0 to use font size specified in the font importer.")]
		[Range(0f, 100f)]
		[SerializeField]
		private int fontSize = 14;

		// Token: 0x04000B5D RID: 2909
		[Tooltip("Space between lines in labels.")]
		[Range(0f, 10f)]
		[SerializeField]
		private float lineSpacing = 1f;

		// Token: 0x04000B5E RID: 2910
		[Tooltip("Lines count between different counters in a single label.")]
		[Range(0f, 10f)]
		[SerializeField]
		private int countersSpacing;

		// Token: 0x04000B5F RID: 2911
		[Tooltip("Pixel offset for anchored labels. Automatically applied to all labels.")]
		[SerializeField]
		private Vector2 paddingOffset = new Vector2(5f, 5f);

		// Token: 0x04000B60 RID: 2912
		[Tooltip("Controls own canvas Pixel Perfect property.")]
		[SerializeField]
		private bool pixelPerfect = true;

		// Token: 0x04000B61 RID: 2913
		[Tooltip("Sorting order to use for the canvas.\nSet higher value to get closer to the user.")]
		[SerializeField]
		private int sortingOrder = 10000;
	}
}
