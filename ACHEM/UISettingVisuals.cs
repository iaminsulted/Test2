using UnityEngine;
using UnityEngine.Serialization;

public class UISettingVisuals : MonoBehaviour
{
	public UIPopupList ResolutionDropdown;

	public UIPopupList DisplayModeDropdown;

	public UIPopupList VsyncDropdown;

	public UIPopupList FpsLimitDropdown;

	public UIPopupList GraphicsPresetDropdown;

	public UIPopupList TextureQualityDropdown;

	public UIPopupList AnisotropicDropdown;

	public UIPopupList AntiAliasingDropdown;

	public UIPopupList ShadowQualityDropdown;

	public UIPopupList ShadowDistanceDropdown;

	public UIPopupList ParticleQualityDropdown;

	public UIPopupList HDParticleDropdown;

	public UIPopupList DrawDistanceDropdown;

	public UIPopupList ParticleLimitDropdown;

	public UIPopupList InterfaceModeDropdown;

	public UISlider GrassSlider;

	public UISlider VignetteOpacitySlider;

	private readonly Vector2 GrassDistanceRange = new Vector2(0f, 400f);

	private readonly Vector2 VignetteOpacityRange = new Vector2(0f, 1f);

	public UIToggle BloomToggle;

	public UIToggle DepthToggle;

	public UIToggle GrassToggle;

	[FormerlySerializedAs("GraphicsBox")]
	public UISettings UISettings;

	public UITable Table;

	public int graphicsPreset;

	private void OnEnable()
	{
		Init();
		UISettings.SetAvailableOptions();
		Table.Reposition();
	}

	private void Init()
	{
		if (Platform.IsDesktop || Platform.IsEditor)
		{
			InitResolution();
			InitFpsLimit();
		}
		DisplayModeDropdown.SetToIndex(SettingsManager.DisplayMode);
		VsyncDropdown.SetToIndex(SettingsManager.VerticalSync);
		GraphicsPresetDropdown.SetToIndex(SettingsManager.GraphicsQuality);
		graphicsPreset = SettingsManager.GraphicsQuality;
		TextureQualityDropdown.SetToIndex(SettingsManager.TextureQuality);
		AnisotropicDropdown.SetToIndex(SettingsManager.AnisotropicFilter);
		AntiAliasingDropdown.SetToIndex(SettingsManager.AntiAliasing);
		ShadowQualityDropdown.SetToIndex(SettingsManager.ShadowQuality);
		ShadowDistanceDropdown.SetToIndex(SettingsManager.ShadowDistance);
		ParticleQualityDropdown.SetToIndex(SettingsManager.ParticleRaycasts);
		HDParticleDropdown.SetToIndex(SettingsManager.ParticleQuality);
		DrawDistanceDropdown.SetToIndex(SettingsManager.DrawDistance);
		ParticleLimitDropdown.SetToIndex(SettingsManager.ParticleLimit);
		InterfaceModeDropdown.SetToIndex(SettingsManager.InterfaceMode);
		GrassSlider.SetValue(SettingsManager.GrassDistance, GrassDistanceRange);
		VignetteOpacitySlider.SetValue(SettingsManager.VignetteOpacity, VignetteOpacityRange);
		DepthToggle.value = SettingsManager.UseDepthOfField;
		BloomToggle.value = SettingsManager.UseBloom;
		GrassToggle.value = SettingsManager.ShowMobileGrass;
	}

	private void InitResolution()
	{
		ResolutionDropdown.Clear();
		foreach (Resolution availableResolution in SettingsManager.AvailableResolutions)
		{
			ResolutionDropdown.AddItem(SettingsManager.ResolutionName(availableResolution));
		}
		string value = SettingsManager.ResolutionName(SettingsManager.CurrentResolution);
		ResolutionDropdown.Set(value);
	}

