using UnityEngine;

public class DeviceSupport : MonoBehaviour
{
	public DeviceType targetDeviceType;

	private void Awake()
	{
		if (targetDeviceType == SystemInfo.deviceType)
		{
			base.gameObject.SetActive(value: true);
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
