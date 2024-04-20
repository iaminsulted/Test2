using UnityEngine;

public class QuestListItem : MonoBehaviour
{
	public Quest quest;

	public UIEventListener.VoidDelegate onClick;

	private void OnClick()
	{
		if (onClick != null)
		{
			onClick(base.gameObject);
		}
	}
}
