using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SavedLightProbeData : ScriptableObject
{
	public List<probedata> ProbeData;

	public void CreateProbeData(int id, string name, LightProbes probes)
	{
		int num = ProbeData.FindIndex((probedata x) => x.id == id);
		if (num == -1)
		{
			ProbeData.Add(new probedata
			{
				id = id,
				name = name,
				bakedProbes = probes,
				Count = 0
			});
		}
		else
		{
			ProbeData[num] = new probedata
			{
				id = id,
				name = name,
				bakedProbes = probes,
				Count = 0
			};
		}
	}

	public void InsertProbes(LightProbes bakedProbes, int id)
	{
		for (int i = 0; i < ProbeData.Count; i++)
		{
			if (ProbeData[i].id == id)
			{
				ProbeData[i].bakedProbes = bakedProbes;
			}
		}
	}

	public LightProbes bakedProbes(int id)
	{
		for (int i = 0; i < ProbeData.Count; i++)
		{
			if (ProbeData[i].id == id)
			{
				return ProbeData[i].bakedProbes;
			}
		}
		return null;
	}
}
