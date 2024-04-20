using UnityEngine;

public class UICameraPlatformInput : MonoBehaviour
{
	private void Start()
	{
		if (Input.touchSupported)
		{
			GetComponent<UICamera>().useMouse = false;
		}
	}
}
