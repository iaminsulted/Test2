using UnityEngine;

public class UIUtility : MonoBehaviour
{
	public void EnableAndDisable()
	{
		base.gameObject.SetActive(!base.gameObject.activeSelf);
	}
}
