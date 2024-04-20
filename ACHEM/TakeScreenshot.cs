using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class TakeScreenshot : MonoBehaviour
{
	private bool multipleScreenshots;

	private Vector2Int[] screenSizes = new Vector2Int[4]
	{
		new Vector2Int(2732, 2048),
		new Vector2Int(2208, 1242),
		new Vector2Int(2688, 1242),
		new Vector2Int(1920, 1080)
	};

	private float[] uiScaleFactors = new float[4] { 1.5f, 2f, 2f, 2f };

	private bool includeUI = true;

	private Camera uiCamera;

	private UIPanel uiGamePanel;

	private UIScaleAdjustDPI uiScale;

	public string ScreenCapDirectory = "C:\\Screenshots";

	private void Awake()
	{
		if (!multipleScreenshots)
		{
			includeUI = false;
			screenSizes = new Vector2Int[1]
			{
				new Vector2Int(4096, 2160)
			};
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F11))
		{
			if (!Directory.Exists("C:/Screenshots"))
			{
				Directory.CreateDirectory("C:/Screenshots");
			}
			StartCoroutine(TakeScreenshots(screenSizes));
		}
		if (Input.GetKeyDown(KeyCode.Mouse4))
		{
			Camera.main.allowDynamicResolution = true;
			Camera.main.allowMSAA = true;
			Camera.main.allowHDR = true;
			if (!Directory.Exists("C:/Screenshots"))
			{
				Directory.CreateDirectory("C:/Screenshots");
			}
			RenderTexture renderTexture = new RenderTexture(4096, 2160, 24);
			Camera.main.targetTexture = renderTexture;
			Texture2D texture2D = new Texture2D(4096, 2160, TextureFormat.RGBA32, mipChain: false);
			Camera.main.Render();
			RenderTexture.active = renderTexture;
			texture2D.ReadPixels(new Rect(0f, 0f, 4096f, 2160f), 0, 0);
			Camera.main.targetTexture = null;
			RenderTexture.active = null;
			UnityEngine.Object.Destroy(renderTexture);
			byte[] bytes = texture2D.EncodeToPNG();
			string text = string.Format("C:/Screenshots/Screenshot_{0}.png", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
			File.WriteAllBytes(text, bytes);
			Debug.Log($"Took screenshot: {text}");
		}
	}

	private IEnumerator TakeScreenshots(params Vector2Int[] screenSizes)
	{
		Time.timeScale = 0f;
		Game.IngoreResponses = true;
		for (int i = 0; i < screenSizes.Length; i++)
		{
			Vector2Int screenSize = screenSizes[i];
			yield return new WaitForSecondsRealtime(0.2f);
			yield return new WaitForEndOfFrame();
			int x = screenSize.x;
			int y = screenSize.y;
			_ = $"Screenshot size {x}, {y}";
			Screen.SetResolution(x, y, SettingsManager.IsFullScreen);
			if (uiScale == null)
			{
				uiScale = UnityEngine.Object.FindObjectOfType<UIScaleAdjustDPI>();
			}
			if (uiScale != null && i < uiScaleFactors.Length)
			{
				uiScale.ScaleFactor = uiScaleFactors[i];
			}
			yield return new WaitForSecondsRealtime(0.2f);
			yield return new WaitForEndOfFrame();
			RenderTexture renderTexture = new RenderTexture(x, y, 24);
			Camera.main.targetTexture = renderTexture;
			Texture2D texture2D = new Texture2D(x, y, TextureFormat.RGBA32, mipChain: false);
			Camera.main.Render();
			RenderTexture.active = renderTexture;
			texture2D.ReadPixels(new Rect(0f, 0f, x, y), 0, 0);
			texture2D.Apply();
			RenderTexture renderTexture2 = null;
			Texture2D texture2D2 = null;
			string text;
			if (includeUI)
			{
				if (uiCamera == null)
				{
					uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
				}
				if (uiGamePanel == null)
				{
					uiGamePanel = UnityEngine.Object.FindObjectOfType<UIGame>().GetComponent<UIPanel>();
				}
				uiGamePanel.UpdateAnchors();
				renderTexture2 = new RenderTexture(x, y, 24);
				uiCamera.targetTexture = renderTexture2;
				texture2D2 = new Texture2D(x, y, TextureFormat.RGBA32, mipChain: false);
				uiCamera.Render();
				RenderTexture.active = renderTexture2;
				texture2D2.ReadPixels(new Rect(0f, 0f, x, y), 0, 0);
				texture2D2.Apply();
				byte[] bytes = texture2D.AlphaBlend(texture2D2).EncodeToPNG();
				text = string.Format("C:/Screenshots/{0}-({1}x{2}).png", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), x, y);
				File.WriteAllBytes(text, bytes);
			}
			else
			{
				byte[] bytes2 = texture2D.EncodeToPNG();
				text = string.Format("C:/Screenshots/Screenshot_{0}_{1}x{2}.png", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), x, y);
				File.WriteAllBytes(text, bytes2);
			}
			Camera.main.targetTexture = null;
			RenderTexture.active = null;
			UnityEngine.Object.Destroy(renderTexture);
			UnityEngine.Object.Destroy(texture2D);
			if (includeUI)
			{
				uiCamera.targetTexture = null;
				UnityEngine.Object.Destroy(renderTexture2);
				UnityEngine.Object.Destroy(texture2D2);
			}
			Debug.Log($"Took screenshot: {text}");
		}
		Time.timeScale = 1f;
		Game.IngoreResponses = false;
	}
}
