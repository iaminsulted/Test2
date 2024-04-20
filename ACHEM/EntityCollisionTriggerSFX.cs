using System.Collections;
using UnityEngine;

public class EntityCollisionTriggerSFX : EntityCollisionTrigger
{
	public Transform targetTransform;

	public float delayStart;

	public float volumeOverTime;

	public float targetVolume;

	public bool forceStop;

	private AudioSource audioSource;

	private float volumeStart;

	private void Start()
	{
		audioSource = targetTransform.GetComponent<AudioSource>();
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
		if (!audioSource.loop)
		{
			while (audioSource.isPlaying)
			{
				yield return null;
			}
		}
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
