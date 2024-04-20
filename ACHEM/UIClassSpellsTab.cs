using UnityEngine;
using UnityEngine.Serialization;

public class UIClassSpellsTab : MonoBehaviour, IRefreshable
{
	public UIScrollView ScrollView;

	public UITable AllSpellsTable;

	[FormerlySerializedAs("tblSpells")]
	public UITable SpellsTable;

	public PassiveSkillDetails ResourceDetails;

	public PassiveSkillDetails PassiveSkillDetails;

	public UIClassSkill CrossSkillDetails;

	public UIClassSkill UltimateSkillDetails;

	public GameObject UIClassSkillPrefab;

	public Transform UIClassSkillsContainer;

	private CharClass charClass;

	private CombatClass combatClass;

	public int ClassID => charClass.ClassID;

	public void Refresh(CharClass charClass)
	{
		this.charClass = charClass;
		combatClass = charClass.ToCombatClass();
		PassiveSkillDetails.Init(combatClass.PassiveEffectID);
		int i = 1;
		for (int childCount = UIClassSkillsContainer.childCount; i < childCount; i++)
		{
			UIClassSkillsContainer.GetChild(i).gameObject.SetActive(value: false);
			Object.Destroy(UIClassSkillsContainer.GetChild(i).gameObject);
		}
		int classRank = Session.MyPlayerData.GetClassRank(charClass.ClassID);
		for (int j = 0; j < combatClass.Spells.Count; j++)
		{
			SpellTemplate spellTemplate = SpellTemplates.Get(combatClass.Spells[j], null, classRank, charClass.ClassID, 0);
			if (spellTemplate.isUlt)
			{
				UltimateSkillDetails.Init(spellTemplate);
				continue;
			}
			if (combatClass.CrossSkillID == combatClass.Spells[j])
			{
				CrossSkillDetails.Init(spellTemplate);
				continue;
			}
			GameObject obj = Object.Instantiate(UIClassSkillPrefab, UIClassSkillsContainer);
			UIClassSkillPrefab.SetActive(value: false);
			obj.SetActive(value: true);
			obj.GetComponent<UIClassSkill>().Init(spellTemplate);
		}
		SpellsTable.Reposition();
		AllSpellsTable.Reposition();
		ScrollView.ResetPosition();
	}

	public void Refresh()
	{
		Refresh(charClass);
	}
}
