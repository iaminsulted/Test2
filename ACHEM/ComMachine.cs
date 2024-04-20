using System.Collections.Generic;
using StatCurves;

public class ComMachine
{
	public int ID;

	public byte State;

	public int OwnerID;

	public bool HasResource;

	public int NodeID;

	public int TradeSkillLevel;

	public int TotalUsages;

	public int Usages;

	public RarityType Rarity;

	public string JsonActions;

	public string JsonCTActions;

	public string JsonRequirements;

	public List<int> Areas = new List<int>();

	public List<int> DropIDs = new List<int>();

	public float VerticalRotation;

	public float HorizontalRotation;
}
