using System.Collections;
using UnityEngine;

[AddComponentMenu("Interactivity/Machine Listeners/Machine State Instantiate")]
public class MachineStateInstantiate : MachineState
{
	public GameObject Prefab;

	public GameObject PrefabInit;

	public float LifePhysics = 1f;

	public float LifeTime = 8f;

	public Vector3 scaleMin = Vector3.one;

	public Vector3 scaleMax = Vector3.one;

	public Vector3 offset = Vector3.zero;

	public bool useUniformScale;

	private float uniformScaleMin;

	private float uniformScaleMax;

	private void Start()
	{
		uniformScaleMin = Mathf.Min(scaleMin.x, scaleMin.y, scaleMin.z);
		uniformScaleMax = Mathf.Max(scaleMax.x, scaleMax.y, scaleMax.z);
		InteractiveObject.StateUpdated += OnMachineStateUpdate;
		OnMachineStateInit(InteractiveObject.State);
	}

	private void OnDestroy()
	{
		InteractiveObject.StateUpdated -= OnMachineStateUpdate;
	}

	private void OnMachineStateInit(byte state)
	{
		if (state == State && PrefabInit != null)
		{
			InstantiatePrefab();
		}
	}

	private void OnMachineStateUpdate(byte state)
	{
		if (state == State)
		{
			InstantiatePrefab();
		}
	}

	private void InstantiatePrefab()
	{
		Vector3 vector = new Vector3(Random.Range(0f - offset.x, offset.x), Random.Range(0f - offset.y, offset.y), Random.Range(0f - offset.z, offset.z));
		GameObject gameObject = Object.Instantiate(Prefab, base.TargetTransform.position + vector, base.TargetTransform.rotation * Prefab.transform.rotation);
		gameObject.transform.SetParent(base.TargetTransform);
		if (useUniformScale)
		{
			float num = Random.Range(uniformScaleMin, uniformScaleMax);
			gameObject.transform.localScale = new Vector3(num, num, num);
		}
		else
		{
			gameObject.transform.localScale = new Vector3(Random.Range(scaleMin.x, scaleMax.x), Random.Range(scaleMin.y, scaleMax.y), Random.Range(scaleMin.z, scaleMax.z));
		}
		if (LifePhysics > 0f)
		{
			StartCoroutine(DisablePhysics(gameObject, LifePhysics));
		}
		if (LifeTime > 0f)
		{
			StartCoroutine(DestroyInstance(gameObject, LifeTime));
		}
	}

	private IEnumerator DisablePhysics(GameObject go, float t)
	{
		if (go != null)
		{
			yield return new WaitForSeconds(t);
			Rigidbody[] componentsInChildren = go.GetComponentsInChildren<Rigidbody>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].isKinematic = true;
			}
		}
	}

	private IEnumerator DestroyInstance(GameObject go, float t)
	{
		if (go != null)
		{
			yield return new WaitForSeconds(t);
			Object.Destroy(go);
		}
	}
}
