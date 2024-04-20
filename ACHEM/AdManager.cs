using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Networking;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
	private string placementId = "";

	public string adRequestTokenGUID;

	private bool isInitialized;

	private static AdManager instance;

	public static AdManager Instance => instance;

	public bool IsInitialized => isInitialized;

	public event Action<int, int> OnAdAvailability;

	public event Action<bool> OnAdFinished;

	public void Awake()
	{
		instance = this;
	}

	public void OnInitializationComplete()
	{
		Debug.Log("Unity Ads initialization complete.");
		isInitialized = true;
	}

	public void OnInitializationFailed(UnityAdsInitializationError error, string message)
	{
		Debug.LogError("Unity Ads Initialization Failed: " + error.ToString() + " - " + message);
	}

	public void RequestAdWatch()
	{
		BusyDialog.Show("Requesting Advertisement");
		Advertisement.Load(placementId, this);
	}

	public void OnUnityAdsAdLoaded(string adUnitId)
	{
		Debug.Log("Ad Loaded: " + adUnitId);
		if (adUnitId.Equals(placementId))
		{
			StartCoroutine(RequestAdWatchRoutine());
		}
		else
		{
			AdComplete(success: false);
		}
	}

	public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
	{
		Debug.LogError("Error loading Ad Unit " + adUnitId + ": " + error.ToString() + " - " + message);
		AdComplete(success: false);
	}

	private IEnumerator RequestAdWatchRoutine()
	{
		string errorTitle = "Ad Error";
		string friendlyMsg = "An error occurred while playing advertisement. Please check your connection or try again later.";
		using (UnityWebRequest www = UnityWebRequest.Get(Main.WebServiceURL + "/Game/RequestAdvertisement?CharID=" + Session.MyPlayerData.ID + "&UserID=" + Session.MyPlayerData.UserID + "&Session=" + UnityWebRequest.EscapeURL(Session.Account.strToken)))
		{
			string customContext = "URL: " + www.url;
			yield return www.SendWebRequest();
			if (www.isHttpError)
			{
				friendlyMsg = "There was an issue connecting to ad provider. Please try again later.";
				ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, null, customContext);
				BusyDialog.Close();
				yield break;
			}
			if (www.isNetworkError)
			{
				ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, null, customContext);
				BusyDialog.Close();
				yield break;
			}
			if (www.error != null)
			{
				ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, null, customContext);
				BusyDialog.Close();
				yield break;
			}
			string text = www.downloadHandler.text.Replace("\"", "");
			if (text.Contains("Error"))
			{
				ErrorReporting.Instance.ReportError(errorTitle, text, text, null, customContext);
				BusyDialog.Close();
				yield break;
			}
			if (text.Length != 36)
			{
				ErrorReporting.Instance.ReportError(errorTitle, text, text, null, customContext);
				BusyDialog.Close();
				yield break;
			}
			adRequestTokenGUID = text;
		}
		if (string.IsNullOrEmpty(adRequestTokenGUID))
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, "Missing Ad Token");
			BusyDialog.Close();
			yield break;
		}
		string gamerSid = "";
		Advertisement.Show(placementId, new ShowOptions
		{
			gamerSid = gamerSid
		}, this);
		BusyDialog.Close();
	}

	public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
	{
		if (adUnitId.Equals(placementId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
		{
			Debug.Log("Unity Ads Rewarded Ad Completed");
			AdComplete(success: true);
		}
		else
		{
			AdComplete(success: false);
		}
	}

	public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
	{
		Debug.LogError("Error showing Ad Unit " + adUnitId + ": " + error.ToString() + " - " + message);
		AdComplete(success: false);
	}

	public void OnUnityAdsShowStart(string adUnitId)
	{
	}

	public void OnUnityAdsShowClick(string adUnitId)
	{
	}

	private void AdComplete(bool success)
	{
		StartCoroutine(AdCompleteCoroutine(success));
	}

	private IEnumerator AdCompleteCoroutine(bool success)
	{
		yield return 0;
		if (this.OnAdFinished != null)
		{
			this.OnAdFinished(success);
		}
	}

	public void GetAdAvailability()
	{
		StartCoroutine(GetAdAvailabilityRoutine());
	}

	private IEnumerator GetAdAvailabilityRoutine()
	{
		using UnityWebRequest www = UnityWebRequest.Get(Main.WebServiceURL + "/Game/GetAdAvailability?CharID=" + Session.MyPlayerData.ID + "&UserID=" + Session.MyPlayerData.UserID);
		string errorTitle = "Ad Error";
		string friendlyMsg = "Request for advertisement failed. Please check your connection or try again later.";
		string customContext = "URL: " + www.url;
		yield return www.SendWebRequest();
		if (www.isHttpError)
		{
			friendlyMsg = "There was an issue connecting to ad provider. Please try again later.";
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
		string[] array = www.downloadHandler.text.Replace("\"", "").Split(';');
		if (array.Length == 2)
		{
			int arg = int.Parse(array[0]);
			int arg2 = int.Parse(array[1]);
			if (this.OnAdAvailability != null)
			{
				this.OnAdAvailability(arg, arg2);
			}
		}
	}
}