	private void InitFpsLimit()
	{
		FpsLimitDropdown.Clear();
		foreach (int availableFpsLimit in SettingsManager.AvailableFpsLimits)
		{
			string text = ((availableFpsLimit == -1) ? "Unlimited" : (availableFpsLimit + " fps"));
			FpsLimitDropdown.AddItem(text);
		}
		FpsLimitDropdown.SetToIndex(SettingsManager.FpsLimit);
	}

	public void ResetToDefault()
	{
		ResolutionDropdown.SetToIndex(ResolutionDropdown.items.Count - 1);
		DisplayModeDropdown.SetToIndex(SettingsManager.DisplayMode.Default);
		VsyncDropdown.SetToIndex(SettingsManager.VerticalSync.Default);
		FpsLimitDropdown.SetToIndex(SettingsManager.FpsLimit.Default);
		GraphicsPresetDropdown.SetToIndex(SettingsManager.GraphicsQuality.Default);
		graphicsPreset = SettingsManager.GraphicsQuality.Default;
		TextureQualityDropdown.SetToIndex(SettingsManager.TextureQuality.Default);
		AnisotropicDropdown.SetToIndex(SettingsManager.AnisotropicFilter.Default);
		AntiAliasingDropdown.SetToIndex(SettingsManager.AntiAliasing.Default);
		ShadowQualityDropdown.SetToIndex(SettingsManager.ShadowQuality.Default);
		ShadowDistanceDropdown.SetToIndex(SettingsManager.ShadowDistance.Default);
		ParticleQualityDropdown.SetToIndex(SettingsManager.ParticleRaycasts.Default);
		HDParticleDropdown.SetToIndex(SettingsManager.ParticleQuality.Default);
		DrawDistanceDropdown.SetToIndex(SettingsManager.DrawDistance.Default);
		ParticleLimitDropdown.SetToIndex(SettingsManager.ParticleLimit.Default);
		InterfaceModeDropdown.SetToIndex(SettingsManager.InterfaceMode.Default);
		GrassSlider.SetValue(SettingsManager.GrassDistance.Default, GrassDistanceRange);
		VignetteOpacitySlider.SetValue(SettingsManager.VignetteOpacity.Default, VignetteOpacityRange);
		DepthToggle.value = SettingsManager.UseDepthOfField.Default;
		BloomToggle.value = SettingsManager.UseBloom.Default;
		GrassToggle.value = SettingsManager.ShowMobileGrass.Default;
	}

	public void SaveSettings()
	{
		SettingsManager.SaveAndApplyResolutionSettings(ResolutionDropdown.CurrentIndex(), DisplayModeDropdown.CurrentIndex());
		SettingsManager.SaveGraphicsSettings(graphicsPreset, TextureQualityDropdown.CurrentIndex(), AnisotropicDropdown.CurrentIndex(), AntiAliasingDropdown.CurrentIndex(), ShadowQualityDropdown.CurrentIndex(), ShadowDistanceDropdown.CurrentIndex(), ParticleQualityDropdown.CurrentIndex(), HDParticleDropdown.CurrentIndex(), DrawDistanceDropdown.CurrentIndex(), ParticleLimitDropdown.CurrentIndex(), GrassSlider.GetValue(GrassDistanceRange), VignetteOpacitySlider.GetValue(VignetteOpacityRange), BloomToggle.value, DepthToggle.value, VsyncDropdown.CurrentIndex(), FpsLimitDropdown.CurrentIndex(), InterfaceModeDropdown.CurrentIndex());
		Notification.ShowText("Settings saved successfully!");
	}

	public void OnVsyncChange()
	{
		FpsLimitDropdown.gameObject.SetActive(VsyncDropdown.CurrentIndex() == 0);
		Table.Reposition();
		if (VsyncDropdown.CurrentIndex() == 0)
		{
			InitFpsLimit();
		}
	}

	public void OnInterfaceModeChange()
	{
		Debug.Log("CHANGED " + InterfaceModeDropdown.CurrentIndex());
	}

