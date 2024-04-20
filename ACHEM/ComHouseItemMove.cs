using System;

public class ComHouseItemMove
{
	public int ID;

	public int PlacerID;

	public ComVector3 Position;

	public ComVector3 Rotation;

	public ComVector3 Scale;

	public DateTime Timestamp;

	public ComHouseItemMove Clone()
	{
		return MemberwiseClone() as ComHouseItemMove;
	}
}
