using System.Collections;
using System.Collections.Generic;

public class XMLNodeSimple
{
	public string tagName;

	public XMLNodeSimple parentNode;

	public ArrayList children;

	public Dictionary<string, string> attributes;

	public XMLNodeSimple()
	{
		tagName = "NONE";
		parentNode = null;
		children = new ArrayList();
		attributes = new Dictionary<string, string>();
	}
}
