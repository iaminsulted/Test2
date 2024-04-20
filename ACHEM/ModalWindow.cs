using UnityEngine;

public class ModalWindow : MonoBehaviour
{
	private static ModalWindow instance;

	public static void Clear()
	{
		if (instance != null)
		{
			instance.Close();
		}
	}

	protected void Init()
	{
		Clear();
		instance = this;
	}

	protected virtual void Close()
	{
		Object.Destroy(base.gameObject);
		instance = null;
	}
}
