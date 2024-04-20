public static class Layers
{
	public static int DEFAULT;

	public static int WATER;

	public static int UI;

	public static int UIORTHO;

	public static int PLAYER_ME;

	public static int OTHER_PLAYERS;

	public static int TRIGGERS;

	public static int TERRAIN;

	public static int NPCS;

	public static int CLICKIES;

	public static int CUTSCENE;

	public static int SCREENTRANSITIONS;

	public static int SELECTIONCAMERA;

	public static int UI3D;

	public static int SPAWNEDITOR;

	public static int SNOW;

	public static int INVIZ_WALL;

	public static int PHYSICS;

	public static int MIDDLEFAR;

	public static int CLOSE;

	public static int MIDDLE;

	public static int FAR;

	public static int BG;

	public static int NGUI;

	public static float[] MinCullDistances;

	public static float[] MaxCullDistances;

	public static int LAYER_MASK_MOUSECLICK;

	public static int MASK_GROUNDTRACK;

	public static int MASK_GROUNDTRACK_LOOT;

	public static int MASK_LOS;

	public static int MASK_CAMERACOLLISION;

	public static int MASK_FLYTHROUGH;

	public static int MASK_NAMEPLATE;

	public static int MASK_MY_NAMEPLATE;

	static Layers()
	{
		DEFAULT = 0;
		WATER = 4;
		UI = 5;
		UIORTHO = 7;
		PLAYER_ME = 8;
		OTHER_PLAYERS = 9;
		TRIGGERS = 10;
		TERRAIN = 11;
		NPCS = 12;
		CLICKIES = 13;
		CUTSCENE = 15;
		SCREENTRANSITIONS = 16;
		SELECTIONCAMERA = 17;
		UI3D = 19;
		SPAWNEDITOR = 20;
		SNOW = 22;
		INVIZ_WALL = 23;
		PHYSICS = 24;
		MIDDLEFAR = 25;
		CLOSE = 26;
		MIDDLE = 27;
		FAR = 28;
		BG = 29;
		NGUI = 30;
		MinCullDistances = new float[32];
		MaxCullDistances = new float[32];
		LAYER_MASK_MOUSECLICK = 0;
		MASK_GROUNDTRACK = 0;
		MASK_GROUNDTRACK_LOOT = 0;
		MASK_LOS = 0;
		MASK_CAMERACOLLISION = 0;
		MASK_FLYTHROUGH = 0;
		MASK_NAMEPLATE = 0;
		MASK_MY_NAMEPLATE = 0;
		MinCullDistances[OTHER_PLAYERS] = 150f;
		MinCullDistances[NPCS] = 150f;
		MinCullDistances[PHYSICS] = 150f;
		MinCullDistances[MIDDLEFAR] = 150f;
		MinCullDistances[CLOSE] = 40f;
		MinCullDistances[MIDDLE] = 80f;
		MinCullDistances[FAR] = 300f;
		MinCullDistances[BG] = 1000f;
		MinCullDistances[TRIGGERS] = 75f;
		MaxCullDistances[OTHER_PLAYERS] = 100000f;
		MaxCullDistances[NPCS] = 100000f;
		MaxCullDistances[PHYSICS] = 100000f;
		MaxCullDistances[MIDDLEFAR] = 100000f;
		MaxCullDistances[CLOSE] = 100000f;
		MaxCullDistances[MIDDLE] = 100000f;
		MaxCullDistances[FAR] = 100000f;
		MaxCullDistances[BG] = 100000f;
		MaxCullDistances[TRIGGERS] = 100000f;
		LAYER_MASK_MOUSECLICK |= 1 << DEFAULT;
		LAYER_MASK_MOUSECLICK |= 1 << WATER;
		LAYER_MASK_MOUSECLICK |= 1 << TERRAIN;
		LAYER_MASK_MOUSECLICK |= 1 << FAR;
		LAYER_MASK_MOUSECLICK |= 1 << MIDDLEFAR;
		LAYER_MASK_MOUSECLICK |= 1 << MIDDLE;
		LAYER_MASK_MOUSECLICK |= 1 << CLOSE;
		LAYER_MASK_MOUSECLICK |= 1 << BG;
		LAYER_MASK_MOUSECLICK |= 1 << CLICKIES;
		LAYER_MASK_MOUSECLICK |= 1 << INVIZ_WALL;
		LAYER_MASK_MOUSECLICK |= 1 << SELECTIONCAMERA;
		LAYER_MASK_MOUSECLICK |= 1 << SPAWNEDITOR;
		LAYER_MASK_MOUSECLICK |= 1 << UI3D;
		MASK_GROUNDTRACK |= 1 << DEFAULT;
		MASK_GROUNDTRACK |= 1 << TERRAIN;
		MASK_GROUNDTRACK |= 1 << FAR;
		MASK_GROUNDTRACK |= 1 << MIDDLEFAR;
		MASK_GROUNDTRACK |= 1 << MIDDLE;
		MASK_GROUNDTRACK |= 1 << CLOSE;
		MASK_GROUNDTRACK |= 1 << BG;
		MASK_GROUNDTRACK |= 1 << INVIZ_WALL;
		MASK_GROUNDTRACK_LOOT = MASK_GROUNDTRACK;
		MASK_GROUNDTRACK_LOOT |= 1 << WATER;
		MASK_GROUNDTRACK_LOOT |= 1 << SNOW;
		MASK_LOS |= 1 << DEFAULT;
		MASK_LOS |= 1 << WATER;
		MASK_LOS |= 1 << TERRAIN;
		MASK_LOS |= 1 << FAR;
		MASK_LOS |= 1 << MIDDLEFAR;
		MASK_LOS |= 1 << MIDDLE;
		MASK_LOS |= 1 << BG;
		MASK_LOS |= 1 << INVIZ_WALL;
		MASK_CAMERACOLLISION |= 1 << WATER;
		MASK_CAMERACOLLISION |= 1 << PLAYER_ME;
		MASK_CAMERACOLLISION |= 1 << OTHER_PLAYERS;
		MASK_CAMERACOLLISION |= 1 << NPCS;
		MASK_CAMERACOLLISION |= 1 << TRIGGERS;
		MASK_CAMERACOLLISION |= 1 << CLICKIES;
		MASK_CAMERACOLLISION |= 1 << INVIZ_WALL;
		MASK_CAMERACOLLISION |= 1 << SELECTIONCAMERA;
		MASK_CAMERACOLLISION = ~MASK_CAMERACOLLISION;
		MASK_FLYTHROUGH |= 1 << DEFAULT;
		MASK_FLYTHROUGH |= 1 << WATER;
		MASK_FLYTHROUGH |= 1 << TERRAIN;
		MASK_FLYTHROUGH |= 1 << FAR;
		MASK_FLYTHROUGH |= 1 << MIDDLE;
		MASK_FLYTHROUGH |= 1 << MIDDLEFAR;
		MASK_FLYTHROUGH |= 1 << CLOSE;
		MASK_FLYTHROUGH |= 1 << NPCS;
		MASK_FLYTHROUGH |= 1 << BG;
		MASK_FLYTHROUGH |= 1 << SNOW;
		MASK_NAMEPLATE |= 1 << INVIZ_WALL;
		MASK_NAMEPLATE |= 1 << TRIGGERS;
		MASK_NAMEPLATE |= 1 << CLICKIES;
		MASK_NAMEPLATE |= 1 << NPCS;
		MASK_NAMEPLATE = ~MASK_NAMEPLATE;
		MASK_MY_NAMEPLATE |= 1 << INVIZ_WALL;
		MASK_MY_NAMEPLATE |= 1 << TRIGGERS;
		MASK_MY_NAMEPLATE |= 1 << CLICKIES;
		MASK_MY_NAMEPLATE |= 1 << NPCS;
		MASK_MY_NAMEPLATE |= 1 << PLAYER_ME;
		MASK_MY_NAMEPLATE = ~MASK_MY_NAMEPLATE;
	}

	public static int GetCam(int i)
	{
		return 1 | (1 << DEFAULT) | (1 << WATER) | (1 << TERRAIN) | (1 << FAR) | (1 << MIDDLE) | (1 << MIDDLEFAR) | (1 << CLOSE) | (1 << BG) | (1 << PLAYER_ME) | (1 << OTHER_PLAYERS) | (1 << NPCS);
	}

	public static int GetCutsceneMask(int i)
	{
		int num = 0;
		num |= 1 << DEFAULT;
		num |= 1 << WATER;
		num |= 1 << TERRAIN;
		num |= 1 << FAR;
		num |= 1 << MIDDLE;
		num |= 1 << MIDDLEFAR;
		num |= 1 << CLOSE;
		num |= 1 << BG;
		num |= 1 << SNOW;
		num |= 1 << SCREENTRANSITIONS;
		switch (i)
		{
		case 0:
			num |= 1 << CUTSCENE;
			break;
		case 1:
			num |= 1 << PLAYER_ME;
			num |= 1 << CUTSCENE;
			break;
		case 3:
			num |= 1 << PLAYER_ME;
			num |= 1 << CUTSCENE;
			num |= 1 << OTHER_PLAYERS;
			break;
		case 4:
			num |= 1 << PLAYER_ME;
			num |= 1 << CUTSCENE;
			num |= 1 << OTHER_PLAYERS;
			num |= 1 << NPCS;
			break;
		case 5:
			num |= 1 << NPCS;
			num |= 1 << CUTSCENE;
			num |= 1 << OTHER_PLAYERS;
			break;
		case 6:
			num |= 1 << PLAYER_ME;
			num |= 1 << CUTSCENE;
			num |= 1 << NPCS;
			break;
		case 7:
			num |= 1 << OTHER_PLAYERS;
			num |= 1 << NPCS;
			break;
		}
		return num;
	}

	public static float[] GetAdjustedCullDistances(float[] cullDistanceMin, float[] cullDistanceMax)
	{
		int num;
		if ((num = SettingsManager.DrawDistance) >= 4)
		{
			num = 10;
		}
		float[] array = new float[32];
		array[CLOSE] = cullDistanceMin[CLOSE] + (float)(num * 10);
		array[MIDDLE] = cullDistanceMin[MIDDLE] + (float)(num * 20);
		array[OTHER_PLAYERS] = cullDistanceMin[OTHER_PLAYERS] + (float)(num * 30);
		array[NPCS] = cullDistanceMin[NPCS] + (float)(num * 30);
		array[PHYSICS] = cullDistanceMin[PHYSICS] + (float)(num * 30);
		array[MIDDLEFAR] = cullDistanceMin[MIDDLEFAR] + (float)(num * 30);
		array[FAR] = cullDistanceMin[FAR] + (float)(num * 30);
		array[BG] = cullDistanceMin[BG] + (float)(num * 30);
		array[TRIGGERS] = cullDistanceMin[TRIGGERS] + (float)(num * 30);
		array[CLOSE] = ((array[CLOSE] > cullDistanceMax[CLOSE]) ? cullDistanceMax[CLOSE] : array[CLOSE]);
		array[MIDDLE] = ((array[MIDDLE] > cullDistanceMax[MIDDLE]) ? cullDistanceMax[MIDDLE] : array[MIDDLE]);
		array[OTHER_PLAYERS] = ((array[OTHER_PLAYERS] > cullDistanceMax[OTHER_PLAYERS]) ? cullDistanceMax[OTHER_PLAYERS] : array[OTHER_PLAYERS]);
		array[NPCS] = ((array[NPCS] > cullDistanceMax[NPCS]) ? cullDistanceMax[NPCS] : array[NPCS]);
		array[PHYSICS] = ((array[PHYSICS] > cullDistanceMax[PHYSICS]) ? cullDistanceMax[PHYSICS] : array[PHYSICS]);
		array[MIDDLEFAR] = ((array[MIDDLEFAR] > cullDistanceMax[MIDDLEFAR]) ? cullDistanceMax[MIDDLEFAR] : array[MIDDLEFAR]);
		array[FAR] = ((array[FAR] > cullDistanceMax[FAR]) ? cullDistanceMax[FAR] : array[FAR]);
		array[BG] = ((array[BG] > cullDistanceMax[BG]) ? cullDistanceMax[BG] : array[BG]);
		array[TRIGGERS] = ((array[TRIGGERS] > cullDistanceMax[TRIGGERS]) ? cullDistanceMax[TRIGGERS] : array[TRIGGERS]);
		return array;
	}
}
