using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace Assets.Scripts.Game;

public class WebAPI
{
	public static IEnumerator Request<T>(string link, string friendlyMsg, Action<T> callback, Action<float> loaderCallback = null)
	{
		using UnityWebRequest www = UnityWebRequest.Get(Main.WebServiceURL + link);
		string customContext = "URL: " + www.url;
		string errorTitle = "Loading Error";
		if (loaderCallback != null)
		{
			UnityWebRequestAsyncOperation request = www.SendWebRequest();
			while (!request.isDone)
			{
				loaderCallback(request.progress);
				yield return null;
			}
		}
		else
		{
			yield return www.SendWebRequest();
		}
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
		try
		{
			T obj = JsonConvert.DeserializeObject<T>(www.downloadHandler.text);
			callback?.Invoke(obj);
		}
		catch (Exception ex)
		{
			customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, null, customContext, ex);
			yield break;
		}
	}
}
