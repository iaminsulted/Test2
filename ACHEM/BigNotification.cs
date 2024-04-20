using UnityEngine;

public class BigNotification : MonoBehaviour
{
	private static BigNotification instance;

	public UILabel text;

	private float mTarget;

	private float mCurrent;

	private float fadeSpeed = 0.3f;

	private float startTime;

	private bool hiding;

	private void Awake()
	{
		instance = this;
	}

	private void OnDestroy()
	{
		instance = null;
	}

	private void Update()
	{
		if (!hiding && Time.time > startTime)
		{
			hiding = true;
			mTarget = 0f;
		}
		if (hiding)
		{
			mCurrent = Mathf.Lerp(mCurrent, mTarget, Time.deltaTime * fadeSpeed / mCurrent);
			if (Mathf.Abs(mCurrent - mTarget) < 0.1f)
			{
				mCurrent = mTarget;
			}
			SetAlpha(mCurrent * mCurrent);
			if (Mathf.Approximately(mCurrent, 0f))
			{
				base.gameObject.SetActive(value: false);
			}
		}
	}

	private void SetAlpha(float val)
	{
		Color color = text.color;
		color.a = val;
		text.color = color;
	}

	private void SetColor(Color c)
	{
		text.color = c;
	}

	private void SetText(string notificationText, Color c)
	{
		if (!string.IsNullOrEmpty(notificationText))
		{
			hiding = false;
			startTime = Time.time + 3f;
			mCurrent = 1f;
			SetColor(c);
			SetAlpha(mCurrent);
			text.text = notificationText;
			base.gameObject.SetActive(value: true);
		}
	}

	public static void ShowText(string notificationText)
	{
		if (instance == null)
		{
			Instantiate();
		}
		instance.SetText(notificationText, Color.black);
	}

	public static void ShowWarning(string notificationText)
	{
		if (instance == null)
		{
			Instantiate();
		}
		instance.SetText(notificationText, Color.red);
	}

	private static void Instantiate()
	{
		GameObject obj = Object.Instantiate(Resources.Load<GameObject>("Notification"), UIManager.Instance.transform);
		obj.name = "Notification";
		instance = obj.GetComponent<BigNotification>();
	}
}
