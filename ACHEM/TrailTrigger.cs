using PigeonCoopToolkit.Effects.Trails;
using UnityEngine;

public class TrailTrigger : MonoBehaviour
{
	public TrailRenderer_Base Trail;

	private Vector3 lastpos;

	public float velocity;

	public float threshold;

	private float startTime;

	private void OnEnable()
	{
		startTime = Time.time;
		lastpos = base.transform.position;
	}

	private void OnDisable()
	{
		Trail.Emit = false;
	}

	private void Update()
	{
		velocity = (base.transform.position - lastpos).magnitude / Time.deltaTime;
		lastpos = base.transform.position;
		if (velocity > threshold && Time.time > startTime + 0.4f)
		{
			Emit();
		}
		else
		{
			StopEmit();
		}
	}

	public void Emit()
	{
		if (!Trail.Emit)
		{
			Trail.Emit = true;
		}
	}

	public void StopEmit()
	{
		if (Trail.Emit)
		{
			Trail.Emit = false;
			base.enabled = false;
		}
	}
}
