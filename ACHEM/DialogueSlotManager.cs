using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AQ3D.DialogueSystem;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class DialogueSlotManager : MonoBehaviour
{
	public DialogueBGSlot BGSlot;

	public DialogueCameraSlot CamSlot;

	public DialogueSlotPositioner SlotManager;

	private bool nextframe;

	private bool inprogress;

	private int DialogueID = -1;

	private DialogueOutline dialogueOutline;

	private Action Callback;

	public GameObject MusicPlayerGO;

	private AssetBundleLoader soundtrackloader;

	private AssetBundleLoader bgstageLoader;

	private List<Camera> dialogueCams = new List<Camera>();

	private Camera _uicamera;

	private int _cameraMask;

	private Dictionary<string, CutsceneCinema> cinematicMap = new Dictionary<string, CutsceneCinema>();

	private Dictionary<string, List<GameObject>> cinematicCharMap = new Dictionary<string, List<GameObject>>();

	private bool isLoading;

	private bool isPreLoaded;

	private bool timeout = true;

	private bool skipCompleteAction;

	private int curFrame;

	private string loadErrorMsg;

	public static int ReloadID = -1;

	public static int ReloadFrame = 0;

	public GameObject Container;

	private static DialogueSlotManager mInstance;

	private int SkipToFrame;

	private GameObject stage;

	public bool Inprogress
	{
		get
		{
			return inprogress;
		}
		private set
		{
			inprogress = value;
		}
	}

	private static string LoadErrorMsg
	{
		get
		{
			return Instance.loadErrorMsg;
		}
		set
		{
			Instance.loadErrorMsg = value;
		}
	}

	public static DialogueSlotManager Instance
	{
		get
		{
			if (mInstance == null)
			{
				GameObject obj = UnityEngine.Object.Instantiate(Resources.Load("DialogueSlots") as GameObject);
				obj.name = "DialogueSlots";
				mInstance = obj.GetComponent<DialogueSlotManager>();
			}
			return mInstance;
		}
	}

	public static event Action Initialized;

	public static event Action Closed;

	private void Awake()
	{
		_uicamera = UIManager.Instance.uiCamera;
		_cameraMask = _uicamera.cullingMask;
	}

	public void IncomingMessage(ResponseCommandDialog dComm)
	{
		if (DialogueID == -1)
		{
			return;
		}
		int result = -1;
		if (!int.TryParse(dComm.args[0], out result) || result != DialogueID || isLoading)
		{
			return;
		}
		string text = dComm.args[1].ToLower();
		bool flag = false;
		if (!(text == "refresh"))
		{
			if (text == "loadandrender")
			{
				int result2 = -1;
				if (dComm.args.Count >= 3 && int.TryParse(dComm.args[2], out result2))
				{
					Instance.SkipToFrame = result2;
					flag = true;
				}
			}
		}
		else
		{
			flag = true;
		}
		if (flag)
		{
			Close();
			Show(DialogueID, Callback, skipCompleteAction, Instance.SkipToFrame);
		}
	}

	private void OnDestroy()
	{
		mInstance = null;
	}

	public static void Show(int id, Action Callback = null, bool skipCompleteAction = false, int skipTo = 0)
	{
		if (id > 0 && !Instance.inprogress)
		{
			Instance.DialogueID = id;
			Instance.isLoading = true;
			Instance.inprogress = true;
			Instance.SkipToFrame = skipTo;
			Instance.Callback = Callback;
			Instance.skipCompleteAction = skipCompleteAction;
			Instance.Load(id);
		}
	}

	public static void Show(string cutscenename, Action Callback = null)
	{
		if (!Instance.inprogress)
		{
			Instance.inprogress = true;
			Instance.Callback = Callback;
			Instance.Load(cutscenename);
		}
	}

	public static void Close(bool reload = false)
	{
		if (Instance != null)
		{
			if (reload)
			{
				ReloadID = Instance.DialogueID;
				ReloadFrame = Instance.curFrame;
				Instance.inprogress = false;
			}
			Instance.Clear();
		}
	}

	private void CommonInit()
	{
		Game.Instance.IsUIVisible = true;
		if (DialogueSlotManager.Initialized != null)
		{
			DialogueSlotManager.Initialized();
		}
	}

	private void Load(int id)
	{
		StartCoroutine(LoadCoroutine(id));
	}

	public static bool HasLoadError()
	{
		if (LoadErrorMsg != null)
		{
			return true;
		}
		return false;
	}

	public static void ShowLoadErrorMsg()
	{
		MessageBox.Show("Load Error!", LoadErrorMsg);
	}

	public static bool HasPreloadedDialog()
	{
		if (HasLoadError())
		{
			return false;
		}
		if (Instance.isPreLoaded && Instance.DialogueID > 0)
		{
			return true;
		}
		return false;
	}

	public static bool LateShow(Action onComplete)
	{
		Instance.inprogress = true;
		Instance.Callback = onComplete;
		Instance.CommonInit();
		Instance.StartCoroutine("RunDialogue");
		return true;
	}

	public IEnumerator PreLoad(int id, Action onLoad = null)
	{
		if (id > 0 && !inprogress)
		{
			DialogueID = id;
			isLoading = true;
			inprogress = true;
			SkipToFrame = 0;
			Callback = null;
			skipCompleteAction = false;
			timeout = false;
			isPreLoaded = true;
			yield return GetDialogData(id);
			yield return loadBackground(dialogueOutline.BackgroundImage);
			yield return StartCoroutine("RunLoader");
		}
	}

	private IEnumerator LoadCoroutine(int id)
	{
		CommonInit();
		LoaderScreenie.Show("Loading Cutscene");
		yield return GetDialogData(id);
		yield return loadBackground(dialogueOutline.BackgroundImage);
		yield return StartCoroutine("RunLoader");
		if (HasLoadError())
		{
			ShowLoadErrorMsg();
		}
		else
		{
			yield return StartCoroutine("RunDialogue");
		}
	}

	private void Load(string cutscenename)
	{
		StartCoroutine(LoadCoroutine(cutscenename));
	}

	private IEnumerator LoadCoroutine(string cutscenename)
	{
		CommonInit();
		LoaderScreenie.Show("Loading Cinematic");
		CreateDialogueData(cutscenename);
		yield return StartCoroutine("RunLoader");
		if (HasLoadError())
		{
			ShowLoadErrorMsg();
		}
		else
		{
			yield return StartCoroutine("RunDialogue");
		}
	}

	private IEnumerator GetDialogData(int id)
	{
		using UnityWebRequest www = UnityWebRequest.Get(Main.WebServiceURL + "/Utilities/GetDialogueByID?dialogueID=" + id);
		string errorTitle = "Cinematic Error";
		string friendlyMsg = "Failed to load cinematic.";
		string customContext = "DialogueID: " + id;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		if (www.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, null, customContext);
			Clear();
			yield break;
		}
		if (www.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, null, customContext);
			Clear();
			yield break;
		}
		if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, null, customContext);
			Clear();
			yield break;
		}
		try
		{
			dialogueOutline = JsonConvert.DeserializeObject<DialogueOutline>(www.downloadHandler.text);
		}
		catch (Exception ex)
		{
			customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
			friendlyMsg = "Unable to process response from the server.";
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, null, customContext, ex);
			Clear();
			yield break;
		}
	}

	private void CreateDialogueData(string cname)
	{
		dialogueOutline = new DialogueOutline
		{
			BackgroundImage = string.Empty,
			ID = -1,
			LightingType = DialogueLightingType.Scene
		};
		dialogueOutline.Characters = new List<DialogueCharacter>();
		dialogueOutline.FrameCollection = new DialogueFrame[1]
		{
			new DialogueFrame
			{
				ID = -1,
				CutsceneName = cname,
				DialogueName = string.Empty,
				DialogueText = string.Empty,
				FrameActions = new DialogueFrameAction[0],
				FrameType = DialogueFrameType.CinematicOnly
			}
		};
	}

	private IEnumerator StartTimeout()
	{
		yield return new WaitForSeconds(15f);
		ErrorReporting.Instance.ReportError("Dialogue/Cutscene Timed Out", "Dialogue/Cutscene timed Out", "Dialogue ID: " + DialogueID, null, "", null, showMessageBox: false);
		MessageBox.Show("Error", "This operation timed out.");
		Clear();
	}

	private IEnumerator RunLoader()
	{
		LoadErrorMsg = null;
		if (dialogueOutline.FrameCollection == null)
		{
			Clear();
			LoadErrorMsg = "Dialog has no frames!";
			OnCompleteCallback();
			yield break;
		}
		foreach (string item in from p in dialogueOutline.FrameCollection
			where p.IsCinematic
			select p.CutsceneName)
		{
			CutsceneCinema cutsceneCinema = CutsceneCinema.FindByName(item);
			if (cutsceneCinema == null)
			{
				Clear();
				LoadErrorMsg = "Cinematic '" + item + "' not found in the map.";
				OnCompleteCallback();
				yield break;
			}
			dialogueOutline.Characters.AddRange(from p in cutsceneCinema.AllCharacters
				group p by p.OffsetY into grp
				select grp.First());
			cutsceneCinema.CinematicComplete = (Action)Delegate.Combine(cutsceneCinema.CinematicComplete, new Action(OnCinematicComplete));
			cinematicMap[item] = cutsceneCinema;
			cinematicCharMap[item] = new List<GameObject>();
		}
		if (timeout)
		{
			StartCoroutine("StartTimeout");
		}
		yield return LoadCharacters(dialogueOutline.Characters);
	}

	private IEnumerator RunDialogue()
	{
		int skipLocal = SkipToFrame;
		SkipToFrame = 0;
		StopCoroutine("StartTimeout");
		DisableCams();
		yield return LoadSoundTrack();
		DialogueFrame frame = dialogueOutline.FrameCollection[skipLocal];
		DialogueOverlay.Init(this);
		UIPanel fadePanel = DialogueOverlay.mInstance.DialogueFadePanel;
		if (frame.dialogueFadeType == DialogueFadeType.None)
		{
			fadePanel.alpha = 0f;
		}
		else if (frame.dialogueFadeType == DialogueFadeType.FadeIn || frame.dialogueFadeType == DialogueFadeType.Both)
		{
			fadePanel.alpha = 1f;
		}
		isLoading = false;
		Entities entities = Entities.Instance;
		SlotManager.SetPosition(dialogueOutline.SceneType);
		CamSlot.InitLighting((DialogueCameraSlot.LightingType)dialogueOutline.LightingType);
		LoaderScreenie.Close();
		Loader.close();
		if (SkipToFrame >= dialogueOutline.FrameCollection.Length)
		{
			SkipToFrame = 0;
		}
		for (curFrame = skipLocal; curFrame < dialogueOutline.FrameCollection.Length; curFrame++)
		{
			frame = dialogueOutline.FrameCollection[curFrame];
			if (dialogueOutline.SceneType == DialogueSceneType.Scene)
			{
				SlotManager.SetPosition(dialogueOutline.SceneType, frame.dialogueScenePosition);
			}
			if (frame.dialogueFadeType == DialogueFadeType.None || frame.IsCinematic)
			{
				fadePanel.alpha = 0f;
			}
			else if (frame.dialogueFadeType == DialogueFadeType.FadeIn || frame.dialogueFadeType == DialogueFadeType.Both)
			{
				fadePanel.alpha = 1f;
			}
			DialogueOverlay.Show((int)frame.FrameType);
			CamSlot.ActiveCam.gameObject.SetActive(frame.FrameType == DialogueFrameType.DialogOnly);
			if (frame.IsDialog)
			{
				DialogueOverlay.SetText(entities.me.ScrubText(frame.DialogueName), entities.me.ScrubText(frame.DialogueText), (int)frame.FrameType, frame.alignment, entities.me.ScrubText(frame.DialogueTitle));
			}
			if (frame.FrameType == DialogueFrameType.DialogOnly)
			{
				SlotManager.InitSlots(frame.SlotData, CamSlot.ActiveCam.gameObject);
				RunStartActions(frame);
				Loader.close();
				if (frame.dialogueFadeType == DialogueFadeType.FadeIn || frame.dialogueFadeType == DialogueFadeType.Both)
				{
					yield return StartCoroutine(DialogueFade(frame.fadeDuration, DialogueFadeType.FadeIn, frame.fadeInColor));
				}
				if (frame.CameraShakeIntensity > 0f && frame.CameraShakeDuration > 0f)
				{
					Vector3 vector = new Vector3(UnityEngine.Random.Range(0f - frame.CameraShakeIntensity, frame.CameraShakeIntensity), UnityEngine.Random.Range(0f - frame.CameraShakeIntensity, frame.CameraShakeIntensity), 0f);
					Hashtable args = iTween.Hash("name", base.gameObject.name + "_shake", "amount", vector, "time", frame.CameraShakeDuration);
					iTween.ShakePosition(CamSlot.ActiveCam.gameObject, args);
				}
				else
				{
					iTween.Stop(CamSlot.ActiveCam.gameObject);
				}
			}
			else
			{
				cinematicCharMap[frame.CutsceneName].ForEach(delegate(GameObject p)
				{
					p.SetActive(value: true);
				});
				Loader.close();
				cinematicMap[frame.CutsceneName].PlayCutscene();
			}
			nextframe = false;
			float elapsedFrameTime = frame.frameTime;
			while (!nextframe)
			{
				if (frame.timedFrame)
				{
					if (elapsedFrameTime > 0f)
					{
						elapsedFrameTime -= Time.deltaTime;
					}
					else
					{
						nextframe = true;
					}
				}
				yield return null;
			}
			if (frame.IsCinematic)
			{
				cinematicMap[frame.CutsceneName].StopCutscene();
				cinematicCharMap[frame.CutsceneName].ForEach(delegate(GameObject p)
				{
					p.SetActive(value: false);
				});
			}
			if (frame.FrameType == DialogueFrameType.DialogOnly && (frame.dialogueFadeType == DialogueFadeType.FadeOut || frame.dialogueFadeType == DialogueFadeType.Both))
			{
				yield return StartCoroutine(DialogueFade(frame.fadeDuration, DialogueFadeType.FadeOut, frame.fadeOutColor));
			}
		}
		Clear();
		OnCompleteCallback();
	}

	private IEnumerator DialogueFade(float time, DialogueFadeType dialogueFadeType, Color color)
	{
		float curtime = time;
		UIPanel fadePanel = DialogueOverlay.mInstance.DialogueFadePanel;
		fadePanel.GetComponentInChildren<UISprite>().SetColorNoAlpha(color);
		while (curtime > 0f)
		{
			curtime -= Time.deltaTime;
			float num = curtime / time;
			switch (dialogueFadeType)
			{
			case DialogueFadeType.FadeIn:
				fadePanel.alpha = num;
				break;
			case DialogueFadeType.FadeOut:
				fadePanel.alpha = 1f - num;
				break;
			}
			yield return null;
		}
	}

	private void RunStartActions(DialogueFrame frame)
	{
		for (int i = 0; i < frame.FrameActions.Length; i++)
		{
			if (frame.FrameActions[i].FrameID == 0 && frame.FrameActions[i].TransitionID != -1)
			{
				int num = frame.SlotData.FindIndex((SlotPosition x) => x.SlotID == 0);
				if (num > -1)
				{
					CamSlot.MoveCam(frame.FrameActions[i].TransitionData, frame.SlotData[num]);
				}
			}
			else if (frame.FrameActions[i].FrameID != 0)
			{
				SlotManager.PlayAnimation(dialogueOutline.ID, frame.ID, frame.FrameActions[i].AnimationState, frame.FrameActions[i].SlotID);
				SlotManager.SetFace(frame.FrameActions[i].Mouth, frame.FrameActions[i].Eyes, frame.FrameActions[i].SlotID);
			}
			SlotManager.SetWeapon(frame.FrameActions[i].SlotID, frame.FrameActions[i].UsesWeaponOverride);
			if (frame.FrameActions[i].AudioPan != 0f)
			{
				_ = frame.FrameActions[i].AudioPan;
			}
			else
			{
				_ = frame.SlotData[i].AudioPan;
			}
			AudioManager.Play2DSFX(frame.FrameActions[i].AudioClip, frame.FrameActions[i].AudioDelay, frame.FrameActions[i].AudioVolume, frame.SlotData[i].AudioPan, SFXType.Dialogue);
		}
	}

	private IEnumerator loadBackground(string url)
	{
		BGSlot.SetSphereActive(show: false);
		if (dialogueOutline.SceneType == DialogueSceneType.BackgroundImage && !string.IsNullOrEmpty(dialogueOutline.BackgroundImage))
		{
			using UnityWebRequest www = UnityWebRequestTexture.GetTexture(Main.APPLICATION_PATH + "/gamefiles/images/" + url);
			string errorTitle = "Loading Error";
			string friendlyMsg = "Failed to load dialogue background.";
			string customContext = "URL: " + url;
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
			BGSlot.SetBackground(DownloadHandlerTexture.GetContent(www));
			BGSlot.SetSphereActive(show: true);
		}
		else
		{
			if (dialogueOutline.SceneType != DialogueSceneType.LoadedStage || dialogueOutline.BackgroundBundle == null)
			{
				yield break;
			}
			bgstageLoader = AssetBundleManager.LoadAssetBundle(dialogueOutline.BackgroundBundle);
			while (!bgstageLoader.isDone)
			{
				LoaderScreenie.Show("Loading Background " + (int)(bgstageLoader.GetProgress() * 100f) + "%");
				yield return null;
			}
			if (!string.IsNullOrEmpty(bgstageLoader.Error))
			{
				Debug.LogError(bgstageLoader.Error);
				yield break;
			}
			AssetBundleRequest abr = bgstageLoader.Asset.LoadAssetAsync<GameObject>(dialogueOutline.BackgroundPrefab);
			yield return abr;
			if (!(abr.asset == null))
			{
				stage = UnityEngine.Object.Instantiate(abr.asset as GameObject, SlotManager.transform);
				Closed += UnloadStage;
			}
		}
	}

	private void UnloadStage()
	{
		if (stage != null)
		{
			UnityEngine.Object.Destroy(stage);
		}
	}

	private IEnumerator LoadCharacters(List<DialogueCharacter> characters)
	{
		if (characters.Count <= 0)
		{
			yield break;
		}
		Dictionary<int, EntityAsset> dictionary = new Dictionary<int, EntityAsset>();
		List<int> list = (from p in characters
			where p.NPCID > 0
			select p.NPCID).Distinct().ToList();
		if (list.Count > 0)
		{
			string text = string.Join(",", list.Select((int p) => p.ToString()).ToArray());
			using UnityWebRequest www = UnityWebRequest.Get(Main.WebServiceURL + "/Utilities/GetAllNPCAssetData?idtosplit=" + text);
			string errorTitle = "Loading Error";
			string friendlyMsg = "Failed to load NPC data";
			string customContext = "idtosplit: " + text;
			yield return www.SendWebRequest();
			customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
			if (www.isHttpError)
			{
				ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, null, customContext);
				Clear();
				yield break;
			}
			if (www.isNetworkError)
			{
				ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, null, customContext);
				Clear();
				yield break;
			}
			if (www.error != null)
			{
				ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, null, customContext);
				Clear();
				yield break;
			}
			try
			{
				dictionary = JsonConvert.DeserializeObject<Dictionary<int, EntityAsset>>(www.downloadHandler.text);
			}
			catch (Exception ex)
			{
				customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
				ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, null, customContext, ex);
				Clear();
				yield break;
			}
		}
		if (characters.Any((DialogueCharacter p) => p.NPCID == 0))
		{
			dictionary[0] = Entities.Instance.me.baseAsset;
		}
		Dictionary<int, AssetController> assetMap = new Dictionary<int, AssetController>();
		foreach (KeyValuePair<int, EntityAsset> item in dictionary)
		{
			EntityAsset entityAsset = dictionary[item.Key];
			GameObject gameObject = new GameObject();
			gameObject.layer = Layers.CUTSCENE;
			AssetController assetController = ((entityAsset.gender == "N") ? ((AssetController)gameObject.AddComponent<NPCAssetController>()) : ((AssetController)gameObject.AddComponent<PlayerAssetController>()));
			assetController.showorb = false;
			assetController.Init(entityAsset);
			assetMap[item.Key] = assetController;
			assetController.Load();
			gameObject.transform.parent = Container.transform;
		}
		while (assetMap.Values.Any((AssetController p) => !p.IsAssetLoadComplete))
		{
			yield return null;
		}
		List<int> list2 = new List<int>();
		foreach (DialogueCharacter character in characters)
		{
			GameObject gameObject2;
			if (!list2.Contains(character.NPCID))
			{
				gameObject2 = assetMap[character.NPCID].gameObject;
				list2.Add(character.NPCID);
			}
			else
			{
				gameObject2 = UnityEngine.Object.Instantiate(assetMap[character.NPCID].gameObject);
				gameObject2.transform.parent = Container.transform;
			}
			gameObject2.name = "DialogChar" + character.NPCID;
			gameObject2.SetActive(value: false);
			SlotManager.AddCharacterToSlot(character.SlotID, gameObject2);
			SkinnedMeshRenderer[] componentsInChildren = gameObject2.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].updateWhenOffscreen = true;
			}
			if (!string.IsNullOrEmpty(character.CutsceneName))
			{
				cinematicMap[character.CutsceneName].AddCharacterToGroup(character, gameObject2);
				cinematicCharMap[character.CutsceneName].Add(gameObject2);
			}
		}
	}

	public void ContinueAction()
	{
		nextframe = true;
	}

	public void SkipAction()
	{
		if (!dialogueOutline.FrameCollection[curFrame].IsCinematic)
		{
			for (int i = curFrame; i < dialogueOutline.FrameCollection.Length && !dialogueOutline.FrameCollection[i].IsCinematic; i++)
			{
				curFrame = i;
			}
		}
		nextframe = true;
	}

	private void OnCinematicComplete()
	{
		ContinueAction();
	}

	private IEnumerator LoadSoundTrack()
	{
		if (dialogueOutline.SoundTrackBundle == null)
		{
			yield break;
		}
		soundtrackloader = AssetBundleManager.LoadAssetBundle(dialogueOutline.SoundTrackBundle);
		while (!soundtrackloader.isDone)
		{
			LoaderScreenie.Show("Loading SoundTrack " + (int)(soundtrackloader.GetProgress() * 100f) + "%");
			yield return null;
		}
		if (!string.IsNullOrEmpty(soundtrackloader.Error))
		{
			Debug.LogError(soundtrackloader.Error);
			yield break;
		}
		AssetBundleRequest abr = soundtrackloader.Asset.LoadAssetAsync<GameObject>(dialogueOutline.SoundTrack);
		yield return abr;
		if (!(abr.asset == null))
		{
			MusicPlayerGO = (GameObject)UnityEngine.Object.Instantiate(abr.asset);
		}
	}

	private IEnumerator CleanupMusic()
	{
		if (dialogueOutline != null && dialogueOutline.SoundTrack != null)
		{
			if (MusicPlayerGO != null)
			{
				UnityEngine.Object.Destroy(MusicPlayerGO);
				soundtrackloader.Dispose();
			}
			yield return new WaitForFixedUpdate();
		}
	}

	public void DisableCams()
	{
		_uicamera.cullingMask &= ~(1 << LayerMask.NameToLayer("NGUI"));
		_uicamera.cullingMask |= 1 << LayerMask.NameToLayer("NGUIFLY");
		dialogueCams = Camera.allCameras.Where((Camera p) => p.enabled && p != _uicamera).ToList();
		dialogueCams.ForEach(delegate(Camera p)
		{
			p.enabled = false;
		});
	}

	public void EnableCams(bool setbloom = true)
	{
		_uicamera.cullingMask = _cameraMask;
		dialogueCams.ForEach(delegate(Camera p)
		{
			p.enabled = true;
		});
		dialogueCams.Clear();
	}

	private void OnCompleteCallback()
	{
		if (!skipCompleteAction)
		{
			ClientTriggerAction.CallFromString(dialogueOutline.CompleteAction);
		}
		Callback?.Invoke();
		Callback = null;
	}

	private void Clear()
	{
		foreach (CutsceneCinema value in cinematicMap.Values)
		{
			value.CinematicComplete = (Action)Delegate.Remove(value.CinematicComplete, new Action(OnCinematicComplete));
			value.StopCutscene();
		}
		cinematicMap.Clear();
		cinematicCharMap.Clear();
		DialogueOverlay.Close();
		Loader.close();
		CamSlot.ResetLighting();
		StartCoroutine(CleanupMusic());
		StopCoroutine("RunLoader");
		StopCoroutine("RunDialogue");
		StopCoroutine("StartTimeout");
		SlotManager.ClearObjects();
		UnityEngine.Object.Destroy(Container);
		Container = new GameObject("Container");
		Container.transform.parent = base.transform;
		CamSlot.ActiveCam.gameObject.SetActive(value: false);
		EnableCams(setbloom: false);
		inprogress = false;
		isPreLoaded = false;
		LoadErrorMsg = null;
		Resources.UnloadUnusedAssets();
		LoaderScreenie.Close();
		if (UINPCDialog.Instance == null)
		{
			Entities.Instance?.me?.EndNPCInteraction();
		}
		DialogueSlotManager.Closed?.Invoke();
	}
}
