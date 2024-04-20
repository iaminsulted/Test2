using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA Set Animator Layer Weight")]
public class MASetAnimatorLayerWeight : ListenerAction
{
	public NPCSpawn npcSpawn;

	public int layerID;

	public float crossfade;

	public float weight;
}
