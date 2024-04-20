using UnityEngine;

public class CapstoneFill : MonoBehaviour
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
		Object.Instantiate(Resources.Load<GameObject>("UIElements/CapstoneFill"), UIManager.Instance.transform);
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
		SkillIcon.spriteName = classRankDetails.Icon;
		if (classRankDetails.RewardID == CombatClass.GetClassByID(classRankDetails.ClassID).ClassTokenID)
		{
			SkillIcon.spriteName = Items.Get(classRankDetails.RewardID).Icon;
			IconFg.spriteName = Items.Get(classRankDetails.RewardID).IconFg;
			IconFg.gameObject.SetActive(value: true);
		}
		else
		{
			SkillIcon.spriteName = "icon_menu_capstone_nulgath";
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
