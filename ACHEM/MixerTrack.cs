using System;
using UnityEngine;

[Serializable]
public class MixerTrack
{
	public enum TagType
	{
		NONE,
		ATTACK,
		DEATH,
		TRAVELFORM
	}

	[ReadOnly]
	public string Name;

	public AudioClip Clip;

	public TagType Tag;

	public bool Loop;

	public float MinVolume = 0.3f;

	public float MaxVolume = 0.3f;

	public bool AutoPlay;

	public float MinPitch = 1f;

	public float MaxPitch = 1f;

	public float Volume
	{
		get
		{
			if (MinVolume == MaxVolume)
			{
				return MinVolume;
			}
			return UnityEngine.Random.Range(MinVolume, MaxVolume);
		}
	}

	public float Pitch
	{
		get
		{
			if (MinPitch == 1f && MaxPitch == 1f)
			{
				return 1f;
			}
			return UnityEngine.Random.Range(MinPitch, MaxPitch);
		}
	}

	public static MixerTrack Clone(MixerTrack mt)
	{
		return new MixerTrack
		{
			Name = mt.Name,
			Clip = mt.Clip,
			Loop = mt.Loop,
			MinVolume = mt.MinVolume,
			MaxVolume = mt.MaxVolume,
			MinPitch = mt.MinPitch,
			MaxPitch = mt.MaxPitch
		};
	}
}
