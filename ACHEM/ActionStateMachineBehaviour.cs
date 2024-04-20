using System;
using UnityEngine;

public class ActionStateMachineBehaviour : StateMachineBehaviour
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
