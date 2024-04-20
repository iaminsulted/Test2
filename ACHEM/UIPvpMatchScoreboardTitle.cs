using UnityEngine;

public class UIPvpMatchScoreboardTitle : MonoBehaviour
{
	private const string Victory = "Victory";

	private const string Defeat = "Defeat";

	[SerializeField]
	private UILabel title;

	[SerializeField]
	private UILabel shadow;

	public void Init(bool isWinner)
	{
		if (isWinner)
		{
			MarkAsVictory();
		}
		else
		{
			MarkAsDefeat();
		}
	}

	private void MarkAsVictory()
	{
		title.text = "Victory";
		shadow.text = "Victory";
	}

	private void MarkAsDefeat()
	{
		title.text = "Defeat";
		shadow.text = "Defeat";
	}
}
