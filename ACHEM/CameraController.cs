using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CameraController : MonoBehaviour
{
	private const float CAMERA_DISTANCE_MAX = 22f;

	private const float CAMERA_DISTANCE_MIN = 0.7f;

	private const float Camera_Collision_Buffer = 0.25f;

	public const float Default_Camera_Bob_Magnitude = 0.1f;

	public const float Default_Camera_Bob_Duration = 0.1f;

	private const float Camera_Fast_Zoom_Speed = 30f;

	private const float Camera_Normal_Zoom_Speed = 5f;

	private const float Recenter_Max_Angle = 35f;

	private const float Recenter_Min_Angle = 0f;

	private const float Pan_Angle_Per_Second = 180f;

	public bool panToTarget = true;

	private float panTime;

	private float startPanAngle;

	public float cameraDistanceTarget = 6f;

	public float cameraDistanceCurrent = 6f;

	public float cameraDistanceUser = 6f;

	public float CameraDistanceMax = 22f;

	public float CameraDistanceMin = 0.7f;

	private float cameraDeltaDivisor = 1f;

	private bool isBloomOn;

	private bool isDepthOfFieldOn;

	private Transform camTransform;

	private Transform targetTransform;

	private Vector3 cameraOffset = new Vector3(0f, 2.58f, -6.2f);

	public Vector3 pivotOffset = new Vector3(0f, 1.74f, 0f);

	private const float MIN_Y = -60f;

	private const float MAX_Y = 80f;

	private int touchID = int.MinValue;

	private float totalDrag;

	private bool leftMouseDrag;

	private bool rightMouseDrag;

	private Vector2 Deg;

	private Vector3 lastFramePosition;

	private OmniMovementController movementController;

	private bool isPinchZooming;

	private Camera cam;

	private DepthOfField dof;

	private BloomOptimized bloom;

	private IEnumerator currentShakeRoutine;

	private int InvertX = 1;

	private int InvertY = -1;

	private GameObject ParticleGO;

	public bool lockedOnTarget { get; private set; }

	private Camera Cam
	{
		get
		{
			if (cam == null)
			{
				cam = GetComponentInChildren<Camera>();
			}
			return cam;
		}
	}

	public event Action CameraMoved;

	public void Awake()
	{
		UpdateSettings();
		SettingsManager.CameraSettingUpdated += UpdateSettings;
		SettingsManager.BloomUpdated += SetBloom;
		SettingsManager.DepthOfFieldUpdated += SetDepthOfField;
		currentShakeRoutine = null;
	}

	public void OnDestroy()
	{
		SettingsManager.CameraSettingUpdated -= UpdateSettings;
		SettingsManager.BloomUpdated -= SetBloom;
		SettingsManager.DepthOfFieldUpdated -= SetDepthOfField;
	}

	public void OnDisable()
	{
		if (!SettingsManager.IsFullScreen)
		{
			Cursor.lockState = CursorLockMode.None;
		}
	}

	public void Update()
	{
		if (!(camTransform == null))
		{
			UpdateZoom();
			UpdateMouseCamera();
			PanTowardsTarget();
			if (!SettingsManager.FreeCamera)
			{
				RecenterCamera();
			}
			DetectCollisions();
			lastFramePosition = camTransform.position;
		}
	}

	public void Init(Transform target)
	{
		movementController = target.GetComponent<OmniMovementController>();
		camTransform = base.transform;
		targetTransform = target;
		base.transform.SetParent(target, worldPositionStays: false);
		lastFramePosition = camTransform.position;
		SetBloom(SettingsManager.UseBloom);
		SetDepthOfField(SettingsManager.UseDepthOfField);
	}

	private void UpdateSettings()
	{
		Cam.fieldOfView = SettingsManager.FieldOfView;
		InvertX = ((!SettingsManager.InvertX) ? 1 : (-1));
		InvertY = (SettingsManager.InvertY ? 1 : (-1));
		cameraDistanceTarget = SettingsManager.CameraZoomDistance;
		if (Platform.IsDesktop)
		{
			cameraDeltaDivisor = 1.1f;
		}
		else if (!Platform.IsEditor && Platform.IsIOS)
		{
			cameraDeltaDivisor = 2f;
		}
		else if (!Platform.IsEditor && Platform.IsAndroid)
		{
			cameraDeltaDivisor = 1.2f;
		}
	}

	public void OnPress(bool isPressed)
	{
		if (isPressed)
		{
			if (UICamera.isOverUI)
			{
				return;
			}
			touchID = UICamera.currentTouchID;
			totalDrag = 0f;
			if (UICamera.currentScheme == UICamera.ControlScheme.Mouse)
			{
				if (touchID == -1)
				{
					leftMouseDrag = true;
				}
				else if (touchID == -2)
				{
					rightMouseDrag = true;
				}
			}
		}
		else if (touchID == UICamera.currentTouchID)
		{
			touchID = int.MinValue;
			if (totalDrag > 50f && this.CameraMoved != null)
			{
				this.CameraMoved();
			}
		}
	}

	public void OnDrag(Vector2 delta)
	{
		if (touchID == UICamera.currentTouchID && UICamera.currentScheme != 0 && !isPinchZooming)
		{
			delta /= cameraDeltaDivisor;
			Deg = Vector2.Lerp(Deg, delta * SettingsManager.CameraSpeed, 0.5f);
			if (movementController.IsMoving())
			{
				Deg.y /= 3f;
			}
			Drag(Deg * Time.deltaTime);
		}
	}

	private void Drag(Vector2 delta)
	{
		if (base.enabled)
		{
			panToTarget = false;
			delta.x *= InvertX;
			delta.y *= InvertY;
			RotateHorizontal(delta.x);
			RotateVertical(delta.y);
			totalDrag += delta.magnitude;
		}
	}

	public void SetDepthOfField(bool value)
	{
		if (isDepthOfFieldOn == value)
		{
			return;
		}
		isDepthOfFieldOn = value;
		if (value)
		{
			dof = Cam.gameObject.AddComponent<DepthOfField>();
			if (Game.Instance.CurrentCell != null)
			{
				dof.focalTransform = targetTransform;
				dof.focalSize = Game.Instance.CurrentCell.focalSize;
				dof.aperture = Game.Instance.CurrentCell.aperture;
			}
			else
			{
				dof.focalTransform = targetTransform;
				dof.focalSize = 0.2f;
				dof.aperture = 0.3f;
			}
			dof.dofHdrShader = Shader.Find("Hidden/Dof/DepthOfFieldHdr");
		}
		else
		{
			UnityEngine.Object.Destroy(dof);
		}
	}

	public void SetBloom(bool value)
	{
		if (isBloomOn != value)
		{
			isBloomOn = value;
			if (value)
			{
				bloom = Cam.gameObject.AddComponent<BloomOptimized>();
				bloom.intensity = ((Game.Instance.CurrentCell != null) ? Game.Instance.CurrentCell.intensity : 0.4f);
				bloom.fastBloomShader = Shader.Find("Hidden/FastBloom");
			}
			else
			{
				UnityEngine.Object.Destroy(bloom);
			}
		}
	}

	public void Reset()
	{
		camTransform.localPosition = cameraOffset;
		camTransform.LookAt(targetTransform.TransformPoint(pivotOffset));
		camTransform.Translate(0f, 0f, (camTransform.position - targetTransform.TransformPoint(pivotOffset)).magnitude - cameraDistanceCurrent);
		MoveInFrontOfObjects();
		lastFramePosition = camTransform.position;
	}

	public void ResetToFront()
	{
		camTransform.localPosition = cameraOffset;
		camTransform.LookAt(targetTransform.TransformPoint(pivotOffset));
		RotateHorizontal(180f);
		camTransform.Translate(0f, 0f, (camTransform.position - targetTransform.TransformPoint(pivotOffset)).magnitude - cameraDistanceCurrent);
		MoveInFrontOfObjects();
	}

	private void DetectCollisions()
	{
		Vector3 direction = camTransform.position - lastFramePosition;
		if (Physics.Raycast(new Ray(lastFramePosition, direction), out var _, direction.magnitude + 0.25f, Layers.MASK_CAMERACOLLISION))
		{
			MoveInFrontOfObjects();
		}
	}

	public void MoveInFrontOfObjects()
	{
		float maxDistance = cameraDistanceTarget + 0.25f;
		Vector3 vector = targetTransform.TransformPoint(pivotOffset);
		if (Physics.Raycast(new Ray(vector, camTransform.position - vector), out var hitInfo, maxDistance, Layers.MASK_CAMERACOLLISION))
		{
			maxDistance = hitInfo.distance - 0.25f;
			maxDistance = Math.Min(maxDistance, cameraDistanceTarget);
			camTransform.Translate(0f, 0f, cameraDistanceCurrent - maxDistance);
			cameraDistanceCurrent = maxDistance;
		}
	}

	private void UpdateZoom()
	{
		DetectPinchToZoom();
		DetectScrollWheelZoom();
		MoveTowardTargetZoom();
	}

	private void DetectScrollWheelZoom()
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Mouse && !UICamera.isOverUI && InputManager.IsMouseInWindow() && (bool)SettingsManager.IsCameraZoomEnabled && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
		{
			if (Input.GetAxis("Mouse ScrollWheel") > 0f)
			{
				ZoomInStep();
			}
			else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
			{
				ZoomOutStep();
			}
		}
	}

	private void DetectPinchToZoom()
	{
		IDraggable draggable = Game.Instance.CurrentInteractable as IDraggable;
		if (UIGame.ControlScheme != ControlScheme.HANDHELD || Input.touchCount < 2 || UICamera.isOverUI || !SettingsManager.IsCameraZoomEnabled || UIGame.JSDelta != Vector2.zero || (draggable != null && draggable.IsDraggable(null)))
		{
			isPinchZooming = false;
			return;
		}
		Touch touch = Input.GetTouch(0);
		Touch touch2 = Input.GetTouch(1);
		Vector2 vector = touch.position - touch.deltaPosition;
		Vector2 vector2 = touch2.position - touch2.deltaPosition;
		float magnitude = (vector - vector2).magnitude;
		float magnitude2 = (touch.position - touch2.position).magnitude;
		float num = magnitude - magnitude2;
		UpdateCameraDistance((float)SettingsManager.ZoomSpeed * num * 0.05f);
		isPinchZooming = true;
	}

	public void ZoomInStep()
	{
		UpdateCameraDistance((0f - (float)SettingsManager.ZoomSpeed) * 0.5f);
	}

	public void ZoomOutStep()
	{
		UpdateCameraDistance((float)SettingsManager.ZoomSpeed * 0.5f);
	}

	private void UpdateCameraDistance(float delta)
	{
		cameraDistanceTarget += delta;
		cameraDistanceTarget = Mathf.Clamp(cameraDistanceTarget, CameraDistanceMin, CameraDistanceMax);
	}

	private float GetCameraDistancePercent()
	{
		return cameraDistanceTarget / CameraDistanceMax;
	}

	private void MoveTowardTargetZoom()
	{
		if (Mathf.Abs(cameraDistanceTarget - cameraDistanceCurrent) <= 0.25f && cameraDistanceTarget > 2f)
		{
			SaveZoomSetting();
			return;
		}
		float z = (cameraDistanceCurrent - cameraDistanceTarget) * Mathf.Clamp01(5f * Time.deltaTime);
		camTransform.Translate(0f, 0f, z);
		cameraDistanceCurrent = (camTransform.position - targetTransform.TransformPoint(pivotOffset)).magnitude;
	}

	private void SaveZoomSetting()
	{
		if ((float)SettingsManager.CameraZoomDistance != cameraDistanceTarget)
		{
			SettingsManager.CameraZoomDistance.Set(cameraDistanceTarget);
		}
	}

	private void UpdateMouseCamera()
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Mouse)
		{
			if (leftMouseDrag && !Input.GetMouseButton(0))
			{
				leftMouseDrag = false;
			}
			if (rightMouseDrag && !Input.GetMouseButton(1))
			{
				rightMouseDrag = false;
			}
			if (leftMouseDrag || rightMouseDrag)
			{
				float axis = Input.GetAxis("Mouse X");
				float axis2 = Input.GetAxis("Mouse Y");
				Vector2 vector = new Vector2(axis, axis2);
				Deg = vector * SettingsManager.CameraSpeed;
				Drag(Deg);
				Cursor.lockState = CursorLockMode.Confined;
			}
			else if (!SettingsManager.IsFullScreen)
			{
				Cursor.lockState = CursorLockMode.None;
			}
		}
	}

	public void ResetTargetLock()
	{
		panToTarget = true;
		lockedOnTarget = false;
		startPanAngle = 0f;
		panTime = 0f;
	}

	private void PanTowardsTarget()
	{
		if (!SettingsManager.TargetCameraLock || movementController == null)
		{
			return;
		}
		if (!panToTarget || touchID != int.MinValue || movementController.State.IsRotating())
		{
			lockedOnTarget = false;
			startPanAngle = 0f;
			return;
		}
		Entity me = Entities.Instance.me;
		if (me == null || me.OwnsMachine || me.TargetTransform == null || me.visualState == Entity.State.Dead)
		{
			return;
		}
		Vector3 from = me.position - camTransform.position;
		Vector3 to = me.TargetTransform.position - me.position;
		from.y = 0f;
		to.y = 0f;
		if (to.magnitude > Game.Max_Click_Distance)
		{
			return;
		}
		if (to.magnitude < 0.75f)
		{
			panToTarget = false;
			return;
		}
		float num = Vector3.SignedAngle(from, to, Vector3.up);
		if (lockedOnTarget)
		{
			RotateHorizontal(num);
			return;
		}
		if (startPanAngle == 0f)
		{
			startPanAngle = num;
			panTime = 0f;
		}
		float panPercent = GetPanPercent();
		float num2 = Mathf.Lerp(0f, startPanAngle, panPercent);
		float num3 = startPanAngle - num;
		float deg = num2 - num3;
		RotateHorizontal(deg);
		panTime += Time.deltaTime;
		if (panPercent >= 1f)
		{
			lockedOnTarget = true;
		}
	}

	private float GetPanPercent()
	{
		float num = Mathf.Clamp(Mathf.Abs(startPanAngle) / 180f, 0.2f, 0.5f);
		float num2 = Mathf.Clamp01(panTime / num);
		return num2 * num2 * num2 * (num2 * (6f * num2 - 15f) + 10f);
	}

	private void RecenterCamera()
	{
		if (!(movementController == null) && touchID == int.MinValue && movementController.IsMoving())
		{
			HorizontalRecenter();
			VerticalRecenter();
		}
	}

	private void HorizontalRecenter()
	{
		float num = camTransform.localEulerAngles.y;
		if (num > 180f)
		{
			num -= 360f;
		}
		if ((double)Mathf.Abs(num) > 0.5)
		{
			RotateHorizontal((0f - num) * 5f * Time.deltaTime);
		}
	}

	private void VerticalRecenter()
	{
		float num = camTransform.localEulerAngles.x;
		if (num > 180f)
		{
			num -= 360f;
		}
		if (num > 35f)
		{
			RotateVertical((0f - num + 35f) * 4f * Time.deltaTime);
			if (camTransform.localEulerAngles.x < 35.3f)
			{
				RotateVertical(-0.1f);
			}
		}
		else if (num < 0f)
		{
			RotateVertical((0f - num) * 2f * Time.deltaTime);
			if (camTransform.localEulerAngles.x > -0.3f)
			{
				RotateVertical(0.1f);
			}
		}
	}

	public void RotateHorizontal(float deg)
	{
		camTransform.RotateAround(targetTransform.TransformPoint(pivotOffset), Vector3.up, deg);
	}

	public void RotateVertical(float degrees)
	{
		Vector3 eulerAngles = camTransform.localRotation.eulerAngles;
		if (eulerAngles.x > 180f)
		{
			eulerAngles.x -= 360f;
		}
		if (eulerAngles.x + degrees > 80f)
		{
			degrees = 80f - eulerAngles.x;
		}
		else if (eulerAngles.x + degrees < -60f)
		{
			degrees = -60f - eulerAngles.x;
		}
		camTransform.RotateAround(targetTransform.TransformPoint(pivotOffset), camTransform.right, degrees);
	}

	public void SetupCellCamera(float minDistance, float maxDistance, float intensity, float focalLength, float focalSize, float aperture)
	{
		if (CameraDistanceMin.ApproximatelyEquals(0.7f) && CameraDistanceMax.ApproximatelyEquals(22f))
		{
			cameraDistanceUser = cameraDistanceTarget;
		}
		CameraDistanceMax = ((maxDistance > 0f) ? maxDistance : 22f);
		CameraDistanceMin = ((minDistance > 0f) ? minDistance : 0.7f);
		if (CameraDistanceMax < CameraDistanceMin)
		{
			CameraDistanceMax = CameraDistanceMin;
		}
		cameraDistanceTarget = Mathf.Clamp(cameraDistanceUser, CameraDistanceMin, CameraDistanceMax);
		cameraDistanceCurrent = cameraDistanceTarget;
		Reset();
		if (bloom != null)
		{
			bloom.intensity = intensity;
		}
		if (dof != null)
		{
			dof.focalLength = focalLength;
			dof.focalSize = focalSize;
			dof.aperture = aperture;
		}
	}

	public void SetupCellParticles(Transform t, bool onstart)
	{
		if (!(t == null))
		{
			ParticleGO = t.gameObject;
			ParticleGO.transform.SetParent(base.transform, worldPositionStays: false);
			ParticleGO.transform.localPosition = Vector3.zero;
			ParticleGO.transform.localEulerAngles = Vector3.zero;
			t.gameObject.SetActive(onstart);
		}
	}

	public void Clear()
	{
		UnityEngine.Object.Destroy(ParticleGO);
	}

	public void PlayCameraShake(List<SpellCamShake> camShakes, SpellCamShake.Trigger trigger, SpellCamShake.Target target, float duration = 0.1f)
	{
		SpellCamShake spellCamShake = camShakes.FirstOrDefault((SpellCamShake shake) => shake.trigger == trigger && shake.target == target);
		if (spellCamShake != null)
		{
			float multiplier = spellCamShake.multiplier;
			if (trigger == SpellCamShake.Trigger.Impact)
			{
				_ = target;
				_ = 1;
			}
			PlayCameraShake(spellCamShake.style, multiplier, duration);
		}
	}

	public void PlayCameraShake(SpellCamShake.Style shakeStyle, float shakeMultiplier = 1f, float duration = 0.1f)
	{
		if (shakeStyle == SpellCamShake.Style.None || shakeMultiplier <= 0f || duration <= 0f || (float)SettingsManager.CameraShake <= 0f)
		{
			return;
		}
		if (currentShakeRoutine != null)
		{
			StopCoroutine(currentShakeRoutine);
		}
		Vector3 zero = Vector3.zero;
		float num = 0.1f * shakeMultiplier * (float)SettingsManager.CameraShake * GetCameraDistancePercent();
		if (!(num <= 0f))
		{
			switch (shakeStyle)
			{
			case SpellCamShake.Style.Random:
			{
				Vector2 insideUnitCircle2 = UnityEngine.Random.insideUnitCircle;
				zero = base.transform.TransformVector(insideUnitCircle2).normalized;
				currentShakeRoutine = CameraBobRoutine(zero * num, duration);
				StartCoroutine(currentShakeRoutine);
				break;
			}
			case SpellCamShake.Style.WeaponFollow:
				zero = Entities.Instance.me.GetWeaponDirection();
				zero.z = 0f;
				zero = base.transform.TransformVector(zero).normalized;
				currentShakeRoutine = CameraBobRoutine(zero * num, duration);
				StartCoroutine(currentShakeRoutine);
				break;
			case SpellCamShake.Style.Jitter:
			{
				Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
				zero = base.transform.TransformVector(insideUnitCircle).normalized;
				currentShakeRoutine = CameraJitterRoutine(zero * num, 0.1f);
				StartCoroutine(currentShakeRoutine);
				break;
			}
			case SpellCamShake.Style.Rotate:
				currentShakeRoutine = CameraRotateRoutine(num, duration);
				StartCoroutine(currentShakeRoutine);
				break;
			}
		}
	}

	private IEnumerator CameraRotateRoutine(float magnitude, float duration)
	{
		iTween.ShakeRotation(Cam.gameObject, iTween.Hash("islocal", true, "amount", new Vector3(0f, 0f, magnitude * 4f), "time", duration));
		yield break;
	}

	private IEnumerator CameraJitterRoutine(Vector3 jitterDirection, float duration)
	{
		if (!(jitterDirection == Vector3.zero) && !(duration <= 0f))
		{
			float jitterDuration = duration / 3f;
			for (int currentJitter = 1; currentJitter <= 3; currentJitter++)
			{
				yield return StartCoroutine(CameraBobRoutine(jitterDirection, jitterDuration));
				jitterDirection *= -1f;
			}
		}
	}

	private IEnumerator CameraBobRoutine(Vector3 bobDirection, float duration)
	{
		if (!(bobDirection == Vector3.zero) && !(duration <= 0f))
		{
			float bobOutDuration = duration / 5f;
			float bobBackDuration = duration - bobOutDuration;
			Vector3 targetPos = targetTransform.TransformPoint(pivotOffset) + bobDirection;
			Vector3 currentOffset = Vector3.zero;
			_ = Vector3.zero;
			Vector3 dampVelocity = Vector3.zero;
			float startTime = GameTime.realtimeSinceServerStartup;
			while (GameTime.realtimeSinceServerStartup - startTime <= bobOutDuration)
			{
				Vector3 worldPosition = Vector3.SmoothDamp(targetTransform.TransformPoint(pivotOffset) + currentOffset, targetPos, ref dampVelocity, bobOutDuration);
				camTransform.LookAt(worldPosition);
				currentOffset = bobDirection * (GameTime.realtimeSinceServerStartup - startTime) / bobOutDuration;
				yield return null;
			}
			startTime = GameTime.realtimeSinceServerStartup;
			dampVelocity = Vector3.zero;
			while (GameTime.realtimeSinceServerStartup - startTime <= bobBackDuration)
			{
				Vector3 worldPosition = Vector3.SmoothDamp(targetTransform.TransformPoint(pivotOffset) + currentOffset, targetTransform.TransformPoint(pivotOffset), ref dampVelocity, bobBackDuration);
				camTransform.LookAt(worldPosition);
				currentOffset = bobDirection * (1f - (GameTime.realtimeSinceServerStartup - startTime) / bobBackDuration);
				yield return null;
			}
			camTransform.LookAt(targetTransform.TransformPoint(pivotOffset));
		}
	}
}
