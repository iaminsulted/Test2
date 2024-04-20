using System;
using UnityEngine;

[Serializable]
public class RectTelegraph : BaseTelegraph
{
	private const string rectMaterialName = "Materials/Spell_RectTelegraph_Material";

	public Vector2 Size;

	private static Material RectMaterial => ResourceCache.Load<Material>("Materials/Spell_RectTelegraph_Material");

	public RectTelegraph(Vector3 offsetPosition, float offsetRotation, Vector2 size)
		: base(offsetPosition, offsetRotation, null)
	{
		Size = size;
	}

	public override void Draw(Projector projector, Color color)
	{
		base.Draw(projector, color);
		projector.transform.localPosition = new Vector3(0f, 0f, Size.y / 2f);
		projector.material = new Material(RectMaterial);
		projector.material.SetColor("_Color", color);
		projector.material.SetColor("_OutlineColor", color);
		projector.material.SetFloat("_Hardness", 15f);
		projector.material.SetFloat("_OutlineWidth", 0.2f);
		projector.aspectRatio = Size.x / Size.y;
		projector.orthographicSize = Size.y * 0.5f;
	}

	public override string ToString()
	{
		return $"Length: {Size.y}\nWidth: {Size.x}";
	}
}
