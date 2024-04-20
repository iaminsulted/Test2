using UnityEngine;

public class PopupIA : MonoBehaviour
{
	public InteractiveObject IA;

	public void Start()
	{
		if (IA != null)
		{
			Object.Instantiate(ResourceCache.Load<GameObject>("PopupIA"), base.transform.position, Quaternion.identity, base.transform.parent).GetComponent<PopupIATrigger>().IA = IA;
		}
		Object.Destroy(base.gameObject);
	}
}
