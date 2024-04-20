using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityResists
{
	public float[] values;

	private static readonly int Resist_Count = Enum.GetValues(typeof(CombatSolver.Element)).Length;

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

	public float this[CombatSolver.Element s]
	{
		get
		{
			return values[(int)s];
		}
		set
		{
			values[(int)s] = value;
		}
	}

	public EntityResists()
	{
		values = new float[Resist_Count];
		for (int i = 0; i < values.Length; i++)
		{
			values[0] = 0f;
		}
	}

	public Dictionary<CombatSolver.Element, float> GetValues()
	{
		Dictionary<CombatSolver.Element, float> dictionary = new Dictionary<CombatSolver.Element, float>();
		for (int i = 0; i < values.Length; i++)
		{
			if (Mathf.Approximately(values[i], 0f))
			{
				dictionary[(CombatSolver.Element)i] = values[i];
			}
		}
		return dictionary;
	}

	public void SetValues(Dictionary<CombatSolver.Element, float> map)
	{
		foreach (KeyValuePair<CombatSolver.Element, float> item in map)
		{
			values[(int)item.Key] = item.Value;
		}
	}
}
