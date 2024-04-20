using System;
using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine Active Material Swap")]
public class MachineActiveMaterialSwap : MachineActive
{
	public Material MatEnable;

	public Material MatCurrent;

	public Transform TargetGO;

	public float Distance = 12f;

	private bool highlighted;

	public bool Highlighted
	{
		get
		{
			return highlighted;
		}
		set
		{
			if (highlighted != value)
			{
				highlighted = value;
				base.TargetTransform.GetComponent<Renderer>().sharedMaterial = (highlighted ? MatEnable : MatCurrent);
			}
		}
	}

	private void Start()
	{
		InteractiveObject.ActiveUpdated += OnMachineActiveUpdated;
		TargetGO = Entities.Instance.me.wrapper.transform;
		base.enabled = TargetGO == null;
		OnMachineActiveUpdated(InteractiveObject.IsActive);
	}

	private void OnMachineActiveUpdated(bool active)
	{
		if (!InteractiveObject.IsActive)
		{
			Highlighted = false;
		}
		base.enabled = InteractiveObject.IsActive;
	}

	public void Update()
	{
		try
		{
			if ((base.TargetTransform.position - TargetGO.position).magnitude <= Distance)
			{
				Highlighted = true;
			}
			else
			{
				Highlighted = false;
			}
		}
		catch (Exception innerException)
		{
			Debug.LogException(new Exception("AQ3DException-> [GameObject:" + this.GetPath() + "]", innerException));
			base.enabled = false;
		}
	}

	public void OnDestroy()
	{
		InteractiveObject.ActiveUpdated -= OnMachineActiveUpdated;
	}
}
