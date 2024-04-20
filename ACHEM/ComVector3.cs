using UnityEngine;

public class ComVector3
{
	public float x;

	public float y;

	public float z;

	public ComVector3()
	{
	}

	public ComVector3(Vector3 v)
	{
		x = v.x;
		y = v.y;
		z = v.z;
	}

	public ComVector3(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public static implicit operator ComVector3(Vector3 v)
	{
		return new ComVector3(v);
	}

	public static implicit operator Vector3(ComVector3 v)
	{
		return new Vector3(v.x, v.y, v.z);
	}

	public static ComVector3 operator +(ComVector3 a, ComVector3 b)
	{
		return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
	}
}
