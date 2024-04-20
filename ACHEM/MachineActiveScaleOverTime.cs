using StatCurves;
using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine Active Scale Over Time")]
public class MachineActiveScaleOverTime : MachineActiveNonStatic, ITimedListener
{
	public Vector3 FromSize;

	public Vector3 ToSize;

	public float TimeToScale;

	public Interpolator.EaseType EaseType;

	public Interpolator.TransformationType TransformationType;

	private Interpolator.AxisType AxisType;

	private Vector3 start;

	private Vector3 end;

	private Vector3 current;

	private Vector3 target;

	private Vector3 to;

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
		start = FromSize;
		end = ToSize;
		to = end - start;
		distance = 0f;
		if ((distance = Mathf.Abs(to.x)) > 0f)
		{
			AxisType = Interpolator.AxisType.X;
		}
		else if ((distance = Mathf.Abs(to.y)) > 0f)
		{
			AxisType = Interpolator.AxisType.Y;
		}
		else if ((distance = Mathf.Abs(to.z)) > 0f)
		{
			AxisType = Interpolator.AxisType.Z;
		}
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
				else if (num > TimeToScale)
				{
					num = TimeToScale;
				}
				float num2 = ((TimeToScale > 0f) ? (num / TimeToScale) : 1f);
				timeElapsed += num;
				if (InteractiveObject.IsActive)
				{
					current = base.TargetTransform.localScale;
					target = base.TargetTransform.localScale + to;
				}
				else
				{
					current = base.TargetTransform.localScale + to;
					target = base.TargetTransform.localScale;
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
				base.TargetTransform.localScale += to;
			}
		}
		else if (InteractiveObject.IsActive)
		{
			base.TargetTransform.localScale = end;
		}
		else
		{
			base.TargetTransform.localScale = start;
		}
	}

	private void OnMachineActiveUpdate(bool active)
	{
		Interpolate(active);
		ApplyAction();
	}

	private void Interpolate(bool active)
	{
		float num = ((!(TimeToScale > 0f)) ? 1 : 0);
		timeElapsed = 0f;
		if (InteractiveObject is BaseMachine)
		{
			float num2 = GameTime.realtimeSinceServerStartup - lastServerTimeStamp;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			else if (num2 > TimeToScale)
			{
				num2 = TimeToScale;
			}
			num = ((TimeToScale > 0f) ? (num2 / TimeToScale) : num);
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
			current = base.TargetTransform.localScale;
			if (active)
			{
				target = current + to;
			}
			else
			{
				target = current - to;
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
				float num3 = AxisType switch
				{
					Interpolator.AxisType.X => Mathf.Abs(base.TargetTransform.localScale.x - current.x), 
					Interpolator.AxisType.Y => Mathf.Abs(base.TargetTransform.localScale.y - current.y), 
					Interpolator.AxisType.Z => Mathf.Abs(base.TargetTransform.localScale.z - current.z), 
					_ => 0f, 
				} / distance;
				timeElapsed = num3 * TimeToScale;
			}
			break;
		}
		StartTransformation();
	}

	private void ApplyAction()
	{
		float num;
		if (TimeToScale > 0f)
		{
			num = timeElapsed / TimeToScale;
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
		if (timeElapsed >= TimeToScale)
		{
			update = false;
		}
	}

	protected override void ApplyTransformation(Vector3 transformation)
	{
		base.TargetTransform.localScale = transformation;
	}
}
