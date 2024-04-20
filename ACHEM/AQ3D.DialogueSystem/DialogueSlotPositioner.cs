using System.Collections.Generic;
using UnityEngine;

namespace AQ3D.DialogueSystem;

public class DialogueSlotPositioner : MonoBehaviour
{
	[SerializeField]
	private DialogueSlot[] DialogueSlots;

	public void AddCharacterToSlot(int slotid, GameObject go)
	{
		if (slotid > 0 && slotid <= DialogueSlots.Length - 1)
		{
			DialogueSlots[slotid].Init(go);
		}
	}

	public void SetWeapon(int slotid, bool useweapon)
	{
		if ((bool)DialogueSlots[slotid].player)
		{
			DialogueSlots[slotid].player.HideWeaponGos(useweapon);
		}
	}

	public void SetPosition(DialogueSceneType sceneType, int sceneIndex = 0)
	{
		if (sceneType == DialogueSceneType.Scene && (bool)DialogueScene.currentDialogueScene)
		{
			Transform scene = DialogueScene.currentDialogueScene.GetScene(sceneIndex);
			Vector3 position = scene.position;
			Vector3 eulerAngles = scene.eulerAngles;
			base.transform.localPosition = Vector3.zero + position;
			base.transform.eulerAngles = eulerAngles;
		}
		else
		{
			base.transform.position = new Vector3(0f, -1000f, 0f);
			base.transform.localEulerAngles = Vector3.zero;
		}
	}

	public void InitSlots(List<SlotPosition> Slots, GameObject cam = null)
	{
		if (Slots == null)
		{
			return;
		}
		for (int i = 0; i < Slots.Count; i++)
		{
			GameObject gameObject;
			if (Slots[i].SlotID == 0)
			{
				if (cam != null)
				{
					gameObject = cam;
					Vector3 localPosition = Slots[i].Position;
					Vector3 localEulerAngles = Slots[i].Rotation;
					gameObject.transform.localPosition = localPosition;
					gameObject.transform.localEulerAngles = localEulerAngles;
					gameObject.GetComponent<Camera>().fieldOfView = Slots[i].FOV;
				}
				continue;
			}
			gameObject = DialogueSlots[Slots[i].SlotID].gameObject;
			if (gameObject != null)
			{
				Vector3 localPosition2 = Slots[i].Position;
				Vector3 localEulerAngles2 = Slots[i].Rotation;
				gameObject.transform.localPosition = localPosition2;
				gameObject.transform.localEulerAngles = localEulerAngles2;
				if (gameObject.transform.childCount > 0)
				{
					Vector3 localPosition3 = Slots[i].OffsetPosition;
					Vector3 localEulerAngles3 = Slots[i].OffsetRotation;
					float scale = Slots[i].Scale;
					gameObject.transform.GetChild(0).localPosition = localPosition3;
					gameObject.transform.GetChild(0).localEulerAngles = localEulerAngles3;
					gameObject.transform.GetChild(0).localScale = Vector3.one * scale;
					gameObject.transform.GetChild(0).gameObject.SetActive(value: true);
				}
			}
		}
	}

	public void PlayAnimation(int dialogid, int frameid, string animationstate, int slotid)
	{
		if (!string.IsNullOrEmpty(animationstate))
		{
			DialogueSlots[slotid].PlayAnimation(dialogid, frameid, animationstate, slotid);
		}
	}

	public void SetFace(float[] mouth, float[] eyes, int slotid, bool isempty = true)
	{
		if (mouth[0] != -1f && mouth[1] != -1f && eyes[0] != -1f && eyes[1] != -1f)
		{
			Vector4 faceOffset = new Vector4(eyes[0], eyes[1], mouth[0], mouth[1]);
			if ((bool)DialogueSlots[slotid].player)
			{
				DialogueSlots[slotid].player.SetFaceOffset(faceOffset);
			}
		}
	}

	public void ClearObjects()
	{
		for (int i = 0; i < 5; i++)
		{
			SetFace(new float[2], new float[2], i);
			DialogueSlots[i].Clear();
		}
	}
}
