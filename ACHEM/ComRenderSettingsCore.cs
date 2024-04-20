using System.Collections.Generic;

public class ComRenderSettingsCore
{
	public List<int> _backgroundColor;

	public List<int> _ambientLight;

	public List<int> _fogColor;

	public bool fog;

	public string _fogMode;

	public float fogDensity;

	public float fogStartDistance;

	public float fogEndDistance;

	public float haloStrength;

	public float flareStrength;

	public string _skybox;

	public bool sunRays;

	public bool colorCorrection;

	public bool dof;

	public bool bloom;

	public string bloomMode;

	public float bloomIntensity;

	public float bloomThreshhold;

	public float bloomBlurSpread;
}
