using UnityEngine;

public class UIContextMenuItem : MonoBehaviour
{
	public UIContextMenu parent;

	public int Index;

	public UILabel Label;

	public void Init(int i, string s)
	{
		Index = i;
		Label.text = s;
	}

	private void OnClick()
	{
		if (parent != null)
		{
			parent.ContextSelect(Index);
		}
	}
}
