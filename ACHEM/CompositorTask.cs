using UnityEngine;

public class CompositorTask
{
	public Rectangle Rect;

	public Texture2D TextureD;

	public CompositorTask()
	{
	}

	public CompositorTask(Rectangle rect, Texture2D tex)
	{
		Rect = rect;
		TextureD = tex;
	}
}
