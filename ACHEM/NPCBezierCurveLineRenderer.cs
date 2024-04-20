using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class NPCBezierCurveLineRenderer : MonoBehaviour
{
	public NPCSpawn npcSpawn;

	public List<Transform> lineRendererPoints = new List<Transform>();

	public int vertexCount = 12;

	public bool setToEnd = true;

	private List<Transform> transforms;

	private bool active;

	private LineRenderer lineRenderer;

	public void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		transforms = new List<Transform>();
		foreach (Transform lineRendererPoint in lineRendererPoints)
		{
			if ((bool)lineRendererPoint && !transforms.Contains(lineRendererPoint))
			{
				transforms.Add(lineRendererPoint);
			}
		}
		npcSpawn.StateUpdated += OnNPCStateUpdated;
		OnNPCStateUpdated(npcSpawn.State);
	}

	private void Update()
	{
		if (!active || transforms == null || transforms.Count <= 0 || vertexCount < 2)
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

	public void OnDestroy()
	{
		npcSpawn.StateUpdated -= OnNPCStateUpdated;
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

	public void OnNPCStateUpdated(byte state)
	{
		active = state == 1;
		transforms.RemoveAll((Transform i) => i == null);
		if (active)
		{
			NPC npcBySpawnId = Entities.Instance.GetNpcBySpawnId(npcSpawn.ID);
			if (npcBySpawnId != null)
			{
				npcBySpawnId.assetController.AssetUpdated += OnAssetUpdated;
			}
		}
	}

	public void OnAssetUpdated(GameObject go)
	{
		NPC npcBySpawnId = Entities.Instance.GetNpcBySpawnId(npcSpawn.ID);
		if (npcBySpawnId != null)
		{
			npcBySpawnId.assetController.AssetUpdated -= OnAssetUpdated;
			if (setToEnd)
			{
				transforms.Add(npcBySpawnId.HitSpot);
			}
			else
			{
				transforms.Insert(0, npcBySpawnId.HitSpot);
			}
		}
	}
}
