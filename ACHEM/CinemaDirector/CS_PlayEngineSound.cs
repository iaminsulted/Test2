namespace CinemaDirector;

[CutsceneItem("Artix", "Play Engine Sound", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class CS_PlayEngineSound : CinemaGlobalEvent
{
	public string ClipName;

	public string GroupName = "Battle";

	public override void Trigger()
	{
		AudioManager.Play2DSFX(ClipName);
	}
}
