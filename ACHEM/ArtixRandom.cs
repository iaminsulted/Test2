using System;
using System.Collections.Generic;
using System.Linq;

public static class ArtixRandom
{
	private static readonly object randomLock;

	private static readonly Random random;

	static ArtixRandom()
	{
		randomLock = new object();
		random = new Random();
	}

	public static int Range(int min, int max)
	{
		if (min >= max)
		{
			return min;
		}
		lock (randomLock)
		{
			return min + random.Next(max - min + 1);
		}
	}

	public static float Range(float min, float max)
	{
		lock (randomLock)
		{
			return (float)((double)min + random.NextDouble() * (double)(max - min));
		}
	}

	public static bool RandomBool()
	{
		return Range(0, 1) == 0;
	}

	public static float GetRandomFactor(float factor)
	{
		return Range(1f - factor, 1f + factor);
	}

	public static T GetEnumValue<T>()
	{
		return GetEnumValue<T>(0, Enum.GetNames(typeof(T)).Length - 1);
	}

	public static T GetEnumValue<T>(int startIndex, int endIndex)
	{
		Array values = Enum.GetValues(typeof(T));
		startIndex = startIndex.Clamp(0, endIndex);
		endIndex = endIndex.Clamp(startIndex, values.Length - 1);
		return (T)values.GetValue(Range(startIndex, endIndex));
	}

	public static T GetEnumValue<T>(List<T> excludedValues)
	{
		Array values = Enum.GetValues(typeof(T));
		List<T> list = new List<T>();
		for (int i = 0; i < values.Length; i++)
		{
			T item = (T)values.GetValue(i);
			if (!excludedValues.Contains(item))
			{
				list.Add(item);
			}
		}
		return GetElementOfList(list);
	}

	public static IList<T> ShuffleList<T>(IList<T> list)
	{
		return ShuffleList(list, 0, list.Count - 1);
	}

	public static IList<T> ShuffleList<T>(IList<T> list, int startIndex, int endIndex)
	{
		IList<T> list2 = new List<T>(list.Count);
		foreach (T item in list)
		{
			list2.Add(item);
		}
		startIndex = startIndex.Clamp(0, endIndex);
		endIndex = endIndex.Clamp(startIndex, list.Count - 1);
		for (int i = startIndex; i < endIndex; i++)
		{
			int index = Range(i, endIndex);
			T value = list2[index];
			list2[index] = list2[i];
			list2[i] = value;
		}
		return list2;
	}

	public static T GetElementOfList<T>(List<T> list)
	{
		if (list == null || list.Count == 0)
		{
			return default(T);
		}
		return list[Range(0, list.Count - 1)];
	}

	public static T GetElementOfList<T>(T[] list)
	{
		if (list == null || list.Length == 0)
		{
			return default(T);
		}
		return list[Range(0, list.Length - 1)];
	}

	public static T GetElementOfList<T>(List<T> list, List<T> excludedValues)
	{
		return GetElementOfList(list.Where((T t) => !excludedValues.Contains(t)).ToList());
	}

	public static T GetElementOfList<T>(T[] list, T[] excludedValues)
	{
		return GetElementOfList(list.ToList(), excludedValues.ToList());
	}

	public static void RandomPositionInCircle(float radius, out float x, out float y)
	{
		double num = (double)radius * Math.Sqrt(Range(0f, 1f));
		double num2 = Math.PI * 2.0 * (double)Range(0f, 1f);
		x = (float)(num * Math.Cos(num2));
		y = (float)(num * Math.Sin(num2));
	}
}
