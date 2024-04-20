using System;
using System.Globalization;
using UnityEngine;

public class EntityAssetData : MonoBehaviour
{
	public Transform HeadSpot;

	public Transform HitSpot;

	public Transform CastSpot;

	public Transform CameraSpot;

	public Transform RightWeaponMount;

	public Transform LeftWeaponMount;

	public Transform RightShoulderMount;

	public Transform LeftShoulderMount;

	public Transform HelmetMount;

	public Transform ProbeAnchor;

	public float Radius = 1f;

	public Action RunStepLand;

	public Action JumpBegin;

	public Action JumpLand;

	public Action CastFx;

	public Action<int, int, float> MakeProjectile;

	public Action<int, int, float> SpellImpact;

	public Action TrailStart;

	public Action TrailEnd;

	public Action Sheathe;

	public Action Unsheathe;

	public void StartTrail()
	{
		TrailStart?.Invoke();
	}

	public void EndTrail()
	{
		TrailEnd?.Invoke();
	}

	public void OnRunStepLand()
	{
		RunStepLand?.Invoke();
	}

	public void OnJumpBegin()
	{
		JumpBegin?.Invoke();
	}

	public void OnJumpLand()
	{
		JumpLand?.Invoke();
	}

	public void MountWeapons(AnimationEvent ae)
	{
		Sheathe?.Invoke();
	}

	public void GrabWeapons(AnimationEvent ae)
	{
		Unsheathe?.Invoke();
	}

	public void OnSpellImpact(AnimationEvent animationEvent)
	{
		string stringParameter = animationEvent.stringParameter;
		int num = 1;
		float arg = 1f;
		if (!string.IsNullOrEmpty(stringParameter))
		{
			string[] array = stringParameter.Replace(" ", "").Split(',');
			if (array.Length >= 1)
			{
				num = Convert.ToInt32(array[0]);
				if (num < 1)
				{
					num = 1;
				}
				arg = 1f / (float)num;
				if (array.Length > 1)
				{
					arg = Convert.ToSingle(array[1], CultureInfo.InvariantCulture);
				}
			}
		}
		SpellImpact?.Invoke(animationEvent.animatorStateInfo.shortNameHash, num, arg);
	}

	public void OnCastFx()
	{
		CastFx?.Invoke();
	}

	public void OnProjectile(AnimationEvent animationEvent)
	{
		string stringParameter = animationEvent.stringParameter;
		int num = 1;
		float arg = 1f;
		if (!string.IsNullOrEmpty(stringParameter))
		{
			string[] array = stringParameter.Replace(" ", "").Split(',');
			if (array.Length >= 1)
			{
				num = Convert.ToInt32(array[0], CultureInfo.InvariantCulture);
				if (num < 1)
				{
					num = 1;
				}
				arg = 1f / (float)num;
				if (array.Length > 1)
				{
					try
					{
						arg = Convert.ToSingle(array[1], CultureInfo.InvariantCulture);
					}
					catch (Exception ex)
					{
						Debug.LogError("Invalid impact data in the animation");
						Debug.LogError(ex.ToString());
					}
				}
			}
		}
		MakeProjectile?.Invoke(animationEvent.animatorStateInfo.shortNameHash, num, arg);
	}
}
