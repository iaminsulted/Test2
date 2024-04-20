using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StylesUtil
{
	public static Dictionary<int, BeardStyle> BeardStyles = new Dictionary<int, BeardStyle>();

	public static Dictionary<int, BraidStyle> BraidStyles = new Dictionary<int, BraidStyle>();

	public static Dictionary<int, HairStyle> HairStyles = new Dictionary<int, HairStyle>();

	public static Dictionary<int, StacheStyle> StacheStyles = new Dictionary<int, StacheStyle>();

	public static List<Color32> EntityHairColors = new List<Color32>
	{
		new Color32(92, 43, 43, byte.MaxValue),
		new Color32(176, 22, 22, byte.MaxValue),
		new Color32(241, 105, 105, byte.MaxValue),
		new Color32(229, 192, 192, byte.MaxValue),
		new Color32(38, 37, 55, byte.MaxValue),
		new Color32(64, 66, 69, byte.MaxValue),
		new Color32(153, 152, 156, byte.MaxValue),
		new Color32(220, 220, 221, byte.MaxValue),
		new Color32(53, 43, 39, byte.MaxValue),
		new Color32(88, 54, 38, byte.MaxValue),
		new Color32(111, 86, 33, byte.MaxValue),
		new Color32(166, 159, 145, byte.MaxValue),
		new Color32(132, 84, 41, byte.MaxValue),
		new Color32(222, 189, 13, byte.MaxValue),
		new Color32(199, 180, 101, byte.MaxValue),
		new Color32(212, 206, 185, byte.MaxValue),
		new Color32(89, 51, 101, byte.MaxValue),
		new Color32(149, 75, 165, byte.MaxValue),
		new Color32(212, 151, 226, byte.MaxValue),
		new Color32(249, 238, byte.MaxValue, byte.MaxValue),
		new Color32(94, 0, 91, byte.MaxValue),
		new Color32(182, 0, 176, byte.MaxValue),
		new Color32(byte.MaxValue, 86, byte.MaxValue, byte.MaxValue),
		new Color32(233, 186, 233, byte.MaxValue),
		new Color32(15, 65, 7, byte.MaxValue),
		new Color32(30, 126, 14, byte.MaxValue),
		new Color32(43, 189, 44, byte.MaxValue),
		new Color32(165, 213, 154, byte.MaxValue),
		new Color32(0, 90, 114, byte.MaxValue),
		new Color32(17, 145, 179, byte.MaxValue),
		new Color32(0, 202, byte.MaxValue, byte.MaxValue),
		new Color32(176, 222, 239, byte.MaxValue),
		new Color32(13, 8, 111, byte.MaxValue),
		new Color32(57, 0, 172, byte.MaxValue),
		new Color32(39, 84, 229, byte.MaxValue),
		new Color32(153, 153, 223, byte.MaxValue),
		new Color32(byte.MaxValue, 108, 3, byte.MaxValue)
	};

	public static List<EntitySkinColor> EntitySkinColors = new List<EntitySkinColor>
	{
		new EntitySkinColor
		{
			A = new Color32(249, 233, 220, byte.MaxValue),
			B = new Color32(174, 118, 84, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(byte.MaxValue, 220, 142, byte.MaxValue),
			B = new Color32(126, 89, 44, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(254, 200, 128, byte.MaxValue),
			B = new Color32(159, 88, 26, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(173, 112, 55, byte.MaxValue),
			B = new Color32(96, 36, 0, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(249, 232, 200, byte.MaxValue),
			B = new Color32(196, 161, 135, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(247, 242, 210, byte.MaxValue),
			B = new Color32(183, 124, 88, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(byte.MaxValue, 228, 188, byte.MaxValue),
			B = new Color32(182, 124, 90, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(219, 197, 103, byte.MaxValue),
			B = new Color32(169, 117, 86, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(249, 224, 156, byte.MaxValue),
			B = new Color32(229, 118, 72, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(byte.MaxValue, 220, 142, byte.MaxValue),
			B = new Color32(158, 142, 79, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(254, 200, 128, byte.MaxValue),
			B = new Color32(159, 88, 26, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(173, 112, 55, byte.MaxValue),
			B = new Color32(96, 36, 0, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(159, 111, 74, byte.MaxValue),
			B = new Color32(60, 17, 14, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(byte.MaxValue, 246, 246, byte.MaxValue),
			B = new Color32(203, 200, 183, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(byte.MaxValue, 252, 219, byte.MaxValue),
			B = new Color32(110, 134, 146, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(152, 209, 214, byte.MaxValue),
			B = new Color32(90, 147, 186, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(192, 214, 229, byte.MaxValue),
			B = new Color32(61, 96, 150, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(70, 218, 231, byte.MaxValue),
			B = new Color32(47, 120, 189, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(116, 228, 248, byte.MaxValue),
			B = new Color32(45, 139, 151, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(102, 220, 231, byte.MaxValue),
			B = new Color32(9, 75, 53, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(176, 172, 108, byte.MaxValue),
			B = new Color32(46, 137, 114, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(117, 219, 34, byte.MaxValue),
			B = new Color32(17, 103, 24, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(201, 93, byte.MaxValue, byte.MaxValue),
			B = new Color32(90, 66, 152, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(253, 158, 227, byte.MaxValue),
			B = new Color32(195, 54, 175, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(212, 17, 17, byte.MaxValue),
			B = new Color32(101, 7, 7, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(204, 38, 38, byte.MaxValue),
			B = new Color32(43, 7, 7, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(216, 189, 79, byte.MaxValue),
			B = new Color32(158, 131, 21, byte.MaxValue)
		},
		new EntitySkinColor
		{
			A = new Color32(60, 64, 73, byte.MaxValue),
			B = new Color32(16, 17, 21, byte.MaxValue)
		}
	};

	public static List<EntityArmorColor> EntityArmorColors = new List<EntityArmorColor>
	{
		new EntityArmorColor
		{
			R = new Color32(113, 175, byte.MaxValue, byte.MaxValue),
			G = new Color32(219, 118, 62, byte.MaxValue),
			B = new Color32(132, 146, 71, byte.MaxValue)
		},
		new EntityArmorColor
		{
			R = new Color32(byte.MaxValue, 58, 59, byte.MaxValue),
			G = new Color32(60, 60, 60, byte.MaxValue),
			B = new Color32(byte.MaxValue, 181, 142, byte.MaxValue)
		},
		new EntityArmorColor
		{
			R = new Color32(byte.MaxValue, 210, 59, byte.MaxValue),
			G = new Color32(182, 57, 27, byte.MaxValue),
			B = new Color32(118, 165, 219, byte.MaxValue)
		},
		new EntityArmorColor
		{
			R = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue),
			G = new Color32(77, 82, 120, byte.MaxValue),
			B = new Color32(131, 88, 46, byte.MaxValue)
		}
	};

	public static List<Color32> EntityEyeColors = new List<Color32>
	{
		new Color32(0, 201, byte.MaxValue, byte.MaxValue),
		new Color32(61, 165, 236, byte.MaxValue),
		new Color32(0, 140, byte.MaxValue, byte.MaxValue),
		new Color32(138, 230, byte.MaxValue, byte.MaxValue),
		new Color32(0, byte.MaxValue, 244, byte.MaxValue),
		new Color32(40, 230, 200, byte.MaxValue),
		new Color32(30, 190, 150, byte.MaxValue),
		new Color32(90, 225, 100, byte.MaxValue),
		new Color32(30, 190, 75, byte.MaxValue),
		new Color32(109, 200, 87, byte.MaxValue),
		new Color32(250, 130, 50, byte.MaxValue),
		new Color32(200, 100, 40, byte.MaxValue),
		new Color32(227, 143, 44, byte.MaxValue),
		new Color32(150, 81, 6, byte.MaxValue),
		new Color32(byte.MaxValue, 0, 0, byte.MaxValue),
		new Color32(190, 90, byte.MaxValue, byte.MaxValue),
		new Color32(245, 74, byte.MaxValue, byte.MaxValue),
		new Color32(250, 115, byte.MaxValue, byte.MaxValue),
		new Color32(byte.MaxValue, byte.MaxValue, 50, byte.MaxValue),
		new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue),
		new Color32(75, 75, 75, byte.MaxValue)
	};

	public static List<Color32> EntityLipColors = new List<Color32>
	{
		new Color32(byte.MaxValue, 154, 103, byte.MaxValue),
		new Color32(244, 72, 72, byte.MaxValue),
		new Color32(221, 73, 72, byte.MaxValue),
		new Color32(159, 16, 16, byte.MaxValue),
		new Color32(0, 0, 0, byte.MaxValue),
		new Color32(98, 39, 0, byte.MaxValue),
		new Color32(150, 60, 2, byte.MaxValue),
		new Color32(229, 118, 72, byte.MaxValue),
		new Color32(169, 117, 86, byte.MaxValue),
		new Color32(byte.MaxValue, 165, 96, byte.MaxValue),
		new Color32(196, 161, 135, byte.MaxValue),
		new Color32(249, 150, 95, byte.MaxValue),
		new Color32(236, 96, 96, byte.MaxValue),
		new Color32(byte.MaxValue, 129, 205, byte.MaxValue),
		new Color32(245, 74, byte.MaxValue, byte.MaxValue),
		new Color32(141, 96, 236, byte.MaxValue),
		new Color32(156, 0, byte.MaxValue, byte.MaxValue),
		new Color32(96, 236, 230, byte.MaxValue),
		new Color32(0, 201, byte.MaxValue, byte.MaxValue),
		new Color32(96, 166, 236, byte.MaxValue),
		new Color32(0, 140, byte.MaxValue, byte.MaxValue),
		new Color32(96, 120, 236, byte.MaxValue),
		new Color32(65, 56, 131, byte.MaxValue),
		new Color32(106, 236, 96, byte.MaxValue),
		new Color32(19, 200, 57, byte.MaxValue),
		new Color32(109, 200, 87, byte.MaxValue),
		new Color32(236, 221, 96, byte.MaxValue),
		new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue),
		new Color32(182, 182, 182, byte.MaxValue),
		new Color32(0, 0, 0, byte.MaxValue)
	};

	public static Color32 RandomHairColor => ArtixRandom.GetElementOfList(EntityHairColors);

	public static EntitySkinColor RandomSkinColor => ArtixRandom.GetElementOfList(EntitySkinColors);

	public static EntityArmorColor RandomArmorColor => EntityArmorColors[3];

	public static void Init(StyleCollection styleCollection)
	{
		BeardStyles = styleCollection.BeardStyles.ToDictionary((BeardStyle x) => x.ID);
		BraidStyles = styleCollection.BraidStyles.ToDictionary((BraidStyle x) => x.ID);
		HairStyles = styleCollection.HairStyles.ToDictionary((HairStyle x) => x.ID);
		StacheStyles = styleCollection.StacheStyles.ToDictionary((StacheStyle x) => x.ID);
	}
}
