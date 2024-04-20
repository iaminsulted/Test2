using System;
using System.Linq;

public static class ArtixMath
{
	public static T Clamp<T>(this T value, T min, T max) where T : IComparable
	{
		value = ((value.CompareTo(max) > 0) ? max : value);
		value = ((value.CompareTo(min) < 0) ? min : value);
		return value;
	}

	public static bool Between<T>(this T value, T min, T max) where T : IComparable
	{
		if (value.CompareTo(min) >= 0)
		{
			return value.CompareTo(max) <= 0;
		}
		return false;
	}

	public static int Mod(int a, int n)
	{
		if (n == 0)
		{
			throw new ArgumentOutOfRangeException("n", "(a mod 0) is undefined.");
		}
		int num = a % n;
		if ((n > 0 && num < 0) || (n < 0 && num > 0))
		{
			return num + n;
		}
		return num;
	}

	public static T Max<T>(params T[] values)
	{
		return values.Max();
	}

	public static T Min<T>(params T[] values)
	{
		return values.Min();
	}

	public static float Normalize(float value, float min, float max)
	{
		return (value - min) / (max - min);
	}

	public static float RoundToNearest(float value, float roundTo)
	{
		return (float)(Math.Round(value / roundTo) * (double)roundTo);
	}

	public static bool ApproximatelyEquals(this float value, float num)
	{
		return Math.Abs(value - num) < 0.02f;
	}

	public static bool BitGet(long num, int index)
	{
		return (num & (1L << index - 1)) != 0;
	}

	public static long BitSet(long num, int index)
	{
		num |= 1L << index - 1;
		return num;
	}

	public static long BitClear(long num, int index)
	{
		num &= ~(1L << index - 1);
		return num;
	}

	public static bool DoSegmentsOverlap(float aStart, float aEnd, float bStart, float bEnd)
	{
		if (!DoesAngleIntersectSegment(aStart, bStart, bEnd) && !DoesAngleIntersectSegment(aEnd, bStart, bEnd) && !DoesAngleIntersectSegment(bStart, aStart, aEnd))
		{
			return DoesAngleIntersectSegment(bEnd, aStart, aEnd);
		}
		return true;
	}

	public static bool DoesAngleIntersectSegment(float angle, float segmentStart, float segmentEnd)
	{
		angle = (angle + 360f) % 360f;
		segmentStart = (segmentStart + 360f) % 360f;
		segmentEnd = (segmentEnd + 360f) % 360f;
		if (segmentStart > segmentEnd)
		{
			if (angle >= segmentStart || angle <= segmentEnd)
			{
				return true;
			}
		}
		else if (angle >= segmentStart && angle <= segmentEnd)
		{
			return true;
		}
		return false;
	}
}
