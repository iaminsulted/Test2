using System.Collections;
using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine Active LineRenderer Width Over Time")]
public class MachineActiveLineRendererWidthOverTime : MachineActive
{
	public float TargetWidth;

	public float TimeToMove;

	private LineRenderer lineRenderer;

	private float StartWidth;

	private void Start()
	{
		lineRenderer = base.TargetTransform.GetComponent<LineRenderer>();
		StartWidth = lineRenderer.widthMultiplier;
		InteractiveObject.ActiveUpdated += OnMachineActiveUpdate;
		OnMachineInitialized(InteractiveObject.IsActive);
	}

	private void OnMachineActiveUpdate(bool active)
	{
		if (active)
		{
			StartCoroutine(ChangeMultiplier(TargetWidth));
		}
		else
		{
			StartCoroutine(ChangeMultiplier(StartWidth));
		}
	}

	private void OnMachineInitialized(bool active)
	{
		if (active)
		{
			lineRenderer.widthMultiplier = TargetWidth;
		}
		else
		{
			lineRenderer.widthMultiplier = StartWidth;
		}
	}

	private void OnDestroy()
	{
		InteractiveObject.ActiveUpdated -= OnMachineActiveUpdate;
	}

	private IEnumerator ChangeMultiplier(float targetMultiplier)
	{
		float timeElapsed = 0f;
		float currentMutlipler = lineRenderer.widthMultiplier;
		while (!Mathf.Approximately(lineRenderer.widthMultiplier, targetMultiplier))
		{
			timeElapsed += Time.deltaTime / TimeToMove;
			lineRenderer.widthMultiplier = Mathf.Lerp(currentMutlipler, targetMultiplier, timeElapsed);
			yield return null;
		}
	}
}
