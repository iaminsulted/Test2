using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class CameraPlayerBlend : MonoBehaviour
{
	public Transform mainCamera;

	public float MaxListenerDistance = 4f;

	private Vector3 offset = new Vector3(0f, 1.74f, 0f);

	private void Update()
	{
		if (!(Entities.Instance.me?.wrapper == null))
		{
			Vector3 position = Entities.Instance.me.wrapper.transform.position;
			Vector3 vector = mainCamera.transform.position - (position + offset);
			vector = Vector3.ClampMagnitude(vector, MaxListenerDistance);
			base.transform.position = vector + (position + offset);
			base.transform.rotation = mainCamera.transform.rotation;
		}
	}
}
