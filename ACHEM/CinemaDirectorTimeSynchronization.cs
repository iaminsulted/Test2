using CinemaDirector;

public class CinemaDirectorTimeSynchronization : TimeSynchronization
{
	public Cutscene cutScene;

	protected override void SetState(float time)
	{
		cutScene.RunningTime = time;
		cutScene.Play();
	}
}
