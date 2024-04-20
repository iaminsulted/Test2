using UnityEngine;

public class Notification : MonoBehaviour
{
	private const float Default_Duration = 3f;

	private const float Fade_Duration = 0.65f;

	private static Notification Instance;

	public UILabel text;

	private float currentAlpha;

	private float fadeStartTime;

	private void Awake()
	{
		Instance = this;
	}

	private void OnDestroy()
	{
		Instance = null;
	}

	private void Update()
	{
		if (!(Time.time < fadeStartTime) || !(currentAlpha > 0f))
		{
			currentAlpha = Mathf.Lerp(1f, 0f, (Time.time - fadeStartTime) / 0.65f);
			SetAlpha(currentAlpha * currentAlpha);
			if (currentAlpha < 0.1f)
			{
				currentAlpha = 0f;
				base.gameObject.SetActive(value: false);
			}
		}
	}

	private void SetAlpha(float alpha)
	{
		Color color = text.color;
		color.a = alpha;
		text.color = color;
	}

	private void SetColor(Color color)
	{
		text.color = color;
	}

	private void SetText(string notificationText, Color color, float duration)
	{
		if (!string.IsNullOrEmpty(notificationText))
		{
			fadeStartTime = Time.time + duration;
			currentAlpha = 1f;
			SetColor(color);
			SetAlpha(currentAlpha);
			text.text = notificationText;
			base.gameObject.SetActive(value: true);
		}
	}

	public static void ShowText(string notificationText, float duration = 3f)
	{
		Color color = new Color(1f, 0.8901961f, 0.61960787f);
		if (Instance == null)
		{
			Instance = CreateInstance();
		}
		Instance.SetText(notificationText, color, duration);
	}

	public static void ShowSpellWarning(string spellWarning, float duration = 1f)
	{
		if ((bool)SettingsManager.ShowSpellErrors)
		{
			ShowWarning(spellWarning, duration);
		}
	}

	public static void ShowWarning(string notificationText, float duration = 3f)
	{
		if (Instance == null)
		{
			Instance = CreateInstance();
		}
		Instance.SetText(notificationText, InterfaceColors.Names.Staff, duration);
	}

	private static Notification CreateInstance()
	{
		GameObject obj = Object.Instantiate(Resources.Load<GameObject>("Notification"), UIManager.Instance.transform);
		obj.name = "Notification";
		return obj.GetComponent<Notification>();
	}

	public static void Clear()
	{
		if (Instance != null)
		{
			Instance.gameObject.SetActive(value: false);
		}
	}
}
