using System.Collections.Generic;

public class ChatFilteredMessage
{
	public int code;

	public List<string> terms = new List<string>();

	public List<int> indices = new List<int>();

	public string maskedMessage;

	public override string ToString()
	{
		string text = "Code: " + code + ", Terms: ";
		int count = terms.Count;
		for (int i = 0; i < count; i++)
		{
			text += terms[i];
			if (i < count - 1)
			{
				text += ", ";
			}
		}
		text += ", Indices:";
		count = indices.Count;
		for (int i = 0; i < count; i++)
		{
			text += indices[i];
			if (i < count - 1)
			{
				text += ", ";
			}
		}
		return text + ", msg: " + maskedMessage;
	}
}
