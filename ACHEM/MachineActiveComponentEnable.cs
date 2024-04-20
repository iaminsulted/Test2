using System;
using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine Active Component Enable")]
public class MachineActiveComponentEnable : MachineActive
{
	public MonoBehaviour Component;

	[SerializeField]
	[HideInInspector]
	private string componentType;

	private void Start()
	{
		try
		{
			InteractiveObject.ActiveUpdated += OnMachineActiveUpdated;
			OnMachineActiveUpdated(InteractiveObject.IsActive);
		}
		catch (Exception innerException)
		{
			Debug.LogException(new Exception("AQ3DException-> [GameObject:" + this.GetPath() + "]", innerException));
		}
	}

	private void OnMachineActiveUpdated(bool active)
	{
		if (componentType != null)
		{
			((MonoBehaviour)base.TargetTransform.GetComponent(componentType)).enabled = active;
		}
	}

	private void OnValidate()
	{
		componentType = ((Component == null) ? null : Component.GetType().ToString());
	}
}
