using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.Housing;
using Assets.Scripts.NetworkClient.CommClasses;
using UnityEngine;

public static class ChatCommands
{
	[Flags]
	public enum Role
	{
		None = 0,
		Player = 1,
		Tester = 2,
		WhiteHat = 4,
		Mod = 8,
		Admin = 0x10,
		SuperAdmin = 0x20,
		Everyone = 0x3F,
		TesterPlus = 0x3E,
		ModPlus = 0x38,
		AdminPlus = 0x30,
		TesterAndAdmin = 0x36
	}

	private static readonly List<ChatCommand> ClientCommands = new List<ChatCommand>
	{
		new ChatCommand("apop", "Open the specified apop", Role.AdminPlus, OpenApop, null, new List<ChatParam>
		{
			new ChatParam("apopID", "7377")
		}),
		new ChatCommand("houseitems", "Shows house items UI", Role.AdminPlus, ShowHouseItems),
		new ChatCommand("joinhouse", "Joins a house with a specified house ID", Role.AdminPlus, JoinHouse),
		new ChatCommand("clearhouse", "Clear house", Role.AdminPlus, ClearHouse),
		new ChatCommand("bank", "Open bank", Role.TesterPlus, OpenBank),
		new ChatCommand("cancelanim", "Cancel animations", Role.TesterPlus, CancelAnimation),
		new ChatCommand("chaterror", "Turn chat errors on or off", Role.TesterPlus, ToggleChatError, null, new List<ChatParam>
		{
			new ChatParam("on OR off", "off")
		}),
		new ChatCommand("cinematic", "Load specified cinematic", Role.TesterAndAdmin, ShowCinematic, null, new List<ChatParam>
		{
			new ChatParam("cinematicName", "DeathIntro01")
		}),
		new ChatCommand("clear", "Clear all chat window text", Role.Everyone, ClearChat, new List<string> { "cls" }),
		new ChatCommand("clearprefs", "Delete all Unity PlayerPrefs values", Role.TesterAndAdmin, ClearPrefs),
		new ChatCommand("clearpromo", "Reset news popup timer", Role.TesterAndAdmin, ClearPromo),
		new ChatCommand("customize", "Open the barber shop menu", Role.TesterAndAdmin, Customize, new List<string> { "barber" }),
		new ChatCommand("devmode", "Enter the developer mode for editing anything accessible via admin.", Role.AdminPlus, ActivateDevMode, new List<string> { "devon", "devoff", "dev" }),
		new ChatCommand("dialog", "Open the specified dialog", Role.TesterAndAdmin, Dialog, null, new List<ChatParam>
		{
			new ChatParam("dialogID", "10"),
			new ChatParam("skipCompleteAction", "true", isOptional: true, "false")
		}),
		new ChatCommand("disableautologin", "Disable logging into the server automatically", Role.TesterPlus, DisableAutoLogin),
		new ChatCommand("disconnect", "Disconnect yourself from the server", Role.Everyone, Disconnect),
		new ChatCommand("duel", "Request a duel", Role.Everyone, Duel, null, new List<ChatParam>
		{
			new ChatParam("playerName", "RabbleFroth")
		}),
		new ChatCommand("duelaccept", "Accept pending duel", Role.Everyone, DuelAccept, new List<string> { "acceptduel" }),
		new ChatCommand("forfeit", "Forfeit your current duel", Role.Everyone, DuelForfeit, new List<string> { "ff", "forfeit" }),
		new ChatCommand("friend", "Send a friend request", Role.Everyone, Friend, null, new List<ChatParam>
		{
			new ChatParam("playerName", "RabbleFroth")
		}),
		new ChatCommand("fullscreen", "Toggle fullscreen mode", Role.Everyone, FullScreen),
		new ChatCommand("genderswap", "Open gender swap UI", Role.TesterAndAdmin, GenderSwap),
		new ChatCommand("getanimationnames", "Print animations from targeted entity's animator", Role.SuperAdmin, GetAnimationNames, new List<string> { "getanims" }),
		new ChatCommand("getmyanimationnames", "Print animations from current animator on self", Role.SuperAdmin, GetMyAnimationNames, new List<string> { "getmyanims" }),
		new ChatCommand("greenscreendistance", "Turn on greenscreen at specified distance", Role.SuperAdmin, GreenScreenDistance, new List<string> { "greenscreen" }, new List<ChatParam>
		{
			new ChatParam("distance", "10")
		}),
		new ChatCommand("dbg", "Create dialogue bg sphere", Role.SuperAdmin, CreateDialogueBg, new List<string>(), new List<ChatParam>
		{
			new ChatParam("imageFormat", "jpg", isOptional: true, "jpg")
		}),
		new ChatCommand("help", "Get info about chat commands", Role.TesterPlus, CommandHelp, new List<string> { "?" }, new List<ChatParam>
		{
			new ChatParam("command", "spawn", isOptional: true, "Show all commands")
		}),
		new ChatCommand("ignore", "Ignore specified player", Role.Everyone, Ignore, null, new List<ChatParam>
		{
			new ChatParam("playerName", "RabbleFroth")
		}),
		new ChatCommand("ignorelist", "Print list of ignored players", Role.Everyone, IgnoreList),
		new ChatCommand("invite", "Invite player to party", Role.Everyone, PartyInvite, new List<string> { "partyinvite" }, new List<ChatParam>
		{
			new ChatParam("playerName", "RabbleFroth")
		}),
		new ChatCommand("join", "Travel to a specified map", Role.Everyone, AreaJoin, null, new List<ChatParam>
		{
			new ChatParam("mapID OR joinName OR displayName", "yulgar's inn")
		}),
		new ChatCommand("leaveparty", "Leave current party", Role.Everyone, PartyLeave, new List<string> { "partyleave", "leave" }),
		new ChatCommand("lootbox", "Open lootbox menu", Role.AdminPlus, Lootbox),
		new ChatCommand("macro", "Assign macros to ctrl+shift+[macroSlot]", Role.TesterAndAdmin, Macro, null, new List<ChatParam>
		{
			new ChatParam("macroSlot", "1", isOptional: true, "show all macros"),
			new ChatParam("command", "/spawn frogzard 20 neutral", isOptional: true, "show macro in macroSlot")
		}),
		new ChatCommand("resetmacros", "Reset all macros to default", Role.TesterAndAdmin, ResetMacros),
		new ChatCommand("mergeshop", "Loads specified merge shop", Role.AdminPlus, MergeShop, null, new List<ChatParam>
		{
			new ChatParam("mergeShopID", "4")
		}),
		new ChatCommand("movetome", "Move targeted entity to your position", Role.SuperAdmin, MoveToMe),
		new ChatCommand("openlootbox", "Open specified loot box", Role.AdminPlus, OpenLootBox, null, new List<ChatParam>
		{
			new ChatParam("lootBoxID", "1200")
		}),
		new ChatCommand("partydecline", "Decline current party invite", Role.Everyone, PartyDecline),
		new ChatCommand("partyjoin", "Accept current party invite", Role.Everyone, PartyJoin),
		new ChatCommand("partykick", "Kick player from party by name", Role.Everyone, PartyKick, null, new List<ChatParam>
		{
			new ChatParam("playerName", "RabbleFroth")
		}),
		new ChatCommand("partypromote", "Promote player to party leader by name", Role.Everyone, PartyPromote, new List<string> { "promote" }, new List<ChatParam>
		{
			new ChatParam("playerName", "RabbleFroth")
		}),
		new ChatCommand("report", "Report specified or targeted player", Role.Everyone, Report, null, new List<ChatParam>
		{
			new ChatParam("playerName", "RabbleFroth", isOptional: true, "targeted player")
		}),
		new ChatCommand("resetmyanimations", "Reset the animator controller. Character will T-pose.", Role.SuperAdmin, ResetMyAnimations, new List<string> { "resetmyanims" }),
		new ChatCommand("setanimation", "Set animation for targeted entity", Role.SuperAdmin, SetAnimation, new List<string> { "setanim" }, new List<ChatParam>
		{
			new ChatParam("animationName", "GroundPound"),
			new ChatParam("frameNumber", "5"),
			new ChatParam("animSpeed", "0.25")
		}),
		new ChatCommand("setallanimation", "Set animation for targeted entity", Role.SuperAdmin, SetAllAnimations, new List<string> { "setanim" }, new List<ChatParam>
		{
			new ChatParam("animationName", "GroundPound"),
			new ChatParam("frameNumber", "5"),
			new ChatParam("animSpeed", "0.25")
		}),
		new ChatCommand("setmyanimation", "Set animation on self", Role.SuperAdmin, SetMyAnimation, new List<string> { "setmyanim" }, new List<ChatParam>
		{
			new ChatParam("animationName", "GroundPound"),
			new ChatParam("frameNumber", "5"),
			new ChatParam("animSpeed", "0.25")
		}),
		new ChatCommand("setentityface", "Set targeted entity's face offset", Role.SuperAdmin, SetEntityFace, null, new List<ChatParam>
		{
			new ChatParam("x", "1"),
			new ChatParam("y", "1"),
			new ChatParam("z", "1"),
			new ChatParam("w", "1")
		}),
		new ChatCommand("setmyface", "Set your character's face offset", Role.SuperAdmin, SetMyFace, null, new List<ChatParam>
		{
			new ChatParam("x", "1"),
			new ChatParam("y", "1"),
			new ChatParam("z", "1"),
			new ChatParam("w", "1")
		}),
		new ChatCommand("shop", "Load specified shop", Role.AdminPlus, OpenShop, null, new List<ChatParam>
		{
			new ChatParam("shopID", "12")
		}),
		new ChatCommand("spell", "Cast any spell", Role.AdminPlus, Spell, null, new List<ChatParam>
		{
			new ChatParam("spellID", "10")
		}),
		new ChatCommand("unignore", "Remove a player from your ignore list", Role.Everyone, Unignore, null, new List<ChatParam>
		{
			new ChatParam("playerName", "RabbleFroth")
		}),
		new ChatCommand("votekick", "Initiate a vote kick against a player in your party", Role.Everyone, ProcessVoteKick, null, new List<ChatParam>
		{
			new ChatParam("playerName", "RabbleFroth")
		}),
		new ChatCommand("createguild", "Create a guild", Role.SuperAdmin, CreateGuild, null, new List<ChatParam>
		{
			new ChatParam("guildName", "Greatest of all Time"),
			new ChatParam("guildTag", "GOAT")
		}),
		new ChatCommand("joinguild", "Join a guild", Role.SuperAdmin, JoinGuild, null, new List<ChatParam>
		{
			new ChatParam("??", "??"),
			new ChatParam("??", "??")
		}),
		new ChatCommand("cheat", "Turn on Cheats", Role.Everyone, CheatMode),
		new ChatCommand("qsinfo", "View Index and max Value of quests here", Role.AdminPlus, QSInfo),
		new ChatCommand("mapinfo", "Returns map ID, name, and current cell", Role.AdminPlus, MapInfo),
		new ChatCommand("leaveguild", "Leave current guild", Role.SuperAdmin, LeaveGuild),
		new ChatCommand("changerole", "Change guild role of a member", Role.SuperAdmin, ChangeGuildRole, new List<string> { "changeguildrole" }, new List<ChatParam>
		{
			new ChatParam("memberID", "5"),
			new ChatParam("newGuildRole", "1")
		}),
		new ChatCommand("switchserver", "Switch what server you are connected to", Role.ModPlus, SwitchServer, null, new List<ChatParam>
		{
			new ChatParam("server name", "red dragon")
		}),
		new ChatCommand("queue", "Join the specified queue", Role.Everyone, JoinQueue, new List<string> { "joinqueue", "joinq" }, new List<ChatParam>
		{
			new ChatParam("queueID", "1")
		}),
		new ChatCommand("leavequeue", "Leave current queue", Role.Everyone, LeaveQueue, new List<string> { "leaveq" }),
		new ChatCommand("victory", "Show Victory Screen", Role.SuperAdmin, ShowVictory),
		new ChatCommand("defeat", "Show Defeat Screen", Role.SuperAdmin, ShowDefeat),
		new ChatCommand("spawneditor", "Spawn Editor Tool", Role.AdminPlus, NPCEditor, new List<string> { "se", "seditor", "spawne", "spawns" }),
		new ChatCommand("temporarymapeditor", "Temporary Map Editor Tool", Role.AdminPlus, MapEditor, new List<string> { "me", "tme", "tmeditor", "tmape", "tmapeditor", "doors" }),
		new ChatCommand("buyguildpower", "Buy Guild Power", Role.Everyone, BuyGuildPower, new List<string> { "buyguildpower", "bgp" })
	};

