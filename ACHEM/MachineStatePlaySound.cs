using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine State Play Sound")]
public class MachineStatePlaySound : MachineState
{
	public string ClipName;

	public float Delay;

	private void Start()
	{
		InteractiveObject.StateUpdated += OnStateUpdated;
	}

	private void OnDestroy()
	{
		InteractiveObject.StateUpdated -= OnStateUpdated;
	}

	private void OnStateUpdated(byte state)
	{
		if (state == State)
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
}
