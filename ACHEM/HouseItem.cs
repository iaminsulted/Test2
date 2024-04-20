using System;
using System.Linq;
using UnityEngine;

public class HouseItem : MonoBehaviour, IClickable, IInteractable, IDraggable
{
	public int ID;

	public int PlacerID;

	public Vector3 ClickOffset;

	public int ItemID;

	private DateTime placeTime;

	public Rigidbody Rb;

	public Renderer Renderer;

	private Color SolidColor;

	private Color LiquidColor = new Color(0.7f, 0.7f, 0.7f, 0.7f);

	private bool IsSolid = true;

	private int PLACEMENT_LAYERS;

	private Vector2 firstTouchScreenPos;

	private DateTime pressTime;

	private Vector3 pressPosition;

	private Quaternion pressRotation;

	private Vector3 pressScale;

	private Vector2 firstDragPos;

	private Vector2 secondTouchScreenPos;

	public bool Locked { get; private set; }

	public void CreateRigidbody()
	{
		if (Rb == null)
		{
			Rb = base.gameObject.AddComponent<Rigidbody>();
			Rb.isKinematic = true;
		}
	}

	public void Init()
	{
		PLACEMENT_LAYERS = Layers.DEFAULT | Layers.CLOSE | Layers.MIDDLE | Layers.MIDDLEFAR | Layers.FAR | Layers.TERRAIN;
		CreateRigidbody();
		Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
		foreach (Collider collider in componentsInChildren)
		{
			if (!(collider.gameObject == base.gameObject))
			{
				ClickForwarder clickForwarder = collider.gameObject.GetComponent<ClickForwarder>();
				if (clickForwarder == null)
				{
					clickForwarder = collider.gameObject.AddComponent<ClickForwarder>();
				}
				clickForwarder.iaParent = this;
			}
		}
		SetCollisionEnabled();
	}

	public void LockUntilServerSync()
	{
		Locked = true;
	}

	public void ComSync(ComHouseItem comHouseItem)
	{
		Locked = false;
		ID = comHouseItem.ID;
		ItemID = comHouseItem.ItemID;
		base.transform.position = comHouseItem.Position;
		base.transform.localScale = comHouseItem.Scale;
		base.transform.rotation = Quaternion.Euler(comHouseItem.Rotation);
		PlacerID = comHouseItem.PlacerID;
	}

	public void Move(ComHouseItemMove comMove)
	{
		Locked = false;
		ID = comMove.ID;
		base.transform.position = comMove.Position;
		base.transform.localScale = comMove.Scale;
		base.transform.rotation = Quaternion.Euler(comMove.Rotation);
		PlacerID = comMove.PlacerID;
		if (PlacerID <= 0)
		{
			placeTime = GameTime.ServerTime;
		}
	}

	public ComHouseItemMove GenerateMove()
	{
		return new ComHouseItemMove
		{
			ID = ID,
			Position = base.transform.position,
			Rotation = base.transform.rotation.eulerAngles,
			Scale = base.transform.localScale,
			PlacerID = PlacerID
		};
	}

	public ComHouseItem GenerateAdd()
	{
		return new ComHouseItem
		{
			ID = ID,
			Position = base.transform.position,
			Rotation = base.transform.rotation.eulerAngles,
			Scale = base.transform.localScale,
			PlacerID = -1,
			ItemID = ItemID
		};
	}

