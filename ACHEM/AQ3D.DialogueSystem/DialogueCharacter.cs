using System;
using UnityEngine;

namespace AQ3D.DialogueSystem;

[Serializable]
public class DialogueCharacter
{
	public int NPCID;

	public int SlotID;

	public string CutsceneName;

	public Vector3 WorldPosition;

	public Vector3 Rotation;

	public float OffsetY;
}
