using System.Collections;
using UnityEngine;

public class LookAtEntityTarget : MonoBehaviour
{
	public enum TargetType
	{
		Root,
		HitSpot,
		CastSpot,
		HeadSpot
	}

	public Transform pivot;

	public float degreesPerSecond = 24f;

	public bool limitRotation;

	public float maxUpAngle;

	public float maxDownAngle;

	public TargetType targetType = TargetType.HeadSpot;

	public Vector3 targetOffset;

	public bool resetOnTargetChange;

	public bool resetOnDeath;

	private float actualUpAngle;

	private Quaternion rotateStart;

	private EntityController entityController;

	private Transform lookAtTarget;

	private bool died;

	private void Awake()
	{
		actualUpAngle = 360f - maxUpAngle;
		entityController = GetComponentInParent<EntityController>();
		if (entityController != null)
		{
			entityController.Entity.TargetUpdateEvent += UpdateTarget;
			if (!resetOnDeath)
			{
				entityController.Entity.DeathEvent += StopRotationOnDeath;
			}
		}
		if (!pivot)
		{
			pivot = base.transform;
		}
		rotateStart = pivot.rotation;
	}

	private void OnDestroy()
	{
		if ((bool)entityController)
		{
			entityController.Entity.TargetUpdateEvent -= UpdateTarget;
			entityController.Entity.DeathEvent -= StopRotationOnDeath;
		}
	}

	private void LateUpdate()
	{
		if (entityController == null)
		{
			entityController = GetComponentInParent<EntityController>();
			if (entityController != null)
			{
				entityController.Entity.TargetUpdateEvent += UpdateTarget;
				if (!resetOnDeath)
				{
					entityController.Entity.DeathEvent += StopRotationOnDeath;
				}
			}
		}
		if ((bool)lookAtTarget)
		{
			Quaternion to = Quaternion.LookRotation(lookAtTarget.position + targetOffset - pivot.position);
			Quaternion quaternion = Quaternion.RotateTowards(pivot.rotation, to, degreesPerSecond * Time.deltaTime);
			if (limitRotation)
			{
				quaternion = ClampRotation(quaternion);
			}
			pivot.rotation = quaternion;
		}
	}

	private IEnumerator Reset()
	{
		if ((bool)pivot)
		{
			while (pivot.rotation != rotateStart && !died)
			{
				pivot.rotation = Quaternion.RotateTowards(pivot.rotation, rotateStart, degreesPerSecond * Time.deltaTime);
				yield return null;
			}
		}
	}

	private void StopRotationOnDeath(Entity e)
	{
		died = true;
	}

	private void UpdateTarget(Entity e)
	{
		if (e != null && e.wrapperTransform != null)
		{
			StopAllCoroutines();
			EntityAssetData componentInChildren = e.wrapperTransform.GetComponentInChildren<EntityAssetData>();
			if ((bool)componentInChildren)
			{
				if (targetType == TargetType.CastSpot && (bool)componentInChildren.CastSpot)
				{
					lookAtTarget = componentInChildren.CastSpot;
				}
				else if (targetType == TargetType.HitSpot && (bool)componentInChildren.HitSpot)
				{
					lookAtTarget = componentInChildren.HitSpot;
				}
				else if (targetType == TargetType.HeadSpot && (bool)componentInChildren.HeadSpot)
				{
					lookAtTarget = componentInChildren.HeadSpot;
				}
				else
				{
					lookAtTarget = e.wrapperTransform;
				}
			}
			else
			{
				lookAtTarget = e.wrapperTransform;
			}
		}
		else
		{
			lookAtTarget = null;
			if (base.gameObject.activeInHierarchy && resetOnTargetChange)
			{
				StartCoroutine(Reset());
			}
		}
	}

	private Quaternion ClampRotation(Quaternion targetRotation)
	{
		Vector3 eulerAngles = targetRotation.eulerAngles;
		if (eulerAngles.x < 180f)
		{
			eulerAngles.x = Mathf.Clamp(eulerAngles.x, 0f, maxDownAngle);
		}
		else
		{
			eulerAngles.x = Mathf.Clamp(eulerAngles.x, actualUpAngle, 360f);
		}
		eulerAngles.z = 0f;
		return Quaternion.Euler(eulerAngles);
	}
}
