namespace CinemaDirector;

[CutsceneItem("Artix", "Play Custom Audio Clip", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class CS_PlayAudioClip : CinemaGlobalEvent
{
	public MixerTrack Audio;

	public bool is3D;

	public string GroupName = "Battle";

	public override void Trigger()
	{
		AudioManager.PlaySFXByMixerTrack(Audio, SFXType.NotClassified, base.gameObject.transform);
	}
}
