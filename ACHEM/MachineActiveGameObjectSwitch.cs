using System;
using UnityEngine;

public class MachineActiveGameObjectSwitch : MachineActive
{
	public GameObject GameObjectToShowWhenMachineIsInactive;

	public GameObject GameObjectToShowWhenMachineIsActive;

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
		if (GameObjectToShowWhenMachineIsInactive == null || GameObjectToShowWhenMachineIsActive == null)
		{
			Debug.LogError("ERROR: Active or inactive gameobject was null. Both need to be defined to use MachineActiveRenderersSwitched.");
		}
		else if (active)
		{
			GameObjectToShowWhenMachineIsActive.SetActive(value: true);
			GameObjectToShowWhenMachineIsInactive.SetActive(value: false);
		}
		else
		{
			GameObjectToShowWhenMachineIsActive.SetActive(value: false);
			GameObjectToShowWhenMachineIsInactive.SetActive(value: true);
		}
	}
}
