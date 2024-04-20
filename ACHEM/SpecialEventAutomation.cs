using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class SpecialEventAutomation : MonoBehaviour
{
	private string routineDirectory = "D:/aq3dscripts/";

	public string RemoteURL = "https://www.adambohn.com/routines/";

	public float ExecutionDelay;

	public string[] routineLines;

	public List<string> recordingValues;

	public bool recording;

	public Dictionary<string, float> floatValueDictionary = new Dictionary<string, float>();

	public void Start()
	{
		Chat.Instance.chatInput.onChatSubmit += OnChatSubmit;
	}

	public void OnDestroy()
	{
		Chat.Instance.chatInput.onChatSubmit -= OnChatSubmit;
	}

	private bool HandleCommand(string command, string[] args)
	{
		switch (command)
		{
		case "/screenshot":
		{
			string text = "";
			text = ((args.Length >= 1) ? args[0] : "monster");
			if (!Directory.Exists("C:/Screenshots"))
			{
				Directory.CreateDirectory("C:/Screenshots");
			}
			RenderTexture renderTexture = new RenderTexture(1920, 1080, 24);
			Camera.main.targetTexture = renderTexture;
			Texture2D texture2D = new Texture2D(1920, 1080, TextureFormat.RGB24, mipChain: false);
			Camera.main.Render();
			RenderTexture.active = renderTexture;
			texture2D.ReadPixels(new Rect(0f, 0f, 1920f, 1080f), 0, 0);
			Camera.main.targetTexture = null;
			RenderTexture.active = null;
			UnityEngine.Object.Destroy(renderTexture);
			byte[] bytes = texture2D.EncodeToPNG();
			string text2 = $"C:/Screenshots/{text}.png";
			File.WriteAllBytes(text2, bytes);
			Debug.Log($"Took screenshot: {text2}");
			break;
		}
		case "/startrec":
			recording = true;
			recordingValues = new List<string>();
			Chat.AddMessage(InterfaceColors.Chat.Green.ToBBCode() + "Recording has started.");
			break;
		case "/stoprec":
			recording = false;
			Chat.AddMessage(InterfaceColors.Chat.Green.ToBBCode() + "Recording has stopped.");
			break;
		case "/saverec":
		{
			if (args.Length < 1)
			{
				Chat.AddMessage(InterfaceColors.Chat.Red.ToBBCode() + "/saverec filename.txt");
				break;
			}
			using (StreamWriter streamWriter = new StreamWriter(Path.Combine(routineDirectory, args[0])))
			{
				foreach (string recordingValue in recordingValues)
				{
					streamWriter.WriteLine(recordingValue);
				}
			}
			break;
		}
		case "/clearvars":
			floatValueDictionary.Clear();
			Chat.AddMessage(InterfaceColors.Chat.Yellow.ToBBCode() + "Cleared variable dictionary.");
			break;
		case "/add":
		{
			if (args.Length < 2)
			{
				Chat.AddMessage(InterfaceColors.Chat.Red.ToBBCode() + "/add <varname> <number>");
				break;
			}
			float result = 0f;
			if (float.TryParse(args[1], out result))
			{
				if (floatValueDictionary.ContainsKey(args[0]))
				{
					floatValueDictionary[args[0]] += result;
					Chat.AddMessage(InterfaceColors.Chat.Yellow.ToBBCode() + $"Updated {args[0]} to {floatValueDictionary[args[0]]} within the dictionary");
				}
				else
				{
					floatValueDictionary.Add(args[0], result);
					Chat.AddMessage(InterfaceColors.Chat.Yellow.ToBBCode() + $"{args[0]} doesn't exist within the dictionary. It has been added.");
				}
			}
			else
			{
				Chat.AddMessage(InterfaceColors.Chat.Red.ToBBCode() + "/add <varname> <number>");
			}
			break;
		}
		case "/set":
		{
			if (args.Length < 2)
			{
				Chat.AddMessage(InterfaceColors.Chat.Red.ToBBCode() + "/set <varname> <number>");
				break;
			}
			float result5 = 0f;
			if (float.TryParse(args[1], out result5))
			{
				if (floatValueDictionary.ContainsKey(args[0]))
				{
					floatValueDictionary[args[0]] = result5;
				}
				else
				{
					floatValueDictionary.Add(args[0], result5);
				}
				Chat.AddMessage(InterfaceColors.Chat.Yellow.ToBBCode() + $"Added {args[0]} - {result5} to the dictionary");
			}
			else
			{
				Chat.AddMessage(InterfaceColors.Chat.Red.ToBBCode() + "/set <varname> <number>");
			}
			break;
		}
		case "/getloc":
		{
			OmniMovementController omniMovementController = UnityEngine.Object.FindObjectOfType<OmniMovementController>();
			Chat.AddMessage($"Your current position {omniMovementController.transform.position.x} {omniMovementController.transform.position.y} {omniMovementController.transform.position.z}");
			break;
		}
		case "/setpos":
		{
			OmniMovementController omniMovementController = UnityEngine.Object.FindObjectOfType<OmniMovementController>();
			float result2 = 0f;
			float result3 = 0f;
			float result4 = 0f;
			if (!float.TryParse(args[0], out result2))
			{
				Chat.AddMessage(InterfaceColors.Chat.Red.ToBBCode() + $"Invalid number inserted for X: {args[0]}");
				break;
			}
			if (!float.TryParse(args[1], out result3))
			{
				Chat.AddMessage(InterfaceColors.Chat.Red.ToBBCode() + $"Invalid number inserted for Y: {args[1]}");
				break;
			}
			if (!float.TryParse(args[2], out result4))
			{
				Chat.AddMessage(InterfaceColors.Chat.Red.ToBBCode() + $"Invalid number inserted for Z: {args[2]}");
				break;
			}
			Vector3 translation = new Vector3(result2, result3, result4);
			omniMovementController.transform.localPosition = translation;
			break;
		}
		case "/moveright":
		{
			OmniMovementController omniMovementController = UnityEngine.Object.FindObjectOfType<OmniMovementController>();
			float z = ((args.Length != 0) ? float.Parse(args[0]) : 1f);
			Vector3 translation = new Vector3(z, 0f, 0f);
			omniMovementController.transform.Translate(translation);
			break;
		}
		case "/moveleft":
		{
			OmniMovementController omniMovementController = UnityEngine.Object.FindObjectOfType<OmniMovementController>();
			float z = ((args.Length != 0) ? float.Parse(args[0]) : 1f);
			Vector3 translation = new Vector3(0f - z, 0f, 0f);
			omniMovementController.transform.Translate(translation);
			break;
		}
		case "/moveforward":
		{
			OmniMovementController omniMovementController = UnityEngine.Object.FindObjectOfType<OmniMovementController>();
			float z = ((args.Length != 0) ? float.Parse(args[0]) : 1f);
			Vector3 translation = new Vector3(0f, 0f, z);
			omniMovementController.transform.Translate(translation);
			break;
		}
		case "/movebackward":
		{
			OmniMovementController omniMovementController = UnityEngine.Object.FindObjectOfType<OmniMovementController>();
			float z = ((args.Length != 0) ? float.Parse(args[0]) : 1f);
			Vector3 translation = new Vector3(0f, 0f, 0f - z);
			omniMovementController.transform.Translate(translation);
			break;
		}
		case "/rotate":
		{
			OmniMovementController omniMovementController = UnityEngine.Object.FindObjectOfType<OmniMovementController>();
			float z = ((args.Length != 0) ? float.Parse(args[0]) : 1f);
			Vector3 translation = new Vector3(0f, z, 0f);
			omniMovementController.transform.Rotate(translation, Space.Self);
			break;
		}
		case "/rotateabsolute":
		{
			OmniMovementController omniMovementController = UnityEngine.Object.FindObjectOfType<OmniMovementController>();
			float z = ((args.Length != 0) ? float.Parse(args[0]) : 1f);
			Vector3 translation = new Vector3(0f, z, 0f);
			omniMovementController.transform.Rotate(translation, Space.World);
			break;
		}
		case "/run":
			if (args.Length == 0)
			{
				Chat.AddMessage("Please enter a file name.");
			}
			else
			{
				StartCoroutine(LoadRoutineRemote(args[0]));
			}
			break;
		case "/runlocal":
			if (args.Length == 0)
			{
				Chat.AddMessage("Please enter a file name.");
			}
			else
			{
				LoadRoutineLocal(args[0]);
			}
			break;
		default:
			return false;
		case "/label":
			break;
		}
		return true;
	}

	public void OnChatSubmit(string msg)
	{
		if (msg.Length <= 0 || (Entities.Instance.me.AccessLevel < 50 && !(Entities.Instance.me.name.ToLower() == "nazzer")))
		{
			return;
		}
		string[] array = msg.Split(' ');
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = array[i].Trim();
			string pattern = "(?<=\\{)(.*?)(?=\\})";
			string value = Regex.Match(array[i], pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Value;
			if (floatValueDictionary.ContainsKey(value))
			{
				array[i] = Regex.Replace(array[i], "{(.*?)}", floatValueDictionary[value].ToString() ?? "");
			}
		}
		msg = string.Join(" ", array);
		string text = array[0].ToLower();
		string[] array2 = msg.Split(new string[1] { array[0] }, StringSplitOptions.RemoveEmptyEntries);
		if (recording && text != "/stoprec")
		{
			if (text == "/logpos")
			{
				OmniMovementController omniMovementController = UnityEngine.Object.FindObjectOfType<OmniMovementController>();
				recordingValues.Add($"/setpos {omniMovementController.transform.position.x} {omniMovementController.transform.position.y} {omniMovementController.transform.position.z}");
			}
			else
			{
				recordingValues.Add(msg);
			}
		}
		array2 = ((array2.Length != 0) ? array2[0].Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries) : new string[0]);
		HandleCommand(text, array2);
	}

	private IEnumerator LoadRoutineRemote(string file)
	{
		using UnityWebRequest www = UnityWebRequest.Get(RemoteURL + file);
		string errorTitle = "Loading Error";
		string friendlyMsg = "Failed to load event routine.";
		string customContext = "URL: " + www.url;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		if (www.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, null, customContext);
			yield break;
		}
		if (www.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, null, customContext);
			yield break;
		}
		if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, null, customContext);
			yield break;
		}
		routineLines = www.downloadHandler.text.Split(new string[1] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
		StartCoroutine(HandleRoutine());
	}

	private void LoadRoutineLocal(string file)
	{
		if (!Directory.Exists(routineDirectory))
		{
			Directory.CreateDirectory(routineDirectory);
		}
		if (File.Exists(routineDirectory + file))
		{
			routineLines = File.ReadAllLines(routineDirectory + file);
			StartCoroutine(HandleRoutine());
		}
		else
		{
			Chat.AddMessage("That routine doesn't exist.");
		}
	}

	private IEnumerator HandleRoutine()
	{
		Dictionary<string, int> pointers = new Dictionary<string, int>();
		for (int i = 0; i < routineLines.Length; i++)
		{
			string obj = routineLines[i];
			string[] array = obj.Split(' ');
			string text = array[0].ToLower();
			string[] array2 = obj.Split(new string[1] { array[0] }, StringSplitOptions.RemoveEmptyEntries);
			array2 = ((array2.Length != 0) ? array2[0].Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries) : new string[0]);
			if (text == "/label")
			{
				if (array2.Length == 0)
				{
					Chat.AddMessage($"Unnamed label found on line {i + 1}");
				}
				else
				{
					pointers.Add(array2[0], i);
				}
			}
		}
		for (int a = 0; a < routineLines.Length; a++)
		{
			string text2 = routineLines[a];
			string[] array3 = text2.Split(' ');
			for (int j = 0; j < array3.Length; j++)
			{
				array3[j] = array3[j].Trim();
				string pattern = "(?<=\\{)(.*?)(?=\\})";
				string value = Regex.Match(array3[j], pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Value;
				if (floatValueDictionary.ContainsKey(value))
				{
					array3[j] = Regex.Replace(array3[j], "{(.*?)}", floatValueDictionary[value].ToString() ?? "");
				}
			}
			text2 = string.Join(" ", array3);
			string text3 = array3[0].ToLower();
			string[] array4 = text2.Split(new string[1] { array3[0] }, StringSplitOptions.RemoveEmptyEntries);
			array4 = ((array4.Length != 0) ? array4[0].Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries) : new string[0]);
			switch (text3)
			{
			case "/if":
			{
				if (array4.Length < 4)
				{
					Chat.AddMessage(InterfaceColors.Chat.Red.ToBBCode() + "/if varname operator number label");
					break;
				}
				float result2 = 0f;
				if (float.TryParse(array4[2], out result2))
				{
					if (floatValueDictionary.ContainsKey(array4[0]))
					{
						switch (array4[1])
						{
						case ">":
							if (floatValueDictionary[array4[0]] > result2)
							{
								if (pointers.ContainsKey(array4[3]))
								{
									a = pointers[array4[3]];
								}
								else
								{
									Chat.AddMessage($"Unable to find label {array4[3]} on line {a + 1}");
								}
							}
							break;
						case "<":
							if (floatValueDictionary[array4[0]] < result2)
							{
								if (pointers.ContainsKey(array4[3]))
								{
									a = pointers[array4[3]];
								}
								else
								{
									Chat.AddMessage($"Unable to find label {array4[3]} on line {a + 1}");
								}
							}
							break;
						case "=":
							if (floatValueDictionary[array4[0]] == result2)
							{
								if (pointers.ContainsKey(array4[3]))
								{
									a = pointers[array4[3]];
								}
								else
								{
									Chat.AddMessage($"Unable to find label {array4[3]} on line {a + 1}");
								}
							}
							break;
						default:
							Chat.AddMessage(InterfaceColors.Chat.Red.ToBBCode() + "Invalid operator provided");
							break;
						}
					}
					else
					{
						Chat.AddMessage(InterfaceColors.Chat.Red.ToBBCode() + $"{array4[0]} doesn't exist within the dictionary");
					}
				}
				else
				{
					Chat.AddMessage(InterfaceColors.Chat.Red.ToBBCode() + "Invalid value provided for value.");
				}
				break;
			}
			case "/jmp":
				if (array4.Length == 0)
				{
					Chat.AddMessage($"Unable to execute jmp, no label provided. Line {a + 1}");
				}
				else if (pointers.ContainsKey(array4[0]))
				{
					a = pointers[array4[0]];
				}
				else
				{
					Chat.AddMessage($"Unable to find label {array4[0]} on line {a + 1}");
				}
				break;
			case "/execdelay":
			{
				float result3;
				if (array4.Length == 0)
				{
					Chat.AddMessage(InterfaceColors.Chat.Red.ToBBCode() + "Execdelay has no arguments provided.");
				}
				else if (float.TryParse(array4[0], out result3))
				{
					Chat.AddMessage(InterfaceColors.Chat.Yellow.ToBBCode() + "Setting ExecutationDelay to " + result3 + " second(s).");
					ExecutionDelay = result3;
				}
				else
				{
					Chat.AddMessage(InterfaceColors.Chat.Red.ToBBCode() + "Invalid execdelay number provided.");
				}
				break;
			}
			case "/wait":
			{
				float result;
				if (array4.Length == 0)
				{
					Chat.AddMessage(InterfaceColors.Chat.Red.ToBBCode() + "Sleep has no arguments provided.");
				}
				else if (float.TryParse(array4[0], out result))
				{
					Chat.AddMessage(InterfaceColors.Chat.Yellow.ToBBCode() + "Sleeping for " + result + " second(s).");
					yield return new WaitForSeconds(result);
				}
				else
				{
					Chat.AddMessage(InterfaceColors.Chat.Red.ToBBCode() + "Invalid sleep number provided.");
				}
				break;
			}
			case "/end":
				Chat.AddMessage(InterfaceColors.Chat.Yellow.ToBBCode() + "Ending routine.");
				yield break;
			default:
				if (!HandleCommand(text3, array4))
				{
					Chat.AddMessage(InterfaceColors.Chat.Yellow.ToBBCode() + "Sending " + text2);
					Chat.Instance.OnChatSubmit(text2);
				}
				break;
			}
			yield return new WaitForSeconds(ExecutionDelay);
		}
	}
}
