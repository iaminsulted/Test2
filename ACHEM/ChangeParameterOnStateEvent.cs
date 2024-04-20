using UnityEngine;

public class ChangeParameterOnStateEvent : StateMachineBehaviour
{
	public enum StateEventWhenToChangeParameterValue
	{
		OnStateEnter,
		OnStateExit
	}

	public enum ChangeParameterValueType
	{
		Boolean,
		Integer,
		Float
	}

	public StateEventWhenToChangeParameterValue stateEventWhenToChangeParameterValue;

	public ChangeParameterValueType changeParameterValueType;

	public string parameterName = "";

	public string parameterType = "";

	public string parameterDefaultValue = "";

	public int integerTargetParameterValue;

	public float floatTargetParameterValue;

	public bool booleanTargetParameterValue;

	public float parameterChangeDuration;

	private float parameterChangeLerp;

	private float currentParameterValue;

	private int transitionHashToThisState;

	private int currentTransitionHash;

	public float delayToManageParameterMin;

	public bool randomizeDelayBetweenTwoValues;

	public float delayToManageParameterMax;

	private float timeToManageParameter;

	private bool manageParameterStarted;

	public float layerWeightThresholdToTrigger;

	public void SetParameterValue(Animator animator, AnimatorStateInfo animatorStateInfo)
	{
		if (changeParameterValueType == ChangeParameterValueType.Integer)
		{
			animator.SetInteger(parameterName, integerTargetParameterValue);
		}
		else if (changeParameterValueType == ChangeParameterValueType.Boolean)
		{
			animator.SetBool(parameterName, booleanTargetParameterValue);
		}
		else if (changeParameterValueType == ChangeParameterValueType.Float)
		{
			if (parameterChangeDuration <= 0f)
			{
				parameterChangeLerp = 1f;
				animator.SetFloat(parameterName, floatTargetParameterValue);
			}
			else
			{
				currentParameterValue = animator.GetFloat(parameterName);
				parameterChangeLerp = 0f;
			}
		}
	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		if (animator.GetLayerWeight(layerIndex) < layerWeightThresholdToTrigger)
		{
			return;
		}
		transitionHashToThisState = animator.GetAnimatorTransitionInfo(layerIndex).fullPathHash;
		if (stateEventWhenToChangeParameterValue != 0)
		{
			return;
		}
		if (delayToManageParameterMin == 0f)
		{
			SetParameterValue(animator, animatorStateInfo);
			manageParameterStarted = false;
			return;
		}
		if (randomizeDelayBetweenTwoValues)
		{
			timeToManageParameter = Time.time + Random.Range(delayToManageParameterMin, delayToManageParameterMax);
		}
		else
		{
			timeToManageParameter = Time.time + delayToManageParameterMin;
		}
		manageParameterStarted = true;
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		if (!(animator.GetLayerWeight(layerIndex) < layerWeightThresholdToTrigger) && (stateEventWhenToChangeParameterValue == StateEventWhenToChangeParameterValue.OnStateExit || (parameterChangeLerp > 0f && parameterChangeLerp < 1f)))
		{
			SetParameterValue(animator, animatorStateInfo);
		}
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		if (animator.GetLayerWeight(layerIndex) < layerWeightThresholdToTrigger || stateEventWhenToChangeParameterValue != 0 || !(Time.time > timeToManageParameter))
		{
			return;
		}
		currentTransitionHash = animator.GetAnimatorTransitionInfo(layerIndex).fullPathHash;
		if (changeParameterValueType == ChangeParameterValueType.Float && parameterChangeDuration > 0f)
		{
			if (parameterChangeLerp < 1f && (currentTransitionHash == 0 || currentTransitionHash == transitionHashToThisState))
			{
				parameterChangeLerp += Time.deltaTime / parameterChangeDuration;
				animator.SetFloat(parameterName, Mathf.Lerp(currentParameterValue, floatTargetParameterValue, parameterChangeLerp));
			}
		}
		else if (changeParameterValueType == ChangeParameterValueType.Integer)
		{
			SetParameterValue(animator, animatorStateInfo);
		}
	}
}
