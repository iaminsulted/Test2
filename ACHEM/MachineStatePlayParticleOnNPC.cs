using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine State Play Particle On NPC")]
public class MachineStatePlayParticleOnNPC : MachineState
{
	public NPCSpawn spawn;

	public bool clearParticles;

	public bool playAtSpawnIfDead;

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
}
