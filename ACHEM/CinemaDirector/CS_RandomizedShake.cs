using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Artix", "Ranomized Shake", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class CS_RandomizedShake : CinemaActorAction
{
	public AnimationCurve intensityCurve;

	public Vector3 CameraShakeIntensity = Vector3.one;

	private Vector3 startingPos;

	private Vector3 randVec;

	public override void Trigger(GameObject Actor)
	{
		startingPos = Actor.transform.position;
	}

	public override void End(GameObject Actor)
	{
		Actor.transform.position = startingPos;
	}

	public override void ReverseEnd(GameObject Actor)
	{
		startingPos = Actor.transform.position;
	}

	public override void ReverseTrigger(GameObject Actor)
	{
		Actor.transform.position = startingPos;
	}

	public override void Stop(GameObject Actor)
	{
		Actor.transform.position = startingPos;
	}

	public override void UpdateTime(GameObject Actor, float time, float deltaTime)
	{
		randVec = new Vector3(Random.Range(0f - CameraShakeIntensity.x, CameraShakeIntensity.x), Random.Range(0f - CameraShakeIntensity.y, CameraShakeIntensity.y), Random.Range(0f - CameraShakeIntensity.z, CameraShakeIntensity.z));
		Actor.transform.position = startingPos + randVec * intensityCurve.Evaluate(time / base.Duration);
	}
}
