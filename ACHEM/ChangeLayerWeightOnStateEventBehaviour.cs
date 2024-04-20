using UnityEngine;

public class ChangeLayerWeightOnStateEventBehaviour : StateMachineBehaviour
{
	public StateEventWhenToChangeLayerWeight stateEventWhenToChangeLayerWeight;

	public string layerNameToChangeWeightOnStateEvent;

	public float layerWeightToChangeOnStateEvent;

	public float layerChangeDuration;

	public float layerChangeDelay;

	private float startTime;

	private float currentLayerWeight;

	private float layerWeightChangeLerp;

	public int selectedLayerNameIndex;

	private int transitionHashToThisState;

	private int currentTransitionHash;

	public float layerWeightThresholdToTrigger;

	public void ChangeLayerWeight(Animator animator, AnimatorStateInfo animatorStateInfo)
	{
		if (animator.GetLayerIndex(layerNameToChangeWeightOnStateEvent) == -1)
		{
			Debug.Log(animator.gameObject.name + " tried to Change Layer Weight of the " + layerNameToChangeWeightOnStateEvent + " but the layer was not found.");
		}
		else if (layerChangeDuration <= 0f || stateEventWhenToChangeLayerWeight == StateEventWhenToChangeLayerWeight.OnStateExit)
		{
			layerWeightChangeLerp = 1f;
			animator.SetLayerWeight(animator.GetLayerIndex(layerNameToChangeWeightOnStateEvent), layerWeightToChangeOnStateEvent);
		}
		else
		{
			currentLayerWeight = animator.GetLayerWeight(animator.GetLayerIndex(layerNameToChangeWeightOnStateEvent));
			layerWeightChangeLerp = 0f;
		}
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		if (!(animator.GetLayerWeight(layerIndex) < layerWeightThresholdToTrigger) && (stateEventWhenToChangeLayerWeight == StateEventWhenToChangeLayerWeight.OnStateExit || (layerWeightChangeLerp > 0f && layerWeightChangeLerp < 1f)))
		{
			ChangeLayerWeight(animator, animatorStateInfo);
		}
	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		if (!(animator.GetLayerWeight(layerIndex) < layerWeightThresholdToTrigger))
		{
			startTime = Time.time + layerChangeDelay;
			transitionHashToThisState = animator.GetAnimatorTransitionInfo(layerIndex).fullPathHash;
			if (stateEventWhenToChangeLayerWeight == StateEventWhenToChangeLayerWeight.OnStateEnter)
			{
				ChangeLayerWeight(animator, animatorStateInfo);
			}
		}
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if ((!(layerChangeDelay > 0f) || !(Time.time < startTime)) && stateEventWhenToChangeLayerWeight == StateEventWhenToChangeLayerWeight.OnStateEnter && layerChangeDuration > 0f)
		{
			currentTransitionHash = animator.GetAnimatorTransitionInfo(layerIndex).fullPathHash;
			if (layerWeightChangeLerp < 1f && (currentTransitionHash == 0 || currentTransitionHash == transitionHashToThisState))
			{
				layerWeightChangeLerp += Time.deltaTime / layerChangeDuration;
				animator.SetLayerWeight(animator.GetLayerIndex(layerNameToChangeWeightOnStateEvent), Mathf.Lerp(currentLayerWeight, layerWeightToChangeOnStateEvent, layerWeightChangeLerp));
			}
		}
	}
}