	private static List<ChatCommand> ServerCommands = new List<ChatCommand>();

	public static readonly List<string> SingleCommands = new List<string> { "help", "macro", "adminyell", "notify", "notifyall", "yell" };

	public static bool AreAllCommandsLoaded => ServerCommands.Count > 0;

	public static void RunCommand(string msg)
	{
		ProcessCommand(msg.Substring(1).Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList());
	}

	public static void ProcessCommand(List<string> args)
	{
		string text = args[0].ToLower();
		ChatCommand command = GetCommand(text, Entities.Instance.me.AccessLevel);
		if (command == null)
		{
			SendServerCommand(args);
		}
		else if (args.Count - 1 < command.ArgsRequired)
		{
			Chat.AddMessage(InterfaceColors.Chat.Red.ToBBCode() + "\nMissing /" + text + " arguments. Correct use:[-]");
			ShowHelp(command);
		}
		else
		{
			ChatActionParams obj = new ChatActionParams(args);
			command.method(obj);
		}
	}

	public static List<ChatCommand> GetAllCommands()
	{
		return (from c in ClientCommands.Where((ChatCommand c) => HasPermission(Entities.Instance.me.AccessLevel, c.permission)).ToList().Concat(ServerCommands)
			orderby c.command
			select c).ToList();
	}

