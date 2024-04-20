using UnityEngine;

public class UITimeButton : MonoBehaviour
{
	public UITimePanel timePanel;

	public UISprite timeRadial;

	public void OnClick()
	{
		ShowPanel();
		HideButton();
	}

	public void OnHover(bool isOver)
	{
		GetComponent<UIWidget>().alpha = (isOver ? 1f : 0.6f);
	}

	public void ShowButton()
	{
		base.gameObject.SetActive(value: true);
	}

	public void HideButton()
	{
		base.gameObject.SetActive(value: false);
	}

	private void ShowPanel()
	{
		timePanel.TurnOn();
	}
}
