using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ErrorReporting : MonoBehaviour
{
	private static List<ErrorObject> sessionErrors = new List<ErrorObject>();

	private static List<ErrorObject> unprocessedErrors = new List<ErrorObject>();

	private static bool processingList = false;

	private static ErrorReporting instance;

	public static ErrorReporting Instance => instance;

	public void Awake()
	{
		instance = this;
	}

	private void Update()
	{
		if (unprocessedErrors.Count > 0 && !processingList)
		{
			processingList = true;
			List<ErrorObject> listClone = new List<ErrorObject>(unprocessedErrors);
			unprocessedErrors.Clear();
			StartCoroutine(ProcessList(listClone));
		}
	}

	private IEnumerator ProcessList(List<ErrorObject> listClone)
	{
		int UserID = 0;
		int CharID = 0;
		if (Session.MyPlayerData != null && Session.MyPlayerData.UserID > 0)
		{
			UserID = Session.MyPlayerData.UserID;
		}
		else if (Session.Account != null && Session.Account.UserID > 0)
		{
			UserID = Session.Account.UserID;
		}
		if (Session.MyPlayerData != null && Session.MyPlayerData.ID > 0)
		{
			CharID = Session.MyPlayerData.ID;
		}
		else if (Session.Account != null && Session.Account.chars.Count > 0)
		{
			CharID = Session.Account.chars[0].ID;
		}
		foreach (ErrorObject item in listClone)
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("UserID", UserID);
			wWWForm.AddField("CharID", CharID);
			wWWForm.AddField("ClientID", Main.ClientID);
			wWWForm.AddField("DevBuild", Main.DevBuild);
			wWWForm.AddField("ClientDisplayVersion", Main.ClientDisplayVersion);
			wWWForm.AddField("DeviceUniqueIdentifier", SystemInfo.deviceUniqueIdentifier);
			wWWForm.AddField("Title", item.Title);
			wWWForm.AddField("StackTrace", item.StackTrace);
			wWWForm.AddField("Message", item.Message);
			wWWForm.AddField("Context", item.Context);
			wWWForm.AddField("Form", item.Form);
			using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL + "/Game/RecordErrorReport", wWWForm);
			yield return www.SendWebRequest();
		}
		processingList = false;
	}

	private void ReportErrorToDB_WebAPI(string errorTitle, string errorMessage, string stackTrace, string context, string form)
	{
		if (!Main.EnvironmentsInitialized)
		{
			Debug.Log("Environments must be initialized to Record to the database");
			return;
		}
		ErrorObject errorObject = new ErrorObject(errorTitle, errorMessage, stackTrace, context, form);
		if (IsNewErrorThisSession(errorObject))
		{
			sessionErrors.Add(errorObject);
			unprocessedErrors.Add(errorObject);
		}
	}

	public void ReportHttpError(string errorTitle, string friendlyMessage, string technicalMessage, long httpResponseCode, WWWForm form = null, string customContext = "", Exception e = null, bool showMessageBox = true, Action messageBoxCallback = null, MessageBoxWithDetails.Type type = MessageBoxWithDetails.Type.Status)
	{
		technicalMessage = "Http Response Code: " + httpResponseCode + "\nHttp Error: " + technicalMessage;
		Instance.ReportError(errorTitle, friendlyMessage, technicalMessage, form, customContext, e, showMessageBox, messageBoxCallback, type);
	}

	public void ReportNetworkError(string errorTitle, string friendlyMessage, string technicalMessage, WWWForm form = null, string customContext = "", Exception e = null, bool showMessageBox = true, Action messageBoxCallback = null, MessageBoxWithDetails.Type type = MessageBoxWithDetails.Type.Status)
	{
		technicalMessage = "Network Error: " + technicalMessage;
		Instance.ReportError(errorTitle, friendlyMessage, technicalMessage, form, customContext, e, showMessageBox, messageBoxCallback, type);
	}

	public void ReportError(string errorTitle, string friendlyMessage, string technicalMessage, WWWForm form = null, string customContext = "", Exception e = null, bool showMessageBox = true, Action messageBoxCallback = null, MessageBoxWithDetails.Type type = MessageBoxWithDetails.Type.Status)
	{
		string text = ((customContext != "") ? ("INFO\n" + customContext) : "");
		string text2 = ((form != null) ? new string(Encoding.UTF8.GetChars(form.data)) : "");
		string text3 = ((form != null) ? ("FORM:\n" + text2) : "");
		string text4 = ((technicalMessage != null) ? ("TECHNICAL INFO\n" + technicalMessage) : "");
		string text5 = errorTitle + "\n\n";
		text5 += ((friendlyMessage != "") ? (friendlyMessage + "\n\n") : "");
		text5 += ((text4 != "") ? (text4 + "\n\n") : "");
		text5 += ((text != "") ? (text + "\n\n") : "");
		text5 += ((text3 != "") ? text3 : "");
		if (e == null)
		{
			e = new Exception(text5);
		}
		Debug.LogException(new Exception(text5, e));
		string text6 = "";
		text6 += ((friendlyMessage != "") ? (friendlyMessage + "\n\n") : "");
		text6 += ((text4 != "") ? (text4 + "\n\n") : "");
		if (showMessageBox)
		{
			MessageBoxWithDetails.Show(errorTitle, friendlyMessage, type, text6, messageBoxCallback);
		}
		if (Game.Instance != null)
		{
			Game.Instance.ReportErrorToDB_Socket(errorTitle, friendlyMessage + " - " + technicalMessage, e.StackTrace, customContext, text2);
		}
		ReportErrorToDB_WebAPI(errorTitle, friendlyMessage + " - " + technicalMessage, e.StackTrace, customContext, text2);
	}

	public RequestReportError GetRequestReportError(string errorTitle, string errorMessage, string stacktrace, string context, string form)
	{
		int userID = 0;
		int charID = 0;
		if (Session.MyPlayerData != null && Session.MyPlayerData.UserID > 0)
		{
			userID = Session.MyPlayerData.UserID;
		}
		else if (Session.Account != null && Session.Account.UserID > 0)
		{
			userID = Session.Account.UserID;
		}
		if (Session.MyPlayerData != null && Session.MyPlayerData.ID > 0)
		{
			charID = Session.MyPlayerData.ID;
		}
		else if (Session.Account != null && Session.Account.chars.Count > 0)
		{
			charID = Session.Account.chars[0].ID;
		}
		RequestReportError requestReportError = new RequestReportError();
		requestReportError.UserID = userID;
		requestReportError.CharID = charID;
		requestReportError.ClientID = Main.ClientID;
		requestReportError.DevBuild = Main.DevBuild;
		requestReportError.ClientDisplayVersion = Main.ClientDisplayVersion;
		requestReportError.DeviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
		if (errorTitle != null)
		{
			requestReportError.Title = ((errorTitle.Length > 150) ? errorTitle.Substring(0, 150) : errorTitle);
		}
		if (errorMessage != null)
		{
			requestReportError.Message = ((errorMessage.Length > 250) ? errorMessage.Substring(0, 250) : errorMessage);
		}
		if (stacktrace != null)
		{
			requestReportError.StackTrace = ((stacktrace.Length > 250) ? stacktrace.Substring(0, 250) : stacktrace);
		}
		if (context != null)
		{
			requestReportError.Context = ((context.Length > 200) ? context.Substring(0, 200) : context);
		}
		if (form != null)
		{
			requestReportError.Form = ((form.Length > 150) ? form.Substring(0, 150) : form);
		}
		return requestReportError;
	}

	private bool IsNewErrorThisSession(ErrorObject error)
	{
		return sessionErrors.Where((ErrorObject e) => e.StackTrace == error.StackTrace && e.Message == error.Message).FirstOrDefault() == null;
	}
}
