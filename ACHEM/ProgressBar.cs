using UnityEngine;

public class ProgressBar : MonoBehaviour
{
	public static ProgressBar Instance;

	public UILabel Label;

	public UIProgressBar UIProgressBar;

	public UISprite Fill;

	private Color defaultColor;

	private void Awake()
	{
		Instance = this;
		defaultColor = Fill.color;
		Hide();
	}

	public static void Show(float perc)
	{
		Show(perc, "", Instance.defaultColor);
	}

	public static void Show(float perc, string label)
	{
		Show(perc, label, Instance.defaultColor);
	}

	public static void Show(float perc, Color color)
	{
		Show(perc, "", color);
	}

	public static void Show(float perc, string label, Color color)
	{
		Instance.Label.text = label;
		Instance.Fill.color = color;
		Instance.UIProgressBar.Set(perc);
		Instance.UIProgressBar.gameObject.SetActive(value: true);
	}

	public static void Hide()
	{
		Instance.UIProgressBar.gameObject.SetActive(value: false);
		Instance.Fill.color = Instance.defaultColor;
		Instance.Label.text = "";
	}
}
