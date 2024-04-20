using UnityEngine;

public class FishStateMachineBehaviour : StateMachineBehaviour
{
	[Range(0f, 1f)]
	public float chance = 0.02f;

	public string stateName;

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (Random.value <= chance)
		{
			animator.Play(stateName, -1, 0.1f);
		}
	}
}
