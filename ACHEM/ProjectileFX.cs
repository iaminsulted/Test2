using System.Linq;
using UnityEngine;

public class ProjectileFX : MonoBehaviour
{
	private const float Max_Lifetime = 5f;

	private Entity target;

	private float startTime;

	private Vector3 startPosition;

	private KeyframeSpellData spellData;

	private Spellfx spellFX;

	private bool wasInit;

	private bool hasHit;

	private bool followTarget;

	private float stayTime;

	private float impactDelay;

	private float initialTravelTime;

	private float statDeltaPercent;

	private Transform endSpot;

	private Vector3 endPosition;

	private bool isReverseProj;

	public void InitWithTarget(Entity target, Transform endSpot, KeyframeSpellData spellData, Spellfx spellFX, float statDeltaPercent = 1f)
	{
		followTarget = true;
		this.target = target;
		this.statDeltaPercent = statDeltaPercent;
		this.endSpot = endSpot;
		Init(endSpot.position, spellData, spellFX, spellData.spellAction.projStayTime);
	}

	public void Init(Vector3 endPosition, KeyframeSpellData spellData, Spellfx spellFX, float stayTime)
	{
		this.spellData = spellData;
		this.spellFX = spellFX;
		this.stayTime = stayTime;
		this.endPosition = endPosition;
		startTime = Time.time;
		startPosition = base.transform.position;
		impactDelay = spellData.spellAction.projImpactDelay;
		spellFX.LiveForever = true;
		wasInit = true;
		hasHit = false;
		initialTravelTime = spellData.spellAction.GetProjectileImpactDelay(startPosition, endPosition);
		base.transform.LookAt(endPosition);
		isReverseProj = spellData.spellAction.projType == CombatSolver.ProjectileType.Reverse;
		if (isReverseProj)
		{
			ImpactTarget();
		}
	}

	private void Update()
	{
		if (!wasInit)
		{
			return;
		}
		if (followTarget && endSpot != null)
		{
			endPosition = endSpot.position;
		}
		if (hasHit)
		{
			spellFX.transform.position = endPosition;
			base.transform.position = endPosition;
		}
		else if (!followTarget || (target != null && endSpot != null))
		{
			float num = initialTravelTime - spellData.spellAction.GetProjectileImpactDelay(startPosition, endPosition);
			float num2 = Time.time - startTime;
			base.transform.position = Vector3.Lerp(startPosition, endPosition, num2 / (initialTravelTime + num));
			base.transform.LookAt(endPosition);
			if ((double)(endPosition - base.transform.position).sqrMagnitude < 0.01 || num2 >= 5f)
			{
				hasHit = true;
				if (stayTime > 0f && endSpot != null)
				{
					spellFX.transform.parent = endSpot.transform;
				}
				if (isReverseProj)
				{
					Invoke("ImpactCaster", impactDelay);
				}
				else
				{
					Invoke("ImpactTarget", impactDelay);
				}
				Invoke("DestroyFX", stayTime);
			}
		}
		else if (wasInit && followTarget && (target == null || endSpot == null))
		{
			DestroyFX();
		}
		else if (wasInit && Time.time - startTime > 15f)
		{
			if (!isReverseProj)
			{
				ImpactTarget();
			}
			DestroyFX();
		}
	}

	private void ImpactTarget()
	{
		if (followTarget && target != null)
		{
			target.HandleProjectileImpact(spellData, statDeltaPercent);
		}
		else if (!followTarget)
		{
			spellData.currentImpact++;
			spellData.totalFxImpacts = spellData.totalImpacts;
			AudioManager.PlayCombatSFX(spellData.spellAction.impactSFX, spellData.caster.isMe, base.transform);
			SpellFXContainer.mInstance.CreateFXAtPosition(spellData.spellAction.impactFX, base.transform.position, base.transform.rotation, spellData.spellAction);
		}
	}

	private void ImpactCaster()
	{
		SpellFXContainer.mInstance.HandleImpactFX(spellData.caster, spellData.spellAction.projImpactCasterFX, spellData.targets.Any((Entity t) => t.isMe), spellData);
	}

	private void DestroyFX()
	{
		Object.Destroy(spellFX.gameObject);
	}
}