	public static void LoadServerCommands()
	{
		Game.Instance.SendGetChatCommandsRequest();
	}

	public static void SetServerCommands(List<ChatCommand> serverCommands)
	{
		ServerCommands = serverCommands;
		UIChatCommands.OnCommandsLoaded();
	}

	private static ChatCommand GetCommand(string strCommand, int playerAccessLevel)
	{
		return ClientCommands.FirstOrDefault((ChatCommand cmd) => (cmd.command == strCommand || cmd.aliases.Contains(strCommand)) && HasPermission(playerAccessLevel, cmd.permission));
	}

	private static bool HasPermission(int playerAccessLevel, Role permission)
	{
		if ((permission & Role.Player) > Role.None)
		{
			return playerAccessLevel >= 0;
		}
		if ((permission & Role.Tester) > Role.None)
		{
			return playerAccessLevel >= 50;
		}
		if ((permission & Role.WhiteHat) > Role.None)
		{
			return playerAccessLevel >= 55;
		}
		if ((permission & Role.Mod) > Role.None)
		{
			return playerAccessLevel >= 60;
		}
		if ((permission & Role.Admin) > Role.None)
		{
			return playerAccessLevel >= 100;
		}
		if ((permission & Role.SuperAdmin) > Role.None)
		{
			return playerAccessLevel >= 101;
		}
		return false;
	}

