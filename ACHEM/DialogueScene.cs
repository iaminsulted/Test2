using System.Collections.Generic;
using UnityEngine;

public class DialogueScene : MonoBehaviour
{
	public static DialogueScene currentDialogueScene;

	public List<Transform> SceneSpace = new List<Transform>();

	public int currentScene;

	private void Awake()
	{
		if (currentDialogueScene != null)
		{
			Object.Destroy(currentDialogueScene.gameObject);
		}
		currentDialogueScene = this;
		if (SceneSpace.Count == 0 && base.transform.childCount > 0)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				child.name = base.transform.name + " Scene " + i;
				SceneSpace.Add(child);
			}
		}
	}

	public Transform GetScene(int index)
	{
		if (SceneSpace.Count > 0)
		{
			if (index > SceneSpace.Count - 1)
			{
				currentScene = index % SceneSpace.Count;
			}
			else
			{
				currentScene = index;
			}
			return SceneSpace[currentScene];
		}
		return base.transform;
	}
}
