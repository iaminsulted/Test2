using UnityEngine;

public abstract class UINotificationIcon : MonoBehaviour
{
	public GameObject notifIcon;

	public bool requiresOn;

	public abstract bool ShouldIBeOn();
}
