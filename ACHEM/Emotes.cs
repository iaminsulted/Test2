using System.Collections.Generic;
using System.Linq;

public class Emotes
{
	private static Dictionary<StateEmote, Emote> Map = new Dictionary<StateEmote, Emote>
	{
		{
			StateEmote.Comeon,
			new Emote
			{
				em = StateEmote.Comeon,
				sfxLocation = SFXLocation.Feet,
				name = "Come On",
				cmd = "comeon",
				anim = "Comeon",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Cry,
			new Emote
			{
				em = StateEmote.Cry,
				sfxLocation = SFXLocation.Feet,
				name = "Cry",
				cmd = "cry",
				anim = "Cry",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Exorcist,
			new Emote
			{
				em = StateEmote.Exorcist,
				sfxLocation = SFXLocation.Feet,
				name = "Exorcist",
				cmd = "exorcist",
				anim = "Exorcist",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Facepalm,
			new Emote
			{
				em = StateEmote.Facepalm,
				sfxLocation = SFXLocation.Feet,
				name = "Facepalm",
				cmd = "facepalm",
				anim = "Facepalm",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Fistshake,
			new Emote
			{
				em = StateEmote.Fistshake,
				sfxLocation = SFXLocation.Feet,
				name = "Fist Shake",
				cmd = "fistshake",
				anim = "Fistshake",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Headbang,
			new Emote
			{
				em = StateEmote.Headbang,
				sfxLocation = SFXLocation.Feet,
				name = "Headbang",
				cmd = "headbang",
				anim = "Headbang",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.MJ,
			new Emote
			{
				em = StateEmote.MJ,
				sfxLocation = SFXLocation.Feet,
				name = "MJ",
				cmd = "mj",
				anim = "MJ",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Powerup,
			new Emote
			{
				em = StateEmote.Powerup,
				sfxLocation = SFXLocation.Feet,
				name = "Power Up",
				cmd = "powerup",
				anim = "Powerup",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Squat,
			new Emote
			{
				em = StateEmote.Squat,
				sfxLocation = SFXLocation.Feet,
				name = "Squat",
				cmd = "squat",
				anim = "Squat",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Thewave,
			new Emote
			{
				em = StateEmote.Thewave,
				sfxLocation = SFXLocation.Feet,
				name = "The Wave",
				cmd = "thewave",
				anim = "Thewave",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Wave,
			new Emote
			{
				em = StateEmote.Wave,
				sfxLocation = SFXLocation.Feet,
				name = "Wave",
				cmd = "wave",
				anim = "Waving",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Levitate,
			new Emote
			{
				em = StateEmote.Levitate,
				sfxLocation = SFXLocation.Feet,
				name = "Levitate",
				cmd = "levitate",
				anim = "Levitate",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Talk,
			new Emote
			{
				em = StateEmote.Talk,
				sfxLocation = SFXLocation.Feet,
				name = "Talk",
				cmd = "talk",
				anim = "Talk1,Talk2,Talk3,Talk4",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Talk1,
			new Emote
			{
				em = StateEmote.Talk1,
				sfxLocation = SFXLocation.Feet,
				name = "Talk1",
				cmd = "talk1",
				anim = "Talk1",
				sfxName = "",
				looping = false,
				hidden = true,
				accessLevel = 0
			}
		},
		{
			StateEmote.Talk2,
			new Emote
			{
				em = StateEmote.Talk2,
				sfxLocation = SFXLocation.Feet,
				name = "Talk2",
				cmd = "talk2",
				anim = "Talk2",
				sfxName = "",
				looping = false,
				hidden = true,
				accessLevel = 0
			}
		},
		{
			StateEmote.Talk3,
			new Emote
			{
				em = StateEmote.Talk3,
				sfxLocation = SFXLocation.Feet,
				name = "Talk3",
				cmd = "talk3",
				anim = "Talk3",
				sfxName = "",
				looping = false,
				hidden = true,
				accessLevel = 0
			}
		},
		{
			StateEmote.Talk4,
			new Emote
			{
				em = StateEmote.Talk4,
				sfxLocation = SFXLocation.Feet,
				name = "Talk4",
				cmd = "talk4",
				anim = "Talk4",
				sfxName = "",
				looping = false,
				hidden = true,
				accessLevel = 0
			}
		},
		{
			StateEmote.Alligator,
			new Emote
			{
				em = StateEmote.Alligator,
				sfxLocation = SFXLocation.Feet,
				name = "Alligator",
				cmd = "alligator",
				anim = "alligator",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Busta,
			new Emote
			{
				em = StateEmote.Busta,
				sfxLocation = SFXLocation.Feet,
				name = "Busta",
				cmd = "busta",
				anim = "busta",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Busta2,
			new Emote
			{
				em = StateEmote.Busta2,
				sfxLocation = SFXLocation.Feet,
				name = "Busta 2",
				cmd = "busta2",
				anim = "busta2",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Coffeegrinder,
			new Emote
			{
				em = StateEmote.Coffeegrinder,
				sfxLocation = SFXLocation.Feet,
				name = "Coffeegrinder",
				cmd = "coffeegrinder",
				anim = "coffeegrinder",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Flares,
			new Emote
			{
				em = StateEmote.Flares,
				sfxLocation = SFXLocation.Feet,
				name = "Flares",
				cmd = "flares",
				anim = "flares",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Jesterkick,
			new Emote
			{
				em = StateEmote.Jesterkick,
				sfxLocation = SFXLocation.Feet,
				name = "Jesterkick",
				cmd = "jesterkick",
				anim = "jesterkick",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Powermove,
			new Emote
			{
				em = StateEmote.Powermove,
				sfxLocation = SFXLocation.Feet,
				name = "Powermove",
				cmd = "powermove",
				anim = "powermove",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Strikewave,
			new Emote
			{
				em = StateEmote.Strikewave,
				sfxLocation = SFXLocation.Feet,
				name = "Strikewave",
				cmd = "strikewave",
				anim = "strikewave",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Strikewave2,
			new Emote
			{
				em = StateEmote.Strikewave2,
				sfxLocation = SFXLocation.Feet,
				name = "Strikewave 2",
				cmd = "strikewave2",
				anim = "strikewave2",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Toprock,
			new Emote
			{
				em = StateEmote.Toprock,
				sfxLocation = SFXLocation.Feet,
				name = "Toprock",
				cmd = "toprock",
				anim = "toprock",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Jumpdance,
			new Emote
			{
				em = StateEmote.Jumpdance,
				sfxLocation = SFXLocation.Feet,
				name = "Jumpdance",
				cmd = "jumpdance",
				anim = "jumpdance",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Fistpump,
			new Emote
			{
				em = StateEmote.Fistpump,
				sfxLocation = SFXLocation.Feet,
				name = "Fistpump",
				cmd = "fistpump",
				anim = "FistPump",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Nickledumb,
			new Emote
			{
				em = StateEmote.Nickledumb,
				sfxLocation = SFXLocation.Feet,
				name = "Nickledumb",
				cmd = "nickledumb",
				anim = "Nickledumb",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Sit,
			new Emote
			{
				em = StateEmote.Sit,
				sfxLocation = SFXLocation.Feet,
				name = "Sit",
				cmd = "sit",
				anim = "Sit",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Sit2,
			new Emote
			{
				em = StateEmote.Sit2,
				sfxLocation = SFXLocation.Feet,
				name = "Sit 2",
				cmd = "sit",
				anim = "Sit2",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Sit3,
			new Emote
			{
				em = StateEmote.Sit3,
				sfxLocation = SFXLocation.Feet,
				name = "Sit 3",
				cmd = "sit3",
				anim = "Sit3",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Kneel,
			new Emote
			{
				em = StateEmote.Kneel,
				sfxLocation = SFXLocation.Feet,
				name = "Kneel",
				cmd = "kneel",
				anim = "Kneel",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.BlowKiss,
			new Emote
			{
				em = StateEmote.BlowKiss,
				sfxLocation = SFXLocation.Feet,
				name = "Blow Kiss",
				cmd = "blowkiss",
				anim = "BlowKiss",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.FishCast,
			new Emote
			{
				em = StateEmote.FishCast,
				sfxLocation = SFXLocation.Feet,
				name = "Fish Cast",
				cmd = "FishCast",
				anim = "FishCast",
				sfxName = "SFX_Fishing_Cast",
				looping = false,
				hidden = true,
				accessLevel = 0
			}
		},
		{
			StateEmote.Fishing,
			new Emote
			{
				em = StateEmote.Fishing,
				sfxLocation = SFXLocation.Feet,
				name = "Fishing",
				cmd = "Fishing",
				anim = "Fishing",
				sfxName = "SFX_Fishing_Idle",
				looping = true,
				hidden = true,
				accessLevel = 0
			}
		},
		{
			StateEmote.ReelLoop,
			new Emote
			{
				em = StateEmote.ReelLoop,
				sfxLocation = SFXLocation.CastSpot,
				name = "ReelLoop",
				cmd = "ReelLoop",
				anim = "ReelLoop",
				sfxName = "SFX_Fishing_Reel",
				looping = true,
				hidden = true,
				accessLevel = 0
			}
		},
		{
			StateEmote.FishingCatch,
			new Emote
			{
				em = StateEmote.FishingCatch,
				sfxLocation = SFXLocation.CastSpot,
				name = "FishingCatch",
				cmd = "FishingCatch",
				anim = "FishingCatch",
				sfxName = "SFX_Fishing_Catch",
				looping = false,
				hidden = true,
				accessLevel = 0
			}
		},
		{
			StateEmote.Lol,
			new Emote
			{
				em = StateEmote.Lol,
				sfxLocation = SFXLocation.Feet,
				name = "Lol",
				cmd = "lol",
				anim = "Lol",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Honor,
			new Emote
			{
				em = StateEmote.Honor,
				sfxLocation = SFXLocation.Feet,
				name = "Honor",
				cmd = "salute",
				anim = "Honor",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Nod,
			new Emote
			{
				em = StateEmote.Nod,
				sfxLocation = SFXLocation.Feet,
				name = "Nod",
				cmd = "nod",
				anim = "Nod",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Cheer,
			new Emote
			{
				em = StateEmote.Cheer,
				sfxLocation = SFXLocation.Feet,
				name = "Cheer",
				cmd = "cheer",
				anim = "Cheer",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.IdleConfusedTravolta,
			new Emote
			{
				em = StateEmote.IdleConfusedTravolta,
				sfxLocation = SFXLocation.Feet,
				name = "Sus",
				cmd = "sus",
				anim = "IdleConfusedTravolta",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Dansen,
			new Emote
			{
				em = StateEmote.Dansen,
				sfxLocation = SFXLocation.Feet,
				name = "Dansen",
				cmd = "dansen",
				anim = "Dansen",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.Joy,
			new Emote
			{
				em = StateEmote.Joy,
				sfxLocation = SFXLocation.Feet,
				name = "Joy",
				cmd = "joy",
				anim = "JumpCheer",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.ShoulderWeapon,
			new Emote
			{
				em = StateEmote.ShoulderWeapon,
				sfxLocation = SFXLocation.Feet,
				name = "Shoulder Weapon",
				cmd = "shoulder",
				anim = "1stClassVictory",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 0
			}
		},
		{
			StateEmote.YeaBaby,
			new Emote
			{
				em = StateEmote.YeaBaby,
				sfxLocation = SFXLocation.Feet,
				name = "Yea Baby",
				cmd = "yeababy",
				anim = "WooYeahBaby",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 0,
				badgeID = 501
			}
		},
		{
			StateEmote.Emote1,
			new Emote
			{
				em = StateEmote.Emote1,
				sfxLocation = SFXLocation.Feet,
				name = "Emote1",
				cmd = "emote1",
				anim = "Emote1",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Dance2D,
			new Emote
			{
				em = StateEmote.Dance2D,
				sfxLocation = SFXLocation.Feet,
				name = "AQW:I Dance",
				cmd = "dance2d",
				anim = "Dance2D",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.PlayDead,
			new Emote
			{
				em = StateEmote.PlayDead,
				sfxLocation = SFXLocation.Feet,
				name = "Play Dead",
				cmd = "playdead",
				anim = "PlayDead",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Chillout,
			new Emote
			{
				em = StateEmote.Chillout,
				sfxLocation = SFXLocation.Feet,
				name = "Chill out",
				cmd = "chillout",
				anim = "Chillout",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Thanks,
			new Emote
			{
				em = StateEmote.Thanks,
				sfxLocation = SFXLocation.Feet,
				name = "Thanks",
				cmd = "thanks",
				anim = "Thanks",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Thankful,
			new Emote
			{
				em = StateEmote.Thankful,
				sfxLocation = SFXLocation.Feet,
				name = "Thankful",
				cmd = "thankful",
				anim = "Thankful",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Bow,
			new Emote
			{
				em = StateEmote.Bow,
				sfxLocation = SFXLocation.Feet,
				name = "Bow",
				cmd = "bow",
				anim = "Bow",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.BowSalute,
			new Emote
			{
				em = StateEmote.BowSalute,
				sfxLocation = SFXLocation.Feet,
				name = "Bow Salute",
				cmd = "BowSalute",
				anim = "BowSalute",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Meditate,
			new Emote
			{
				em = StateEmote.Meditate,
				sfxLocation = SFXLocation.Feet,
				name = "Meditate",
				cmd = "meditate",
				anim = "Meditate",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.LyingDown,
			new Emote
			{
				em = StateEmote.LyingDown,
				sfxLocation = SFXLocation.Feet,
				name = "Lying Down",
				cmd = "lyingdown",
				anim = "LyingDown",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.SlowClap,
			new Emote
			{
				em = StateEmote.SlowClap,
				sfxLocation = SFXLocation.Feet,
				name = "Slow Clap",
				cmd = "slowclap",
				anim = "Slowclap",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Headscratch,
			new Emote
			{
				em = StateEmote.Headscratch,
				sfxLocation = SFXLocation.Feet,
				name = "Head Scratch",
				cmd = "headscratch",
				anim = "Headscratch",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.NailedIt,
			new Emote
			{
				em = StateEmote.NailedIt,
				sfxLocation = SFXLocation.Feet,
				name = "Nailed It",
				cmd = "nailedit",
				anim = "NailedIt",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Shock,
			new Emote
			{
				em = StateEmote.Shock,
				sfxLocation = SFXLocation.Feet,
				name = "Shock",
				cmd = "shock",
				anim = "Shock",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Startled,
			new Emote
			{
				em = StateEmote.Startled,
				sfxLocation = SFXLocation.Feet,
				name = "Startled",
				cmd = "startled",
				anim = "Startled",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Sigh,
			new Emote
			{
				em = StateEmote.Sigh,
				sfxLocation = SFXLocation.Feet,
				name = "Sigh",
				cmd = "sigh",
				anim = "Sigh",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Victory,
			new Emote
			{
				em = StateEmote.Victory,
				sfxLocation = SFXLocation.Feet,
				name = "Victory",
				cmd = "victory",
				anim = "Victory",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Yes,
			new Emote
			{
				em = StateEmote.Yes,
				sfxLocation = SFXLocation.Feet,
				name = "Yes",
				cmd = "yes",
				anim = "Yes",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.No,
			new Emote
			{
				em = StateEmote.No,
				sfxLocation = SFXLocation.Feet,
				name = "No",
				cmd = "no",
				anim = "No",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.WiseYes,
			new Emote
			{
				em = StateEmote.WiseYes,
				sfxLocation = SFXLocation.Feet,
				name = "Wise Yes",
				cmd = "wiseyes",
				anim = "WiseYes",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Shrug,
			new Emote
			{
				em = StateEmote.Shrug,
				sfxLocation = SFXLocation.Feet,
				name = "Shrug",
				cmd = "shrug",
				anim = "Shrug",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Give,
			new Emote
			{
				em = StateEmote.Give,
				sfxLocation = SFXLocation.Feet,
				name = "Give",
				cmd = "give",
				anim = "Give",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Receive,
			new Emote
			{
				em = StateEmote.Receive,
				sfxLocation = SFXLocation.Feet,
				name = "Receive",
				cmd = "Receive",
				anim = "Receive",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.PraiseIt,
			new Emote
			{
				em = StateEmote.PraiseIt,
				sfxLocation = SFXLocation.Feet,
				name = "Praise It",
				cmd = "praiseit",
				anim = "PraiseIt",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Theforce,
			new Emote
			{
				em = StateEmote.Theforce,
				sfxLocation = SFXLocation.Feet,
				name = "The Force",
				cmd = "theforce",
				anim = "Theforce",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.WipeSweat,
			new Emote
			{
				em = StateEmote.WipeSweat,
				sfxLocation = SFXLocation.Feet,
				name = "Wipe Sweat",
				cmd = "wipesweat",
				anim = "WipeSweat",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Nooo,
			new Emote
			{
				em = StateEmote.Nooo,
				sfxLocation = SFXLocation.Feet,
				name = "Nooo!!!",
				cmd = "nooo",
				anim = "Nooo",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.DramaticNooo,
			new Emote
			{
				em = StateEmote.DramaticNooo,
				sfxLocation = SFXLocation.Feet,
				name = "Dramatic Nooo!!!",
				cmd = "dramaticnooo",
				anim = "DramaticNooo",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.KickDance,
			new Emote
			{
				em = StateEmote.KickDance,
				sfxLocation = SFXLocation.Feet,
				name = "Kick Dance",
				cmd = "kickdance",
				anim = "KickDance",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Buttwiggle,
			new Emote
			{
				em = StateEmote.Buttwiggle,
				sfxLocation = SFXLocation.Feet,
				name = "Buttwiggle",
				cmd = "buttwiggle",
				anim = "Buttwiggle",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Worship,
			new Emote
			{
				em = StateEmote.Worship,
				sfxLocation = SFXLocation.Feet,
				name = "Worship",
				cmd = "worship",
				anim = "Worship",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.KneelSearch,
			new Emote
			{
				em = StateEmote.KneelSearch,
				sfxLocation = SFXLocation.Feet,
				name = "Kneel and Search",
				cmd = "kneelandsearch",
				anim = "KneelAndSearch",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Twerk,
			new Emote
			{
				em = StateEmote.Twerk,
				sfxLocation = SFXLocation.Feet,
				name = "Twerk",
				cmd = "twerk",
				anim = "Twerk",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.CheckSword,
			new Emote
			{
				em = StateEmote.CheckSword,
				sfxLocation = SFXLocation.Feet,
				name = "Check Sword",
				cmd = "checksword",
				anim = "CheckSword",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.CheckShoulders,
			new Emote
			{
				em = StateEmote.CheckShoulders,
				sfxLocation = SFXLocation.Feet,
				name = "Check Shoulders",
				cmd = "checkshoulders",
				anim = "CheckShoulders",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.CheckCape,
			new Emote
			{
				em = StateEmote.CheckCape,
				sfxLocation = SFXLocation.Feet,
				name = "Check Cape",
				cmd = "checkcape",
				anim = "CheckCape",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.CheckHelm,
			new Emote
			{
				em = StateEmote.CheckHelm,
				sfxLocation = SFXLocation.Feet,
				name = "Check Helm",
				cmd = "checkhelm",
				anim = "CheckHelm",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Check2Hands,
			new Emote
			{
				em = StateEmote.Check2Hands,
				sfxLocation = SFXLocation.Feet,
				name = "Check Hands",
				cmd = "checkhands",
				anim = "Check2Hands",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.CheckBoots,
			new Emote
			{
				em = StateEmote.CheckBoots,
				sfxLocation = SFXLocation.Feet,
				name = "Check Boots",
				cmd = "checkboots",
				anim = "CheckBoots",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.CheckBelt,
			new Emote
			{
				em = StateEmote.CheckBelt,
				sfxLocation = SFXLocation.Feet,
				name = "Check Belt",
				cmd = "checkbelt",
				anim = "CheckBelt",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.LongingReach,
			new Emote
			{
				em = StateEmote.LongingReach,
				sfxLocation = SFXLocation.Feet,
				name = "Longing Reach",
				cmd = "longingreach",
				anim = "LongingReach",
				sfxName = "",
				looping = false,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.HorserideStyle,
			new Emote
			{
				em = StateEmote.HorserideStyle,
				sfxLocation = SFXLocation.Feet,
				name = "Horseride Style",
				cmd = "horseridestyle",
				anim = "GangnamStyle",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Victory2,
			new Emote
			{
				em = StateEmote.Victory2,
				sfxLocation = SFXLocation.Feet,
				name = "Victory 2",
				cmd = "victory2",
				anim = "JumpCheer",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.Point,
			new Emote
			{
				em = StateEmote.Point,
				sfxLocation = SFXLocation.Feet,
				name = "Point",
				cmd = "point",
				anim = "Point",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 100
			}
		},
		{
			StateEmote.BackFlip,
			new Emote
			{
				em = StateEmote.BackFlip,
				sfxLocation = SFXLocation.Feet,
				name = "Back Flip",
				cmd = "backflip",
				anim = "BackFlip",
				sfxName = "",
				looping = true,
				hidden = false,
				accessLevel = 100
			}
		}
	};

	public static List<Emote> GetAll()
	{
		return Map.Values.ToList();
	}

	public static List<Emote> GetUnlockedEmotes()
	{
		return Map.Values.Where(IsEmoteUnlocked).ToList();
	}

	public static Emote GetByState(StateEmote state)
	{
		if (Map.TryGetValue(state, out var value))
		{
			return value;
		}
		return null;
	}

	public static Emote GetByCommand(string cmd)
	{
		return Map.Values.FirstOrDefault((Emote e) => e.cmd.ToLower() == cmd.ToLower());
	}

	public static bool IsEmoteUnlocked(Emote emote)
	{
		if (emote != null && !emote.hidden && Session.MyPlayerData.AccessLevel >= emote.accessLevel)
		{
			if (emote.badgeID != 0)
			{
				return Session.MyPlayerData.HasBadgeID(emote.badgeID);
			}
			return true;
		}
		return false;
	}
}
