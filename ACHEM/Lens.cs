using System;
using UnityEngine;

public class Lens
{
	public string name;

	public int length;

	public float vfov;

	public float offset;

	public Lens(string name, int length, float vfov, float offset)
	{
		this.name = name;
		this.length = length;
		this.vfov = vfov;
		this.offset = offset;
	}

	public static float GetHorizontalFOV(float aspectRatio, float vertical)
	{
		float num = vertical * (MathF.PI / 180f);
		return 2f * Mathf.Atan(Mathf.Tan(num / 2f) * aspectRatio) * 57.29578f;
	}
}
