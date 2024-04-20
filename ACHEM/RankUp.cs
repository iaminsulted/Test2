using UnityEngine;

public class RankUp : MonoBehaviour
{
	public Animator Anime;

	public ParticleSystem ParticleSystem;

	public GameObject ClassRankItemTemplate;

	public UILabel NameLabel;

	public UILabel DescriptionLabel;

	public UISprite SkillIcon;

	public UISprite IconFg;

	public static void Show()
	{
		Object.Instantiate(Resources.Load<GameObject>("UIElements/RankUp"), UIManager.Instance.transform);
	}

	private void Start()
	{
		CharClass currentClass = Session.MyPlayerData.CurrentClass;
		CombatClass combatClass = currentClass.ToCombatClass();
		ClassRankDetails classRankDetails = (combatClass.ClassRankDetails.ContainsKey(currentClass.ClassRank) ? combatClass.ClassRankDetails[currentClass.ClassRank] : new ClassRankDetails
		{
			ClassID = currentClass.ClassID,
			ClassRank = Session.MyPlayerData.GetClassRank(currentClass.ClassID),
			RewardID = combatClass.ClassTokenID,
			RewardType = 2
		});
		NameLabel.text = "Rank " + classRankDetails.ClassRank;
		SkillIcon.spriteName = classRankDetails.Icon;
		if (classRankDetails.RewardID != 0 && classRankDetails.RewardID == CombatClass.GetClassByID(classRankDetails.ClassID).ClassTokenID)
		{
			SkillIcon.spriteName = Items.Get(classRankDetails.RewardID).Icon;
			IconFg.spriteName = Items.Get(classRankDetails.RewardID).IconFg;
			IconFg.gameObject.SetActive(value: true);
			DescriptionLabel.text = Items.Get(classRankDetails.RewardID).Name + " x 25";
		}
		else
		{
			SkillIcon.spriteName = classRankDetails.Icon;
			DescriptionLabel.text = classRankDetails.GetDescription(usePastTense: true);
		}
	}

	private void Remove()
	{
		Object.Destroy(base.transform.parent.gameObject);
	}

	private void ActivateParticles()
	{
	}
}
