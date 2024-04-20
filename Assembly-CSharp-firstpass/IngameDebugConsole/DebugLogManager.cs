using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IngameDebugConsole
{
	// Token: 0x020001EF RID: 495
	public class DebugLogManager : MonoBehaviour
	{
		// Token: 0x06000F75 RID: 3957 RVA: 0x0002DEA4 File Offset: 0x0002C0A4
		private void Awake()
		{
			if (DebugLogManager.instance == null)
			{
				DebugLogManager.instance = this;
				if (this.singleton)
				{
					UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
				}
			}
			else if (this != DebugLogManager.instance)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			this.pooledLogItems = new List<DebugLogItem>();
			this.queuedLogs = new List<QueuedDebugLogEntry>();
			this.commandHistory = new CircularBuffer<string>(this.commandHistorySize);
			this.canvasTR = (RectTransform)base.transform;
			this.logSpriteRepresentations = new Dictionary<LogType, Sprite>
			{
				{
					LogType.Log,
					this.infoLog
				},
				{
					LogType.Warning,
					this.warningLog
				},
				{
					LogType.Error,
					this.errorLog
				},
				{
					LogType.Exception,
					this.errorLog
				},
				{
					LogType.Assert,
					this.errorLog
				}
			};
			this.filterInfoButton.color = this.filterButtonsSelectedColor;
			this.filterWarningButton.color = this.filterButtonsSelectedColor;
			this.filterErrorButton.color = this.filterButtonsSelectedColor;
			this.collapsedLogEntries = new List<DebugLogEntry>(128);
			this.collapsedLogEntriesMap = new Dictionary<DebugLogEntry, int>(128);
			this.uncollapsedLogEntriesIndices = new DebugLogIndexList();
			this.indicesOfListEntriesToShow = new DebugLogIndexList();
			this.recycledListView.Initialize(this, this.collapsedLogEntries, this.indicesOfListEntriesToShow, this.logItemPrefab.Transform.sizeDelta.y);
			this.recycledListView.UpdateItemsInTheList(true);
			if (this.minimumHeight < 200f)
			{
				this.minimumHeight = 200f;
			}
			this.nullPointerEventData = new PointerEventData(null);
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x0002E03C File Offset: 0x0002C23C
		private void OnEnable()
		{
			Application.logMessageReceived -= this.ReceivedLog;
			Application.logMessageReceived += this.ReceivedLog;
			InputField inputField = this.commandInputField;
			inputField.onValidateInput = (InputField.OnValidateInput)Delegate.Remove(inputField.onValidateInput, new InputField.OnValidateInput(this.OnValidateCommand));
			InputField inputField2 = this.commandInputField;
			inputField2.onValidateInput = (InputField.OnValidateInput)Delegate.Combine(inputField2.onValidateInput, new InputField.OnValidateInput(this.OnValidateCommand));
			bool flag = this.receiveLogcatLogsInAndroid;
			DebugLogConsole.AddCommandInstance("save_logs", "Saves logs to a file", "SaveLogsToFile", this);
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0002E0D8 File Offset: 0x0002C2D8
		private void OnDisable()
		{
			if (DebugLogManager.instance != this)
			{
				return;
			}
			Application.logMessageReceived -= this.ReceivedLog;
			InputField inputField = this.commandInputField;
			inputField.onValidateInput = (InputField.OnValidateInput)Delegate.Remove(inputField.onValidateInput, new InputField.OnValidateInput(this.OnValidateCommand));
			DebugLogConsole.RemoveCommand("save_logs");
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x0002E135 File Offset: 0x0002C335
		private void Start()
		{
			if (this.enablePopup && this.startInPopupMode)
			{
				this.ShowPopup();
				return;
			}
			this.ShowLogWindow();
			this.popupManager.gameObject.SetActive(this.enablePopup);
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0002E16A File Offset: 0x0002C36A
		private void OnRectTransformDimensionsChange()
		{
			this.screenDimensionsChanged = true;
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x0002E174 File Offset: 0x0002C374
		private void LateUpdate()
		{
			int count = this.queuedLogs.Count;
			if (count > 0)
			{
				for (int i = 0; i < count; i++)
				{
					QueuedDebugLogEntry queuedDebugLogEntry = this.queuedLogs[i];
					this.ReceivedLog(queuedDebugLogEntry.logString, queuedDebugLogEntry.stackTrace, queuedDebugLogEntry.logType);
				}
				this.queuedLogs.Clear();
			}
			if (this.screenDimensionsChanged)
			{
				if (this.isLogWindowVisible)
				{
					this.recycledListView.OnViewportDimensionsChanged();
				}
				else
				{
					this.popupManager.OnViewportDimensionsChanged();
				}
				this.screenDimensionsChanged = false;
			}
			if (this.snapToBottom)
			{
				this.logItemsScrollRect.verticalNormalizedPosition = 0f;
				if (this.snapToBottomButton.activeSelf)
				{
					this.snapToBottomButton.SetActive(false);
				}
			}
			else
			{
				float verticalNormalizedPosition = this.logItemsScrollRect.verticalNormalizedPosition;
				if (this.snapToBottomButton.activeSelf != (verticalNormalizedPosition > 1E-06f && verticalNormalizedPosition < 0.9999f))
				{
					this.snapToBottomButton.SetActive(!this.snapToBottomButton.activeSelf);
				}
			}
			if (this.toggleWithKey && Input.GetKeyDown(this.toggleKey))
			{
				if (this.isLogWindowVisible)
				{
					this.ShowPopup();
				}
				else
				{
					this.ShowLogWindow();
				}
			}
			if (this.isLogWindowVisible && this.commandInputField.isFocused)
			{
				if (Input.GetKeyDown(KeyCode.UpArrow))
				{
					if (this.commandHistoryIndex == -1)
					{
						this.commandHistoryIndex = this.commandHistory.Count - 1;
					}
					else
					{
						int num = this.commandHistoryIndex - 1;
						this.commandHistoryIndex = num;
						if (num < 0)
						{
							this.commandHistoryIndex = 0;
						}
					}
					if (this.commandHistoryIndex >= 0 && this.commandHistoryIndex < this.commandHistory.Count)
					{
						this.commandInputField.text = this.commandHistory[this.commandHistoryIndex];
						this.commandInputField.caretPosition = this.commandInputField.text.Length;
						return;
					}
				}
				else if (Input.GetKeyDown(KeyCode.DownArrow))
				{
					if (this.commandHistoryIndex == -1)
					{
						this.commandHistoryIndex = this.commandHistory.Count - 1;
					}
					else
					{
						int num = this.commandHistoryIndex + 1;
						this.commandHistoryIndex = num;
						if (num >= this.commandHistory.Count)
						{
							this.commandHistoryIndex = this.commandHistory.Count - 1;
						}
					}
					if (this.commandHistoryIndex >= 0 && this.commandHistoryIndex < this.commandHistory.Count)
					{
						this.commandInputField.text = this.commandHistory[this.commandHistoryIndex];
					}
				}
			}
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x0002E3F8 File Offset: 0x0002C5F8
		public void ShowLogWindow()
		{
			this.logWindowCanvasGroup.interactable = true;
			this.logWindowCanvasGroup.blocksRaycasts = true;
			this.logWindowCanvasGroup.alpha = 1f;
			this.popupManager.Hide();
			this.recycledListView.OnLogEntriesUpdated(true);
			this.isLogWindowVisible = true;
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x0002E44C File Offset: 0x0002C64C
		public void ShowPopup()
		{
			this.logWindowCanvasGroup.interactable = false;
			this.logWindowCanvasGroup.blocksRaycasts = false;
			this.logWindowCanvasGroup.alpha = 0f;
			this.popupManager.Show();
			this.commandHistoryIndex = -1;
			this.isLogWindowVisible = false;
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x0002E49C File Offset: 0x0002C69C
		public char OnValidateCommand(string text, int charIndex, char addedChar)
		{
			if (addedChar == '\t')
			{
				if (!string.IsNullOrEmpty(text))
				{
					string autoCompleteCommand = DebugLogConsole.GetAutoCompleteCommand(text);
					if (!string.IsNullOrEmpty(autoCompleteCommand))
					{
						this.commandInputField.text = autoCompleteCommand;
					}
				}
				return '\0';
			}
			if (addedChar == '\n')
			{
				if (this.clearCommandAfterExecution)
				{
					this.commandInputField.text = "";
				}
				if (text.Length > 0)
				{
					if (this.commandHistory.Count == 0 || this.commandHistory[this.commandHistory.Count - 1] != text)
					{
						this.commandHistory.Add(text);
					}
					this.commandHistoryIndex = -1;
					DebugLogConsole.ExecuteCommand(text);
					this.SetSnapToBottom(true);
				}
				return '\0';
			}
			return addedChar;
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x0002E54C File Offset: 0x0002C74C
		private void ReceivedLog(string logString, string stackTrace, LogType logType)
		{
			if (CanvasUpdateRegistry.IsRebuildingGraphics() || CanvasUpdateRegistry.IsRebuildingLayout())
			{
				this.queuedLogs.Add(new QueuedDebugLogEntry(logString, stackTrace, logType));
				return;
			}
			DebugLogEntry debugLogEntry = new DebugLogEntry(logString, stackTrace, null);
			int count;
			bool flag = this.collapsedLogEntriesMap.TryGetValue(debugLogEntry, out count);
			if (!flag)
			{
				debugLogEntry.logTypeSpriteRepresentation = this.logSpriteRepresentations[logType];
				count = this.collapsedLogEntries.Count;
				this.collapsedLogEntries.Add(debugLogEntry);
				this.collapsedLogEntriesMap[debugLogEntry] = count;
			}
			else
			{
				debugLogEntry = this.collapsedLogEntries[count];
				debugLogEntry.count++;
			}
			this.uncollapsedLogEntriesIndices.Add(count);
			Sprite logTypeSpriteRepresentation = debugLogEntry.logTypeSpriteRepresentation;
			if (this.isCollapseOn && flag)
			{
				if (this.isLogWindowVisible)
				{
					this.recycledListView.OnCollapsedLogEntryAtIndexUpdated(count);
				}
			}
			else if (this.logFilter == DebugLogFilter.All || (logTypeSpriteRepresentation == this.infoLog && (this.logFilter & DebugLogFilter.Info) == DebugLogFilter.Info) || (logTypeSpriteRepresentation == this.warningLog && (this.logFilter & DebugLogFilter.Warning) == DebugLogFilter.Warning) || (logTypeSpriteRepresentation == this.errorLog && (this.logFilter & DebugLogFilter.Error) == DebugLogFilter.Error))
			{
				this.indicesOfListEntriesToShow.Add(count);
				if (this.isLogWindowVisible)
				{
					this.recycledListView.OnLogEntriesUpdated(false);
				}
			}
			if (logType == LogType.Log)
			{
				this.infoEntryCount++;
				this.infoEntryCountText.text = this.infoEntryCount.ToString();
				if (!this.isLogWindowVisible)
				{
					this.popupManager.NewInfoLogArrived();
					return;
				}
			}
			else if (logType == LogType.Warning)
			{
				this.warningEntryCount++;
				this.warningEntryCountText.text = this.warningEntryCount.ToString();
				if (!this.isLogWindowVisible)
				{
					this.popupManager.NewWarningLogArrived();
					return;
				}
			}
			else
			{
				this.errorEntryCount++;
				this.errorEntryCountText.text = this.errorEntryCount.ToString();
				if (!this.isLogWindowVisible)
				{
					this.popupManager.NewErrorLogArrived();
				}
			}
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x0002E743 File Offset: 0x0002C943
		public void SetSnapToBottom(bool snapToBottom)
		{
			this.snapToBottom = snapToBottom;
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x0002E74C File Offset: 0x0002C94C
		public void ValidateScrollPosition()
		{
			this.logItemsScrollRect.OnScroll(this.nullPointerEventData);
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x0002E75F File Offset: 0x0002C95F
		public void HideButtonPressed()
		{
			this.ShowPopup();
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x0002E768 File Offset: 0x0002C968
		public void ClearButtonPressed()
		{
			this.snapToBottom = true;
			this.infoEntryCount = 0;
			this.warningEntryCount = 0;
			this.errorEntryCount = 0;
			this.infoEntryCountText.text = "0";
			this.warningEntryCountText.text = "0";
			this.errorEntryCountText.text = "0";
			this.collapsedLogEntries.Clear();
			this.collapsedLogEntriesMap.Clear();
			this.uncollapsedLogEntriesIndices.Clear();
			this.indicesOfListEntriesToShow.Clear();
			this.recycledListView.DeselectSelectedLogItem();
			this.recycledListView.OnLogEntriesUpdated(true);
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x0002E804 File Offset: 0x0002CA04
		public void CollapseButtonPressed()
		{
			this.isCollapseOn = !this.isCollapseOn;
			this.snapToBottom = true;
			this.collapseButton.color = (this.isCollapseOn ? this.collapseButtonSelectedColor : this.collapseButtonNormalColor);
			this.recycledListView.SetCollapseMode(this.isCollapseOn);
			this.FilterLogs();
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0002E860 File Offset: 0x0002CA60
		public void FilterLogButtonPressed()
		{
			this.logFilter ^= DebugLogFilter.Info;
			if ((this.logFilter & DebugLogFilter.Info) == DebugLogFilter.Info)
			{
				this.filterInfoButton.color = this.filterButtonsSelectedColor;
			}
			else
			{
				this.filterInfoButton.color = this.filterButtonsNormalColor;
			}
			this.FilterLogs();
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x0002E8B0 File Offset: 0x0002CAB0
		public void FilterWarningButtonPressed()
		{
			this.logFilter ^= DebugLogFilter.Warning;
			if ((this.logFilter & DebugLogFilter.Warning) == DebugLogFilter.Warning)
			{
				this.filterWarningButton.color = this.filterButtonsSelectedColor;
			}
			else
			{
				this.filterWarningButton.color = this.filterButtonsNormalColor;
			}
			this.FilterLogs();
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x0002E900 File Offset: 0x0002CB00
		public void FilterErrorButtonPressed()
		{
			this.logFilter ^= DebugLogFilter.Error;
			if ((this.logFilter & DebugLogFilter.Error) == DebugLogFilter.Error)
			{
				this.filterErrorButton.color = this.filterButtonsSelectedColor;
			}
			else
			{
				this.filterErrorButton.color = this.filterButtonsNormalColor;
			}
			this.FilterLogs();
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x0002E950 File Offset: 0x0002CB50
		public void Resize(BaseEventData dat)
		{
			float num = (((PointerEventData)dat).position.y - this.logWindowTR.position.y) / -this.canvasTR.localScale.y + 36f;
			if (num < this.minimumHeight)
			{
				num = this.minimumHeight;
			}
			Vector2 anchorMin = this.logWindowTR.anchorMin;
			anchorMin.y = Mathf.Max(0f, 1f - num / this.canvasTR.sizeDelta.y);
			this.logWindowTR.anchorMin = anchorMin;
			this.recycledListView.OnViewportDimensionsChanged();
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x0002E9F4 File Offset: 0x0002CBF4
		private void FilterLogs()
		{
			if (this.logFilter == DebugLogFilter.None)
			{
				this.indicesOfListEntriesToShow.Clear();
			}
			else if (this.logFilter == DebugLogFilter.All)
			{
				if (this.isCollapseOn)
				{
					this.indicesOfListEntriesToShow.Clear();
					for (int i = 0; i < this.collapsedLogEntries.Count; i++)
					{
						this.indicesOfListEntriesToShow.Add(i);
					}
				}
				else
				{
					this.indicesOfListEntriesToShow.Clear();
					for (int j = 0; j < this.uncollapsedLogEntriesIndices.Count; j++)
					{
						this.indicesOfListEntriesToShow.Add(this.uncollapsedLogEntriesIndices[j]);
					}
				}
			}
			else
			{
				bool flag = (this.logFilter & DebugLogFilter.Info) == DebugLogFilter.Info;
				bool flag2 = (this.logFilter & DebugLogFilter.Warning) == DebugLogFilter.Warning;
				bool flag3 = (this.logFilter & DebugLogFilter.Error) == DebugLogFilter.Error;
				if (this.isCollapseOn)
				{
					this.indicesOfListEntriesToShow.Clear();
					for (int k = 0; k < this.collapsedLogEntries.Count; k++)
					{
						DebugLogEntry debugLogEntry = this.collapsedLogEntries[k];
						if (debugLogEntry.logTypeSpriteRepresentation == this.infoLog && flag)
						{
							this.indicesOfListEntriesToShow.Add(k);
						}
						else if (debugLogEntry.logTypeSpriteRepresentation == this.warningLog && flag2)
						{
							this.indicesOfListEntriesToShow.Add(k);
						}
						else if (debugLogEntry.logTypeSpriteRepresentation == this.errorLog && flag3)
						{
							this.indicesOfListEntriesToShow.Add(k);
						}
					}
				}
				else
				{
					this.indicesOfListEntriesToShow.Clear();
					for (int l = 0; l < this.uncollapsedLogEntriesIndices.Count; l++)
					{
						DebugLogEntry debugLogEntry2 = this.collapsedLogEntries[this.uncollapsedLogEntriesIndices[l]];
						if (debugLogEntry2.logTypeSpriteRepresentation == this.infoLog && flag)
						{
							this.indicesOfListEntriesToShow.Add(this.uncollapsedLogEntriesIndices[l]);
						}
						else if (debugLogEntry2.logTypeSpriteRepresentation == this.warningLog && flag2)
						{
							this.indicesOfListEntriesToShow.Add(this.uncollapsedLogEntriesIndices[l]);
						}
						else if (debugLogEntry2.logTypeSpriteRepresentation == this.errorLog && flag3)
						{
							this.indicesOfListEntriesToShow.Add(this.uncollapsedLogEntriesIndices[l]);
						}
					}
				}
			}
			this.recycledListView.DeselectSelectedLogItem();
			this.recycledListView.OnLogEntriesUpdated(true);
			this.ValidateScrollPosition();
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x0002EC68 File Offset: 0x0002CE68
		public string GetAllLogs()
		{
			int count = this.uncollapsedLogEntriesIndices.Count;
			int num = 0;
			int length = Environment.NewLine.Length;
			for (int i = 0; i < count; i++)
			{
				DebugLogEntry debugLogEntry = this.collapsedLogEntries[this.uncollapsedLogEntriesIndices[i]];
				num += debugLogEntry.logString.Length + debugLogEntry.stackTrace.Length + length * 3;
			}
			num += 100;
			StringBuilder stringBuilder = new StringBuilder(num);
			for (int j = 0; j < count; j++)
			{
				DebugLogEntry debugLogEntry2 = this.collapsedLogEntries[this.uncollapsedLogEntriesIndices[j]];
				stringBuilder.AppendLine(debugLogEntry2.logString).AppendLine(debugLogEntry2.stackTrace).AppendLine();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0002ED34 File Offset: 0x0002CF34
		private void SaveLogsToFile()
		{
			string text = Path.Combine(Application.persistentDataPath, DateTime.Now.ToString("dd-MM-yyyy--HH-mm-ss") + ".txt");
			File.WriteAllText(text, DebugLogManager.instance.GetAllLogs());
			Debug.Log("Logs saved to: " + text);
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x0002ED88 File Offset: 0x0002CF88
		public void PoolLogItem(DebugLogItem logItem)
		{
			logItem.gameObject.SetActive(false);
			this.pooledLogItems.Add(logItem);
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x0002EDA4 File Offset: 0x0002CFA4
		public DebugLogItem PopLogItem()
		{
			DebugLogItem debugLogItem;
			if (this.pooledLogItems.Count > 0)
			{
				debugLogItem = this.pooledLogItems[this.pooledLogItems.Count - 1];
				this.pooledLogItems.RemoveAt(this.pooledLogItems.Count - 1);
				debugLogItem.gameObject.SetActive(true);
			}
			else
			{
				debugLogItem = UnityEngine.Object.Instantiate<DebugLogItem>(this.logItemPrefab, this.logItemsContainer, false);
				debugLogItem.Initialize(this.recycledListView);
			}
			return debugLogItem;
		}

		// Token: 0x04000AD9 RID: 2777
		private static DebugLogManager instance;

		// Token: 0x04000ADA RID: 2778
		[Header("Properties")]
		[SerializeField]
		[HideInInspector]
		private bool singleton = true;

		// Token: 0x04000ADB RID: 2779
		[SerializeField]
		[HideInInspector]
		private float minimumHeight = 200f;

		// Token: 0x04000ADC RID: 2780
		[SerializeField]
		[HideInInspector]
		private bool enablePopup = true;

		// Token: 0x04000ADD RID: 2781
		[SerializeField]
		[HideInInspector]
		private bool startInPopupMode = true;

		// Token: 0x04000ADE RID: 2782
		[SerializeField]
		[HideInInspector]
		private bool toggleWithKey;

		// Token: 0x04000ADF RID: 2783
		[SerializeField]
		[HideInInspector]
		private KeyCode toggleKey = KeyCode.BackQuote;

		// Token: 0x04000AE0 RID: 2784
		[SerializeField]
		[HideInInspector]
		private bool clearCommandAfterExecution = true;

		// Token: 0x04000AE1 RID: 2785
		[SerializeField]
		[HideInInspector]
		private int commandHistorySize = 15;

		// Token: 0x04000AE2 RID: 2786
		[SerializeField]
		[HideInInspector]
		private bool receiveLogcatLogsInAndroid;

		// Token: 0x04000AE3 RID: 2787
		[SerializeField]
		[HideInInspector]
		private string logcatArguments;

		// Token: 0x04000AE4 RID: 2788
		[Header("Visuals")]
		[SerializeField]
		private DebugLogItem logItemPrefab;

		// Token: 0x04000AE5 RID: 2789
		[SerializeField]
		private Sprite infoLog;

		// Token: 0x04000AE6 RID: 2790
		[SerializeField]
		private Sprite warningLog;

		// Token: 0x04000AE7 RID: 2791
		[SerializeField]
		private Sprite errorLog;

		// Token: 0x04000AE8 RID: 2792
		private Dictionary<LogType, Sprite> logSpriteRepresentations;

		// Token: 0x04000AE9 RID: 2793
		[SerializeField]
		private Color collapseButtonNormalColor;

		// Token: 0x04000AEA RID: 2794
		[SerializeField]
		private Color collapseButtonSelectedColor;

		// Token: 0x04000AEB RID: 2795
		[SerializeField]
		private Color filterButtonsNormalColor;

		// Token: 0x04000AEC RID: 2796
		[SerializeField]
		private Color filterButtonsSelectedColor;

		// Token: 0x04000AED RID: 2797
		[Header("Internal References")]
		[SerializeField]
		private RectTransform logWindowTR;

		// Token: 0x04000AEE RID: 2798
		private RectTransform canvasTR;

		// Token: 0x04000AEF RID: 2799
		[SerializeField]
		private RectTransform logItemsContainer;

		// Token: 0x04000AF0 RID: 2800
		[SerializeField]
		private InputField commandInputField;

		// Token: 0x04000AF1 RID: 2801
		[SerializeField]
		private Image collapseButton;

		// Token: 0x04000AF2 RID: 2802
		[SerializeField]
		private Image filterInfoButton;

		// Token: 0x04000AF3 RID: 2803
		[SerializeField]
		private Image filterWarningButton;

		// Token: 0x04000AF4 RID: 2804
		[SerializeField]
		private Image filterErrorButton;

		// Token: 0x04000AF5 RID: 2805
		[SerializeField]
		private Text infoEntryCountText;

		// Token: 0x04000AF6 RID: 2806
		[SerializeField]
		private Text warningEntryCountText;

		// Token: 0x04000AF7 RID: 2807
		[SerializeField]
		private Text errorEntryCountText;

		// Token: 0x04000AF8 RID: 2808
		[SerializeField]
		private GameObject snapToBottomButton;

		// Token: 0x04000AF9 RID: 2809
		[SerializeField]
		private CanvasGroup logWindowCanvasGroup;

		// Token: 0x04000AFA RID: 2810
		[SerializeField]
		private DebugLogPopup popupManager;

		// Token: 0x04000AFB RID: 2811
		[SerializeField]
		private ScrollRect logItemsScrollRect;

		// Token: 0x04000AFC RID: 2812
		[SerializeField]
		private DebugLogRecycledListView recycledListView;

		// Token: 0x04000AFD RID: 2813
		private int infoEntryCount;

		// Token: 0x04000AFE RID: 2814
		private int warningEntryCount;

		// Token: 0x04000AFF RID: 2815
		private int errorEntryCount;

		// Token: 0x04000B00 RID: 2816
		private bool isLogWindowVisible = true;

		// Token: 0x04000B01 RID: 2817
		private bool screenDimensionsChanged;

		// Token: 0x04000B02 RID: 2818
		private bool isCollapseOn;

		// Token: 0x04000B03 RID: 2819
		private DebugLogFilter logFilter = DebugLogFilter.All;

		// Token: 0x04000B04 RID: 2820
		private bool snapToBottom = true;

		// Token: 0x04000B05 RID: 2821
		private List<DebugLogEntry> collapsedLogEntries;

		// Token: 0x04000B06 RID: 2822
		private Dictionary<DebugLogEntry, int> collapsedLogEntriesMap;

		// Token: 0x04000B07 RID: 2823
		private DebugLogIndexList uncollapsedLogEntriesIndices;

		// Token: 0x04000B08 RID: 2824
		private DebugLogIndexList indicesOfListEntriesToShow;

		// Token: 0x04000B09 RID: 2825
		private List<QueuedDebugLogEntry> queuedLogs;

		// Token: 0x04000B0A RID: 2826
		private List<DebugLogItem> pooledLogItems;

		// Token: 0x04000B0B RID: 2827
		private CircularBuffer<string> commandHistory;

		// Token: 0x04000B0C RID: 2828
		private int commandHistoryIndex = -1;

		// Token: 0x04000B0D RID: 2829
		private PointerEventData nullPointerEventData;
	}
}
