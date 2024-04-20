using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine Active Play Sound")]
public class MachineActivePlaySound : MachineActive
{
	public string ClipName;

	public float Delay;

	private void Start()
	{
		InteractiveObject.ActiveUpdated += OnActiveUpdated;
	}

	private void OnActiveUpdated(bool active)
	{
		if (active)
		{
			SFXPlayer componentInChildren = base.TargetTransform.gameObject.transform.GetComponentInChildren<SFXPlayer>();
			if (componentInChildren != null)
			{
				componentInChildren.Play(ClipName);
			}
			else
			{
				AudioManager.Play2DSFX(ClipName, Delay);
			}
		}
	}

	private void OnDestroy()
	{
		InteractiveObject.ActiveUpdated -= OnActiveUpdated;
	}
}
