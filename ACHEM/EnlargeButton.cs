using UnityEngine;

public class EnlargeButton : MonoBehaviour
{
	public UISprite ThisButton;

	public UILabel InfoToDisplay;

	public GameObject ObjectToDisplay;

	public bool opened;

	public UIGrid grid;

	public UILabel Value;

	public int ClosedHeight = 40;

	public int OpenedHeight = 60;

	public void ButtonClick()
	{
		opened = !opened;
		if (opened)
		{
			ObjectToDisplay.SetActive(value: true);
			ThisButton.height = OpenedHeight;
		}
		else
		{
			ObjectToDisplay.SetActive(value: false);
			ThisButton.height = ClosedHeight;
		}
		grid.Reposition();
	}
}
