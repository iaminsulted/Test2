using System.Linq;
using UnityEngine;

public class UINPCItem : MonoBehaviour
{
	public UILabel title;

	public UILabel description;

	public GameObject DeleteBtn;

	public GameObject Highlight;

	public GameObject ApopBtn;

	public ComNPCMeta npc;

	public int SpawnID;

	public bool IsDB;

	public bool reqReload;

	public void TeleportToNPC()
	{
		ComSpawnMeta comSpawnMeta = Game.Instance.AreaData.spawnMetas.Where((ComSpawnMeta x) => x.SpawnID == SpawnID).FirstOrDefault();
		AEC.getInstance().sendRequest(new RequestTeleport(comSpawnMeta.Path.First().Value.x, comSpawnMeta.Path.First().Value.y, comSpawnMeta.Path.First().Value.z, comSpawnMeta.RotationY.First().Value, GameTime.realtimeSinceServerStartup));
		Player me = Entities.Instance.me;
		me.position = new Vector3(comSpawnMeta.Path.First().Value.x, comSpawnMeta.Path.First().Value.y, comSpawnMeta.Path.First().Value.z);
		me.rotation = Quaternion.Euler(0f, comSpawnMeta.RotationY.First().Value, 0f);
		me.wrapper.transform.SetPositionAndRotation(me.position, me.rotation);
	}

	public void OpenApop()
	{
		AEC.getInstance().sendRequest(new RequestOpenApopAdmin(npc.ApopIDs));
	}

	public void DeleteNPC()
	{
		Confirmation.Show("Spawn Editor", "Are you sure you want to delete NPC ID#" + npc.NPCID + " from SpawnID#" + SpawnID + "?", delegate(bool b)
		{
			if (b)
			{
				AEC.getInstance().sendRequest(new RequestDeleteNPC(npc.SpawnListNpcID, SpawnID));
			}
		});
	}

	public void Load(int SpawnID, ComNPCMeta npc, bool IsDB = false, bool reqReload = false)
	{
		this.SpawnID = SpawnID;
		this.npc = npc;
		this.IsDB = IsDB;
		this.reqReload = reqReload;
		if (reqReload)
		{
			title.color = Color.red;
		}
		title.text = npc.NameOverride;
		description.text = "NpcID " + npc.NPCID + ", Rate: " + npc.Rate + "%";
		_ = UINPCEditor.Instance.recentSpawnID;
		_ = UINPCEditor.Instance.recentNpcID;
		if (UINPCEditor.Instance.recentSpawnID == SpawnID && UINPCEditor.Instance.recentNpcID == npc.NPCID)
		{
			Highlight.SetActive(value: true);
			return;
		}
		Highlight.SetActive(value: false);
		ApopBtn.SetActive(npc.ApopIDs != null && npc.ApopIDs.Count > 0);
	}

	private void OnClick()
	{
		Highlight.SetActive(value: true);
		UINPCEditor.Instance.OnNpcClicked(this);
	}

	private void OnDoubleClick()
	{
		UINPCEditor.Instance.TeleToSpawner();
	}

	public void OnTooltip(bool show)
	{
	}
}
