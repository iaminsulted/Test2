using System.Collections.Generic;

public static class GameTips
{
	private static List<string> Tips = new List<string>
	{
		"You can activate AUTO RUN by clicking your mouse wheel.",
		"You can cycle through targets using the " + ArtixUnity.KeyCodeName(SettingsManager.GetKeyCodeByAction(InputAction.Target_Next)) + " key.",
		"You can use the WASD keys or arrow keys to move around by default.",
		"We're building the game under your feet while you play.",
		"You can freely switch between classes.",
		"AQ3D is a true cross platform game! You can play with your friends no matter what device they are playing on!",
		"You can tell an item's quality by the color of its name.",
		"You can change your graphics quality and sound volume in the SETTINGS menu.",
		"The original AdventureQuest web browser game has been updated every week since it was released in 2002!",
		"NPCs can have quests, shops, and crafting recipes!  Make sure you check them all!",
		"Crafting is the best way to get new gear!",
		"Help us make this game! If you find a bug, report it on AQ3D.com/bugs!",
		"The Quest Arrow can help you find some quest objectives!",
		"Players with gold names over their heads are Developers who make AQ3D.",
		"As with all of our games, NEVER give your login information to anyone.",
		"You can use the Track Quest option to keep a quest active in your quest tracker.",
		"Players with orange names over their heads are Moderators who help keep the game safe.",
		"Guardians get special quests and gear inside Guardian Tower.",
		"AdventureQuest 3D is 100% supported and funded by the players.",
		"Botting, Cheating, and/or Hacking will result in a permanent ban.",
		"You can play AQ3D on Mac or PC through Steam, or on Android and iOS phones and tablets!",
		"Check the Adventures menu to see where to go next!",
		"Reach Rank 10 on each class to unlock a Cross Skill that can be used by all classes.",
		"Be kind to each other.",
		"You can freely change your look by equipping gear as Cosmetics!",
		"AQ3D is an excellent source of calcium when served with milk.",
		"We should make a Surgeon General NPC that gives warnings.",
		"A family that plays together, stays together.",
		"Ghosts are terrible liars. You can see right through them.",
		"Zombies only like girls for their brains.",
		"Now with 75.3% more combat!",
		"You are reading this statement!",
		"Don't think about a pencil.",
		"Yulgar smells like fresh sawdust and cinnamon.",
		"AQ3D comes with complimentary bugs!",
		"FUN FACT: Cysero has hidden six magical socks in the game. But you can't get to any of them.",
		"Which came first, the Korin or the paper bag?",
		"FUN FACT: Artix's hands are exactly the same size.",
		"The way to a dragon's heart is through its stomach. Just head up then veer right when you see the lungs.",
		"FUN FACT: AQ3D was made by the same team that dressed themselves this morning.",
		"The only thing that Zhoom can't do is fail.",
		"The next update will come when it's Ready O'Clock.",
		"404: tip not found.",
		"Oops, we forgot to take this one out of the tips list.",
		"AFK = A Free Kill.",
		"FUN FACT: If Thyton had Cysero's height and hair, he could take over the world.",
		"FUN FACT: Fisn hates that one thing you do but she's too polite to say it.",
		"FUN FACT: Yergen is more superior.",
		"FUN FACT: Dage never stops.",
		"FUN FACT: Xero is a modeler, texture artist and a healthy, low sodium, low carb snack.",
		"DO NOT FEED THE MONSTERS!"
	};

	public static string RandomTip
	{
		get
		{
			if (Tips == null || Tips.Count <= 0)
			{
				return "No hints available right now! You are on your own!";
			}
			return ArtixRandom.GetElementOfList(Tips);
		}
	}
}
