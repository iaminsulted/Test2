namespace CinemaDirector;

[CutsceneItem("Artix", "Play Engine Music", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class CS_PlayEngineMusic : CinemaGlobalEvent
{
	public string ClipName;

	public override void Trigger()
	{
		AudioManager.Play2DSFX(ClipName);
	}
}
