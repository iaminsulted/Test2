public class ChatParam
{
	public readonly string name;

	public readonly string example;

	public readonly bool isOptional;

	public readonly string defaultValue;

	public ChatParam(string name, string example, bool isOptional = false, string defaultValue = "")
	{
		this.name = name;
		this.example = example;
		this.isOptional = isOptional;
		this.defaultValue = defaultValue;
	}
}
