using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Artix", "Move Towards", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class CS_MoveTowards : CinemaActorAction
{
	public bool useCurve;

	public AnimationCurve intensityCurve;

	public Transform target;

	public float stoppingDistance = 1f;

	public bool faceTarget;

	private Vector3 startingPos;

	private Vector3 heading;

	private Vector3 startHeading;

	public override void Trigger(GameObject Actor)
	{
		startingPos = Actor.transform.position;
		startHeading = Actor.transform.forward;
	}

	public override void ReverseTrigger(GameObject Actor)
	{
		Actor.transform.position = startingPos;
		Actor.transform.forward = startHeading;
	}

	public override void End(GameObject Actor)
	{
	}

	public override void Stop(GameObject Actor)
	{
		Actor.transform.position = startingPos;
		Actor.transform.forward = startHeading;
	}

	public override void UpdateTime(GameObject Actor, float time, float deltaTime)
	{
		heading = target.position - Actor.transform.position;
		heading /= heading.magnitude;
		float t = ((!useCurve) ? (time / base.Duration) : intensityCurve.Evaluate(time / base.Duration));
		Actor.transform.position = Vector3.Lerp(startingPos, target.position - heading * stoppingDistance, t);
		if (faceTarget)
		{
			Actor.transform.forward = heading;
		}
	}

	public override void SetTime(GameObject Actor, float time, float deltaTime)
	{
		if (time > 0f && time < base.Duration)
		{
			UpdateTime(Actor, time, deltaTime);
		}
		if (time > base.Duration)
		{
			UpdateTime(Actor, base.Duration, deltaTime);
		}
		if (time < 0f)
		{
			startingPos = Actor.transform.position;
			startHeading = Actor.transform.forward;
		}
	}
}
