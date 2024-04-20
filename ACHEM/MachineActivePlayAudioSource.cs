using System.Collections;
using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine Active Play Audio Source")]
public class MachineActivePlayAudioSource : MachineActive
{
	public float delayStart;

	public float volumeOverTime;

	public float targetVolume;

	public bool forceStop;

	private AudioSource audioSource;

	private float volumeStart;

	private void Start()
	{
		audioSource = base.TargetTransform.GetComponent<AudioSource>();
		volumeStart = audioSource.volume;
		InteractiveObject.ActiveUpdated += OnActiveUpdated;
	}

	private void OnActiveUpdated(bool active)
	{
		if (active)
		{
			if ((bool)audioSource && audioSource.enabled)
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
		else if ((bool)audioSource && audioSource.enabled)
		{
			if (forceStop)
			{
				StartCoroutine(StopAudio());
			}
			StartCoroutine(VolumeOverTimeDisabled(volumeStart));
		}
	}

	private void OnDestroy()
	{
		InteractiveObject.ActiveUpdated -= OnActiveUpdated;
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
