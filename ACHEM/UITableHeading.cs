using UnityEngine;

public class UITableHeading : MonoBehaviour
{
	public UILabel Title;

	public void Init(string title = "Movement")
	{
		Title.text = title;
	}
}
