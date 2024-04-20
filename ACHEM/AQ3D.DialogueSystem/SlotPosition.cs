using System;

namespace AQ3D.DialogueSystem;

[Serializable]
public class SlotPosition
{
	public int SlotID;

	public float FOV = 60f;

	public ComVector3 Position;

	public ComVector3 Rotation;

	public ComVector3 OffsetPosition;

	public ComVector3 OffsetRotation;

	public float Scale = 1f;

	public float AudioPan;
}
