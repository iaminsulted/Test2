using System;
using UnityEngine;

public class SheatheCompletedBehaviour : StateMachineBehaviour
{
	public event Action ActionCompleted;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.ResetTrigger("CancelAction");
		if (this.ActionCompleted != null)
		{
			this.ActionCompleted();
		}
	}
}
