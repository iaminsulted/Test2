using System.Collections.Generic;

public class TravelParams
{
	public float speedBonus;

	public int mNpcID;

	public int fNpcID;

	public List<int> NpcIDPool = new List<int>();

	public string SpeedText => (100f + speedBonus * 100f).ToString("0") + "%";
}
