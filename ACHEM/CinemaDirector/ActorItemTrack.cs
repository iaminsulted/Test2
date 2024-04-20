using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CinemaDirector;

[TimelineTrack("Actor Track", new TimelineTrackGenre[]
{
	TimelineTrackGenre.ActorTrack,
	TimelineTrackGenre.MultiActorTrack
}, new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class ActorItemTrack : TimelineTrack, IActorTrack, IMultiActorTrack
{
	public Transform Actor
	{
		get
		{
			ActorTrackGroup actorTrackGroup = base.TrackGroup as ActorTrackGroup;
			if (actorTrackGroup == null)
			{
				Debug.LogError("No ActorTrackGroup found on parent.", this);
				return null;
			}
			return actorTrackGroup.Actor;
		}
	}

	public List<Transform> Actors
	{
		get
		{
			ActorTrackGroup actorTrackGroup = base.TrackGroup as ActorTrackGroup;
			if (actorTrackGroup != null)
			{
				return new List<Transform> { actorTrackGroup.Actor };
			}
			MultiActorTrackGroup multiActorTrackGroup = base.TrackGroup as MultiActorTrackGroup;
			if (multiActorTrackGroup != null)
			{
				return multiActorTrackGroup.Actors;
			}
			return null;
		}
	}

	public CinemaActorEvent[] ActorEvents => GetComponentsInChildren<CinemaActorEvent>();

	public CinemaActorAction[] ActorActions => GetComponentsInChildren<CinemaActorAction>();

	public override void Initialize()
	{
		base.Initialize();
		for (int i = 0; i < ActorEvents.Length; i++)
		{
			for (int j = 0; j < Actors.Count; j++)
			{
				if (Actors[j] != null)
				{
					ActorEvents[i].Initialize(Actors[j].gameObject);
				}
			}
		}
	}

	public override void SetTime(float time)
	{
		float num = elapsedTime;
		base.SetTime(time);
		TimelineItem[] timelineItems = GetTimelineItems();
		for (int i = 0; i < timelineItems.Length; i++)
		{
			CinemaActorEvent cinemaActorEvent = timelineItems[i] as CinemaActorEvent;
			if (cinemaActorEvent != null)
			{
				if ((num < cinemaActorEvent.Firetime && time >= cinemaActorEvent.Firetime) || (cinemaActorEvent.Firetime == 0f && num <= cinemaActorEvent.Firetime && time > cinemaActorEvent.Firetime))
				{
					for (int j = 0; j < Actors.Count; j++)
					{
						if (Actors[j] != null)
						{
							cinemaActorEvent.Trigger(Actors[j].gameObject);
						}
					}
				}
				else if (num > cinemaActorEvent.Firetime && time <= cinemaActorEvent.Firetime)
				{
					for (int k = 0; k < Actors.Count; k++)
					{
						if (Actors[k] != null)
						{
							cinemaActorEvent.Reverse(Actors[k].gameObject);
						}
					}
				}
			}
			CinemaActorAction cinemaActorAction = timelineItems[i] as CinemaActorAction;
			if (!(cinemaActorAction != null))
			{
				continue;
			}
			for (int l = 0; l < Actors.Count; l++)
			{
				if (Actors[l] != null)
				{
					cinemaActorAction.SetTime(Actors[l].gameObject, time - cinemaActorAction.Firetime, time - num);
				}
			}
		}
	}

	public override void UpdateTrack(float time, float deltaTime)
	{
		float num = elapsedTime;
		base.UpdateTrack(time, deltaTime);
		TimelineItem[] array = (from p in GetTimelineItems()
			orderby p.Firetime
			select p).ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			CinemaActorEvent cinemaActorEvent = array[i] as CinemaActorEvent;
			if (cinemaActorEvent != null)
			{
				if ((num < cinemaActorEvent.Firetime && time >= cinemaActorEvent.Firetime) || (cinemaActorEvent.Firetime == 0f && num <= cinemaActorEvent.Firetime && time > cinemaActorEvent.Firetime))
				{
					for (int j = 0; j < Actors.Count; j++)
					{
						if (Actors[j] != null)
						{
							cinemaActorEvent.Trigger(Actors[j].gameObject);
						}
					}
				}
				else if (num >= cinemaActorEvent.Firetime && elapsedTime <= cinemaActorEvent.Firetime)
				{
					for (int k = 0; k < Actors.Count; k++)
					{
						if (Actors[k] != null)
						{
							cinemaActorEvent.Reverse(Actors[k].gameObject);
						}
					}
				}
			}
			CinemaActorAction cinemaActorAction = array[i] as CinemaActorAction;
			if (!(cinemaActorAction != null))
			{
				continue;
			}
			if ((num < cinemaActorAction.Firetime || num <= 0f) && elapsedTime >= cinemaActorAction.Firetime && elapsedTime < cinemaActorAction.EndTime)
			{
				for (int l = 0; l < Actors.Count; l++)
				{
					if (Actors[l] != null)
					{
						cinemaActorAction.Trigger(Actors[l].gameObject);
					}
				}
			}
			else if (num < cinemaActorAction.EndTime && elapsedTime >= cinemaActorAction.EndTime)
			{
				for (int m = 0; m < Actors.Count; m++)
				{
					if (Actors[m] != null)
					{
						cinemaActorAction.End(Actors[m].gameObject);
					}
				}
			}
			else if (num >= cinemaActorAction.Firetime && num < cinemaActorAction.EndTime && elapsedTime <= cinemaActorAction.Firetime)
			{
				for (int n = 0; n < Actors.Count; n++)
				{
					if (Actors[n] != null)
					{
						cinemaActorAction.ReverseTrigger(Actors[n].gameObject);
					}
				}
			}
			else if ((num > cinemaActorAction.EndTime || num >= cinemaActorAction.Cutscene.Duration) && elapsedTime > cinemaActorAction.Firetime && elapsedTime <= cinemaActorAction.EndTime)
			{
				for (int num2 = 0; num2 < Actors.Count; num2++)
				{
					if (Actors[num2] != null)
					{
						cinemaActorAction.ReverseEnd(Actors[num2].gameObject);
					}
				}
			}
			else
			{
				if (!(elapsedTime > cinemaActorAction.Firetime) || !(elapsedTime <= cinemaActorAction.EndTime))
				{
					continue;
				}
				for (int num3 = 0; num3 < Actors.Count; num3++)
				{
					if (Actors[num3] != null)
					{
						float time2 = time - cinemaActorAction.Firetime;
						cinemaActorAction.UpdateTime(Actors[num3].gameObject, time2, deltaTime);
					}
				}
			}
		}
	}

	public override void Pause()
	{
		base.Pause();
		TimelineItem[] timelineItems = GetTimelineItems();
		for (int i = 0; i < timelineItems.Length; i++)
		{
			CinemaActorAction cinemaActorAction = timelineItems[i] as CinemaActorAction;
			if (!(cinemaActorAction != null) || !(elapsedTime > cinemaActorAction.Firetime) || !(elapsedTime < cinemaActorAction.Firetime + cinemaActorAction.Duration))
			{
				continue;
			}
			for (int j = 0; j < Actors.Count; j++)
			{
				if (Actors[j] != null)
				{
					cinemaActorAction.Pause(Actors[j].gameObject);
				}
			}
		}
	}

	public override void Resume()
	{
		base.Resume();
		TimelineItem[] timelineItems = GetTimelineItems();
		for (int i = 0; i < timelineItems.Length; i++)
		{
			CinemaActorAction cinemaActorAction = timelineItems[i] as CinemaActorAction;
			if (!(cinemaActorAction != null) || !(elapsedTime > cinemaActorAction.Firetime) || !(elapsedTime < cinemaActorAction.Firetime + cinemaActorAction.Duration))
			{
				continue;
			}
			for (int j = 0; j < Actors.Count; j++)
			{
				if (Actors[j] != null)
				{
					cinemaActorAction.Resume(Actors[j].gameObject);
				}
			}
		}
	}

	public override void Stop()
	{
		base.Stop();
		elapsedTime = 0f;
		TimelineItem[] timelineItems = GetTimelineItems();
		for (int i = 0; i < timelineItems.Length; i++)
		{
			CinemaActorEvent cinemaActorEvent = timelineItems[i] as CinemaActorEvent;
			if (cinemaActorEvent != null)
			{
				for (int j = 0; j < Actors.Count; j++)
				{
					if (Actors[j] != null)
					{
						cinemaActorEvent.Stop(Actors[j].gameObject);
					}
				}
			}
			CinemaActorAction cinemaActorAction = timelineItems[i] as CinemaActorAction;
			if (!(cinemaActorAction != null))
			{
				continue;
			}
			for (int k = 0; k < Actors.Count; k++)
			{
				if (Actors[k] != null)
				{
					cinemaActorAction.Stop(Actors[k].gameObject);
				}
			}
		}
	}
}
