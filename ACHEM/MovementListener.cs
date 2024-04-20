using System.Collections.Generic;
using UnityEngine;

public class MovementListener : MonoBehaviour
{
	public List<ClientTriggerAction> CTAs = new List<ClientTriggerAction>();

	private void Start()
	{
		Entities.Instance.me.moveController.Moved += OnMoved;
	}

	private void OnMoved()
	{
		Entities.Instance.me.moveController.Moved -= OnMoved;
		foreach (ClientTriggerAction cTA in CTAs)
		{
			cTA.Execute();
		}
	}

	private void OnDestroy()
	{
		if (Entities.Instance != null)
		{
			Entities.Instance.me.moveController.Moved -= OnMoved;
		}
	}
}
