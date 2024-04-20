using UnityEngine;

public class UIAcceptTerms : MonoBehaviour
{
	public delegate void onAcceptedTerms();

	public UIToggle conditionsToggle;

	public UIButton okButton;

	private static onAcceptedTerms handler;

	public static UIAcceptTerms Instance { get; protected set; }

	public void onAccepted()
	{
		handler();
		Object.Destroy(base.gameObject);
	}

	public void onTermsToggle()
	{
		okButton.enabled = conditionsToggle.value;
	}

	protected virtual void Awake()
	{
		Instance = this;
		conditionsToggle.value = false;
		okButton.enabled = false;
	}

	protected virtual void OnDestroy()
	{
		Instance = null;
	}

	public static void Show(onAcceptedTerms callback)
	{
		if (Instance == null)
		{
			handler = callback;
			GameObject obj = Object.Instantiate(Resources.Load<GameObject>("TermsAndConditions"), UIManager.Instance.transform);
			obj.name = "MessageBox";
			Instance = obj.GetComponent<UIAcceptTerms>();
		}
	}

	public void openTerms()
	{
		WebView.OpenURL(Main.TermsURL);
	}
}
