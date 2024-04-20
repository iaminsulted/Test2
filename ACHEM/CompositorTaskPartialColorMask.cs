using UnityEngine;

public class CompositorTaskPartialColorMask : CompositorTask
{
	public Texture2D TextureCM;

	public Texture2D TextureC;

	public Color32 ColorR;

	public Color32 ColorG;

	public Color32 ColorB;

	public CompositorTaskPartialColorMask(Rectangle rect, Texture2D texCM, Texture2D texD, Texture2D texC, EntityArmorColor c)
	{
		Rect = rect;
		TextureD = texD;
		TextureCM = texCM;
		TextureC = texC;
		ColorR = c.R;
		ColorG = c.G;
		ColorB = c.B;
	}
}
