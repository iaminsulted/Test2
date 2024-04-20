using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using StatCurves;

public class EntityAsset
{
	public string gender;

	public string prefab;

	public BundleInfo bundle;

	public int ColorSkin;

	public int ColorHair;

	public int Hair;

	public int ColorEye;

	public int ColorLip;

	public int Stache;

	public int Beard;

	public int Braid;

	public int Face;

	public string ColorR;

	public string ColorG;

	public string ColorB;

	public string ColorSkinA;

	public string ColorSkinB;

	public Dictionary<EquipItemSlot, EquipItem> equips = new Dictionary<EquipItemSlot, EquipItem>();

	public float ScaleFactor = 1f;

	public int NPCID;

	[DefaultValue(EquipItemSlot.Weapon)]
	public EquipItemSlot WeaponRequired = EquipItemSlot.Weapon;

	public bool DualWield;

	public EquipItemSlot Current = EquipItemSlot.Weapon;

	public EntityAsset()
	{
	}

	public EntityAsset(EntityAsset target)
	{
		gender = target.gender;
		prefab = target.gender;
		bundle = target.bundle;
		ColorSkin = target.ColorSkin;
		ColorHair = target.ColorHair;
		ColorEye = target.ColorEye;
		ColorLip = target.ColorLip;
		Hair = target.Hair;
		Stache = target.Stache;
		Beard = target.Beard;
		Braid = target.Braid;
		Face = target.Face;
		ColorR = target.ColorR;
		ColorG = target.ColorG;
		ColorB = target.ColorB;
		ColorSkinA = target.ColorSkinA;
		ColorSkinB = target.ColorSkinB;
		equips = target.equips.ToDictionary((KeyValuePair<EquipItemSlot, EquipItem> entry) => entry.Key, (KeyValuePair<EquipItemSlot, EquipItem> entry) => entry.Value);
		ScaleFactor = target.ScaleFactor;
		NPCID = target.NPCID;
		WeaponRequired = target.WeaponRequired;
		DualWield = target.DualWield;
		Current = target.Current;
	}
}
