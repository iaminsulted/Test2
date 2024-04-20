using System;
using UnityEngine;
using UnityEngine.Serialization;

public class UIAudio : MonoBehaviour
{
	public UIToggle btnMaster;

	public UIToggle btnMusic;

	public UIToggle btnSound;

	public UIToggle btnCinematic;

	public UIToggle btnAmbient;

	public UISlider MasterSlider;

	public UISlider MusicSlider;

	public UISlider SFXSlider;

	public UISlider CinematicSlider;

	public UISlider AmbientSlider;

	[FormerlySerializedAs("GraphicsBox")]
	public UISettings UISettings;

	public UITable Table;

	public UIToggle btnFocus;

	public void OnEnable()
	{
		MasterSlider.value = SettingsManager.MasterVolume;
		MusicSlider.value = SettingsManager.MusicVolume;
		SFXSlider.value = SettingsManager.SFXVolume;
		CinematicSlider.value = SettingsManager.CinematicVolume;
		AmbientSlider.value = SettingsManager.AmbientVolume;
		btnMaster.value = SettingsManager.MasterEnabled;
		btnMusic.value = SettingsManager.MusicEnabled;
		btnSound.value = SettingsManager.SFXEnabled;
		btnCinematic.value = SettingsManager.CinematicEnabled;
		btnAmbient.value = SettingsManager.AmbientEnabled;
		btnFocus.value = SettingsManager.SoundOnlyWhenFocused;
		UISettings.SetAvailableOptions();
		Table.Reposition();
		UISlider musicSlider = MusicSlider;
		musicSlider.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(musicSlider.onDragFinished, new UIProgressBar.OnDragFinished(OnMusicSliderDragFinished));
		UISlider masterSlider = MasterSlider;
		masterSlider.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(masterSlider.onDragFinished, new UIProgressBar.OnDragFinished(OnMasterSliderDragFinished));
		UISlider sFXSlider = SFXSlider;
		sFXSlider.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(sFXSlider.onDragFinished, new UIProgressBar.OnDragFinished(OnSoundSliderDragFinished));
		UISlider cinematicSlider = CinematicSlider;
		cinematicSlider.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(cinematicSlider.onDragFinished, new UIProgressBar.OnDragFinished(OnCinematicSliderDragFinished));
		UISlider ambientSlider = AmbientSlider;
		ambientSlider.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(ambientSlider.onDragFinished, new UIProgressBar.OnDragFinished(OnAmbientSliderDragFinished));
	}

	public void OnDisable()
	{
		UISlider musicSlider = MusicSlider;
		musicSlider.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Remove(musicSlider.onDragFinished, new UIProgressBar.OnDragFinished(OnMusicSliderDragFinished));
		UISlider masterSlider = MasterSlider;
		masterSlider.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Remove(masterSlider.onDragFinished, new UIProgressBar.OnDragFinished(OnMasterSliderDragFinished));
		UISlider sFXSlider = SFXSlider;
		sFXSlider.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Remove(sFXSlider.onDragFinished, new UIProgressBar.OnDragFinished(OnSoundSliderDragFinished));
		UISlider cinematicSlider = CinematicSlider;
		cinematicSlider.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Remove(cinematicSlider.onDragFinished, new UIProgressBar.OnDragFinished(OnCinematicSliderDragFinished));
		UISlider ambientSlider = AmbientSlider;
		ambientSlider.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Remove(ambientSlider.onDragFinished, new UIProgressBar.OnDragFinished(OnAmbientSliderDragFinished));
	}

