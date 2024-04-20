using System.Collections;
using System.Linq;
using UnityEngine;

public class EntityCollisionTriggerSFXFromBundle : EntityCollisionTrigger
{
	public string clipName;

	public Transform targetTransform;

	public float delayStart;

	public float volumeOverTime;

	public float targetVolume;

	public bool forceStop;

	private AudioSource audioSource;

	private float volumeStart;

	private void Start()
	{
		MixerTrack mixerTrack = AudioManager.gameSFXPlayer.MixerTracks.FirstOrDefault((MixerTrack x) => x.Clip.name == clipName);
		if (mixerTrack == null)
		{
			Chat.SendAdminMessage("GameAudio prefab in Maps is missing the audio clip " + clipName + ". Please add the audio track to the GameAudio prefab and rebuild.");
			return;
		}
		audioSource = targetTransform.GetComponent<AudioSource>();
		if (audioSource == null)
		{
			Chat.SendAdminMessage("Entity Collision Trigger SFX From Bundle is missing a target transform with an audio source component. Please add one to the target transform and rebuild.");
			return;
		}
		audioSource.clip = mixerTrack.Clip;
		volumeStart = audioSource.volume;
	}

	private void OnTriggerEnter(Collider other)
	{
		EntityController component = other.gameObject.GetComponent<EntityController>();
		if (component != null && CheckEntityCollision(component.Entity) && (bool)audioSource && audioSource.enabled)
		{
			if (Mathf.Approximately(delayStart, 0f))
			{
				audioSource.Play();
			}
			else
			{
				audioSource.PlayDelayed(delayStart);
			}
			StartCoroutine(VolumeOverTimeEnabled(targetVolume, delayStart));
		}
	}

	private void OnTriggerExit(Collider other)
	{
		EntityController component = other.gameObject.GetComponent<EntityController>();
		if (component != null && CheckEntityCollision(component.Entity) && (bool)audioSource && audioSource.enabled)
		{
			if (forceStop)
			{
				StartCoroutine(StopAudio());
			}
			StartCoroutine(VolumeOverTimeDisabled(volumeStart));
		}
	}

	private IEnumerator VolumeOverTimeEnabled(float volume, float delay)
	{
		float elapsedTime = 0f;
		float currentVolume = audioSource.volume;
		while (!Mathf.Approximately(audioSource.volume, volume))
		{
			elapsedTime += Time.deltaTime / (volumeOverTime + delay);
			audioSource.volume = Mathf.Lerp(currentVolume, volume, elapsedTime);
			yield return null;
		}
	}

	private IEnumerator VolumeOverTimeDisabled(float volume)
	{
		float elapsedTime = 0f;
		float currentVolume = audioSource.volume;
		while (!Mathf.Approximately(audioSource.volume, volume))
		{
			elapsedTime += Time.deltaTime / volumeOverTime;
			audioSource.volume = Mathf.Lerp(currentVolume, volume, elapsedTime);
			yield return null;
		}
		audioSource.Stop();
	}

	private IEnumerator StopAudio()
	{
		float elapsedTime = 0f;
		while (elapsedTime < volumeOverTime)
		{
			elapsedTime += Time.deltaTime / volumeOverTime;
			yield return null;
		}
		audioSource.Stop();
	}
}
