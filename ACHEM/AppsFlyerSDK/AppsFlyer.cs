using System.Collections.Generic;
using AFMiniJSON;
using UnityEngine;

namespace AppsFlyerSDK;

public class AppsFlyer : MonoBehaviour
{
	public static readonly string kAppsFlyerPluginVersion = "5.4.2";

	public static void initSDK(string devKey, string appID)
	{
		initSDK(devKey, appID, null);
	}

	public static void initSDK(string devKey, string appID, MonoBehaviour gameObject)
	{
	}

	public static void startSDK()
	{
	}

	public static void sendEvent(string eventName, Dictionary<string, string> eventValues)
	{
	}

	public static void stopSDK(bool isSDKStopped)
	{
	}

	public static bool isSDKStopped()
	{
		return false;
	}

	public static string getSdkVersion()
	{
		return "";
	}

	public static void setIsDebug(bool shouldEnable)
	{
	}

	public static void setCustomerUserId(string id)
	{
	}

	public static void setAppInviteOneLinkID(string oneLinkId)
	{
	}

	public static void setAdditionalData(Dictionary<string, string> customData)
	{
	}

	public static void setResolveDeepLinkURLs(params string[] urls)
	{
	}

	public static void setOneLinkCustomDomain(params string[] domains)
	{
	}

	public static void setCurrencyCode(string currencyCode)
	{
	}

	public static void recordLocation(double latitude, double longitude)
	{
	}

	public static void anonymizeUser(bool shouldAnonymizeUser)
	{
	}

	public static string getAppsFlyerId()
	{
		return "";
	}

	public static void setMinTimeBetweenSessions(int seconds)
	{
	}

	public static void setHost(string hostPrefixName, string hostName)
	{
	}

	public static void setUserEmails(EmailCryptType cryptMethod, params string[] emails)
	{
	}

	public static void setPhoneNumber(string phoneNumber)
	{
	}

	public static void setSharingFilterForAllPartners()
	{
	}

	public static void setSharingFilter(params string[] partners)
	{
	}

	public static void getConversionData(string objectName)
	{
	}

	public static void attributeAndOpenStore(string appID, string campaign, Dictionary<string, string> userParams, MonoBehaviour gameObject)
	{
	}

	public static void recordCrossPromoteImpression(string appID, string campaign, Dictionary<string, string> parameters)
	{
	}

	public static void generateUserInviteLink(Dictionary<string, string> parameters, MonoBehaviour gameObject)
	{
	}

	public static Dictionary<string, object> CallbackStringToDictionary(string str)
	{
		return Json.Deserialize(str) as Dictionary<string, object>;
	}

	public static void AFLog(string methodName, string str)
	{
		Debug.Log($"AppsFlyer_Unity_v{kAppsFlyerPluginVersion} {methodName} called with {str}");
	}
}
