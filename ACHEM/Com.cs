public sealed class Com
{
	public enum Type : byte
	{
		NULL,
		Move,
		NpcMove,
		Login,
		Chat,
		Joinmychannels,
		Channel,
		Area,
		Cell,
		Command,
		Item,
		Trade,
		Combat,
		NPCTemplates,
		SpellTemplates,
		Quest,
		Loot,
		Entity,
		NPC,
		Machine,
		PvP,
		Emote,
		_DEBUG,
		Report,
		SyncIgnore,
		Message,
		CombatClasses,
		Misc,
		Merge,
		Friend,
		Disconnect,
		Party,
		Bank,
		EndTransfer,
		Admin,
		AdWatch,
		Player,
		SystemPerformance,
		DirectCommand,
		Resource,
		Guild,
		DailyTask,
		ServerDailyTask,
		Leaderboard,
		SuperClient,
		DailyQuestReset,
		ServerTools,
		Audio,
		Sheathing,
		ServerEvent,
		Housing,
		Mail
	}

	public enum CmdAdmin : byte
	{
		NULL,
		QSSet,
		Dialog,
		ReloadData,
		ReloadShop,
		ReloadQuest,
		ReloadMongo,
		QSInfo,
		ApopEdit
	}

	public enum CmdBank : byte
	{
		NULL,
		LoadItems,
		TransferItem,
		PurchaseSlot,
		LoadAllItems
	}

	public enum CmdMove : byte
	{
		NULL,
		Full,
		Synch,
		Keys,
		Jump
	}

	public enum CmdNpcMove : byte
	{
		NULL,
		Path,
		Speed,
		Stop,
		StopSync
	}

	public enum CmdChat : byte
	{
		NULL,
		Multi,
		Whisper,
		Party,
		Guild,
		NPC
	}

	public enum CmdArea : byte
	{
		NULL,
		Join,
		Remove,
		List,
		WarSync,
		MatchState,
		ExitInstance,
		DynamicScale,
		StartSpeedrun,
		EndSpeedrun
	}

	public enum CmdCell : byte
	{
		NULL,
		Join,
		Add,
		Remove,
		Teleport,
		Score,
		SoundTrackUpdate,
		TimerPause,
		TimerStart,
		TimerStop,
		TimerUnpause
	}

	public enum CmdChannel : byte
	{
		NULL,
		Join,
		Add,
		Remove
	}

	public enum CmdCommand : byte
	{
		NULL,
		Admin,
		Join,
		VoteKick,
		Help,
		Get
	}

	public enum CmdItem : byte
	{
		NULL,
		Equip,
		Unequip,
		Update,
		Remove,
		Add,
		Use,
		Load,
		Claim,
		OpenLootBox,
		DailyReward,
		DustLookBoxItem,
		EquipBest,
		Infuse,
		Extract,
		Log,
		InfuseResponse,
		ExtractResponse,
		ReloadInventory,
		ModifierReroll,
		ModifierRerollResponse,
		ModifierRerollConfirm,
		ModifierRerollConfirmResponse
	}

	public enum CmdTrade : byte
	{
		NULL,
		Buy,
		Sell,
		ShopLoad,
		BuyCollection
	}

	public enum CmdMerge : byte
	{
		NULL,
		Merge,
		MergeShopLoad,
		Claim,
		BuyOut,
		Speedup,
		Add,
		Remove
	}

	public enum CmdCombat : byte
	{
		NULL,
		SpellRequest,
		SpellTrigger,
		SpellCancel,
		Spell,
		EffectPulse,
		Item,
		IAO,
		MachineSpell,
		EffectRemove,
		ResetCD,
		AuraUpdate
	}

	public enum CmdQuest : byte
	{
		NULL,
		Load,
		Accept,
		Abandon,
		Complete,
		Progress,
		Meta
	}

	public enum CmdLoot : byte
	{
		NULL,
		AddDropBag,
		RemoveDropItem,
		GetDropItem,
		GetLootBag,
		LootAll
	}

	public enum CmdEntity : byte
	{
		NULL,
		AddGoldXP,
		LevelUp,
		Update,
		PlayerRespawn,
		Unused,
		Class,
		MCUpdate,
		GoldUpdate,
		BitFlagUpdate,
		AssetOverride,
		Customize,
		DataSync,
		KillSelf,
		TransferMap,
		AssetUpdate,
		PortraitUpdate,
		TitleUpdate,
		LoadBadges,
		PetEquip,
		PetUnequip,
		RankUp,
		AFK,
		Disconnect,
		Gender,
		PetInteract,
		WeaponUpdate,
		ToolUpdate,
		TradeSkillAddXP,
		TradeSkillLevelUp,
		CapstoneBarFill,
		EquipPvpActions,
		GloryXpUpdate,
		Sheathe
	}

	public enum CmdNPC : byte
	{
		NULL,
		Despawn,
		Spawn,
		DialogueStart,
		DialogueEnd,
		TurnTo,
		TeamEvent,
		Animation,
		Baited,
		Behavior,
		Notify
	}

	public enum CmdMachine : byte
	{
		NULL,
		TransferPad,
		Update,
		Click,
		Enter,
		Exit,
		Cast,
		Animation,
		AnimatorLayerWeight,
		AnimatorParameter,
		AreaFlag,
		Trigger,
		CTState,
		Collision,
		HarpoonSync,
		HarpoonFire,
		ListenerUpdate,
		ResourceChannel,
		ResourceCollect,
		ResourceDespawn,
		ResourceDrop,
		ResourceInteract,
		ResourceInterrupt,
		ResourceSpawn,
		ResourceTrigger,
		ResourceUsageUpdate
	}

	public enum CmdClass : byte
	{
		NULL,
		Add,
		Equip,
		EquipCrossSkill,
		SpellCooldown
	}

	public enum CmdLeaderboard : byte
	{
		NULL,
		Refresh,
		PlayerScoreUpdate
	}

	public enum CmdMisc : byte
	{
		NULL,
		Notify,
		W6vS2MD75O,
		ReportError,
		ServerEvent
	}

	public enum CmdFriend : byte
	{
		NULL,
		Request,
		Add,
		Delete,
		List,
		Summon,
		Goto,
		Added
	}

	public enum ItemTransferResponseType : byte
	{
		Success,
		EquippedItem,
		Duplicate,
		NotFound,
		Failure,
		QuestItem
	}

	public enum BankSlotPurchaseResponse : byte
	{
		Success,
		NotSufficientFunds,
		Failure
	}

	public enum CmdParty : byte
	{
		NULL,
		Invite,
		Join,
		Remove,
		Promote,
		List,
		AreaCreate,
		Goto,
		VoteKickStart,
		VoteKickChoice,
		VoteKickEnd,
		Privacy,
		Disconnect,
		Reconnect
	}

	public enum CmdPvP : byte
	{
		NULL,
		PvPToggle,
		DuelChallenge,
		DuelAccept,
		DuelCountdown,
		DuelStart,
		DuelComplete,
		DuelForfeit,
		PvPScore,
		CapturePoint,
		CapturePointTick,
		SoundTrackCountdown,
		MatchStart,
		MatchEnd,
		MatchLeave,
		TimerStart
	}

	public enum CmdPlayer : byte
	{
		NULL,
		ProductOfferSet,
		TimedChoice,
		TimedChoiceCancel,
		JoinQueue,
		LeaveQueue,
		QueueStatus,
		PvPStats,
		PvPRecords
	}

	public enum CmdSystemPerformance : byte
	{
		NULL,
		Ping,
		SessionPing
	}

	public enum CmdResource : byte
	{
		NULL,
		BobberDespawn,
		BobberSpawn,
		FishCatch,
		FishHook,
		FishRelease
	}

	public enum CmdGuild : byte
	{
		NULL,
		Join,
		Info,
		Leave,
		Invite,
		Update,
		ChangeRole,
		GuildMemberStatus,
		NameChange,
		TagChange,
		MOTDUpdate,
		LevelUp,
		BuyPower,
		PowerActivated,
		UpdateTax,
		GoldXpInfo,
		LeaderboardEntries
	}

	public enum CmdDailyTask : byte
	{
		NULL,
		Info
	}

	public enum CmdServerDaily : byte
	{
		NULL,
		Initialize
	}

	public enum CmdServerTools : byte
	{
		NULL,
		Teleport,
		UpdatePosSpawn,
		HijackSpawn,
		AddSpawn,
		DeleteSpawn,
		AddNPC,
		DeleteNPC,
		EditNPC,
		UpdatePathNPC,
		DeletePathNPC,
		ResUpdatePosSpawn,
		ResHijackSpawn,
		ResAddSpawn,
		ResDeleteSpawn,
		ResAddNPC,
		ResDeleteNPC,
		ResEditNPC,
		ResUpdatePathNPC,
		ResDeletePathNPC,
		OpenInAdmin,
		ResOpenInAdmin,
		OpenApopAdmin,
		UpdateRequirements,
		ResUpdateRequirements,
		AddMapEntity,
		ResAddMapEntity,
		DeleteMapEntity,
		ResDeleteMapEntity,
		UpdateMapEntity,
		ResUpdateMapEntity,
		OpenApops,
		OpenNPCs,
		GetMapAssets,
		ResGetMapAssets,
		GetFavoriteMapAssets,
		ResGetFavoriteMapAssets
	}

	public enum CmdAudio : byte
	{
		NULL,
		Play2DClip,
		Play3DClip
	}

	public enum CmdSheathing : byte
	{
		NULL,
		Sheathe,
		Unsheathe
	}

	public enum CmdHousing : byte
	{
		NULL,
		HouseItemAdd,
		HouseItemMove,
		HouseItemRemove,
		HouseItemClearAll,
		HouseJoin,
		HouseSave,
		HouseExit,
		HouseData,
		HouseAdd,
		HouseUpdate,
		HouseSaveExit,
		HouseForceExit,
		HouseQuest,
		HouseItemList,
		HouseItemForceRemove,
		MapPreview
	}

	public enum CmdMail : byte
	{
		NULL,
		Box,
		Send,
		Receive,
		Delete,
		Favorite,
		Redeem,
		Seen
	}

	public static readonly byte CmdNone = 1;
}
