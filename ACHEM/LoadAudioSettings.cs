using UnityEngine;
using UnityEngine.Audio;

public class LoadAudioSettings : MonoBehaviour
{
	public AudioMixer audioMixer;

	public AudioMixerGroup masterChannel;

	public AudioMixerGroup soundEffectsChannel;

	public AudioMixerGroup backgroundMusicChannel;

	public AudioMixerGroup cinematicChannel;

	public AudioMixerGroup ambientChannel;

	private void Awake()
	{
		SettingsManager.MasterVolumeUpdated += SetMaster;
		SettingsManager.MasterEnabledUpdated += SetMaster;
		SettingsManager.SfxVolumeUpdated += SetSoundEffects;
		SettingsManager.SfxEnabledUpdated += SetSoundEffects;
		SettingsManager.MusicVolumeUpdated += SetBackgroundMusic;
		SettingsManager.MusicEnabledUpdated += SetBackgroundMusic;
		SettingsManager.CinematicVolumeUpdated += SetCinematic;
		SettingsManager.CinematicEnabledUpdated += SetCinematic;
		SettingsManager.AmbientVolumeUpdated += SetAmbient;
		SettingsManager.AmbientEnabledUpdated += SetAmbient;
	}

	private void Start()
	{
		SetMaster();
		SetBackgroundMusic();
		SetSoundEffects();
		SetCinematic();
		SetAmbient();
	}

	public void OnApplicationFocus(bool hasFocus)
	{
		if ((bool)SettingsManager.MasterEnabled && (bool)SettingsManager.SoundOnlyWhenFocused && Platform.IsDesktop)
		{
			SetMaster(hasFocus);
		}
	}

	private void SetBackgroundMusic(bool enabled)
	{
		float num = SettingsManager.MusicVolume;
		if (enabled)
		{
			if (num <= 0.0001f)
			{
				num = 0.0001f;
			}
		}
		else
		{
			num = 1E-07f;
		}
		audioMixer.SetFloat("BGM Volume", Mathf.Log(num) * 20f);
	}

	private void SetBackgroundMusic(float volume)
	{
		if ((bool)SettingsManager.MusicEnabled)
		{
			if (volume <= 0.0001f)
			{
				volume = 0.0001f;
			}
		}
		else
		{
			volume = 0.0001f;
		}
		audioMixer.SetFloat("BGM Volume", Mathf.Log(volume) * 20f);
	}

	private void SetBackgroundMusic()
	{
		float num = SettingsManager.MusicVolume;
		if ((bool)SettingsManager.MusicEnabled)
		{
			if (num <= 0.0001f)
			{
				num = 0.0001f;
			}
		}
		else
		{
			num = 0.0001f;
		}
		audioMixer.SetFloat("BGM Volume", Mathf.Log(num) * 20f);
	}

	private void SetSoundEffects(bool enabled)
	{
		float num = SettingsManager.SFXVolume;
		if (enabled)
		{
			if (num <= 0.0001f)
			{
				num = 0.0001f;
			}
		}
		else
		{
			num = 1E-06f;
		}
		audioMixer.SetFloat("SFX Volume", Mathf.Log(num) * 20f);
	}

	private void SetSoundEffects(float volume)
	{
		if ((bool)SettingsManager.SFXEnabled)
		{
			if (volume <= 0.0001f)
			{
				volume = 0.0001f;
			}
		}
		else
		{
			volume = 1E-06f;
		}
		audioMixer.SetFloat("SFX Volume", Mathf.Log(volume) * 20f);
	}

	private void SetSoundEffects()
	{
		float num = SettingsManager.SFXVolume;
		if ((bool)SettingsManager.SFXEnabled)
		{
			if (num <= 0.0001f)
			{
				num = 0.0001f;
			}
		}
		else
		{
			num = 1E-07f;
		}
		audioMixer.SetFloat("SFX Volume", Mathf.Log(num) * 20f);
	}

