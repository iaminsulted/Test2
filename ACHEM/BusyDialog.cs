using System;
using UnityEngine;

public class BusyDialog : MonoBehaviour
{
	private static BusyDialog mInstance;

	private static BusyDialog toBeDestroyed;

	public UILabel Message;

	public static bool IsVisible => mInstance != null;

	public static BusyDialog Instance
	{
		get
		{
			if (mInstance != null)
			{
				return mInstance;
			}
			return null;
		}
	}

	public event Action OnClose;

	private void Awake()
	{
		mInstance = this;
	}

	private void OnDestroy()
	{
		toBeDestroyed = null;
	}

	public static void Close()
	{
		try
		{
			mInstance.OnClose = null;
			toBeDestroyed = mInstance;
			mInstance = null;
			UnityEngine.Object.Destroy(toBeDestroyed.gameObject);
		}
		catch
		{
			Debug.LogWarning("Busy Dialog Is Not Open!");
		}
	}

	public static void Show(string text, bool closable = false)
	{
		if (mInstance == null)
		{
			GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(closable ? "BusyDialogClosable" : "BusyDialog"), UIManager.Instance.transform);
			obj.name = "BusyDialog";
			mInstance = obj.GetComponent<BusyDialog>();
		}
		mInstance.Message.text = text.ToUpper();
	}

	public void ManualClose()
	{
		if (this.OnClose != null)
		{
			this.OnClose();
		}
		Close();
	}
}
