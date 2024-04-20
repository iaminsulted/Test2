using UnityEngine;

public class ShakeCam : MonoBehaviour
{
	public enum NoiseType
	{
		None,
		Position,
		Rotation,
		Dual
	}

	public float positionShakeSpeed = 0.1f;

	public Vector3 positionShakeRange = new Vector3(0.1f, 0.1f, 0.1f);

	public float rotationShakeSpeed = 0.1f;

	public Vector3 rotationShakeRange = new Vector3(4f, 4f, 4f);

	private Vector3 position;

	private Quaternion initialRotation;

	private Quaternion rotationQuat;

	public NoiseType type = NoiseType.Dual;

	private void Start()
	{
		position = base.transform.localPosition;
		rotationQuat = (initialRotation = base.transform.localRotation);
	}

	private void Update()
	{
		if (type == NoiseType.Position || type == NoiseType.Dual)
		{
			ShakePosition();
		}
		if (type == NoiseType.Rotation || type == NoiseType.Dual)
		{
			ShakeRotation();
		}
	}

	public void ShakeRotation()
	{
		rotationQuat = Quaternion.Euler(initialRotation.eulerAngles + Vector3.Scale(SmoothRandom.GetVector3(rotationShakeSpeed), rotationShakeRange));
		base.transform.localRotation = rotationQuat;
	}

	public void ShakePosition()
	{
		base.transform.localPosition = position + Vector3.Scale(SmoothRandom.GetVector3(positionShakeSpeed), positionShakeRange);
	}
}
