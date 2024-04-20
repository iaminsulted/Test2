using System.Collections.Generic;
using UnityEngine;

namespace AQ3D.DialogueSystem;

public class DialogueFrame
{
	public int ID;

	public DialogueFrameType FrameType;

	public float frameTime;

	public bool timedFrame;

	public int dialogueScenePosition;

	public string DialogueName;

	public string DialogueTitle;

	public string DialogueText;

	public int alignment;

	public float CameraShakeIntensity;

	public float CameraShakeDuration;

	public string CutsceneName;

	public DialogueFadeType dialogueFadeType;

	public float fadeDuration;

	public Color fadeInColor;

	public Color fadeOutColor;

	public DialogueFrameAction[] FrameActions = new DialogueFrameAction[0];

	public List<SlotPosition> SlotData;

	public bool IsCinematic
	{
		get
		{
			if (FrameType != DialogueFrameType.CinematicOnly)
			{
				return FrameType == DialogueFrameType.Mixed;
			}
			return true;
		}
	}

	public bool IsDialog
	{
		get
		{
			if (FrameType != 0)
			{
				return FrameType == DialogueFrameType.Mixed;
			}
			return true;
		}
	}
}
