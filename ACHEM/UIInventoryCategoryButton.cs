using UnityEngine;

public class UIInventoryCategoryButton : MonoBehaviour
{
	public UIButton Button;

	public GameObject NewIcon;

	public UITexture Texture;

	public void ShowNewIcon(bool show)
	{
		NewIcon.SetActive(show);
	}
}
