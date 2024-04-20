using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA NPCIA")]
public class CTANPCIACore : ClientTriggerActionCore
{
	public string Title;

	public Transform CameraSpot;

	public Transform FocusSpot;

	public List<NPCIA> Apops = new List<NPCIA>();

	public List<int> ApopIds = new List<int>();

	private bool loaded;

	private bool executed;

	public event Action<NPCIA> ApopsLoaded;

	private void Start()
	{
		if (ApopIds != null && ApopIds.Count > 0)
		{
			ApopDownloader.GetApops(ApopIds, Instance_LoadEnd);
		}
	}

	protected override void OnExecute()
	{
		if (Apops != null && Apops.Count > 0)
		{
			UINPCDialog.Load(Apops, Title, CameraSpot, FocusSpot);
		}
		if (!loaded)
		{
			executed = true;
		}
	}

	private void Instance_LoadEnd(List<NPCIA> loadedApops)
	{
		if (loadedApops == null || loadedApops.Count == 0)
		{
			string text = string.Join(",", ApopIds.Select((int x) => x.ToString()).ToArray());
			Debug.LogError("Fix in admin. Apops not found: " + text);
			return;
		}
		NPCIA apop = ApopMap.GetApop(ApopIds[0]);
		if (apop != null && apop == loadedApops[0])
		{
			loaded = true;
			Apops = loadedApops;
			if (this.ApopsLoaded != null)
			{
				this.ApopsLoaded(apop);
			}
			if (executed)
			{
				OnExecute();
			}
		}
	}
}
