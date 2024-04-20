using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserTracking : MonoBehaviour
{
	public enum UserEvent
	{
		Login_Success = 1,
		Login_TemplatesDownloaded = 2,
		Login_BundlesDownloaded = 3,
		CharCreate_StartedOk = 4,
		CharCreate_BaseItemsOk = 5,
		CharCreate_InitComplete = 6,
		CharCreate_CharLoadedOk = 7,
		CharCreate_CreateClicked = 8,
		CharCreate_CallWebApi = 9,
		CharCreate_CreateOk = 10,
		CharCreate_DeserializeOk = 11,
		CharCreate_AttemptLoadCharSelect = 12,
		CharSelect_StartedOk = 20,
		CharSelect_PlayerObjectFound = 21,
		CharSelect_PlayerAssetControllerLoadStarted = 22,
		CharSelect_LoadServerListStarted = 23,
		CharSelect_LoadServerListCompleted = 24,
		CharSelect_InitServerListOk = 25,
		CharSelect_PlayClicked = 26,
		CharSelect_ConnectOkAttemptLoadSceneGame = 27,
		Game_AwakeOk = 30,
		Game_StartOk = 31,
		Game_AttemptMoveToNextCell = 32,
		Game_LoadMapBundleOk = 33,
		Game_InstantiateMapOk = 34,
		Game_LoadSoundBundleOk = 35,
		Game_InstantiateSoundOk = 36,
		Game_TransferPadsOk = 37,
		Game_MachinesOk = 38,
		Game_NpcsOk = 39,
		Game_PlayersOk = 40,
		Game_AttemptOnCellLoaded = 41,
		Game_OnCellLoadedOk = 42,
		CharCreate_ZoomedIn = 50,
		CharCreate_ClientError_NameOffensive = 51,
		CharCreate_ClientError_NameLength = 52,
		CharCreate_ServerError_CouldNotValidateUserAccount = 55,
		CharCreate_ServerError_AlreadyHaveCharacterOnAccount = 56,
		CharCreate_ServerError_ContainsBadWord = 57,
		CharCreate_ServerError_NameIsTaken = 58,
		CharCreate_ServerError_Other = 59,
		CharSelect_Error_FailedToLoadedServerList = 60
	}

	public static bool Active = false;

	private static bool processingList = false;

	private static List<WWWForm> wwwList = new List<WWWForm>();

	public static UserTracking Instance { get; private set; }

	public void Awake()
	{
		Instance = this;
	}

	private void Update()
	{
		if (wwwList.Count > 0 && !processingList)
		{
			processingList = true;
			List<WWWForm> listClone = new List<WWWForm>(wwwList);
			wwwList.Clear();
			StartCoroutine(ProcessList(listClone));
		}
	}

	private IEnumerator ProcessList(List<WWWForm> listClone)
	{
		foreach (WWWForm item in listClone)
		{
			using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL + "/Game/RecordUserEvent", item);
			yield return www.SendWebRequest();
		}
		processingList = false;
	}

	public void RecordUserEvent(UserEvent userEvent)
	{
		if (!EventComplete(userEvent) && Session.Account != null)
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("UserID", Session.Account.UserID);
			wWWForm.AddField("DeviceUniqueIdentifier", SystemInfo.deviceUniqueIdentifier);
			wWWForm.AddField("UserEvent", (int)userEvent);
			wwwList.Add(wWWForm);
		}
	}

	private bool EventComplete(UserEvent userEvent)
	{
		return Util.BitGet(Session.Account.EventBitTracker, (int)userEvent);
	}
}
