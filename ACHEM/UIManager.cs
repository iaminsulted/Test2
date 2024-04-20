using Assets.Scripts.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public Camera uiCamera;

	private static UIManager instance;

	public static UIManager Instance => instance;

	private void Start()
	{
		instance = this;
		Object.Instantiate(Resources.Load<GameObject>("UIElements/Tooltip"), base.transform);
	}

	public void AddPTRWatermark(int buildNumber)
	{
		Object.Instantiate(Resources.Load<GameObject>("UIElements/PTR watermark"), base.transform).GetComponent<PtrWatermark>().Init(buildNumber);
	}

	private void OnDestroy()
	{
		instance = null;
	}

	public static void Reset()
	{
		ModalWindow.Clear();
		UIWindow.ClearWindows();
		UIContextMenu.Close();
		UIContextIconsMenu.Close();
		UITutorialPopup.Clear();
		UITutorialHighlighter.Close();
	}
}
