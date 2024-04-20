using System;
using Assets.Scripts.Game;
using UnityEngine;

public abstract class GizmoHandle : MonoBehaviour, IInteractable
{
	protected static bool IsAnyHandleDragging;

	protected bool hovered;

	protected bool dragging;

	protected Color color;

	protected Vector3 ClickOffset;

	private Color addedColor = new Color(0.7f, 0.7f, 0.7f, 0.3f);

	private Color subtractedColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);

	public Action onDragStart;

	public Action onDragEnd;

	public int Priority = 1;

	private void Start()
	{
		Material material = base.transform.GetChild(0).GetComponent<MeshRenderer>().material;
		color = material.GetColor("_Color");
	}

	private void Update()
	{
		Material material = base.transform.GetChild(0).GetComponent<MeshRenderer>().material;
		if ((hovered && !IsAnyHandleDragging) || dragging)
		{
			material.SetColor("_Color", color + addedColor);
			CursorManager.Instance.SetCursor(CursorManager.Icon.Interact);
		}
		else if (IsAnyHandleDragging)
		{
			material.SetColor("_Color", color - subtractedColor);
		}
		else
		{
			material.SetColor("_Color", color);
		}
		OnUpdate();
		hovered = false;
	}

	protected virtual void OnUpdate()
	{
	}

	public void SetColor(Color color)
	{
		this.color = color;
	}

	public void OnHover()
	{
		hovered = true;
	}

	public virtual void OnDragStart(Game.InteractableRaycastResult raycastResult)
	{
	}

	public virtual void OnDragEnd()
	{
	}

	public virtual void OnPress(Game.InteractableRaycastResult raycastResult, bool isPressed)
	{
		if (isPressed)
		{
			dragging = true;
			IsAnyHandleDragging = true;
			ClickOffset = raycastResult.Hit.point - base.transform.position;
			onDragStart?.Invoke();
		}
		else
		{
			dragging = false;
			IsAnyHandleDragging = false;
			onDragEnd?.Invoke();
		}
	}

	public virtual bool IsInteractable(Game.InteractableRaycastResult raycastResult)
	{
		return true;
	}

	public int GetPriority()
	{
		return Priority;
	}

	public void OnSecondTouchPress(Game.InteractableRaycastResult raycastResult, bool isPressed)
	{
	}
}
