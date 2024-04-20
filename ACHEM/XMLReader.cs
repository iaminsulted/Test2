using System.Collections.Generic;
using UnityEngine;

public class XMLReader
{
	private static char TAG_START = '<';

	private static char TAG_END = '>';

	private static char SPACE = ' ';

	private static char QUOTE = '"';

	private static char SLASH = '/';

	private static char EQUALS = '=';

	private static string BEGIN_QUOTE = EQUALS.ToString() + QUOTE;

	public XMLNodeSimple read(string xml)
	{
		int num = 0;
		int num2 = 0;
		XMLNodeSimple xMLNodeSimple = new XMLNodeSimple();
		XMLNodeSimple xMLNodeSimple2 = xMLNodeSimple;
		while (true)
		{
			num = xml.IndexOf(TAG_START, num2);
			if (num < 0 || num >= xml.Length)
			{
				break;
			}
			num++;
			num2 = xml.IndexOf(TAG_END, num);
			if (num2 < 0 || num2 >= xml.Length)
			{
				break;
			}
			int num3 = num2 - num;
			string text = xml.Substring(num, num3);
			if (text[0] == SLASH)
			{
				xMLNodeSimple2 = xMLNodeSimple2.parentNode;
				continue;
			}
			bool flag = true;
			if (text[num3 - 1] == SLASH)
			{
				text = text.Substring(0, num3 - 1);
				flag = false;
			}
			XMLNodeSimple xMLNodeSimple3 = parseTag(text);
			xMLNodeSimple3.parentNode = xMLNodeSimple2;
			xMLNodeSimple2.children.Add(xMLNodeSimple3);
			if (flag)
			{
				xMLNodeSimple2 = xMLNodeSimple3;
			}
		}
		return xMLNodeSimple;
	}

	public XMLNodeSimple parseTag(string xmlTag)
	{
		XMLNodeSimple xMLNodeSimple = new XMLNodeSimple();
		int num = xmlTag.IndexOf(SPACE, 0);
		if (num < 0)
		{
			xMLNodeSimple.tagName = xmlTag;
			return xMLNodeSimple;
		}
		string tagName = xmlTag.Substring(0, num);
		xMLNodeSimple.tagName = tagName;
		string xmlTag2 = xmlTag.Substring(num, xmlTag.Length - num);
		return parseAttributes(xmlTag2, xMLNodeSimple);
	}

	public XMLNodeSimple parseAttributes(string xmlTag, XMLNodeSimple node)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		while (true)
		{
			num = xmlTag.IndexOf(BEGIN_QUOTE, num3);
			if (num < 0 || num > xmlTag.Length)
			{
				break;
			}
			num2 = xmlTag.LastIndexOf(SPACE, num);
			if (num2 < 0 || num2 > xmlTag.Length)
			{
				break;
			}
			num2++;
			string key = xmlTag.Substring(num2, num - num2);
			num += 2;
			num3 = xmlTag.IndexOf(QUOTE, num);
			if (num3 < 0 || num3 > xmlTag.Length)
			{
				break;
			}
			int length = num3 - num;
			string value = xmlTag.Substring(num, length);
			node.attributes[key] = value;
		}
		return node;
	}

	public void printXML(XMLNodeSimple node, int indent)
	{
		indent++;
		foreach (XMLNodeSimple child in node.children)
		{
			string text = " ";
			foreach (KeyValuePair<string, string> attribute in child.attributes)
			{
				text = text + "[" + attribute.Key + ": " + attribute.Value + "] ";
			}
			string text2 = "";
			for (int i = 0; i < indent; i++)
			{
				text2 += "-";
			}
			Debug.Log(text2 + " " + child.tagName + text);
			printXML(child, indent);
		}
	}
}
