using System.Collections;
using UnityEngine;

public class LoaderScreenie : MonoBehaviour
{
	public UILabel LabelMessage;

	public UITexture BG;

	public UIPanel Panel;

	public Texture2D ScreenShotTexture;

	private static LoaderScreenie mInstance;

	public static bool IsVisible
	{
		get
		{
			if (mInstance != null)
			{
				return mInstance.gameObject.activeSelf;
			}
			return false;
		}
	}

	private void Awake()
	{
		mInstance = this;
	}

	private void OnDestroy()
	{
		mInstance = null;
		Object.Destroy(ScreenShotTexture);
	}

	public static void Show(string message)
	{
		if (mInstance == null)
		{
			GameObject obj = Object.Instantiate(Resources.Load<GameObject>("LoaderScreenie"), UIManager.Instance.transform);
			obj.name = "LoaderScreenie";
			mInstance = obj.GetComponent<LoaderScreenie>();
		}
		mInstance.Init(message);
	}

	private void Init(string message)
	{
		base.gameObject.SetActive(value: true);
		LabelMessage.text = message;
		StartCoroutine(Screenie());
	}

	private IEnumerator Screenie()
	{
		if (!(ScreenShotTexture != null))
		{
			Panel.enabled = false;
			yield return new WaitForEndOfFrame();
			int width = Screen.width;
			int height = Screen.height;
			ScreenShotTexture = new Texture2D(width, height, TextureFormat.RGB24, mipChain: false);
			ScreenShotTexture.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
			ScreenShotTexture.Apply();
			BG.mainTexture = ScreenShotTexture;
			Panel.enabled = true;
		}
	}

	private void close()
	{
		StopAllCoroutines();
		Object.Destroy(ScreenShotTexture);
		base.gameObject.SetActive(value: false);
	}

	public static void Close()
	{
		if (mInstance != null)
		{
			mInstance.close();
		}
	}
}
