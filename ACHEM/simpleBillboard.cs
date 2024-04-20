using UnityEngine;

public class simpleBillboard : NonStatic
{
	public bool Flip;

	private Transform cam;

	public bool ignoreX;

	public bool is3D;

	private void Start()
	{
		try
		{
			if (base.gameObject.layer == Layers.UIORTHO)
			{
				cam = UICamera.mainCamera.transform;
			}
			else
			{
				cam = Camera.main.transform;
			}
		}
		catch
		{
		}
	}

	private void Update()
	{
		if (cam != null)
		{
			Vector3 vector = ((!is3D) ? new Plane(cam.transform.forward, cam.transform.position - Vector3.forward).ClosestPointOnPlane(base.transform.position) : cam.transform.position);
			if (Flip)
			{
				Vector3 eulerAngles = base.transform.rotation.eulerAngles;
				if (ignoreX)
				{
					Vector3 eulerAngles2 = Quaternion.LookRotation(base.transform.position - vector).eulerAngles;
					base.transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles2.y, eulerAngles2.z);
				}
				else
				{
					base.transform.rotation = Quaternion.LookRotation(base.transform.position - vector);
				}
				return;
			}
			Vector3 eulerAngles3 = base.transform.rotation.eulerAngles;
			if (ignoreX)
			{
				base.transform.LookAt(vector);
				Vector3 eulerAngles4 = base.transform.rotation.eulerAngles;
				base.transform.eulerAngles = new Vector3(eulerAngles3.x, eulerAngles4.y, eulerAngles4.z);
			}
			else
			{
				base.transform.LookAt(vector);
			}
			return;
		}
		try
		{
			if (base.gameObject.layer == Layers.UI3D)
			{
				cam = UICamera.mainCamera.transform;
			}
			else
			{
				cam = Camera.main.transform;
			}
		}
		catch
		{
		}
	}
}
