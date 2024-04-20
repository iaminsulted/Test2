using System.Collections;
using UnityEngine;

public class ScoreBar : MonoBehaviour
{
	private const float meterFillRate = 2f;

	public UISprite team1Meter;

	public UISprite team1Point0;

	public UISprite team1Point1;

	public UISprite team2Meter;

	public UISprite team2Point0;

	public UISprite team2Point1;

	private int team1CurrentScore;

	private int team2CurrentScore;

	private bool isTeam1MeterFull;

	private bool isTeam2MeterFull;

	private bool isTeam1MeterEmptying;

	private bool isTeam2MeterEmptying;

	private NPC team1NPCTarget;

	private NPC team2NPCTarget;

	private Coroutine emptyTeam1Meter;

	private Coroutine emptyTeam2Meter;

	public void Init()
	{
		team1Meter.fillAmount = 0f;
		team2Meter.fillAmount = 0f;
		team1Point0.gameObject.SetActive(value: false);
		team1Point1.gameObject.SetActive(value: false);
		team2Point0.gameObject.SetActive(value: false);
		team2Point1.gameObject.SetActive(value: false);
	}

	public void Update()
	{
		UpdateTeam1Meter();
		UpdateTeam2Meter();
	}

	public void UpdateScore()
	{
		UpdateTeam1Score();
		UpdateTeam1Icon();
		UpdateTeam2Score();
		UpdateTeam2Icon();
	}

	private void UpdateTeam1Meter()
	{
		team1NPCTarget = Entities.Instance.GetNpcBySpawnId(Scoreboard.GetTeamNPC(1));
		if (team1NPCTarget != null && team1NPCTarget.Spawn != null && team1NPCTarget.serverState != Entity.State.Dead)
		{
			if (isTeam1MeterEmptying)
			{
				StopEmptyTeam1Meter();
				isTeam1MeterEmptying = false;
			}
			if (!isTeam1MeterFull)
			{
				team1Meter.fillAmount += 2f * Time.deltaTime;
				if (team1Meter.fillAmount >= team1NPCTarget.HealthPercent)
				{
					isTeam1MeterFull = true;
				}
			}
			else
			{
				team1Meter.fillAmount = team1NPCTarget.HealthPercent;
			}
		}
		else
		{
			team1Meter.fillAmount = 0f;
		}
	}

	private void UpdateTeam2Meter()
	{
		team2NPCTarget = Entities.Instance.GetNpcBySpawnId(Scoreboard.GetTeamNPC(2));
		if (team2NPCTarget != null && team2NPCTarget.Spawn != null && team2NPCTarget.serverState != Entity.State.Dead)
		{
			if (isTeam2MeterEmptying)
			{
				StopEmptyTeam2Meter();
				isTeam2MeterEmptying = false;
			}
			if (!isTeam2MeterFull)
			{
				team2Meter.fillAmount += 2f * Time.deltaTime;
				if (team2Meter.fillAmount >= team2NPCTarget.HealthPercent)
				{
					isTeam2MeterFull = true;
				}
			}
			else
			{
				team2Meter.fillAmount = team2NPCTarget.HealthPercent;
			}
		}
		else
		{
			team2Meter.fillAmount = 0f;
		}
	}

	private void UpdateTeam1Score()
	{
		if (team1CurrentScore != Scoreboard.GetTeamScore(1))
		{
			team1CurrentScore = Scoreboard.GetTeamScore(1);
			if (base.gameObject.activeSelf)
			{
				StopEmptyTeam1Meter();
				emptyTeam1Meter = StartCoroutine(EmptyTeam1Meter(1f));
			}
		}
		else if (base.gameObject.activeSelf)
		{
			StopEmptyTeam1Meter();
			emptyTeam1Meter = StartCoroutine(EmptyTeam1Meter());
		}
		isTeam1MeterFull = false;
	}

	private void UpdateTeam2Score()
	{
		if (team2CurrentScore != Scoreboard.GetTeamScore(2))
		{
			team2CurrentScore = Scoreboard.GetTeamScore(2);
			if (base.gameObject.activeSelf)
			{
				StopEmptyTeam2Meter();
				StartCoroutine(EmptyTeam2Meter(1f));
			}
		}
		else if (base.gameObject.activeSelf)
		{
			StopEmptyTeam2Meter();
			emptyTeam2Meter = StartCoroutine(EmptyTeam2Meter());
		}
		isTeam2MeterFull = false;
	}

	private void UpdateTeam1Icon()
	{
		if (team1CurrentScore == 1)
		{
			team1Point0.gameObject.SetActive(value: true);
			team1Point1.gameObject.SetActive(value: false);
		}
		else if (team1CurrentScore == 2)
		{
			team1Point0.gameObject.SetActive(value: true);
			team1Point1.gameObject.SetActive(value: true);
		}
	}

	private void UpdateTeam2Icon()
	{
		if (team2CurrentScore == 1)
		{
			team2Point0.gameObject.SetActive(value: true);
			team2Point1.gameObject.SetActive(value: false);
		}
		else if (team2CurrentScore == 2)
		{
			team2Point0.gameObject.SetActive(value: true);
			team2Point1.gameObject.SetActive(value: true);
		}
	}

	private IEnumerator EmptyTeam1Meter(float delay = 0f)
	{
		isTeam1MeterEmptying = true;
		float elapsed = 0f;
		while (elapsed < delay)
		{
			elapsed += Time.deltaTime;
			yield return null;
		}
		while (team1Meter.fillAmount > 0f && isTeam1MeterEmptying)
		{
			team1Meter.fillAmount -= 2f * Time.deltaTime;
			if (team1Meter.fillAmount < 0f)
			{
				team1Meter.fillAmount = 0f;
			}
			yield return null;
		}
		isTeam1MeterEmptying = false;
	}

	private IEnumerator EmptyTeam2Meter(float delay = 0f)
	{
		isTeam2MeterEmptying = true;
		float elapsed = 0f;
		while (elapsed < delay)
		{
			elapsed += Time.deltaTime;
			yield return null;
		}
		while (team2Meter.fillAmount > 0f && isTeam2MeterEmptying)
		{
			team2Meter.fillAmount -= 2f * Time.deltaTime;
			if (team2Meter.fillAmount < 0f)
			{
				team2Meter.fillAmount = 0f;
			}
			yield return null;
		}
		isTeam2MeterEmptying = false;
	}

	private void StopEmptyTeam1Meter()
	{
		if (emptyTeam1Meter != null)
		{
			StopCoroutine(emptyTeam1Meter);
			emptyTeam1Meter = null;
		}
	}

	private void StopEmptyTeam2Meter()
	{
		if (emptyTeam2Meter != null)
		{
			StopCoroutine(emptyTeam2Meter);
			emptyTeam2Meter = null;
		}
	}
}
