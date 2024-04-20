using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine Active Material Property Tween")]
public class MachineActiveMaterialPropertyTween : MachineActive
{
	public string propertyName;

	public float time;

	public float fromValue;

	public float toValue;

	private Material[] materials;

	private void Start()
	{
		materials = base.TargetTransform.GetComponent<Renderer>().materials;
		InteractiveObject.ActiveUpdated += OnMachineActiveUpdate;
		OnMachineInitialized(InteractiveObject.IsActive);
	}

	private void OnMachineActiveUpdate(bool active)
	{
		if (active)
		{
			iTween.ValueTo(base.TargetTransform.gameObject, iTween.Hash("from", fromValue, "to", toValue, "time", time, "onUpdate", "UpdateMaterials"));
		}
		else
		{
			iTween.ValueTo(base.TargetTransform.gameObject, iTween.Hash("from", toValue, "to", fromValue, "time", time, "onUpdate", "UpdateMaterials"));
		}
	}

	private void UpdateMaterials(float value)
	{
		Material[] array = materials;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetFloat(propertyName, value);
		}
	}

	private void OnMachineInitialized(bool active)
	{
		if (active)
		{
			UpdateMaterials(toValue);
		}
		else
		{
			UpdateMaterials(fromValue);
		}
	}

	private void OnDestroy()
	{
		InteractiveObject.ActiveUpdated -= OnMachineActiveUpdate;
	}
}
