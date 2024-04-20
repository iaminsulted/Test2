using UnityEngine;
using UnityEngine.Serialization;

public class UIClassRankItem : MonoBehaviour
{
	private static readonly Color LockedBackgroundColor = new Color32(0, 0, 0, 100);

	private static readonly Color UnlockedBackgroundColor = Color.clear;

	private static readonly Color LockedRankColor = new Color32(byte.MaxValue, 195, 0, byte.MaxValue);

	private static readonly Color UnlockedRankColor = Color.black;

	private static readonly Color LockedDescriptionColor = Color.white;

	private static readonly Color UnlockedDescriptionColor = new Color32(0, 0, 0, 200);

	[FormerlySerializedAs("labelName")]
	public UILabel NameLabel;

	[FormerlySerializedAs("labelDescription")]
	public UILabel DescriptionLabel;

	[FormerlySerializedAs("sprite")]
	public UISprite SkillIcon;

	[FormerlySerializedAs("sprite")]
	public UISprite IconFg;

	public UISprite LockIcon;

	public UISprite BackgroundImage;

	public GameObject Highlight;

	public void Init(ClassRankDetails rankDetails)
	{
		NameLabel.text = "Rank " + rankDetails.ClassRank;
		SkillIcon.spriteName = rankDetails.Icon;
		SkillIcon.spriteName = rankDetails.Icon;
		DescriptionLabel.text = rankDetails.GetDescription();
		bool flag = Session.MyPlayerData.GetClassRank(rankDetails.ClassID) >= rankDetails.ClassRank;
		LockIcon.gameObject.SetActive(!flag);
		Highlight.SetActive(rankDetails.ClassRank == 5 || rankDetails.ClassRank == 10);
		if (flag)
		{
			NameLabel.color = UnlockedRankColor;
			DescriptionLabel.color = UnlockedDescriptionColor;
			BackgroundImage.color = UnlockedBackgroundColor;
			SkillIcon.color = Color.white;
		}
		else
		{
			NameLabel.color = LockedRankColor;
			DescriptionLabel.color = LockedDescriptionColor;
			BackgroundImage.color = LockedBackgroundColor;
			SkillIcon.color = new Color(0.3f, 0.3f, 0.3f);
		}
	}
}