	public void OnGraphicsPresetChange()
	{
		graphicsPreset = GraphicsPresetDropdown.CurrentIndex();
		switch (GraphicsPresetDropdown.CurrentIndex())
		{
		case 0:
			TextureQualityDropdown.SetToIndex(0);
			AnisotropicDropdown.SetToIndex(0);
			AntiAliasingDropdown.SetToIndex(0);
			ShadowQualityDropdown.SetToIndex(0);
			ShadowDistanceDropdown.SetToIndex(0);
			ParticleQualityDropdown.SetToIndex(0);
			HDParticleDropdown.SetToIndex(0);
			DrawDistanceDropdown.SetToIndex(0);
			ParticleLimitDropdown.SetToIndex(0);
			GrassSlider.SetValue(0f, GrassDistanceRange);
			break;
		case 1:
			TextureQualityDropdown.SetToIndex(1);
			AnisotropicDropdown.SetToIndex(0);
			AntiAliasingDropdown.SetToIndex(0);
			ShadowQualityDropdown.SetToIndex(0);
			ShadowDistanceDropdown.SetToIndex(0);
			ParticleQualityDropdown.SetToIndex(1);
			HDParticleDropdown.SetToIndex(0);
			DrawDistanceDropdown.SetToIndex(0);
			ParticleLimitDropdown.SetToIndex(1);
			GrassSlider.SetValue(0f, GrassDistanceRange);
			break;
		case 2:
			TextureQualityDropdown.SetToIndex(1);
			AnisotropicDropdown.SetToIndex(1);
			AntiAliasingDropdown.SetToIndex(0);
			ShadowQualityDropdown.SetToIndex(1);
			ShadowDistanceDropdown.SetToIndex(0);
			ParticleQualityDropdown.SetToIndex(2);
			HDParticleDropdown.SetToIndex(0);
			DrawDistanceDropdown.SetToIndex(1);
			ParticleLimitDropdown.SetToIndex(2);
			GrassSlider.SetValue(0f, GrassDistanceRange);
			break;
		case 3:
			TextureQualityDropdown.SetToIndex(1);
			AnisotropicDropdown.SetToIndex(1);
			AntiAliasingDropdown.SetToIndex(1);
			ShadowQualityDropdown.SetToIndex(2);
			ShadowDistanceDropdown.SetToIndex(1);
			ParticleQualityDropdown.SetToIndex(3);
			HDParticleDropdown.SetToIndex(0);
			DrawDistanceDropdown.SetToIndex(2);
			ParticleLimitDropdown.SetToIndex(3);
			GrassSlider.SetValue(60f, GrassDistanceRange);
			break;
		case 4:
			TextureQualityDropdown.SetToIndex(2);
			AnisotropicDropdown.SetToIndex(2);
			AntiAliasingDropdown.SetToIndex(2);
			ShadowQualityDropdown.SetToIndex(3);
			ShadowDistanceDropdown.SetToIndex(2);
			ParticleQualityDropdown.SetToIndex(4);
			HDParticleDropdown.SetToIndex(0);
			DrawDistanceDropdown.SetToIndex(3);
			ParticleLimitDropdown.SetToIndex(4);
			GrassSlider.SetValue(200f, GrassDistanceRange);
			break;
		case 5:
			TextureQualityDropdown.SetToIndex(2);
			AnisotropicDropdown.SetToIndex(2);
			AntiAliasingDropdown.SetToIndex(3);
			ShadowQualityDropdown.SetToIndex(4);
			ShadowDistanceDropdown.SetToIndex(3);
			ParticleQualityDropdown.SetToIndex(5);
			HDParticleDropdown.SetToIndex(1);
			DrawDistanceDropdown.SetToIndex(4);
			ParticleLimitDropdown.SetToIndex(5);
			GrassSlider.SetValue(400f, GrassDistanceRange);
			break;
		}
	}
}
