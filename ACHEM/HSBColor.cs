using System;
using UnityEngine;

[Serializable]
public class HSBColor
{
	public float h;

	public float s;

	public float b;

	public float a;

	public HSBColor()
	{
	}

	public HSBColor(float h, float s, float b, float a)
	{
		this.h = h;
		this.s = s;
		this.b = b;
		this.a = a;
	}

	public HSBColor(float h, float s, float b)
	{
		this.h = h;
		this.s = s;
		this.b = b;
		a = 1f;
	}

	public HSBColor(Color col)
	{
		HSBColor hSBColor = FromColorB(col);
		h = hSBColor.h;
		s = hSBColor.s;
		b = hSBColor.b;
		a = hSBColor.a;
	}

	public static HSBColor FromColorB(Color color)
	{
		HSBColor hSBColor = new HSBColor();
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float r = color.r;
		float g = color.g;
		float num4 = color.b;
		float num5 = Mathf.Min(r, Mathf.Min(g, num4));
		float num6 = Mathf.Max(r, Mathf.Max(g, num4));
		float num7 = num6 - num5;
		num3 = num6;
		if (num7 == 0f)
		{
			num = 0f;
			num2 = 0f;
		}
		else
		{
			num2 = num7 / num6;
			float num8 = ((num6 - r) / 6f + num7 / 2f) / num7;
			float num9 = ((num6 - g) / 6f + num7 / 2f) / num7;
			float num10 = ((num6 - num4) / 6f + num7 / 2f) / num7;
			if (r == num6)
			{
				num = num10 - num9;
			}
			else if (g == num6)
			{
				num = 1f / 3f + num8 - num10;
			}
			else if (num4 == num6)
			{
				num = 2f / 3f + num9 - num8;
			}
			if (num < 0f)
			{
				num += 1f;
			}
			if (num > 1f)
			{
				num -= 1f;
			}
		}
		hSBColor.h = num;
		hSBColor.s = num2;
		hSBColor.b = num3;
		hSBColor.a = color.a;
		return hSBColor;
	}

	public static Color ToColorB(HSBColor hsbColor)
	{
		Color result = default(Color);
		result.a = hsbColor.a;
		float num = hsbColor.h;
		float num2 = hsbColor.s;
		float num3 = hsbColor.b;
		if (num2 == 0f)
		{
			result.r = num3;
			result.g = num3;
			result.b = num3;
		}
		else
		{
			float num4 = num * 6f;
			if (num4 == 6f)
			{
				num4 = 0f;
			}
			int num5 = Mathf.FloorToInt(num4);
			float num6 = num3 * (1f - num2);
			float num7 = num3 * (1f - num2 * (num4 - (float)num5));
			float num8 = num3 * (1f - num2 * (1f - (num4 - (float)num5)));
			float r;
			float g;
			float num9;
			if ((float)num5 == 0f)
			{
				r = num3;
				g = num8;
				num9 = num6;
			}
			else if ((float)num5 == 1f)
			{
				r = num7;
				g = num3;
				num9 = num6;
			}
			else if ((float)num5 == 2f)
			{
				r = num6;
				g = num3;
				num9 = num8;
			}
			else if ((float)num5 == 3f)
			{
				r = num6;
				g = num7;
				num9 = num3;
			}
			else if ((float)num5 == 4f)
			{
				r = num8;
				g = num6;
				num9 = num3;
			}
			else
			{
				r = num3;
				g = num6;
				num9 = num7;
			}
			result.r = r;
			result.g = g;
			result.b = num9;
		}
		return result;
	}

	public static HSBColor AddOffset(HSBColor baseColor, HSBColor offset)
	{
		HSBColor hSBColor = new HSBColor(baseColor.h, baseColor.s, baseColor.b, baseColor.a);
		float num = baseColor.h * 360f;
		num += offset.h * 360f;
		num %= 360f;
		num /= 360f;
		hSBColor.h = num;
		hSBColor.s = Mathf.Clamp01(baseColor.s + offset.s);
		hSBColor.b = Mathf.Clamp01(baseColor.b + offset.b);
		return hSBColor;
	}

