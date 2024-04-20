using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class UIPromoWindow : UIWindow
{
	public static List<CustomPromoScreen> promos;

	private static UIPromoWindow instance;

	private static bool wasShownThisSession = false;

	public string URL;

	public UITexture TargetTexture;

	public UIButton btnClose;

	public UIButton btnAction;

	public UILabel LabelButton;

	public GameObject View;

	private static CustomPromoScreen currentPromo;

	private static List<CustomPromoScreen> shownPromos = new List<CustomPromoScreen>();

	public static void Show()
	{
		if (!wasShownThisSession && Entities.Instance.me.Level > 2)
		{
			LoadShownPromos();
			if (instance == null)
			{
				UIWindow.ClearWindows();
				instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/PromoWindow"), UIManager.Instance.transform).GetComponent<UIPromoWindow>();
				instance.Init();
				instance.View.SetActive(value: false);
			}
			instance.StartCoroutine(instance.GrabPromos());
			wasShownThisSession = true;
		}
	}

	protected override void Init()
	{
		base.Init();
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnAction.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnActionClick));
	}

	public IEnumerator GrabPromos()
	{
		using UnityWebRequest www = UnityWebRequest.Get(Main.WebServiceURL + "/Game/GetCustomScreens");
		string errorTitle = "Failed to load promos!";
		string friendlyMsg = "Unable to communicate with the server.";
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
		try
		{
			promos = JsonConvert.DeserializeObject<List<CustomPromoScreen>>(JsonConvert.DeserializeObject<APIResponse>(www.downloadHandler.text).Message);
		}
		catch (Exception ex)
		{
			customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
			friendlyMsg = "Unable to process response from the server.";
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, null, customContext, ex);
		}
		instance.LoadPromo();
	}

	public virtual void OnCloseClick(GameObject go)
	{
		Destroy();
	}

	protected override void Destroy()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnAction.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnActionClick));
		base.Destroy();
	}

	public void LoadPromo()
	{
		foreach (CustomPromoScreen c in shownPromos)
		{
			promos = promos.Where((CustomPromoScreen x) => x.ID != c.ID).ToList();
		}
		if (promos.Count == 0)
		{
			Destroy();
			return;
		}
		currentPromo = promos[0];
		instance.LabelButton.text = currentPromo.Label;
		currentPromo.LastShown = DateTime.Now;
		StartCoroutine(instance.LoadTexture());
	}

	public IEnumerator LoadTexture()
	{
		UnityWebRequest www = UnityWebRequestTexture.GetTexture(Main.APPLICATION_PATH + "/" + currentPromo.URLImage);
		using (www)
		{
			string errorTitle = "Failed to LoadTexture";
			string friendlyMsg = "Unable to communicate with the server.";
			string customContext = "URLImage: " + currentPromo.URLImage;
			yield return www.SendWebRequest();
			customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
			if (www.isHttpError)
			{
				ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, null, customContext);
			}
			else if (www.isNetworkError)
			{
				ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, null, customContext);
			}
			else if (www.error != null)
			{
				ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, null, customContext);
			}
			else
			{
				try
				{
					TargetTexture.mainTexture = DownloadHandlerTexture.GetContent(www);
				}
				catch (Exception ex)
				{
					ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, null, customContext);
				}
			}
			if (currentPromo.Action == "")
			{
				btnAction.gameObject.SetActive(value: false);
			}
			instance.View.SetActive(value: true);
			shownPromos.Add(currentPromo);
			try
			{
				PlayerPrefs.SetString("PromosLastShown", JsonConvert.SerializeObject(shownPromos));
			}
			catch (Exception ex2)
			{
				ErrorReporting.Instance.ReportError(errorTitle, "Unable to save shown promotions.", ex2.Message, null, customContext, ex2);
			}
		}
	}

	public void OnActionClick(GameObject go)
	{
		string action = currentPromo.Action;
		if (action[0] == '/')
		{
			string[] msgparts = action.Substring(1).Split(' ');
			switch (msgparts[0].ToLower())
			{
			case "join":
				if (msgparts.Length <= 1 || msgparts[1].Length <= 0)
				{
					break;
				}
				if (Entities.Instance.me.Level < currentPromo.MinLevel)
				{
					Confirmation.Show("Level", "This area is recommended for level " + currentPromo.MinLevel + " or higher.  Do you want to continue?", delegate(bool b)
					{
						if (b)
						{
							JoinArea(msgparts[1]);
						}
					});
					return;
				}
				JoinArea(msgparts[1]);
				break;
			case "www":
				if (msgparts.Length > 1 && msgparts[1].Length > 0)
				{
					Confirmation.OpenUrl(msgparts[1]);
				}
				break;
			case "iapstore":
				UIIAPStore.Show();
				break;
			case "shop":
				if (msgparts.Length > 1 && msgparts[1].Length > 0)
				{
					UIShop.LoadShop(int.Parse(msgparts[1]));
				}
				break;
			case "craftshop":
				if (msgparts.Length > 1 && msgparts[1].Length > 0)
				{
					UIMerge.Load(int.Parse(msgparts[1]));
				}
				break;
			}
		}
		Destroy();
	}

	public static void LoadShownPromos()
	{
		if (!PlayerPrefs.HasKey("PromosLastShown"))
		{
			return;
		}
		try
		{
			shownPromos = JsonConvert.DeserializeObject<List<CustomPromoScreen>>(PlayerPrefs.GetString("PromosLastShown"));
		}
		catch
		{
			Debug.LogError("Error Parsing PromosLastShown");
		}
		if (shownPromos.Count < 1)
		{
			return;
		}
		try
		{
			foreach (CustomPromoScreen shownPromo in shownPromos)
			{
				if ((DateTime.Now - shownPromo.LastShown.Value).TotalDays >= 1.0)
				{
					shownPromo.ID = -99;
				}
			}
			shownPromos = shownPromos.Where((CustomPromoScreen x) => x.ID != -99).ToList();
		}
		catch
		{
			Debug.LogError("Error culling shownPromos");
		}
	}

	private void JoinArea(string name)
	{
		Game.Instance.SendAreaJoinCommand(name);
	}
}
