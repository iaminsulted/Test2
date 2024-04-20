using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour
{
	// Token: 0x060000FF RID: 255 RVA: 0x0000C740 File Offset: 0x0000A940
	private void Update()
	{
		if (this.axes == MouseLook.RotationAxes.MouseXAndY)
		{
			float y = base.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * this.sensitivityX;
			this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
			this.rotationY = Mathf.Clamp(this.rotationY, this.minimumY, this.maximumY);
			base.transform.localEulerAngles = new Vector3(-this.rotationY, y, 0f);
			return;
		}
		if (this.axes == MouseLook.RotationAxes.MouseX)
		{
			base.transform.Rotate(0f, Input.GetAxis("Mouse X") * this.sensitivityX, 0f);
			return;
		}
		this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
		this.rotationY = Mathf.Clamp(this.rotationY, this.minimumY, this.maximumY);
		base.transform.localEulerAngles = new Vector3(-this.rotationY, base.transform.localEulerAngles.y, 0f);
	}

	// Token: 0x06000100 RID: 256 RVA: 0x0000C868 File Offset: 0x0000AA68
	private void Start()
	{
		if (base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().freezeRotation = true;
		}
	}

	// Token: 0x0400002B RID: 43
	public MouseLook.RotationAxes axes;

	// Token: 0x0400002C RID: 44
	public float sensitivityX = 15f;

	// Token: 0x0400002D RID: 45
	public float sensitivityY = 15f;

	// Token: 0x0400002E RID: 46
	public float minimumX = -360f;

	// Token: 0x0400002F RID: 47
	public float maximumX = 360f;

	// Token: 0x04000030 RID: 48
	public float minimumY = -60f;

	// Token: 0x04000031 RID: 49
	public float maximumY = 60f;

	// Token: 0x04000032 RID: 50
	private float rotationY;

	// Token: 0x0200020D RID: 525
	public enum RotationAxes
	{
		// Token: 0x04000C46 RID: 3142
		MouseXAndY,
		// Token: 0x04000C47 RID: 3143
		MouseX,
		// Token: 0x04000C48 RID: 3144
		MouseY
	}
}
