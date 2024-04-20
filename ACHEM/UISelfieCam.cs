using UnityEngine;

public class UISelfieCam : UIWindow
{
	private static UISelfieCam instance;

	public static void Load()
	{
		if (instance == null)
		{
			instance = Object.Instantiate(Resources.Load<GameObject>("UIElements/SelfieCam"), UIManager.Instance.transform).GetComponent<UISelfieCam>();
			instance.Init();
		}
	}

	protected override void Init()
	{
		base.Init();
		UIGame.Instance.gameObject.SetActive(value: false);
	}

	protected override void Destroy()
	{
		BusyDialog.Close();
		UIGame.Instance.gameObject.SetActive(value: true);
		base.Destroy();
	}

	public void ClickLogo()
	{
		Close();
	}

	public override void Close()
	{
		base.Close();
	}
}
