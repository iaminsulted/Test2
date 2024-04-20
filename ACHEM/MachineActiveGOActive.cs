using System;
using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine Active GO Active")]
public class MachineActiveGOActive : MachineActive
{
	public void Start()
	{
		try
		{
			InteractiveObject.ActiveUpdated += OnMachineActiveUpdated;
			base.TargetTransform.gameObject.SetActive(InteractiveObject.IsActive);
		}
		catch (Exception innerException)
		{
			Debug.LogException(new Exception("AQ3DException-> [GameObject:" + this.GetPath() + "]", innerException), base.gameObject);
		}
	}

	private void OnMachineActiveUpdated(bool active)
	{
		base.TargetTransform.gameObject.SetActive(InteractiveObject.IsActive);
	}
}
