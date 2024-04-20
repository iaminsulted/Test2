using UnityEngine;

public abstract class NotificationQuest : MonoBehaviour
{
	public virtual void Activate()
	{
		base.gameObject.SetActive(value: true);
	}
}
