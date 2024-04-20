using System;

namespace Assets.Scripts.NetworkClient.CommClasses;

public class ResponsePlayAudioClip : Response
{
	public string ClipName;

	public int EntityID;

	public float Delay;

	public float Volume;

	public DateTime TimeStamp;
}
