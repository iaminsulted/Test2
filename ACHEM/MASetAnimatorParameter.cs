using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA Set Animator Parameter")]
public class MASetAnimatorParameter : ListenerAction
{
	public NPCSpawn npcSpawn;

	public AnimatorParameterType animatorParameterType;

	public string parameterName;

	public int valueInt;

	public float valueFloat;

	public bool valueBool;

	public bool valueTrigger;
}
