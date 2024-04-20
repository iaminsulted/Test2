using System.Collections.Generic;
using UnityEngine;

public class UIMapEditorExistingActionItem : MonoBehaviour
{
	public UILabel LabelTitle;

	public string ActionClassName;

	public List<UINPCEditItem> PropertyEditItems = new List<UINPCEditItem>();

	public int index;

	public void DeleteEntry()
	{
		UIMapEditor.Instance.DeleteActionEntry(ActionClassName, index);
	}

	public void SaveEntry()
	{
		UIMapEditor.Instance.UpdateActionEntry(ActionClassName, PropertyEditItems, index);
	}
}
