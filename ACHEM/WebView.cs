using UnityEngine;

public static class WebView
{
	private static GameObject go;

	private static UniWebView webView;

	public static bool IsWebViewSupported => false;

	public static void OpenURL(string url)
	{
		if (IsWebViewSupported)
		{
			Init();
			webView.Load(url);
			webView.Show(fade: true, UniWebViewTransitionEdge.Bottom);
		}
		else
		{
			Confirmation.OpenUrl(url);
		}
	}

	private static void Init()
	{
		if (go == null)
		{
			go = new GameObject("UniWebView");
		}
		if (webView == null)
		{
			webView = go.AddComponent<UniWebView>();
			webView.Frame = new Rect(0f, 0f, Screen.width, Screen.height);
			webView.SetShowToolbar(show: true);
			webView.SetShowSpinnerWhileLoading(flag: true);
		}
	}
}
