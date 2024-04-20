using UnityEngine;

public class MinimizeChat : MonoBehaviour
{
	public UIWidget BG;

	public Transform OptionBarArrowButton;

	private void Awake()
	{
		SettingsManager.ChatWindowUpdated += ToggleChat;
		ToggleChat(SettingsManager.ChatWindowSize);
	}

	public void OnDestroy()
	{
		SettingsManager.ChatWindowUpdated -= ToggleChat;
	}

	public void ToggleChatSizeBtn()
	{
		SettingsManager.ChatWindowSize.Set(((int)SettingsManager.ChatWindowSize + 1) % 3);
		ToggleChat(SettingsManager.ChatWindowSize);
	}

	public void ToggleChat(int size)
	{
		Quaternion localRotation = Quaternion.Euler(0f, 0f, 180f);
		switch (size)
		{
		case 0:
			BG.height = 22;
			if (UIGame.ControlScheme == ControlScheme.HANDHELD)
			{
				OptionBarArrowButton.localRotation = Quaternion.identity;
			}
			else
			{
				OptionBarArrowButton.localRotation = localRotation;
			}
			break;
		case 1:
			BG.height = 182;
			if (UIGame.ControlScheme == ControlScheme.HANDHELD)
			{
				OptionBarArrowButton.localRotation = Quaternion.identity;
			}
			else
			{
				OptionBarArrowButton.localRotation = localRotation;
			}
			break;
		case 2:
			BG.height = 382;
			if (UIGame.ControlScheme == ControlScheme.HANDHELD)
			{
				OptionBarArrowButton.localRotation = localRotation;
			}
			else
			{
				OptionBarArrowButton.localRotation = Quaternion.identity;
			}
			break;
		default:
			BG.height = 182;
			break;
		}
		BroadcastMessage("UpdateAnchors", SendMessageOptions.DontRequireReceiver);
	}
}
