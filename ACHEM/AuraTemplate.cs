using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class AuraTemplate
{
	public List<int> actionIDs = new List<int>();

	public List<AoeShape> aoes = new List<AoeShape>();

	public float tickRate;

	public float duration;

	public int maxCount = int.MaxValue;

	public int maxHits = int.MaxValue;

	[JsonIgnore]
	public int tickCount => Mathf.FloorToInt(duration / tickRate);
}
