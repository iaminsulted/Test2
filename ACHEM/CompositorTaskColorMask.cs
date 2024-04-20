using UnityEngine;

public class CompositorTaskColorMask : CompositorTask
{
	public Texture2D TextureCM;

	public Color32 ColorR;

	public Color32 ColorG;

	public Color32 ColorB;

	public CompositorTaskColorMask(Rectangle rect, Texture2D texCM, Texture2D texD, EntityArmorColor c)
	{
		Rect = rect;
		TextureD = texD;
		TextureCM = texCM;
		ColorR = c.R;
		ColorG = c.G;
		ColorB = c.B;
	}
}