	private void SetCollisionEnabled()
	{
		Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			MeshCollider meshCollider = componentsInChildren[i] as MeshCollider;
			if (meshCollider != null)
			{
				meshCollider.convex = false;
			}
		}
	}

	private void SetCollisionDisabled()
	{
		Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			MeshCollider meshCollider = componentsInChildren[i] as MeshCollider;
			if (meshCollider != null)
			{
				meshCollider.convex = true;
			}
		}
	}

	private void unhighlight()
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in componentsInChildren)
		{
			if (renderer.enabled && !(renderer is ParticleSystemRenderer) && renderer.sharedMaterials != null && renderer.sharedMaterials.Any((Material p) => p != null && p.shader.name == "AE/AQ3D/Selection Outline"))
			{
				renderer.sharedMaterials = renderer.sharedMaterials.Where((Material p) => p.shader.name != "AE/AQ3D/Selection Outline").ToArray();
			}
		}
	}

	private void highlight()
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		Shader shader = Shader.Find("AE/AQ3D/Selection Outline");
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			if (renderer.enabled && !(renderer is ParticleSystemRenderer) && renderer.sharedMaterials != null && renderer.sharedMaterial.renderQueue < 2400)
			{
				Material[] array2 = new Material[2]
				{
					renderer.sharedMaterial,
					new Material(shader)
				};
				array2[1].SetColor("_OutlineColor", new Color(0f, 1f, 0f));
				renderer.sharedMaterials = array2;
			}
		}
	}

	public void Solidify()
	{
		SetCollisionEnabled();
		SelectionManager.Instance.Deselect(base.transform);
		IsSolid = true;
	}

	public void Liquify()
	{
		SetCollisionDisabled();
		SelectionManager.Instance.Select(base.transform);
		IsSolid = false;
	}

	public void OnClick(Vector3 hitpoint)
	{
		if (Locked)
		{
			Debug.LogWarning("Clicked com locked object");
		}
		else if (IsSolid && placeTime.AddSeconds(0.2) < GameTime.ServerTime)
		{
			ClickOffset = hitpoint - base.transform.position;
			HousingManager.houseInstance.OnHouseItemClick(this);
		}
	}

	public void OnHover()
	{
	}

	public void OnDoubleClick()
	{
	}

	public void OnPress(Game.InteractableRaycastResult raycastResult, bool isPressed)
	{
		if (isPressed)
		{
			if (raycastResult != null)
			{
				firstTouchScreenPos = raycastResult.ScreenPos;
				ClickOffset = raycastResult.Hit.point - base.transform.position;
			}
			pressTime = DateTime.UtcNow;
			pressPosition = base.transform.position;
			pressRotation = base.transform.rotation;
			pressScale = base.transform.localScale;
			if (IsDraggable(raycastResult) && Input.GetKey(KeyCode.LeftControl))
			{
				HousingManager.houseInstance.DuplicateSelected();
			}
		}
	}

	public void CastObject(Vector2 screenPoint)
	{
		Camera cam = Game.Instance.cam;
		_ = Game.Instance.camController;
		cam.depthTextureMode = DepthTextureMode.Depth;
		if (!(cam == null))
		{
			Ray ray = cam.ScreenPointToRay(screenPoint);
			Vector3 position = base.transform.position;
			base.transform.position = Vector3.zero;
			Bounds bounds = default(Bounds);
			Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
			foreach (Collider collider in componentsInChildren)
			{
				bounds.Encapsulate(collider.bounds);
			}
			ray.origin -= ClickOffset;
			if (Physics.Raycast(ray, out var hitInfo, 200f, PLACEMENT_LAYERS) && hitInfo.distance < bounds.size.magnitude)
			{
				base.transform.position = position;
			}
			ray.origin += ray.direction.normalized * bounds.extents.magnitude;
			base.transform.position = ray.origin;
			bool num = !base.gameObject.activeSelf;
			base.gameObject.SetActive(value: true);
			if (Rb.SweepTest(ray.direction, out hitInfo, 300f))
			{
				base.transform.position = ray.GetPoint(hitInfo.distance);
			}
			else
			{
				base.transform.position = position;
				Debug.LogWarning("Prev pos");
			}
			if (num)
			{
				base.gameObject.SetActive(value: false);
			}
		}
	}

	public void OnDrag(Game.InteractableRaycastResult raycastResult, Game.InteractableRaycastResult prevRaycastResult)
	{
		firstTouchScreenPos = raycastResult.ScreenPos;
		UICamera.MouseOrTouch mouseOrTouch = ((UIGame.ControlScheme != ControlScheme.HANDHELD && !Platform.IsMobile) ? UICamera.GetTouch(-1, createIfMissing: false) : UICamera.GetTouch(0, createIfMissing: false));
		CastObject(mouseOrTouch.pos);
	}

	public void OnDragStart(Game.InteractableRaycastResult raycastResult)
	{
	}

	public void OnDragEnd()
	{
	}

	public bool IsDraggable(Game.InteractableRaycastResult raycastResult)
	{
		if (IsSolid)
		{
			return false;
		}
		if ((GetComponent<GizmoWrapper>() != null) & Gizmo.isArrowsShowing)
		{
			return false;
		}
		return true;
	}

	public bool IsInteractable(Game.InteractableRaycastResult raycastResult)
	{
		if (!HousingManager.houseInstance.IsEditing)
		{
			return false;
		}
		return true;
	}

	public int GetPriority()
	{
		return 0;
	}

	public void OnSecondTouchPress(Game.InteractableRaycastResult raycastResult, bool isPressed)
	{
		secondTouchScreenPos = raycastResult.ScreenPos;
	}

	public bool IsSecondTouchDraggable(Game.InteractableRaycastResult raycastResult)
	{
		return true;
	}

	public void OnSecondTouchDrag(Game.InteractableRaycastResult raycastResult, Game.InteractableRaycastResult prevRaycastResult)
	{
		float num = Vector2.Distance(firstTouchScreenPos, secondTouchScreenPos);
		float num2 = Vector2.Distance(firstTouchScreenPos, raycastResult.ScreenPos);
		float num3 = Vector2.SignedAngle(secondTouchScreenPos - firstTouchScreenPos, raycastResult.ScreenPos - firstTouchScreenPos);
		secondTouchScreenPos = raycastResult.ScreenPos;
		float num4 = num2 - num;
		base.transform.localScale *= 1f + num4 * 0.001f;
		base.transform.Rotate(0f, (0f - num3) * 2f, 0f, Space.World);
	}
}
