using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AQ3D.DialogueSystem;
using CinemaDirector;
using UnityEngine;

[Serializable]
[ExecuteInEditMode]
public class CutsceneCinema : MonoBehaviour
{
	public Cutscene ActiveCutscene;

	private GameObject _TempCutscene;

	public string CutsceneName;

	public int CameraCullingOption;

	public List<TrackGroupSet> CinemaGroups;

	public List<NPCMapSet> NPCData;

	public List<DialogueCharacter> AllCharacters;

	public List<GameObject> CamerasList;

	private bool isplaying;

	public Action CinematicComplete;

	private static Dictionary<string, CutsceneCinema> Map = new Dictionary<string, CutsceneCinema>();

	public List<AnimationMap> animationMaps = new List<AnimationMap>();

	private void Awake()
	{
		Map[CutsceneName] = this;
	}

	private void OnDestroy()
	{
		Map.Remove(CutsceneName);
	}

	public static CutsceneCinema FindByName(string name)
	{
		if (Map.ContainsKey(name))
		{
			return Map[name];
		}
		return null;
	}

	public void PrepForBundle()
	{
		foreach (GameObject cameras in CamerasList)
		{
			cameras.SetActive(value: false);
		}
		ActiveCutscene.gameObject.SetActive(value: false);
	}

	public void SetActorsAndGroups()
	{
		CinemaGroups = new List<TrackGroupSet>();
		CamerasList = (from p in GetComponentsInChildren<Camera>(includeInactive: true)
			select p.gameObject).ToList();
		NPCData = new List<NPCMapSet>();
		AllCharacters = new List<DialogueCharacter>();
		TrackGroup[] trackGroups = ActiveCutscene.TrackGroups;
		foreach (TrackGroup trackGroup in trackGroups)
		{
			int instanceID = trackGroup.gameObject.GetInstanceID();
			CinemaGroups.Add(new TrackGroupSet
			{
				id = instanceID,
				Group = trackGroup
			});
			GameObject go = null;
			if (trackGroup is MultiActorTrackGroup)
			{
				foreach (Transform actor in ((MultiActorTrackGroup)trackGroup).Actors)
				{
					go = actor.gameObject;
					SetGameObject(instanceID, go, ismulti: true);
				}
			}
			else if (trackGroup is ActorTrackGroup && ((ActorTrackGroup)trackGroup).Actor.gameObject.GetComponent<CutsceneNPC>() != null)
			{
				SetGameObject(instanceID, go);
			}
		}
	}

	private void SetGameObject(int instanceid, GameObject go, bool ismulti = false)
	{
		if (!(go != null))
		{
			return;
		}
		int localid = go.GetInstanceID();
		int num = -1;
		try
		{
			num = NPCData.FindIndex((NPCMapSet x) => x.instanceid == localid);
		}
		catch
		{
		}
		if (num > -1)
		{
			NPCData[num].Info.NPCID = go.GetComponent<CutsceneNPC>().NPCID;
			if (NPCData[num].Info.GroupNumbers == null)
			{
				NPCData[num].Info.GroupNumbers = new List<float>();
			}
			if (!NPCData[num].Info.GroupNumbers.Contains(instanceid))
			{
				NPCData[num].Info.GroupNumbers.Add(instanceid);
			}
			if (ismulti)
			{
				AllCharacters.Add(go.GetComponent<CutsceneNPC>().CreateCharacterData(CutsceneName, NPCData[num].Info.NPCID, localid, ismulti));
			}
		}
		else
		{
			NPCMap nPCMap = new NPCMap();
			nPCMap.NPCID = go.GetComponent<CutsceneNPC>().NPCID;
			nPCMap.GroupNumbers = new List<float>();
			nPCMap.GroupNumbers.Add(instanceid);
			NPCData.Add(new NPCMapSet
			{
				instanceid = localid,
				Info = nPCMap
			});
			AllCharacters.Add(go.GetComponent<CutsceneNPC>().CreateCharacterData(CutsceneName, nPCMap.NPCID, localid, ismulti));
		}
	}

	public void GetAllCharacters(string name, Action<List<DialogueCharacter>> charAction)
	{
		if (charAction != null && name == CutsceneName)
		{
			charAction(AllCharacters);
		}
	}

	public void PlayCutscene()
	{
		int cullingMask = 0;
		if (_TempCutscene != null)
		{
			UnityEngine.Object.Destroy(_TempCutscene);
		}
		try
		{
			cullingMask = Layers.GetCutsceneMask(CameraCullingOption);
		}
		catch
		{
			Debug.LogError("Layers class not there");
		}
		foreach (GameObject cameras in CamerasList)
		{
			Camera component = cameras.GetComponent<Camera>();
			component.cullingMask = cullingMask;
			component.farClipPlane = Game.Instance.CurrentCell.CameraFarPlane;
		}
		_TempCutscene = UnityEngine.Object.Instantiate(ActiveCutscene.gameObject, ActiveCutscene.transform.position, ActiveCutscene.transform.rotation, ActiveCutscene.gameObject.transform.parent);
		_TempCutscene.transform.parent = ActiveCutscene.gameObject.transform.parent;
		_TempCutscene.SetActive(value: true);
		_TempCutscene.GetComponent<Cutscene>().CutsceneFinished += ActiveCutscene_CutsceneFinished;
		isplaying = true;
		_TempCutscene.GetComponent<Cutscene>().Play();
	}

