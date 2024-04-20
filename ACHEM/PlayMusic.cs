using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayMusic : MonoBehaviour
{
	private AudioSource source;

	private void Start()
	{
		source = GetComponent<AudioSource>();
		if ((bool)SettingsManager.MusicEnabled)
		{
			source.Play();
		}
		SettingsManager.MusicEnabledUpdated += Session_MusicStateChanged;
	}

	private void Session_MusicStateChanged(bool isEnabled)
	{
		if (isEnabled)
		{
			source.Play();
		}
		else
		{
			source.Stop();
		}
	}

	public void OnDestroy()
	{
		source.Stop();
		SettingsManager.MusicEnabledUpdated -= Session_MusicStateChanged;
	}
}
