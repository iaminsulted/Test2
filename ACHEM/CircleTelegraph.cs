using System;
using UnityEngine;

[Serializable]
public class CircleTelegraph : BaseTelegraph
{
	private const string circleMaterialName = "Materials/Spell_CircleTelegraph_Material";

	public float MinRadius;

	public float MaxRadius;

	private static Material CircleMaterial => ResourceCache.Load<Material>("Materials/Spell_CircleTelegraph_Material");

	public CircleTelegraph(Vector3 offsetPosition, float offsetRotation, float minRadius, float maxRadius, string mainTextureName)
		: base(offsetPosition, offsetRotation, mainTextureName)
	{
		MinRadius = minRadius;
		MaxRadius = maxRadius;
	}

	public override void Draw(Projector projector, Color color)
	{
		base.Draw(projector, color);
		projector.material = new Material(CircleMaterial);
		projector.orthographicSize = MaxRadius;
		projector.material.SetColor("_Color", color);
		projector.material.SetColor("_OutlineColor", color);
		projector.material.SetFloat("_Hardness", 4.5f);
		projector.material.SetFloat("_InnerRadius", MinRadius / MaxRadius);
		projector.material.SetFloat("_ConeAngle", 360f);
		projector.material.SetFloat("_Transparency", 1f);
		if (MainTextureName != null)
		{
			projector.material.SetTexture("_MainTex", base.MainTexture);
		}
	}

	public override string ToString()
	{
		return $"Min radius: {MinRadius}\nMax radius: {MaxRadius}";
	}
}
