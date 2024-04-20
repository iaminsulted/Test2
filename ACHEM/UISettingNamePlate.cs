using UnityEngine;

public class UISettingNamePlate : MonoBehaviour
{
	public UIToggle[] NamePlates;

	private int savedPlate;

	private int currentPlate;

	private void Start()
	{
		if (!Session.MyPlayerData.CheckBitFlag("iu0", 2))
		{
			NamePlates[3].gameObject.SetActive(value: false);
		}
		if (!Session.MyPlayerData.CheckBitFlag("iu0", 8))
		{
			NamePlates[4].gameObject.SetActive(value: false);
		}
		savedPlate = (currentPlate = Entities.Instance.me.Portrait);
		NamePlates[currentPlate - 1].value = true;
	}

	public void OnButtonClick(int plateNumber)
	{
		switch (plateNumber)
		{
		case 1:
			currentPlate = plateNumber;
			break;
		case 2:
			if (Session.MyPlayerData.CheckBitFlag("iu0", 6))
			{
				currentPlate = plateNumber;
				break;
			}
			Confirmation.Show("Guardian Only", "You need to be a Guardian to use this Portrait, would you like to become a Guardian?", delegate(bool b)
			{
				if (b)
				{
					UIIAPStore.Show();
				}
			});
			break;
		case 3:
			if (Session.MyPlayerData.CheckBitFlag("iu0", 16))
			{
				currentPlate = plateNumber;
				break;
			}
			Confirmation.Show("Dragon Guardian Only", "You need to be a Dragon Guardian to use this portrait, would you like to become a Dragon Guardian?", delegate(bool b)
			{
				if (b)
				{
					UIIAPStore.Show();
				}
			});
			break;
		case 4:
			if (Session.MyPlayerData.CheckBitFlag("iu0", 2))
			{
				currentPlate = plateNumber;
			}
			break;
		case 5:
			if (Session.MyPlayerData.CheckBitFlag("iu0", 8))
			{
				currentPlate = plateNumber;
			}
			break;
		}
		NamePlates[currentPlate - 1].value = true;
	}

	public void OnSaveClick()
	{
		if (savedPlate != currentPlate)
		{
			savedPlate = currentPlate;
			Game.Instance.SendEntityPortraitUpdateRequest(currentPlate);
		}
	}
}
