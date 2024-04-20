using System.Collections.Generic;
using UnityEngine;

public class TextAndSpriteSeprator : MonoBehaviour
{
	private void OnValidate()
	{
		UILabel[] componentsInChildren = GetComponentsInChildren<UILabel>();
		foreach (UILabel uILabel in componentsInChildren)
		{
			uILabel.depth += ((uILabel.depth < 100) ? 100 : 0);
		}
		UISprite[] componentsInChildren2 = GetComponentsInChildren<UISprite>();
		List<string> list = new List<string>();
		UISprite[] array = componentsInChildren2;
		foreach (UISprite uISprite in array)
		{
			if (!list.Contains(uISprite.spriteName))
			{
				list.Add(uISprite.spriteName);
			}
		}
	}
}
