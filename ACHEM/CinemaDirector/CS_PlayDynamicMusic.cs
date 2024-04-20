using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Artix", "Play Dynamic Music", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class CS_PlayDynamicMusic : CinemaGlobalEvent
{
	public AudioClip Clip;

	public bool Loop = true;

	public override void Trigger()
	{
		AudioManager.PlaySFXByMixerTrack(new MixerTrack
		{
			Loop = Loop,
			Clip = Clip
		});
	}
}
