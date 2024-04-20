using UnityEngine;

public class FancyNotification : MonoBehaviour
{
	private static FancyNotification mInstance;

	public UILabel Title;

	public UILabel Message;

	public TweenScale tweenScale;

	public TweenAlpha tweenAlpha;

	private void Awake()
	{
		mInstance = this;
	}

	private void OnDestroy()
	{
		mInstance = null;
	}

	public void OnTweenComplete()
	{
		base.gameObject.SetActive(value: false);
	}

	private void SetText(string title, string message)
	{
		if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(message))
		{
			Title.text = title;
			Message.text = message;
			tweenScale.ResetToBeginning();
			tweenAlpha.ResetToBeginning();
			tweenScale.PlayForward();
			tweenAlpha.PlayForward();
			base.gameObject.SetActive(value: true);
		}
	}

	public static void ShowText(string title, string message)
	{
		if (mInstance == null)
		{
			GameObject obj = Object.Instantiate(Resources.Load<GameObject>("FancyNotification"), UIManager.Instance.transform);
			obj.name = "FancyNotification";
			mInstance = obj.GetComponent<FancyNotification>();
		}
		mInstance.SetText(title, message);
	}

	public static void Clear()
	{
		if (mInstance != null)
		{
			mInstance.gameObject.SetActive(value: false);
		}
	}
}
