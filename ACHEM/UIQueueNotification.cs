using System.Collections;
using UnityEngine;

public class UIQueueNotification : MonoBehaviour
{
	public UILabel queueCount;

	private float queuePenaltyRemaining;

	public void OnClick()
	{
		Confirmation.Show("Leave Queue", "Would you like to leave your current queue?", delegate(bool b)
		{
			if (b)
			{
				LeaveQueue();
			}
		});
	}

	private void LeaveQueue()
	{
		AEC.getInstance().sendRequest(new RequestLeaveQueue());
	}

	public void Toggle(bool isEnabled)
	{
		if (base.gameObject.activeInHierarchy != isEnabled)
		{
			if (isEnabled)
			{
				AudioManager.Play2DSFX("SFX_PVP_QueueJoin");
				Notification.ShowText("Queue Joined");
			}
			else
			{
				StopAllCoroutines();
			}
			base.gameObject.SetActive(isEnabled);
		}
	}

	public void UpdateQueueInfo(bool isEnabled, int current, int max, float queueDelayPenalty)
	{
		Toggle(isEnabled);
		StopAllCoroutines();
		if (queueDelayPenalty > 0f)
		{
			StartCoroutine(QueueDelayRoutine(queueDelayPenalty));
			return;
		}
		queueCount.color = Color.white;
		queueCount.text = ((max == 0) ? "Queued" : (current + " / " + max));
	}

	private IEnumerator QueueDelayRoutine(float duration)
	{
		queueCount.color = Color.red;
		for (queuePenaltyRemaining = duration; queuePenaltyRemaining > 0f; queuePenaltyRemaining -= 1f)
		{
			queueCount.text = ArtixString.FormatDuration(Mathf.CeilToInt(queuePenaltyRemaining));
			yield return new WaitForSecondsRealtime(1f);
		}
	}
}
