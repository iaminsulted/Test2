using System.Collections.Generic;
using UnityEngine;

public class CutsceneCinemaManager : MonoBehaviour
{
	public List<CutsceneCinema> _cutscenes;

	public int CutsceneCount
	{
		get
		{
			if (_cutscenes != null)
			{
				return _cutscenes.Count;
			}
			return 0;
		}
	}

	public void GetAllCutscenes()
	{
		_cutscenes = new List<CutsceneCinema>();
		Dictionary<string, CutsceneCinema> dictionary = new Dictionary<string, CutsceneCinema>();
		CutsceneCinema[] array = Object.FindObjectsOfType<CutsceneCinema>();
		_cutscenes.AddRange(array);
		CutsceneCinema[] array2 = array;
		foreach (CutsceneCinema cutsceneCinema in array2)
		{
			dictionary.Add(cutsceneCinema.CutsceneName, cutsceneCinema);
			cutsceneCinema.SetActorsAndGroups();
		}
	}

	public void PrepForBundle()
	{
		foreach (CutsceneCinema cutscene in _cutscenes)
		{
			cutscene.PrepForBundle();
		}
	}
}
