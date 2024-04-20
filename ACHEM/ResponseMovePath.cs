using System.Collections.Generic;

public class ResponseMovePath : Response
{
	public enum PathType
	{
		Move,
		Knockback,
		Summon,
		Sync
	}

	public int ID;

	public List<ComVector3> Path;

	public float StartTS;

	public float Speed;

	public PathType PathingType;

	public string Animations;

	public bool SequentialAnimations;

	public bool UseRotation;

	public int RotationY;
}
