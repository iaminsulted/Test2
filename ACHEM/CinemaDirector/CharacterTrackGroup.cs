using System.Collections.Generic;
using CinemaDirector.Helpers;
using UnityEngine;

namespace CinemaDirector;

[TrackGroup("Character Track Group", new TimelineTrackGenre[] { TimelineTrackGenre.CharacterTrack })]
public class CharacterTrackGroup : ActorTrackGroup, IRevertable, IBakeable
{
	[SerializeField]
	private RevertMode editorRevertMode;

	[SerializeField]
	private RevertMode runtimeRevertMode;

	private bool hasBeenBaked;

	public RevertMode EditorRevertMode
	{
		get
		{
			return editorRevertMode;
		}
		set
		{
			editorRevertMode = value;
		}
	}

	public RevertMode RuntimeRevertMode
	{
		get
		{
			return runtimeRevertMode;
		}
		set
		{
			runtimeRevertMode = value;
		}
	}

	public void Bake()
	{
		if (base.Actor == null || Application.isPlaying)
		{
			return;
		}
		Animator component = base.Actor.GetComponent<Animator>();
		if (component == null)
		{
			return;
		}
		AnimatorCullingMode cullingMode = component.cullingMode;
		component.cullingMode = AnimatorCullingMode.AlwaysAnimate;
		List<RevertInfo> list = new List<RevertInfo>();
		MonoBehaviour[] componentsInChildren = GetComponentsInChildren<MonoBehaviour>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] is IRevertable revertable)
			{
				list.AddRange(revertable.CacheState());
			}
		}
		Vector3 localPosition = base.Actor.transform.localPosition;
		Quaternion localRotation = base.Actor.transform.localRotation;
		Vector3 localScale = base.Actor.transform.localScale;
		float num = 30f;
		int num2 = (int)(base.Cutscene.Duration * num + 2f);
		component.StopPlayback();
		component.recorderStartTime = 0f;
		component.StartRecording(num2);
		base.SetRunningTime(0f);
		for (int j = 0; j < num2 - 1; j++)
		{
			TimelineTrack[] tracks = GetTracks();
			for (int k = 0; k < tracks.Length; k++)
			{
				if (!(tracks[k] is DialogueTrack))
				{
					tracks[k].UpdateTrack((float)j * (1f / num), 1f / num);
				}
			}
			component.Update(1f / num);
		}
		component.recorderStopTime = (float)num2 * (1f / num);
		component.StopRecording();
		component.StartPlayback();
		hasBeenBaked = true;
		base.Actor.transform.localPosition = localPosition;
		base.Actor.transform.localRotation = localRotation;
		base.Actor.transform.localScale = localScale;
		for (int l = 0; l < list.Count; l++)
		{
			RevertInfo revertInfo = list[l];
			if (revertInfo != null && ((revertInfo.EditorRevert == RevertMode.Revert && !Application.isPlaying) || (revertInfo.RuntimeRevert == RevertMode.Revert && Application.isPlaying)))
			{
				revertInfo.Revert();
			}
		}
		component.cullingMode = cullingMode;
		base.Initialize();
	}

	public RevertInfo[] CacheState()
	{
		RevertInfo[] array = new RevertInfo[3];
		if (base.Actor == null)
		{
			return new RevertInfo[0];
		}
		array[0] = new RevertInfo(this, base.Actor.transform, "localPosition", base.Actor.transform.localPosition);
		array[1] = new RevertInfo(this, base.Actor.transform, "localRotation", base.Actor.transform.localRotation);
		array[2] = new RevertInfo(this, base.Actor.transform, "localScale", base.Actor.transform.localScale);
		return array;
	}

	public override void Initialize()
	{
		base.Initialize();
		if (!Application.isPlaying && !(base.Actor == null))
		{
			Animator component = base.Actor.GetComponent<Animator>();
			if (!(component == null))
			{
				component.StartPlayback();
			}
		}
	}

	public override void UpdateTrackGroup(float time, float deltaTime)
	{
		if (Application.isPlaying)
		{
			base.UpdateTrackGroup(time, deltaTime);
			return;
		}
		TimelineTrack[] tracks = GetTracks();
		for (int i = 0; i < tracks.Length; i++)
		{
			if (!(tracks[i] is MecanimTrack))
			{
				tracks[i].UpdateTrack(time, deltaTime);
			}
		}
		if (!(base.Actor == null))
		{
			Animator component = base.Actor.GetComponent<Animator>();
			if (!(component == null) && base.Actor.gameObject.activeInHierarchy)
			{
				component.playbackTime = time;
				component.Update(0f);
			}
		}
	}

	public override void SetRunningTime(float time)
	{
		if (Application.isPlaying)
		{
			TimelineTrack[] tracks = GetTracks();
			for (int i = 0; i < tracks.Length; i++)
			{
				tracks[i].SetTime(time);
			}
			return;
		}
		TimelineTrack[] tracks2 = GetTracks();
		for (int j = 0; j < tracks2.Length; j++)
		{
			if (!(tracks2[j] is MecanimTrack))
			{
				tracks2[j].SetTime(time);
			}
		}
		if (!(base.Actor == null))
		{
			Animator component = base.Actor.GetComponent<Animator>();
			if (!(component == null) && base.Actor.gameObject.activeInHierarchy)
			{
				component.playbackTime = time;
				component.Update(0f);
			}
		}
	}

	public override void Stop()
	{
		base.Stop();
		if (!Application.isPlaying && hasBeenBaked)
		{
			hasBeenBaked = false;
			Animator component = base.Actor.GetComponent<Animator>();
			if (!(component == null) && component.recorderStopTime > 0f && base.Actor.gameObject.activeInHierarchy)
			{
				component.StartPlayback();
				component.playbackTime = 0f;
				component.Update(0f);
				component.StopPlayback();
				component.Rebind();
			}
		}
	}
}
