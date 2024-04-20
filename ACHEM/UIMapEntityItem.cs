using Newtonsoft.Json.Linq;
using UnityEngine;

public class UIMapEntityItem : MonoBehaviour
{
	public UILabel title;

	public UILabel description;

	public GameObject DeleteBtn;

	public GameObject Highlight;

	public ComMapEntity entity;

	public string Type;

	public int MachineID;

	public void DeleteEntity()
	{
		Confirmation.Show("Temporary Map Editor", "Are you sure you want to delete Map Entity ID#" + entity.ID + "?", delegate(bool b)
		{
			if (b)
			{
				AEC.getInstance().sendRequest(new RequestDeleteMapEntity(entity.ID));
			}
		});
	}

	public void Load(ComMapEntity entity)
	{
		this.entity = entity;
		JObject jObject = null;
		if (entity.Data != null && entity.Data.Length > 0)
		{
			jObject = JObject.Parse(entity.Data);
		}
		switch (entity.Type)
		{
		case MapEntityTypes.PlayerSpawner:
			title.text = "Spawn ID# " + (string?)jObject["SpawnID"];
			description.text = "";
			break;
		case MapEntityTypes.TransferPad:
			title.text = "Asset Name:  " + (string?)jObject["AssetName"];
			description.text = "ID# " + entity.ID;
			break;
		case MapEntityTypes.Machine:
		{
			int num = 1;
			jObject.TryGetValue("MachineID", out JToken value);
			if (value != null)
			{
				num = (int)value;
			}
			if (UIMapEditor.nextMachineId <= num)
			{
				UIMapEditor.nextMachineId = num + 1;
			}
			MachineID = num;
			string text = "Machine";
			jObject.TryGetValue("MachineName", out JToken value2);
			if (value2 != null)
			{
				text = value2.ToString();
			}
			title.text = text + ":  " + num;
			description.text = "ID# " + num;
			break;
		}
		default:
			title.text = "Unknown Type";
			description.text = "";
			break;
		}
	}

	private void OnClick()
	{
		UIMapEditor.Instance.OnMapEntityClicked(this);
	}

	public void OnTooltip(bool show)
	{
		if (show)
		{
			JObject jObject = JObject.Parse(entity.Data);
			string tooltipText = "";
			switch (entity.Type)
			{
			case MapEntityTypes.PlayerSpawner:
				tooltipText = "[000000]Spawn ID# " + (string?)jObject["SpawnID"];
				break;
			case MapEntityTypes.TransferPad:
				tooltipText = "[000000]Asset Bundle: " + (string?)jObject["AssetBundle"] + "\nAsset Name: " + (string?)jObject["AssetName"] + "\nUnique ID: " + (string?)jObject["UniqueID"] + "\nSpawn ID: " + (string?)jObject["SpawnID"] + "\nAreaID: " + (string?)jObject["AreaID"] + "\nCellID: " + (string?)jObject["CellID"] + "\nShowConfirmation: " + (string?)jObject["ShowConfirmation"];
				break;
			case MapEntityTypes.Machine:
				if (MachineID > 0)
				{
					tooltipText = "[000000]Machine ID# " + MachineID;
				}
				break;
			default:
				tooltipText = "Unknown Type";
				break;
			}
			Tooltip.ShowAtPosition(tooltipText, UIGame.Instance.MouseToolTipPosition, UIWidget.Pivot.TopRight);
		}
		else
		{
			Tooltip.Hide();
		}
	}
}
