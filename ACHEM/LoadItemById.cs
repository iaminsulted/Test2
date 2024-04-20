using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class LoadItemById : MonoBehaviour
{
	public int Id;

	public List<InteractionRequirement> Requirements = new List<InteractionRequirement>();

	private AssetBundleLoader assetBundleLoader;

	private Item item;

	private GameObject itemGO;

	private bool isLoading;

	public bool AreRequirementsMet
	{
		get
		{
			foreach (InteractionRequirement requirement in Requirements)
			{
				if (!requirement.IsRequirementMet(Session.MyPlayerData))
				{
					return false;
				}
			}
			return true;
		}
	}

	private void Start()
	{
		UpdateState();
	}

	private void OnEnable()
	{
		foreach (InteractionRequirement requirement in Requirements)
		{
			requirement.Updated += OnRequirementUpdated;
		}
	}

	private void OnDisable()
	{
		foreach (InteractionRequirement requirement in Requirements)
		{
			requirement.Updated -= OnRequirementUpdated;
		}
	}

	private void OnRequirementUpdated()
	{
		UpdateState();
	}

	private void UpdateState()
	{
		if (AreRequirementsMet)
		{
			if (itemGO != null)
			{
				itemGO.SetActive(value: true);
			}
			else
			{
				StartCoroutine(SendGetItemsRequest(Id));
			}
		}
		else if (itemGO != null)
		{
			itemGO.SetActive(value: false);
		}
	}

	private IEnumerator SendGetItemsRequest(int id)
	{
		if (isLoading)
		{
			yield break;
		}
		isLoading = true;
		WWWForm form = new WWWForm();
		form.AddField("IDs", Id.ToString());
		using (UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL + "/Game/GetItems", form))
		{
			string errorTitle = "Get Items Load Failed!";
			string friendlyMsg = "Failed to connect to the server.";
			string customContext = "ID: " + id;
			yield return www.SendWebRequest();
			customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
			if (www.isHttpError)
			{
				ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, form, customContext);
			}
			else if (www.isNetworkError)
			{
				ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, form, customContext);
			}
			else if (www.error != null)
			{
				ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, form, customContext);
			}
			else
			{
				try
				{
					item = JsonConvert.DeserializeObject<List<Item>>(www.downloadHandler.text)[0];
				}
				catch (Exception ex)
				{
					customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
					ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, form, customContext, ex);
				}
				assetBundleLoader = AssetBundleManager.LoadAssetBundle(item.bundle);
				yield return assetBundleLoader;
				if (!string.IsNullOrEmpty(assetBundleLoader.Error))
				{
					yield break;
				}
				AssetBundleRequest abr = assetBundleLoader.Asset.LoadAssetAsync<GameObject>(item.AssetName);
				yield return abr;
				GameObject gameObject = UnityEngine.Object.Instantiate(abr.asset as GameObject, base.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.layer = 0;
				itemGO = gameObject;
			}
		}
		isLoading = false;
	}

	private void OnDestroy()
	{
		assetBundleLoader.Dispose();
	}
}
