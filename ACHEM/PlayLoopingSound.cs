using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SelectChannel))]
public class PlayLoopingSound : MonoBehaviour
{
	public string SFXName;

	private AudioSource myAudioSource;

	private void Awake()
	{
		myAudioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		StartSound();
	}

	private void StartSound()
	{
		if (string.IsNullOrEmpty(SFXName) || AudioManager.gameSFXPlayer == null)
		{
			return;
		}
		MixerTrack mixerTrack = AudioManager.gameSFXPlayer.MixerTracks.FirstOrDefault((MixerTrack x) => string.Compare(x.Name, SFXName, StringComparison.OrdinalIgnoreCase) == 0);
		if (mixerTrack == null)
		{
			Debug.LogError("Can't play looping SFX " + SFXName + " since mixer track returned null");
			return;
		}
		if (myAudioSource == null)
		{
			myAudioSource = base.gameObject.AddComponent<AudioSource>();
		}
		myAudioSource.loop = true;
		myAudioSource.clip = mixerTrack.Clip;
		myAudioSource.spatialBlend = 0f;
		myAudioSource.volume = (mixerTrack.MinVolume + mixerTrack.MinVolume) * 0.1f;
	}
}