	public static HSBColor FromColor(Color color)
	{
		HSBColor hSBColor = new HSBColor(0f, 0f, 0f, color.a);
		float r = color.r;
		float g = color.g;
		float num = color.b;
		float num2 = Mathf.Max(r, Mathf.Max(g, num));
		if (num2 <= 0f)
		{
			return hSBColor;
		}
		float num3 = Mathf.Min(r, Mathf.Min(g, num));
		float num4 = num2 - num3;
		if (num2 > num3)
		{
			if (g == num2)
			{
				hSBColor.h = (num - r) / num4 * 60f + 120f;
			}
			else if (num == num2)
			{
				hSBColor.h = (r - g) / num4 * 60f + 240f;
			}
			else if (num > g)
			{
				hSBColor.h = (g - num) / num4 * 60f + 360f;
			}
			else
			{
				hSBColor.h = (g - num) / num4 * 60f;
			}
			if (hSBColor.h < 0f)
			{
				hSBColor.h += 360f;
			}
		}
		else
		{
			hSBColor.h = 0f;
		}
		hSBColor.h *= 0.0027777778f;
		hSBColor.s = num4 / num2 * 1f;
		hSBColor.b = num2;
		return hSBColor;
	}

	public static Color ToColor(HSBColor hsbColor)
	{
		float value = hsbColor.b;
		float value2 = hsbColor.b;
		float value3 = hsbColor.b;
		if (hsbColor.s != 0f)
		{
			float num = hsbColor.b;
			float num2 = hsbColor.b * hsbColor.s;
			float num3 = hsbColor.b - num2;
			float num4 = hsbColor.h * 360f;
			if (num4 < 60f)
			{
				value = num;
				value2 = num4 * num2 / 60f + num3;
				value3 = num3;
			}
			else if (num4 < 120f)
			{
				value = (0f - (num4 - 120f)) * num2 / 60f + num3;
				value2 = num;
				value3 = num3;
			}
			else if (num4 < 180f)
			{
				value = num3;
				value2 = num;
				value3 = (num4 - 120f) * num2 / 60f + num3;
			}
			else if (num4 < 240f)
			{
				value = num3;
				value2 = (0f - (num4 - 240f)) * num2 / 60f + num3;
				value3 = num;
			}
			else if (num4 < 300f)
			{
				value = (num4 - 240f) * num2 / 60f + num3;
				value2 = num3;
				value3 = num;
			}
			else if (num4 <= 360f)
			{
				value = num;
				value2 = num3;
				value3 = (0f - (num4 - 360f)) * num2 / 60f + num3;
			}
			else
			{
				value = 0f;
				value2 = 0f;
				value3 = 0f;
			}
		}
		return new Color(Mathf.Clamp01(value), Mathf.Clamp01(value2), Mathf.Clamp01(value3), hsbColor.a);
	}

	public Color ToColor()
	{
		return ToColor(this);
	}

	public Color ToColorB()
	{
		return ToColorB(this);
	}

	public override string ToString()
	{
		return "H:" + h + " S:" + s + " B:" + b;
	}

	public static HSBColor Lerp(HSBColor a, HSBColor b, float t)
	{
		float num;
		float num2;
		if (a.b == 0f)
		{
			num = b.h;
			num2 = b.s;
		}
		else if (b.b == 0f)
		{
			num = a.h;
			num2 = a.s;
		}
		else
		{
			if (a.s == 0f)
			{
				num = b.h;
			}
			else if (b.s == 0f)
			{
				num = a.h;
			}
			else
			{
				float num3;
				for (num3 = Mathf.LerpAngle(a.h * 360f, b.h * 360f, t); num3 < 0f; num3 += 360f)
				{
				}
				while (num3 > 360f)
				{
					num3 -= 360f;
				}
				num = num3 / 360f;
			}
			num2 = Mathf.Lerp(a.s, b.s, t);
		}
		return new HSBColor(num, num2, Mathf.Lerp(a.b, b.b, t), Mathf.Lerp(a.a, b.a, t));
	}

	public static void Test()
	{
		Debug.Log("red: " + new HSBColor(Color.red));
		Debug.Log("green: " + new HSBColor(Color.green));
		Debug.Log("blue: " + new HSBColor(Color.blue));
		Debug.Log("grey: " + new HSBColor(Color.grey));
		Debug.Log("white: " + new HSBColor(Color.white));
		Debug.Log("0.4, 1f, 0.84: " + new HSBColor(new Color(0.4f, 1f, 0.84f, 1f)));
		Debug.Log("164,82,84   .... 0.643137f, 0.321568f, 0.329411f  :" + ToColor(new HSBColor(new Color(0.643137f, 0.321568f, 0.329411f))).ToString());
	}
}
