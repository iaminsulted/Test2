using System.Linq;
using UnityEngine;

public class UIChatCommandItem : MonoBehaviour
{
	public UILabel title;

	public UILabel description;

	private ChatCommand chatCommand;

	public void Load(ChatCommand chatCommand)
	{
		this.chatCommand = chatCommand;
		title.text = "/" + chatCommand.command;
		foreach (string alias in chatCommand.aliases)
		{
			UILabel uILabel = title;
			uILabel.text = uILabel.text + ", /" + alias;
		}
		description.text = chatCommand.description;
	}

	private void OnClick()
	{
		Chat.Instance.chatInput.mInput.value = "/" + chatCommand.command;
		if (chatCommand.args.Any((ChatParam arg) => !arg.isOptional))
		{
			Chat.Instance.chatInput.mInput.value += " ";
		}
		Chat.Instance.Focus();
	}

	public void OnTooltip(bool show)
	{
		if (chatCommand != null)
		{
			if (show)
			{
				Tooltip.ShowAtPosition("[000000]" + chatCommand.HelpMessage, UIGame.Instance.MouseToolTipPosition, UIWidget.Pivot.TopRight);
			}
			else
			{
				Tooltip.Hide();
			}
		}
	}
}
