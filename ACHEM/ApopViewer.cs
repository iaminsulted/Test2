using System.Collections.Generic;
using UnityEngine;

public static class ApopViewer
{
	private static int requestedApopID;

	public static void Show(int apopID)
	{
		requestedApopID = apopID;
		NPCIA apop = ApopMap.GetApop(requestedApopID);
		if (apop == null)
		{
			ApopDownloader.GetApops(new List<int> { requestedApopID }, OnLoadEndHandler);
		}
		else
		{
			LoadApop(apop);
		}
	}

	private static void OnClearWindowsHandler()
	{
		UIWindow.OnClearWindows -= OnClearWindowsHandler;
		UIWindow.ClearWindows();
	}

	private static void OnLoadEndHandler(List<NPCIA> loadedApops)
	{
		if (loadedApops == null || loadedApops.Count == 0)
		{
			Debug.LogError("Fix in admin. Apop not found: " + requestedApopID);
		}
		else if (loadedApops[0].ID == requestedApopID)
		{
			NPCIA apop = ApopMap.GetApop(requestedApopID);
			if (apop != null)
			{
				ApopDownloader.LoadEnd -= OnLoadEndHandler;
				LoadApop(apop);
			}
		}
	}

	private static void LoadApop(NPCIA apopToOpen)
	{
		UINPCDialog.Load(new List<NPCIA> { apopToOpen }, apopToOpen.Title, null, null, clearWindows: false);
	}
}
