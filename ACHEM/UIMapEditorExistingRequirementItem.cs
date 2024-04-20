using System.Collections.Generic;
using UnityEngine;

public class UIMapEditorExistingRequirementItem : MonoBehaviour
{
	public UILabel LabelTitle;

	public string RequirementClassName;

	public List<UINPCEditItem> PropertyEditItems = new List<UINPCEditItem>();

	public int index;

	public void DeleteEntry()
	{
		UIMapEditor.Instance.DeleteRequirementEntry(RequirementClassName, index);
	}

	public void SaveEntry()
	{
		UIMapEditor.Instance.UpdateRequirementEntry(RequirementClassName, PropertyEditItems, index);
	}
}
