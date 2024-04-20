using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LinkedLineRenderer : MonoBehaviour
{
	public Transform[] lineRendererPoints;

	private LineRenderer lineRenderer;

	private Vector3[] pointList;

	private int pointCount;

	public bool animated;

	private Transform endPoint;

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
		pointCount = lineRendererPoints.Length;
		lineRenderer.positionCount = pointCount;
		pointList = new Vector3[pointCount];
		endPoint = lineRendererPoints[lineRendererPoints.Length - 1];
		UpdatePositions();
	}

	private void LateUpdate()
	{
		if (animated)
		{
			UpdatePositions();
		}
	}

	private void UpdatePositions()
	{
		if (pointCount != lineRendererPoints.Length)
		{
			lineRenderer.positionCount = lineRendererPoints.Length;
			pointCount = lineRendererPoints.Length;
		}
		for (int i = 0; i < pointCount; i++)
		{
			if (lineRendererPoints[i] == null)
			{
				return;
			}
			if (lineRenderer.useWorldSpace)
			{
				pointList[i] = lineRendererPoints[i].position;
			}
			else
			{
				pointList[i] = lineRendererPoints[i].localPosition;
			}
		}
		lineRenderer.SetPositions(pointList);
	}

	public void ResetFishingEndPoint()
	{
		lineRendererPoints[lineRendererPoints.Length - 1] = endPoint;
	}

	public void SetFishingEndPoint(Transform transform)
	{
		lineRendererPoints[lineRendererPoints.Length - 1] = transform;
	}
}
