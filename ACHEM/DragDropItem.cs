using UnityEngine;

public class DragDropItem : MonoBehaviour
{
	public delegate void onEvent(DragDropItem ddItem, DropSurface ds);

	public IUIItem Item;

	private Transform mTrans;

	private bool mIsDragging;

	private Transform mParent;

	public event onEvent OnDropEvent;

	private void Update()
	{
	}

	private void Drop()
	{
		ResetParentTransform();
		DropSurface component = mParent.GetComponent<DropSurface>();
		DropSurface dropSurface = null;
		GameObject current = UICamera.currentTouch.current;
		if (current != null)
		{
			dropSurface = current.GetComponent<DropSurface>();
		}
		Debug.Log("currentTouch.current: " + UICamera.currentTouch.current);
		Debug.Log("From: " + component);
		Debug.Log("To: " + dropSurface);
		if (this.OnDropEvent != null)
		{
			this.OnDropEvent(this, dropSurface);
		}
		GetComponent<Collider>().enabled = true;
	}

	private void Awake()
	{
		mTrans = base.transform;
	}

	private void OnDrag(Vector2 delta)
	{
		if (UICamera.currentTouchID > -2)
		{
			if (!mIsDragging)
			{
				GetComponent<Collider>().enabled = false;
				mIsDragging = true;
				mParent = mTrans.parent;
				mTrans.parent = DragDropRoot.root;
				Vector3 localPosition = mTrans.localPosition;
				localPosition.z = 0f;
				mTrans.localPosition = localPosition;
				mTrans.BroadcastMessage("CheckParent", SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				mTrans.localPosition += (Vector3)delta;
			}
		}
	}

	private void OnPress(bool isPressed)
	{
		Debug.Log("DragDropItem.OnPress(" + isPressed + ") -> " + UICamera.lastHit.collider.GetPath());
		if (!isPressed && mIsDragging)
		{
			mIsDragging = false;
			Drop();
		}
	}

	public void ResetParentTransform()
	{
		SetParentTransform(mParent);
	}

	public void SetParentTransform(Transform tform)
	{
		Transform transform2 = (mTrans.parent = tform);
		mParent = transform2;
		mTrans.localPosition = Vector3.zero;
		BroadcastMessage("MarkAsChanged", SendMessageOptions.DontRequireReceiver);
		UIGrid component = mParent.GetComponent<UIGrid>();
		if (component != null)
		{
			component.repositionNow = true;
		}
		Debug.Log("Parenting >>>> " + base.name);
		BroadcastMessage("CheckParent", SendMessageOptions.DontRequireReceiver);
	}
}
