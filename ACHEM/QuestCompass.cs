using UnityEngine;

public class QuestCompass : MonoBehaviour
{
	public Transform target;

	public Transform player;

	public Transform playerCam;

	public Transform compass3d;

	public TweenPosition tweenPosition;

	public TweenScale tweenScale;

	private bool isBouncing;

	private bool isFlashing;

	private bool IsBouncing
	{
		get
		{
			return isBouncing;
		}
		set
		{
			if (isBouncing != value)
			{
				isBouncing = value;
				if (isBouncing)
				{
					compass3d.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
					compass3d.transform.localScale = new Vector3(1f, 1f, 0.8f);
					compass3d.transform.localPosition = Vector3.zero;
					tweenPosition.tweenFactor = 0f;
					tweenPosition.PlayForward();
				}
				else
				{
					tweenPosition.enabled = false;
					compass3d.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
					compass3d.transform.localScale = Vector3.one;
					compass3d.transform.localPosition = Vector3.zero;
				}
			}
		}
	}

	public bool IsFlashing
	{
		get
		{
			return isFlashing;
		}
		set
		{
			if (isFlashing != value)
			{
				isFlashing = value;
				if (isFlashing)
				{
					tweenScale.tweenFactor = 0f;
					tweenScale.PlayForward();
				}
				else
				{
					tweenScale.enabled = false;
					compass3d.transform.localScale = new Vector3(1f, 1f, 1f);
				}
			}
		}
	}

	public void Setup(Transform player, Transform camera)
	{
		this.player = player;
		playerCam = camera;
	}

	private void Update()
	{
		if (target != null)
		{
			LookAt(target);
		}
	}

	private void LookAt(Transform target)
	{
		if ((target.position - player.position).magnitude < 5f)
		{
			IsBouncing = true;
			return;
		}
		IsBouncing = false;
		float num = (target.position - player.position).Angle2D();
		float y = playerCam.rotation.eulerAngles.y;
		Quaternion quaternion = Quaternion.LookRotation(target.position - player.position);
		float num2 = quaternion.eulerAngles.x;
		if (num2 > 180f)
		{
			num2 -= 360f;
		}
		if (num2 > -15f && num2 < 10f)
		{
			compass3d.transform.localRotation = Quaternion.Euler(-25f, 0f, 0f) * Quaternion.Euler(0f, num - y, 0f);
		}
		else
		{
			compass3d.transform.localRotation = Quaternion.Euler(quaternion.eulerAngles.x, quaternion.eulerAngles.y - y, quaternion.eulerAngles.z);
		}
	}
}
