using UnityEngine;

public class UITutorialHighlighter : MonoBehaviour
{
	private const int PanelDepth = 100000;

	private const float GlowPaddingScaleFactor = 3f;

	private static UITutorialHighlighter instance;

	public Transform PointerTransform;

	public GameObject LeftPointer;

	public GameObject RightPointer;

	public GameObject UpPointer;

	public GameObject DownPointer;

	public GameObject Glow;

	public Animator ShineAnimator;

	private UIPanel panel;

	private GameObject target;

	private HighlightPointerLocation pointerLocation;

	private Vector2 pointerOffset;

	public static void ShowFadeOnly()
	{
		ShowFadeAndHighlightTarget(null);
	}

	public static void ShowFadeAndHighlightTarget(GameObject target)
	{
		ShowFadeAndHighlightTarget(target, HighlightPointerLocation.None, default(Vector2));
	}

	public static void ShowFadeAndHighlightTarget(GameObject target, HighlightPointerLocation pointerLocation, Vector2 pointerOffset)
	{
		if (instance != null)
		{
			Close();
		}
		Game.Instance.DisableControls();
		instance = Object.Instantiate(Resources.Load<GameObject>("TutorialHighlighter"), UIGame.Instance.Container).GetComponent<UITutorialHighlighter>();
		if (target != null)
		{
			instance.Initialize(target, pointerLocation, pointerOffset);
			return;
		}
		instance.LeftPointer.SetActive(value: false);
		instance.RightPointer.SetActive(value: false);
		instance.UpPointer.SetActive(value: false);
		instance.DownPointer.SetActive(value: false);
		instance.Glow.SetActive(value: false);
	}

	private void Initialize(GameObject target, HighlightPointerLocation pointerLocation, Vector2 pointerOffset)
	{
		this.target = target;
		this.pointerLocation = pointerLocation;
		this.pointerOffset = pointerOffset;
		panel = target.AddComponent<UIPanel>();
		panel.depth = 100000;
		target.SetActive(value: false);
		target.SetActive(value: true);
		LeftPointer.SetActive(value: false);
		RightPointer.SetActive(value: false);
		UpPointer.SetActive(value: false);
		DownPointer.SetActive(value: false);
		Glow.SetActive(value: true);
		switch (pointerLocation)
		{
		case HighlightPointerLocation.Down:
			DownPointer.SetActive(value: true);
			ShineAnimator.SetTrigger("Play");
			break;
		case HighlightPointerLocation.Up:
			UpPointer.SetActive(value: true);
			ShineAnimator.SetTrigger("Play");
			break;
		case HighlightPointerLocation.Right:
			RightPointer.SetActive(value: true);
			ShineAnimator.SetTrigger("Play");
			break;
		case HighlightPointerLocation.Left:
			LeftPointer.SetActive(value: true);
			ShineAnimator.SetTrigger("Play");
			break;
		}
		UpdatePointerPosition();
	}

	private void Update()
	{
		UpdatePointerPosition();
	}

	private void UpdatePointerPosition()
	{
		if (target == null)
		{
			return;
		}
		Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(target.transform);
		if (!(Vector3.Distance(bounds.center, Vector3.positiveInfinity) < 1f))
		{
			switch (pointerLocation)
			{
			case HighlightPointerLocation.Down:
				PointerTransform.position = bounds.min + Vector3.right * (bounds.size.x / 2f);
				break;
			case HighlightPointerLocation.Up:
				PointerTransform.position = bounds.max + Vector3.left * (bounds.size.x / 2f);
				break;
			case HighlightPointerLocation.Right:
				PointerTransform.position = bounds.max + Vector3.down * (bounds.size.y / 2f);
				break;
			case HighlightPointerLocation.Left:
				PointerTransform.position = bounds.min + Vector3.up * (bounds.size.y / 2f);
				break;
			}
			PointerTransform.localPosition += (Vector3)pointerOffset;
			Bounds bounds2 = NGUIMath.CalculateRelativeWidgetBounds(target.transform);
			ShineAnimator.transform.position = bounds.center;
			Glow.transform.position = bounds.center;
			Glow.GetComponent<UIWidget>().width = (int)(bounds2.size.x * 3f);
			Glow.GetComponent<UIWidget>().height = (int)(bounds2.size.y * 3f);
		}
	}

	public static void Close()
	{
		if (Game.Instance != null)
		{
			Game.Instance.EnableControls();
		}
		if (instance != null)
		{
			Object.Destroy(instance.panel);
			Object.Destroy(instance.gameObject);
		}
	}

	public static void HidePointer()
	{
		if (instance != null)
		{
			instance.LeftPointer.SetActive(value: false);
			instance.RightPointer.SetActive(value: false);
			instance.UpPointer.SetActive(value: false);
			instance.DownPointer.SetActive(value: false);
			instance.Glow.SetActive(value: false);
		}
	}
}
