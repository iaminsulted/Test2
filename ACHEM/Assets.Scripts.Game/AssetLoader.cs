using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game;

public class AssetLoader<T> where T : class
{
	private class BundleDownload
	{
		public Action<T> OnComplete;

		public AssetBundleLoader BundleLoader;
	}

	private Dictionary<string, BundleDownload> pendingDownloads = new Dictionary<string, BundleDownload>();

	private Dictionary<string, BundleDownload> completedDownloads = new Dictionary<string, BundleDownload>();

	public bool IsFinished => pendingDownloads.Count == 0;

	public void DisposeAll()
	{
		foreach (BundleDownload value in completedDownloads.Values)
		{
			value.OnComplete = null;
			value.BundleLoader.Dispose(forceUnloadAssets: true);
		}
		foreach (BundleDownload value2 in pendingDownloads.Values)
		{
			value2.OnComplete = null;
			value2.BundleLoader.Dispose(forceUnloadAssets: true);
		}
		pendingDownloads.Clear();
		completedDownloads.Clear();
	}

	public void Get(string assetName, BundleInfo bInfo, Action<T> onRecieved, bool isAssetCacheEnabled = false)
	{
		if (assetName == null || bInfo == null)
		{
			return;
		}
		if (completedDownloads.TryGetValue(assetName, out var value))
		{
			AssetBundleRequest abr = value.BundleLoader.Asset.LoadAssetAsync(assetName);
			abr.completed += delegate
			{
				onRecieved?.Invoke(abr.asset as T);
			};
			return;
		}
		if (pendingDownloads.TryGetValue(assetName, out var value2))
		{
			BundleDownload bundleDownload2 = value2;
			bundleDownload2.OnComplete = (Action<T>)Delegate.Combine(bundleDownload2.OnComplete, (Action<T>)delegate(T asset)
			{
				onRecieved?.Invoke(asset);
			});
			return;
		}
		BundleDownload bundleDownload = new BundleDownload();
		bundleDownload.OnComplete = delegate(T asset)
		{
			onRecieved?.Invoke(asset);
		};
		pendingDownloads.Add(assetName, bundleDownload);
		bundleDownload.BundleLoader = AssetBundleManager.LoadAssetBundle(bInfo, delegate(AssetBundle bundle)
		{
			AssetBundleRequest abr = bundle.LoadAssetAsync(assetName);
			abr.completed += delegate
			{
				bundleDownload.OnComplete?.Invoke(abr.asset as T);
				pendingDownloads.Remove(assetName);
				completedDownloads.Add(assetName, bundleDownload);
			};
		});
	}
}
