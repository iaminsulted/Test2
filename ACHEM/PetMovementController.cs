using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using StatCurves;
using UnityEngine;

public class PetMovementController : MonoBehaviour
{
	private const float rotSpeed = 30f;

	private const float maxDistance = 8f;

	private const float minDistance = 2f;

	private const float teleportDistance = 20f;

	private const float Gravity = 20f;

	public Entity owner;

	public Transform originalParent;

	private GameObject petTriggerPrefab;

	private NPCAssetController petAssetController;

	private Transform target;

	private Animator animator;

	private CharacterController characterController;

	private CollisionFlags collisionFlags;

	private float fallSpeed;

	private bool hasMoveBlend;

	public Animator getAnimator
	{
		get
		{
			if (animator == null)
			{
				animator = base.transform.GetComponentInChildren<Animator>();
			}
			return animator;
		}
	}

	public void Init(Entity entity, string assetName, BundleInfo bundle, List<string> animations, string ColorR, string ColorG, string ColorB)
	{
		owner = entity;
		target = owner.wrapperTransform;
		base.transform.SetParent(target.parent, worldPositionStays: false);
		base.transform.position = target.position - target.forward * 2f;
		characterController = base.gameObject.AddComponent<CharacterController>();
		characterController.height = 2f;
		characterController.radius = 0.5f;
		characterController.center = new Vector3(0f, 1.08f, 0f);
		characterController.slopeLimit = 180f;
		characterController.stepOffset = 0.4f;
		petAssetController = base.gameObject.AddComponent<NPCAssetController>();
		petAssetController.Init(new EntityAsset
		{
			prefab = assetName,
			bundle = bundle,
			ColorR = ColorR,
			ColorG = ColorG,
			ColorB = ColorB
		});
		petAssetController.LoadPet();
		owner.Destroyed += OnOwnerDestroyed;
		petAssetController.AssetUpdated += OnAssetComplete;
		petTriggerPrefab = base.gameObject.AddChild(Resources.Load("PetTrigger") as GameObject);
		petTriggerPrefab.GetComponent<PetTrigger>().Init(owner, animations);
	}

	public void ProcessPetInteractions(Player player, string animation)
	{
		if (hasMoveBlend)
		{
			PlayAnimatorState(player, animation);
			return;
		}
		string[] array = Regex.Split(animation, "(?=[0123456789])");
		if (array[0] == "Slot")
		{
			PlayAnimatorStateMonsterSlot(player, string.Concat(array.Skip(1)));
		}
		else
		{
			PlayAnimatorState(player, animation);
		}
	}

	private void Start()
	{
		MovePetAboveGround();
		originalParent = base.transform.parent;
	}

