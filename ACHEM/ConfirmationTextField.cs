using System;
using UnityEngine;

public class ConfirmationTextField : ModalWindow
{
	protected static ConfirmationTextField m2Instance;

	protected Action<bool> callback;

	public UILabel Title;

	public UILabel Message;

	public UILabel Prompt;

	public UIButton BtnYes;

	public UIButton BtnNo;

	public UIInput TextInput;

	private string confirmationText = "";

	private void Awake()
	{
		m2Instance = this;
	}

	private void OnDestroy()
	{
		m2Instance = null;
	}

	private void Start()
	{
		UIEventListener uIEventListener = UIEventListener.Get(BtnYes.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(onYesClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(BtnNo.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(onNoClick));
	}

	protected void ShowConfirmation(string title, string message, Action<bool> callback)
	{
		Title.text = title;
		Message.text = message;
		this.callback = callback;
	}

	protected void onYesClick(GameObject go)
	{
		if (TextInput.value == confirmationText)
		{
			TextInput.value = null;
			callback(obj: true);
			Close();
		}
	}

	public void SubmitText()
	{
		Debug.Log("TEXT VALUE =  " + UIInput.current.value);
		if (UIInput.current.value == confirmationText)
		{
			UIInput.current.value = null;
			callback(obj: true);
			Close();
		}
	}

	protected void onNoClick(GameObject go)
	{
		callback(obj: false);
		Close();
	}

	protected override void Close()
	{
		callback = null;
		base.Close();
	}

	public static void Show(string title, string message, Action<bool> callback, string textFieldConfirmation)
	{
		if (m2Instance == null)
		{
			GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ConfirmationTextField"), UIManager.Instance.transform);
			obj.name = "ConfirmationTextField";
			m2Instance = obj.GetComponent<ConfirmationTextField>();
			m2Instance.Init();
		}
		m2Instance.confirmationText = textFieldConfirmation;
		m2Instance.ShowConfirmation(title, message, callback);
	}

	public static void SetLabels(string yes, string no)
	{
		if (m2Instance != null)
		{
			UILabel componentInChildren = m2Instance.BtnYes.GetComponentInChildren<UILabel>();
			UILabel componentInChildren2 = m2Instance.BtnNo.GetComponentInChildren<UILabel>();
			if (componentInChildren != null)
			{
				Debug.Log("Setting yes to " + yes);
				componentInChildren.text = yes;
			}
			if (componentInChildren2 != null)
			{
				Debug.Log("Setting no to " + no);
				componentInChildren2.text = no;
			}
		}
	}

	public static void Show(string title, string message, Action<bool> callback, string textFieldConfirmation, string prompt, bool password, int charLimit)
	{
		if (m2Instance == null)
		{
			GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ConfirmationTextField"), UIManager.Instance.transform);
			obj.name = "ConfirmationTextField";
			m2Instance = obj.GetComponent<ConfirmationTextField>();
			m2Instance.Init();
		}
		m2Instance.confirmationText = textFieldConfirmation;
		if (password)
		{
			m2Instance.TextInput.inputType = UIInput.InputType.Password;
		}
		m2Instance.TextInput.characterLimit = charLimit;
		m2Instance.Prompt.text = prompt;
		m2Instance.ShowConfirmation(title, message, callback);
	}
}
