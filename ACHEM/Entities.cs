using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entities
{
	private Dictionary<int, NPC> npcMap = new Dictionary<int, NPC>();

	private Dictionary<int, Player> playerMap = new Dictionary<int, Player>();

	public List<NPC> NpcList = new List<NPC>();

	public List<Player> PlayerList = new List<Player>();

	private static Entities instance;

	public IEnumerable<Entity> AllEntities => NpcList.Cast<Entity>().Union(PlayerList);

	public Player me { get; private set; }

	public static Entities Instance => instance ?? (instance = new Entities());

	public event Action EntityListUpdated;

	public event Action<Player> MeInitialized;

	protected void OnEntityListUpdated()
	{
		this.EntityListUpdated?.Invoke();
	}

	public void Clear()
	{
		ClearNpcs();
		ClearPlayers();
		me = null;
		OnEntityListUpdated();
	}

	private void ClearNpcs()
	{
		foreach (NPC value in npcMap.Values)
		{
			value.Dispose();
		}
		npcMap.Clear();
		NpcList.Clear();
	}

	private void ClearPlayers()
	{
		foreach (Player value in playerMap.Values)
		{
			value.Dispose();
		}
		playerMap.Clear();
		PlayerList.Clear();
	}

	public void ClearEntitiesExceptMe()
	{
		ClearNpcs();
		foreach (Player item in PlayerList.Where((Player player) => player != me))
		{
			item.Dispose();
		}
		playerMap.Clear();
		PlayerList.Clear();
		if (me != null)
		{
			AddPlayer(me);
		}
		OnEntityListUpdated();
	}

	public void FilterByCell(int cellID)
	{
		Dictionary<int, NPC> dictionary = new Dictionary<int, NPC>();
		foreach (KeyValuePair<int, NPC> item in npcMap)
		{
			NPC value = item.Value;
			if (value.cellID != cellID)
			{
				Debug.Log("Entities.FilterByCell() trying to close " + value.name + ", " + value.cellID + "/" + cellID);
				value.Dispose();
			}
			else
			{
				dictionary.Add(item.Key, item.Value);
			}
		}
		npcMap = dictionary;
		NpcList = npcMap.Values.ToList();
		Dictionary<int, Player> dictionary2 = new Dictionary<int, Player>();
		foreach (KeyValuePair<int, Player> item2 in playerMap)
		{
			Player value2 = item2.Value;
			if (value2.cellID != cellID)
			{
				Debug.Log("Entities.FilterByCell() trying to close " + value2.name + ", " + value2.cellID + "/" + cellID);
				value2.Dispose();
			}
			else
			{
				dictionary2.Add(item2.Key, item2.Value);
			}
		}
		playerMap = dictionary2;
		PlayerList = playerMap.Values.ToList();
		OnEntityListUpdated();
	}

	public void AddNpc(NPC npc)
	{
		if (npcMap.ContainsKey(npc.ID))
		{
			npcMap[npc.ID] = npc;
			for (int i = 0; i < NpcList.Count; i++)
			{
				if (NpcList[i].ID == npc.ID)
				{
					NpcList[i] = npc;
				}
			}
		}
		else
		{
			npcMap.Add(npc.ID, npc);
			NpcList.Add(npc);
		}
		OnEntityListUpdated();
	}

	public void AddPlayer(Player player, bool isMe = false)
	{
		if (isMe)
		{
			me = player;
		}
		if (playerMap.ContainsKey(player.ID))
		{
			playerMap[player.ID] = player;
			for (int i = 0; i < PlayerList.Count; i++)
			{
				if (PlayerList[i].ID == player.ID)
				{
					PlayerList[i] = player;
				}
			}
		}
		else
		{
			playerMap.Add(player.ID, player);
			PlayerList.Add(player);
			OnEntityListUpdated();
		}
	}

	public NPC GetNpcById(int id)
	{
		if (npcMap.ContainsKey(id))
		{
			return npcMap[id];
		}
		return null;
	}

	public List<NPC> GetActiveNPCs()
	{
		return NpcList;
	}

	public NPC GetNpcBySpawnId(int spawnID)
	{
		return NpcList.FirstOrDefault((NPC npc) => npc.SpawnID == spawnID);
	}

	public Player GetPlayerById(int id)
	{
		if (playerMap.ContainsKey(id))
		{
			return playerMap[id];
		}
		return null;
	}

	public NPC GetNpcByName(string name)
	{
		return NpcList.FirstOrDefault((NPC npc) => npc.name == name);
	}

	public Player GetPlayerByName(string name)
	{
		foreach (Player player in PlayerList)
		{
			if (player.name.ToUpperInvariant() == name.ToUpperInvariant())
			{
				return player;
			}
		}
		return null;
	}

	public Entity GetEntityByID(int id)
	{
		if (npcMap.TryGetValue(id, out var value))
		{
			return value;
		}
		if (playerMap.TryGetValue(id, out var value2))
		{
			return value2;
		}
		return null;
	}

	public Entity GetEntity(Entity.Type type, int id)
	{
		if (type == Entity.Type.NPC)
		{
			if (npcMap.ContainsKey(id))
			{
				return npcMap[id];
			}
		}
		else if (playerMap.ContainsKey(id))
		{
			return playerMap[id];
		}
		return null;
	}

	public List<NPC> GetActiveNpcsByNpcId(int id)
	{
		return NpcList.Where((NPC x) => x.NPCID == id && x.serverState != Entity.State.Dead).ToList();
	}

	public List<NPC> GetActiveNpcsByNpcIds(List<int> ids)
	{
		return NpcList.Where((NPC x) => ids.Contains(x.NPCID) && x.serverState != Entity.State.Dead).ToList();
	}

	public List<Entity> GetEntities(List<int> IDs, List<Entity.Type> Types)
	{
		if (IDs == null || Types == null || IDs.Count != Types.Count)
		{
			return null;
		}
		List<Entity> list = new List<Entity>();
		for (int i = 0; i < IDs.Count; i++)
		{
			if (Types[i] == Entity.Type.NPC && npcMap.ContainsKey(IDs[i]))
			{
				list.Add(npcMap[IDs[i]]);
			}
			else if (Types[i] == Entity.Type.Player && playerMap.ContainsKey(IDs[i]))
			{
				list.Add(playerMap[IDs[i]]);
			}
		}
		return list;
	}

	public void RemoveNpcById(int id)
	{
		if (npcMap.ContainsKey(id))
		{
			npcMap[id].Dispose();
			NpcList.Remove(npcMap[id]);
			npcMap.Remove(id);
			OnEntityListUpdated();
		}
	}

	public void RemovePlayerById(int id)
	{
		if (playerMap.ContainsKey(id))
		{
			playerMap[id].Dispose();
			PlayerList.Remove(playerMap[id]);
			playerMap.Remove(id);
			OnEntityListUpdated();
		}
	}

	public int GetPlayerCountByCellId(int cellID)
	{
		return PlayerList.Count((Player p) => p.cellID == cellID);
	}
}
