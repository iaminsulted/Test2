using StatCurves;

public class DropSurfaceEquip : DropSurface
{
	public EquipItemSlot EquipSlot;

	public DropSurfaceEquip()
	{
		type = DropSurfaceType.Equip;
	}
}