	private static void SendServerCommand(List<string> args)
	{
		if (args.Count > 0)
		{
			if (Entities.Instance.me.AccessLevel == 60)
			{
				SendModeratorServerCommand(args);
			}
			else
			{
				SendTesterServerCommand(args);
			}
		}
	}

	private static void SendTesterServerCommand(List<string> args)
	{
		RequestCmd requestCmd = new RequestCmd();
		requestCmd.cmd = 1;
		requestCmd.args = new List<string>(args);
		if (Session.MyPlayerData.HasCurrentlyTrackedQuest)
		{
			Quest currentlyTrackedQuest = Session.MyPlayerData.CurrentlyTrackedQuest;
			foreach (QuestObjective objective in currentlyTrackedQuest.Objectives)
			{
				if (Session.MyPlayerData.GetQuestObjectiveProgress(objective) < objective.Qty)
				{
					requestCmd.questID = currentlyTrackedQuest.ID;
					requestCmd.objectiveID = objective.ID;
					break;
				}
			}
		}
		Entity target = Entities.Instance.me.Target;
		if (target != null)
		{
			requestCmd.eID = target.ID;
			requestCmd.etype = target.type;
		}
		Game.Instance.aec.sendRequest(requestCmd);
	}

	private static void SendModeratorServerCommand(List<string> args)
	{
		RequestCmd requestCmd = new RequestCmd();
		requestCmd.cmd = 1;
		requestCmd.args = new List<string>(args);
		Entity target = Entities.Instance.me.Target;
		if (target != null)
		{
			requestCmd.eID = target.ID;
			requestCmd.etype = target.type;
		}
		Game.Instance.aec.sendRequest(requestCmd);
	}

	private static string GetCharName(string input)
	{
		if (Regex.Match(input, "^<(.*?)>$").Success)
		{
			return input.Substring(1, input.Length - 2);
		}
		return input;
	}

	private static void CommandHelp(ChatActionParams ap)
	{
		if (ap.args.Count < 2)
		{
			UIChatCommands.Show();
			return;
		}
		ChatCommand command = GetCommand(ap.args[1].ToLower(), Entities.Instance.me.AccessLevel);
		if (command == null)
		{
			RequestCmd requestCmd = new RequestCmd();
			requestCmd.cmd = 1;
			requestCmd.args = new List<string>(ap.args);
			Game.Instance.aec.sendRequest(requestCmd);
		}
		else
		{
			ShowHelp(command);
		}
	}

	public static void ShowHelp(ChatCommand command)
	{
		Chat.AddMessage("\n" + command.HelpMessage);
	}

	private static void GetAnimationNames(ChatActionParams _)
	{
		AdamTools.GetEntityAnimationNames();
	}

	private static void GetMyAnimationNames(ChatActionParams _)
	{
		AdamTools.GetMyAnimationNames();
	}

	private static void SetAnimation(ChatActionParams ap)
	{
		if (ap.args.Count == 4 && float.TryParse(ap.args[3], out var result) && float.TryParse(ap.args[2], out var result2))
		{
			AdamTools.SetEntityAnimation(ap.args[1], result2, result);
		}
	}

	private static void SetAllAnimations(ChatActionParams ap)
	{
		if (ap.args.Count == 4 && float.TryParse(ap.args[3], out var result) && float.TryParse(ap.args[2], out var result2))
		{
			AdamTools.SetAllEntityAnimations(ap.args[1], result2, result);
		}
	}

	private static void SetMyAnimation(ChatActionParams ap)
	{
		if (ap.args.Count == 4 && float.TryParse(ap.args[3], out var result) && float.TryParse(ap.args[2], out var result2))
		{
			AdamTools.SetMyAnimation(ap.args[1], result2, result);
		}
	}

	private static void ResetMyAnimations(ChatActionParams _)
	{
		AdamTools.ResetMyController();
	}

	private static void SetEntityFace(ChatActionParams ap)
	{
		if (ap.args.Count >= 5)
		{
			Vector4 zero = Vector4.zero;
			float.TryParse(ap.args[1], out zero.x);
			float.TryParse(ap.args[2], out zero.y);
			float.TryParse(ap.args[3], out zero.z);
			float.TryParse(ap.args[4], out zero.w);
			AdamTools.SetEntityFace(zero);
		}
	}

	private static void SetMyFace(ChatActionParams ap)
	{
		if (ap.args.Count >= 5)
		{
			Vector4 zero = Vector4.zero;
			float.TryParse(ap.args[1], out zero.x);
			float.TryParse(ap.args[2], out zero.y);
			float.TryParse(ap.args[3], out zero.z);
			float.TryParse(ap.args[4], out zero.w);
			AdamTools.SetMyFace(zero);
		}
	}

	private static void CancelAnimation(ChatActionParams ap)
	{
		Entities.Instance.me.entitycontroller.CancelAction();
	}

	private static void GreenScreenDistance(ChatActionParams ap)
	{
		if (ap.args.Count >= 2 && float.TryParse(ap.args[1], out var result))
		{
			AdamTools.GreenScreenDistance(result);
		}
	}

	private static void CreateDialogueBg(ChatActionParams ap)
	{
		string text = "jpg";
		if (ap.args.Count > 1)
		{
			text = ap.args[1];
		}
		AdamTools.CreateDialogueBg(text);
		Chat.AddMessage("Screenshot taken using " + text + " format!");
	}

