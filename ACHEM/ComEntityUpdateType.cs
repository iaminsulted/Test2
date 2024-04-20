using System;

[Flags]
public enum ComEntityUpdateType
{
	None = 0,
	Asset = 1,
	Status = 2,
	Option3 = 4,
	Option4 = 8,
	Option5 = 0x10
}
