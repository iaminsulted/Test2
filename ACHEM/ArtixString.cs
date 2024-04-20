using System;

public static class ArtixString
{
	public static string UpArrow => "↑";

	public static string DownArrow => "↓";

	public static string AddCommas(int number)
	{
		return $"{number:#,###0}";
	}

	public static string AddCommas(float number)
	{
		return number.ToString("##,##0.##");
	}

	public static string AddCommas(decimal number)
	{
		return number.ToString("##,##0.##");
	}

	public static int[] ParseCommaDelimited(this string s)
	{
		return Array.ConvertAll(s.Split(','), int.Parse);
	}

	public static string FormatDuration(float seconds)
	{
		if (seconds == 0f)
		{
			return "Instant";
		}
		float num = Math.Abs(seconds - (float)(int)seconds);
		TimeSpan timeSpan = new TimeSpan(0, 0, (int)seconds);
		string text = "";
		if (timeSpan.Days > 0)
		{
			text = text + timeSpan.Days + "d ";
		}
		if (timeSpan.Hours > 0)
		{
			text = text + timeSpan.Hours + "h ";
		}
		if (timeSpan.Minutes > 0)
		{
			text = text + timeSpan.Minutes + "m ";
		}
		if ((float)timeSpan.Seconds + num > 0f)
		{
			text += ((float)timeSpan.Seconds + num).ToString("0.##");
			text += "s";
		}
		return text;
	}
}
