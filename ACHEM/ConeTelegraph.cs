using System;
using UnityEngine;

[Serializable]
public class ConeTelegraph : CircleTelegraph
{
	public float Angle;

	public ConeTelegraph(Vector3 offsetPosition, float offsetRotation, float angle, float minRadius, float maxRadius, string mainTextureName)
		: base(offsetPosition, offsetRotation + 180f, minRadius, maxRadius, mainTextureName)
	{
		Angle = angle;
	}

	public override void Draw(Projector projector, Color color)
	{
		base.Draw(projector, color);
		projector.material.SetFloat("_ConeAngle", Angle);
	}

	public override string ToString()
	{
		return base.ToString() + "\nAngle: " + Angle;
	}
}
