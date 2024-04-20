using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
	public List<MixerTrack> MixerTracks;

	private void Start()
	{
		foreach (MixerTrack mixerTrack in MixerTracks)
		{
			if (mixerTrack.AutoPlay)
			{
				Play(mixerTrack);
			}
		}
	}

	public void Animation2DSoundEvent(AnimationEvent ae)
	{
		MixerTrack mixerTrack = FindMixerTrack(ae.stringParameter);
		if (mixerTrack != null)
		{
			AudioManager.Play2DSFX(mixerTrack.Clip);
		}
		else
		{
			AudioManager.Play2DSFX(ae.stringParameter);
		}
	}

	public void Animation3DSoundEvent(AnimationEvent ae)
	{
		MixerTrack mixerTrack = FindMixerTrack(ae.stringParameter);
		if (mixerTrack != null)
		{
			AudioManager.PlaySFXByMixerTrack(mixerTrack, SFXType.NotClassified, base.transform);
		}
		else
		{
			AudioManager.Play3DSFX(ae.stringParameter, SFXType.Ambient, base.transform);
		}
	}

	public void Play(MixerTrack.TagType tag, SFXType sfxType = SFXType.NotClassified, Transform t = null, float minVolOverride = -1f, float maxVolOverride = -1f, float minPitchOverride = -1f, float maxPitchOverride = -1f, float playDelay = 0f)
	{
		MixerTrack mixerTrack = FindTrackByTag(tag);
		if (mixerTrack != null)
		{
			Play(mixerTrack, sfxType, t, minVolOverride, maxVolOverride, minPitchOverride, maxPitchOverride, playDelay);
		}
	}

	public void Play(string name, SFXType sfxType = SFXType.NotClassified, Transform t = null, float minVolOverride = -1f, float maxVolOverride = -1f, float minPitchOverride = -1f, float maxPitchOverride = -1f, float playDelay = 0f)
	{
		MixerTrack mixerTrack = FindMixerTrack(name);
		if (mixerTrack != null)
		{
			Play(mixerTrack, sfxType, t, minVolOverride, maxVolOverride, minPitchOverride, maxPitchOverride, playDelay);
		}
		else
		{
			AudioManager.Play3DSFX(name, sfxType, t, playDelay);
		}
	}

	public void Play(MixerTrack mt, SFXType type = SFXType.NotClassified, Transform t = null, float minVolOverride = -1f, float maxVolOverride = -1f, float minPitchOverride = -1f, float maxPitchOverride = -1f, float playDelay = 0f)
	{
		if (mt != null)
		{
			if ((minVolOverride > -1f && maxVolOverride > -1f) || (minPitchOverride > -1f && minPitchOverride > -1f))
			{
				MixerTrack mixerTrack = MixerTrack.Clone(mt);
				mixerTrack.MinVolume = ((minVolOverride > -1f) ? minVolOverride : mt.MinVolume);
				mixerTrack.MaxVolume = ((maxVolOverride > -1f) ? maxVolOverride : mt.MaxVolume);
				mixerTrack.MinPitch = ((minPitchOverride > -1f) ? minPitchOverride : mt.MinPitch);
				mixerTrack.MaxPitch = ((maxPitchOverride > -1f) ? maxPitchOverride : mt.MaxPitch);
				AudioManager.PlaySFXByMixerTrack(mixerTrack, type, t, playDelay);
			}
			else
			{
				AudioManager.PlaySFXByMixerTrack(mt, type, t, playDelay);
			}
		}
	}

	private MixerTrack FindMixerTrack(string name)
	{
		if (MixerTracks.Count > 0)
		{
			MixerTrack mixerTrack = MixerTracks.FirstOrDefault((MixerTrack x) => string.Equals(x.Clip.name, name, StringComparison.OrdinalIgnoreCase));
			if (mixerTrack != null)
			{
				return mixerTrack;
			}
		}
		return null;
	}

	public bool HasTrackWithTag(MixerTrack.TagType tag)
	{
		return MixerTracks.Any((MixerTrack x) => x.Tag == tag);
	}

	private MixerTrack FindTrackByTag(MixerTrack.TagType tag)
	{
		if (tag == MixerTrack.TagType.NONE)
		{
			return null;
		}
		return MixerTracks.FirstOrDefault((MixerTrack x) => x.Tag == tag);
	}

	public void AddMixerTrack(MixerTrack track)
	{
		MixerTracks.Remove(MixerTracks.Find((MixerTrack p) => p.Clip.name == track.Clip.name));
		MixerTracks.Add(track);
	}
}
