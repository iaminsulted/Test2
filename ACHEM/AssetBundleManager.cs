using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundleManager : MonoBehaviour
{
	private class AssetBundleRef
	{
		public string url;

		public AssetBundle assetBundle;

		public int m_ReferencedCount;

		public AssetBundleRef(AssetBundle bundle, string url)
		{
			assetBundle = bundle;
			this.url = url;
		}
	}

	private static List<UnityWebRequest> loads = new List<UnityWebRequest>();

	private static Dictionary<string, AssetBundleRef> assetbundles = new Dictionary<string, AssetBundleRef>();

	private static Dictionary<string, List<AssetBundleLoader>> listeners = new Dictionary<string, List<AssetBundleLoader>>();

	public void Update()
	{
		if (loads.Count <= 0)
		{
			return;
		}
		UnityWebRequest[] array = loads.ToArray();
		string errorTitle = "AssetBundle Load Failed";
		foreach (UnityWebRequest unityWebRequest in array)
		{
			string text = "URL: " + unityWebRequest.url;
			if (unityWebRequest.isHttpError || unityWebRequest.isNetworkError || !string.IsNullOrEmpty(unityWebRequest.error))
			{
				text = "CF-Ray: " + unityWebRequest.GetResponseHeader("CF-Ray") + " " + text;
				CallbackError(unityWebRequest.url, unityWebRequest.error);
				ErrorReporting.Instance.ReportError(errorTitle, "Failed to communicate with the server.", unityWebRequest.error, null, text, null, showMessageBox: false);
				unityWebRequest.Dispose();
				loads.Remove(unityWebRequest);
			}
			else
			{
				if (!unityWebRequest.isDone)
				{
					continue;
				}
				try
				{
					AssetBundle content = DownloadHandlerAssetBundle.GetContent(unityWebRequest);
					if (content == null)
					{
						text = "CF-Ray: " + unityWebRequest.GetResponseHeader("CF-Ray") + " " + text;
						CallbackError(unityWebRequest.url, "Asset bundle could not be loaded.");
						ErrorReporting.Instance.ReportError(errorTitle, "No bundle found.", "bundle == null", null, text, null, showMessageBox: false);
					}
					else
					{
						Debug.Log("Load done" + unityWebRequest.url);
						Callback(unityWebRequest.url, new AssetBundleRef(content, unityWebRequest.url));
					}
				}
				catch (Exception ex)
				{
					CallbackError(unityWebRequest.url, ex.Message);
					ErrorReporting.Instance.ReportError(errorTitle, "Unable to process asset bundle.", ex.Message, null, text, ex, showMessageBox: false);
				}
				unityWebRequest.Dispose();
				loads.Remove(unityWebRequest);
			}
		}
	}

	private static void Callback(string url, AssetBundleRef bundle)
	{
		if (!listeners.ContainsKey(url) || listeners[url].Count == 0)
		{
			Debug.Log("[AssetBundleManager]: Listener Not Found - " + url);
			bundle.assetBundle.Unload(unloadAllLoadedObjects: false);
			return;
		}
		assetbundles[url] = bundle;
		List<AssetBundleLoader> list = listeners[url];
		listeners.Remove(url);
		if (list.Count <= 0)
		{
			return;
		}
		foreach (AssetBundleLoader item in list)
		{
			bundle.m_ReferencedCount++;
			item.Complete("", bundle.assetBundle);
		}
	}

	private static void CallbackError(string url, string error)
	{
		if (!listeners.ContainsKey(url))
		{
			Debug.Log("Assets.Update() - Key Not Found: " + url);
			return;
		}
		List<AssetBundleLoader> list = listeners[url];
		listeners.Remove(url);
		if (list.Count <= 0)
		{
			return;
		}
		foreach (AssetBundleLoader item in list)
		{
			item.Complete(error, null);
		}
	}

	public static AssetBundle GetBundle(string url)
	{
		if (assetbundles.ContainsKey(url))
		{
			return assetbundles[url].assetBundle;
		}
		return null;
	}

	private static void Unload(string url, bool forceUnloadAssets)
	{
		if (assetbundles.ContainsKey(url) && --assetbundles[url].m_ReferencedCount == 0)
		{
			Debug.Log("[AssetBundleManager]: Unloading " + url);
			assetbundles[url].assetBundle.Unload(forceUnloadAssets);
			assetbundles.Remove(url);
		}
	}

	public static void UnloadAll()
	{
		foreach (AssetBundleRef item in assetbundles.Values.ToList())
		{
			Debug.Log("[AssetBundleManager]: Unloading " + item.url);
			item.assetBundle.Unload(unloadAllLoadedObjects: false);
			assetbundles.Remove(item.url);
			listeners.Remove(item.url);
		}
	}

	private static void WWWLoad(AssetBundleLoader request)
	{
		if (!loads.Exists((UnityWebRequest p) => p.url == request.FullUrl))
		{
			Debug.Log("[AssetBundleManager]: WWWLoad - " + request.FullUrl + " " + request.CRC);
			UnityWebRequest assetBundle = UnityWebRequestAssetBundle.GetAssetBundle(request.FullUrl, request.Version, (uint)request.CRC);
			assetBundle.SendWebRequest();
			loads.Add(assetBundle);
		}
		if (!listeners.ContainsKey(request.FullUrl))
		{
			listeners.Add(request.FullUrl, new List<AssetBundleLoader>());
		}
		listeners[request.FullUrl].Add(request);
	}

	public static float GetProgress(AssetBundleLoader request)
	{
		if (assetbundles.ContainsKey(request.FullUrl))
		{
			return 1f;
		}
		foreach (UnityWebRequest load in loads)
		{
			if (load.url == request.FullUrl)
			{
				return load.downloadProgress;
			}
		}
		return 0f;
	}

	public static AssetBundleLoader LoadAssetBundle(BundleInfo bundleinfo, Action<AssetBundle> bundle = null)
	{
		AssetBundleLoader assetBundleLoader = new AssetBundleLoader(bundleinfo);
		assetBundleLoader.OnComplete = (Action<AssetBundle>)Delegate.Combine(assetBundleLoader.OnComplete, bundle);
		if (assetbundles.ContainsKey(assetBundleLoader.FullUrl))
		{
			assetbundles[assetBundleLoader.FullUrl].m_ReferencedCount++;
			assetBundleLoader.Complete("", assetbundles[assetBundleLoader.FullUrl].assetBundle);
			return assetBundleLoader;
		}
		WWWLoad(assetBundleLoader);
		return assetBundleLoader;
	}

	public static void Remove(AssetBundleLoader abl, bool forceUnloadAssets)
	{
		if (listeners.ContainsKey(abl.FullUrl))
		{
			listeners[abl.FullUrl].Remove(abl);
		}
		Unload(abl.FullUrl, forceUnloadAssets);
	}
}
