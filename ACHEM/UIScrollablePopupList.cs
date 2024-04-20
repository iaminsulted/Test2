using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Scrollable Popup List")]
public class UIScrollablePopupList : UIWidgetContainer
{
	public enum Position
	{
		Auto,
		Above,
		Below
	}

	public delegate void LegacyEvent(string val);

	public static UIScrollablePopupList current;

	private const float animSpeed = 0.15f;

	public INGUIAtlas atlas;

	public INGUIAtlas scrollbarAtlas;

	public string scrollbarSpriteName;

	public string scrollbarForegroundName;

	public UIFont bitmapFont;

	public Font trueTypeFont;

	public int fontSize = 16;

	public FontStyle fontStyle;

	public string backgroundSprite;

	public string highlightSprite;

	public Position position;

	public List<string> items = new List<string>();

	public Vector2 padding = new Vector3(4f, 4f);

	public Color textColor = Color.white;

	public Color backgroundColor = Color.white;

	public Color highlightColor = new Color(0.88235295f, 40f / 51f, 0.5882353f, 1f);

	public Color scrollbarBgDefColour;

	public Color scrollbarBgHovColour;

	public Color scrollbarBgPrsColour;

	public Color scrollbarFgDefColour;

	public Color scrollbarFgHovColour;

	public Color scrollbarFgPrsColour;

	public bool isAnimated = true;

	public bool isLocalized;

	public int maxHeight = 100;

	public List<EventDelegate> onChange = new List<EventDelegate>();

	[HideInInspector]
	[SerializeField]
	private string mSelectedItem;

	private UIPanel mPanel;

	private GameObject mChild;

	private UISprite mBackground;

	private UISprite mHighlight;

	private UILabel mHighlightedLabel;

	private List<UILabel> mLabelList = new List<UILabel>();

	private float mBgBorder;

	private UIPanel mClippingPanel;

	private UISprite scrollbarSprite;

	private UISprite scrForegroundSprite;

	[HideInInspector]
	[SerializeField]
	private GameObject eventReceiver;

	[HideInInspector]
	[SerializeField]
	private string functionName = "OnSelectionChange";

	[HideInInspector]
	[SerializeField]
	private float textScale;

	[HideInInspector]
	[SerializeField]
	private UIFont font;

	[HideInInspector]
	[SerializeField]
	private UILabel textLabel;

	private LegacyEvent mLegacyEvent;

	private bool mUseDynamicFont;

	public UnityEngine.Object ambigiousFont
	{
		get
		{
			if (trueTypeFont != null)
			{
				return trueTypeFont;
			}
			if (bitmapFont != null)
			{
				return bitmapFont;
			}
			return font;
		}
		set
		{
			if (value is Font)
			{
				trueTypeFont = value as Font;
				bitmapFont = null;
				font = null;
			}
			else if (value is UIFont)
			{
				bitmapFont = value as UIFont;
				trueTypeFont = null;
				font = null;
			}
		}
	}

	[Obsolete("Use EventDelegate.Add(popup.onChange, YourCallback) instead, and UIPopupList.current.value to determine the state")]
	public LegacyEvent onSelectionChange
	{
		get
		{
			return mLegacyEvent;
		}
		set
		{
			mLegacyEvent = value;
		}
	}

	public bool isOpen => mChild != null;

	public string value
	{
		get
		{
			return mSelectedItem;
		}
		set
		{
			mSelectedItem = value;
			if (mSelectedItem != null && mSelectedItem != null)
			{
				TriggerCallbacks();
			}
		}
	}

	[Obsolete("Use 'value' instead")]
	public string selection
	{
		get
		{
			return value;
		}
		set
		{
			this.value = value;
		}
	}

	private bool handleEvents
	{
		get
		{
			UIKeyNavigation component = GetComponent<UIKeyNavigation>();
			if (!(component == null))
			{
				return !component.enabled;
			}
			return true;
		}
		set
		{
			UIKeyNavigation component = GetComponent<UIKeyNavigation>();
			if (component != null)
			{
				component.enabled = !value;
			}
		}
	}

