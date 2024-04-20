using System.Collections.Generic;
using StatCurves;
using UnityEngine;

public abstract class MachineListener : MonoBehaviour
{
	public static Dictionary<int, MachineListener> Map = new Dictionary<int, MachineListener>();

	public int ID;

	[ReadOnly]
	public string TargetId;

	[SerializeField]
	private Transform targetTransform;

	protected bool update;

	protected float lastServerTimeStamp;

	public Transform TargetTransform
	{
		get
		{
			if (targetTransform == null)
			{
				if (TargetId == null || TargetId == "")
				{
					targetTransform = base.transform;
				}
				else if (UniqueID.Get(TargetId) != null)
				{
					targetTransform = UniqueID.Get(TargetId).transform;
				}
			}
			return targetTransform;
		}
	}

	protected virtual void Awake()
	{
		if (!Map.ContainsKey(ID))
		{
			Map[ID] = this;
		}
		lastServerTimeStamp = GameTime.realtimeSinceServerStartup;
	}

	public void SetIdAndAddListenerToMap(int ID)
	{
		this.ID = ID;
		AddListenerToMap();
	}

	public void AddListenerToMap()
	{
		if (Map.ContainsKey(ID))
		{
			Debug.LogWarning("Warning: Machine Listener ID: " + ID + " already exists! MACHINE LISTENER WILL NOT BE ADDED TO MAP DICTIONARY");
		}
		else
		{
			Map[ID] = this;
		}
	}

	public void UpdateServerTimeStamp(float timeStamp)
	{
		lastServerTimeStamp = timeStamp;
	}

	public abstract InteractiveObject GetInteractiveObj();

	protected void StartTransformation()
	{
		update = true;
		lastServerTimeStamp = GameTime.realtimeSinceServerStartup;
	}

