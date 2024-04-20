using System;
using System.Collections;
using UnityEngine;

public class AssetBundleLoader : IEnumerator
{
	private bool isComplete;

	public string FullUrl;

	public string Error;

	public BundleInfo bundleinfo;

	public Action<AssetBundle> OnComplete;

	public AssetBundle Asset;

	public uint Version => (uint)bundleinfo.Version;

	public long CRC => bundleinfo.CRC;

	public object Current => null;

	public bool isDone => isComplete;

	public bool isError
	{
		get
		{
			if (isDone)
			{
				return !string.IsNullOrEmpty(Error);
			}
			return false;
		}
	}

	public AssetBundleLoader(BundleInfo bundleinfo, Action<AssetBundle> OnComplete)
	{
		this.bundleinfo = bundleinfo;
		FullUrl = Main.AssetBundleURL + bundleinfo.FileName;
	}

	public AssetBundleLoader(BundleInfo bundleinfo)
	{
		this.bundleinfo = bundleinfo;
		FullUrl = Main.AssetBundleURL + bundleinfo.FileName;
	}

	public void Complete(string error, AssetBundle asset)
	{
		Error = error;
		Asset = asset;
		isComplete = true;
		OnComplete?.Invoke(asset);
	}

	public float GetProgress()
	{
		return Mathf.Clamp01(AssetBundleManager.GetProgress(this));
	}

	public bool MoveNext()
	{
		return !isDone;
	}

	public void Reset()
	{
	}

	public void Dispose(bool forceUnloadAssets = false)
	{
		AssetBundleManager.Remove(this, forceUnloadAssets);
	}
}