	private static void MoveToMe(ChatActionParams _)
	{
		AdamTools.MoveEntity();
	}

	private static void OpenBank(ChatActionParams _)
	{
		UIBankManager.Show();
	}

	private static void ToggleChatError(ChatActionParams ap)
	{
		if (ap.args.Count >= 2)
		{
			if (ap.args[1].ToLower() == "on")
			{
				PlayerPrefs.SetInt("CHATERROR", 1);
				PlayerPrefs.Save();
				Chat.Notify("Chat Errors Enabled!", InterfaceColors.Chat.Red.ToBBCode());
			}
			else if (ap.args[1].ToLower() == "off")
			{
				PlayerPrefs.SetInt("CHATERROR", 0);
				PlayerPrefs.Save();
				Chat.Notify("Chat Errors Disabled!", InterfaceColors.Chat.Red.ToBBCode());
			}
		}
	}

	private static void ShowCinematic(ChatActionParams ap)
	{
		if (ap.args.Count >= 2)
		{
			DialogueSlotManager.Show(ap.args[1]);
		}
	}

	private static void ClearChat(ChatActionParams _)
	{
		Chat.Instance.ClearChat();
	}

	private static void ClearPrefs(ChatActionParams _)
	{
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
	}

	private static void ClearPromo(ChatActionParams _)
	{
		PlayerPrefs.DeleteKey("PromosLastShown");
		PlayerPrefs.Save();
	}

	private static void Customize(ChatActionParams _)
	{
		UICharCustomize.Show();
	}

	private static void Dialog(ChatActionParams ap)
	{
		if (ap.args.Count >= 2 && int.TryParse(ap.args[1], out var result))
		{
			bool skipCompleteAction = ap.args.Count >= 3 && ap.args[2].ToLower() == "false";
			DialogueSlotManager.Show(result, null, skipCompleteAction);
		}
	}

	private static void DisableAutoLogin(ChatActionParams _)
	{
		PlayerPrefs.SetInt("DisableAutoLogin", 1);
		PlayerPrefs.Save();
	}

	private static void Disconnect(ChatActionParams _)
	{
		AEC.getInstance().Disconnect();
	}

	private static void Duel(ChatActionParams ap)
	{
		if (ap.args.Count > 1 && !string.IsNullOrEmpty(ap.args[1]))
		{
			Game.Instance.SendPvPDuelChallengeRequest(ap.args[1]);
			return;
		}
		Entity target = Entities.Instance.me.Target;
		if (target != null && target.type == Entity.Type.Player)
		{
			Game.Instance.SendPvPDuelChallengeRequest(target.name);
		}
	}

	private static void DuelAccept(ChatActionParams ap)
	{
		if (ap.args.Count >= 2 && int.TryParse(ap.args[1], out var result))
		{
			Game.Instance.SendPvPDuelAcceptRequest(result, accept: true);
		}
	}

	private static void DuelForfeit(ChatActionParams _)
	{
		if (Entities.Instance.me.DuelOpponentID != -1)
		{
			Game.Instance.SendDuelForfeitRequest();
		}
	}

	private static void Friend(ChatActionParams ap)
	{
		if (!Session.MyPlayerData.CanAddMoreFriends)
		{
			Chat.Notify("You can only have have up to " + 80 + " friends.", InterfaceColors.Chat.Red.ToBBCode());
			return;
		}
		string name = GetCharName(string.Join(" ", ap.args.Skip(1).ToArray())).ToLower();
		Game.Instance.SendFriendRequest(name);
	}

	private static void FullScreen(ChatActionParams _)
	{
		Screen.fullScreen = !Screen.fullScreen;
	}

	private static void GenderSwap(ChatActionParams _)
	{
		UICharGender.Show();
	}

	private static void Ignore(ChatActionParams ap)
	{
		if (ap.args.Count > 1)
		{
			Game.Instance.Ignore(GetCharName(string.Join(" ", ap.args.Skip(1).ToArray())).ToLower());
		}
	}

	private static void IgnoreList(ChatActionParams _)
	{
		Chat.Instance.PrintIgnoreList();
	}

	private static void PartyInvite(ChatActionParams ap)
	{
		if (PartyManager.IsInParty && !PartyManager.IsLeader)
		{
			Chat.Notify("Only party leaders can send invites.");
		}
		else
		{
			Game.Instance.SendPartyInviteRequest(GetCharName(string.Join(" ", ap.args.Skip(1).ToArray())).ToLower());
		}
	}

	private static void PartyJoin(ChatActionParams _)
	{
		if (PartyManager.PartyInvite != null)
		{
			int leaderID = PartyManager.PartyInvite.LeaderID;
			Game.Instance.SendPartyJoinRequest(leaderID);
		}
	}

	private static void PartyDecline(ChatActionParams _)
	{
		if (PartyManager.PartyInvite != null)
		{
			int leaderID = PartyManager.PartyInvite.LeaderID;
			Game.Instance.SendPartyJoinRequest(leaderID, accept: false);
			PartyManager.RemovePartyInvite();
		}
	}

