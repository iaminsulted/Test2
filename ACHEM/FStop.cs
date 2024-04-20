public class FStop
{
	private static FStop[] l;

	public float fstop;

	public string name;

	public static FStop[] list => l;

	static FStop()
	{
		if (list == null || list.Length == 0)
		{
			InitList();
		}
	}

	public FStop(string name, float fstop)
	{
		this.name = name;
		this.fstop = fstop;
	}

	private static void InitList()
	{
		l = new FStop[9]
		{
			new FStop("f1.4", 1.4f),
			new FStop("f2", 2f),
			new FStop("f2.8", 2.8f),
			new FStop("f4", 4f),
			new FStop("f5.6", 5.6f),
			new FStop("f8", 8f),
			new FStop("f11", 11f),
			new FStop("f16", 16f),
			new FStop("f22", 22f)
		};
	}
}