	private void SetMaster(bool enabled)
	{
		float num = SettingsManager.MasterVolume;
		if (enabled)
		{
			if (num <= 0.0001f)
			{
				num = 0.0001f;
			}
		}
		else
		{
			num = 1E-07f;
		}
		audioMixer.SetFloat("Master Volume", Mathf.Log(num) * 20f);
	}

	private void SetMaster(float volume)
	{
		if ((bool)SettingsManager.MasterEnabled)
		{
			if (volume <= 0.0001f)
			{
				volume = 0.0001f;
			}
		}
		else
		{
			volume = 1E-07f;
		}
		audioMixer.SetFloat("Master Volume", Mathf.Log(volume) * 20f);
	}

	private void SetMaster()
	{
		float num = SettingsManager.MasterVolume;
		if ((bool)SettingsManager.MasterEnabled)
		{
			if (num <= 0.0001f)
			{
				num = 0.0001f;
			}
		}
		else
		{
			num = 1E-07f;
		}
		audioMixer.SetFloat("Master Volume", Mathf.Log(num) * 20f);
	}

	private void SetCinematic(bool enabled)
	{
		float num = SettingsManager.CinematicVolume;
		if (enabled)
		{
			if (num <= 0.0001f)
			{
				num = 0.0001f;
			}
		}
		else
		{
			num = 1E-07f;
		}
		audioMixer.SetFloat("Cinematic Volume", Mathf.Log(num) * 20f);
	}

	private void SetCinematic(float volume)
	{
		if ((bool)SettingsManager.CinematicEnabled)
		{
			if (volume <= 0.0001f)
			{
				volume = 0.0001f;
			}
		}
		else
		{
			volume = 1E-07f;
		}
		audioMixer.SetFloat("Cinematic Volume", Mathf.Log(volume) * 20f);
	}

	private void SetCinematic()
	{
		float num = SettingsManager.CinematicVolume;
		if ((bool)SettingsManager.CinematicEnabled)
		{
			if (num <= 0.0001f)
			{
				num = 0.0001f;
			}
		}
		else
		{
			num = 1E-07f;
		}
		audioMixer.SetFloat("Cinematic Volume", Mathf.Log(num) * 20f);
	}

	private void SetAmbient(bool enabled)
	{
		float num = SettingsManager.AmbientVolume;
		if (enabled)
		{
			if (num <= 0.0001f)
			{
				num = 0.0001f;
			}
		}
		else
		{
			num = 1E-07f;
		}
		audioMixer.SetFloat("Ambient Volume", Mathf.Log(num) * 20f);
	}

	private void SetAmbient(float volume)
	{
		if ((bool)SettingsManager.AmbientEnabled)
		{
			if (volume <= 0.0001f)
			{
				volume = 0.0001f;
			}
		}
		else
		{
			volume = 1E-07f;
		}
		audioMixer.SetFloat("Ambient Volume", Mathf.Log(volume) * 20f);
	}

	private void SetAmbient()
	{
		float num = SettingsManager.AmbientVolume;
		if ((bool)SettingsManager.AmbientEnabled)
		{
			if (num <= 0.0001f)
			{
				num = 0.0001f;
			}
		}
		else
		{
			num = 1E-07f;
		}
		audioMixer.SetFloat("Ambient Volume", Mathf.Log(num) * 20f);
	}

	private void OnDisable()
	{
		SettingsManager.MasterVolumeUpdated -= SetMaster;
		SettingsManager.MasterEnabledUpdated -= SetMaster;
		SettingsManager.SfxVolumeUpdated -= SetSoundEffects;
		SettingsManager.SfxEnabledUpdated -= SetSoundEffects;
		SettingsManager.MusicVolumeUpdated -= SetBackgroundMusic;
		SettingsManager.MusicEnabledUpdated -= SetBackgroundMusic;
		SettingsManager.CinematicVolumeUpdated -= SetCinematic;
		SettingsManager.CinematicEnabledUpdated -= SetCinematic;
	}
}
