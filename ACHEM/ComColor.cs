using UnityEngine;

public class ComColor
{
	public float R;

	public float G;

	public float B;

	public float A;

	public ComColor(Color color)
	{
		R = color.r;
		G = color.g;
		B = color.b;
		A = color.a;
	}

	public Color GetColor()
	{
		return new Color(R, G, B, A);
	}
}
