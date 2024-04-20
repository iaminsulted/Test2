using UnityEngine;

public class NPCStatePlayParticle : MonoBehaviour
{
	public NPCSpawn spawn;

	public byte state;

	private void Awake()
	{
		spawn.StateUpdated += OnNPCStateUpdate;
	}

	private void OnNPCStateUpdate(byte state)
	{
		if (state == this.state)
		{
			ParticleSystem[] componentsInChildren = GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem obj in componentsInChildren)
			{
				obj.Stop();
				obj.Play();
			}
		}
	}
}