	private void Update()
	{
		if (getAnimator == null || base.transform == null || target == null)
		{
			return;
		}
		float num = Vector2.Distance(base.transform.position.xz(), target.position.xz());
		float num2 = Vector3.Distance(base.transform.position, target.position);
		if (num2 < 1f)
		{
			getAnimator.SetFloat("Speed", 1f);
			getAnimator.SetFloat("CharBlend", 0f, 0.1f, Time.deltaTime);
			if (hasMoveBlend)
			{
				getAnimator.SetFloat("MoveBlend", 0f, 0.5f, Time.deltaTime);
			}
		}
		else if (num2 >= 20f || (num < 0.1f && num2 > 2f))
		{
			base.transform.position = target.position - target.forward * 2f;
			MovePetAboveGround();
		}
		else if (num2 > 2f)
		{
			float num3 = (Mathf.Clamp(num2, 2f, 8f) - 2f) * 0.8f / 6f + 0.2f;
			float num4 = num3 * owner.RunSpeed;
			float y = Quaternion.LookRotation(target.position - base.transform.position).eulerAngles.y;
			Quaternion b = Quaternion.Euler(0f, y, 0f);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, 30f * Time.deltaTime);
			Vector3 vector = base.transform.forward * num4;
			bool flag = (collisionFlags & CollisionFlags.Below) != 0;
			fallSpeed = (flag ? 0f : (fallSpeed - 20f * Time.deltaTime));
			vector += Vector3.up * fallSpeed;
			collisionFlags = characterController.Move(vector * Time.deltaTime);
			if (getAnimator != null)
			{
				float value = owner.RunSpeed / 6.5f;
				num3 = 1f - (1f - num3) * (1f - num3);
				getAnimator.SetFloat("Speed", value);
				getAnimator.SetFloat("CharBlend", num3, 0.1f, Time.deltaTime);
				if (hasMoveBlend)
				{
					getAnimator.SetTrigger("CancelAction");
					getAnimator.SetFloat("MoveBlend", num3, 0.5f, Time.deltaTime);
				}
				else
				{
					getAnimator.SetTrigger("CancelPetAction");
				}
			}
		}
		else
		{
			getAnimator.SetFloat("Speed", 1f);
			getAnimator.SetFloat("CharBlend", 0f, 0.1f, Time.deltaTime);
			if (hasMoveBlend)
			{
				getAnimator.ResetTrigger("CancelAction");
				getAnimator.SetFloat("MoveBlend", 0f, 0.5f, Time.deltaTime);
			}
			else
			{
				getAnimator.ResetTrigger("CancelPetAction");
			}
		}
	}

	private void OnDestroy()
	{
		owner.Destroyed -= OnOwnerDestroyed;
		petAssetController.AssetUpdated -= OnAssetComplete;
	}

	private void OnOwnerDestroyed()
	{
		Object.Destroy(base.gameObject);
	}

	private void OnAssetComplete(GameObject go)
	{
		hasMoveBlend = getAnimator.HasAnimatorState("IAmHumanBean");
		EntityAssetData componentInChildren = go.GetComponentInChildren<EntityAssetData>();
		if (componentInChildren != null && componentInChildren.HitSpot != null)
		{
			petTriggerPrefab.transform.SetParent(componentInChildren.HitSpot);
			petTriggerPrefab.transform.localPosition = Vector3.zero;
			petTriggerPrefab.GetComponent<SphereCollider>().center = Vector3.zero;
		}
	}

	private void MovePetAboveGround()
	{
		base.transform.localPosition += Vector3.up;
		if (Physics.Raycast(new Ray(base.transform.position + Vector3.up, -Vector3.up), out var hitInfo, 8f, Layers.MASK_GROUNDTRACK))
		{
			base.transform.position = hitInfo.point;
		}
		collisionFlags = CollisionFlags.Below;
	}

	private void NotifyPetAnimationStateNotFound(Player player, string animation)
	{
		Chat.SendAdminMessage("Animation state '" + animation + "' not found on item ID " + player.currentAsset.equips[EquipItemSlot.Pet].ID + ". Remove or rename the animation in admin.");
	}

	private void PlayAnimatorStateMonsterSlot(Player player, string index)
	{
		if (!(getAnimator.GetFloat("CharBlend") <= 0.5f))
		{
			return;
		}
		RuntimeAnimatorController runtimeAnimatorController = getAnimator.runtimeAnimatorController;
		if (runtimeAnimatorController is AnimatorOverrideController)
		{
			AnimatorOverrideController animatorOverrideController = runtimeAnimatorController as AnimatorOverrideController;
			AnimationClip animationClip = animatorOverrideController["D_Cast" + index];
			if (animationClip != null && !animationClip.name.Contains("D_"))
			{
				getAnimator.CrossFade("Cast" + index, 0.25f);
				return;
			}
			animationClip = animatorOverrideController["D_Skill" + index];
			if (animationClip != null && !animationClip.name.Contains("D_"))
			{
				getAnimator.CrossFade("Skill" + index, 0.25f);
				return;
			}
		}
		NotifyPetAnimationStateNotFound(player, "Slot" + index);
	}

	private void PlayAnimatorState(Player player, string animation)
	{
		if (getAnimator.HasAnimatorState(animation))
		{
			if (getAnimator.GetFloat("CharBlend") <= 0.5f)
			{
				getAnimator.CrossFadeInFixedTime(animation, 0.25f);
			}
		}
		else
		{
			NotifyPetAnimationStateNotFound(player, animation);
		}
	}
}
