using UnityEngine;

public class TargetReticleScript : MonoBehaviour
{
	private const float ProjectionDistance = 5f;

	private const float FarClipPlane = 10f;

	public Projector Projector;

	private Entity target;

	private Material runtimeMaterial;

	private Transform targetTransform;

	public Entity Target
	{
		get
		{
			return target;
		}
		set
		{
			if (value != null && value.IsTargetReticleHidden)
			{
				value = null;
			}
			if (target != value)
			{
				if (target != null)
				{
					target.Destroyed -= Hide;
					target.ReactUpdated -= UpdateReactionColor;
					target.VisualStateChanged -= UpdateReactionColor;
				}
				target = value;
				if (target == null)
				{
					targetTransform = null;
					base.gameObject.SetActive(value: false);
				}
				else if (!(target.wrapper == null))
				{
					target.Destroyed += Hide;
					target.ReactUpdated += UpdateReactionColor;
					target.VisualStateChanged += UpdateReactionColor;
					targetTransform = target.wrapper.transform;
					Projector.orthographicSize = target.combatRadius;
					Projector.farClipPlane = Mathf.Max(2f, target.combatRadius / 3f);
					UpdateReactionColor(target);
					Update();
					base.gameObject.SetActive(value: true);
				}
			}
		}
	}

	private void Update()
	{
		base.transform.position = targetTransform.position;
	}

	public void Hide()
	{
		Target = null;
	}

	public void InitMaterial()
	{
		if (runtimeMaterial == null)
		{
			runtimeMaterial = new Material(Projector.material);
			Projector.material = runtimeMaterial;
			runtimeMaterial.SetFloat("_Hardness", 1f);
			runtimeMaterial.SetFloat("_InnerRadius", 0.9f);
			runtimeMaterial.SetFloat("_OuterRadius", 1f);
			runtimeMaterial.SetFloat("_ConeAngle", 360f);
			runtimeMaterial.SetFloat("_Transparency", 1f);
		}
	}

	private void UpdateReactionColor(Entity target)
	{
		InitMaterial();
		Color value = target.GetReactionColor(Entities.Instance.me);
		runtimeMaterial.SetColor("_Color", value);
		runtimeMaterial.SetColor("_OutlineColor", value);
	}
}
