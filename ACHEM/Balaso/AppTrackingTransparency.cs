using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Balaso;

public static class AppTrackingTransparency
{
	public enum AuthorizationStatus
	{
		NOT_DETERMINED,
		RESTRICTED,
		DENIED,
		AUTHORIZED
	}

	private static TaskScheduler currentSynchronizationContext;

	public static Action<AuthorizationStatus> OnAuthorizationRequestDone;

	public static AuthorizationStatus TrackingAuthorizationStatus => AuthorizationStatus.NOT_DETERMINED;

	static AppTrackingTransparency()
	{
		currentSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext();
	}

	public static void UpdateConversionValue(int value)
	{
	}

	public static void RegisterAppForAdNetworkAttribution()
	{
	}

	public static void RequestTrackingAuthorization()
	{
		Debug.Log($"Platform '{Application.platform}' not supported");
	}

	public static string IdentifierForAdvertising()
	{
		Debug.Log($"Platform '{Application.platform}' not supported");
		return null;
	}
}
