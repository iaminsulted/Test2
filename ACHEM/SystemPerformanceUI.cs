using UnityEngine;

public class SystemPerformanceUI : MonoBehaviour
{
	public bool isMobile;

	public GameObject wifiIcon;

	public GameObject exclamationMark;

	public GameObject cross;

	public UILabel pingText;

	public UIGrid notificationGrid;

	public UITable pingGrid;

	private float lastPing = -1f;

	private LastExecutedStatement lastExecutedStatement;

	private Animator wifiIconAnimator;

	private void Start()
	{
		wifiIcon.SetActive(value: false);
		cross.SetActive(value: false);
		exclamationMark.SetActive(value: false);
		pingText.gameObject.SetActive(value: false);
		wifiIconAnimator = wifiIcon.GetComponent<Animator>();
	}

	private void Update()
	{
		float ping = Game.Instance.ping.GetPing();
		if (ping != lastPing)
		{
			if (Game.Instance.ping.GetPing() >= (float)(Session.MyPlayerData.LatencyNotifyThreshold * 2))
			{
				ShowLostConnection();
			}
			else if (Game.Instance.ping.GetPing() >= (float)Session.MyPlayerData.LatencyNotifyThreshold)
			{
				ShowPoorConnection();
			}
			else
			{
				ShowGoodConnection();
			}
			lastPing = ping;
		}
	}

	private void ShowLostConnection()
	{
		wifiIconAnimator.SetBool("Flash", value: true);
		wifiIcon.SetActive(value: true);
		pingText.gameObject.SetActive(value: false);
		exclamationMark.SetActive(value: false);
		cross.SetActive(value: true);
		if (!isMobile && lastExecutedStatement != 0)
		{
			RepositionGrids();
		}
		lastExecutedStatement = LastExecutedStatement.NoConnection;
	}

	private void ShowPoorConnection()
	{
		wifiIconAnimator.SetBool("Flash", value: true);
		wifiIcon.SetActive(value: true);
		pingText.gameObject.SetActive(value: true);
		exclamationMark.SetActive(value: true);
		cross.SetActive(value: false);
		pingText.text = Game.Instance.ping.GetPing().ToString("0") + "ms";
		if (!isMobile && lastExecutedStatement != LastExecutedStatement.PoorConnection)
		{
			RepositionGrids();
		}
		lastExecutedStatement = LastExecutedStatement.PoorConnection;
	}

	private void ShowGoodConnection()
	{
		wifiIconAnimator.SetBool("Flash", value: false);
		if (!SettingsManager.LatencyMonitor)
		{
			wifiIcon.SetActive(value: false);
			pingText.gameObject.SetActive(value: false);
			exclamationMark.SetActive(value: false);
			cross.SetActive(value: false);
			if (!isMobile && lastExecutedStatement != LastExecutedStatement.StableConnection)
			{
				RepositionGrids();
			}
			lastExecutedStatement = LastExecutedStatement.StableConnection;
			return;
		}
		wifiIcon.SetActive(value: true);
		pingText.gameObject.SetActive(value: true);
		exclamationMark.SetActive(value: false);
		cross.SetActive(value: false);
		if (!isMobile && lastExecutedStatement != LastExecutedStatement.StableConnection)
		{
			RepositionGrids();
		}
		lastExecutedStatement = LastExecutedStatement.StableConnection;
		pingText.text = (int)Game.Instance.ping.GetPing() + "ms";
	}

	private void RepositionGrids()
	{
		notificationGrid.Reposition();
		pingGrid.Reposition();
	}
}
