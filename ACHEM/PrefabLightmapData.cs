using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PrefabLightmapData : MonoBehaviour
{
	[Serializable]
	public struct RendererInfo
	{
		public Renderer renderer;

		public int lightmapIndex;

		public Vector4 lightmapOffsetScale;
	}

	[Serializable]
	public struct TerrainInfo
	{
		public Terrain terrain;

		public int lightmapIndex;

		public Vector4 lightmapOffsetScale;
	}

	[HideInInspector]
	public RendererInfo[] m_RendererInfo;

	[HideInInspector]
	public TerrainInfo[] m_TerrainInfo;

	[HideInInspector]
	public Texture2D[] m_Lightmaps;

	public bool fog;

	public Color fogColor;

	public float fogDensity;

	public float fogEndDistance;

	public FogMode fogMode;

	public float fogStartDistance;

	private void Awake()
	{
		RenderSettings.fog = fog;
		RenderSettings.fogColor = fogColor;
		RenderSettings.fogDensity = fogDensity;
		RenderSettings.fogEndDistance = fogEndDistance;
		RenderSettings.fogMode = fogMode;
		RenderSettings.fogStartDistance = fogStartDistance;
		if ((m_RendererInfo != null && m_RendererInfo.Length != 0) || (m_TerrainInfo != null && m_TerrainInfo.Length != 0))
		{
			ApplyLightmaps();
		}
	}

	private void ApplyLightmaps()
	{
		LightmapData[] lightmaps = LightmapSettings.lightmaps;
		LightmapData[] array = new LightmapData[lightmaps.Length + m_Lightmaps.Length];
		lightmaps.CopyTo(array, 0);
		for (int i = 0; i < m_Lightmaps.Length; i++)
		{
			array[i + lightmaps.Length] = new LightmapData();
			array[i + lightmaps.Length].lightmapColor = m_Lightmaps[i];
		}
		if (m_RendererInfo != null)
		{
			ApplyRendererInfo(m_RendererInfo, lightmaps.Length);
		}
		if (m_TerrainInfo != null)
		{
			ApplyTerrainInfo(m_TerrainInfo, lightmaps.Length);
		}
		LightmapSettings.lightmaps = array;
		LightmapSettings.lightmapsMode = LightmapsMode.NonDirectional;
	}

	private static void ApplyRendererInfo(RendererInfo[] infos, int lightmapOffsetIndex)
	{
		for (int i = 0; i < infos.Length; i++)
		{
			RendererInfo rendererInfo = infos[i];
			if (rendererInfo.renderer == null)
			{
				Debug.LogError("Renderer is null at index: " + i);
				continue;
			}
			rendererInfo.renderer.lightmapIndex = rendererInfo.lightmapIndex + lightmapOffsetIndex;
			rendererInfo.renderer.lightmapScaleOffset = rendererInfo.lightmapOffsetScale;
			rendererInfo.renderer.shadowCastingMode = ShadowCastingMode.Off;
		}
	}

	private static void ApplyTerrainInfo(TerrainInfo[] infos, int lightmapOffsetIndex)
	{
		for (int i = 0; i < infos.Length; i++)
		{
			try
			{
				TerrainInfo terrainInfo = infos[i];
				terrainInfo.terrain.lightmapIndex = terrainInfo.lightmapIndex + lightmapOffsetIndex;
				terrainInfo.terrain.lightmapScaleOffset = terrainInfo.lightmapOffsetScale;
				terrainInfo.terrain.shadowCastingMode = ShadowCastingMode.Off;
			}
			catch (Exception ex)
			{
				Debug.LogError("index: " + i + " " + ex.ToString());
			}
		}
	}
}
