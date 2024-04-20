using UnityEngine;

public class Tooltip : MonoBehaviour
{
	protected static Tooltip mInstance;

	public Camera uiCamera;

	public UILabel text;

	public GameObject tooltipRoot;

	public UISprite background;

	public float appearSpeed = 10f;

	public bool scalingTransitions = true;

	protected GameObject mTooltip;

	protected Transform mTrans;

	protected float mTarget;

	protected float mCurrent;

	protected Vector3 mPos;

	protected Vector3 mSize = Vector3.zero;

	protected UIWidget[] mWidgets;

	public static bool isVisible
	{
		get
		{
			if (mInstance != null)
			{
				return mInstance.mTarget == 1f;
			}
			return false;
		}
	}

	private void Awake()
	{
		mInstance = this;
	}

	private void OnDestroy()
	{
		mInstance = null;
	}

	protected virtual void Start()
	{
		mTrans = base.transform;
		mWidgets = GetComponentsInChildren<UIWidget>();
		mPos = mTrans.localPosition;
		if (uiCamera == null)
		{
			uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		SetAlpha(0f);
	}

	protected virtual void Update()
	{
		if (mTooltip != UICamera.tooltipObject)
		{
			mTooltip = null;
			mTarget = 0f;
		}
		if (mCurrent != mTarget)
		{
			mCurrent = Mathf.Lerp(mCurrent, mTarget, RealTime.deltaTime * appearSpeed);
			if (Mathf.Abs(mCurrent - mTarget) < 0.001f)
			{
				mCurrent = mTarget;
			}
			SetAlpha(mCurrent * mCurrent);
			if (scalingTransitions)
			{
				Vector3 vector = mSize * 0.25f;
				vector.y = 0f - vector.y;
				Vector3 localScale = Vector3.one * (1.5f - mCurrent * 0.5f);
				Vector3 localPosition = Vector3.Lerp(mPos - vector, mPos, mCurrent);
				mTrans.localPosition = localPosition;
				mTrans.localScale = localScale;
			}
		}
	}

	protected virtual void SetAlpha(float val)
	{
		int i = 0;
		for (int num = mWidgets.Length; i < num; i++)
		{
			UIWidget obj = mWidgets[i];
			Color color = obj.color;
			color.a = val;
			obj.color = color;
		}
	}

	protected virtual void SetText(string tooltipText, Vector3 tooltipPosition, UIWidget.Pivot alignment, Color? color = null)
	{
		if (text != null && !string.IsNullOrEmpty(tooltipText))
		{
			mTarget = 1f;
			mTooltip = UICamera.tooltipObject;
			text.text = tooltipText;
			if (color.HasValue)
			{
				text.color = color.Value;
			}
			mPos = tooltipPosition;
			Transform obj = text.transform;
			Vector3 localPosition = obj.localPosition;
			Vector3 localScale = obj.localScale;
			mSize = text.printedSize;
			mSize.x *= localScale.x;
			mSize.y *= localScale.y;
			if (background != null)
			{
				Vector4 border = background.border;
				mSize.x += border.x + border.z + (localPosition.x - border.x) * 2f;
				mSize.y += border.y + border.w + (0f - localPosition.y - border.y) * 2f;
				background.width = Mathf.RoundToInt(mSize.x);
				float f = mSize.y;
				if (mSize.y < 100f)
				{
					f = 100f;
				}
				background.height = Mathf.RoundToInt(f);
				mSize.x = background.width;
				mSize.y = background.height;
			}
			if (uiCamera != null)
			{
				mPos.x = Mathf.Clamp01(mPos.x / (float)Screen.width);
				mPos.y = Mathf.Clamp01(mPos.y / (float)Screen.height);
				float num = uiCamera.orthographicSize / mTrans.parent.lossyScale.y;
				float num2 = (float)Screen.height * 0.5f / num;
				Vector2 vector = new Vector2(num2 * mSize.x / (float)Screen.width, num2 * mSize.y / (float)Screen.height);
				switch (alignment)
				{
				case UIWidget.Pivot.TopRight:
				case UIWidget.Pivot.Right:
				case UIWidget.Pivot.BottomRight:
					mPos.x -= vector.x;
					break;
				default:
					mPos.x -= vector.x / 2f;
					break;
				case UIWidget.Pivot.TopLeft:
				case UIWidget.Pivot.Left:
				case UIWidget.Pivot.BottomLeft:
					break;
				}
				switch (alignment)
				{
				case UIWidget.Pivot.BottomLeft:
				case UIWidget.Pivot.Bottom:
				case UIWidget.Pivot.BottomRight:
					mPos.y += vector.y;
					break;
				default:
					mPos.y += vector.y / 2f;
					break;
				case UIWidget.Pivot.TopLeft:
				case UIWidget.Pivot.Top:
				case UIWidget.Pivot.TopRight:
					break;
				}
				mPos.x = Mathf.Min(mPos.x, 1f - vector.x);
				mPos.y = Mathf.Max(mPos.y, vector.y);
				mTrans.position = uiCamera.ViewportToWorldPoint(mPos);
				mPos = mTrans.localPosition;
				mPos.x = Mathf.Round(mPos.x);
				mPos.y = Mathf.Round(mPos.y);
			}
			else
			{
				if (mPos.x + mSize.x > (float)Screen.width)
				{
					mPos.x = (float)Screen.width - mSize.x;
				}
				if (mPos.y - mSize.y < 0f)
				{
					mPos.y = mSize.y;
				}
				mPos.x -= (float)Screen.width * 0.5f;
				mPos.y -= (float)Screen.height * 0.5f;
			}
			mTrans.localPosition = mPos;
			if (tooltipRoot != null)
			{
				tooltipRoot.BroadcastMessage("UpdateAnchors");
			}
			else
			{
				text.BroadcastMessage("UpdateAnchors");
			}
		}
		else
		{
			mTooltip = null;
			mTarget = 0f;
		}
	}

	public static void Hide()
	{
		if (mInstance != null)
		{
			mInstance.mTooltip = null;
			mInstance.mTarget = 0f;
		}
	}

	public static void ShowAtMousePosition(string tooltipText, UIWidget.Pivot alignment = UIWidget.Pivot.TopLeft, Color? color = null)
	{
		if (mInstance != null)
		{
			mInstance.SetText(tooltipText, Input.mousePosition, alignment, color);
		}
	}

	public static void ShowAtPosition(string tooltipText, Vector3 position, UIWidget.Pivot alignment = UIWidget.Pivot.BottomRight)
	{
		if (mInstance != null)
		{
			mInstance.SetText(tooltipText, position, alignment);
		}
	}
}
