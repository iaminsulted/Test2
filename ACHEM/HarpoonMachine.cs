using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interaction/Machines/Harpoon Machine")]
public class HarpoonMachine : SpellMachine
{
	public const float DegreesPerSecond = 60f;

	public List<MachineAction> fireActions = new List<MachineAction>();

	public Transform cam;

	public Transform firePoint;

	public GameObject fireFX;

	public AudioSource fireSFX;

	public int fireRate;

	public Transform horizontalPivot;

	public float horizontalAngleClamp;

	public Transform verticalPivot;

	public float verticalDownAngleClamp;

	public float verticalUpAngleClamp;

	public GameObject harpoonPrefab;

	public float harpoonVelocity;

	public float harpoonDuration;

	public int itemID;

	private HarpoonMachineController controller;

	private Transform spellFXTransform;

	private bool syncHorizontalRot;

	private bool syncVerticalRot;

	private Vector3 currentHorizontalEuler;

	private Vector3 currentVerticalEuler;

	private float targetHorizontalEulerY;

	private float targetVerticalEulerX;

	private float syncHorizontalTime;

	private float syncVerticalTime;

	public override void Start()
	{
		base.Start();
		spellFXTransform = SpellFXContainer.mInstance.transform;
		InitializeController();
		OwnLocally = (Action)Delegate.Combine(OwnLocally, new Action(OnOwnLocally));
		DisownLocally = (Action)Delegate.Combine(DisownLocally, new Action(OnDisownLocally));
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		OwnLocally = (Action)Delegate.Remove(OwnLocally, new Action(OnOwnLocally));
		DisownLocally = (Action)Delegate.Remove(DisownLocally, new Action(OnDisownLocally));
	}

	public void Update()
	{
		if (syncHorizontalRot)
		{
			syncHorizontalTime += Time.deltaTime * 60f;
			horizontalPivot.localEulerAngles = new Vector3(currentHorizontalEuler.x, Mathf.Lerp(currentHorizontalEuler.y, targetHorizontalEulerY, syncHorizontalTime), currentHorizontalEuler.z);
			if (Mathf.Abs(targetHorizontalEulerY - horizontalPivot.localEulerAngles.y) < 1f)
			{
				syncHorizontalRot = false;
			}
		}
		if (syncVerticalRot)
		{
			syncVerticalTime += Time.deltaTime * 60f;
			verticalPivot.localEulerAngles = new Vector3(Mathf.Lerp(currentVerticalEuler.x, targetVerticalEulerX, syncVerticalTime), currentVerticalEuler.y, currentVerticalEuler.z);
			if (Mathf.Abs(targetVerticalEulerX - verticalPivot.localEulerAngles.x) < 1f)
			{
				syncVerticalRot = false;
			}
		}
	}

	public void Sync(float verticalRotation, float horizontalRotation, bool snap = false)
	{
		if (verticalRotation > 180f)
		{
			verticalRotation -= 360f;
		}
		if (horizontalRotation > 180f)
		{
			horizontalRotation -= 360f;
		}
		if (snap)
		{
			Vector3 localEulerAngles = verticalPivot.localEulerAngles;
			Vector3 localEulerAngles2 = horizontalPivot.localEulerAngles;
			localEulerAngles.x = verticalRotation;
			localEulerAngles2.y = horizontalRotation;
			verticalPivot.localEulerAngles = localEulerAngles;
			horizontalPivot.localEulerAngles = localEulerAngles2;
		}
		else
		{
			currentVerticalEuler = verticalPivot.localEulerAngles;
			currentHorizontalEuler = horizontalPivot.localEulerAngles;
			targetVerticalEulerX = verticalRotation;
			targetHorizontalEulerY = horizontalRotation;
			syncHorizontalRot = true;
			syncVerticalRot = true;
			syncHorizontalTime = 0f;
			syncVerticalTime = 0f;
		}
	}

	public void Fire()
	{
		if (fireFX != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(fireFX, firePoint.position, firePoint.rotation, spellFXTransform);
			ParticleSystem[] componentsInChildren = gameObject.GetComponentsInChildren<ParticleSystem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Play();
			}
			UnityEngine.Object.Destroy(gameObject, 3f);
		}
		if (fireSFX != null)
		{
			fireSFX.Play();
		}
		StartCoroutine(InstantiateHarpoon());
	}

	public override bool HasQuestObjective(int qoid)
	{
		foreach (MachineAction fireAction in fireActions)
		{
			if (fireAction is MAQuestObjective mAQuestObjective)
			{
				if (mAQuestObjective.QOID == qoid)
				{
					return true;
				}
			}
			else if (fireAction is MARandomAction mARandomAction)
			{
				foreach (MachineAction action in mARandomAction.Actions)
				{
					if (action is MAQuestObjective mAQuestObjective2 && mAQuestObjective2.QOID == qoid)
					{
						return true;
					}
				}
			}
			else
			{
				if (!(fireAction is CompletionAction completionAction))
				{
					continue;
				}
				foreach (MachineAction completionAction2 in completionAction.completionActions)
				{
					if (completionAction2 is MAQuestObjective mAQuestObjective3 && mAQuestObjective3.QOID == qoid)
					{
						return true;
					}
				}
			}
		}
		return base.HasQuestObjective(qoid);
	}

	private void OnOwnLocally()
	{
		Game.Instance.camController.enabled = false;
		Game.Instance.DisableMovementController();
		Game.Instance.isInputEnabled = false;
		base.Owner.moveController.Stop();
		base.Owner.entitycontroller.CancelAction();
		controller.Enable();
	}

	private void OnDisownLocally()
	{
		controller.Disable();
		Game.Instance.camController.enabled = true;
		Game.Instance.EnablePlayerController();
		Game.Instance.isInputEnabled = true;
	}

	private void InitializeController()
	{
		controller = base.gameObject.AddComponent<HarpoonMachineController>();
		controller.machine = this;
		controller.cam = cam;
		controller.horizontalPivot = horizontalPivot;
		controller.horizontalAngleClamp = horizontalAngleClamp;
		controller.verticalPivot = verticalPivot;
		controller.verticalDownAngleClamp = verticalDownAngleClamp;
		controller.verticalUpAngleClamp = verticalUpAngleClamp;
		controller.firePoint = firePoint;
		controller.fireCooldown = 60f / (float)fireRate;
		controller.itemID = itemID;
		controller.Init();
	}

	private IEnumerator InstantiateHarpoon()
	{
		yield return new WaitForSeconds(0.25f);
		Projectile projectile = UnityEngine.Object.Instantiate(harpoonPrefab, firePoint.position, firePoint.rotation, spellFXTransform).AddComponent<Projectile>();
		projectile.velocity = harpoonVelocity;
		projectile.duration = harpoonDuration;
	}
}
