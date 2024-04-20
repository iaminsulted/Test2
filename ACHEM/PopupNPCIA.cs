using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopupNPCIA : MonoBehaviour
{
	public string Title;

	public Transform CameraSpot;

	public Transform FocusSpot;

	public List<NPCIA> Apops;

	public void Start()
	{
		if (Apops != null && Apops.Count > 0)
		{
			GameObject obj = Object.Instantiate(ResourceCache.Load<GameObject>("NPCPlate"), base.transform.position, Quaternion.identity);
			obj.name = "npcPlate";
			obj.transform.parent = base.transform.parent;
			obj.GetComponent<NPCIATrigger>().Init(Apops.Select((NPCIA x) => x.ID).ToList(), Title, CameraSpot, FocusSpot, 0);
		}
		Object.Destroy(base.gameObject);
	}
}
