public class AECollider
{
	public string Name;

	public int ID;

	public AEVector3 Center = new AEVector3();

	public AECollider()
	{
	}

	public AECollider(AEVector3 center)
	{
		Center = center;
	}
}
