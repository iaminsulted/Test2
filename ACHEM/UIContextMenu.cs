using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIContextMenu : MonoBehaviour
{
	public GameObject itemGOprefab;

	private List<UIContextMenuItem> itemGOs;

	private ObjectPool<GameObject> itemGOpool;

	public Camera uiCamera;

	public UICamera nguiCamera;

	public UIGrid grid;

	public List<string> Options;

	public IContext parent;

	public static UIContextMenu instance;

	private void OnEnable()
	{
		UICamera.onClick = (UICamera.VoidDelegate)Delegate.Combine(UICamera.onClick, new UICamera.VoidDelegate(cameraClick));
	}

	private void OnDisable()
	{
		UICamera.onClick = (UICamera.VoidDelegate)Delegate.Remove(UICamera.onClick, new UICamera.VoidDelegate(cameraClick));
	}

	private void OnDestroy()
	{
		instance = null;
	}

	public static void Show(IContext p, List<string> opts)
	{
		Show(p, null, opts);
	}

	public static void Show(IContext p, Transform t, List<string> opts)
	{
		Close();
		instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/ContextMenu"), UIManager.Instance.transform).GetComponent<UIContextMenu>();
		instance.uiCamera = UICamera.currentCamera;
		instance.Options = opts;
		instance.parent = p;
		instance.refresh(t);
	}

	public void ContextSelect(int selected)
	{
		if (parent != null)
		{
			parent.ContextSelect(selected);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void refresh(Transform t)
	{
		if (grid != null)
		{
			itemGOs = new List<UIContextMenuItem>();
			itemGOpool = new ObjectPool<GameObject>(itemGOprefab);
			itemGOprefab.SetActive(value: false);
			foreach (UIContextMenuItem itemGO in itemGOs)
			{
				itemGO.gameObject.transform.SetAsLastSibling();
				itemGOpool.Release(itemGO.gameObject);
			}
			itemGOs.Clear();
			int num = 0;
			foreach (string option in Options)
			{
				GameObject obj = itemGOpool.Get();
				obj.transform.SetParent(grid.gameObject.transform, worldPositionStays: false);
				obj.SetActive(value: true);
				UIContextMenuItem component = obj.GetComponent<UIContextMenuItem>();
				component.Init(num, option);
				itemGOs.Add(component);
				num++;
			}
			int width = Mathf.RoundToInt(itemGOs.Max((UIContextMenuItem p) => p.Label.printedSize.x)) + 18;
			foreach (UIContextMenuItem itemGO2 in itemGOs)
			{
				itemGO2.GetComponent<UISprite>().width = width;
			}
			grid.Reposition();
		}
		if (t == null)
		{
			SetPos(Input.mousePosition);
		}
		else
		{
			SetPos(instance.uiCamera.WorldToScreenPoint(t.position));
		}
	}

	public void SetPos(Vector3 pos)
	{
		Vector3 position = pos;
		Vector3 vector = new Vector3(grid.cellWidth / (float)Screen.width, grid.cellHeight * (float)(Options.Count + 1) / (float)Screen.height, 0f);
		if (uiCamera != null)
		{
			position.x = Mathf.Clamp01(position.x / (float)Screen.width);
			position.y = Mathf.Clamp01(position.y / (float)Screen.height);
			float num = uiCamera.orthographicSize / base.transform.parent.lossyScale.y;
			float num2 = (float)Screen.height * 0.5f / num;
			Vector2 vector2 = new Vector2(num2 * vector.x, num2 * vector.y);
			position.x = Mathf.Min(position.x, 1f - vector.x);
			position.y = Mathf.Max(position.y, vector2.y);
			base.transform.position = uiCamera.ViewportToWorldPoint(position);
			base.transform.localPosition = base.transform.localPosition.Round();
		}
	}

	public static void Close()
	{
		if (instance != null)
		{
			UnityEngine.Object.Destroy(instance.gameObject);
		}
	}

	public void cameraClick(GameObject go)
	{
		if (go != null && go.transform.parent != null && go.transform.parent.parent != base.transform)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
