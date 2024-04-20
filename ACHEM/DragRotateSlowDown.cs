using System;
using UnityEngine;

public class DragRotateSlowDown : MonoBehaviour
{
	public float rotationSpeed = 9f;

	public float lerpSpeed = 15f;

	private Vector3 theSpeed;

	private bool onUI;

	private void Start()
	{
		UICamera.onPress = (UICamera.BoolDelegate)Delegate.Combine(UICamera.onPress, new UICamera.BoolDelegate(MyListener));
		if (Input.touchSupported)
		{
			rotationSpeed /= 3f;
		}
	}

	private void MyListener(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			if (UICamera.isOverUI)
			{
				onUI = true;
			}
		}
		else
		{
			onUI = false;
		}
	}

	private void Update()
	{
		if (Input.GetMouseButton(0) && !onUI)
		{
			float num = Input.GetAxis("Mouse X");
			float y = Input.GetAxis("Mouse Y");
			if (Input.touchCount > 0)
			{
				num = Input.touches[0].deltaPosition.x;
				y = Input.touches[0].deltaPosition.y;
			}
			theSpeed = new Vector3(0f - num, y, 0f);
		}
		else
		{
			theSpeed = Vector3.Lerp(theSpeed, Vector3.zero, Time.deltaTime * lerpSpeed);
		}
		base.transform.Rotate(0f, theSpeed.x * rotationSpeed, 0f);
	}
}