	public void ResetToDefault()
	{
		MasterSlider.value = SettingsManager.MasterVolume.Default;
		SettingsManager.MasterVolume.SaveToPrefs();
		MusicSlider.value = SettingsManager.MusicVolume.Default;
		SettingsManager.MusicVolume.SaveToPrefs();
		SFXSlider.value = SettingsManager.SFXVolume.Default;
		SettingsManager.SFXVolume.SaveToPrefs();
		CinematicSlider.value = SettingsManager.CinematicVolume.Default;
		SettingsManager.CinematicVolume.SaveToPrefs();
		AmbientSlider.value = SettingsManager.AmbientVolume.Default;
		SettingsManager.AmbientVolume.SaveToPrefs();
		btnMaster.value = SettingsManager.MasterEnabled.Default;
		SettingsManager.MasterEnabled.SaveToPrefs();
		btnMusic.value = SettingsManager.MusicEnabled.Default;
		SettingsManager.MusicEnabled.SaveToPrefs();
		btnSound.value = SettingsManager.SFXEnabled.Default;
		SettingsManager.SFXEnabled.SaveToPrefs();
		btnCinematic.value = SettingsManager.CinematicEnabled.Default;
		SettingsManager.CinematicEnabled.SaveToPrefs();
		btnAmbient.value = SettingsManager.AmbientEnabled.Default;
		SettingsManager.AmbientEnabled.SaveToPrefs();
		btnFocus.value = SettingsManager.SoundOnlyWhenFocused;
		SettingsManager.SoundOnlyWhenFocused.SaveToPrefs();
	}

	public void OnMasterChange()
	{
		if (btnMaster.value != (bool)SettingsManager.MasterEnabled)
		{
			SettingsManager.MasterEnabled.Set(btnMaster.value);
		}
	}

	public void OnMusicChange()
	{
		if (btnMusic.value != (bool)SettingsManager.MusicEnabled)
		{
			SettingsManager.MusicEnabled.Set(btnMusic.value);
		}
	}

	public void OnSoundChange()
	{
		if (btnSound.value != (bool)SettingsManager.SFXEnabled)
		{
			SettingsManager.SFXEnabled.Set(btnSound.value);
		}
	}

	public void OnCinematicChange()
	{
		if (btnCinematic.value != (bool)SettingsManager.CinematicEnabled)
		{
			SettingsManager.CinematicEnabled.Set(btnCinematic.value);
		}
	}

	public void OnAmbientChange()
	{
		if (btnAmbient.value != (bool)SettingsManager.AmbientEnabled)
		{
			SettingsManager.AmbientEnabled.Set(btnAmbient.value);
		}
	}

	public void OnFocusChange()
	{
		if (btnFocus.value != (bool)SettingsManager.SoundOnlyWhenFocused)
		{
			SettingsManager.SoundOnlyWhenFocused.Set(btnFocus.value);
		}
	}

	private void OnMasterSliderDragFinished()
	{
		SettingsManager.MasterVolume.SaveToPrefs();
	}

	private void OnMusicSliderDragFinished()
	{
		SettingsManager.MusicVolume.SaveToPrefs();
	}

	private void OnSoundSliderDragFinished()
	{
		SettingsManager.SFXVolume.SaveToPrefs();
	}

	private void OnCinematicSliderDragFinished()
	{
		SettingsManager.CinematicVolume.SaveToPrefs();
	}

	private void OnAmbientSliderDragFinished()
	{
		SettingsManager.AmbientVolume.SaveToPrefs();
	}

	public void OnMasterSliderChange()
	{
		SettingsManager.MasterVolume.Set(MasterSlider.value, updatePref: false, savePrefsToDisk: false);
	}

	public void OnMusicSliderChange()
	{
		SettingsManager.MusicVolume.Set(MusicSlider.value, updatePref: false, savePrefsToDisk: false);
	}

	public void OnSoundSliderChange()
	{
		SettingsManager.SFXVolume.Set(SFXSlider.value, updatePref: false, savePrefsToDisk: false);
	}

	public void OnCinematicSliderChange()
	{
		SettingsManager.CinematicVolume.Set(CinematicSlider.value, updatePref: false, savePrefsToDisk: false);
	}

	public void OnAmbientSliderChange()
	{
		SettingsManager.AmbientVolume.Set(AmbientSlider.value, updatePref: false, savePrefsToDisk: false);
	}
}
