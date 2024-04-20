using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TutorialEnumerator : IEnumerator<TutorialStep>, IEnumerator, IDisposable
{
	private TutorialStep[] steps;

	private int index = -1;

	public TutorialStep Current
	{
		get
		{
			if (index >= 0 && index < steps.Length)
			{
				return steps[index];
			}
			return null;
		}
	}

	object IEnumerator.Current
	{
		get
		{
			if (index >= 0 && index < steps.Length)
			{
				return steps[index];
			}
			return null;
		}
	}

	public TutorialEnumerator(TutorialStep[] steps)
	{
		this.steps = steps.OrderBy((TutorialStep x) => x.Order).ToArray();
	}

	public void Dispose()
	{
		steps = null;
	}

	public bool MoveNext()
	{
		return ++index < steps.Length;
	}

	public void Reset()
	{
		index = 0;
	}
}
