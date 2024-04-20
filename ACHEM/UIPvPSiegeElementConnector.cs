using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIPvPSiegeElementConnector : MonoBehaviour
{
	public List<UIPvPSiegeElement> SiegeElements;

	public void OnEnable()
	{
		foreach (UIPvPSiegeElement siegeElement in SiegeElements)
		{
			siegeElement.StateUpdated += OnSpawnStateUpdated;
		}
		UpdateState();
	}

	private void OnSpawnStateUpdated()
	{
		UpdateState();
	}

	private void UpdateState()
	{
		GetComponent<UISprite>().enabled = SiegeElements.All((UIPvPSiegeElement p) => p.spawn.State == 1) && SiegeElements.Any((UIPvPSiegeElement p) => !p.IsUnLocked);
	}

	public void OnDisable()
	{
		foreach (UIPvPSiegeElement siegeElement in SiegeElements)
		{
			siegeElement.StateUpdated -= OnSpawnStateUpdated;
		}
	}
}
