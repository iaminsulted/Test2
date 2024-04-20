using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AudioManager
{
	private static Queue<GameObject> AudioObjectsQueue;

	private static GameObject backgroundAudioObject;

	private static GameObject audioPrefab;

	private static GameObject noReverbPrefab;

	private static GameObject bossPrefab;

	private static GameObject audioSources;

	private static float thresholdDistance = 50f;

	private static float closeThresholdDistance = 15f;

	private static float shortestClipPercent = 0.3f;

	private static int maxCount = 10;

	private static int greetingPrev = -1;

	private static int farewellPrev = -1;

	private static Dictionary<int, AudioPoolObject> entityAudioLoops;

	private static List<AudioSource> AudioObjectsInUse;

	public static SFXPlayer gameSFXPlayer;

	public static void AddToAudioQueue(GameObject newObject)
	{
		newObject.SetActive(value: false);
		AudioObjectsQueue.Enqueue(newObject);
		if (AudioObjectsInUse.Contains(newObject.GetComponent<AudioSource>()))
		{
			AudioObjectsInUse.Remove(newObject.GetComponent<AudioSource>());
		}
	}

	private static GameObject RemoveFromAudioQueue()
	{
		GameObject gameObject = AudioObjectsQueue.Dequeue();
		gameObject.SetActive(value: true);
		AudioObjectsInUse.Add(gameObject.GetComponent<AudioSource>());
		return gameObject;
	}

	public static void Init(SFXPlayer sfxPlayer)
	{
		entityAudioLoops = new Dictionary<int, AudioPoolObject>();
		audioSources = new GameObject("Audio Sources");
		audioPrefab = Resources.Load("Audio Objects/SFX Object") as GameObject;
		bossPrefab = Resources.Load("Audio Objects/BossProfile") as GameObject;
		noReverbPrefab = Resources.Load("Audio Objects/NoReverbProfile") as GameObject;
		AudioObjectsQueue = new Queue<GameObject>();
		AudioObjectsInUse = new List<AudioSource>();
		for (int i = 0; i < maxCount; i++)
		{
			AddToAudioQueue(UnityEngine.Object.Instantiate(audioPrefab, Vector3.zero, Quaternion.identity, audioSources.transform));
		}
		backgroundAudioObject = UnityEngine.Object.Instantiate(Resources.Load("Audio Objects/BGM Object") as GameObject, Vector3.zero, Quaternion.identity, audioSources.transform);
		RegisterSFXPlayer(sfxPlayer);
	}

	public static string GetRandomAudioClip(List<string> clips, ref int prevClipIndex)
	{
		if (clips.Count == 0)
		{
			return "";
		}
		int num;
		if (clips.Count == 1)
		{
			num = 0;
		}
		else if (prevClipIndex == -1 || prevClipIndex > clips.Count - 1)
		{
			num = ArtixRandom.Range(0, clips.Count - 1);
		}
		else
		{
			num = ArtixRandom.Range(0, clips.Count - 2);
			if (num >= prevClipIndex)
			{
				num++;
			}
		}
		prevClipIndex = num;
		return clips[num];
	}

	public static void PlayNpcGreeting(NPC npc)
	{
		Dictionary<int, List<string>> dictionary = Game.Instance?.AreaData?.NpcSfxGreetings;
		if (dictionary != null && npc != null && dictionary.TryGetValue(npc.NPCID, out var value))
		{
			Play2DSFX(GetRandomAudioClip(value, ref greetingPrev), 0f, 0.4f, 0f, SFXType.Dialogue);
		}
	}

	public static void PlayNpcFarewell(NPC npc)
	{
		Dictionary<int, List<string>> dictionary = Game.Instance?.AreaData?.NpcSfxFarewells;
		if (dictionary != null && npc?.wrapperTransform != null && dictionary.TryGetValue(npc.NPCID, out var value))
		{
			Play3DSFX(GetRandomAudioClip(value, ref farewellPrev), SFXType.Dialogue, npc.wrapperTransform, 0f, toFollow: true, loop: false, 0, 0.2f);
		}
	}

	public static void PlaySFXClip(AudioClip audioClip, SFXType type, Transform transformPosition = null, float delay = 0f, float volume = 0f, bool shouldFollow = false, bool is2D = false, float pitch = 1f, bool loop = false, int entityID = 0, float pan = 0f)
	{
		int num = (int)type;
		int index = 0;
		float num2 = 0f;
		float num3 = 0f;
		bool flag = false;
		float num4 = (float)AudioSettings.dspTime;
		bool flag2 = true;
		bool flag3 = ((Entities.Instance.me != null && Entities.Instance.me.wrapper != null) ? true : false);
		if (!is2D && transformPosition != null && flag3 && (transformPosition.position - Entities.Instance.me.wrapper.transform.position).sqrMagnitude > thresholdDistance * thresholdDistance)
		{
			return;
		}
		if (type == SFXType.MyCombat)
		{
			volume *= 1.4f;
		}
		if (type == SFXType.Boss)
		{
			volume *= 1.8f;
		}
		AudioSource audioSource = null;
		if (type == SFXType.Dialogue)
		{
			audioSource = AudioObjectsInUse.Where((AudioSource x) => x.priority == 1).FirstOrDefault();
		}
		if (audioSource != null)
		{
			audioSource.GetComponent<AudioPoolObject>().Complete();
		}
		else if (AudioObjectsQueue.Count == 0)
		{
			for (int i = 0; i < AudioObjectsInUse.Count; i++)
			{
				AudioSource audioSource2 = AudioObjectsInUse[i];
				float num5 = (num4 - audioSource2.GetComponent<AudioPoolObject>().timePlayed) / audioSource2.clip.length;
				float num6 = ((!flag3) ? 0f : ((!(audioSource2.transform != null)) ? 0f : (audioSource2.transform.position - Entities.Instance.me.wrapper.transform.position).sqrMagnitude));
				if (audioSource2.priority > num)
				{
					num = audioSource2.priority;
					flag2 = true;
					num3 = num6;
					num2 = num5;
					index = i;
					flag = true;
				}
				else if (audioSource2.priority == num && num6 > closeThresholdDistance * closeThresholdDistance && num6 > num3)
				{
					num3 = num6;
					flag2 = false;
					index = i;
					flag = true;
				}
				else if (audioSource2.priority == num && num5 > num2 && flag2)
				{
					num2 = num5;
					if (num2 > shortestClipPercent)
					{
						index = i;
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return;
			}
			AudioObjectsInUse[index].GetComponent<AudioPoolObject>().Complete();
		}
		AudioPoolObject component = RemoveFromAudioQueue().GetComponent<AudioPoolObject>();
		SetAudioCurves(component.MyAudioSource, audioPrefab);
		switch (type)
		{
		case SFXType.Boss:
			SetAudioCurves(component.MyAudioSource, bossPrefab);
			break;
		case SFXType.Cinematic:
		case SFXType.UI:
			SetAudioCurves(component.MyAudioSource, noReverbPrefab);
			break;
		}
		component.PlayAudioClip(audioClip, type, transformPosition, delay, volume, shouldFollow, is2D, pitch, loop, pan);
		if (loop)
		{
			if (!entityAudioLoops.ContainsKey(entityID))
			{
				entityAudioLoops.Add(entityID, null);
			}
			else if (entityAudioLoops[entityID] != null)
			{
				entityAudioLoops[entityID].Complete();
			}
			entityAudioLoops[entityID] = component;
		}
	}

	private static void SetAudioCurves(AudioSource audioSource, GameObject profile)
	{
		audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, profile.GetComponent<AudioSource>().GetCustomCurve(AudioSourceCurveType.CustomRolloff));
		audioSource.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, profile.GetComponent<AudioSource>().GetCustomCurve(AudioSourceCurveType.ReverbZoneMix));
		audioSource.maxDistance = profile.GetComponent<AudioSource>().maxDistance;
	}

	public static void PlayBGMClip(AudioClip audioClip)
	{
		backgroundAudioObject.GetComponent<AudioSourceUtilities>().PlayClip(audioClip);
	}

	public static void Play2DSFX(string name, float playDelay = 0f, float volume = 0f, float pan = 0f, SFXType type = SFXType.UI)
	{
		PlaySFXByString(name, type, null, playDelay, toFollow: true, is2D: true, loop: false, 0, volume, pan);
	}

	public static void Play2DSFX(AudioClip clip, float playDelay = 0f)
	{
		PlaySFXClip(clip, SFXType.UI, null, 0f, 0f, shouldFollow: false, is2D: true);
	}

	public static void PlayCombatSFX(string name, bool isMe, Transform position)
	{
		if (name != null)
		{
			SFXType type = (isMe ? SFXType.MyCombat : SFXType.Combat);
			PlaySFXByString(name, type, position);
		}
	}

	public static void PlayCombatSFX(string name, SFXType type, Transform position)
	{
		if (name != null)
		{
			PlaySFXByString(name, type, position);
		}
	}

	public static void Play3DSFX(string name, SFXType sfxType, Transform position, float playDelay = 0f, bool toFollow = true, bool loop = false, int entityID = 0, float volume = 0f)
	{
		if (name != null)
		{
			PlaySFXByString(name, sfxType, position, playDelay, toFollow, is2D: false, loop, entityID, volume);
		}
	}

	private static void PlaySFXByString(string name, SFXType type = SFXType.NotClassified, Transform position = null, float playDelay = 0f, bool toFollow = true, bool is2D = false, bool loop = false, int entityID = 0, float volume = 0f, float pan = 0f)
	{
		if (string.IsNullOrEmpty(name) || gameSFXPlayer == null)
		{
			return;
		}
		MixerTrack mixerTrack = gameSFXPlayer.MixerTracks.FirstOrDefault((MixerTrack x) => string.Compare(x.Name, name, StringComparison.OrdinalIgnoreCase) == 0);
		if (mixerTrack != null && mixerTrack.Clip != null)
		{
			if (volume == 0f)
			{
				PlaySFXClip(mixerTrack.Clip, type, position, playDelay, mixerTrack.Volume, toFollow, is2D, mixerTrack.Pitch, loop, entityID, pan);
			}
			else
			{
				PlaySFXClip(mixerTrack.Clip, type, position, playDelay, volume, toFollow, is2D, mixerTrack.Pitch, loop, entityID, pan);
			}
		}
	}

	public static void PlaySFXByMixerTrack(MixerTrack mt, SFXType type = SFXType.NotClassified, Transform position = null, float playDelay = 0f, bool toFollow = true)
	{
		PlaySFXClip(mt.Clip, type, position, playDelay, mt.Volume, toFollow, is2D: false, mt.Pitch);
	}

	public static void Stop(string clipname)
	{
		for (int num = AudioObjectsInUse.Count - 1; num >= 0; num--)
		{
			if (AudioObjectsInUse[num] == null)
			{
				Debug.LogError("Object is null");
				break;
			}
			if (string.Equals(AudioObjectsInUse[num].clip.name, clipname, StringComparison.OrdinalIgnoreCase))
			{
				AudioObjectsInUse[num].GetComponent<AudioPoolObject>()?.Complete();
			}
		}
	}

	public static void RegisterSFXPlayer(SFXPlayer sfx)
	{
		gameSFXPlayer = sfx;
	}

	public static void AddTracks(List<MixerTrack> tracks)
	{
		foreach (MixerTrack track in tracks)
		{
			gameSFXPlayer.AddMixerTrack(track);
		}
	}

	public static void StopBGM()
	{
		backgroundAudioObject.GetComponent<AudioSourceUtilities>().FadeAndStop(1.5f);
	}

	public static void CleanUp()
	{
		AudioSource[] array = UnityEngine.Object.FindObjectsOfType<AudioSource>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].clip = null;
		}
	}

	public static void StopLoopingTrack(int entityID)
	{
		if (entityAudioLoops != null && entityAudioLoops.ContainsKey(entityID))
		{
			if (entityAudioLoops[entityID] != null)
			{
				entityAudioLoops[entityID].Complete();
			}
			entityAudioLoops.Remove(entityID);
		}
	}
}
