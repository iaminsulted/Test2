using UnityEngine;

public class UIOptions : UIWindow
{
	private static UIOptions mInstance;

	public Transform container;

	public UIItem exit;

	public UIItem settings;

	public UIItem cache;

	public UIItem fblogout;

	public UIItem ResetSettings;

	private void Awake()
	{
		mInstance = this;
		Init();
		settings.Clicked += OnSettingsClick;
		exit.Clicked += OnExitClick;
		cache.Clicked += OnCacheClick;
		fblogout.Clicked += OnLogoutClick;
		ResetSettings.Clicked += OnResetSettingsClick;
	}

	private void OnDestroy()
	{
		mInstance = null;
		settings.Clicked -= OnSettingsClick;
		exit.Clicked -= OnExitClick;
		cache.Clicked -= OnCacheClick;
		fblogout.Clicked -= OnLogoutClick;
		ResetSettings.Clicked -= OnResetSettingsClick;
	}

	public static void Show()
	{
		UIWindow.ClearWindows();
		if (mInstance == null)
		{
			GameObject obj = Object.Instantiate(Resources.Load<GameObject>("UIElements/UIOptions"), UIManager.Instance.transform);
			obj.name = "UIOptions";
			mInstance = obj.GetComponent<UIOptions>();
		}
	}

	protected override void Init()
	{
		base.Init();
		fblogout.gameObject.SetActive(!string.IsNullOrEmpty(FBManager.FacebookAccessToken));
		container.GetComponent<UIGrid>().Reposition();
	}

	private void OnExitClick(UIItem item)
	{
		Confirmation.Show("Exit Game", "The application will close, are you sure?", delegate(bool b)
		{
			if (b)
			{
				Application.Quit();
			}
		});
	}

	private void OnSettingsClick(UIItem item)
	{
		UISettings.Show();
	}

	private void OnCacheClick(UIItem item)
	{
		Confirmation.Show("Clear Cache", "Warning! This will delete all local content. Are you sure you want to clear cache?", delegate(bool b)
		{
			if (b)
			{
				AssetBundleManager.UnloadAll();
				if (Caching.ClearCache())
				{
					Debug.LogWarning("Cache cleared!");
				}
				else
				{
					Debug.LogWarning("Could not clear cache!");
				}
				Close();
			}
		});
	}

	private void OnResetSettingsClick(UIItem item)
	{
		Confirmation.Show("Reset Settings", "Warning! This will delete all user preferences and guest account progress. Are you sure you want to reset settings?", delegate(bool b)
		{
			if (b)
			{
				PlayerPrefs.DeleteAll();
				PlayerPrefs.Save();
				Close();
			}
		});
	}

	private void OnLogoutClick(UIItem item)
	{
		FBManager.Logout();
		fblogout.gameObject.SetActive(value: false);
		container.GetComponent<UIGrid>().Reposition();
		container.parent.GetComponent<UIScrollView>().InvalidateBounds();
		container.parent.GetComponent<UIPanel>().SetDirty();
	}
}
