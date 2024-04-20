using UnityEngine;

public class UnlockReqUI : MonoBehaviour
{
	public UILabel UILabel;

	public UISprite ChekForCheckbox;

	public void UpdateLabel(string newText)
	{
		UILabel.text = newText;
	}
}
