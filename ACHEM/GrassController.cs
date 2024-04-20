using UnityEngine;

public class GrassController : MonoBehaviour
{
	private void Awake()
	{
		SettingsManager.GrassUpdated += OnGrassUpdated;
		OnGrassUpdated((float)SettingsManager.GrassDistance > 0f);
	}

	private void OnGrassUpdated(bool isEnabled)
	{
		base.gameObject.SetActive(isEnabled);
	}

	private void OnDestroy()
	{
		SettingsManager.GrassUpdated -= OnGrassUpdated;
	}
}
