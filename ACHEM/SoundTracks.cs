using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTracks : MonoBehaviour
{
	private Dictionary<int, SoundTrack> soundTracks = new Dictionary<int, SoundTrack>();

	private MusicPlayer currentMusicPlayer;

	private static SoundTracks instance;

	public static SoundTracks Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject obj = Object.Instantiate(new GameObject());
				obj.name = "SoundTracks";
				instance = obj.AddComponent<SoundTracks>();
				Object.DontDestroyOnLoad(obj);
			}
			return instance;
		}
	}

	public void Clear()
	{
		if (currentMusicPlayer != null)
		{
			Object.Destroy(currentMusicPlayer.gameObject);
			currentMusicPlayer = null;
		}
		soundTracks.Clear();
	}

	public void Set(Dictionary<int, SoundTrack> soundTracks)
	{
		Clear();
		this.soundTracks = soundTracks;
	}

	public IEnumerator Play(int soundTrackID, bool showLoader)
	{
		if (soundTracks.ContainsKey(soundTrackID))
		{
			yield return StartCoroutine(LoadSoundTrack(soundTrackID, showLoader));
			AudioManager.PlayBGMClip(currentMusicPlayer.Playlist[0].Clip);
		}
	}

	private IEnumerator LoadSoundTrack(int soundTrackID, bool showLoader)
	{
		SoundTrack soundTrack = soundTracks[soundTrackID];
		AssetBundleLoader soundTrackLoader = AssetBundleManager.LoadAssetBundle(soundTrack.bundle);
		while (!soundTrackLoader.isDone)
		{
			if (showLoader)
			{
				Loader.show("Loading Sound Track..", soundTrackLoader.GetProgress());
			}
			yield return null;
		}
		if (!string.IsNullOrEmpty(soundTrackLoader.Error))
		{
			Debug.LogError(soundTrackLoader.Error);
			MessageBox.Show("Load Failure", "Sound Track asset bundle could not be loaded.");
			yield break;
		}
		AssetBundleRequest abr = soundTrackLoader.Asset.LoadAssetAsync<GameObject>(soundTrack.prefab);
		yield return abr;
		if (abr.asset == null)
		{
			MessageBox.Show("Load Failure", "Sound Track prefab could not be loaded from the assetbundle.");
			yield break;
		}
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.Game_LoadSoundBundleOk);
		GameObject gameObject = Object.Instantiate(abr.asset) as GameObject;
		currentMusicPlayer = gameObject.GetComponent<MusicPlayer>();
		gameObject.transform.parent = base.transform;
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.Game_InstantiateSoundOk);
	}
}