	private static void PartyKick(ChatActionParams ap)
	{
		int memberIDByName = PartyManager.GetMemberIDByName(GetCharName(string.Join(" ", ap.args.Skip(1).ToArray())).ToLower());
		if (memberIDByName > 0)
		{
			Game.Instance.SendPartyRemoveRequest(memberIDByName);
		}
	}

	private static void PartyLeave(ChatActionParams _)
	{
		Game.Instance.SendPartyRemoveRequest(Entities.Instance.me.ID);
	}

	private static void PartyPromote(ChatActionParams ap)
	{
		int memberIDByName = PartyManager.GetMemberIDByName(GetCharName(string.Join(" ", ap.args.Skip(1).ToArray())).ToLower());
		if (memberIDByName > 0)
		{
			Game.Instance.SendPartyPromoteRequest(memberIDByName);
		}
	}

	private static void AreaJoin(ChatActionParams ap)
	{
		if (ap.args.Count > 1)
		{
			ap.args.RemoveAt(0);
			Game.Instance.SendAreaJoinCommand(string.Join(" ", ap.args));
		}
	}

	private static void Lootbox(ChatActionParams _)
	{
		UIPreviewLoot.LoadShop();
	}

	private static void Macro(ChatActionParams ap)
	{
		if (ap.args.Count < 2 || !int.TryParse(ap.args[1], out var result))
		{
			ShowMacros();
			return;
		}
		if (ap.args.Count < 3)
		{
			Chat.AddMessage(GetMacroMessage(result));
			return;
		}
		ap.args.RemoveRange(0, 2);
		string macroText = string.Join(" ", ap.args);
		SettingsManager.SaveMacro(result, macroText);
		Chat.AddMessage("Updated " + GetMacroMessage(result));
	}

	private static void ShowMacros()
	{
		string text = "";
		bool flag = false;
		for (int num = 1; num < SettingsManager.MacroCount; num = ((num != SettingsManager.MacroCount - 1) ? (num + 1) : 0))
		{
			if (!string.IsNullOrEmpty(SettingsManager.GetMacroText(num)))
			{
				text = text + "\n" + GetMacroMessage(num);
				flag = true;
			}
			if (num == 0)
			{
				break;
			}
		}
		if (!flag)
		{
			text += "\nNo macros currently set.";
			text += "\nAssign macro with /macro <slot> <text>";
			text += "\nUse macros with ctrl+shift+<slot>";
		}
		Chat.AddMessage(text);
	}

	private static string GetMacroMessage(int macroSlot)
	{
		return "Macro " + macroSlot + ": " + SettingsManager.GetMacroText(macroSlot);
	}

	private static void ResetMacros(ChatActionParams _)
	{
		SettingsManager.ResetAllMacros();
		Chat.AddMessage("All macros reset to defaults");
	}

	private static void MergeShop(ChatActionParams ap)
	{
		if (ap.args.Count >= 2)
		{
			UIMerge.Load(int.Parse(ap.args[1]));
		}
	}

	private static void OpenLootBox(ChatActionParams ap)
	{
		if (ap.args.Count >= 2)
		{
			RequestOpenLootBox requestOpenLootBox = new RequestOpenLootBox();
			requestOpenLootBox.ItemID = int.Parse(ap.args[1]);
			Game.Instance.aec.sendRequest(requestOpenLootBox);
		}
	}

	private static void Report(ChatActionParams ap)
	{
		if (ap.args.Count >= 2)
		{
			UICharReport.Show(GetCharName(string.Join(" ", ap.args.Skip(1).ToArray())));
		}
	}

	private static void ClearHouse(ChatActionParams ap)
	{
		HousingManager.houseInstance.RequestClearAll();
	}

	private static void JoinHouse(ChatActionParams ap)
	{
		if (ap.args.Count >= 2)
		{
			int houseID = int.Parse(ap.args[1]);
			HouseData houseData = new HouseData();
			houseData.HouseID = houseID;
			AEC.getInstance().sendRequest(new RequestHouseJoin(houseData));
		}
	}

	private static void ShowHouseItems(ChatActionParams ap)
	{
		UIHouseItemList.Show();
	}

	private static void OpenApop(ChatActionParams ap)
	{
		if (ap.args.Count >= 2)
		{
			ApopViewer.Show(int.Parse(ap.args[1]));
		}
	}

	private static void OpenShop(ChatActionParams ap)
	{
		if (ap.args.Count >= 2)
		{
			UIShop.LoadShop(int.Parse(ap.args[1]));
		}
	}

	private static void Unignore(ChatActionParams ap)
	{
		if (ap.args.Count >= 2)
		{
			Game.Instance.UnIgnore(GetCharName(string.Join(" ", ap.args.Skip(1).ToArray())).ToLower());
		}
	}

