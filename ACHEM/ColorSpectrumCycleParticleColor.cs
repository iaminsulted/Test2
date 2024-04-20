using UnityEngine;

public class ColorSpectrumCycleParticleColor : MonoBehaviour
{
	private enum TargetColor
	{
		Blue,
		Magenta,
		Red,
		Yellow,
		Green,
		Cyan
	}

	private Material material;

	private Color color = Color.blue;

	private TargetColor targetColor = TargetColor.Magenta;

	[SerializeField]
	private string materialColorName = "_Color";

	public float colorRate = 0.2f;

	public Color additive = Color.black;

	private void Start()
	{
		material = GetComponent<ParticleSystemRenderer>().material;
		material.SetColor(materialColorName, color);
	}

	private void Update()
	{
		switch (targetColor)
		{
		case TargetColor.Blue:
			color.g -= Time.deltaTime * colorRate;
			if (color.g <= 0f)
			{
				color.g = 0f;
				targetColor = TargetColor.Magenta;
			}
			break;
		case TargetColor.Magenta:
			color.r += Time.deltaTime * colorRate;
			if (color.r >= 1f)
			{
				color.r = 1f;
				targetColor = TargetColor.Red;
			}
			break;
		case TargetColor.Red:
			color.b -= Time.deltaTime * colorRate;
			if (color.b <= 0f)
			{
				color.b = 0f;
				targetColor = TargetColor.Yellow;
			}
			break;
		case TargetColor.Yellow:
			color.g += Time.deltaTime * colorRate;
			if (color.g >= 1f)
			{
				color.g = 1f;
				targetColor = TargetColor.Green;
			}
			break;
		case TargetColor.Green:
			color.r -= Time.deltaTime * colorRate;
			if (color.r <= 0f)
			{
				color.r = 0f;
				targetColor = TargetColor.Cyan;
			}
			break;
		case TargetColor.Cyan:
			color.b += Time.deltaTime * colorRate;
			if (color.b >= 1f)
			{
				color.b = 1f;
				targetColor = TargetColor.Blue;
			}
			break;
		}
		material.SetColor(materialColorName, color + additive);
	}
}
