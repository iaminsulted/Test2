using System;
using StatCurves;

public class EntityStats
{
	private float[] values;

	private static readonly int Stat_Count = Enum.GetValues(typeof(Stat)).Length;

	public float this[int i]
	{
		get
		{
			return values[i];
		}
		set
		{
			values[i] = value;
		}
	}

	public float this[Stat s]
	{
		get
		{
			return this[(int)s];
		}
		set
		{
			this[(int)s] = value;
		}
	}

	public EntityStats()
	{
		values = new float[Stat_Count];
		values[10] = 6.5f;
	}

	public float[] GetValues()
	{
		return values;
	}

	public void SetValues(float[] values)
	{
		this.values = values;
	}

	public static bool DoesCreatePopup(Stat stat)
	{
		if (stat != 0)
		{
			return stat == Stat.Resource;
		}
		return true;
	}
}
