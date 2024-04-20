using UnityEngine;

public class ProjectionFillTime : MonoBehaviour
{
	public float projectionTimeComplete;

	private float projectionTimeElapsed;

	private Material projectionMaterial;

	private void Start()
	{
		Projector component = GetComponent<Projector>();
		projectionMaterial = new Material(component.material);
		component.material = projectionMaterial;
	}

	private void Update()
	{
		if ((bool)projectionMaterial && projectionTimeElapsed > 0f)
		{
			projectionTimeElapsed -= Time.deltaTime / projectionTimeComplete;
			if (projectionTimeElapsed <= 0f)
			{
				projectionTimeElapsed = 0f;
			}
			projectionMaterial.SetFloat("_FillTime", projectionTimeElapsed);
		}
	}

	private void OnEnable()
	{
		if (!projectionMaterial)
		{
			projectionMaterial = GetComponent<Projector>().material;
		}
		projectionMaterial.SetFloat("_FillTime", 1f);
	}

	private void OnDisable()
	{
		projectionTimeElapsed = 1f;
	}
}
