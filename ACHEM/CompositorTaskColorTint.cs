using System.Collections.Generic;
using UnityEngine;

public class CompositorTaskColorTint : CompositorTask
{
	public List<Rectangle> Rectangles;

	public List<Texture2D> Textures;

	public List<Color32> Colors;

	public CompositorTaskColorTint(List<Rectangle> rects, List<Texture2D> textures, List<Color32> colors)
	{
		Rectangles = rects;
		Textures = textures;
		Rect = Rectangles[0];
		TextureD = Textures[0];
		Colors = colors;
	}
}
