using UnityEngine;

public class RotateOnTick : MonoBehaviour
{
	public float TickDelay;

	public float RotationX;

	public float RotationY;

	public float RotationZ;

	private void Start()
	{
		InvokeRepeating("Rotate", TickDelay, TickDelay);
	}

	private void Rotate()
	{
		base.transform.Rotate(RotationX, RotationY, RotationZ);
	}
}
