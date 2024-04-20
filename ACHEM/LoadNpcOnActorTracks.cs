using System;
using System.Collections;
using System.Collections.Generic;
using CinemaDirector;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class LoadNpcOnActorTracks : MonoBehaviour
{
	public int ID;

	public List<ActorTrackGroup> TrackGroups;

	public void Awake()
	{
		StartCoroutine(LoadNPC());
	}

	private IEnumerator LoadNPC()
	{
		new Dictionary<int, EntityAsset>();
		Dictionary<int, EntityAsset> dictionary;
		using (UnityWebRequest www = UnityWebRequest.Get(Main.WebServiceURL + "/Utilities/GetAllNPCAssetData?idtosplit=" + ID))
		{
			string errorTitle = "Loading Error";
			string friendlyMsg = "Failed to load NPC data";
			string customContext = "idtosplit: " + ID;
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
			try
			{
				dictionary = JsonConvert.DeserializeObject<Dictionary<int, EntityAsset>>(www.downloadHandler.text);
			}
			catch (Exception ex)
			{
				customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
				ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, null, customContext, ex);
				yield break;
			}
		}
		if (!dictionary.ContainsKey(ID))
		{
			yield break;
		}
		EntityAsset entityAsset = dictionary[ID];
		AssetController assetController = ((entityAsset.gender == "N") ? ((AssetController)base.gameObject.AddComponent<NPCAssetController>()) : ((AssetController)base.gameObject.AddComponent<PlayerAssetController>()));
		assetController.Init(entityAsset);
		yield return assetController.LoadAsset();
		foreach (ActorTrackGroup trackGroup in TrackGroups)
		{
			trackGroup.Actor = assetController.currentAsset.transform;
			if (trackGroup.Cutscene.State != 0)
			{
				trackGroup.Initialize();
				trackGroup.SetRunningTime(trackGroup.Cutscene.RunningTime);
			}
		}
	}
}
