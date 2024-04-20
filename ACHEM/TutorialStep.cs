using System;
using System.Collections;
using UnityEngine;

public abstract class TutorialStep : ScriptableObject
{
	public bool IsStarted { get; protected set; }

	public int Order
	{
		get
		{
			int length = base.name.IndexOf("_");
			return int.Parse(base.name.Substring(0, length));
		}
	}

	public event Action Completed;

	public virtual IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		IsStarted = true;
	}

	public void Complete()
	{
		if (this.Completed != null)
		{
			this.Completed();
		}
	}
}
