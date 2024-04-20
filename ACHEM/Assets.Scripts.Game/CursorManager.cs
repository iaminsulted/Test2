using UnityEngine;

namespace Assets.Scripts.Game;

public class CursorManager : MonoBehaviour
{
	public enum Icon
	{
		Default,
		Combat,
		Talk,
		Interact
	}

	[SerializeField]
	private Texture2D defaultCursor;

	[SerializeField]
	private Texture2D combatCursor;

	[SerializeField]
	private Texture2D talkCursor;

	[SerializeField]
	private Texture2D interactCursor;

	private static CursorManager instance;

	public static CursorManager Instance => instance;

	private void Awake()
	{
		instance = this;
		SetCursor(Icon.Default);
	}

	private void OnDestroy()
	{
		SetCursor(Icon.Default);
	}

	public void SetCursor(Icon icon)
	{
		switch (icon)
		{
		default:
			Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
			break;
		case Icon.Combat:
			Cursor.SetCursor(combatCursor, Vector2.zero, CursorMode.Auto);
			break;
		case Icon.Talk:
			Cursor.SetCursor(talkCursor, Vector2.zero, CursorMode.Auto);
			break;
		case Icon.Interact:
			Cursor.SetCursor(interactCursor, Vector2.zero, CursorMode.Auto);
			break;
		}
	}
}
