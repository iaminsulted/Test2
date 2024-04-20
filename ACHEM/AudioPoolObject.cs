using System.Collections;
using UnityEngine;

public class AudioPoolObject : MonoBehaviour
{
	private AudioSource myAudioSource;

	private Coroutine coroutine;

	private bool isFollowing;

	private Transform toFollow;

	private float _timePlayed;

	public float timePlayed => _timePlayed;

	public AudioSource MyAudioSource
	{
		get
		{
			return myAudioSource;
		}
		set
		{
			myAudioSource = value;
		}
	}

	private void OnEnable()
	{
		if (!myAudioSource)
		{
			myAudioSource = GetComponent<AudioSource>();
		}
	}

	private void Update()
	{
		if (isFollowing && toFollow != null)
		{
			base.transform.position = toFollow.position;
		}
	}

	public void PlayAudioClip(AudioClip audioClip, SFXType type, Transform transformPosition = null, float delay = 0f, float volume = 1f, bool shouldFollow = false, bool is2D = false, float pitch = 1f, bool loop = false, float pan = 0f)
	{
		float num = (_timePlayed = (float)(AudioSettings.dspTime + (double)delay));
		myAudioSource.clip = audioClip;
		myAudioSource.volume = volume;
		myAudioSource.priority = (int)type;
		myAudioSource.pitch = pitch;
		myAudioSource.loop = loop;
		myAudioSource.panStereo = pan;
		if (shouldFollow && transformPosition != null)
		{
			toFollow = transformPosition;
			isFollowing = true;
			if (is2D)
			{
				Debug.LogError("A following sound with the 2D cannot be played");
			}
		}
		if ((bool)transformPosition && !is2D)
		{
			base.transform.position = transformPosition.position;
		}
		if (is2D)
		{
			base.transform.position = Vector3.zero;
			myAudioSource.spatialBlend = 0f;
		}
		myAudioSource.PlayScheduled(num);
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
			coroutine = null;
		}
		if (!loop)
		{
			coroutine = StartCoroutine(WaitForClipToFinish(delay));
		}
	}

	private IEnumerator WaitForClipToFinish(float time = 0f)
	{
		yield return new WaitForSeconds(myAudioSource.clip.length * (1f / myAudioSource.pitch) + time);
		Complete();
	}

	public void Complete()
	{
		myAudioSource.Stop();
		isFollowing = false;
		toFollow = null;
		_timePlayed = 0f;
		myAudioSource.spatialBlend = 1f;
		myAudioSource.priority = 128;
		myAudioSource.loop = false;
		myAudioSource.clip = null;
		AudioManager.AddToAudioQueue(base.gameObject);
	}

	private void OnDisable()
	{
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
			coroutine = null;
		}
	}
}
