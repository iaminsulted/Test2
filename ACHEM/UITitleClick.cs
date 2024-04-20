using UnityEngine;

public class UITitleClick : MonoBehaviour
{
	public Badge badge;

	public UITitles UITitles;

	public GameObject Lock;

	public GameObject CheckMark;

	public UISprite BG;

	public UILabel Title;

	public UISprite IconNew;

	private void OnClick()
	{
		badge.isNew = false;
		ShowNotif();
		UITitles.SetTitle(this);
	}

	public void OnTooltip(bool show)
	{
		if (badge != null)
		{
			if (show)
			{
				Tooltip.ShowAtMousePosition(badge.ToolTipText, UIWidget.Pivot.BottomRight);
			}
			else
			{
				Tooltip.Hide();
			}
		}
	}

	public void ShowNotif()
	{
		if (IconNew != null)
		{
			IconNew.gameObject.SetActive(badge.isNew);
		}
	}
}
