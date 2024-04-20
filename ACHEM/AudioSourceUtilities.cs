using System.Collections;
using UnityEngine;

public class AudioSourceUtilities : MonoBehaviour
{
	private const float BlendTime = 3f;

	private AudioSource myAudioSource;

	private Coroutine blend;

	private void Awake()
	{
		myAudioSource = GetComponent<AudioSource>();
	}

	public void PlayClip(AudioClip audioClip)
	{
		if (blend != null)
		{
			StopCoroutine(blend);
		}
		blend = StartCoroutine(Blend(audioClip));
	}

	private IEnumerator Blend(AudioClip audioClip)
	{
		if (myAudioSource.isPlaying)
		{
			if (myAudioSource.clip != audioClip)
			{
				while (myAudioSource.volume > 0.0001f && (bool)SettingsManager.MusicEnabled)
				{
					myAudioSource.volume -= Time.deltaTime / 3f;
					yield return null;
				}
				myAudioSource.clip = audioClip;
				myAudioSource.volume = 0f;
				myAudioSource.Play();
				float targetVolume;
				do
				{
					targetVolume = 1f;
					myAudioSource.volume += Time.deltaTime / 3f;
					if (myAudioSource.volume > targetVolume)
					{
						myAudioSource.volume = targetVolume;
					}
					yield return null;
				}
				while (myAudioSource.volume < targetVolume);
			}
		}
		else
		{
			myAudioSource.clip = audioClip;
			myAudioSource.volume = 0f;
			myAudioSource.Play();
			float targetVolume;
			do
			{
				targetVolume = 1f;
				myAudioSource.volume += Time.deltaTime / 3f;
				if (myAudioSource.volume > targetVolume)
				{
					myAudioSource.volume = targetVolume;
				}
				yield return null;
			}
			while (myAudioSource.volume < targetVolume);
		}
		blend = null;
	}

	public void FadeAndStop(float fadeSpeed)
	{
		StartCoroutine(FadeOut(fadeSpeed));
	}

	private IEnumerator FadeOut(float fadeSpeed)
	{
		while (myAudioSource.volume > 0f)
		{
			yield return new WaitForEndOfFrame();
			myAudioSource.volume -= Time.deltaTime * fadeSpeed;
		}
	}
}
