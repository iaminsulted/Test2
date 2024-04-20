using StatCurves;

public class ResponseItemLog : Response
{
	public string start;

	public string end;

	public string name;

	public RarityType rarity;

	public int count;

	public ResponseItemLog()
	{
		type = 10;
		cmd = 15;
	}

	public ResponseItemLog(string start, RarityType rarity, string name, int count, string end)
	{
		type = 10;
		cmd = 15;
		this.start = start;
		this.rarity = rarity;
		this.name = name;
		this.count = count;
		this.end = end;
	}
}
