using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EntityController : MonoBehaviour
{
	public enum CharState
	{
		Idle,
		Jumping,
		Falling,
		Dead
	}

	private const int Target_Deselect_Frame_Interval = 5;

	private const float Jump_Height = 1.5f;

	private const float Gravity = 20f;

	private const float Jump_Buffer = 0.2f;

	private const float MoveBlend_Damp_Time = 0.05f;

	private const float CharBlend_Damp_Time = 0.05f;

	private const float IdleBlend_Damp_Time = 0.2f;

	private EntityAnimation currentAction;

	private Entity entity;

	private GameObject asset;

	private Animator assetAnimator;

	private MovementController movementController;

	private CharState charState;

	private Entity.PhaseState phaseState;

	private Quaternion strafeRotation;

	private Transform boneSpine2;

	private Transform boneSpine3;

	private Transform boneSpine4;

	private Vector3 playerDirection = Vector3.zero;

	private Vector3 startJumpDirection = Vector3.zero;

	private Vector3 inAirDirection = Vector3.zero;

	private MovementState startJumpState;

	private MovementState lastJumpState;

	private float verticalSpeed;

	private float moveSpeed;

	private float jumpBuffer;

	private float jumpForgivenessLeft = 0.1f;

	private bool lastGroundedState = true;

	private Vector3 dashStartPosition = Vector3.zero;

	private Vector3 dashEndPosition = Vector3.zero;

	private float dashDuration;

	private float dashTotalDuration;

	private bool syncAfterDash;

	private int targetDeselectFrameCount;

	private float combatSpeed = 1f;

	private IEnumerator hitPauseRoutine;

	private float lastSelfKillTS = float.MinValue;

	private CollisionFlags collisionFlags;

	private bool skipNextCrossfade;

	public SFXPlayer customSFXPlayer;

	public Transform originalParent;

	private const float POSITION_BOUND_EXTENT = 10000f;

	public CharacterController CharacterController { get; private set; }

	public bool isStandardRig
	{
		get
		{
			if (assetAnimator != null)
			{
				return HasAnimatorState("IAmHumanBean");
			}
			return false;
		}
	}

	private bool isHumanoidSpine
	{
		get
		{
			if (boneSpine2 != null && boneSpine3 != null)
			{
				return boneSpine4 != null;
			}
			return false;
		}
	}

	public string currentAnimName
	{
		get
		{
			if (currentAction != null)
			{
				return currentAction.name;
			}
			return "";
		}
	}

	private float JumpForgivenessDuration
	{
		get
		{
			if (!Platform.IsDesktop)
			{
				return 0.2f;
			}
			return 0.1f;
		}
	}

	public Entity Entity => entity;

	public bool IsBlockingMovement
	{
		get
		{
			if (currentAction != null)
			{
				return currentAction.blockMovement;
			}
			return false;
		}
	}

	private bool isDashing => dashDuration < dashTotalDuration;

	private bool movingBack
	{
		get
		{
			if (!movementController.State.IsBackward() && !startJumpState.IsBackward())
			{
				return lastJumpState.IsBackward();
			}
			return true;
		}
	}

	private bool movingSideways
	{
		get
		{
			bool num = (movementController.State.IsRightStrafe() || movementController.State.IsLeftStrafe()) && !movementController.State.IsForward();
			bool flag = (startJumpState.IsRightStrafe() || startJumpState.IsLeftStrafe()) && !startJumpState.IsForward();
			bool flag2 = (lastJumpState.IsRightStrafe() || lastJumpState.IsLeftStrafe()) && !lastJumpState.IsForward();
			return num || flag || flag2;
		}
	}

	public bool IsJumping()
	{
		return charState == CharState.Jumping;
	}

	public int GetPhaseState()
	{
		return (int)phaseState;
	}

	public bool HasAnimatorState(string animState, int layer = -1)
	{
		if (assetAnimator == null)
		{
			return false;
		}
		if (layer != -1)
		{
			if (assetAnimator.HasState(layer, Animator.StringToHash(animState)))
			{
				return true;
			}
		}
		else
		{
			for (int i = 0; i < assetAnimator.layerCount; i++)
			{
				if (assetAnimator.HasState(i, Animator.StringToHash(animState)))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool HasOverrideAnimation(string animState)
	{
		if (assetAnimator == null)
		{
			return false;
		}
		if (!(assetAnimator.runtimeAnimatorController is AnimatorOverrideController))
		{
			return true;
		}
		AnimatorOverrideController animatorOverrideController = (AnimatorOverrideController)assetAnimator.runtimeAnimatorController;
		AnimationClip animationClip = animatorOverrideController[animState] ?? animatorOverrideController["D_" + animState];
		if (animationClip == null)
		{
			return false;
		}
		return !animationClip.name.StartsWith("D_");
	}

	public void Start()
	{
		movementController = GetComponent<MovementController>();
		movementController.Jumped += OnJumped;
		entity.EffectAdded += OnEffectAdded;
		jumpForgivenessLeft = JumpForgivenessDuration;
		if (entity.CanMove)
		{
			base.transform.localPosition += Vector3.up * 0.3f;
			if (Physics.Raycast(new Ray(base.transform.position + Vector3.up, -Vector3.up), out var hitInfo, 8f, Layers.MASK_GROUNDTRACK))
			{
				base.transform.position = hitInfo.point;
			}
			collisionFlags = CollisionFlags.Below;
			UpdateJumpForgiveness();
		}
		originalParent = base.transform.parent;
	}

	public void Init(Entity entity)
	{
		this.entity = entity;
		CharacterController = entity.wrapper.GetComponent<CharacterController>();
		if (CharacterController == null)
		{
			CharacterController = entity.wrapper.AddComponent<CharacterController>();
		}
		CharacterController.height = 2f;
		CharacterController.radius = 0.5f;
		CharacterController.center = new Vector3(0f, 1.08f, 0f);
		CharacterController.slopeLimit = 60f;
		CharacterController.stepOffset = 0.4f;
		if (entity.type == Entity.Type.NPC)
		{
			CharacterController.name = "controller_npc_" + entity.ID;
		}
		else if (entity.type == Entity.Type.Player)
		{
			CharacterController.name = "controller_player_" + entity.ID;
		}
	}

	public void Update()
	{
		if (!CharacterController.enabled)
		{
			return;
		}
		MoveController();
		UpdateAssetController();
		if (!(asset == null))
		{
			UpdateRotation();
			if (targetDeselectFrameCount == 0)
			{
				CheckForTargetDeselect();
			}
			targetDeselectFrameCount = (targetDeselectFrameCount + 1) % 5;
		}
	}

	public void LateUpdate()
	{
		if (!(asset == null) && !(asset.transform == null) && isHumanoidSpine)
		{
			RotateSpine();
		}
	}

	public void setAnimPhaseState(Entity.PhaseState phase)
	{
		phaseState = phase;
	}

	public void SetAsset(GameObject _asset)
	{
		if (asset == _asset)
		{
			return;
		}
		currentAction = null;
		asset = _asset;
		assetAnimator = asset.GetComponent<Animator>();
		if (assetAnimator != null)
		{
			try
			{
				ActionStateMachineBehaviour[] behaviours = assetAnimator.GetBehaviours<ActionStateMachineBehaviour>();
				for (int i = 0; i < behaviours.Length; i++)
				{
					behaviours[i].ActionCompleted += OnActionComplete;
				}
				SheatheCompletedBehaviour[] behaviours2 = assetAnimator.GetBehaviours<SheatheCompletedBehaviour>();
				for (int i = 0; i < behaviours2.Length; i++)
				{
					behaviours2[i].ActionCompleted += OnSheatheComplete;
				}
				if (phaseState >= Entity.PhaseState.Phase2)
				{
					Animator animator = assetAnimator;
					int i = (int)phaseState;
					if (animator.GetLayerIndex("Phase" + i) != -1)
					{
						Animator animator2 = assetAnimator;
						Animator animator3 = assetAnimator;
						i = (int)phaseState;
						animator2.SetLayerWeight(animator3.GetLayerIndex("Phase" + i), 1f);
						assetAnimator.SetLayerWeight(assetAnimator.GetLayerIndex("Phase1"), 0f);
						assetAnimator.SetInteger("CurrentPhase", (int)phaseState);
					}
				}
				assetAnimator.SetFloat("Offset", UnityEngine.Random.Range(0f, 1f));
				if (entity.visualState == Entity.State.Dead)
				{
					assetAnimator.Play("Death", 0, 1f);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.ToString());
			}
		}
		if (isStandardRig)
		{
			boneSpine2 = asset.transform.Find("Root/Pelvis/Spine");
			boneSpine3 = asset.transform.Find("Root/Pelvis/Spine/Spine01");
			boneSpine4 = asset.transform.Find("Root/Pelvis/Spine/Spine01/Neck");
		}
		customSFXPlayer = asset.GetComponent<SFXPlayer>();
	}

	public void Close()
	{
		asset = null;
		entity = null;
	}

	public bool PlayAnimation(EntityAnimation entityAnim, float duration = 0f, bool isCancellableByMovement = false, float castSpeed = 1f)
	{
		if (string.IsNullOrEmpty(entityAnim?.name))
		{
			return false;
		}
		if (assetAnimator == null)
		{
			entity.InterruptKeyframeSpell();
			return false;
		}
		if (!HasAnimatorState(entityAnim.name, entityAnim.layer))
		{
			Debug.LogWarning(entityAnim.name + " animation not found on " + entity.name);
			entity.InterruptKeyframeSpell();
			return false;
		}
		if (!HasOverrideAnimation(entityAnim.name) && !assetAnimator.HasAnimatorState("IAmHumanBean"))
		{
			if (entityAnim.name != "Hit")
			{
				Debug.LogWarning("No override animation clip, interrupt and continue: " + entityAnim.name, base.gameObject);
			}
			entity.InterruptKeyframeSpell();
		}
		if (currentAction != null && !entityAnim.ignorePriority && (entityAnim.priority < currentAction.priority || (currentAction.name == entityAnim.name && currentAction.name == "Hit")))
		{
			return false;
		}
		CancelInvoke("CancelAction");
		assetAnimator.ResetTrigger("CancelAction");
		EndTrail();
		if (castSpeed > 0f)
		{
			StopHitPause();
			combatSpeed = 1f / castSpeed;
			assetAnimator.SetFloat("CombatSpeed", combatSpeed);
		}
		if (skipNextCrossfade)
		{
			assetAnimator.Play(entityAnim.name);
			skipNextCrossfade = false;
		}
		else
		{
			assetAnimator.CrossFadeInFixedTime(entityAnim.name, entityAnim.crossfadeSpeed, entityAnim.layer, entityAnim.normalizedTime);
		}
		if (currentAction != null && (currentAction.animationType == EntityAnimation.Type.SHEATHING || currentAction.animationType == EntityAnimation.Type.UNSHEATHING))
		{
			PlayerAssetController playerAssetController = entity.assetController as PlayerAssetController;
			if (playerAssetController != null)
			{
				if (currentAction.animationType == EntityAnimation.Type.SHEATHING)
				{
					playerAssetController.SetWeaponsMounted(toSheathed: true);
				}
				else if (currentAction.animationType == EntityAnimation.Type.UNSHEATHING)
				{
					playerAssetController.SetWeaponsMounted(toSheathed: false);
				}
			}
		}
		currentAction = entityAnim;
		currentAction.isCancellableByMovement = isCancellableByMovement;
		if (duration > 0.1f)
		{
			Invoke("CancelAction", duration);
		}
		return true;
	}

	public void InterruptAnimation()
	{
		skipNextCrossfade = true;
	}

	public void CancelAction()
	{
		currentAction = null;
		EndTrail();
		if (assetAnimator != null)
		{
			assetAnimator.SetTrigger("CancelAction");
		}
	}

	private void OnActionComplete()
	{
		currentAction = null;
		EndTrail();
	}

	private void OnSheatheComplete()
	{
		if (currentAction != null && (currentAction.animationType == EntityAnimation.Type.SHEATHING || currentAction.animationType == EntityAnimation.Type.UNSHEATHING))
		{
			currentAction = null;
		}
		EndTrail();
	}

	private void EndTrail()
	{
		if (entity != null && entity.entitySpots != null)
		{
			entity.entitySpots.EndTrail();
		}
	}

	public void ResetAnimation()
	{
		verticalSpeed = 0f;
		ResetJump();
		if (assetAnimator != null)
		{
			assetAnimator.SetInteger("State", 0);
			assetAnimator.SetFloat("Speed", 1f);
			assetAnimator.SetFloat("CharBlend", 0f);
			assetAnimator.SetFloat("IdleBlend", 0f);
			assetAnimator.Play("Idle", 0, 0f);
		}
	}

	private void ResetJump()
	{
		charState = CharState.Idle;
		inAirDirection = Vector3.zero;
		startJumpDirection = Vector3.zero;
		startJumpState = MovementState.None;
		lastJumpState = MovementState.None;
	}

	public bool IsDoingPriorityAnimation(SpellTemplate spellT)
	{
		if (currentAction != null)
		{
			return currentAction.priority > spellT.GetAnimationPriority();
		}
		return false;
	}

	private void OnEffectAdded(Effect effect)
	{
		if ((effect.template.status & Entity.StatusType.NoMovement) > Entity.StatusType.None)
		{
			string text = "Idle";
			if ((effect.template.status & Entity.StatusType.Stun) > Entity.StatusType.None && HasOverrideAnimation("D_Stun"))
			{
				text = "Stun";
			}
			PlayAnimation(EntityAnimations.Get(text));
		}
	}

	public bool ContainsNPCStaticAnimations()
	{
		if (assetAnimator == null || !assetAnimator.isInitialized)
		{
			return true;
		}
		if (NPC.StaticAnimations.Contains(currentAnimName))
		{
			return true;
		}
		AnimatorClipInfo[] currentAnimatorClipInfo = assetAnimator.GetCurrentAnimatorClipInfo(0);
		foreach (AnimatorClipInfo animatorClipInfo in currentAnimatorClipInfo)
		{
			if (NPC.StaticAnimations.Contains(animatorClipInfo.clip.name))
			{
				return true;
			}
		}
		return false;
	}

	public void Die()
	{
		charState = CharState.Dead;
		StopDash();
		if (assetAnimator != null)
		{
			StopHitPause();
			assetAnimator.SetFloat("Speed", 1f);
			CancelAction();
			assetAnimator.SetInteger("State", (int)charState);
			assetAnimator.CrossFade("Death", 0.1f);
		}
	}

	private void UpdateVerticalSpeed()
	{
		if (IsGrounded())
		{
			verticalSpeed = 0f;
		}
		verticalSpeed -= 20f * Time.deltaTime;
	}

	private void CheckForFallDeath()
	{
		if (entity.isMe && verticalSpeed < -100f && entity.serverState != Entity.State.Dead && GameTime.realtimeSinceServerStartup - lastSelfKillTS > 10f)
		{
			Game.Instance.SendEntityKillRequest();
			lastSelfKillTS = GameTime.realtimeSinceServerStartup;
			ErrorReporting.Instance.ReportError("Fell Through Floor", "Fell Through Floor", "Postion X = " + entity.wrapper.transform.position.x + ", Postion Z = " + entity.wrapper.transform.position.z + " Map Name: " + Game.CurrentAreaName, null, "", null, showMessageBox: false);
		}
	}

	private Vector3 GetDeadMovement()
	{
		return new Vector3(0f, verticalSpeed, 0f) * Time.deltaTime;
	}

	private float CalculateJumpVerticalSpeed(float targetJumpHeight)
	{
		return Mathf.Sqrt(2f * targetJumpHeight * 20f);
	}

	private void OnJumped()
	{
		if (movementController is ClientMovementController clientMovementController)
		{
			if (!CanJump())
			{
				if (jumpBuffer <= 0f)
				{
					StartCoroutine(BufferJump());
				}
				return;
			}
			clientMovementController.BroadcastMovement(forcesync: true, jump: true);
		}
		Jump();
	}

	private bool CanJump()
	{
		if ((IsGrounded() || jumpForgivenessLeft > 0f) && entity.visualState != Entity.State.Dead)
		{
			return !entity.HasStatus(Entity.StatusType.NoMovement);
		}
		return false;
	}

	private IEnumerator BufferJump()
	{
		jumpBuffer = 0.2f;
		while (jumpBuffer > 0f)
		{
			yield return null;
			jumpBuffer -= Time.deltaTime;
			if (CanJump())
			{
				yield return null;
				OnJumped();
				jumpBuffer = 0f;
			}
		}
	}

	private void Jump()
	{
		verticalSpeed = CalculateJumpVerticalSpeed(1.5f);
		collisionFlags = CollisionFlags.None;
		jumpForgivenessLeft = 0f;
		startJumpDirection = playerDirection;
		startJumpDirection.y = 0f;
		startJumpState = movementController.State;
		lastJumpState = movementController.State;
		charState = CharState.Jumping;
		if (assetAnimator != null)
		{
			if (entity.serverState != Entity.State.InCombat && HasAnimatorState("Flip") && ArtixRandom.Range(1, 100) <= 30)
			{
				assetAnimator.CrossFade("Flip", 0.05f);
			}
			else
			{
				assetAnimator.CrossFade("Jump", 0.05f);
			}
		}
	}

	private Vector3 GetMoveDirection()
	{
		Vector3 result = (IsGrounded() ? GetGroundDirection() : GetJumpDirection());
		if (IsBlockingMovement || isDashing)
		{
			result.x = 0f;
			result.z = 0f;
		}
		return result;
	}

	private Vector3 GetGroundDirection()
	{
		Vector3 inputDirection = GetInputDirection();
		inputDirection = Vector3.ClampMagnitude(inputDirection, 1f);
		inputDirection.y = -1f;
		return inputDirection;
	}

	private Vector3 GetJumpDirection()
	{
		if (entity.HasStatus(Entity.StatusType.NoMovement))
		{
			return Vector3.zero;
		}
		Vector3 inputDirection = GetInputDirection();
		if (inputDirection != Vector3.zero)
		{
			if (startJumpDirection == Vector3.zero)
			{
				inAirDirection = inputDirection * 0.5f;
			}
			else if (inputDirection != startJumpDirection)
			{
				inAirDirection += inputDirection * 10f * Time.deltaTime;
				inAirDirection = Vector3.ClampMagnitude(inAirDirection, 1.5f);
			}
		}
		Vector3 result = Vector3.ClampMagnitude(startJumpDirection + inAirDirection, 1f);
		result.y = 0f;
		lastJumpState = movementController.State;
		return result;
	}

	private Vector3 GetInputDirection()
	{
		Vector3 forward = base.transform.forward;
		Vector3 vector = new Vector3(forward.z, 0f, 0f - forward.x);
		Vector3 zero = Vector3.zero;
		if (movementController.State.IsForward())
		{
			zero += forward;
		}
		if (movementController.State.IsBackward())
		{
			zero += -forward;
		}
		if (movementController.State.IsLeftStrafe())
		{
			zero += -vector;
		}
		if (movementController.State.IsRightStrafe())
		{
			zero += vector;
		}
		return zero;
	}

	private float GetMovementSpeed()
	{
		if (movementController is NPCMovementController)
		{
			return ((NPCMovementController)movementController).Speed;
		}
		if (!movementController.State.IsMoving() && startJumpDirection == Vector3.zero && inAirDirection == Vector3.zero)
		{
			return 0f;
		}
		if (Entities.Instance.me != null && Entities.Instance.me.AccessLevel >= 100 && entity.isMe && !UICamera.isOverUI && Input.GetKey(KeyCode.LeftShift))
		{
			return entity.WalkSpeed;
		}
		if (movingBack)
		{
			return entity.BackpedalSpeed;
		}
		if (movingSideways)
		{
			return entity.SideSpeed;
		}
		return entity.RunSpeed;
	}

	private void RotateSpine()
	{
		if (asset.transform.localRotation.y != 0f)
		{
			float y = asset.transform.localRotation.eulerAngles.y;
			y = ((y < 180f) ? y : (y - 360f));
			float num = y / 4f;
			if (num != 0f)
			{
				Quaternion quaternion = Quaternion.Euler(num, 0f, 0f);
				boneSpine2.transform.localRotation *= quaternion;
				boneSpine3.transform.localRotation *= quaternion;
				boneSpine4.transform.localRotation *= quaternion;
			}
		}
	}

	private void MoveController()
	{
		UpdateVerticalSpeed();
		CheckForFallDeath();
		Vector3 movement = GetMovement();
		if (!(movement.magnitude > 0.001f) || !IsMoveWithinBounds(movement))
		{
			return;
		}
		collisionFlags = CharacterController.Move(movement);
		if (movementController is ClientMovementController clientMovementController)
		{
			UpdateJumpForgiveness();
			if (syncAfterDash)
			{
				clientMovementController.BroadcastMovement(forcesync: true);
			}
		}
	}

	private Vector3 GetMovement()
	{
		Vector3 result;
		if (entity.visualState == Entity.State.Dead || charState == CharState.Dead)
		{
			result = GetDeadMovement();
		}
		else if (entity.type == Entity.Type.NPC)
		{
			result = GetNpcMovement();
			moveSpeed = GetMovementSpeed();
		}
		else
		{
			result = GetPlayerMovement();
		}
		return result;
	}

	private Vector3 GetPlayerMovement()
	{
		if (entity.TargetTransform != null && Game.Instance.camController.lockedOnTarget)
		{
			Vector3 vector = entity.position - entity.TargetTransform.position;
			vector.y = 0f;
			if (vector.magnitude < 0.75f)
			{
				Game.Instance.camController.panToTarget = false;
			}
			if (!isDashing && movementController.State.IsStrafeOnly())
			{
				return GetCircleStrafeMovement();
			}
		}
		if (isDashing)
		{
			return GetDashMovement();
		}
		playerDirection = GetMoveDirection();
		moveSpeed = GetMovementSpeed();
		Vector3 vector2 = playerDirection * moveSpeed;
		vector2.y += verticalSpeed;
		return vector2 * Time.deltaTime;
	}

	private Vector3 GetCircleStrafeMovement()
	{
		playerDirection = GetMoveDirection();
		moveSpeed = GetMovementSpeed();
		float y = playerDirection.y;
		Vector3 vector = playerDirection * moveSpeed * Time.deltaTime;
		vector.y = 0f;
		Vector3 vector2 = entity.position - entity.TargetTransform.position;
		vector2.y = 0f;
		float magnitude = vector2.magnitude;
		float num = MathF.PI * 2f * magnitude;
		float num2 = vector.magnitude / num * 360f;
		if (movementController.State.IsRightStrafe())
		{
			num2 *= -1f;
		}
		Vector3 result = Quaternion.AngleAxis(num2, Vector3.up) * vector2 - vector2;
		result.y = y;
		result.y += verticalSpeed * Time.deltaTime;
		return result;
	}

	private Vector3 GetNpcMovement()
	{
		Vector3 result = default(Vector3);
		if (!entity.CanMove)
		{
			return result;
		}
		result = ((NPCMovementController)movementController).Movement;
		if (result.sqrMagnitude > 0.001f || !IsGrounded())
		{
			result.y = verticalSpeed * Time.deltaTime;
		}
		return result;
	}

	public void DashToPosition(Vector3 position, float speed)
	{
		dashStartPosition = entity.position;
		dashEndPosition = position;
		if (!((dashEndPosition - dashStartPosition).magnitude > 25f))
		{
			dashDuration = 0f;
			dashTotalDuration = (dashEndPosition - dashStartPosition).magnitude / speed;
		}
	}

	public void StopDash()
	{
		dashDuration = 0f;
		dashTotalDuration = 0f;
	}

	private Vector3 GetDashMovement()
	{
		if (dashTotalDuration <= 0f)
		{
			return Vector3.zero;
		}
		float t = 1f - Mathf.Pow(1f - dashDuration / dashTotalDuration, 2f);
		Vector3 result = Vector3.Lerp(dashStartPosition, dashEndPosition, t) - entity.wrapperTransform.position;
		if (dashDuration >= dashTotalDuration && movementController is ClientMovementController)
		{
			syncAfterDash = true;
		}
		dashDuration += Time.deltaTime;
		return result;
	}

	public bool IsMoveWithinBounds(Vector3 movement)
	{
		return IsMoveWithinBounds(movement.x, movement.y, movement.z);
	}

	public bool IsMoveWithinBounds(float x, float y, float z)
	{
		if (Mathf.Abs(base.transform.position.x + x) < 10000f && Mathf.Abs(base.transform.position.y + y) < 10000f)
		{
			return Mathf.Abs(base.transform.position.z + z) < 10000f;
		}
		return false;
	}

	private void UpdateAssetController()
	{
		float num = 1f;
		if (entity.HasStatus(Entity.StatusType.Stun) && HasOverrideAnimation("D_Stun"))
		{
			num = 1f;
		}
		else if (entity.HasStatus(Entity.StatusType.StopAnimation))
		{
			num = 0f;
		}
		if (entity.type == Entity.Type.Player && verticalSpeed < -2.5f && charState == CharState.Idle)
		{
			charState = CharState.Falling;
		}
		if (IsGrounded())
		{
			ResetJump();
		}
		if (assetAnimator == null || entity.assetController == null)
		{
			return;
		}
		if ((moveSpeed > 0f || charState != 0) && currentAction != null && !currentAction.canMix && currentAction.isCancellableByMovement)
		{
			CancelAction();
		}
		if (moveSpeed > 0f)
		{
			if (movingBack)
			{
				num *= -1f;
				assetAnimator.SetFloat("Offset", 0f);
			}
			num = ((!(moveSpeed < GetEntityRunSpeed())) ? (num * (entity.RunSpeed / 6.5f)) : (num * (entity.WalkSpeed / 1.5f)));
			num /= asset.transform.lossyScale.y;
		}
		assetAnimator.SetInteger("State", (int)charState);
		assetAnimator.SetFloat("CharBlend", GetCharBlend(), 0.05f, Time.deltaTime);
		assetAnimator.SetFloat("IdleBlend", GetIdleBlend(), 0.2f, Time.deltaTime);
		assetAnimator.SetFloat("Speed", num);
		if (isStandardRig)
		{
			float @float = assetAnimator.GetFloat("MoveBlend");
			float moveBlend = GetMoveBlend();
			float dampTime = ((moveBlend < @float) ? (-0.05f) : 0.05f);
			if ((@float > 0f && movingBack) || (@float < 0f && !movingBack))
			{
				assetAnimator.SetFloat("MoveBlend", 0f);
			}
			assetAnimator.SetFloat("MoveBlend", moveBlend, dampTime, Time.deltaTime);
		}
	}

	public void HitPause(float duration)
	{
		StopHitPause();
		hitPauseRoutine = HitPauseRoutine(duration);
		StartCoroutine(hitPauseRoutine);
	}

	private void StopHitPause()
	{
		if (hitPauseRoutine != null)
		{
			StopCoroutine(hitPauseRoutine);
		}
		assetAnimator.SetFloat("CombatSpeed", combatSpeed);
	}

	private IEnumerator HitPauseRoutine(float duration)
	{
		combatSpeed = assetAnimator.GetFloat("CombatSpeed");
		assetAnimator.SetFloat("CombatSpeed", 0.1f);
		yield return new WaitForSeconds(duration);
		assetAnimator.SetFloat("CombatSpeed", combatSpeed);
	}

	private float GetCharBlend()
	{
		float value = 0f;
		if (moveSpeed > 0f)
		{
			value = ((moveSpeed < GetEntityRunSpeed()) ? 0.5f : 1f);
		}
		return Mathf.Clamp(value, 0.001f, 1f);
	}

	private float GetIdleBlend()
	{
		if (entity.visualState != Entity.State.InCombat)
		{
			return 0f;
		}
		return 0.5f;
	}

	private float GetMoveBlend()
	{
		return ((moveSpeed < GetEntityRunSpeed()) ? 0.5f : 1f) * (float)((!movingBack) ? 1 : (-1));
	}

	private void UpdateRotation()
	{
		if (entity.type == Entity.Type.Player && charState == CharState.Idle)
		{
			Vector3 toDirection = new Vector3(playerDirection.x, 0f, playerDirection.z);
			if (movingBack)
			{
				toDirection *= -1f;
			}
			strafeRotation = Quaternion.FromToRotation(base.transform.forward, toDirection);
			Quaternion quaternion = Quaternion.Lerp(asset.transform.localRotation, strafeRotation, Time.deltaTime * 6f);
			asset.transform.localRotation = Quaternion.Euler(0f, (quaternion.eulerAngles.y > 1f) ? quaternion.eulerAngles.y : 0f, 0f);
		}
		entity.position = base.transform.position;
		entity.rotation = base.transform.rotation;
	}

	private void CheckForTargetDeselect()
	{
		if (entity == null || entity != Entities.Instance.me || entity.TargetTransform == null || (entity.Target != null && PartyManager.IsMember(entity.Target.ID)))
		{
			return;
		}
		float magnitude = ((Vector3)(entity.TargetTransform.position.xz() - entity.position.xz())).magnitude;
		if (magnitude > Game.Max_Click_Distance + 5f)
		{
			entity.Target = null;
			entity.TargetNode = null;
		}
		else
		{
			if (!movementController.IsMoving())
			{
				return;
			}
			if (entity.Target != null)
			{
				if (!entity.Target.InCameraView() && ((magnitude > Game.Max_Click_Distance - 20f && entity.serverState != Entity.State.InCombat) || magnitude > Game.Max_Click_Distance - 5f))
				{
					entity.Target = null;
					entity.TargetNode = null;
				}
			}
			else if (entity.TargetNode != null && !entity.TargetNode.InCameraView() && ((magnitude > Game.Max_Click_Distance - 20f && entity.serverState != Entity.State.InCombat) || magnitude > Game.Max_Click_Distance - 5f))
			{
				entity.Target = null;
				entity.TargetNode = null;
			}
		}
	}

	private float GetEntityRunSpeed()
	{
		return (entity.RunSpeed - entity.WalkSpeed) / 2f;
	}

	public bool IsGrounded()
	{
		if ((collisionFlags & CollisionFlags.Below) == 0 && CharacterController.enabled)
		{
			return !entity.CanMove;
		}
		return true;
	}

	private void UpdateJumpForgiveness()
	{
		bool flag = (collisionFlags & CollisionFlags.Below) != 0;
		if (lastGroundedState && !flag)
		{
			lastGroundedState = false;
			StartCoroutine(JumpForgivenessRoutine());
		}
		else if (flag)
		{
			lastGroundedState = true;
			jumpForgivenessLeft = JumpForgivenessDuration;
		}
	}

	private IEnumerator JumpForgivenessRoutine()
	{
		while (jumpForgivenessLeft > 0f && !lastGroundedState)
		{
			yield return null;
			jumpForgivenessLeft -= Time.deltaTime;
		}
	}
}
