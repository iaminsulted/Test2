public class ResponseMachineAnimatorParameter : Response
{
	public int npcID;

	public AnimatorParameterType animatorParameterType;

	public string parameterName;

	public int valueInt;

	public float valueFloat;

	public bool valueBool;

	public ResponseMachineAnimatorParameter(int npcID, AnimatorParameterType animatorParameterType, string parameterName, int valueInt, float valueFloat, bool valueBool)
	{
		type = 19;
		cmd = 9;
		this.npcID = npcID;
		this.animatorParameterType = animatorParameterType;
		this.parameterName = parameterName;
		this.valueInt = valueInt;
		this.valueFloat = valueFloat;
		this.valueBool = valueBool;
	}
}