	private bool isValid
	{
		get
		{
			if (!(bitmapFont != null))
			{
				return trueTypeFont != null;
			}
			return true;
		}
	}

	private int activeFontSize
	{
		get
		{
			if (!(trueTypeFont != null) && !(bitmapFont == null))
			{
				return bitmapFont.defaultSize;
			}
			return fontSize;
		}
	}

	private float activeFontScale
	{
		get
		{
			if (!(trueTypeFont != null) && !(bitmapFont == null))
			{
				return (float)fontSize / (float)bitmapFont.defaultSize;
			}
			return 1f;
		}
	}

	protected void TriggerCallbacks()
	{
		if (current != this)
		{
			UIScrollablePopupList uIScrollablePopupList = current;
			current = this;
			if (mLegacyEvent != null)
			{
				mLegacyEvent(mSelectedItem);
			}
			if (EventDelegate.IsValid(onChange))
			{
				EventDelegate.Execute(onChange);
			}
			else if (eventReceiver != null && !string.IsNullOrEmpty(functionName))
			{
				eventReceiver.SendMessage(functionName, mSelectedItem, SendMessageOptions.DontRequireReceiver);
			}
			current = uIScrollablePopupList;
		}
	}

	private void OnEnable()
	{
		if (EventDelegate.IsValid(onChange))
		{
			eventReceiver = null;
			functionName = null;
		}
		if (font != null)
		{
			if (font.isDynamic)
			{
				trueTypeFont = font.dynamicFont;
				fontStyle = font.dynamicFontStyle;
				mUseDynamicFont = true;
			}
			else if (bitmapFont == null)
			{
				bitmapFont = font;
				mUseDynamicFont = false;
			}
			font = null;
		}
		if (textScale != 0f)
		{
			fontSize = ((bitmapFont != null) ? Mathf.RoundToInt((float)bitmapFont.defaultSize * textScale) : 16);
			textScale = 0f;
		}
		if (trueTypeFont == null && bitmapFont != null && bitmapFont.isDynamic)
		{
			trueTypeFont = bitmapFont.dynamicFont;
			bitmapFont = null;
		}
	}

	private void OnValidate()
	{
		Font font = trueTypeFont;
		UIFont uIFont = bitmapFont;
		bitmapFont = null;
		trueTypeFont = null;
		if (font != null && (uIFont == null || !mUseDynamicFont))
		{
			bitmapFont = null;
			trueTypeFont = font;
			mUseDynamicFont = true;
		}
		else if (uIFont != null)
		{
			if (uIFont.isDynamic)
			{
				trueTypeFont = uIFont.dynamicFont;
				fontStyle = uIFont.dynamicFontStyle;
				fontSize = uIFont.defaultSize;
				mUseDynamicFont = true;
			}
			else
			{
				bitmapFont = uIFont;
				mUseDynamicFont = false;
			}
		}
		else
		{
			trueTypeFont = font;
			mUseDynamicFont = true;
		}
	}

	private void Start()
	{
		if (textLabel != null)
		{
			EventDelegate.Add(onChange, textLabel.SetCurrentSelection);
			textLabel = null;
		}
		if (!Application.isPlaying)
		{
			return;
		}
		if (string.IsNullOrEmpty(mSelectedItem))
		{
			if (items.Count > 0)
			{
				value = items[0];
			}
		}
		else
		{
			string text = mSelectedItem;
			mSelectedItem = null;
			value = text;
		}
	}

	private void OnLocalize()
	{
		if (isLocalized)
		{
			TriggerCallbacks();
		}
	}

	private void Highlight(UILabel lbl, bool instant)
	{
		if (!(mHighlight != null))
		{
			return;
		}
		TweenPosition component = lbl.GetComponent<TweenPosition>();
		if (component != null && component.enabled)
		{
			return;
		}
		mHighlightedLabel = lbl;
		UISpriteData atlasSprite = mHighlight.GetAtlasSprite();
		if (atlasSprite != null)
		{
			float pixelSize = atlas.pixelSize;
			float num = (float)atlasSprite.borderLeft * pixelSize;
			float y = (float)atlasSprite.borderTop * pixelSize;
			Vector3 vector = lbl.cachedTransform.localPosition + new Vector3(0f - num, y, 1f);
			if (instant || !isAnimated)
			{
				mHighlight.cachedTransform.localPosition = vector;
			}
			else
			{
				TweenPosition.Begin(mHighlight.gameObject, 0.1f, vector).method = UITweener.Method.EaseOut;
			}
		}
	}

