using UnityEngine;

public class UITimer : MonoBehaviour
{
	public delegate void onCompleteDelegate(GameObject go);

	public delegate void onUpdateDelegate(float remainingTime);

	private float remainingTime;

	private float totalTime;

	private bool running;

	public float TotalTime => totalTime;

	public float RemainingTime => remainingTime;

	public event onCompleteDelegate OnTimerComplete;

	public event onUpdateDelegate OnTimerUpdate;

	private void Start()
	{
	}

	private void Update()
	{
		if (!running)
		{
			return;
		}
		remainingTime -= Time.deltaTime;
		if (remainingTime < 0f)
		{
			remainingTime = 0f;
		}
		if (this.OnTimerUpdate != null)
		{
			this.OnTimerUpdate(remainingTime);
		}
		if (remainingTime <= 0f)
		{
			running = false;
			if (this.OnTimerComplete != null)
			{
				this.OnTimerComplete(base.gameObject);
			}
		}
	}

	public void Start(float time)
	{
		totalTime = (remainingTime = time);
		running = true;
	}

	public void StartTimer(float timeTotal, float timeElapsed = 0f)
	{
		if (timeTotal > timeElapsed)
		{
			totalTime = timeTotal;
			remainingTime = timeTotal - timeElapsed;
			running = true;
		}
	}

	public void Stop()
	{
		running = false;
	}
}
