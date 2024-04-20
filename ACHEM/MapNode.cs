using UnityEngine;

public class MapNode : MonoBehaviour
{
	public enum NodeType
	{
		QuestNPC,
		Dungeon,
		Teleport,
		Location
	}

	public string BitFlagName;

	public byte BitFlagIndex;

	public NodeType SetNodeType;

	public GameObject TeleportToRegion;

	[HideInInspector]
	public string choice;

	public int CellID;

	public int SpawnID;

	private bool locked;

	public string description;

	private void Start()
	{
	}

	private void OnEnable()
	{
		UISprite component = GetComponent<UISprite>();
		if (!(component != null))
		{
			return;
		}
		if (Session.MyPlayerData.CheckBitFlag(BitFlagName, BitFlagIndex))
		{
			switch (SetNodeType)
			{
			case NodeType.QuestNPC:
				component.spriteName = "Icon-Quest";
				break;
			case NodeType.Dungeon:
				component.spriteName = "DungeonMapIcon";
				break;
			case NodeType.Teleport:
				component.spriteName = "ArrowMapIcon";
				break;
			case NodeType.Location:
				component.spriteName = "NodeMapIcon";
				break;
			}
		}
		else if (SetNodeType == NodeType.Teleport)
		{
			component.spriteName = "ArrowMapIcon";
		}
		else
		{
			component.spriteName = "UndiscoveredMapIcon";
			locked = true;
		}
	}

	public void OnClick()
	{
		if (!locked)
		{
			MapController component = base.transform.parent.parent.GetComponent<MapController>();
			if (SetNodeType == NodeType.Teleport)
			{
				component.LoadRegion(TeleportToRegion);
				base.transform.parent.gameObject.SetActive(value: false);
				Object.Destroy(base.transform.parent.gameObject);
			}
			else
			{
				component.GoToNode(base.name, description, choice, CellID, SpawnID);
			}
		}
		else
		{
			Notification.ShowText("You must discover this location");
		}
	}

	private void Update()
	{
	}
}
