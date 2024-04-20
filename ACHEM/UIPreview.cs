using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using StatCurves;
using UnityEngine;
using UnityEngine.Networking;

public class UIPreview : UIWindow
{
	public UISprite itemicon;

	public UILabel itemname;

	public PreviewGenerator preview;

	public static UIPreview instance;

	public Camera previewCam;

	public GameObject modelShadow;

	public float initShadowPosX;

	public float initShadowPosY;

	public static void Show(Item item)
	{
		if (instance == null)
		{
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/ItemPreview"), UIManager.Instance.transform).GetComponent<UIPreview>();
			instance.Init();
		}
		instance.Load(item);
	}

	public void Awake()
	{
		modelShadow = base.transform.GetChild(0).GetChild(1).gameObject;
		previewCam = base.transform.GetComponentInChildren<Camera>();
		initShadowPosX = modelShadow.transform.position.x;
		initShadowPosY = modelShadow.transform.position.y;
	}

	public void Update()
	{
		UpdatePreviewZoom();
	}

	private void UpdatePreviewZoom()
	{
		if (UIGame.ControlScheme == ControlScheme.PC)
		{
			if (Input.mouseScrollDelta.y == 0f)
			{
				return;
			}
			previewCam.fieldOfView += (float)SettingsManager.ZoomSpeed * 0.05f - Input.mouseScrollDelta.y;
			if (previewCam.fieldOfView < 20f)
			{
				previewCam.fieldOfView = 20f;
			}
			else if (previewCam.fieldOfView > 80f)
			{
				previewCam.fieldOfView = 80f;
			}
		}
		else
		{
			if (Input.touchCount < 2)
			{
				return;
			}
			Touch touch = Input.GetTouch(0);
			Touch touch2 = Input.GetTouch(1);
			Vector2 vector = touch.position - touch.deltaPosition;
			Vector2 vector2 = touch2.position - touch2.deltaPosition;
			float magnitude = (vector - vector2).magnitude;
			float magnitude2 = (touch.position - touch2.position).magnitude;
			float num = magnitude - magnitude2;
			previewCam.fieldOfView += (float)SettingsManager.ZoomSpeed * num * 0.05f;
			if (previewCam.fieldOfView < 20f)
			{
				previewCam.fieldOfView = 20f;
			}
			else if (previewCam.fieldOfView > 80f)
			{
				previewCam.fieldOfView = 80f;
			}
		}
		modelShadow.transform.position = new Vector3(initShadowPosX * 40f / previewCam.fieldOfView, initShadowPosY * 40f / previewCam.fieldOfView, modelShadow.transform.position.z);
		modelShadow.transform.localScale = new Vector3(40f / previewCam.fieldOfView, 40f / previewCam.fieldOfView, modelShadow.transform.localScale.z);
	}

	public void Load(Item item)
	{
		itemicon.spriteName = item.Icon;
		itemname.text = "[" + item.RarityColor + "]" + item.Name + "[-]";
		StartCoroutine(LoadPreview(item));
	}

	public IEnumerator LoadPreview(Item item)
	{
		Player me = Entities.Instance.me;
		if (item.TravelParams != null && (item.Type == ItemType.Item || item.Type == ItemType.Consumable))
		{
			int npcID = ArtixRandom.GetElementOfList(item.TravelParams.NpcIDPool);
			if (npcID == 0)
			{
				npcID = ((me.baseAsset.gender == "M") ? item.TravelParams.mNpcID : item.TravelParams.fNpcID);
			}
			using (UnityWebRequest www = UnityWebRequest.Get(Main.WebServiceURL + "/Utilities/GetAllNPCAssetData?idtosplit=" + npcID))
			{
				string errorTitle = "Failed to Load Travel Form";
				string friendlyMsg = "Unable to communicate with the server.";
				string customContext = "npcID: " + npcID;
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
					Dictionary<int, EntityAsset> dictionary = JsonConvert.DeserializeObject<Dictionary<int, EntityAsset>>(www.downloadHandler.text);
					if (dictionary.ContainsKey(npcID))
					{
						preview.ShowWithMarkers(dictionary[npcID], isPetOrTravelform: true);
					}
				}
				catch (Exception ex)
				{
					customContext = "Invalid NPC Data: " + customContext;
					friendlyMsg = "Unable to process response from the server.";
					ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, null, customContext);
					yield break;
				}
			}
			yield break;
		}
		if (item.Type == ItemType.Pet)
		{
			EntityAsset assetdata = new EntityAsset
			{
				prefab = item.AssetName,
				bundle = item.bundle,
				gender = "N",
				ColorR = item.ColorR,
				ColorG = item.ColorG,
				ColorB = item.ColorB
			};
			preview.ShowWithMarkers(assetdata, isPetOrTravelform: true);
			yield break;
		}
		if (item.Type == ItemType.Bobber)
		{
			EntityAsset assetdata2 = new EntityAsset
			{
				prefab = item.AssetName,
				bundle = item.bundle,
				gender = "N"
			};
			preview.ShowWithMarkers(assetdata2);
			yield break;
		}
		if (item.Type == ItemType.HouseItem)
		{
			EntityAsset assetdata3 = new EntityAsset
			{
				prefab = item.AssetName,
				bundle = item.bundle,
				gender = "N"
			};
			preview.ShowWithMarkers(assetdata3);
			yield break;
		}
		EntityAsset entityAsset = new EntityAsset(me.baseAsset);
		if (item.IsEquipType)
		{
			entityAsset.equips[item.EquipSlot] = item;
			if (item.IsWeapon)
			{
				entityAsset.Current = item.EquipSlot;
				entityAsset.WeaponRequired = item.EquipSlot;
				entityAsset.DualWield = false;
			}
			else if (item.IsTool)
			{
				entityAsset.Current = item.EquipSlot;
				entityAsset.DualWield = false;
			}
		}
		preview.ShowWithMarkers(entityAsset);
	}
}