	private static void ProcessVoteKick(ChatActionParams ap)
	{
		if (!Game.Instance.AreaData.isDungeon || PartyManager.IsPrivate)
		{
			Chat.Notify("Vote kick can only be used in a public dungeon.", InterfaceColors.Chat.Yellow.ToBBCode());
		}
		else if (ap.args.Count >= 2)
		{
			string text = string.Join(" ", ap.args.Skip(1)).ToLower();
			if (text == Entities.Instance.me.name.ToLower())
			{
				Chat.Notify("You cannot initiate a vote kick against yourself.", InterfaceColors.Chat.Yellow.ToBBCode());
			}
			else if (Entities.Instance.GetPlayerByName(text) == null)
			{
				Chat.Notify("The user does not exist in this area.", InterfaceColors.Chat.Yellow.ToBBCode());
			}
			else
			{
				Game.Instance.SendVoteKickCommand(text);
			}
		}
		else
		{
			Entity target = Entities.Instance.me.Target;
			if (target != null && target.type == Entity.Type.Player)
			{
				Game.Instance.SendVoteKickCommand(Entities.Instance.me.Target.name.ToLower());
			}
		}
	}

	private static void CreateGuild(ChatActionParams ap)
	{
		if (ap.args.Count >= 3)
		{
			Game.Instance.SendCreateGuildRequest(ap.args[1], ap.args[2]);
		}
	}

	private static void JoinGuild(ChatActionParams ap)
	{
		if (ap.args.Count >= 3 && int.TryParse(ap.args[1], out var result) && byte.TryParse(ap.args[2], out var result2))
		{
			if (result2 == 0)
			{
				Chat.AddMessage("Can't be a leader for an existing guild. Change role and send request again.");
			}
			else
			{
				Game.Instance.SendJoinGuildRequest(result, result2, 0);
			}
		}
	}

	private static void LeaveGuild(ChatActionParams _)
	{
		Game.Instance.SendLeaveGuildRequest();
	}

	private static void DevMode(ChatActionParams _)
	{
		Game.Instance.DeveloperModeToggle();
	}

	private static void CheatMode(ChatActionParams _)
	{
		Game.Instance.ToggleCheat();
	}

	private static void QSInfo(ChatActionParams _)
	{
		Session.MyPlayerData.GetQSValue();
	}

	private static void MapInfo(ChatActionParams _)
	{
		Session.MyPlayerData.GetMapInfo();
	}

	private static void ChangeGuildRole(ChatActionParams ap)
	{
		if (ap.args.Count >= 3 && int.TryParse(ap.args[1], out var result) && byte.TryParse(ap.args[2], out var result2))
		{
			if (result2 == 0)
			{
				Chat.AddMessage("Can't make a member a leader.");
			}
			else
			{
				Game.Instance.SendGuildChangeRoleRequest(Session.MyPlayerData.ID, result, Session.MyPlayerData.Guild.guildID, result2);
			}
		}
	}

	private static void SwitchServer(ChatActionParams ap)
	{
		ServerInfo serverInfo = null;
		if (ap.args.Count > 1)
		{
			if (int.TryParse(ap.args[1], out var idinput))
			{
				serverInfo = ServerInfo.Servers.FirstOrDefault((ServerInfo x) => x.ID == idinput || (idinput == -1 && x.Name == "Localhost"));
			}
			else
			{
				string input = string.Join(" ", ap.args.ToArray(), 1, ap.args.Count - 1);
				serverInfo = ServerInfo.Servers.FirstOrDefault((ServerInfo x) => x.Name.ToLower() == input || x.Name.ToLower().Contains(input));
			}
		}
		if (serverInfo == null)
		{
			Chat.AddMessage("The server you input was invalid! Use the ID, full name, or part of the name!");
			return;
		}
		Session.pendingServer = serverInfo;
		AEC.getInstance().sendRequest(new RequestDisconnect());
	}

	private static void Spell(ChatActionParams ap)
	{
		if (int.TryParse(ap.args[1], out var result))
		{
			SpellTemplate spellT = SpellTemplates.Get(result, Entities.Instance.me.effects, Entities.Instance.me.ScaledClassRank, Entities.Instance.me.EquippedClassID, Entities.Instance.me.comboState.Get(result));
			Game.Instance.combat.TryCastSpell(spellT);
		}
	}

	private static void JoinQueue(ChatActionParams ap)
	{
		if (!int.TryParse(ap.args[1], out var result))
		{
			return;
		}
		string password = ((ap.args.Count > 2) ? ap.args[2] : "");
		bool createPrivate = false;
		if (ap.args.Count > 3)
		{
			switch (ap.args[3].ToLower())
			{
			case "t":
			case "true":
			case "1":
				createPrivate = true;
				break;
			}
		}
		if (result != 4)
		{
			AEC.getInstance().sendRequest(new RequestJoinQueue(result, password, createPrivate));
		}
	}

	private static void LeaveQueue(ChatActionParams ap)
	{
		AEC.getInstance().sendRequest(new RequestLeaveQueue());
	}

