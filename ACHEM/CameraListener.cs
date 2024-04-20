using System.Collections.Generic;
using UnityEngine;

public class CameraListener : MonoBehaviour
{
	public List<ClientTriggerAction> CTAs = new List<ClientTriggerAction>();

	private void Start()
	{
		Game.Instance.camController.CameraMoved += OnCameraMoved;
	}

	private void OnCameraMoved()
	{
		Game.Instance.camController.CameraMoved -= OnCameraMoved;
		foreach (ClientTriggerAction cTA in CTAs)
		{
			cTA.Execute();
		}
	}

	private void OnDestroy()
	{
		if (Game.Instance != null)
		{
			Game.Instance.camController.CameraMoved -= OnCameraMoved;
		}
	}
}
