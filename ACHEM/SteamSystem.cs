using System;
using System.Collections;
using Newtonsoft.Json;
using Steamworks;
using UnityEngine;
using UnityEngine.Networking;

public class SteamSystem : MonoBehaviour
{
	public delegate void _stxn(long stid, int userid, string id);

	protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;

	protected Callback<MicroTxnAuthorizationResponse_t> m_MicroTxnAuthorizationResponse;

	private uint m_pcbTicket;

	private byte[] m_Ticket;

	private HAuthTicket m_HAuthTicket;

	protected Callback<GetAuthSessionTicketResponse_t> m_GetAuthSessionTicketResponse;

	public static event _stxn _StartTransaction;

	private void Update()
	{
	}

	private void OnEnable()
	{
		if (SteamManager.Initialized)
		{
			m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
			m_MicroTxnAuthorizationResponse = Callback<MicroTxnAuthorizationResponse_t>.Create(MicroTxnAuthorizationResponse);
		}
		_StartTransaction += _starttransaction;
		Invoke("LoginWithSteam", 1f);
	}

	private void OnDisable()
	{
		_StartTransaction -= _starttransaction;
	}

	public static void StartTransaction(long stid, int userid, string id)
	{
		if (SteamSystem._StartTransaction != null)
		{
			SteamSystem._StartTransaction(stid, userid, id);
		}
	}

	private void _starttransaction(long stid, int userid, string id)
	{
		StartCoroutine(S_StartNewTransaction(stid, userid, id));
	}

	private IEnumerator S_StartNewTransaction(long stid, int userid, string id)
	{
		yield return null;
		string uri = SteamStore.GetWEBAPIURL + "StartNewTransaction?prodid=" + stid + "&steamid=" + id + "&userid=" + userid + "&webkey=Art789369!";
		using UnityWebRequest www = UnityWebRequest.Get(uri);
		yield return www.SendWebRequest();
		if (string.IsNullOrEmpty(www.error))
		{
			string text = www.downloadHandler.text.Replace("\\", "").Remove(0, 1);
			SteamStore.NewTxnResponse = JsonConvert.DeserializeObject<InitTxnResponse>(text.Remove(text.Length - 1, 1));
			if (SteamStore.NewTxnResponse.response.result != "OK")
			{
				StrategySteam.Instance.SteamPurchaseFailed(SteamStore.NewTxnResponse.response.error.errordesc);
				yield break;
			}
			SteamTxn txn = new SteamTxn(SteamStore.NewTxnResponse);
			StrategySteam.Instance.PurchaseSuccess(txn);
		}
		else
		{
			SteamStore.NewTxnResponse.response = new InitTxnR
			{
				error = new InitTxnResponseError
				{
					errorcode = -10,
					errordesc = www.error
				}
			};
			StrategySteam.Instance.SteamPurchaseFailed(SteamStore.NewTxnResponse.response.error.errordesc);
		}
	}

	private IEnumerator S_FinalizeTransaction(MicroTxnAuthorizationResponse_t pCallback)
	{
		yield return null;
		string text = "0";
		try
		{
			text = SteamUser.GetSteamID().ToString();
		}
		catch
		{
		}
		string uri = SteamStore.GetWEBAPIURL + "OnAuthorized?isAuthorized=" + pCallback.m_bAuthorized + "&appID=" + pCallback.m_unAppID + "&OrderID=" + pCallback.m_ulOrderID + "&webkey=Art789369!&userid=" + Session.MyPlayerData.UserID + "&steamid=" + text;
		using UnityWebRequest www = UnityWebRequest.Get(uri);
		yield return www.SendWebRequest();
		if (string.IsNullOrEmpty(www.error))
		{
			Debug.LogError(www.downloadHandler);
			SteamStore.FinalTxnResponse = JsonConvert.DeserializeObject<FinalizeTxnResponse>(www.downloadHandler.text.Replace("\\", " "));
			if (SteamStore.FinalTxnResponse.response.result == "OK")
			{
				SteamTxn txn = new SteamTxn(new InitTxnResponse
				{
					response = new InitTxnR
					{
						error = new InitTxnResponseError(),
						result = "OK",
						@params = new InitTxnResponseParams
						{
							orderid = SteamStore.FinalTxnResponse.response.@params.orderid,
							steamurl = "",
							transid = SteamStore.FinalTxnResponse.response.@params.transid
						}
					}
				});
				StrategySteam.Instance.ConsumeSuccess(txn);
			}
			else
			{
				string text2 = ((SteamStore.FinalTxnResponse.response.error.errorcode == 0) ? UnityWebRequest.UnEscapeURL(SteamStore.FinalTxnResponse.response.error.errordesc) : SteamStore.FinalTxnResponse.response.error.errordesc);
				Debug.LogError("Store Purchase Error Code: " + SteamStore.FinalTxnResponse.response.error.errorcode + " Message: " + text2);
				StrategySteam.Instance.OnPurchaseFailed("ERROR(" + SteamStore.FinalTxnResponse.response.error.errorcode + "): " + text2);
			}
		}
		else
		{
			SteamStore.FinalTxnResponse.response = new FinalizeTxnR
			{
				error = new InitTxnResponseError
				{
					errorcode = -10,
					errordesc = www.error
				}
			};
			StrategySteam.Instance.OnPurchaseFailed("ERROR(" + SteamStore.FinalTxnResponse.response.error.errorcode + ")" + SteamStore.FinalTxnResponse.response.error.errordesc);
			Debug.LogError("<===== consume product error: " + www.url);
		}
	}

	private void MicroTxnAuthorizationResponse(MicroTxnAuthorizationResponse_t pCallback)
	{
		StartCoroutine("S_FinalizeTransaction", pCallback);
	}

	private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
	{
		_ = pCallback.m_bActive;
	}

	public void LoginWithSteam()
	{
		m_GetAuthSessionTicketResponse = Callback<GetAuthSessionTicketResponse_t>.Create(OnGetAuthSessionTicketResponse);
		m_Ticket = new byte[1024];
		m_HAuthTicket = SteamUser.GetAuthSessionTicket(m_Ticket, 1024, out m_pcbTicket);
	}

	public string ByteArrayToString(byte[] ba)
	{
		return BitConverter.ToString(ba).Replace("-", "");
	}

	private void OnGetAuthSessionTicketResponse(GetAuthSessionTicketResponse_t pCallback)
	{
		if (pCallback.m_eResult == EResult.k_EResultOK)
		{
			byte[] array = new byte[m_pcbTicket];
			Array.Copy(m_Ticket, array, array.Length);
			Debug.LogError(SteamUser.GetSteamID().ToString() + " " + ByteArrayToString(array));
		}
		else
		{
			int eResult = (int)pCallback.m_eResult;
			Debug.LogError("STEAM Session error. Code: " + eResult);
		}
	}
}
