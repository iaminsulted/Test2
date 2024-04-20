using UnityEngine;

public class Loader : MonoBehaviour
{
	public UISlider LoadProgressBarSlider;

	public UILabel LabelProgress;

	public UILabel LabelMessage;

	public UILabel LabelTip;

	public GameObject logoutButton;

	private static Loader mInstance;

	public static bool IsVisible
	{
		get
		{
			if (mInstance != null)
			{
				return mInstance.gameObject.activeSelf;
			}
			return false;
		}
	}

	private void Awake()
	{
		mInstance = this;
	}

	private void OnEnable()
	{
		Invoke("ShowLogOut", 5f);
	}

	private void OnDisable()
	{
		logoutButton.SetActive(value: false);
		CancelInvoke("ShowLogOut");
	}

	private void OnDestroy()
	{
		mInstance = null;
	}

	public static void show(string message, float progress)
	{
		if (mInstance == null)
		{
			GameObject obj = Object.Instantiate(Resources.Load<GameObject>("Loader"), UIManager.Instance.transform);
			obj.name = "Loader";
			mInstance = obj.GetComponent<Loader>();
			if (GameTips.RandomTip != null)
			{
				mInstance.LabelTip.text = GameTips.RandomTip;
			}
		}
		else
		{
			mInstance.gameObject.SetActive(value: true);
		}
		mInstance.LabelMessage.enabled = true;
		mInstance.LabelMessage.text = message;
		mInstance.LabelProgress.text = Mathf.CeilToInt(progress * 100f) + "%";
		mInstance.LoadProgressBarSlider.value = progress;
	}

	public static void close()
	{
		try
		{
			mInstance.LabelMessage.text = "";
			mInstance.LabelMessage.enabled = false;
			mInstance.gameObject.SetActive(value: false);
		}
		catch
		{
			Debug.LogWarning("Loader Is Not Open!");
		}
	}

	public void Logout()
	{
		StateManager.Instance.LoadState("scene.login");
		close();
	}

	private void ShowLogOut()
	{
		logoutButton.SetActive(value: true);
	}
}
