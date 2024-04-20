public class AESphereCollider : AECollider
{
	public float Radius;

	public AESphereCollider()
	{
	}

	public AESphereCollider(AEVector3 center, float radius)
		: base(center)
	{
		Radius = radius;
	}
}
