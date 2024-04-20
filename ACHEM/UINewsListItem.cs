using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class UINewsListItem : MonoBehaviour
{
	public UILabel Title;

	public UILabel Subheading;

	public UILabel Body;

	public UILabel ButtonLabel;

	public UITexture Preview;

	public UIButton Button;

	public NPCIA npcia;

	public void init(NPCIA npcia)
	{
		this.npcia = npcia;
		Title.text = npcia.Label;
		Subheading.text = npcia.ImageTitle;
		Body.text = npcia.ImageDesc;
		ButtonLabel.text = npcia.children[0].Label;
		StartCoroutine(ShowApopImageNew());
	}

	private IEnumerator ShowApopImageNew()
	{
		if (npcia.ImageUrl != Preview.name)
		{
			using UnityWebRequest www = UnityWebRequestTexture.GetTexture(Main.APPLICATION_PATH + "/" + npcia.ImageUrl);
			string errorTitle = "Load Error";
			string friendlyMsg = "Failed to load news image: " + npcia.ImageUrl;
			yield return www.SendWebRequest();
			if (www.isHttpError)
			{
				ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode);
			}
			else if (www.isNetworkError)
			{
				ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error);
			}
			else if (www.error != null)
			{
				ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error);
			}
			else
			{
				try
				{
					Texture content = DownloadHandlerTexture.GetContent(www);
					UnityEngine.Object.DestroyImmediate(Preview.mainTexture, allowDestroyingAssets: true);
					Preview.width = Mathf.CeilToInt((float)Preview.height * (float)content.width / (float)content.height);
					Preview.mainTexture = content;
					Preview.name = npcia.ImageUrl;
				}
				catch (Exception ex)
				{
					ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message);
				}
			}
		}
		if (BusyDialog.IsVisible)
		{
			BusyDialog.Close();
		}
		yield return null;
	}

	public void Load(NPCIA npcia)
	{
		NPCIA nPCIA = npcia.children.Where((NPCIA p) => p.IsTriggerAvailable()).FirstOrDefault();
		if (nPCIA != null)
		{
			if (nPCIA is NPCIAAction && ((NPCIAAction)nPCIA).Action is CTAAsyncCore)
			{
				CTAAsyncCore cTAAsyncCore = ((NPCIAAction)nPCIA).Action as CTAAsyncCore;
				if (cTAAsyncCore.OnCompleteActions != null && cTAAsyncCore.OnCompleteActions.Count == 0)
				{
					CTANPCIACore cTANPCIACore = new CTANPCIACore();
					npcia.Actions.Add(cTANPCIACore);
					cTANPCIACore.Title = "News";
					cTANPCIACore.Apops = new List<NPCIA> { npcia };
					cTAAsyncCore.OnCompleteActions.Add(cTANPCIACore);
				}
			}
			Session.Set(nPCIA.GetSessionDataID(), 0f);
			Load(nPCIA);
			return;
		}
		foreach (IARequiredCore requirement in npcia.Requirements)
		{
			if (!requirement.IsRequirementMet(Session.MyPlayerData) || DateTime.Now <= npcia.DateStart || (DateTime.Now >= npcia.DateEnd && npcia.DateStart != DateTime.MinValue))
			{
				return;
			}
		}
		if (npcia is NPCIAAction)
		{
			((NPCIAAction)npcia).Action.Execute();
		}
		else if (npcia is NPCIAShop)
		{
			UIShop.LoadShop(((NPCIAShop)npcia).ShopID);
		}
		else if (npcia is NPCIAQuest)
		{
			UIQuest.ShowQuests(((NPCIAQuest)npcia).QuestIDs, ((NPCIAQuest)npcia).QuestIDs);
		}
	}

	public void OnClick()
	{
		Load(npcia.children.FirstOrDefault());
	}
}
