using UnityEngine;

public class CompositorTaskDiffuse : CompositorTask
{
	public Texture2D TextureCM;

	public CompositorTaskDiffuse(Rectangle rect, Texture2D texCM, Texture2D texD)
	{
		Rect = rect;
		TextureD = texD;
		TextureCM = texCM;
	}
}
