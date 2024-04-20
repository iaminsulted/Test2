using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine Active Play Particle")]
public class MachineActivePlayParticle : MachineActive
{
	public bool clearParticles;

	private ParticleSystem[] particleSystems;

	private void Start()
	{
		particleSystems = base.TargetTransform.GetComponentsInChildren<ParticleSystem>();
		InteractiveObject.ActiveUpdated += OnMachineActiveUpdate;
		OnMachineActiveUpdate(InteractiveObject.IsActive);
	}

	private void OnMachineActiveUpdate(bool active)
	{
		ParticleSystem[] array;
		if (active)
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

	private void OnDestroy()
	{
		InteractiveObject.ActiveUpdated -= OnMachineActiveUpdate;
	}
}
