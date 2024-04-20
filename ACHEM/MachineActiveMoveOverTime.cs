using StatCurves;
using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine Active Move Over Time")]
public class MachineActiveMoveOverTime : MachineActiveNonStatic, ITimedListener
{
	public Vector3 PositionTo;

	public float TimeToMove;

	public Interpolator.EaseType EaseType;

	public Interpolator.SpaceType SpaceType;

	public Interpolator.TransformationType TransformationType;

	private Vector3 start;

	private Vector3 end;

	private Vector3 current;

	private Vector3 target;

	private float distance;

	private float timeElapsed;

	public float LastServerTimestamp => lastServerTimeStamp;

	private void Start()
	{
		if (InteractiveObject is ClientTrigger)
		{
			Init();
			SetTransformation();
		}
	}

	private void OnDestroy()
	{
		InteractiveObject.ActiveUpdated -= OnMachineActiveUpdate;
	}

	private void Update()
	{
		if (update)
		{
			timeElapsed += GameTime.realtimeSinceServerStartup - lastServerTimeStamp;
			lastServerTimeStamp = GameTime.realtimeSinceServerStartup;
			ApplyAction();
		}
	}

	public void Init()
	{
		start = base.TargetTransform.position;
		switch (SpaceType)
		{
		case Interpolator.SpaceType.Local:
			end = base.TargetTransform.position + PositionTo;
			break;
		case Interpolator.SpaceType.Global:
			end = PositionTo;
			break;
		case Interpolator.SpaceType.Parent:
			end = base.TargetTransform.parent.TransformPoint(PositionTo);
			break;
		}
		distance = Mathf.Abs(Vector3.Distance(start, end));
		InteractiveObject.ActiveUpdated += OnMachineActiveUpdate;
	}

	public void SyncToServer(bool last)
	{
		if (last)
		{
			if (TransformationType == Interpolator.TransformationType.Current)
			{
				float num = GameTime.realtimeSinceServerStartup - lastServerTimeStamp;
				if (num < 0f)
				{
					num = 0f;
				}
				else if (num > TimeToMove)
				{
					num = TimeToMove;
				}
				float num2 = ((TimeToMove > 0f) ? (num / TimeToMove) : 1f);
				timeElapsed += num;
				if (InteractiveObject.IsActive)
				{
					current = base.TargetTransform.position;
					target = base.TargetTransform.position + PositionTo;
				}
				else
				{
					current = base.TargetTransform.position + PositionTo;
					target = base.TargetTransform.position;
				}
				InterpolateTransformation(EaseType, current, target, num2);
				ApplyAction();
				if (num2 < 1f)
				{
					StartTransformation();
				}
			}
			else
			{
				Interpolate(InteractiveObject.IsActive);
				ApplyAction();
			}
		}
		else
		{
			SetTransformation();
		}
	}

	private void SetTransformation()
	{
		if (TransformationType == Interpolator.TransformationType.Current)
		{
			if (InteractiveObject.IsActive)
			{
				base.TargetTransform.position += PositionTo;
			}
		}
		else if (InteractiveObject.IsActive)
		{
			base.TargetTransform.position = end;
		}
		else
		{
			base.TargetTransform.position = start;
		}
	}

	private void OnMachineActiveUpdate(bool active)
	{
		Interpolate(active);
		ApplyAction();
	}

	private void Interpolate(bool active)
	{
		float num = ((!(TimeToMove > 0f)) ? 1 : 0);
		timeElapsed = 0f;
		if (InteractiveObject is BaseMachine)
		{
			float num2 = GameTime.realtimeSinceServerStartup - lastServerTimeStamp;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			else if (num2 > TimeToMove)
			{
				num2 = TimeToMove;
			}
			num = ((TimeToMove > 0f) ? (num2 / TimeToMove) : num);
			timeElapsed += num2;
		}
		switch (TransformationType)
		{
		case Interpolator.TransformationType.Snap:
			if (active)
			{
				current = start;
				target = end;
			}
			else
			{
				current = end;
				target = start;
			}
			InterpolateTransformation(EaseType, current, target, num);
			break;
		case Interpolator.TransformationType.Current:
			current = base.TargetTransform.position;
			if (active)
			{
				target = current + PositionTo;
			}
			else
			{
				target = current - PositionTo;
			}
			InterpolateTransformation(EaseType, current, target, num);
			break;
		case Interpolator.TransformationType.Interpolate:
			if (active)
			{
				current = start;
				target = end;
			}
			else
			{
				current = end;
				target = start;
			}
			if (distance > 0f)
			{
				InterpolateTransformation(EaseType, current, target, num);
				float num3 = Mathf.Abs(Vector3.Distance(base.TargetTransform.position, current)) / distance;
				timeElapsed += num3 * TimeToMove;
			}
			break;
		}
		StartTransformation();
	}

	private void ApplyAction()
	{
		float num;
		if (TimeToMove > 0f)
		{
			num = timeElapsed / TimeToMove;
			if (num > 1f)
			{
				num = 1f;
			}
		}
		else
		{
			num = 1f;
		}
		InterpolateTransformation(EaseType, current, target, num);
		if (timeElapsed >= TimeToMove)
		{
			update = false;
		}
	}

	protected override void ApplyTransformation(Vector3 transformation)
	{
		base.TargetTransform.position = transformation;
	}
}
