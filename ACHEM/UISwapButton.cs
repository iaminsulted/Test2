using UnityEngine;

public class UISwapButton : MonoBehaviour
{
	private bool spun;

	private Vector3 rotationAmount;

	private void Start()
	{
		rotationAmount = base.transform.rotation.eulerAngles;
		rotationAmount = new Vector3(rotationAmount.x, rotationAmount.y + 360f, rotationAmount.z);
		base.transform.rotation = Quaternion.Euler(rotationAmount);
	}

	private void OnClick()
	{
		iTween.RotateBy(base.gameObject, new Vector3(0f, 0f, 0.5f), 0.3f);
		spun = !spun;
	}
}
