using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BezierCurveLineRenderer : MonoBehaviour
{
	public Transform[] lineRendererPoints;

	[Range(2f, 50f)]
	public int vertexCount = 12;

	private List<Transform> transforms;

	private LineRenderer lineRenderer;

	private void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		transforms = new List<Transform>();
		Transform[] array = lineRendererPoints;
		foreach (Transform transform in array)
		{
			if ((bool)transform && !transforms.Contains(transform))
			{
				transforms.Add(transform);
			}
		}
	}

	private void Update()
	{
		if (transforms == null || transforms.Count <= 0 || vertexCount < 2)
		{
			lineRenderer.positionCount = 0;
			lineRenderer.SetPositions(new Vector3[1] { Vector3.zero });
			return;
		}
		List<Vector3> list = new List<Vector3>();
		for (float num = 0f; num <= 1f; num += 1f / (float)vertexCount)
		{
			Vector3 item = CalculateBezierPoint(num, transforms.Select((Transform point) => point.position));
			list.Add(item);
		}
		if (list.Count != vertexCount)
		{
			lineRenderer.positionCount = list.Count;
		}
		lineRenderer.SetPositions(list.ToArray());
	}

	private Vector3 CalculateBezierPoint(float ratio, IEnumerable<Vector3> points)
	{
		if (points.Count() == 1)
		{
			return points.First();
		}
		List<Vector3> list = new List<Vector3>();
		Vector3? vector = null;
		foreach (Vector3 point in points)
		{
			if (!vector.HasValue)
			{
				vector = point;
				continue;
			}
			list.Add(Vector3.Lerp(vector.Value, point, ratio));
			vector = point;
		}
		return CalculateBezierPoint(ratio, list);
	}
}
