using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class MapEntityDropDown : MonoBehaviour
{
	public UIGrid Grid;

	public List<GameObject> Contents = new List<GameObject>();

	public bool isOpen = true;

	public GameObject PlusIcon;

	public GameObject MinusIcon;

	public GameObject EntityPrefab;

	public GameObject Title;

	public string Type;

	public void OnButtonClick()
	{
		isOpen = !isOpen;
		foreach (GameObject content in Contents)
		{
			content.SetActive(isOpen);
		}
		UpdateIcons();
		Grid.Reposition();
	}

	public void Clear()
	{
		foreach (GameObject content in Contents)
		{
			content.SetActive(value: false);
			Object.Destroy(content.gameObject);
		}
		Contents.Clear();
		Grid.Reposition();
	}

	private void UpdateIcons()
	{
		PlusIcon.gameObject.SetActive(!isOpen);
		MinusIcon.gameObject.SetActive(isOpen);
	}

	private bool DoesNPCContainText(ComMapEntity entity, string text)
	{
		text = text.ToLower();
		string text2 = "";
		string text3 = "";
		if (!string.IsNullOrEmpty(entity.Data.Trim()))
		{
			dynamic val = JsonConvert.DeserializeObject<object>(entity.Data);
			if (val["MachineName"] != null)
			{
				text2 = ((string)val["MachineName"]).ToLower();
			}
			if (val["MachineID"] != null)
			{
				text3 = ((int)val["MachineID"]).ToString().ToLower();
			}
		}
		if (!entity.MapID.ToString().Contains(text) && !entity.CellID.ToString().Contains(text) && !text2.Contains(text))
		{
			return text3.Contains(text);
		}
		return true;
	}

	public void BuildList(List<ComMapEntity> entities, string searchText, ComMapEntity selectedEntity = null)
	{
		if (entities != null)
		{
			if (!string.IsNullOrEmpty(searchText))
			{
				entities = entities.Where((ComMapEntity x) => DoesNPCContainText(x, searchText)).ToList();
			}
			foreach (ComMapEntity entity in entities)
			{
				GameObject gameObject = Object.Instantiate(EntityPrefab, base.gameObject.transform.parent);
				gameObject.SetActive(value: true);
				gameObject.transform.SetSiblingIndex(base.transform.GetSiblingIndex() + 1);
				UIMapEntityItem component = gameObject.GetComponent<UIMapEntityItem>();
				component.Load(entity);
				Contents.Add(gameObject);
				UIMapEditor.Instance.checkIfEntityWasSelected(component);
				if (selectedEntity != null && selectedEntity == entity)
				{
					component.Highlight.SetActive(value: true);
				}
			}
		}
		Title.GetComponent<UILabel>().text = Type;
		Grid.Reposition();
	}
}
