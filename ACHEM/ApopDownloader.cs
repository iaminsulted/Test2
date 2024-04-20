using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class ApopDownloader : MonoBehaviour
{
	private static ApopDownloader mInstance;

	private List<ApopRequest> apopRequests = new List<ApopRequest>();

	private HashSet<int> fetchingApopIds = new HashSet<int>();

	public static event Action<List<NPCIA>> LoadEnd;

	private void Awake()
	{
		mInstance = this;
	}

	private void OnDestroy()
	{
		mInstance = null;
	}

	public static void Init(Transform parent)
	{
		GameObject obj = new GameObject("ApopLoader");
		obj.transform.SetParent(parent);
		obj.AddComponent<ApopDownloader>();
	}

	public static void GetApops(List<int> ids, Action<List<NPCIA>> setApops)
	{
		if (mInstance != null)
		{
			mInstance.GetApopsByIds(ids, setApops);
		}
	}

	private void GetApopsByIds(List<int> ids, Action<List<NPCIA>> setApops)
	{
		List<int> list = new List<int>();
		List<NPCIA> list2 = ids.Select((int id) => ApopMap.GetApop(id)).ToList();
		if (list2.All((NPCIA x) => x != null))
		{
			setApops?.Invoke(list2);
			return;
		}
		foreach (int id in ids)
		{
			if (ApopMap.GetApop(id) == null && !fetchingApopIds.Contains(id))
			{
				list.Add(id);
			}
		}
		apopRequests.Add(new ApopRequest(ids, setApops));
		Debug.Log("ApopRequests count + " + apopRequests.Count);
		if (list.Count > 0)
		{
			StartCoroutine(WaitForRequestApops(list));
		}
	}

	private IEnumerator WaitForRequestApops(List<int> request)
	{
		request.ForEach(delegate(int apopId)
		{
			fetchingApopIds.Add(apopId);
		});
		Debug.Log("Downloading: " + string.Join(",", request));
		using UnityWebRequest www = UnityWebRequest.Get(Main.WebServiceURL + "/Game/GetApops?IDs=" + string.Join(",", request));
		string errorTitle = "Loading Error";
		string friendlyMsg = "Failed to load menu data.";
		string customContext = "URL: " + www.url;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		if (www.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, null, customContext);
			yield break;
		}
		if (www.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, null, customContext);
			yield break;
		}
		if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, null, customContext);
			yield break;
		}
		Debug.Log("WWW Ok!: " + www.downloadHandler.text);
		try
		{
			List<NPCIA> list = ApopMap.BuildApops(JsonConvert.DeserializeObject<List<ApopData>>(www.downloadHandler.text));
			List<int> loadedApopIds = list.Select((NPCIA npcia) => npcia.ID).Distinct().ToList();
			fetchingApopIds.RemoveWhere((int apopId) => loadedApopIds.Contains(apopId));
			List<NPCIA> allLoadedApops = ApopMap.GetAllApops().ToList();
			List<int> allLoadedApopIds = allLoadedApops.Select((NPCIA npcia) => npcia.ID).ToList();
			apopRequests.ForEach(delegate(ApopRequest ar)
			{
				bool foundAll = true;
				ar.ids.ForEach(delegate(int reqApopId)
				{
					if (!allLoadedApopIds.Contains(reqApopId))
					{
						foundAll = false;
					}
				});
				List<NPCIA> obj = allLoadedApops.FindAll((NPCIA npcia) => ar.ids.Contains(npcia.ID));
				if (foundAll)
				{
					ar.IsComplete = true;
					ar.SetApops?.Invoke(obj);
				}
			});
			apopRequests.RemoveAll((ApopRequest ar) => ar.IsComplete);
			Debug.Log("ApopRequests count - " + apopRequests.Count);
			if (ApopDownloader.LoadEnd != null)
			{
				ApopDownloader.LoadEnd(list);
			}
		}
		catch (Exception ex)
		{
			if (ApopDownloader.LoadEnd != null)
			{
				ApopDownloader.LoadEnd(null);
			}
			customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, null, customContext, ex);
		}
	}
}
