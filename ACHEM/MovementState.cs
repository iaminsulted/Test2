using System;

[Flags]
public enum MovementState : byte
{
	None = 0,
	Forward = 1,
	Backward = 2,
	LeftStrafe = 4,
	RightStrafe = 8,
	LeftRotate = 0x10,
	RightRotate = 0x20
}
