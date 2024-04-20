public class AEBoxCollider : AECollider
{
	public AEVector3 Size = new AEVector3();

	public AEQuaternion Rotation = new AEQuaternion();

	public AEBoxCollider()
	{
	}

	public AEBoxCollider(AEVector3 center, AEVector3 size, AEQuaternion rotation)
		: base(center)
	{
		Size = size;
		Rotation = rotation;
	}
}
