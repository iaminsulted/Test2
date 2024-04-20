using System.Collections.Generic;
using UnityEngine;

public class UIPvPSiege : MonoBehaviour
{
	public List<int> SpawnIDs;

	public List<UIPvPSiegeElement> SiegeElements;

	public GameObject spriteBackground;

	public Color flashColor;

	public Color baseColor;

	public int numberFlashes;

	public GameObject spriteBackgroundFlash;

	public float flashDuration;

	public void Init()
	{
		if (Entities.Instance.me.teamID == 2)
		{
			for (int i = 0; i < SpawnIDs.Count; i++)
			{
				SiegeElements[i].spawn = Game.Instance.GetNpcSpawn(SpawnIDs[i]);
				SiegeElements[i].Init(this);
			}
		}
		else
		{
			for (int j = 0; j < SpawnIDs.Count; j++)
			{
				SiegeElements[j].spawn = Game.Instance.GetNpcSpawn(SpawnIDs[SpawnIDs.Count - 1 - j]);
				SiegeElements[j].Init(this);
			}
		}
	}
}
