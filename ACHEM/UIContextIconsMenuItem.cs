using UnityEngine;

public class UIContextIconsMenuItem : MonoBehaviour
{
	public UIContextIconsMenu parent;

	public int Index;

	public UILabel Label;

	public UISprite Sprite;

	public void Init(int i, string s, string sprite)
	{
		Index = i;
		Label.text = s;
		Sprite.spriteName = sprite;
	}

	private void OnClick()
	{
		if (parent != null)
		{
			parent.ContextSelect(Index);
		}
	}
}
