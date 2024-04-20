using System;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
	public enum GizmoType
	{
		Position,
		Rotation,
		Scale
	}

	public enum GizmoMode
	{
		Local,
		Global
	}

	public PositionGizmoHandle xPos;

	public PositionGizmoHandle yPos;

	public PositionGizmoHandle zPos;

	public RotationGizmoHandle xRot;

	public RotationGizmoHandle yRot;

	public RotationGizmoHandle zRot;

	public ScaleGizmoHandle xScale;

	public ScaleGizmoHandle yScale;

	public ScaleGizmoHandle zScale;

	private Transform target;

	public Action onUpdate;

	private const float extra = 0.5f;

	private Bounds bounds;

	private GameObject gizmoGO;

	private UIGizmoButtons controls;

	private Vector3 initialPosition;

	private Quaternion initialRotation;

	private Vector3 initialScale;

	private Quaternion globalRotation = Quaternion.identity;

	public Vector3 center { get; private set; }

	public static GizmoType currentType { get; private set; }

	public static GizmoMode currentMode { get; private set; }

	public static bool isGlobal { get; private set; }

	public static bool isArrowsShowing { get; private set; }

	private float scale => (Game.Instance.cam.transform.position - center).magnitude / 10f;

	private void OnDestroy()
	{
		UnityEngine.Object.Destroy(controls.gameObject);
		PositionGizmoHandle positionGizmoHandle = xPos;
		positionGizmoHandle.onUpdate = (Action<AxisGizmoHandle, Vector3>)Delegate.Remove(positionGizmoHandle.onUpdate, new Action<AxisGizmoHandle, Vector3>(OnPositionHandleUpdate));
		PositionGizmoHandle positionGizmoHandle2 = yPos;
		positionGizmoHandle2.onUpdate = (Action<AxisGizmoHandle, Vector3>)Delegate.Remove(positionGizmoHandle2.onUpdate, new Action<AxisGizmoHandle, Vector3>(OnPositionHandleUpdate));
		PositionGizmoHandle positionGizmoHandle3 = zPos;
		positionGizmoHandle3.onUpdate = (Action<AxisGizmoHandle, Vector3>)Delegate.Remove(positionGizmoHandle3.onUpdate, new Action<AxisGizmoHandle, Vector3>(OnPositionHandleUpdate));
		PositionGizmoHandle positionGizmoHandle4 = xPos;
		positionGizmoHandle4.onDragStart = (Action)Delegate.Remove(positionGizmoHandle4.onDragStart, new Action(OnDragStart));
		PositionGizmoHandle positionGizmoHandle5 = yPos;
		positionGizmoHandle5.onDragStart = (Action)Delegate.Remove(positionGizmoHandle5.onDragStart, new Action(OnDragStart));
		PositionGizmoHandle positionGizmoHandle6 = zPos;
		positionGizmoHandle6.onDragStart = (Action)Delegate.Remove(positionGizmoHandle6.onDragStart, new Action(OnDragStart));
		ScaleGizmoHandle scaleGizmoHandle = xScale;
		scaleGizmoHandle.onUpdate = (Action<ScaleGizmoHandle>)Delegate.Remove(scaleGizmoHandle.onUpdate, new Action<ScaleGizmoHandle>(OnScaleHandleUpdate));
		ScaleGizmoHandle scaleGizmoHandle2 = yScale;
		scaleGizmoHandle2.onUpdate = (Action<ScaleGizmoHandle>)Delegate.Remove(scaleGizmoHandle2.onUpdate, new Action<ScaleGizmoHandle>(OnScaleHandleUpdate));
		ScaleGizmoHandle scaleGizmoHandle3 = zScale;
		scaleGizmoHandle3.onUpdate = (Action<ScaleGizmoHandle>)Delegate.Remove(scaleGizmoHandle3.onUpdate, new Action<ScaleGizmoHandle>(OnScaleHandleUpdate));
		ScaleGizmoHandle scaleGizmoHandle4 = xScale;
		scaleGizmoHandle4.onDragStart = (Action)Delegate.Remove(scaleGizmoHandle4.onDragStart, new Action(OnDragStart));
		ScaleGizmoHandle scaleGizmoHandle5 = yScale;
		scaleGizmoHandle5.onDragStart = (Action)Delegate.Remove(scaleGizmoHandle5.onDragStart, new Action(OnDragStart));
		ScaleGizmoHandle scaleGizmoHandle6 = zScale;
		scaleGizmoHandle6.onDragStart = (Action)Delegate.Remove(scaleGizmoHandle6.onDragStart, new Action(OnDragStart));
		ScaleGizmoHandle scaleGizmoHandle7 = xScale;
		scaleGizmoHandle7.onDragEnd = (Action)Delegate.Remove(scaleGizmoHandle7.onDragEnd, new Action(OnScalerDragEnd));
		ScaleGizmoHandle scaleGizmoHandle8 = yScale;
		scaleGizmoHandle8.onDragEnd = (Action)Delegate.Remove(scaleGizmoHandle8.onDragEnd, new Action(OnScalerDragEnd));
		ScaleGizmoHandle scaleGizmoHandle9 = zScale;
		scaleGizmoHandle9.onDragEnd = (Action)Delegate.Remove(scaleGizmoHandle9.onDragEnd, new Action(OnScalerDragEnd));
		RotationGizmoHandle rotationGizmoHandle = xRot;
		rotationGizmoHandle.onUpdate = (Action<float>)Delegate.Remove(rotationGizmoHandle.onUpdate, new Action<float>(OnXRotationUpdate));
		RotationGizmoHandle rotationGizmoHandle2 = yRot;
		rotationGizmoHandle2.onUpdate = (Action<float>)Delegate.Remove(rotationGizmoHandle2.onUpdate, new Action<float>(OnYRotationUpdate));
		RotationGizmoHandle rotationGizmoHandle3 = zRot;
		rotationGizmoHandle3.onUpdate = (Action<float>)Delegate.Remove(rotationGizmoHandle3.onUpdate, new Action<float>(OnZRotationUpdate));
		RotationGizmoHandle rotationGizmoHandle4 = xRot;
		rotationGizmoHandle4.onDragStart = (Action)Delegate.Remove(rotationGizmoHandle4.onDragStart, new Action(OnDragStart));
		RotationGizmoHandle rotationGizmoHandle5 = yRot;
		rotationGizmoHandle5.onDragStart = (Action)Delegate.Remove(rotationGizmoHandle5.onDragStart, new Action(OnDragStart));
		RotationGizmoHandle rotationGizmoHandle6 = zRot;
		rotationGizmoHandle6.onDragStart = (Action)Delegate.Remove(rotationGizmoHandle6.onDragStart, new Action(OnDragStart));
		RotationGizmoHandle rotationGizmoHandle7 = xRot;
		rotationGizmoHandle7.onDragEnd = (Action)Delegate.Remove(rotationGizmoHandle7.onDragEnd, new Action(OnRotatorDragEnd));
		RotationGizmoHandle rotationGizmoHandle8 = yRot;
		rotationGizmoHandle8.onDragEnd = (Action)Delegate.Remove(rotationGizmoHandle8.onDragEnd, new Action(OnRotatorDragEnd));
		RotationGizmoHandle rotationGizmoHandle9 = zRot;
		rotationGizmoHandle9.onDragEnd = (Action)Delegate.Remove(rotationGizmoHandle9.onDragEnd, new Action(OnRotatorDragEnd));
	}

	public static void Enable(Transform target, bool showControls = false)
	{
		if (!target.gameObject.GetComponent<GizmoWrapper>())
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/Housing/Gizmo/Gizmo"));
			Gizmo component = gameObject.GetComponent<Gizmo>();
			component.controls = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/Housing/Gizmo/GizmoButtons"), UIManager.Instance.transform).GetComponent<UIGizmoButtons>();
			component.Init(gameObject, target, showControls);
		}
	}

	public static void Disable(Transform target)
	{
		UnityEngine.Object.Destroy(target.GetComponentInChildren<GizmoWrapper>());
	}

	public void Revert()
	{
		target.transform.position = initialPosition;
		target.transform.rotation = initialRotation;
		target.transform.localScale = initialScale;
	}

	public void ShowArrows(bool isArrowsShowing)
	{
		Gizmo.isArrowsShowing = !Gizmo.isArrowsShowing;
	}

	private void Init(GameObject gizmoGO, Transform target, bool showControls = false)
	{
		this.target = target;
		initialPosition = target.transform.position;
		initialRotation = target.transform.rotation;
		initialScale = target.transform.localScale;
		this.gizmoGO = gizmoGO;
		if (showControls)
		{
			isArrowsShowing = false;
		}
		GameObject obj = UnityEngine.Object.Instantiate(xPos.gameObject);
		UnityEngine.Object.Destroy(obj.GetComponent<PositionGizmoHandle>());
		ScaleGizmoHandle scaleGizmoHandle = obj.AddComponent<ScaleGizmoHandle>();
		xScale = UnityEngine.Object.Instantiate(scaleGizmoHandle, base.transform);
		ClickForwarder[] componentsInChildren = xScale.GetComponentsInChildren<ClickForwarder>();
		foreach (ClickForwarder obj2 in componentsInChildren)
		{
			obj2.clickParentGO = null;
			obj2.iaParent = xScale;
		}
		yScale = UnityEngine.Object.Instantiate(scaleGizmoHandle, base.transform);
		componentsInChildren = yScale.GetComponentsInChildren<ClickForwarder>();
		foreach (ClickForwarder obj3 in componentsInChildren)
		{
			obj3.clickParentGO = null;
			obj3.iaParent = yScale;
		}
		zScale = UnityEngine.Object.Instantiate(scaleGizmoHandle, base.transform);
		componentsInChildren = zScale.GetComponentsInChildren<ClickForwarder>();
		foreach (ClickForwarder obj4 in componentsInChildren)
		{
			obj4.clickParentGO = null;
			obj4.iaParent = zScale;
		}
		UnityEngine.Object.Destroy(scaleGizmoHandle.gameObject);
		controls.Show(this);
		this.target.gameObject.AddComponent<GizmoWrapper>().Init(gizmoGO, this);
		refresh();
		PositionGizmoHandle positionGizmoHandle = xPos;
		positionGizmoHandle.onUpdate = (Action<AxisGizmoHandle, Vector3>)Delegate.Combine(positionGizmoHandle.onUpdate, new Action<AxisGizmoHandle, Vector3>(OnPositionHandleUpdate));
		PositionGizmoHandle positionGizmoHandle2 = yPos;
		positionGizmoHandle2.onUpdate = (Action<AxisGizmoHandle, Vector3>)Delegate.Combine(positionGizmoHandle2.onUpdate, new Action<AxisGizmoHandle, Vector3>(OnPositionHandleUpdate));
		PositionGizmoHandle positionGizmoHandle3 = zPos;
		positionGizmoHandle3.onUpdate = (Action<AxisGizmoHandle, Vector3>)Delegate.Combine(positionGizmoHandle3.onUpdate, new Action<AxisGizmoHandle, Vector3>(OnPositionHandleUpdate));
		PositionGizmoHandle positionGizmoHandle4 = xPos;
		positionGizmoHandle4.onDragStart = (Action)Delegate.Combine(positionGizmoHandle4.onDragStart, new Action(OnDragStart));
		PositionGizmoHandle positionGizmoHandle5 = yPos;
		positionGizmoHandle5.onDragStart = (Action)Delegate.Combine(positionGizmoHandle5.onDragStart, new Action(OnDragStart));
		PositionGizmoHandle positionGizmoHandle6 = zPos;
		positionGizmoHandle6.onDragStart = (Action)Delegate.Combine(positionGizmoHandle6.onDragStart, new Action(OnDragStart));
		ScaleGizmoHandle scaleGizmoHandle2 = xScale;
		scaleGizmoHandle2.onUpdate = (Action<ScaleGizmoHandle>)Delegate.Combine(scaleGizmoHandle2.onUpdate, new Action<ScaleGizmoHandle>(OnScaleHandleUpdate));
		ScaleGizmoHandle scaleGizmoHandle3 = yScale;
		scaleGizmoHandle3.onUpdate = (Action<ScaleGizmoHandle>)Delegate.Combine(scaleGizmoHandle3.onUpdate, new Action<ScaleGizmoHandle>(OnScaleHandleUpdate));
		ScaleGizmoHandle scaleGizmoHandle4 = zScale;
		scaleGizmoHandle4.onUpdate = (Action<ScaleGizmoHandle>)Delegate.Combine(scaleGizmoHandle4.onUpdate, new Action<ScaleGizmoHandle>(OnScaleHandleUpdate));
		ScaleGizmoHandle scaleGizmoHandle5 = xScale;
		scaleGizmoHandle5.onDragStart = (Action)Delegate.Combine(scaleGizmoHandle5.onDragStart, new Action(OnDragStart));
		ScaleGizmoHandle scaleGizmoHandle6 = yScale;
		scaleGizmoHandle6.onDragStart = (Action)Delegate.Combine(scaleGizmoHandle6.onDragStart, new Action(OnDragStart));
		ScaleGizmoHandle scaleGizmoHandle7 = zScale;
		scaleGizmoHandle7.onDragStart = (Action)Delegate.Combine(scaleGizmoHandle7.onDragStart, new Action(OnDragStart));
		ScaleGizmoHandle scaleGizmoHandle8 = xScale;
		scaleGizmoHandle8.onDragEnd = (Action)Delegate.Combine(scaleGizmoHandle8.onDragEnd, new Action(OnScalerDragEnd));
		ScaleGizmoHandle scaleGizmoHandle9 = yScale;
		scaleGizmoHandle9.onDragEnd = (Action)Delegate.Combine(scaleGizmoHandle9.onDragEnd, new Action(OnScalerDragEnd));
		ScaleGizmoHandle scaleGizmoHandle10 = zScale;
		scaleGizmoHandle10.onDragEnd = (Action)Delegate.Combine(scaleGizmoHandle10.onDragEnd, new Action(OnScalerDragEnd));
		RotationGizmoHandle rotationGizmoHandle = xRot;
		rotationGizmoHandle.onUpdate = (Action<float>)Delegate.Combine(rotationGizmoHandle.onUpdate, new Action<float>(OnXRotationUpdate));
		RotationGizmoHandle rotationGizmoHandle2 = yRot;
		rotationGizmoHandle2.onUpdate = (Action<float>)Delegate.Combine(rotationGizmoHandle2.onUpdate, new Action<float>(OnYRotationUpdate));
		RotationGizmoHandle rotationGizmoHandle3 = zRot;
		rotationGizmoHandle3.onUpdate = (Action<float>)Delegate.Combine(rotationGizmoHandle3.onUpdate, new Action<float>(OnZRotationUpdate));
		RotationGizmoHandle rotationGizmoHandle4 = xRot;
		rotationGizmoHandle4.onDragStart = (Action)Delegate.Combine(rotationGizmoHandle4.onDragStart, new Action(OnDragStart));
		RotationGizmoHandle rotationGizmoHandle5 = yRot;
		rotationGizmoHandle5.onDragStart = (Action)Delegate.Combine(rotationGizmoHandle5.onDragStart, new Action(OnDragStart));
		RotationGizmoHandle rotationGizmoHandle6 = zRot;
		rotationGizmoHandle6.onDragStart = (Action)Delegate.Combine(rotationGizmoHandle6.onDragStart, new Action(OnDragStart));
		RotationGizmoHandle rotationGizmoHandle7 = xRot;
		rotationGizmoHandle7.onDragEnd = (Action)Delegate.Combine(rotationGizmoHandle7.onDragEnd, new Action(OnRotatorDragEnd));
		RotationGizmoHandle rotationGizmoHandle8 = yRot;
		rotationGizmoHandle8.onDragEnd = (Action)Delegate.Combine(rotationGizmoHandle8.onDragEnd, new Action(OnRotatorDragEnd));
		RotationGizmoHandle rotationGizmoHandle9 = zRot;
		rotationGizmoHandle9.onDragEnd = (Action)Delegate.Combine(rotationGizmoHandle9.onDragEnd, new Action(OnRotatorDragEnd));
	}

	private void OnRotatorDragEnd()
	{
		globalRotation = Quaternion.identity;
	}

	private void OnDragStart()
	{
		if (Input.GetKey(KeyCode.LeftControl))
		{
			HousingManager.houseInstance.DuplicateSelected();
		}
	}

	private void OnScalerDragEnd()
	{
	}

	private void OnXRotationUpdate(float delta)
	{
		target.RotateAround(center, xRot.transform.forward, delta);
		globalRotation *= Quaternion.Euler(base.transform.right * delta);
	}

	private void OnYRotationUpdate(float delta)
	{
		target.RotateAround(center, yRot.transform.forward, delta);
		globalRotation *= Quaternion.Euler(base.transform.up * delta);
	}

	private void OnZRotationUpdate(float delta)
	{
		target.RotateAround(center, zRot.transform.forward, delta);
		globalRotation *= Quaternion.Euler(base.transform.forward * delta);
	}

	private void OnPositionHandleUpdate(AxisGizmoHandle axisHandle, Vector3 delta)
	{
		if (currentType == GizmoType.Position)
		{
			PositionGizmoHandle positionGizmoHandle = axisHandle as PositionGizmoHandle;
			if (positionGizmoHandle != null)
			{
				target.position = positionGizmoHandle.GetNewTargetPosition(target, delta);
				base.transform.position = target.position;
			}
		}
	}

	private void OnScaleHandleUpdate(ScaleGizmoHandle scaleHandle)
	{
		if (currentType == GizmoType.Scale)
		{
			if (currentMode == GizmoMode.Global)
			{
				target.localScale = scaleHandle.GetNewTargetGlobalScale(target);
			}
			else if (currentMode == GizmoMode.Local)
			{
				target.localScale = scaleHandle.GetNewTargetLocalScale(target);
			}
		}
	}

	private void Update()
	{
		refresh();
		onUpdate?.Invoke();
	}

	public void SetGizmoType(GizmoType type)
	{
		currentType = type;
	}

	public void SetGizmoMode(GizmoMode mode)
	{
		currentMode = mode;
	}

	private void refresh()
	{
		base.transform.position = target.transform.position;
		Bounds meshBounds = Util.GetMeshBounds(target.gameObject);
		bounds = meshBounds;
		Transform[] componentsInChildren = target.transform.GetComponentsInChildren<Transform>();
		center = Vector3.zero;
		int num = 0;
		Transform[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			MeshRenderer component = array[i].GetComponent<MeshRenderer>();
			if (component != null)
			{
				num++;
				center += component.bounds.center;
			}
		}
		if (num == 0)
		{
			center = target.transform.position;
		}
		else
		{
			center /= (float)num;
		}
		if (currentType == GizmoType.Position && isArrowsShowing)
		{
			if (currentMode == GizmoMode.Global)
			{
				xPos.transform.position = center + new Vector3(scale, 0f, 0f);
				xPos.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
				yPos.transform.position = center + new Vector3(0f, scale, 0f);
				yPos.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
				zPos.transform.position = center + new Vector3(0f, 0f, scale);
				zPos.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			}
			else
			{
				xPos.transform.position = center + target.right * scale;
				xPos.transform.rotation = target.rotation * Quaternion.Euler(0f, 90f, 0f);
				yPos.transform.position = center + target.up * scale;
				yPos.transform.rotation = target.rotation * Quaternion.Euler(-90f, 0f, 0f);
				zPos.transform.position = center + target.forward * scale;
				zPos.transform.rotation = target.rotation;
			}
			xPos.transform.localScale = Vector3.one * scale;
			yPos.transform.localScale = Vector3.one * scale;
			zPos.transform.localScale = Vector3.one * scale;
			xPos.SetColor(Color.red * 0.8f);
			yPos.SetColor(Color.green * 0.8f);
			zPos.SetColor(Color.blue * 0.8f);
			SetPositionerActive(isActive: true);
		}
		else
		{
			SetPositionerActive(isActive: false);
		}
		if (currentType == GizmoType.Scale && isArrowsShowing)
		{
			xScale.transform.position = center + target.right * scale;
			xScale.transform.rotation = target.rotation * Quaternion.Euler(0f, 90f, 0f);
			yScale.transform.position = center + target.up * scale;
			yScale.transform.rotation = target.rotation * Quaternion.Euler(-90f, 90f, 0f);
			zScale.transform.position = center + target.forward * scale;
			zScale.transform.rotation = target.rotation;
			xScale.SetColor(Color.cyan * 0.8f);
			yScale.SetColor(Color.cyan * 0.8f);
			zScale.SetColor(Color.cyan * 0.8f);
			if (currentMode == GizmoMode.Global)
			{
				xScale.gameObject.SetActive(value: false);
				yScale.gameObject.SetActive(value: false);
				zScale.gameObject.SetActive(value: true);
			}
			else
			{
				xScale.gameObject.SetActive(value: true);
				yScale.gameObject.SetActive(value: true);
				zScale.gameObject.SetActive(value: true);
			}
			xScale.transform.localScale = Vector3.one * scale;
			yScale.transform.localScale = Vector3.one * scale;
			zScale.transform.localScale = Vector3.one * scale;
		}
		else
		{
			xScale.gameObject.SetActive(value: false);
			yScale.gameObject.SetActive(value: false);
			zScale.gameObject.SetActive(value: false);
		}
		if (currentType == GizmoType.Rotation && isArrowsShowing)
		{
			xRot.transform.position = center;
			yRot.transform.position = center;
			zRot.transform.position = center;
			xRot.transform.localScale = Vector3.one * scale;
			yRot.transform.localScale = Vector3.one * scale;
			zRot.transform.localScale = Vector3.one * scale;
			Vector3 upwards = Game.Instance.cam.transform.position - center;
			if (currentMode == GizmoMode.Global)
			{
				xRot.transform.rotation = Quaternion.LookRotation(Vector3.right, upwards);
				yRot.transform.rotation = Quaternion.LookRotation(Vector3.up, upwards);
				zRot.transform.rotation = Quaternion.LookRotation(Vector3.forward, upwards);
			}
			else
			{
				xRot.transform.rotation = Quaternion.LookRotation(target.transform.right, upwards);
				yRot.transform.rotation = Quaternion.LookRotation(target.transform.up, upwards);
				zRot.transform.rotation = Quaternion.LookRotation(target.transform.forward, upwards);
			}
			SetRotatorActive(isActive: true);
		}
		else
		{
			SetRotatorActive(isActive: false);
		}
	}

	private void SetPositionerActive(bool isActive)
	{
		xPos.gameObject.SetActive(isActive);
		yPos.gameObject.SetActive(isActive);
		zPos.gameObject.SetActive(isActive);
	}

	private void SetRotatorActive(bool isActive)
	{
		xRot.gameObject.SetActive(isActive);
		yRot.gameObject.SetActive(isActive);
		zRot.gameObject.SetActive(isActive);
	}
}
