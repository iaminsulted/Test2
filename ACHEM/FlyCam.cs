using UnityEngine;

public class FlyCam : MonoBehaviour
{
	public float mainSpeed = 1f;

	public float shiftAdd = 20f;

	public float maxShift = 20f;

	public float camSens = 0.2f;

	private float totalRun = 1f;

	private bool isRotating;

	private float speedMultiplier;

	public float mouseSensitivity = 5f;

	private float rotationY;

	public bool isToMoveForward;

	private void Update()
	{
		if (!UICamera.inputHasFocus)
		{
			Input.GetMouseButtonDown(0);
			Input.GetMouseButtonUp(0);
			if (Input.GetMouseButtonDown(1))
			{
				isRotating = true;
			}
			if (Input.GetMouseButtonUp(1))
			{
				isRotating = false;
			}
			if (isRotating)
			{
				float y = base.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
				rotationY += Input.GetAxis("Mouse Y") * mouseSensitivity;
				rotationY = Mathf.Clamp(rotationY, -90f, 90f);
				base.transform.localEulerAngles = new Vector3(0f - rotationY, y, 0f);
			}
			Vector3 vector = GetBaseInput();
			if (Input.GetKey(KeyCode.LeftShift))
			{
				totalRun += Time.deltaTime;
				vector = vector * totalRun * shiftAdd;
				vector.x = Mathf.Clamp(vector.x, 0f - maxShift, maxShift);
				vector.y = Mathf.Clamp(vector.y, 0f - maxShift, maxShift);
				vector.z = Mathf.Clamp(vector.z, 0f - maxShift, maxShift);
				speedMultiplier = totalRun * shiftAdd * Time.deltaTime;
				speedMultiplier = Mathf.Clamp(speedMultiplier, 0f - maxShift, maxShift);
			}
			else
			{
				totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
				vector *= mainSpeed;
				speedMultiplier = mainSpeed * Time.deltaTime;
			}
			vector *= Time.deltaTime;
			Vector3 position = base.transform.position;
			base.transform.Translate(vector);
			position.x = base.transform.position.x;
			position.z = base.transform.position.z;
			position.y = base.transform.position.y;
			if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.X))
			{
				position.y += 0f - speedMultiplier;
			}
			if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space))
			{
				position.y += speedMultiplier;
			}
			base.transform.position = position;
		}
	}

	public bool amIRotating()
	{
		return isRotating;
	}

	private Vector3 GetBaseInput()
	{
		Vector3 result = default(Vector3);
		Input.GetMouseButtonDown(0);
		if (Input.GetKey(KeyCode.W))
		{
			result += new Vector3(0f, 0f, 1f);
		}
		if (Input.GetKey(KeyCode.S))
		{
			result += new Vector3(0f, 0f, -1f);
		}
		if (Input.GetKey(KeyCode.A))
		{
			result += new Vector3(-1f, 0f, 0f);
		}
		if (Input.GetKey(KeyCode.D))
		{
			result += new Vector3(1f, 0f, 0f);
		}
		return result;
	}

	private void OnEnable()
	{
		GetComponentInChildren<Camera>().cullingMask &= ~(1 << Layers.CLICKIES);
	}

	private void OnDisable()
	{
		GetComponentInChildren<Camera>().cullingMask |= 1 << Layers.CLICKIES;
	}
}
