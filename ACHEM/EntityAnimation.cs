public class EntityAnimation
{
	public enum Type
	{
		DEFAULT,
		SHEATHING,
		UNSHEATHING
	}

	public string name;

	public bool blockMovement;

	public bool isCancellableByMovement;

	public int priority;

	public bool ignorePriority;

	public float crossfadeSpeed = 0.1f;

	public float length;

	public bool canMix;

	public int layer = -1;

	public float normalizedTime;

	public Type animationType;
}
