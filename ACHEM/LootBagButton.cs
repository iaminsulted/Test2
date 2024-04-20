using UnityEngine;

public class LootBagButton : MonoBehaviour
{
	public UILabel HotKey;

	protected void Awake()
	{
		SettingsManager.KeyMappingUpdated += UpdateHotkeyDisplay;
		UpdateHotkeyDisplay();
	}

	protected void OnDestroy()
	{
		SettingsManager.KeyMappingUpdated -= UpdateHotkeyDisplay;
	}

	private void UpdateHotkeyDisplay()
	{
		if (!(HotKey == null))
		{
			if (SettingsManager.GetKeyCodeByAction(InputAction.OpenLoot) != 0)
			{
				HotKey.text = SettingsManager.GetHotkeyByAction(InputAction.OpenLoot);
			}
			else
			{
				HotKey.text = "";
			}
		}
	}
}
