using UnityEngine;

public class CapturePointMachine : PressureMachine
{
	private GameObject capturePointUI;

	public int capturePointID;

	public float captureRate;

	public float decayRate;

	public int scoreValue;

	public bool passiveScoring;

	public void Start()
	{
		capturePointUI = Object.Instantiate(Resources.Load<GameObject>("CapturePointIcon"));
		capturePointUI.transform.parent = base.gameObject.transform;
		capturePointUI.transform.position = new Vector3(capturePointUI.transform.position.x, capturePointUI.transform.position.y + 15f, capturePointUI.transform.position.z);
		capturePointUI.GetComponent<CapturePointUI>().setCapturePointIcon(capturePointID, passiveScoring);
	}

	public void LateUpdate()
	{
		capturePointUI.transform.position = new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y + 8f, base.gameObject.transform.position.z);
		if (State > 0)
		{
			capturePointUI.SetActive(value: true);
		}
		else
		{
			capturePointUI.SetActive(value: false);
		}
	}
}
