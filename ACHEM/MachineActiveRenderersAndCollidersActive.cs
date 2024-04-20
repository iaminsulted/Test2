using System;
using UnityEngine;

public class MachineActiveRenderersAndCollidersActive : MachineActive
{
	public void Start()
	{
		try
		{
			InteractiveObject.ActiveUpdated += OnMachineActiveUpdated;
			OnMachineActiveUpdated(InteractiveObject.IsActive);
		}
		catch (Exception innerException)
		{
			Debug.LogException(new Exception("AQ3DException-> [GameObject:" + this.GetPath() + "]", innerException), base.gameObject);
		}
	}

	private void OnMachineActiveUpdated(bool active)
	{
		if (base.TargetTransform.gameObject.GetComponent<Collider>() == null)
		{
			Debug.LogError("ERROR: No collider on root game object of machine!");
			return;
		}
		base.TargetTransform.gameObject.GetComponent<Collider>().enabled = InteractiveObject.IsActive;
		Renderer[] componentsInChildren = base.TargetTransform.GetComponentsInChildren<Renderer>(includeInactive: true);
		foreach (Renderer renderer in componentsInChildren)
		{
			if (renderer.gameObject.layer != 10)
			{
				renderer.enabled = InteractiveObject.IsActive;
			}
		}
		Collider[] componentsInChildren2 = base.TargetTransform.GetComponentsInChildren<Collider>(includeInactive: true);
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].enabled = InteractiveObject.IsActive;
		}
	}
}
