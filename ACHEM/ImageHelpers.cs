using UnityEngine;

public static class ImageHelpers
{
	public static Texture2D AddWatermark(Texture2D background, Texture2D watermark, int startX, int startY)
	{
		Texture2D texture2D = new Texture2D(background.width, background.height, background.format, mipChain: false);
		for (int i = 0; i < background.width; i++)
		{
			for (int j = 0; j < background.height; j++)
			{
				if (i >= startX && j >= startY && i < watermark.width && j < watermark.height)
				{
					Color pixel = background.GetPixel(i, j);
					Color pixel2 = watermark.GetPixel(i - startX, j - startY);
					Color color = Color.Lerp(pixel, pixel2, pixel2.a / 1f);
					texture2D.SetPixel(i, j, color);
				}
				else
				{
					texture2D.SetPixel(i, j, background.GetPixel(i, j));
				}
			}
		}
		texture2D.Apply();
		return texture2D;
	}
}
