public class ResponseMachinePlayAnimation : Response
{
	public string animationName;

	public int target;

	public int id;

	public int entityType;

	public float crossfadeDuration;

	public int layer;

	public float normalizedTime;

	public ResponseMachinePlayAnimation(string animationName, int target, int id, int entityType, float crossfadeDuration, int layer, float normalizedTime)
	{
		type = 19;
		cmd = 7;
		this.animationName = animationName;
		this.target = target;
		this.id = id;
		this.entityType = entityType;
		this.crossfadeDuration = crossfadeDuration;
		this.layer = layer;
		this.normalizedTime = normalizedTime;
	}
}
