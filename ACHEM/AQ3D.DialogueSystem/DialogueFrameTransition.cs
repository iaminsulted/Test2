using System;

namespace AQ3D.DialogueSystem;

[Serializable]
public class DialogueFrameTransition
{
	public int FrameID;

	public float TransitionTime;

	public float Delay;

	public int Easing;

	public ComVector3 Position;

	public ComVector3 Rotation;
}
