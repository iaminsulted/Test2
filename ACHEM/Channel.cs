public class Channel
{
	public int id;

	public string name;

	public string desc;

	public int type;

	public const int Type_Instance = 0;

	public const int Type_Stack = 1;

	public const int Type_Trade = 2;

	public Channel(int id, int type)
	{
		this.id = id;
		this.type = type;
	}
}
