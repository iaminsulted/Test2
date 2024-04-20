using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ClientTriggerAction : MonoBehaviour
{
	public float MinDelay;

	public float MaxDelay;

	public void Execute()
	{
		float num = ((MaxDelay <= MinDelay) ? MinDelay : ArtixRandom.Range(MinDelay, MaxDelay));
		if (num <= 0f)
		{
			OnExecute();
		}
		else
		{
			StartCoroutine(Wait(num));
		}
	}

	protected virtual void OnExecute()
	{
		Debug.Log("ClientTriggerAction Executed.");
	}

	private IEnumerator Wait(float delay)
	{
		yield return new WaitForSeconds(delay);
		OnExecute();
	}

	public static void CallFromString(string action)
	{
		if (string.IsNullOrEmpty(action))
		{
			return;
		}
		try
		{
			action = Regex.Replace(action, "\\s+", "");
			string[] array = action.Split('/');
			switch (array[0])
			{
			case "CTAIAPStore":
				UIIAPStore.Show();
				break;
			case "CTACompleteQuest":
			{
				int.TryParse(array[1], out var result15);
				Game.Instance.SendQuestCompleteRequest(result15);
				break;
			}
			case "CTACraftShop":
			{
				int.TryParse(array[1], out var result9);
				UIMerge.Load(result9);
				break;
			}
			case "CTAVideo":
				break;
			case "CTAQuestLoad":
			{
				List<int> list = new List<int>();
				List<int> list2 = new List<int>();
				string[] array2 = array[1].Split(',');
				for (int i = 0; i < array2.Length; i++)
				{
					if (int.TryParse(array2[i], out var result3))
					{
						list.Add(result3);
					}
				}
				if (array.Length > 2)
				{
					string[] array3 = array[2].Split(',');
					for (int j = 0; j < array3.Length; j++)
					{
						if (int.TryParse(array3[j], out var result4))
						{
							list2.Add(result4);
						}
					}
				}
				UIQuest.ShowQuests(list, list2);
				break;
			}
			case "CTACinematic":
			{
				int.TryParse(array[1], out var result16);
				DialogueSlotManager.Show(result16);
				break;
			}
			case "CTADungeonLoad":
			{
				List<int> list4 = new List<int>();
				string[] array5 = array[1].Split(',');
				for (int l = 0; l < array5.Length; l++)
				{
					int.TryParse(array5[l], out var result8);
					list4.Add(result8);
				}
				UIDungeons.Load(list4);
				break;
			}
			case "CTADialogue":
			{
				int.TryParse(array[1], out var result17);
				bool result18 = false;
				if (array.Length > 2)
				{
					bool.TryParse(array[2], out result18);
				}
				DialogueSlotManager.Show(result17, null, result18);
				break;
			}
			case "CTARandomDialogue":
			{
				string[] array6 = array[1].Split(',');
				List<int> list5 = new List<int>();
				for (int m = 0; m < array6.Length; m++)
				{
					int.TryParse(array6[m], out var result13);
					list5.Add(result13);
				}
				bool.TryParse(array[2], out var result14);
				DialogueSlotManager.Show(list5[UnityEngine.Random.Range(0, list5.Count)], null, result14);
				break;
			}
			case "CTATransfer":
				Game.Instance.SendAreaJoinCommand(array[1]);
				break;
			case "CTATransferMap":
			{
				int.TryParse(array[1], out var result10);
				int.TryParse(array[2], out var result11);
				int.TryParse(array[3], out var result12);
				Game.Instance.SendTransferMapRequest(result10, result11, result12, showConfirmation: false);
				break;
			}
			case "CTATransferRandom":
			{
				List<int> list3 = new List<int>();
				string[] array4 = array[1].Split(',');
				for (int k = 0; k < array4.Length; k++)
				{
					int.TryParse(array4[k], out var result7);
					list3.Add(result7);
				}
				int id = list3[UnityEngine.Random.Range(0, list3.Count)];
				Game.Instance.SendAreaJoinRequest(id);
				break;
			}
			case "CTAShop":
			{
				int.TryParse(array[1], out var result6);
				UIShop.LoadShop(result6);
				break;
			}
			case "CTALoadClassUI":
			{
				int.TryParse(array[1], out var result5);
				UICharClasses.LoadByID(result5);
				break;
			}
			case "CTATutorial":
			{
				int.TryParse(array[1], out var result);
				float.TryParse(array[2], out var _);
				Tutorial tutorial = (Tutorial)result;
				if (!UITutorialPopup.Show(tutorial))
				{
					TutorialSequenceManager.Show(tutorial);
				}
				break;
			}
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}
}
