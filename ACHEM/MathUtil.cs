using UnityEngine;

public static class MathUtil
{
	public static bool LinePlaneIntersection(out Vector3 intersection, Vector3 linePoint, Vector3 lineVec, Vector3 planeNormal, Vector3 planePoint)
	{
		intersection = Vector3.zero;
		float num = Vector3.Dot(lineVec, planeNormal);
		if (num != 0f)
		{
			float num2 = Vector3.Dot(planePoint - linePoint, planeNormal) / num;
			Vector3 vector = lineVec * num2;
			intersection = linePoint + vector;
			return true;
		}
		return false;
	}
}
