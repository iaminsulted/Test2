using UnityEngine;

public class UIPvpMatchStatLabel : MonoBehaviour
{
	private readonly Color highScoreColor = Color.yellow;

	[SerializeField]
	private UILabel label;

	private int stat;

	public int Stat
	{
		get
		{
			return stat;
		}
		set
		{
			stat = value;
			label.text = ArtixString.AddCommas(stat);
		}
	}

	public void MarkAsHighestStat()
	{
		label.gradientBottom = highScoreColor;
	}

	public void MarkAsNormal()
	{
		label.gradientBottom = Color.white;
	}
}
