using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class UIAdvanced : MonoBehaviour
{
	public UIScrollView scrollView;

	public UIButton deleteAccount;

	public GameObject accountSectionTitle;

	public void DeleteAccountFirstClick()
	{
		Confirmation.Show("Account Deletion", "Deleting your account is permanent and ALL characters, purchases, and data will be lost. This cannot be reverted. Are you sure you want to delete your account?", DeleteAccountConfirmation);
	}

	private void DeleteAccountConfirmation(bool a)
	{
		if (a)
		{
			if (PlayerPrefs.GetInt("LastLoginMethod") == 1)
			{
				string text = Util.Base64Decode(PlayerPrefs.GetString("PASSWORDENCODE"));
				ConfirmationTextField.Show("Confirmation", "We are sad to see you go. Type your password below to permanently delete your account and its information.", onDeleteConfirmed, text, " ", password: true, text.Length);
			}
			else
			{
				ConfirmationTextField.Show("Confirmation", "We are sad to see you go. Type DELETE below to permanently delete your account and its information.", onDeleteConfirmed, "DELETE");
			}
		}
	}

	private void onDeleteConfirmed(bool b)
	{
		if (b)
		{
			deleteAccount.gameObject.SetActive(value: false);
			accountSectionTitle.SetActive(value: false);
			Loader.show("Good luck and keep Battling On!", 0f);
			StartCoroutine(DeletionWaiter());
		}
	}

	private IEnumerator DeletionWaiter()
	{
		yield return StartCoroutine(DeleteUser());
	}

	private IEnumerator DeleteUser()
	{
		WebApiRequestUserDelete request = new WebApiRequestUserDelete(Session.Account.UserID, Session.Account.strToken);
		using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL + "/Game/UserDelete", request.GetWWWForm());
		string errorTitle = "Failed to delete user";
		string friendlyMsg = "Remote server could not be reached!";
		string customContext = "User ID: " + Session.Account.UserID;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		if (www.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, request.GetWWWForm(), customContext);
			yield break;
		}
		if (www.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, request.GetWWWForm(), customContext);
			yield break;
		}
		if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, request.GetWWWForm(), customContext);
			yield break;
		}
		Debug.Log("WWW Ok!: " + www.downloadHandler.text);
		AEC.getInstance().sendRequest(new RequestDisconnect());
	}
}