	private void ActiveCutscene_CutsceneFinished(object sender, CutsceneEventArgs e)
	{
		if (!ActiveCutscene.IsLooping || !isplaying)
		{
			Cleanup();
			_TempCutscene.GetComponent<Cutscene>().CutsceneFinished -= ActiveCutscene_CutsceneFinished;
			_TempCutscene.SetActive(value: false);
			isplaying = false;
			if (CinematicComplete != null)
			{
				CinematicComplete();
			}
		}
	}

	public void StopCutscene()
	{
		isplaying = false;
		if (_TempCutscene == null)
		{
			ActiveCutscene.Stop();
		}
		else
		{
			_TempCutscene.GetComponent<Cutscene>().Stop();
		}
		Cleanup();
	}

	public void Cleanup()
	{
	}

	private IEnumerator SetCharacter(CharacterTrackGroup tr, GameObject go)
	{
		float timeSinceLevelLoad = Time.timeSinceLevelLoad;
		float timerend = timeSinceLevelLoad + 30f;
		if (!(go.GetComponent<Animator>() == null))
		{
			yield break;
		}
		if (go.GetComponent<PlayerAssetController>() != null)
		{
			bool found = true;
			while (go.GetComponent<PlayerAssetController>().currentAsset == null)
			{
				if (Time.timeSinceLevelLoad >= timerend)
				{
					found = false;
					break;
				}
				yield return null;
			}
			if (found)
			{
				tr.Actor = go.GetComponent<PlayerAssetController>().currentAsset.transform;
			}
			else
			{
				tr.Actor = go.transform;
			}
		}
		else if (go.GetComponent<NPCAssetController>() != null)
		{
			bool found = true;
			while (go.GetComponent<NPCAssetController>().currentAsset == null)
			{
				if (Time.timeSinceLevelLoad >= timerend)
				{
					found = false;
					break;
				}
				yield return null;
			}
			if (found)
			{
				tr.Actor = go.GetComponent<NPCAssetController>().currentAsset.transform;
			}
			else
			{
				tr.Actor = go.transform;
			}
		}
		else
		{
			if (!(go.GetComponent<AssetController>() != null))
			{
				yield break;
			}
			bool found = true;
			while (go.GetComponent<AssetController>().currentAsset == null)
			{
				if (Time.timeSinceLevelLoad >= timerend)
				{
					found = false;
					break;
				}
				yield return null;
			}
			if (found)
			{
				tr.Actor = go.GetComponent<AssetController>().currentAsset.transform;
			}
			else
			{
				tr.Actor = go.transform;
			}
		}
	}

	public void AddCharacterToGroup(DialogueCharacter dchar, GameObject go)
	{
		int num = NPCData.FindIndex((NPCMapSet x) => x.instanceid == (int)dchar.OffsetY);
		if (num <= -1)
		{
			return;
		}
		Transform transform = go.GetComponent<AssetController>().currentAsset.transform;
		foreach (float gnumber in NPCData[num].Info.GroupNumbers)
		{
			int num2 = CinemaGroups.FindIndex((TrackGroupSet x) => x.id == (int)gnumber);
			if (num2 <= -1)
			{
				continue;
			}
			TrackGroup group = CinemaGroups[num2].Group;
			if (group is ActorTrackGroup)
			{
				((ActorTrackGroup)group).Actor = transform;
			}
			else if (group is MultiActorTrackGroup)
			{
				MultiActorTrackGroup multiActorTrackGroup = (MultiActorTrackGroup)group;
				if (multiActorTrackGroup.Actors == null)
				{
					multiActorTrackGroup.Actors = new List<Transform>();
				}
				multiActorTrackGroup.Actors.Add(transform);
			}
			else if (group is CharacterTrackGroup)
			{
				((CharacterTrackGroup)group).Actor = transform;
			}
		}
		SetAnimatorState(go.GetComponentInChildren<Animator>(), animationMaps, dchar.NPCID);
	}

	private void SetAnimatorState(Animator animator, List<AnimationMap> animationMaps, int currentNPCID)
	{
		if (!(animator != null))
		{
			return;
		}
		RuntimeAnimatorController runtimeAnimatorController = animator.runtimeAnimatorController;
		AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController();
		if (animator.runtimeAnimatorController.GetType() == typeof(AnimatorOverrideController))
		{
			animatorOverrideController = UnityEngine.Object.Instantiate(runtimeAnimatorController) as AnimatorOverrideController;
		}
		else
		{
			animatorOverrideController.runtimeAnimatorController = runtimeAnimatorController;
		}
		foreach (AnimationMap animationMap in animationMaps)
		{
			if (currentNPCID == animationMap.NPCID)
			{
				animatorOverrideController[animationMap.originalClipName] = animationMap.clip;
			}
		}
		animator.runtimeAnimatorController = animatorOverrideController;
	}

	private bool HasAnimatorState(Animator animator, string animState)
	{
		if (animator == null)
		{
			return false;
		}
		for (int i = 0; i < animator.layerCount; i++)
		{
			if (animator.HasState(i, Animator.StringToHash(animState)))
			{
				return true;
			}
		}
		return false;
	}
}
