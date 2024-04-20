public class AECapsuleCollider : AECollider
{
	public float Radius;

	public float Height;

	public AEQuaternion Rotation = new AEQuaternion();

	public AECapsuleCollider()
	{
	}

	public AECapsuleCollider(AEVector3 center, float radius, float height, AEQuaternion rotation)
		: base(center)
	{
		Radius = radius;
		Height = height;
		Rotation = rotation;
	}
}
