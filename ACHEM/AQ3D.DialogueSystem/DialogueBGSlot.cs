using UnityEngine;

namespace AQ3D.DialogueSystem;

public class DialogueBGSlot : MonoBehaviour
{
	public MeshRenderer BackgroundSphere;

	public void SetSphereActive(bool show)
	{
		if (BackgroundSphere != null)
		{
			BackgroundSphere.gameObject.SetActive(show);
		}
	}

	public void SetBackground(Texture2D bgtouse = null)
	{
		if (bgtouse != null && BackgroundSphere != null)
		{
			BackgroundSphere.sharedMaterial.mainTexture = bgtouse;
		}
	}
}
