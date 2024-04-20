using UnityEngine;
using UnityEngine.Serialization;

public class UIInventoryClass : UIItem
{
	public Color32 ColorOwned;

	public UILabel SkinLabel;

	[FormerlySerializedAs("Check")]
	[FormerlySerializedAs("combatClass")]
	public CombatClass combatClass;

	[FormerlySerializedAs("Lock")]
	public UISprite LockIcon;

	public UISprite ClassXPIcon;

	private Color32 ColorMaxRank = new Color32(byte.MaxValue, 201, 78, byte.MaxValue);

	private void OnEnable()
	{
		Session.MyPlayerData.ClassEquipped += OnEquipped;
		Session.MyPlayerData.ClassAdded += OnClassAdded;
	}

	private void OnDisable()
	{
		if (Session.MyPlayerData != null)
		{
			Session.MyPlayerData.ClassEquipped -= OnEquipped;
			Session.MyPlayerData.ClassAdded -= OnClassAdded;
		}
	}

	public void Init(CombatClass combatClass)
	{
		this.combatClass = combatClass;
		Refresh();
	}

	public void Refresh()
	{
		CheckIcon.gameObject.SetActive(combatClass.Equipped);
		Icon.gameObject.SetActive(!combatClass.Equipped);
		SkinLabel.gameObject.SetActive(combatClass.IsSkin);
		if (combatClass.IsSkin)
		{
			CombatClass classByID = CombatClass.GetClassByID(combatClass.SkinClassID);
			SkinLabel.text = classByID.Name + " Skin";
			Vector3 localPosition = NameLabel.transform.localPosition;
			localPosition.y = -19f;
			NameLabel.transform.localPosition = localPosition;
			InfoLabel.text = "-";
		}
		else
		{
			InfoLabel.text = Session.MyPlayerData.GetClassRank(combatClass.ID).ToString();
		}
		NameLabel.text = combatClass.Name;
		Icon.spriteName = combatClass.Icon;
		bool flag = Session.MyPlayerData.OwnsClass(combatClass.ID);
		if (flag)
		{
			NameLabel.color = ColorOwned;
			SkinLabel.color = ColorOwned;
			Icon.color = ColorOwned;
			InfoLabel.color = ((combatClass.IsSkin || combatClass.ToCharClass().ClassRank == 100) ? ColorMaxRank : ((Color32)Color.white));
			ClassXPIcon.color = ((combatClass.IsSkin || combatClass.ToCharClass().ClassRank == 100) ? ColorMaxRank : ((Color32)Color.white));
		}
		else
		{
			NameLabel.color = Color.gray;
			SkinLabel.color = Color.gray;
			Icon.color = Color.gray;
			InfoLabel.color = Color.gray;
			ClassXPIcon.color = Color.gray;
		}
		LockIcon.gameObject.SetActive(!flag && !Session.MyPlayerData.IsClassAvailable(combatClass));
	}

	public void OnEquipped(int id)
	{
		CheckIcon.gameObject.SetActive(combatClass.Equipped);
	}

	private void OnClassAdded(int ClassID)
	{
		Refresh();
	}
}
