using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ChatFilter
{
	private string markChars = "~!@#$%^&*()_+-=:\"<>?,.;'\\";

	private string strictComparisonChars = "~!@#$%^&*()_+-=:\"<>?,.;'\\ÇüéâäåçêëèïîìÄÅæÆôöòûùÿ\u05a3¥áíóúñ?£ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝß";

	private string strictComparisonCharsB = "~!#%^&()_+-=:\"<>?,.;'\\ÇüéâäåçêëèïîìÄÅæÆôöòûùÿ\u05a3¥áíóúñ?£ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝß";

	private static int randomMaskIndex = 0;

	private char[] vowels = new char[5] { 'a', 'e', 'i', 'o', 'u' };

	private char[] noise = new char[7] { '@', '#', '$', '!', '%', '&', '*' };

	private int noiseLength;

	public static int DIR_NONE = 0;

	public static int DIR_INBOUND = 1;

	public static int DIR_OUTBOUND = 2;

	private string[] profanityB = new string[18]
	{
		"anal", "ass", "arse", "butt", "cum", "gay", "hore", "niga", "picka", "prat",
		"pron", "rape", "rapist", "rapi", "screw", "spic", "tit", "weed"
	};

	private string[] profanityA = new string[243]
	{
		"@$$", "@ss", "a55", "a5s", "as5", "a$$", "a$s", "as$", "a5$", "a$5",
		"a*s", "*ss", "a**", "as*", "assclown", "assface", "asshole", "asswipe", "bastard", "beating the meat",
		"beef curtains", "beef flaps", "bellend", "biatch", "bich", "b1ch", "b!ch", "blch", "b|ch", "bitch",
		"b1tch", "b!tch", "bltch", "b|tch", "bizzach", "blowjob", "bollocks", "boobies", "boobs", "buggery",
		"bullshit", "carpet muncher", "carpet munchers", "carpetlicker", "carpetlickers", "chode", "clit", "cocaine", "cock", "c0ck",
		"co*k", "c*ck", "cocksucker", "condom", "cracka", "cunt", "c*nt", "cu*t", "*unt", "cun*",
		"cvnt", "damn", "dick", "di*k", "d!ck", "d*ck", "d**k", "dlck", "dildo", "diido",
		"dumbass", "dyke", "ejaculate", "ekrem", "f*ck", "f@g", "fag", "fagot", "f@ggot", "faggot",
		"fata$$", "fatass", "felcher", "ffeg", "foreskin", "fuk", "fck", "fu*k", "fuck", "fuuck",
		"fuuk", "fcuk", "fvck", "fvk", "fvvck", "fvvk", "fock", "fux0r", "fucken", "fucker",
		"fucking", "fudgepacker", "fvdgepacker", "gangbang", "ganja", "hentai", "heroin", "homo", "h0m0", "h0mo",
		"hom0", "horny", "http", "injun", "jackoff", "jerkoff", "jism", "jiz", "jizz", "kanker",
		"kawk", "kike", "klootzak", "knulle", "kraut", "kurac", "kurwa", "kusi", "kyrpa", "l3+ch",
		"l3i+ch", "lesbian", "iesbian", "lesbo", "mamhoon", "marijuana", "masturbate", "meatpuppet", "merd", "mibun",
		"milf", "motherfucker", "mouliwop", "muie", "mulkku", "nads", "nazi", "nigga", "niger", "nigger",
		"nutsack", "orospu", "p0rn", "paska", "pen15", "penis", "penls", "phuck", "pierdol", "pillu",
		"pimmel", "pimpios", "piss", "poontsee", "pouf", "poufter", "porn", "pr0n", "preteen", "prick",
		"prostitute", "pusy", "pussy", "puto", "queef", "raped", "rautenberg", "retard", "rimjob", "schaffer",
		"schencter", "schiess", "schlampe", "scrotum", "secks", "sex", "s*x", "se*", "shaor", "sharmuta",
		"sharmute", "shipal", "shit", "sh1t", "sh!t", "shlt", "sh|t", "shiz", "sh1z", "sh!z",
		"shlz", "sh|z", "shiit", "shi!t", "sh!it", "shilt", "shlit", "sh||t", "shi|t", "sh|it",
		"shiiz", "s hit", "shi t", "sh*t", "s*it", "s**t", "shi*", "s***", "shlong", "skank",
		"skribz", "skurwysyn", "slut", "sl*t", "s**t", "slvt", "smartass", "smut", "spierdalaj", "splooge",
		"threesome", "tits", "titties", "twat", "vagina", "wank", "wetback", "whack off", "whore", "wh*r",
		"wh*re", "wichser", "zabourah"
	};

	public ChatFilter()
	{
		noiseLength = noise.Length;
	}

	public ChatFilteredMessage ProfanityCheck(string input, bool shouldCleanSymbols)
	{
		string text = (shouldCleanSymbols ? cleanStr(input, DIR_INBOUND) : input);
		string text2 = text.ToLower();
		string text3 = "";
		List<int> list = new List<int>();
		for (int i = 0; i < text2.Length; i++)
		{
			if (markChars.IndexOf(text2[i]) == -1)
			{
				text3 += text2[i];
				list.Add(i);
			}
		}
		text2 = text3;
		ChatFilteredMessage chatFilteredMessage = new ChatFilteredMessage();
		for (int j = 0; j < profanityB.Length; j++)
		{
			int startIndex = 0;
			while ((startIndex = text2.IndexOf(profanityB[j], startIndex, StringComparison.Ordinal)) > -1)
			{
				if ((startIndex == 0 || !char.IsLetter(text2[startIndex - 1])) && (startIndex == text2.Length - profanityB[j].Length || !char.IsLetter(text2[startIndex + profanityB[j].Length])))
				{
					chatFilteredMessage.terms.Add(profanityB[j]);
					chatFilteredMessage.code = 1;
					chatFilteredMessage.indices.Add(list[startIndex]);
					chatFilteredMessage.indices.Add(list[startIndex + profanityB[j].Length - 1]);
				}
				startIndex += profanityB[j].Length;
			}
		}
		for (int k = 0; k < profanityA.Length; k++)
		{
			int startIndex2 = 0;
			while ((startIndex2 = text2.IndexOf(profanityA[k], startIndex2, StringComparison.Ordinal)) > -1)
			{
				chatFilteredMessage.terms.Add(profanityA[k]);
				chatFilteredMessage.code = 1;
				chatFilteredMessage.indices.Add(list[startIndex2]);
				chatFilteredMessage.indices.Add(list[startIndex2 + profanityA[k].Length - 1]);
				startIndex2 += profanityA[k].Length;
			}
		}
		if (chatFilteredMessage.code == 1)
		{
			chatFilteredMessage.maskedMessage = maskStringByIndices(text, chatFilteredMessage.indices);
		}
		return chatFilteredMessage;
	}

	private string stripWhite(string str)
	{
		str = string.Join("", str.Split('\r'));
		str = string.Join("", str.Split('\t'));
		str = string.Join("", str.Split(' '));
		return str;
	}

	private string stripWhiteStrict(string str)
	{
		str = stripWhite(str);
		str = removeChars(str, strictComparisonChars);
		return str;
	}

	private string stripWhiteStrictB(string str)
	{
		str = stripWhite(str);
		str = removeChars(str, strictComparisonCharsB);
		return str;
	}

	private string stripMarks(string str)
	{
		str = removeChars(str, markChars);
		return str;
	}

	private string stripDuplicateVowels(string s)
	{
		string text = "";
		for (int i = 0; i < s.Length; i++)
		{
			if (text.Length == 0 || Array.IndexOf(vowels, s[i]) == -1 || s[i] != text[text.Length - 1])
			{
				text += s[i];
			}
		}
		return text;
	}

	private string removeChars(string input, string ignored)
	{
		string text = "";
		for (int i = 0; i < input.Length; i++)
		{
			if (ignored.IndexOf(input[i]) == -1)
			{
				text += input[i];
			}
		}
		return text;
	}

	private string MaskString(string input, int start, int end)
	{
		string text = input.Substring(0, start);
		for (int i = start; i < end; i++)
		{
			if (input[i] == ' ')
			{
				text += " ";
				continue;
			}
			text += noise[randomMaskIndex];
			randomMaskIndex = (randomMaskIndex + 1) % noiseLength;
		}
		return text + input.Substring(end + 1);
	}

	private string maskStringByIndices(string input, List<int> indeces)
	{
		string text = "";
		int count = indeces.Count;
		if (indeces.Count > 0 && indeces.Count % 2 == 0)
		{
			for (int i = 0; i < input.Length; i++)
			{
				bool flag = false;
				for (int j = 0; j < count; j += 2)
				{
					if (i >= indeces[j] && i <= indeces[j + 1])
					{
						flag = true;
					}
				}
				if (flag)
				{
					if (input[i] == ' ')
					{
						text += " ";
						continue;
					}
					text += noise[randomMaskIndex];
					randomMaskIndex = ((randomMaskIndex < noiseLength - 1) ? (randomMaskIndex + 1) : 0);
				}
				else
				{
					text += input[i];
				}
			}
		}
		else
		{
			Debug.Log("*** > Utility.maskStringBetween() > Malformed indeces array.  Must be in format [start,end, start,end, etc] ");
		}
		return text;
	}

	private string removeHTML(string src)
	{
		string text = src.ToLower();
		string text2 = src ?? "";
		string[] array = new string[2] { "&nbsp;", "<br>" };
		foreach (string text3 in array)
		{
			while (text.IndexOf(text3) > -1)
			{
				int num = text.IndexOf(text3);
				text = text.Substring(0, num) + " " + text.Substring(num + text3.Length, text.Length);
				text2 = text2.Substring(0, num) + " " + text2.Substring(num + text3.Length, text2.Length);
			}
		}
		return text2;
	}

	public string cleanStr(string str, int direction)
	{
		bool flag = direction == DIR_INBOUND;
		bool num = direction == DIR_OUTBOUND;
		str = string.Join("", Regex.Split(str, "&#"));
		str = string.Join("", Regex.Split(str, "#038:#"));
		if (num)
		{
			str = removeHTML(str);
		}
		if (num && str.IndexOf("%") > -1)
		{
			str = string.Join("#037:", Regex.Split(str, "%"));
		}
		if (flag && str.IndexOf("#037:") > -1)
		{
			str = string.Join("%", Regex.Split(str, "#037:"));
		}
		if (num && str.IndexOf("&") > -1)
		{
			str = string.Join("#038:", Regex.Split(str, "&"));
		}
		if (flag && str.IndexOf("#038:") > -1)
		{
			str = string.Join("&", Regex.Split(str, "#038:"));
		}
		if (num && str.IndexOf("<") > -1)
		{
			str = string.Join("#060:", Regex.Split(str, "<"));
		}
		if (flag && str.IndexOf("#060:") > -1)
		{
			str = string.Join("&lt;", Regex.Split(str, "#060:"));
		}
		if (num && str.IndexOf(">") > -1)
		{
			str = string.Join("#062:", Regex.Split(str, ">"));
		}
		if (flag && str.IndexOf("#062:") > -1)
		{
			str = string.Join("&gt;", Regex.Split(str, "#062:"));
		}
		if (flag)
		{
			str = removeHTML(str);
		}
		return str;
	}
}
