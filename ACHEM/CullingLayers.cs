using System;
using System.Collections.Generic;

[Serializable]
public class CullingLayers
{
	public float _ENTITIES = 150f;

	public float _NPCS = 150f;

	public float _PHYSICS = 110f;

	public float _MIDDLEFAR = 110f;

	public float _CLOSE = 40f;

	public float _MIDDLE = 80f;

	public float _FAR = 300f;

	public float _BG = 1000f;

	public static int DEFAULT = 0;

	public static int PLAYER = 8;

	public static int ENTITIES = 9;

	public static int TRIGGERS = 10;

	public static int TERRAIN = 11;

	public static int NPCS = 12;

	public static int CLICKIES = 13;

	public static int CUTSCENE = 15;

	public static int PHYSICS = 24;

	public static int MIDDLEFAR = 25;

	public static int CLOSE = 26;

	public static int MIDDLE = 27;

	public static int FAR = 28;

	public static int BG = 29;

	public List<CustomCullingLayers> LayersCollectionMinimum = new List<CustomCullingLayers>();

	public List<CustomCullingLayers> LayersCollectionMaximum = new List<CustomCullingLayers>();

	public CullingLayers()
	{
		LayersCollectionMinimum.Add(new CustomCullingLayers
		{
			Name = "ENTITIES",
			Distance = 150f,
			Index = ENTITIES
		});
		LayersCollectionMinimum.Add(new CustomCullingLayers
		{
			Name = "NPCS",
			Distance = 150f,
			Index = NPCS
		});
		LayersCollectionMinimum.Add(new CustomCullingLayers
		{
			Name = "PHYSICS",
			Distance = 110f,
			Index = PHYSICS
		});
		LayersCollectionMinimum.Add(new CustomCullingLayers
		{
			Name = "MIDDLEFAR",
			Distance = 110f,
			Index = MIDDLEFAR
		});
		LayersCollectionMinimum.Add(new CustomCullingLayers
		{
			Name = "CLOSE",
			Distance = 40f,
			Index = CLOSE
		});
		LayersCollectionMinimum.Add(new CustomCullingLayers
		{
			Name = "MIDDLE",
			Distance = 80f,
			Index = MIDDLE
		});
		LayersCollectionMinimum.Add(new CustomCullingLayers
		{
			Name = "FAR",
			Distance = 300f,
			Index = FAR
		});
		LayersCollectionMinimum.Add(new CustomCullingLayers
		{
			Name = "BG",
			Distance = 1000f,
			Index = BG
		});
		LayersCollectionMaximum.Add(new CustomCullingLayers
		{
			Name = "ENTITIES",
			Distance = 100000f,
			Index = ENTITIES
		});
		LayersCollectionMaximum.Add(new CustomCullingLayers
		{
			Name = "NPCS",
			Distance = 100000f,
			Index = NPCS
		});
		LayersCollectionMaximum.Add(new CustomCullingLayers
		{
			Name = "PHYSICS",
			Distance = 100000f,
			Index = PHYSICS
		});
		LayersCollectionMaximum.Add(new CustomCullingLayers
		{
			Name = "MIDDLEFAR",
			Distance = 100000f,
			Index = MIDDLEFAR
		});
		LayersCollectionMaximum.Add(new CustomCullingLayers
		{
			Name = "CLOSE",
			Distance = 100000f,
			Index = CLOSE
		});
		LayersCollectionMaximum.Add(new CustomCullingLayers
		{
			Name = "MIDDLE",
			Distance = 100000f,
			Index = MIDDLE
		});
		LayersCollectionMaximum.Add(new CustomCullingLayers
		{
			Name = "FAR",
			Distance = 100000f,
			Index = FAR
		});
		LayersCollectionMaximum.Add(new CustomCullingLayers
		{
			Name = "BG",
			Distance = 100000f,
			Index = BG
		});
	}

	public float[] OriginalDistances()
	{
		float[] array = new float[32];
		array[ENTITIES] = 150f;
		array[NPCS] = 150f;
		array[PHYSICS] = 110f;
		array[MIDDLEFAR] = 110f;
		array[CLOSE] = 40f;
		array[MIDDLE] = 80f;
		array[FAR] = 300f;
		array[BG] = 1000f;
		return array;
	}

	public float[] CullDistanceMin()
	{
		float[] array = new float[32];
		foreach (CustomCullingLayers item in LayersCollectionMinimum)
		{
			array[item.Index] = item.Distance;
		}
		return array;
	}

	public float[] CullDistanceMax()
	{
		float[] array = new float[32];
		foreach (CustomCullingLayers item in LayersCollectionMaximum)
		{
			array[item.Index] = item.Distance;
		}
		return array;
	}

	public void SetVal(int index, float dist)
	{
		CustomCullingLayers customCullingLayers = LayersCollectionMinimum.Find((CustomCullingLayers x) => x.Index == index);
		if (customCullingLayers != null)
		{
			customCullingLayers.Distance = dist;
		}
	}
}