	protected Vector3 InterpolateTransformation(Interpolator.EaseType ease, Vector3 current, Vector3 target, float percent)
	{
		Vector3 zero = Vector3.zero;
		switch (ease)
		{
		case Interpolator.EaseType.EaseInQuad:
			zero.x = Interpolator.EaseInQuad(current.x, target.x, percent);
			zero.y = Interpolator.EaseInQuad(current.y, target.y, percent);
			zero.z = Interpolator.EaseInQuad(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseOutQuad:
			zero.x = Interpolator.EaseOutQuad(current.x, target.x, percent);
			zero.y = Interpolator.EaseOutQuad(current.y, target.y, percent);
			zero.z = Interpolator.EaseOutQuad(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseInOutQuad:
			zero.x = Interpolator.EaseInOutQuad(current.x, target.x, percent);
			zero.y = Interpolator.EaseInOutQuad(current.y, target.y, percent);
			zero.z = Interpolator.EaseInOutQuad(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseInCubic:
			zero.x = Interpolator.EaseInCubic(current.x, target.x, percent);
			zero.y = Interpolator.EaseInCubic(current.y, target.y, percent);
			zero.z = Interpolator.EaseInCubic(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseOutCubic:
			zero.x = Interpolator.EaseOutCubic(current.x, target.x, percent);
			zero.y = Interpolator.EaseOutCubic(current.y, target.y, percent);
			zero.z = Interpolator.EaseOutCubic(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseInOutCubic:
			zero.x = Interpolator.EaseInOutCubic(current.x, target.x, percent);
			zero.y = Interpolator.EaseInOutCubic(current.y, target.y, percent);
			zero.z = Interpolator.EaseInOutCubic(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseInQuart:
			zero.x = Interpolator.EaseInQuart(current.x, target.x, percent);
			zero.y = Interpolator.EaseInQuart(current.y, target.y, percent);
			zero.z = Interpolator.EaseInQuart(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseOutQuart:
			zero.x = Interpolator.EaseOutQuart(current.x, target.x, percent);
			zero.y = Interpolator.EaseOutQuart(current.y, target.y, percent);
			zero.z = Interpolator.EaseOutQuart(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseInOutQuart:
			zero.x = Interpolator.EaseInOutQuart(current.x, target.x, percent);
			zero.y = Interpolator.EaseInOutQuart(current.y, target.y, percent);
			zero.z = Interpolator.EaseInOutQuart(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseInQuint:
			zero.x = Interpolator.EaseInQuint(current.x, target.x, percent);
			zero.y = Interpolator.EaseInQuint(current.y, target.y, percent);
			zero.z = Interpolator.EaseInQuint(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseOutQuint:
			zero.x = Interpolator.EaseOutQuint(current.x, target.x, percent);
			zero.y = Interpolator.EaseOutQuint(current.y, target.y, percent);
			zero.z = Interpolator.EaseOutQuint(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseInOutQuint:
			zero.x = Interpolator.EaseInOutQuint(current.x, target.x, percent);
			zero.y = Interpolator.EaseInOutQuint(current.y, target.y, percent);
			zero.z = Interpolator.EaseInOutQuint(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseInSine:
			zero.x = Interpolator.EaseInSine(current.x, target.x, percent);
			zero.y = Interpolator.EaseInSine(current.y, target.y, percent);
			zero.z = Interpolator.EaseInSine(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseOutSine:
			zero.x = Interpolator.EaseOutSine(current.x, target.x, percent);
			zero.y = Interpolator.EaseOutSine(current.y, target.y, percent);
			zero.z = Interpolator.EaseOutSine(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseInOutSine:
			zero.x = Interpolator.EaseInOutSine(current.x, target.x, percent);
			zero.y = Interpolator.EaseInOutSine(current.y, target.y, percent);
			zero.z = Interpolator.EaseInOutSine(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseInExpo:
			zero.x = Interpolator.EaseInExpo(current.x, target.x, percent);
			zero.y = Interpolator.EaseInExpo(current.y, target.y, percent);
			zero.z = Interpolator.EaseInExpo(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseOutExpo:
			zero.x = Interpolator.EaseOutExpo(current.x, target.x, percent);
			zero.y = Interpolator.EaseOutExpo(current.y, target.y, percent);
			zero.z = Interpolator.EaseOutExpo(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseInOutExpo:
			zero.x = Interpolator.EaseInOutExpo(current.x, target.x, percent);
			zero.y = Interpolator.EaseInOutExpo(current.y, target.y, percent);
			zero.z = Interpolator.EaseInOutExpo(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseInCirc:
			zero.x = Interpolator.EaseInCirc(current.x, target.x, percent);
			zero.y = Interpolator.EaseInCirc(current.y, target.y, percent);
			zero.z = Interpolator.EaseInCirc(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseOutCirc:
			zero.x = Interpolator.EaseOutCirc(current.x, target.x, percent);
			zero.y = Interpolator.EaseOutCirc(current.y, target.y, percent);
			zero.z = Interpolator.EaseOutCirc(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseInOutCirc:
			zero.x = Interpolator.EaseInOutCirc(current.x, target.x, percent);
			zero.y = Interpolator.EaseInOutCirc(current.y, target.y, percent);
			zero.z = Interpolator.EaseInOutCirc(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.Linear:
			zero.x = Interpolator.Linear(current.x, target.x, percent);
			zero.y = Interpolator.Linear(current.y, target.y, percent);
			zero.z = Interpolator.Linear(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.Spring:
			zero.x = Interpolator.Spring(current.x, target.x, percent);
			zero.y = Interpolator.Spring(current.y, target.y, percent);
			zero.z = Interpolator.Spring(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseInBounce:
			zero.x = Interpolator.EaseInBounce(current.x, target.x, percent);
			zero.y = Interpolator.EaseInBounce(current.y, target.y, percent);
			zero.z = Interpolator.EaseInBounce(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseOutBounce:
			zero.x = Interpolator.EaseOutBounce(current.x, target.x, percent);
			zero.y = Interpolator.EaseOutBounce(current.y, target.y, percent);
			zero.z = Interpolator.EaseOutBounce(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseInOutBounce:
			zero.x = Interpolator.EaseInOutBounce(current.x, target.x, percent);
			zero.y = Interpolator.EaseInOutBounce(current.y, target.y, percent);
			zero.z = Interpolator.EaseInOutBounce(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseInBack:
			zero.x = Interpolator.EaseInBack(current.x, target.x, percent);
			zero.y = Interpolator.EaseInBack(current.y, target.y, percent);
			zero.z = Interpolator.EaseInBack(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseOutBack:
			zero.x = Interpolator.EaseOutBack(current.x, target.x, percent);
			zero.y = Interpolator.EaseOutBack(current.y, target.y, percent);
			zero.z = Interpolator.EaseOutBack(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseInOutBack:
			zero.x = Interpolator.EaseInOutBack(current.x, target.x, percent);
			zero.y = Interpolator.EaseInOutBack(current.y, target.y, percent);
			zero.z = Interpolator.EaseInOutBack(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseInElastic:
			zero.x = Interpolator.EaseInElastic(current.x, target.x, percent);
			zero.y = Interpolator.EaseInElastic(current.y, target.y, percent);
			zero.z = Interpolator.EaseInElastic(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseOutElastic:
			zero.x = Interpolator.EaseOutElastic(current.x, target.x, percent);
			zero.y = Interpolator.EaseOutElastic(current.y, target.y, percent);
			zero.z = Interpolator.EaseOutElastic(current.z, target.z, percent);
			break;
		case Interpolator.EaseType.EaseInOutElastic:
			zero.x = Interpolator.EaseInOutElastic(current.x, target.x, percent);
			zero.y = Interpolator.EaseInOutElastic(current.y, target.y, percent);
			zero.z = Interpolator.EaseInOutElastic(current.z, target.z, percent);
			break;
		}
		ApplyTransformation(zero);
		return zero;
	}

	protected virtual void ApplyTransformation(Vector3 transformation)
	{
	}
}
