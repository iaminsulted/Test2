using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class Chat : MonoBehaviour
{
	public enum FilterType
	{
		General,
		Party,
		Whisper,
		ServerMessage,
		Notification,
		Guild
	}

	private class ChatMessage
	{
		public FilterType Filter;

		public string Message;

		public DateTime recievedTime;

		public ChatMessage(FilterType filter, string message)
		{
			Filter = filter;
			Message = message;
			recievedTime = DateTime.Now;
		}
	}

	public UIToggle toggleGeneral;

	public UIToggle toggleParty;

	public UIToggle toggleGuild;

	public UILabel lblPrefix;

	public UILabel lblChatInput;

	public UILabel lblChatText;

	public UIGrid tabs;

	public UIInputChat chatInput;

	public UITextList chatText;

	private ChatFilter chatFilter;

	public UISprite inputBG;

	public UISprite chatBG;

	public UISprite chatOptionsBG;

	private int curChType;

	private Com.CmdChat currentChatMode;

	private string pmTo;

	private string pmFrom;

	private FilterType currentFilter;

	public static Chat Instance;

	private Dictionary<FilterType, List<ChatMessage>> ChatHistory = new Dictionary<FilterType, List<ChatMessage>>();

	private List<string> cmdHistory = new List<string>();

	private int cmdIndex;

	private static bool showTimestamp;

	private string lastCommand
	{
		get
		{
			if (cmdHistory.Count <= 0)
			{
				return "";
			}
			return cmdHistory[cmdHistory.Count - 1];
		}
	}

	public bool IsFocused => chatInput.mInput.isSelected;

	private void Start()
	{
		Instance = this;
		foreach (FilterType value in Enum.GetValues(typeof(FilterType)))
		{
			ChatHistory[value] = new List<ChatMessage>();
		}
		lblChatText.fontSize = SettingsManager.ChatTextSize;
		showTimestamp = SettingsManager.IsTimestampEnabled;
		chatText.Clear();
		chatInput.onChatSubmit += OnChatSubmit;
		chatInput.onInputChanged += onInputChanged;
		chatInput.onFocus += onFocus;
		curChType = 0;
		chatFilter = new ChatFilter();
		ChangeChannelGeneral();
		OnPartyUpdated();
		SetOpacityFromPrefs();
		Notify(Session.MyPlayerData.MOTD);
		if (Session.MyPlayerData.IsInGuild && !string.IsNullOrEmpty(Session.MyPlayerData.Guild.MOTD))
		{
			Notify(Session.MyPlayerData.Guild.name + ": " + Session.MyPlayerData.Guild.MOTD + System.Environment.NewLine + "[Current Guild Tax: " + Session.MyPlayerData.Guild.TaxRate + "%]", InterfaceColors.Chat.Guild.ToBBCode(), FilterType.Guild);
		}
		TurnOnGuild();
		tabs.Reposition();
	}

	private void OnEnable()
	{
		PartyManager.PartyUpdated += OnPartyUpdated;
		Session.MyPlayerData.GuildsUpdated += OnGuildUpdated;
		Game.Instance.ChatReceived += ProcessChatResponse;
		SettingsManager.ChatTextSizeUpdated += ChatTextSizeUpdated;
		SettingsManager.TimestampEnabledUpdated += ToggleTimestamp;
		SettingsManager.ChatBackgroundOpacityUpdated += ChangeChatBGOpacity;
	}

	private void OnDisable()
	{
		PartyManager.PartyUpdated -= OnPartyUpdated;
		Session.MyPlayerData.GuildsUpdated -= OnGuildUpdated;
		Game.Instance.ChatReceived -= ProcessChatResponse;
		SettingsManager.ChatTextSizeUpdated -= ChatTextSizeUpdated;
		SettingsManager.TimestampEnabledUpdated -= ToggleTimestamp;
		SettingsManager.ChatBackgroundOpacityUpdated -= ChangeChatBGOpacity;
	}

	private void OnPartyUpdated()
	{
		if (currentFilter == FilterType.Party && !PartyManager.IsInParty)
		{
			ChangeChannelGeneral();
		}
		toggleParty.gameObject.SetActive(PartyManager.IsInParty);
		tabs.Reposition();
	}

	private void OnGuildUpdated(bool leaveGuild)
	{
		if (currentFilter == FilterType.Guild && leaveGuild)
		{
			ChangeChannelGeneral();
		}
		toggleGuild.gameObject.SetActive(Session.MyPlayerData.IsInGuild);
		tabs.Reposition();
	}

	public void Focus()
	{
		chatInput.mInput.isSelected = true;
	}

	private void ChangeChannelGeneral()
	{
		chatInput.mInput.value = "";
		currentChatMode = Com.CmdChat.Multi;
		SetPrefix("", Color.white);
		SetChatFilter(FilterType.General);
	}

	private void ChangeChannelParty()
	{
		chatInput.mInput.value = "";
		currentChatMode = Com.CmdChat.Party;
		SetPrefix(InterfaceColors.Chat.Party.ToBBCode() + "[Party]:", InterfaceColors.Chat.Party);
		SetChatFilter(FilterType.Party);
	}

	public void ChangeChannelGuild()
	{
		chatInput.mInput.value = "";
		currentChatMode = Com.CmdChat.Guild;
		SetPrefix(InterfaceColors.Chat.Guild.ToBBCode() + "[Guild]:", InterfaceColors.Chat.Guild);
		SetChatFilter(FilterType.Guild);
	}

	private void SetChatFilter(FilterType type)
	{
		if (currentFilter != type)
		{
			currentFilter = type;
			if (currentFilter == FilterType.Party)
			{
				toggleParty.value = true;
			}
			else if (currentFilter == FilterType.Guild)
			{
				toggleGuild.value = true;
			}
			else
			{
				toggleGeneral.value = true;
			}
			tabs.Reposition();
			RefreshChat();
		}
	}

	private void ChatTextSizeUpdated(int size)
	{
		if ((bool)lblChatText)
		{
			lblChatText.fontSize = size;
		}
		else
		{
			Debug.LogError("This gameObject does not a have a reference to the label");
		}
		RefreshChat();
	}

	public void ClearChat()
	{
		chatText.Clear();
		foreach (FilterType key in ChatHistory.Keys)
		{
			ChatHistory[key].Clear();
		}
	}

	private void SetGeneralMode()
	{
		chatInput.mInput.value = "";
		currentChatMode = Com.CmdChat.Multi;
		SetPrefix("", Color.white);
		SetChatFilter(FilterType.General);
	}

	public void SetWhisper(string name)
	{
		chatInput.mInput.value = "";
		if (!string.IsNullOrEmpty(name))
		{
			currentChatMode = Com.CmdChat.Whisper;
			pmTo = name.ToLower();
			string prefixText = InterfaceColors.Chat.Dark_Whisper_Pink.ToBBCode() + "[To " + name + "]:";
			SetPrefix(prefixText, InterfaceColors.Chat.Whisper_Pink);
			SetChatFilter(FilterType.General);
		}
	}

	private void SetPartyMode()
	{
		chatInput.mInput.value = "";
		if (!PartyManager.IsInParty)
		{
			Notify("You are not in a party.");
			return;
		}
		currentChatMode = Com.CmdChat.Party;
		SetPrefix(InterfaceColors.Chat.Party.ToBBCode() + "[Party]:", InterfaceColors.Chat.Party);
	}

	private void SetGuildMode()
	{
		chatInput.mInput.value = "";
		if (!Session.MyPlayerData.IsInGuild)
		{
			Notify("You are not in a guild.");
			return;
		}
		currentChatMode = Com.CmdChat.Guild;
		SetPrefix(InterfaceColors.Chat.Guild.ToBBCode() + "[Guild]:", InterfaceColors.Chat.Guild);
	}

	private Color32 GetColorByFilter(FilterType filter)
	{
		Color32 result = InterfaceColors.Chat.Dark_White;
		switch (filter)
		{
		case FilterType.General:
			result = InterfaceColors.Chat.Dark_White;
			break;
		case FilterType.Party:
			result = InterfaceColors.Chat.Party;
			break;
		case FilterType.Whisper:
			result = InterfaceColors.Chat.Whisper_Pink;
			break;
		case FilterType.ServerMessage:
			result = InterfaceColors.Chat.Red;
			break;
		case FilterType.Notification:
			result = InterfaceColors.Chat.Yellow;
			break;
		}
		return result;
	}

	private void SetPrefix(string prefixText, Color32 color)
	{
		lblPrefix.text = prefixText;
		BroadcastMessage("UpdateAnchors", SendMessageOptions.DontRequireReceiver);
		chatInput.mInput.activeTextColor = color;
	}

	private void onFocus(bool focused)
	{
		if (currentChatMode == Com.CmdChat.Party && !PartyManager.IsInParty)
		{
			ChangeChannelGeneral();
		}
	}

	private void onInputChanged(string text)
	{
		if (text.Length >= 3 && text[0] == '/' && text[text.Length - 1] == ' ')
		{
			CheckChatState(text);
		}
	}

	private bool CheckChatState(string text)
	{
		string[] array = text.Substring(1, text.Length - 1).Trim().Split(' ');
		switch (array[0])
		{
		case "tell":
		case "t":
		case "whisper":
		case "w":
		{
			if (array.Length == 2 && !array[1].Contains("<"))
			{
				SetWhisper(array[1]);
				break;
			}
			string text2 = string.Join(" ", array.Skip(1).ToArray());
			if (Regex.Match(text2, "^<(.*?)>$").Success)
			{
				SetWhisper(text2.Substring(1, text2.Length - 2));
			}
			break;
		}
		case "reply":
		case "r":
			SetWhisper(pmFrom);
			break;
		case "say":
		case "s":
			SetGeneralMode();
			break;
		case "party":
		case "p":
			SetPartyMode();
			break;
		case "guild":
		case "g":
			SetGuildMode();
			break;
		default:
			return false;
		}
		return true;
	}

	public void PrintIgnoreList()
	{
		if (SettingsManager.ignoreList.Count == 0)
		{
			AddText("[FFFFFF]Your ignore list is empty.[-]");
			return;
		}
		AddText(InterfaceColors.Chat.White.ToBBCode() + "You are ignoring:[-]\n" + InterfaceColors.Chat.Light_Blue.ToBBCode() + string.Join("\n", SettingsManager.ignoreList.ToArray()) + "[-]");
	}

	public void OpenEmotes()
	{
		UIEmotes.Toggle();
	}

	public void ResubmitLastCommand()
	{
		OnChatSubmit(lastCommand);
	}

	public void OnChatSubmit(string msg)
	{
		cmdIndex = 0;
		if (msg == "")
		{
			return;
		}
		if (msg[0] == '/')
		{
			ProcessMultipleCommands(msg);
			return;
		}
		List<string> list = msg.Split('>').ToList();
		if (list.Count > 1)
		{
			string toName = list[0];
			string msg2 = list[1];
			SendWhisper(msg2, toName);
			return;
		}
		switch (currentChatMode)
		{
		case Com.CmdChat.Party:
			SendPartyChat(msg);
			break;
		case Com.CmdChat.Whisper:
			SendWhisper(msg, pmTo);
			break;
		case Com.CmdChat.Multi:
			SendPublicChatMessage(msg);
			break;
		case Com.CmdChat.Guild:
			SendGuildChat(msg);
			break;
		}
	}

	private void ProcessMultipleCommands(string msg)
	{
		if (Entities.Instance.me.AccessLevel < 50)
		{
			ProcessCommand(msg);
			return;
		}
		string item = msg.Substring(1).Split(' ').FirstOrDefault();
		if (ChatCommands.SingleCommands.Contains(item))
		{
			ProcessCommand(msg);
		}
		else
		{
			StartCoroutine(ProcessCommandsRoutine(msg));
		}
	}

	private IEnumerator ProcessCommandsRoutine(string msg)
	{
		string[] commandParts = msg.Split(new char[1] { '/' }, StringSplitOptions.RemoveEmptyEntries);
		if (commandParts.Length > 1)
		{
			AddCmdToHistory(msg);
		}
		string[] array = commandParts;
		foreach (string text in array)
		{
			string msg2 = "/" + text;
			ProcessCommand(msg2, commandParts.Length == 1);
			yield return new WaitForSecondsRealtime(0.3f);
		}
	}

	private void ProcessCommand(string msg, bool addToHistory = true)
	{
		if (!CheckChatState(msg))
		{
			string[] array = msg.Substring(1).Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			string cmd = array[0].ToLower();
			if (addToHistory)
			{
				AddCmdToHistory(msg);
			}
			Emote byCommand = Emotes.GetByCommand(cmd);
			if (Emotes.IsEmoteUnlocked(byCommand))
			{
				Game.Instance.SendEmoteRequest(byCommand.em);
			}
			else
			{
				ChatCommands.ProcessCommand(array.ToList());
			}
		}
	}

	private void SendPublicChatMessage(string msg)
	{
		RequestChat requestChat = new RequestChat();
		Channel channelByType = ChannelManager.Instance.GetChannelByType(curChType);
		if (channelByType != null)
		{
			requestChat.channelID = channelByType.id;
			requestChat.cmd = (byte)currentChatMode;
			requestChat.msg = msg;
			SendChatRequest(requestChat);
		}
	}

	private void SendPartyChat(string message)
	{
		if (!PartyManager.IsInParty)
		{
			ChangeChannelGeneral();
			return;
		}
		RequestChat requestChat = new RequestChat();
		requestChat.cmd = 3;
		requestChat.msg = message;
		SendChatRequest(requestChat);
	}

	private void SendGuildChat(string message)
	{
		RequestChat requestChat = new RequestChat();
		requestChat.cmd = 4;
		requestChat.msg = message;
		requestChat.guildID = Session.MyPlayerData.Guild.guildID;
		requestChat.sender = Entities.Instance.me.name;
		SendChatRequest(requestChat);
	}

	private void SendWhisper(string msg, string toName)
	{
		RequestChat requestChat = new RequestChat();
		requestChat.cmd = 2;
		requestChat.recipient = toName.ToLower();
		requestChat.msg = msg;
		SendChatRequest(requestChat);
	}

	private void SendChatRequest(RequestChat request)
	{
		if (Session.IsGuest)
		{
			Game.ShowGuestFeature();
		}
		else
		{
			Game.Instance.aec.sendRequest(request);
		}
	}

	private void ProcessChatResponse(ResponseChat response)
	{
		if (response.sender == "SERVER")
		{
			AddText(InterfaceColors.Chat.Red.ToBBCode() + response.msg + "[-]", FilterType.ServerMessage);
			return;
		}
		if (response.sender == "SERVER NOTIFICATION")
		{
			AddText(InterfaceColors.Chat.Yellow.ToBBCode() + response.msg + "[-]", FilterType.Notification);
			return;
		}
		response.msg = StripBBCode(response.msg).Trim();
		if (string.IsNullOrEmpty(response.msg))
		{
			return;
		}
		string text = response.msg;
		if ((bool)SettingsManager.IsChatFiltered)
		{
			ChatFilteredMessage chatFilteredMessage = chatFilter.ProfanityCheck(response.msg, shouldCleanSymbols: false);
			if (chatFilteredMessage.code > 0)
			{
				text = chatFilteredMessage.maskedMessage;
			}
		}
		if (response.cmd == 5)
		{
			Entities.Instance.GetNpcById(response.senderID)?.NamePlateBubble(Entities.Instance.me.ScrubText(response.msg));
		}
		else
		{
			if (SettingsManager.IsIgnoring(response.sender))
			{
				return;
			}
			Player playerByName = Entities.Instance.GetPlayerByName(response.sender);
			if (response.cmd == 1)
			{
				AddText(playerByName.GetNamePlateColor().ToBBCode() + response.sender + ": [-]" + InterfaceColors.Chat.Dark_White.ToBBCode() + text + "[-]");
				playerByName.NamePlateBubble(text);
			}
			else if (response.cmd == 3)
			{
				AddText(InterfaceColors.Chat.Party_Dark.ToBBCode() + response.sender + ":[-] " + InterfaceColors.Chat.Party.ToBBCode() + text + "[-]", FilterType.Party);
			}
			else if (response.cmd == 2)
			{
				string text2 = Entities.Instance.me.name;
				if (response.sender.ToLower() == text2.ToLower())
				{
					AddText(InterfaceColors.Chat.Dark_Whisper_Pink.ToBBCode() + "[To " + response.recipient + "]:[-] " + InterfaceColors.Chat.Whisper_Pink.ToBBCode() + text + "[-]", FilterType.Whisper);
					SetWhisper(response.recipient);
				}
				else
				{
					pmFrom = response.sender;
					AddText(InterfaceColors.Chat.Dark_Whisper_Pink.ToBBCode() + "[From " + pmFrom + "]:[-] " + InterfaceColors.Chat.Whisper_Pink.ToBBCode() + text + "[-]", FilterType.Whisper);
					AudioManager.Play2DSFX("Notif_Whisper");
				}
			}
			else if (response.cmd == 4)
			{
				AddText(InterfaceColors.Chat.Guild_Dark.ToBBCode() + response.sender + ":[-] " + InterfaceColors.Chat.Guild.ToBBCode() + text + "[-]", FilterType.Guild);
			}
		}
	}

	private static string StripBBCode(string text)
	{
		string text2 = text;
		while ((text = Regex.Replace(text, "\\[([^\\[]*?)\\]", string.Empty, RegexOptions.IgnoreCase)) != text2)
		{
			text2 = text;
		}
		text = Regex.Replace(text, "(\\r\\n|\\n\\r|\\r|\\n)+", " ", RegexOptions.IgnoreCase);
		return text;
	}

	public void AddText(string text, FilterType filter = FilterType.General)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		ChatMessage message = new ChatMessage(filter, text);
		AddChatMessageToHistory(FilterType.General, message);
		if (filter != 0)
		{
			AddChatMessageToHistory(filter, message);
		}
		if (currentFilter == FilterType.General || currentFilter == filter)
		{
			if (showTimestamp)
			{
				chatText.Add(AddTimestamp(message));
			}
			else
			{
				chatText.Add(text);
			}
		}
	}

	private string AddTimestamp(ChatMessage message)
	{
		string text = GetColorByFilter(message.Filter).ToBBCode();
		return message.Message.Insert(0, text + "[" + message.recievedTime.ToString("hh:mm tt") + "][-] ");
	}

	private void AddChatMessageToHistory(FilterType filter, ChatMessage message)
	{
		while (ChatHistory[filter].Count > 30)
		{
			ChatHistory[filter].RemoveAt(0);
		}
		ChatHistory[filter].Add(message);
	}

	private void AddCmdToHistory(string cmd)
	{
		while (cmdHistory.Contains(cmd))
		{
			cmdHistory.Remove(cmd);
		}
		while (cmdHistory.Count > 30)
		{
			cmdHistory.RemoveAt(0);
		}
		cmdHistory.Add(cmd);
	}

	public void Close()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public static void AddMessage(string msg)
	{
		if (Instance != null)
		{
			Instance.AddText(msg);
		}
	}

	public static void Notify(string msg, string color = "[FFB71C]", FilterType filter = FilterType.Notification)
	{
		if (Instance != null && !string.IsNullOrEmpty(msg))
		{
			Instance.AddText(color + msg + "[-]", filter);
		}
	}

	public static void SendAdminMessage(string msg, string color = "[FF0000]", FilterType filter = FilterType.Notification)
	{
		Entity me = Entities.Instance.me;
		if (me.AccessLevel == 100 || me.AccessLevel == 101 || me.AccessLevel == 50)
		{
			Notify(msg, color, filter);
		}
	}

	private void Update()
	{
		if (chatInput.IsSelected && cmdHistory.Count > 0)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				cmdIndex = (cmdIndex - 1 + cmdHistory.Count) % cmdHistory.Count;
				chatInput.mInput.value = cmdHistory[cmdIndex];
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				cmdIndex = (cmdIndex + 1) % cmdHistory.Count;
				chatInput.mInput.value = cmdHistory[cmdIndex];
			}
		}
	}

	private void RefreshChat()
	{
		chatText.Clear();
		foreach (ChatMessage item in ChatHistory[currentFilter])
		{
			if (showTimestamp)
			{
				chatText.Add(AddTimestamp(item));
			}
			else
			{
				chatText.Add(item.Message);
			}
		}
	}

	private void ToggleTimestamp(bool isEnabled)
	{
		showTimestamp = isEnabled;
		RefreshChat();
	}

	private void ChangeChatBGOpacity(float newValue)
	{
		chatBG.color = new Color(chatBG.color.r, chatBG.color.g, chatBG.color.b, newValue);
	}

	private void SetOpacityFromPrefs()
	{
		chatBG.color = new Color(chatBG.color.r, chatBG.color.g, chatBG.color.b, SettingsManager.ChatBackgroundOpacity);
	}

	public void TurnOnGuild()
	{
		toggleGuild.gameObject.SetActive(Session.MyPlayerData.IsInGuild);
		if (!Session.MyPlayerData.IsInGuild)
		{
			SetChatFilter(FilterType.General);
		}
		tabs.Reposition();
	}

	private void OnClick()
	{
		int characterIndexAtPosition = lblChatText.GetCharacterIndexAtPosition(UICamera.currentRay.origin, precise: false);
		if (characterIndexAtPosition <= 0)
		{
			return;
		}
		string text = lblChatText.text;
		int num = text.LastIndexOf("[FFFFFF]", characterIndexAtPosition) + 8;
		int num2 = text.IndexOf(':', characterIndexAtPosition);
		if (num2 == -1)
		{
			num2 = text.Length;
		}
		if (num < num2)
		{
			string text2 = text.Substring(num, num2 - num);
			if (!text2.Contains(":") && !text2.Contains("["))
			{
				SetWhisper(text2);
				Focus();
			}
		}
	}
}
