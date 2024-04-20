using UnityEngine;

public class GrassToggle : MonoBehaviour
{
	private void Awake()
	{
		Transform transform = base.transform.Find("_GrassParent");
		if (transform != null)
		{
			if (Platform.IsDesktop)
			{
				transform.gameObject.AddComponent<GrassController>();
				foreach (Transform allChild in transform.transform.GetAllChildren())
				{
					allChild.gameObject.AddComponent<GrassReseed>();
				}
			}
			else
			{
				Object.Destroy(transform.gameObject);
			}
		}
		Transform transform2 = base.transform.Find("_MobileGrassParent");
		if (transform2 != null)
		{
			if (Platform.IsMobile || transform == null)
			{
				transform2.gameObject.AddComponent<GrassController>();
			}
			else
			{
				Object.Destroy(transform2.gameObject);
			}
		}
	}
}
