using UnityEngine;

public class SelectChannel : MonoBehaviour
{
	public enum AudioChannel
	{
		Master,
		SFX,
		BGM,
		Cinematic,
		Ambient
	}

	public AudioChannel myAudioChannel;

	private void Start()
	{
		switch (myAudioChannel)
		{
		case AudioChannel.Master:
			GetComponent<AudioSource>().outputAudioMixerGroup = Object.FindObjectOfType<LoadAudioSettings>().masterChannel;
			break;
		case AudioChannel.SFX:
			GetComponent<AudioSource>().outputAudioMixerGroup = Object.FindObjectOfType<LoadAudioSettings>().soundEffectsChannel;
			break;
		case AudioChannel.BGM:
			GetComponent<AudioSource>().outputAudioMixerGroup = Object.FindObjectOfType<LoadAudioSettings>().backgroundMusicChannel;
			break;
		case AudioChannel.Cinematic:
			GetComponent<AudioSource>().outputAudioMixerGroup = Object.FindObjectOfType<LoadAudioSettings>().cinematicChannel;
			break;
		case AudioChannel.Ambient:
			GetComponent<AudioSource>().outputAudioMixerGroup = Object.FindObjectOfType<LoadAudioSettings>().ambientChannel;
			break;
		default:
			GetComponent<AudioSource>().outputAudioMixerGroup = Object.FindObjectOfType<LoadAudioSettings>().masterChannel;
			break;
		}
	}
}
