using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine State Material Property Tween")]
public class MachineStateMaterialPropertyTween : MachineState
{
	private Material[] materials;

	public string propertyName;

	public float time;

	public float fromValue;

	public float toValue;

	private void Start()
	{
		materials = base.TargetTransform.GetComponent<Renderer>().materials;
		InteractiveObject.StateUpdated += OnMachineStateUpdate;
		OnMachineInitialized(InteractiveObject.State);
	}

	private void OnDestroy()
	{
		InteractiveObject.StateUpdated -= OnMachineStateUpdate;
	}

	private void OnMachineInitialized(byte state)
	{
		if (state == State)
		{
			UpdateMaterials(toValue);
		}
		else
		{
			UpdateMaterials(fromValue);
		}
	}

	private void OnMachineStateUpdate(byte state)
	{
		if (state == State)
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
}
