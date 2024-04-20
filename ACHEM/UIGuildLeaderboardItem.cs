using UnityEngine;

public class UIGuildLeaderboardItem : UIItem
{
	public UILabel ItemNameLabel;

	public UILabel XPLabel;

	public UILabel RankLabel;

	public GameObject firstIcon;

	public GameObject secondIcon;

	public GameObject thirdIcon;

	public GameObject dotIcon;

	public void Init(GuildLeaderboardEntry guildLeaderboardEntry, int rank)
	{
		ItemNameLabel.text = guildLeaderboardEntry.Name;
		XPLabel.text = "XP: " + guildLeaderboardEntry.XP.ToString("n0");
		if (RankLabel != null)
		{
			RankLabel.gameObject.SetActive(value: false);
			firstIcon.SetActive(value: false);
			secondIcon.SetActive(value: false);
			thirdIcon.SetActive(value: false);
		}
		switch (rank)
		{
		case 0:
			dotIcon.SetActive(value: true);
			break;
		case 1:
			firstIcon.SetActive(value: true);
			break;
		case 2:
			secondIcon.SetActive(value: true);
			break;
		case 3:
			thirdIcon.SetActive(value: true);
			break;
		default:
			RankLabel.gameObject.SetActive(value: true);
			RankLabel.text = rank.ToString();
			break;
		}
	}
}
