using UnityEngine;

public class AEVector3
{
	public float X;

	public float Y;

	public float Z;

	public AEVector3()
	{
	}

	public AEVector3(float x, float y, float z)
	{
		X = x;
		Y = y;
		Z = z;
	}

	public AEVector3(Vector3 vector3)
	{
		X = vector3.x;
		Y = vector3.y;
		Z = vector3.z;
	}

	public override string ToString()
	{
		return "(x:" + X + ", y:" + Y + ", z:" + Z + ")";
	}

	public float getMagnitude()
	{
		return Mathf.Pow(Mathf.Pow(X, 2f) + Mathf.Pow(Y, 2f) + Mathf.Pow(Z, 2f), 0.5f);
	}

	public float getMagnitudeSqr()
	{
		return Mathf.Pow(X, 2f) + Mathf.Pow(Y, 2f) + Mathf.Pow(Z, 2f);
	}

	public static AEVector3 operator -(AEVector3 v1, AEVector3 v2)
	{
		return new AEVector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
	}

	public static AEVector3 operator +(AEVector3 v1, AEVector3 v2)
	{
		return new AEVector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
	}

	public static AEVector3 Min(AEVector3 v1, AEVector3 v2)
	{
		return new AEVector3(Mathf.Min(v1.X, v2.X), Mathf.Min(v1.Y, v2.Y), Mathf.Min(v1.Z, v2.Z));
	}

	public static AEVector3 Max(AEVector3 v1, AEVector3 v2)
	{
		return new AEVector3(Mathf.Max(v1.X, v2.X), Mathf.Max(v1.Y, v2.Y), Mathf.Max(v1.Z, v2.Z));
	}
}
