using System;
using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine State Component Enable")]
public class MachineStateComponentEnable : MachineState
{
	public MonoBehaviour Component;

	[SerializeField]
	[HideInInspector]
	private string componentType;

	private void Start()
	{
		try
		{
			InteractiveObject.StateUpdated += OnMachineStateUpdated;
			OnMachineStateUpdated(InteractiveObject.State);
		}
		catch (Exception innerException)
		{
			Debug.LogException(new Exception("AQ3DException-> [GameObject:" + this.GetPath() + "]", innerException));
		}
	}

	private void OnValidate()
	{
		componentType = ((Component == null) ? null : Component.GetType().ToString());
	}

	private void OnMachineStateUpdated(byte state)
	{
		if (componentType != null)
		{
			((MonoBehaviour)base.TargetTransform.GetComponent(componentType)).enabled = state == State;
		}
	}
}
