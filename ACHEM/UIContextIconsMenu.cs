using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIContextIconsMenu : MonoBehaviour
{
	public enum IconImg
	{
		Inspect,
		Goto,
		LeaveParty,
		PromoteToLeader,
		RemoveFromParty,
		InviteParty,
		InviteGuild,
		AddFriend,
		Duel,
		Ignore,
		Report,
		Whisper,
		VoteKick
	}

	public GameObject itemGOprefab;

	private List<UIContextIconsMenuItem> itemGOs;

	private ObjectPool<GameObject> itemGOpool;

	public Camera uiCamera;

	public UICamera nguiCamera;

	public UIGrid grid;

	public List<string> Options;

	public IiconsContext parent;

	public static UIContextIconsMenu instance;

	private List<string> allIconImgs = new List<string>
	{
		"Icon-FriendInspect", "Icon-FriendGoto", "Icon-FriendPartyCancel", "Icon-FriendLeader", "Icon-FriendRemove", "Icon-FriendParty", "icon_guildadd", "Icon-FriendAdd", "Icon-FriendDuel", "Icon-FriendBlock",
		"Icon-FriendReport", "Icon-FriendChat", "Icon-VoteKick"
	};

	public List<string> usedIconImgs = new List<string>();

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

	public static void Show(IiconsContext p, List<string> opts, List<IconImg> imgIcons)
	{
		Show(p, null, opts, imgIcons);
	}

	public static void Show(IiconsContext p, Transform t, List<string> opts, List<IconImg> imgIcons)
	{
		Close();
		instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/ContextIconsMenu"), UIManager.Instance.transform).GetComponent<UIContextIconsMenu>();
		instance.uiCamera = UICamera.currentCamera;
		int num = 0;
		foreach (IconImg imgIcon in imgIcons)
		{
			instance.usedIconImgs.Add(instance.allIconImgs.ElementAt((int)imgIcon));
			num++;
		}
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
			itemGOs = new List<UIContextIconsMenuItem>();
			itemGOpool = new ObjectPool<GameObject>(itemGOprefab);
			itemGOprefab.SetActive(value: false);
			foreach (UIContextIconsMenuItem itemGO in itemGOs)
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
				UIContextIconsMenuItem component = obj.GetComponent<UIContextIconsMenuItem>();
				component.Init(num, option, instance.usedIconImgs.ElementAt(num));
				itemGOs.Add(component);
				num++;
			}
			int width = Mathf.RoundToInt(itemGOs.Max((UIContextIconsMenuItem p) => p.Label.printedSize.x)) + 56;
			foreach (UIContextIconsMenuItem itemGO2 in itemGOs)
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
