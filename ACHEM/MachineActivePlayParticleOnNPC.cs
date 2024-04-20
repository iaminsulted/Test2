using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine Active Play Particle On NPC")]
public class MachineActivePlayParticleOnNPC : MachineActive
{
	public NPCSpawn spawn;

	public bool clearParticles;

	public bool playAtSpawnIfDead;

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
			NPC npcBySpawnId = Entities.Instance.GetNpcBySpawnId(spawn.ID);
			if (npcBySpawnId != null && npcBySpawnId.serverState != Entity.State.Dead)
			{
				base.TargetTransform.position = npcBySpawnId.position;
			}
			else
			{
				if (!playAtSpawnIfDead)
				{
					return;
				}
				base.TargetTransform.position = spawn.transform.position;
			}
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
