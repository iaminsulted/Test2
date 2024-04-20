using System.Collections.Generic;
using System.ComponentModel;
using StatCurves;

public class EquipItem
{
	public ItemGenderBasedType GenderBased;

	public byte RegionType;

	public byte Mesh0;

	public byte Mesh1;

	public byte MeshF0;

	public byte MeshF1;

	public byte ColorType;

	public string ColorR;

	public string ColorG;

	public string ColorB;

	public List<string> animations;

	public BundleInfo bundle;

	public bool DisableSheathing;

	public int ID { get; set; }

	public EquipItemSlot EquipSlot { get; set; }

	[DefaultValue("")]
	public string AssetName { get; set; }

	[DefaultValue("")]
	public string FileCM { get; set; }

	[DefaultValue("")]
	public string FileD { get; set; }

	[DefaultValue("")]
	public string FileC { get; set; }

	public Rectangle Rect
	{
		get
		{
			switch (EquipSlot)
			{
			case EquipItemSlot.Boots:
				return new Rectangle(0, 0, 340, 512);
			case EquipItemSlot.Class:
			case EquipItemSlot.Armor:
				if (RegionType == 0)
				{
					return new Rectangle(0, 0, 640, 1024);
				}
				return new Rectangle(0, 0, 1024, 1024);
			case EquipItemSlot.Belt:
				return new Rectangle(640, 658, 384, 366);
			case EquipItemSlot.Bracers:
			case EquipItemSlot.Gloves:
				return new Rectangle(342, 0, 300, 512);
			default:
				return null;
			}
		}
	}

	public EntityArmorColor ArmorColor
	{
		get
		{
			EntityArmorColor result = default(EntityArmorColor);
			result.R = ColorR.ToColor32();
			result.G = ColorG.ToColor32();
			result.B = ((ColorB == null) ? "000000".ToColor32() : ColorB.ToColor32());
			return result;
		}
	}

	public bool IsWeapon => ItemSlots.IsWeaponSlot(EquipSlot);

	public bool IsTool => ItemSlots.IsToolSlot(EquipSlot);

	public EquipItem()
	{
	}

	public EquipItem(EquipItem eItem)
	{
		ID = eItem.ID;
		GenderBased = eItem.GenderBased;
		EquipSlot = eItem.EquipSlot;
		RegionType = eItem.RegionType;
		Mesh0 = eItem.Mesh0;
		Mesh1 = eItem.Mesh1;
		MeshF0 = eItem.MeshF0;
		MeshF1 = eItem.MeshF1;
		ColorType = eItem.ColorType;
		AssetName = eItem.AssetName;
		FileCM = eItem.FileCM;
		FileD = eItem.FileD;
		FileC = eItem.FileC;
		ColorR = eItem.ColorR;
		ColorG = eItem.ColorG;
		ColorB = eItem.ColorB;
		bundle = eItem.bundle;
		animations = eItem.animations;
		DisableSheathing = eItem.DisableSheathing;
	}

	public EquipItem(EquipItemSlot equipSlot, string assetName, BundleInfo bundle)
	{
		EquipSlot = equipSlot;
		AssetName = assetName;
		this.bundle = bundle;
	}

	public byte GetMeshType0(string gender)
	{
		if (!(gender == "F"))
		{
			return Mesh0;
		}
		return MeshF0;
	}

	public byte GetMeshType1(string gender)
	{
		if (!(gender == "F"))
		{
			return Mesh1;
		}
		return MeshF1;
	}

	public bool HasAnimations()
	{
		return animations.Count > 0;
	}
}