	private void OnItemHover(GameObject go, bool isOver)
	{
		if (isOver)
		{
			UILabel component = go.GetComponent<UILabel>();
			Highlight(component, instant: false);
		}
	}

	private void Select(UILabel lbl, bool instant)
	{
		Highlight(lbl, instant);
		UIEventListener component = lbl.gameObject.GetComponent<UIEventListener>();
		value = component.parameter as string;
		UIPlaySound[] components = GetComponents<UIPlaySound>();
		int i = 0;
		for (int num = components.Length; i < num; i++)
		{
			UIPlaySound uIPlaySound = components[i];
			if (uIPlaySound.trigger == UIPlaySound.Trigger.OnClick)
			{
				NGUITools.PlaySound(uIPlaySound.audioClip, uIPlaySound.volume, 1f);
			}
		}
		Close();
	}

	private void OnItemPress(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			Select(go.GetComponent<UILabel>(), instant: true);
		}
	}

	private void OnKey(KeyCode key)
	{
		if (!base.enabled || !NGUITools.GetActive(base.gameObject) || !handleEvents)
		{
			return;
		}
		int num = mLabelList.IndexOf(mHighlightedLabel);
		if (num == -1)
		{
			num = 0;
		}
		switch (key)
		{
		case KeyCode.UpArrow:
			if (num > 0)
			{
				Select(mLabelList[--num], instant: false);
			}
			break;
		case KeyCode.DownArrow:
			if (num + 1 < mLabelList.Count)
			{
				Select(mLabelList[++num], instant: false);
			}
			break;
		case KeyCode.Escape:
			OnSelect(isSelected: false);
			break;
		}
	}

	private void OnSelect(bool isSelected)
	{
		if (!isSelected)
		{
			Close();
		}
	}

	public void Close()
	{
		if (!(mChild != null) || ((bool)UICamera.hoveredObject && (UICamera.hoveredObject == scrollbarSprite.gameObject || UICamera.hoveredObject == scrForegroundSprite.gameObject)))
		{
			return;
		}
		mLabelList.Clear();
		handleEvents = false;
		if (isAnimated)
		{
			UIWidget[] componentsInChildren = mChild.GetComponentsInChildren<UIWidget>();
			int i = 0;
			for (int num = componentsInChildren.Length; i < num; i++)
			{
				UIWidget obj = componentsInChildren[i];
				Color color = obj.color;
				color.a = 0f;
				TweenColor.Begin(obj.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
			}
			Collider[] componentsInChildren2 = mChild.GetComponentsInChildren<Collider>();
			int j = 0;
			for (int num2 = componentsInChildren2.Length; j < num2; j++)
			{
				componentsInChildren2[j].enabled = false;
			}
			UnityEngine.Object.Destroy(mChild, 0.15f);
		}
		else
		{
			UnityEngine.Object.Destroy(mChild);
		}
		mBackground = null;
		mHighlight = null;
		mChild = null;
		mClippingPanel = null;
		scrollbarSprite = null;
		scrForegroundSprite = null;
	}

	private void AnimateColor(UIWidget widget)
	{
		Color color = widget.color;
		widget.color = new Color(color.r, color.g, color.b, 0f);
		TweenColor.Begin(widget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
	}

	private void AnimatePosition(UIWidget widget, bool placeAbove, float bottom)
	{
		Vector3 localPosition = widget.cachedTransform.localPosition;
		Vector3 localPosition2 = (placeAbove ? new Vector3(localPosition.x, bottom, localPosition.z) : new Vector3(localPosition.x, 0f, localPosition.z));
		widget.cachedTransform.localPosition = localPosition2;
		TweenPosition.Begin(widget.gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
	}

	private void AnimateScale(UIWidget widget, bool placeAbove, float bottom)
	{
		GameObject go = widget.gameObject;
		Transform cachedTransform = widget.cachedTransform;
		float num = (float)activeFontSize * activeFontScale + mBgBorder * 2f;
		cachedTransform.localScale = new Vector3(1f, num / (float)widget.height, 1f);
		TweenScale.Begin(go, 0.15f, Vector3.one).method = UITweener.Method.EaseOut;
		if (placeAbove)
		{
			Vector3 localPosition = cachedTransform.localPosition;
			cachedTransform.localPosition = new Vector3(localPosition.x, localPosition.y - (float)widget.height + num, localPosition.z);
			TweenPosition.Begin(go, 0.15f, localPosition).method = UITweener.Method.EaseOut;
		}
	}

	private void Animate(UIWidget widget, bool placeAbove, float bottom)
	{
		AnimateColor(widget);
		AnimatePosition(widget, placeAbove, bottom);
	}

	private void OnClick()
	{
		if (!base.enabled || !NGUITools.GetActive(base.gameObject) || !(mChild == null) || atlas == null || !isValid || items.Count <= 0)
		{
			return;
		}
		mLabelList.Clear();
		if (mPanel == null)
		{
			mPanel = UIPanel.Find(base.transform);
			if (mPanel == null)
			{
				return;
			}
		}
		handleEvents = true;
		Transform transform = base.transform;
		Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(transform.parent, transform);
		mChild = new GameObject("Drop-down List");
		mChild.layer = base.gameObject.layer;
		Transform transform2 = mChild.transform;
		transform2.parent = transform.parent;
		transform2.localPosition = bounds.min;
		transform2.localRotation = Quaternion.identity;
		transform2.localScale = Vector3.one;
		mBackground = mChild.AddSprite(atlas, backgroundSprite);
		mBackground.pivot = UIWidget.Pivot.TopLeft;
		mBackground.depth = NGUITools.CalculateNextDepth(mPanel.gameObject);
		mBackground.color = backgroundColor;
		mBackground.gameObject.name = "SpriteBackground";
		mBackground.gameObject.AddComponent<UIDragScrollView>();
		GameObject gameObject = new GameObject("Panel");
		gameObject.layer = base.gameObject.layer;
		Transform transform3 = gameObject.transform;
		transform3.parent = mChild.transform;
		transform3.localPosition = Vector3.zero;
		transform3.localRotation = Quaternion.identity;
		transform3.localScale = Vector3.one;
		mClippingPanel = gameObject.AddComponent<UIPanel>();
		mClippingPanel.clipping = UIDrawCall.Clipping.SoftClip;
		mClippingPanel.leftAnchor.target = mBackground.transform;
		mClippingPanel.rightAnchor.target = mBackground.transform;
		mClippingPanel.topAnchor.target = mBackground.transform;
		mClippingPanel.bottomAnchor.target = mBackground.transform;
		mClippingPanel.ResetAnchors();
		mClippingPanel.UpdateAnchors();
		mClippingPanel.depth = NGUITools.CalculateNextDepth(mPanel.gameObject);
		UIScrollView uIScrollView = gameObject.AddComponent<UIScrollView>();
		uIScrollView.contentPivot = UIWidget.Pivot.TopLeft;
		uIScrollView.movement = UIScrollView.Movement.Vertical;
		uIScrollView.scrollWheelFactor = 0.25f;
		uIScrollView.disableDragIfFits = true;
		uIScrollView.dragEffect = UIScrollView.DragEffect.None;
		uIScrollView.ResetPosition();
		uIScrollView.UpdatePosition();
		uIScrollView.RestrictWithinBounds(instant: true);
		Vector4 border = mBackground.border;
		mBgBorder = border.y;
		mBackground.cachedTransform.localPosition = new Vector3(0f, border.y, 0f);
		mHighlight = gameObject.AddSprite(atlas, highlightSprite);
		mHighlight.pivot = UIWidget.Pivot.TopLeft;
		mHighlight.color = highlightColor;
		mHighlight.gameObject.name = "Highlighter";
		UISpriteData atlasSprite = mHighlight.GetAtlasSprite();
		if (atlasSprite == null)
		{
			return;
		}
		float num = atlasSprite.borderTop;
		float num2 = activeFontSize;
		float num3 = activeFontScale;
		float num4 = num2 * num3;
		float a = 0f;
		float num5 = 0f - padding.y;
		int num6 = ((bitmapFont != null) ? bitmapFont.defaultSize : fontSize);
		List<UILabel> list = new List<UILabel>();
		int i = 0;
		for (int count = items.Count; i < count; i++)
		{
			string text = items[i];
			UILabel uILabel = gameObject.AddWidget<UILabel>();
			uILabel.pivot = UIWidget.Pivot.TopLeft;
			uILabel.bitmapFont = bitmapFont;
			uILabel.trueTypeFont = trueTypeFont;
			uILabel.fontSize = num6;
			uILabel.fontStyle = fontStyle;
			uILabel.text = (isLocalized ? Localization.Get(text) : text);
			uILabel.color = textColor;
			uILabel.cachedTransform.localPosition = new Vector3(border.x + padding.x, num5, -1f);
			uILabel.overflowMethod = UILabel.Overflow.ResizeFreely;
			uILabel.MakePixelPerfect();
			uILabel.gameObject.AddComponent<UIDragScrollView>();
			if (num3 != 1f)
			{
				uILabel.cachedTransform.localScale = Vector3.one * num3;
			}
			list.Add(uILabel);
			num5 -= num4;
			num5 -= padding.y;
			a = Mathf.Max(a, uILabel.printedSize.x);
			UIEventListener uIEventListener = UIEventListener.Get(uILabel.gameObject);
			uIEventListener.onHover = OnItemHover;
			uIEventListener.onPress = OnItemPress;
			uIEventListener.parameter = text;
			if (mSelectedItem == text || (i == 0 && string.IsNullOrEmpty(mSelectedItem)))
			{
				Highlight(uILabel, instant: true);
			}
			mLabelList.Add(uILabel);
		}
		a = Mathf.Max(a, bounds.size.x * num3 - (border.x + padding.x) * 2f);
		float num7 = a / num3;
		Vector3 center = new Vector3(num7 * 0.5f, (0f - num2) * 0.5f, 0f);
		Vector3 size = new Vector3(num7, (num4 + padding.y) / num3, 1f);
		int j = 0;
		for (int count2 = list.Count; j < count2; j++)
		{
			UILabel uILabel2 = list[j];
			NGUITools.AddWidgetCollider(uILabel2.gameObject);
			uILabel2.autoResizeBoxCollider = false;
			BoxCollider component = uILabel2.GetComponent<BoxCollider>();
			center.z = component.center.z;
			component.center = center;
			component.size = size;
		}
		a += (border.x + padding.x) * 2f;
		num5 -= border.y;
		mBackground.width = Mathf.RoundToInt(a);
		int num8 = Mathf.RoundToInt(0f - num5 + border.y);
		if (maxHeight == 0)
		{
			maxHeight = num8;
		}
		mBackground.height = ((num8 > maxHeight) ? maxHeight : num8);
		uIScrollView.ResetPosition();
		uIScrollView.UpdatePosition();
		uIScrollView.RestrictWithinBounds(instant: true, horizontal: true, vertical: true);
		transform3.localPosition = Vector3.zero;
		float num9 = 2f * atlas.pixelSize;
		float f = a - (border.x + padding.x) * 2f + (float)atlasSprite.borderLeft * num9;
		float f2 = num4 + num * num9;
		mHighlight.width = Mathf.RoundToInt(f) - 5;
		mHighlight.height = Mathf.RoundToInt(f2);
		bool flag = position == Position.Above;
		if (position == Position.Auto)
		{
			UICamera uICamera = UICamera.FindCameraForLayer(base.gameObject.layer);
			if (uICamera != null)
			{
				flag = uICamera.cachedCamera.WorldToViewportPoint(transform.position).y < 0.5f;
			}
		}
		if (isAnimated)
		{
			float bottom = num5 + num4;
			Animate(mHighlight, flag, bottom);
			int k = 0;
			for (int count3 = list.Count; k < count3; k++)
			{
				Animate(list[k], flag, bottom);
			}
			AnimateColor(mBackground);
			AnimateScale(mBackground, flag, bottom);
		}
		if (flag)
		{
			transform2.localPosition = new Vector3(bounds.min.x, bounds.min.y + (float)mBackground.height + (float)transform.GetComponent<UISprite>().height - border.y, bounds.min.z);
		}
		scrollbarSprite = mBackground.gameObject.AddSprite(scrollbarAtlas, scrollbarSpriteName);
		scrollbarSprite.depth = NGUITools.CalculateNextDepth(mPanel.gameObject);
		scrollbarSprite.color = scrollbarBgDefColour;
		scrollbarSprite.gameObject.name = "Scrollbar";
		UIButtonColor uIButtonColor = scrollbarSprite.gameObject.AddComponent<UIButtonColor>();
		uIButtonColor.defaultColor = scrollbarSprite.color;
		uIButtonColor.hover = scrollbarBgHovColour;
		uIButtonColor.pressed = scrollbarBgPrsColour;
		NGUITools.AddWidgetCollider(scrollbarSprite.gameObject);
		scrollbarSprite.leftAnchor.target = mBackground.transform;
		scrollbarSprite.leftAnchor.relative = 1f;
		scrollbarSprite.leftAnchor.absolute = -11;
		scrollbarSprite.rightAnchor.target = mBackground.transform;
		scrollbarSprite.rightAnchor.relative = 1f;
		scrollbarSprite.rightAnchor.absolute = -1;
		scrollbarSprite.topAnchor.target = mBackground.transform;
		scrollbarSprite.topAnchor.relative = 1f;
		scrollbarSprite.topAnchor.absolute = 0;
		scrollbarSprite.bottomAnchor.target = mBackground.transform;
		scrollbarSprite.bottomAnchor.relative = 0f;
		scrollbarSprite.bottomAnchor.absolute = 0;
		scrollbarSprite.ResetAnchors();
		scrollbarSprite.UpdateAnchors();
		scrForegroundSprite = scrollbarSprite.gameObject.AddSprite(scrollbarAtlas, scrollbarForegroundName);
		scrForegroundSprite.depth = NGUITools.CalculateNextDepth(mPanel.gameObject);
		scrForegroundSprite.color = scrollbarFgDefColour;
		scrForegroundSprite.gameObject.name = "Foreground";
		UIButtonColor uIButtonColor2 = scrForegroundSprite.gameObject.AddComponent<UIButtonColor>();
		uIButtonColor2.defaultColor = scrForegroundSprite.color;
		uIButtonColor2.hover = scrollbarFgHovColour;
		uIButtonColor2.pressed = scrollbarFgPrsColour;
		NGUITools.AddWidgetCollider(scrForegroundSprite.gameObject);
		scrForegroundSprite.leftAnchor.target = scrollbarSprite.transform;
		scrForegroundSprite.rightAnchor.target = scrollbarSprite.transform;
		scrForegroundSprite.topAnchor.target = scrollbarSprite.transform;
		scrForegroundSprite.bottomAnchor.target = scrollbarSprite.transform;
		scrForegroundSprite.ResetAnchors();
		scrForegroundSprite.UpdateAnchors();
		UIScrollBar uIScrollBar = scrollbarSprite.gameObject.AddComponent<UIScrollBar>();
		uIScrollBar.fillDirection = UIProgressBar.FillDirection.TopToBottom;
		uIScrollBar.backgroundWidget = scrollbarSprite;
		uIScrollBar.foregroundWidget = scrForegroundSprite;
		uIScrollView.verticalScrollBar = uIScrollBar;
		uIScrollView.ResetPosition();
		uIScrollBar.value = 0f;
	}
}
