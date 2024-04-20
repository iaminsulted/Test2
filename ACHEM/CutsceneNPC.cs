using AQ3D.DialogueSystem;
using UnityEngine;

public class CutsceneNPC : MonoBehaviour
{
	public int NPCID;

	private float NPCMAPPEDID;

	private DialogueCharacter _char;

	public float NPCMappedID => NPCMAPPEDID;

	public void SetMappedID(float f)
	{
		NPCMAPPEDID = f;
	}

	public DialogueCharacter CreateCharacterData(string cutscenename, int id, int instanceid, bool isMulti = false)
	{
		_char = new DialogueCharacter
		{
			CutsceneName = cutscenename,
			NPCID = id,
			OffsetY = instanceid,
			SlotID = -1,
			Rotation = base.transform.eulerAngles,
			WorldPosition = base.transform.position
		};
		return _char;
	}
}
