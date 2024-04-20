using UnityEngine;

public class DragRotate : MonoBehaviour
{
	public Transform target;

	public float rotationSpeed = -1f;

	public float lerpSpeed = 8f;

	private Vector2 theSpeed;

	private Vector2 lastSpeed;

	private void Start()
	{
		if (Input.touchSupported)
		{
			rotationSpeed /= 3f;
		}
	}

	public void SetTarget(Transform NewTransform)
	{
		target = NewTransform;
	}

	public void OnDrag(Vector2 d)
	{
		lastSpeed = d;
		target.transform.Rotate(0f, d.x * rotationSpeed, 0f);
	}

	public void OnDragEnd()
	{
		theSpeed = lastSpeed;
	}

	private void Update()
	{
		if (target != null)
		{
			theSpeed = Vector2.Lerp(theSpeed, Vector2.zero, Time.deltaTime * lerpSpeed);
			target.transform.Rotate(0f, theSpeed.x * rotationSpeed, 0f);
		}
	}
}
