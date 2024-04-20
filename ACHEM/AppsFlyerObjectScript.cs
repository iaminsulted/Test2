using AppsFlyerSDK;
using UnityEngine;

public class AppsFlyerObjectScript : MonoBehaviour, IAppsFlyerConversionData
{
	public string devKey;

	public string appID;

	public bool isDebug;

	public bool getConversionData;

	private void Start()
	{
		AppsFlyer.setIsDebug(isDebug);
		AppsFlyer.initSDK(devKey, appID, getConversionData ? this : null);
		AppsFlyer.startSDK();
	}

	private void Update()
	{
	}

	public void onConversionDataSuccess(string conversionData)
	{
		AppsFlyer.AFLog("didReceiveConversionData", conversionData);
		AppsFlyer.CallbackStringToDictionary(conversionData);
	}

	public void onConversionDataFail(string error)
	{
		AppsFlyer.AFLog("didReceiveConversionDataWithError", error);
	}

	public void onAppOpenAttribution(string attributionData)
	{
		AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
		AppsFlyer.CallbackStringToDictionary(attributionData);
	}

	public void onAppOpenAttributionFailure(string error)
	{
		AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
	}
}
