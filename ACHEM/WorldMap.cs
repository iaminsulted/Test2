using UnityEngine;

public class WorldMap : MonoBehaviour
{
	public static WorldMap instance;

	public UIPanel UIPanel;

	public void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void Start()
	{
		float num = (float)Screen.width / (float)Screen.height;
		UIPanel.baseClipRegion = new Vector4(0f, 0f, 640f * num, 640f);
	}

	public static void Show()
	{
		if (instance == null)
		{
			Object.Instantiate(Resources.Load<GameObject>("UIElements/WorldMap"), UIManager.Instance.transform);
		}
	}
}
