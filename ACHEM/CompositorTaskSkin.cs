using UnityEngine;

public class CompositorTaskSkin : CompositorTask
{
	public Color32 ColorA;

	public Color32 ColorB;

	public CompositorTaskSkin(Rectangle rect, Texture2D tex, Color32 colorA, Color32 colorB)
	{
		Rect = rect;
		TextureD = tex;
		ColorA = colorA;
		ColorB = colorB;
	}
}
