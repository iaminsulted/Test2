using System;
using UnityEngine;

public class UIItemServerInfo : MonoBehaviour
{
	private static Color32 ColorOnline = new Color32(0, 160, 0, byte.MaxValue);

	private static Color32 ColorOffline = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

	private static Color32 ColorFull = new Color32(160, 136, 0, byte.MaxValue);

	public ServerInfo server;

	public UILabel Name;

	public UILabel State;

	public UIButtonColor Button;

	public event Action<UIItemServerInfo> Clicked;

	public void Init(ServerInfo server)
	{
		this.server = server;
		Name.text = server.DisplayName;
		if (server.State)
		{
			if (server.UserCount < server.MaxUsers)
			{
				State.text = "Online";
				State.color = ColorOnline;
				Button.isEnabled = true;
			}
			else
			{
				State.text = "Full";
				State.color = ColorFull;
				Button.isEnabled = false;
			}
		}
		else
		{
			State.text = "Offline";
			State.color = ColorOffline;
			Button.isEnabled = false;
		}
	}

	private void OnClick()
	{
		if (server.State && server.UserCount < server.MaxUsers)
		{
			OnClicked();
		}
	}

	protected void OnClicked()
	{
		if (this.Clicked != null)
		{
			this.Clicked(this);
		}
	}
}
