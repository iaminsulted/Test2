using UnityEngine;

public class DialogueOverlay : MonoBehaviour
{
	public static DialogueOverlay mInstance;

	public UILabel DialogueName;

	public UILabel DialogueTitle;

	public UILabel DialogueText;

	public UIPanel DialogueFadePanel;

	public GameObject Dongle;

	public GameObject[] OtherStuff;

	public GameObject Skipper;

	public GameObject SkipSeer;

	public int DialogType;

	private DialogueSlotManager dialogueSlotManager;

	public float spamClick;

	private void Awake()
	{
		mInstance = this;
	}

	private void OnDestroy()
	{
		mInstance = null;
	}

	private void SetDialogueType(int dtype)
	{
		bool flag = dtype < 2;
		DialogueName.gameObject.SetActive(flag);
		DialogueText.gameObject.SetActive(flag);
		DialogueTitle.gameObject.SetActive(flag);
		Dongle.SetActive(flag);
		if (flag)
		{
			SkipSeer.SetActive(value: false);
		}
		else if (!Skipper.activeSelf)
		{
			SkipSeer.SetActive(value: true);
		}
		GameObject[] otherStuff = OtherStuff;
		for (int i = 0; i < otherStuff.Length; i++)
		{
			otherStuff[i].gameObject.SetActive(flag);
		}
	}

	public static void SetText(string name, string txt, int _dialogType = 0, int alignment = 0, string title = "")
	{
		mInstance.SetDialogueType(_dialogType);
		mInstance.DialogueName.text = name;
		mInstance.DialogueText.text = txt;
		mInstance.DialogueTitle.text = title;
		float x = 0f;
		switch (alignment)
		{
		case 0:
			x = -375f;
			goto default;
		case 1:
			x = -300f;
			goto default;
		case 2:
			x = -225f;
			goto default;
		case 3:
			x = -150f;
			goto default;
		case 4:
			x = -75f;
			goto default;
		case 5:
			x = 0f;
			goto default;
		case 6:
			x = 75f;
			goto default;
		case 7:
			x = 150f;
			goto default;
		case 8:
			x = 225f;
			goto default;
		case 9:
			x = 300f;
			goto default;
		case 10:
			x = 375f;
			goto default;
		default:
			mInstance.Dongle.SetActive(value: true);
			mInstance.Dongle.transform.localPosition = new Vector3(x, mInstance.Dongle.transform.localPosition.y, mInstance.Dongle.transform.localPosition.z);
			break;
		case 11:
			mInstance.Dongle.SetActive(value: false);
			break;
		}
	}

	public void Skip()
	{
		if (DialogType < 2)
		{
			dialogueSlotManager.SkipAction();
		}
	}

	public void Continue()
	{
		if (DialogType < 2)
		{
			if (spamClick > Time.time - 0.5f)
			{
				skipOn();
			}
			dialogueSlotManager.ContinueAction();
			spamClick = Time.time;
		}
	}

	public void close()
	{
		Destroy();
	}

	public static void Close()
	{
		if (mInstance != null)
		{
			mInstance.close();
		}
	}

	public static void Show(int dtype = 0)
	{
		mInstance.SetDialogueType(dtype);
	}

	public static void Init(DialogueSlotManager dialogueSlotManager)
	{
		if (mInstance == null)
		{
			GameObject obj = Object.Instantiate(Resources.Load<GameObject>("DialogueOverlay"), UIManager.Instance.transform);
			obj.name = "DialogueOverlay";
			mInstance = obj.GetComponent<DialogueOverlay>();
		}
		mInstance.dialogueSlotManager = dialogueSlotManager;
	}

	public void skipOn()
	{
		Skipper.SetActive(value: true);
		SkipSeer.SetActive(value: false);
	}

	public virtual void Destroy()
	{
		Object.Destroy(base.gameObject);
	}
}
