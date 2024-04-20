using System;
using UnityEngine;

public static class TextureExtensions
{
	public static Texture2D AlphaBlend(this Texture2D bottom, Texture2D top)
	{
		if (bottom.width != top.width || bottom.height != top.height)
		{
			throw new InvalidOperationException("AlphaBlend only works with two equal sized images");
		}
		Color[] pixels = bottom.GetPixels();
		Color[] pixels2 = top.GetPixels();
		int num = pixels.Length;
		Color[] array = new Color[num];
		for (int i = 0; i < num; i++)
		{
			Color a = pixels[i];
			Color b = pixels2[i];
			Color color = Color.Lerp(a, b, b.a);
			color.a = 1f;
			array[i] = color;
		}
		Texture2D texture2D = new Texture2D(top.width, top.height);
		texture2D.SetPixels(array);
		texture2D.Apply(updateMipmaps: true);
		return texture2D;
	}
}
