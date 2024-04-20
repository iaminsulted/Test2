using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine State Play Particle")]
public class MachineStatePlayParticle : MachineState
{
	public bool clearParticles;

	private ParticleSystem[] particleSystems;

	private void Start()
	{
		particleSystems = base.TargetTransform.GetComponentsInChildren<ParticleSystem>();
		InteractiveObject.StateUpdated += OnMachineStateUpdate;
		OnMachineStateUpdate(InteractiveObject.State);
	}

	private void OnDestroy()
	{
		InteractiveObject.StateUpdated -= OnMachineStateUpdate;
	}

	private void OnMachineStateUpdate(byte state)
	{
		ParticleSystem[] array;
		if (state == State)
		{
			array = particleSystems;
			foreach (ParticleSystem particleSystem in array)
			{
				particleSystem.Stop();
				if (clearParticles)
				{
					particleSystem.Clear();
				}
				particleSystem.Play();
			}
			return;
		}
		array = particleSystems;
		foreach (ParticleSystem particleSystem2 in array)
		{
			particleSystem2.Stop();
			if (clearParticles)
			{
				particleSystem2.Clear();
			}
		}
	}
}