	private static void ShowVictory(ChatActionParams ap)
	{
		PvpMatchInfo pvpMatchInfo = new PvpMatchInfo();
		pvpMatchInfo.matchSeconds = 0;
		pvpMatchInfo.timeStampServer = GameTime.realtimeSinceServerStartup;
		pvpMatchInfo.yourScore = 750;
		pvpMatchInfo.enemyScore = 360;
		pvpMatchInfo.yourTeam = new List<PvpPlayerStats>
		{
			new PvpPlayerStats(Entities.Instance.me.ID, UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 25)),
			new PvpPlayerStats(Entities.Instance.me.ID, UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 25)),
			new PvpPlayerStats(Entities.Instance.me.ID, UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 25)),
			new PvpPlayerStats(Entities.Instance.me.ID, UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 25)),
			new PvpPlayerStats(Entities.Instance.me.ID, UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 25))
		};
		pvpMatchInfo.enemyTeam = new List<PvpPlayerStats>
		{
			new PvpPlayerStats(Entities.Instance.me.ID, UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 25)),
			new PvpPlayerStats(Entities.Instance.me.ID, UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 25)),
			new PvpPlayerStats(Entities.Instance.me.ID, UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 25)),
			new PvpPlayerStats(Entities.Instance.me.ID, UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 25)),
			new PvpPlayerStats(Entities.Instance.me.ID, UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 25))
		};
		PvpRewardInfo pvpRewardInfo = new PvpRewardInfo();
		pvpRewardInfo.rewards.Add(PvpMatchRewardType.Gold, UnityEngine.Random.Range(100, 1000));
		pvpRewardInfo.rewards.Add(PvpMatchRewardType.MarkOfGlory, UnityEngine.Random.Range(100, 1000));
		pvpRewardInfo.rewards.Add(PvpMatchRewardType.XP, UnityEngine.Random.Range(100, 1000));
		pvpRewardInfo.rewards.Add(PvpMatchRewardType.ClassXP, UnityEngine.Random.Range(100, 1000));
		pvpRewardInfo.rewards.Add(PvpMatchRewardType.Glory, UnityEngine.Random.Range(100, 1000));
		UIPvpMatchScreen.Instance.Init(pvpMatchInfo, pvpRewardInfo);
	}

	private static void ShowDefeat(ChatActionParams ap)
	{
		PvpMatchInfo pvpMatchInfo = new PvpMatchInfo();
		pvpMatchInfo.matchSeconds = 0;
		pvpMatchInfo.timeStampServer = GameTime.realtimeSinceServerStartup;
		pvpMatchInfo.yourScore = 360;
		pvpMatchInfo.enemyScore = 750;
		pvpMatchInfo.yourTeam = new List<PvpPlayerStats>
		{
			new PvpPlayerStats(Entities.Instance.me.ID, UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 25)),
			new PvpPlayerStats(Entities.Instance.me.ID, UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 25)),
			new PvpPlayerStats(Entities.Instance.me.ID, UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 25)),
			new PvpPlayerStats(Entities.Instance.me.ID, UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 25)),
			new PvpPlayerStats(Entities.Instance.me.ID, UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 25))
		};
		pvpMatchInfo.enemyTeam = new List<PvpPlayerStats>
		{
			new PvpPlayerStats(Entities.Instance.me.ID, UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 25)),
			new PvpPlayerStats(Entities.Instance.me.ID, UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 25)),
			new PvpPlayerStats(Entities.Instance.me.ID, UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 25)),
			new PvpPlayerStats(Entities.Instance.me.ID, UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 25)),
			new PvpPlayerStats(Entities.Instance.me.ID, UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 25), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 100000), UnityEngine.Random.Range(0, 25))
		};
		PvpRewardInfo pvpRewardInfo = new PvpRewardInfo();
		pvpRewardInfo.rewards.Add(PvpMatchRewardType.Gold, UnityEngine.Random.Range(100, 1000));
		pvpRewardInfo.rewards.Add(PvpMatchRewardType.MarkOfGlory, UnityEngine.Random.Range(100, 1000));
		pvpRewardInfo.rewards.Add(PvpMatchRewardType.XP, UnityEngine.Random.Range(100, 1000));
		pvpRewardInfo.rewards.Add(PvpMatchRewardType.ClassXP, UnityEngine.Random.Range(100, 1000));
		pvpRewardInfo.rewards.Add(PvpMatchRewardType.Glory, UnityEngine.Random.Range(100, 1000));
		UIPvpMatchScreen.Instance.Init(pvpMatchInfo, pvpRewardInfo);
	}

	private static void NPCEditor(ChatActionParams ap)
	{
		if (Main.Environment == Environment.Content && Session.MyPlayerData.CheckBitFlag("iu1", 30))
		{
			if (!Game.Instance.TesterMode)
			{
				Game.Instance.DeveloperModeToggle();
			}
			UINPCEditor.Toggle();
		}
	}

	private static void MapEditor(ChatActionParams ap)
	{
		if (Main.Environment == Environment.Content && Session.MyPlayerData.CheckBitFlag("iu1", 30))
		{
			if (!Game.Instance.TesterMode)
			{
				Game.Instance.DeveloperModeToggle();
			}
			UIMapEditor.Toggle();
		}
	}

	private static void ActivateDevMode(ChatActionParams ap)
	{
		Game.Instance.NamePlateButtonToggle();
		Session.MyPlayerData.OnDevModeToggled();
	}

	private static void BuyGuildPower(ChatActionParams ap)
	{
		Debug.Log("buy guild power via chat command");
		RequestGuildBuyPower requestGuildBuyPower = new RequestGuildBuyPower();
		int.TryParse(ap.args[1], out requestGuildBuyPower.GuildID);
		int.TryParse(ap.args[2], out requestGuildBuyPower.ItemID);
		Game.Instance.aec.sendRequest(requestGuildBuyPower);
	}
}
