using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	public string SoundName;

	public bool PlayOnAwake;

	public bool LoopPlaylist = true;

	public bool Shuffle;

	public MixerTrack IntroMusic;

	public List<MixerTrack> Playlist;
}
