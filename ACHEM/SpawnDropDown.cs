using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnDropDown : MonoBehaviour
{
	public UIGrid Grid;

	public List<GameObject> Contents = new List<GameObject>();

	public bool isOpen = true;

	public GameObject PlusIcon;

	public GameObject MinusIcon;

	public GameObject NPCPrefab;

	public GameObject DeleteBtn;

	public GameObject Title;

	public GameObject Subtitle;

	public int SpawnID;

	public bool IsDB;

	public bool reqReload;

	public void RemoveSpawn()
	{
		if (!IsDB)
		{
			return;
		}
		string message = "Are you sure you want to Remove SpawnID#" + SpawnID + " AND delete all of its npcs?";
		if (IsDB && NPCSpawn.InactiveMap.ContainsKey(SpawnID))
		{
			message = "Are you sure you want to Convert SpawnID#" + SpawnID + " back to Map?";
		}
		Confirmation.Show("Spawn Editor", message, delegate(bool b)
		{
			if (b)
			{
				AEC.getInstance().sendRequest(new RequestDeleteSpawn(SpawnID));
			}
		});
	}

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

	private bool DoesNPCContainText(ComNPCMeta npc, string text)
	{
		text = text.ToLower();
		if (text == "db" && IsDB)
		{
			return true;
		}
		if (npc == null)
		{
			return false;
		}
		if (npc.NameOverride.ToLower().Contains(text) || SpawnID.ToString().Contains(text) || npc.NPCID.ToString().Contains(text))
		{
			return true;
		}
		return false;
	}

	public void BuildList(List<ComNPCMeta> npcs, string searchText)
	{
		DeleteBtn.SetActive(IsDB);
		if (npcs != null)
		{
			if (!string.IsNullOrEmpty(searchText))
			{
				npcs = npcs.Where((ComNPCMeta npc) => DoesNPCContainText(npc, searchText)).ToList();
			}
			foreach (ComNPCMeta npc in npcs)
			{
				GameObject gameObject = Object.Instantiate(NPCPrefab, base.gameObject.transform.parent);
				gameObject.SetActive(value: true);
				gameObject.transform.SetSiblingIndex(base.transform.GetSiblingIndex() + 1);
				gameObject.GetComponent<UINPCItem>().Load(SpawnID, npc, IsDB);
				Contents.Add(gameObject);
			}
		}
		if (reqReload)
		{
			Title.GetComponent<UILabel>().color = Color.red;
		}
		Title.GetComponent<UILabel>().text = "ID# " + SpawnID + (IsDB ? " (DB)" : "");
		Grid.Reposition();
	}
}
