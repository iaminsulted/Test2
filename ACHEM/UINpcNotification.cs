using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UINpcNotification : MonoBehaviour
{
	private struct NPCNotification
	{
		public string npcName;

		public string npcDialogue;

		public NPCNotification(string npcName, string npcDialogue)
		{
			this.npcName = npcName;
			this.npcDialogue = npcDialogue;
		}
	}

	private static UINpcNotification Instance;

	[SerializeField]
	private UILabel npcName;

	[SerializeField]
	private UILabel npcDialogue;

	[SerializeField]
	private UIPanel panel;

	private Coroutine coroutine;

	private List<NPCNotification> labels = new List<NPCNotification>();

	private void Awake()
	{
		Instance = this;
	}

	private void QueueLabel(string npcName, string npcDialogue)
	{
		labels.Add(new NPCNotification(npcName, npcDialogue));
		if (coroutine == null)
		{
			StartQueue();
		}
	}

	private void StartQueue()
	{
		coroutine = StartCoroutine(ShowNotification());
	}

	private IEnumerator ShowNotification()
	{
		NPCNotification nPCNotification = labels.First();
		npcName.text = nPCNotification.npcName;
		npcDialogue.text = nPCNotification.npcDialogue;
		labels.RemoveAt(0);
		panel.alpha = 0f;
		while (true)
		{
			panel.alpha += Time.deltaTime * 1.75f;
			if (panel.alpha >= 1f)
			{
				break;
			}
			yield return null;
		}
		panel.alpha = 1f;
		yield return new WaitForSeconds(3f);
		coroutine = StartCoroutine(HideNotification());
	}

	private IEnumerator HideNotification()
	{
		panel.alpha = 1f;
		while (true)
		{
			panel.alpha -= Time.deltaTime * 3f;
			if (panel.alpha <= 0f)
			{
				break;
			}
			yield return null;
		}
		panel.alpha = 0f;
		if (labels.Count > 0)
		{
			StartQueue();
		}
		else
		{
			coroutine = null;
		}
	}

	public static void Show(string npcName, string npcDialogue)
	{
		if (Instance == null)
		{
			CreateInstance();
		}
		Instance.QueueLabel(npcName, npcDialogue);
	}

	private static UINpcNotification CreateInstance()
	{
		GameObject obj = Object.Instantiate(Resources.Load<GameObject>("UIElements/NpcNotification"), UIManager.Instance.transform);
		obj.name = "NPC Notification";
		return obj.GetComponent<UINpcNotification>();
	}
}
