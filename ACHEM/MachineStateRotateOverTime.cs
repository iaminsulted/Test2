using StatCurves;
using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine State Rotate Over Time")]
public class MachineStateRotateOverTime : MachineStateNonStatic, ITimedListener
{
	public Vector3 RotateAddAngle;

	public float TimeToRotate;

	public Interpolator.EaseType EaseType;

	public Interpolator.TransformationType TransformationType;

	private Interpolator.AxisType AxisType;

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
		InteractiveObject.StateUpdated -= OnMachineStateUpdate;
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
		start = base.TargetTransform.localEulerAngles;
		end = base.TargetTransform.localEulerAngles + RotateAddAngle;
		distance = 0f;
		if ((distance = Mathf.Abs(start.x - end.x)) > 0f)
		{
			AxisType = Interpolator.AxisType.X;
		}
		else if ((distance = Mathf.Abs(start.y - end.y)) > 0f)
		{
			AxisType = Interpolator.AxisType.Y;
		}
		else if ((distance = Mathf.Abs(start.z - end.z)) > 0f)
		{
			AxisType = Interpolator.AxisType.Z;
		}
		InteractiveObject.StateUpdated += OnMachineStateUpdate;
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
				else if (num > TimeToRotate)
				{
					num = TimeToRotate;
				}
				float num2 = ((TimeToRotate > 0f) ? (num / TimeToRotate) : 1f);
				timeElapsed += num;
				if (InteractiveObject.State == State)
				{
					current = base.TargetTransform.localEulerAngles;
					target = base.TargetTransform.localEulerAngles + RotateAddAngle;
				}
				else
				{
					current = base.TargetTransform.localEulerAngles + RotateAddAngle;
					target = base.TargetTransform.localEulerAngles;
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
				Interpolate(InteractiveObject.State == State);
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
			if (InteractiveObject.State == State)
			{
				base.TargetTransform.localEulerAngles += RotateAddAngle;
			}
		}
		else if (InteractiveObject.State == State)
		{
			base.TargetTransform.localEulerAngles = end;
		}
		else
		{
			base.TargetTransform.localEulerAngles = start;
		}
	}

	private void OnMachineStateUpdate(byte state)
	{
		Interpolate(state == State);
		ApplyAction();
	}

	private void Interpolate(bool active)
	{
		float num = ((!(TimeToRotate > 0f)) ? 1 : 0);
		timeElapsed = 0f;
		if (InteractiveObject is BaseMachine)
		{
			float num2 = GameTime.realtimeSinceServerStartup - lastServerTimeStamp;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			else if (num2 > TimeToRotate)
			{
				num2 = TimeToRotate;
			}
			num = ((TimeToRotate > 0f) ? (num2 / TimeToRotate) : num);
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
			current = base.TargetTransform.localEulerAngles;
			if (active)
			{
				target = current + RotateAddAngle;
			}
			else
			{
				target = current - RotateAddAngle;
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
				Vector3 localEulerAngles = InterpolateTransformation(EaseType, current, target, num);
				float num3 = AxisType switch
				{
					Interpolator.AxisType.X => Mathf.Abs(localEulerAngles.x - current.x), 
					Interpolator.AxisType.Y => Mathf.Abs(localEulerAngles.y - current.y), 
					Interpolator.AxisType.Z => Mathf.Abs(localEulerAngles.z - current.z), 
					_ => 0f, 
				} / distance;
				timeElapsed = num3 * TimeToRotate;
				base.TargetTransform.localEulerAngles = localEulerAngles;
			}
			break;
		}
		StartTransformation();
	}

	private void ApplyAction()
	{
		float num;
		if (TimeToRotate > 0f)
		{
			num = timeElapsed / TimeToRotate;
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
		if (timeElapsed >= TimeToRotate)
		{
			update = false;
		}
	}

	protected override void ApplyTransformation(Vector3 transformation)
	{
		base.TargetTransform.localEulerAngles = transformation;
	}
}
