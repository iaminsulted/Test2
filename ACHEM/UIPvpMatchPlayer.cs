using UnityEngine;

public class UIPvpMatchPlayer : MonoBehaviour
{
	private readonly Color backgroundMe = new Color(1f, 0.7f, 0.3f, 0.6f);

	[SerializeField]
	private UILabel playerName;

	[SerializeField]
	private UISprite background;

	[SerializeField]
	private UISprite classIcon;

	private Color backgroundDefault;

	private bool resourcesLoaded;

	public void Init(string name, string icon, bool isMe)
	{
		if (!resourcesLoaded)
		{
			LoadResources();
		}
		playerName.text = name;
		if (string.IsNullOrEmpty(icon))
		{
			classIcon.gameObject.SetActive(value: false);
		}
		else
		{
			classIcon.gameObject.SetActive(value: true);
			classIcon.spriteName = icon;
		}
		background.color = (isMe ? backgroundMe : backgroundDefault);
	}

	private void LoadResources()
	{
		backgroundDefault = background.color;
		resourcesLoaded = true;
	}
}
