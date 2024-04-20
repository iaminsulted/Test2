using System;
using System.Collections;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag Object")]
public class UIJoystick : MonoBehaviour
{
	public enum Mode
	{
		Fixed,
		Drag,
		Follow
	}

	public Transform target;

	public Vector3 scale = Vector3.one;

	public float radius = 40f;

	public bool Pressed;

	public bool centerOnPress = true;

	public bool allowDragReposition = true;

	private Vector3 userInitTouchPos;

	public int tapCount;

	public bool analogOutput;

	public bool normalize;

	public Vector2 position;

	public float deadZone = 2f;

	public float fadeOutAlpha = 0.2f;

	public float fadeOutDelay = 1f;

	public UIWidget[] widgetsToFade;

	public Transform[] widgetsToCenter;

	private float lastTapTime;

	public float doubleTapTimeWindow = 0.5f;

	public GameObject doubleTapMessageTarget;

	public string doubleTabMethodName;

	public event Action DoubleClicked;

	private void Awake()
	{
		userInitTouchPos = Vector3.zero;
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
		ResetJoystick();
	}

	private void OnDisable()
	{
	}

	private IEnumerator fadeOutJoystick()
	{
		yield return new WaitForSeconds(fadeOutDelay);
		UIWidget[] array = widgetsToFade;
		foreach (UIWidget obj in array)
		{
			Color color = obj.color;
			color.a = fadeOutAlpha;
			TweenColor.Begin(obj.gameObject, 0.5f, color).method = UITweener.Method.EaseOut;
		}
	}

	public void OnPress(bool pressed)
	{
		if ((Game.Instance.IsDragging() && pressed) || !(target != null))
		{
			return;
		}
		Pressed = pressed;
		if (pressed)
		{
			StopAllCoroutines();
			if (Time.time < lastTapTime + doubleTapTimeWindow)
			{
				if (doubleTapMessageTarget != null && doubleTabMethodName != "")
				{
					doubleTapMessageTarget.SendMessage(doubleTabMethodName, SendMessageOptions.DontRequireReceiver);
					tapCount++;
				}
			}
			else
			{
				tapCount = 1;
			}
			lastTapTime = Time.time;
			Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.lastEventPosition);
			float distance = 0f;
			Vector3 point = ray.GetPoint(distance);
			point.z = 0f;
			UIWidget[] array = widgetsToFade;
			for (int i = 0; i < array.Length; i++)
			{
				TweenColor.Begin(array[i].gameObject, 0.1f, Color.white).method = UITweener.Method.EaseIn;
			}
			if (centerOnPress)
			{
				CenterWidgets(point);
				return;
			}
			userInitTouchPos = target.position;
			OnDrag(Vector2.zero);
		}
		else
		{
			ResetJoystick();
		}
	}

	private void OnDrag(Vector2 _)
	{
		if (Game.Instance.IsDragging())
		{
			return;
		}
		Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.lastEventPosition);
		float distance = 0f;
		Vector3 vector = ray.GetPoint(distance) - userInitTouchPos;
		Vector3 vector2 = userInitTouchPos + vector;
		vector2.z = 0f;
		target.position = vector2;
		float magnitude = target.localPosition.magnitude;
		if (magnitude < deadZone)
		{
			position = Vector2.zero;
			target.localPosition = position;
		}
		else if (magnitude > radius)
		{
			if (allowDragReposition)
			{
				float maxLength = magnitude - radius;
				Vector3 vector3 = Vector3.ClampMagnitude(target.localPosition, maxLength);
				vector3 = target.TransformVector(vector3);
				Vector3 vector4 = userInitTouchPos + vector3;
				CenterWidgets(vector4);
			}
			target.localPosition = Vector3.ClampMagnitude(target.localPosition, radius);
			position = target.localPosition;
		}
		if (analogOutput)
		{
			position = target.localPosition;
		}
		if (normalize)
		{
			position = position / radius * Mathf.InverseLerp(radius, deadZone, 1f);
		}
	}

	private void CenterWidgets(Vector3 position)
	{
		userInitTouchPos = position;
		Transform[] array = widgetsToCenter;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].position = userInitTouchPos;
		}
	}

	private void ResetJoystick()
	{
		userInitTouchPos = base.transform.TransformPoint(Vector3.zero);
		Transform[] array = widgetsToCenter;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].position = userInitTouchPos;
		}
		tapCount = 0;
		position = Vector2.zero;
		target.position = userInitTouchPos;
		StartCoroutine(fadeOutJoystick());
	}

	public void Disable()
	{
		base.gameObject.SetActive(value: false);
	}

	private void OnDoubleClick()
	{
		if (!Game.Instance.IsInteracting())
		{
			this.DoubleClicked?.Invoke();
		}
	}
}
