using System;

namespace AQ3D.DialogueSystem;

[Serializable]
public class DialogueFrameAction
{
	public int FrameID;

	public int SlotID;

	public int TransitionID = -1;

	public float[] Eyes;

	public float[] Mouth;

	public string AnimationState;

	public DialogueFrameTransition TransitionData;

	public bool UsesWeaponOverride;

	public string AudioClip;

	public float AudioVolume;

	public float AudioDelay;

	public float AudioPan;
}
