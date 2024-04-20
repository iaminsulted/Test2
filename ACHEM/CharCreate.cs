using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using StatCurves;
using UnityEngine;
using UnityEngine.Networking;

public class CharCreate : State
{
	public UIInput txtCharname;

	public UIToggle chkMale;

	public UIToggle chkFemale;

	public UIButton btnCreate;

	public UIButton btnRandomName;

	public UIButton btnRandomLook;

	public UIButton ContinueButton;

	public GameObject NameCharacter;

	public UIButton CloseCharacterNameButton;

	public UILabel lblClass;

	public UILabel lblClassDesc;

	public GameObject Glow;

	public Transform[] ClassButtons;

	private readonly Vector3 startPosition = new Vector3(-0.3f, 0f, -9.9f);

	private Vector3 endPosition;

	private Coroutine camRoutine;

	private EntityAsset curAsset;

	private PlayerCustomizeAssetController assetController;

	private int classIndex;

	public List<string> classNames;

	private Dictionary<int, EquipItem> baseItems;

	public UISprite HairColor;

	public UISprite SkinColor;

	public UISprite EyeColor;

	public UISprite LipColor;

	public UILabel HairCount;

	public UILabel BraidCount;

	public UILabel BeardCount;

	public UILabel StacheCount;

	public UILabel HairColorCount;

	public UILabel SkinCount;

	public UILabel EyeCount;

	public UILabel LipCount;

	private int beardIndex;

	private int braidIndex;

	private int hairIndex;

	private int stacheIndex;

	private List<BeardStyle> BeardStyles;

	private List<BraidStyle> BraidStyles;

	private List<HairStyle> HairStyles;

	private List<StacheStyle> StacheStyles;

	private readonly Queue<string> fantasyNames = new Queue<string>();

	private bool isGettingFantasyNames;

	private int classID;

	private bool animateClass = true;

	private bool stopIdleBlend;

	private GameObject playerGO;

	private GameObject warriorParticle;

	private GameObject mageParticle;

	private GameObject rogueParticle;

	private GameObject healerParticle;

	private bool canMakeParticle;

	private Animator animator;

	public override void Init()
	{
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharCreate_StartedOk);
		base.Init();
		UIEventListener uIEventListener = UIEventListener.Get(chkMale.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnGenderClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(chkFemale.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnGenderClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnCreate.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnCreateClick));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnRandomName.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnRandomNameClick));
		UIEventListener uIEventListener5 = UIEventListener.Get(btnRandomLook.gameObject);
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener5.onClick, new UIEventListener.VoidDelegate(OnRandomLookClick));
		UIEventListener uIEventListener6 = UIEventListener.Get(ContinueButton.gameObject);
		uIEventListener6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener6.onClick, new UIEventListener.VoidDelegate(ToggleCharacterName));
		UIEventListener uIEventListener7 = UIEventListener.Get(CloseCharacterNameButton.gameObject);
		uIEventListener7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener7.onClick, new UIEventListener.VoidDelegate(ToggleCharacterName));
		txtCharname.onValidate = InputCharNameValidate;
		BeardStyles = (from p in StylesUtil.BeardStyles
			where !p.Value.HasRequirement()
			select p.Value).ToList();
		BraidStyles = (from p in StylesUtil.BraidStyles
			where !p.Value.HasRequirement()
			select p.Value).ToList();
		HairStyles = (from p in StylesUtil.HairStyles
			where !p.Value.HasRequirement()
			select p.Value).ToList();
		StacheStyles = (from p in StylesUtil.StacheStyles
			where !p.Value.HasRequirement()
			select p.Value).ToList();
		classIndex = 0;
		classNames = new List<string> { "Warrior", "Mage", "Rogue", "Healer" };
		endPosition = new Vector3(-0.66f, startPosition.y + 0.7f, startPosition.z + 2.5f);
		HairColor.color = StylesUtil.EntityHairColors[0];
		EyeColor.color = StylesUtil.EntityEyeColors[0];
		SkinColor.color = StylesUtil.EntitySkinColors[0].B;
		LipColor.color = StylesUtil.EntityLipColors[0];
		StartCoroutine(Load());
		SendRequestGetFantasyNames(populateInputField: false);
		HairCount.text = "1/" + HairStyles.Count;
		BraidCount.text = "1/" + BraidStyles.Count;
		BeardCount.text = "1/" + BeardStyles.Count;
		StacheCount.text = "1/" + StacheStyles.Count;
		HairColorCount.text = "1/" + StylesUtil.EntityHairColors.Count;
		SkinCount.text = "1/" + StylesUtil.EntitySkinColors.Count;
		EyeCount.text = "1/" + StylesUtil.EntityEyeColors.Count;
		LipCount.text = "1/" + StylesUtil.EntityLipColors.Count;
		Camera.main.transform.position = new Vector3(-0.35f, 0f, -20f);
		camRoutine = StartCoroutine(MoveCam(startPosition, 1.1f));
	}

	private void ToggleCharacterName(GameObject go)
	{
		NameCharacter.SetActive(!NameCharacter.activeSelf);
	}

	private IEnumerator Load()
	{
		Loader.show("Loading creator...", 1f);
		AssetBundleLoader requestBundle = AssetBundleManager.LoadAssetBundle(Session.Account.CharSelectData.BundleInfo);
		yield return requestBundle;
		GameObject gameObject = requestBundle.Asset.LoadAsset<GameObject>(Session.Account.CharSelectData.EnvPrefab);
		if (gameObject != null)
		{
			GameObject obj = UnityEngine.Object.Instantiate(gameObject, base.transform);
			obj.transform.localPosition = new Vector3(-0.75f, -1.05f, -6.75f);
			obj.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
			obj.AddComponent<GrassToggle>();
		}
		requestBundle.Dispose();
		AssetBundleLoader charactersBundle = AssetBundleManager.LoadAssetBundle(Session.Account.bundles["Characters"]);
		yield return charactersBundle;
		warriorParticle = charactersBundle.Asset.LoadAsset<GameObject>("GroundPoundRocks");
		mageParticle = charactersBundle.Asset.LoadAsset<GameObject>("LightningBoltSplash");
		rogueParticle = charactersBundle.Asset.LoadAsset<GameObject>("DoubleAOESlash");
		healerParticle = charactersBundle.Asset.LoadAsset<GameObject>("HealerUltCast_Fx");
		canMakeParticle = true;
		charactersBundle.Dispose();
		Loader.close();
		WWWForm form = new WWWForm();
		string text = "2, 25, 48, 3, 50, 51, 52, 7503";
		form.AddField("IDs", text);
		using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL + "/Game/GetItems", form);
		string errorTitle = "Loading Error";
		string friendlyMsg = "Failed to load character creation item list.";
		string customContext = "ItemList: " + text;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		if (www.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, form, customContext);
			yield break;
		}
		if (www.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, form, customContext);
			yield break;
		}
		if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, form, customContext);
			yield break;
		}
		Debug.Log("WWW Ok!: " + www.downloadHandler.text);
		try
		{
			List<Item> source = JsonConvert.DeserializeObject<List<Item>>(www.downloadHandler.text);
			baseItems = ((IEnumerable<Item>)source).ToDictionary((Func<Item, int>)((Item p) => p.ID), (Func<Item, EquipItem>)((Item p) => p));
			UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharCreate_BaseItemsOk);
			playerGO = new GameObject("Player");
			playerGO.transform.localPosition = new Vector3(-0.75f, -1.05f, -6.75f);
			playerGO.transform.localRotation = Quaternion.Euler(0f, 150f, 0f);
			playerGO.transform.SetParent(base.transform, worldPositionStays: false);
			playerGO.AddComponent<DragRotateSlowDown>();
			playerGO.layer = Layers.PLAYER_ME;
			curAsset = new EntityAsset();
			curAsset.gender = (ArtixRandom.RandomBool() ? "M" : "F");
			int num = ArtixRandom.Range(0, classNames.Count - 1);
			SetClass(num);
			MoveGlow(ClassButtons[num]);
			assetController = playerGO.AddComponent<PlayerCustomizeAssetController>();
			assetController.AssetUpdated += AssetReady;
			assetController.Init(curAsset);
			assetController.Load();
			UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharCreate_InitComplete);
		}
		catch (Exception ex)
		{
			customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, form, customContext, ex);
		}
	}

	private void AssetReady(GameObject go)
	{
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharCreate_CharLoadedOk);
		animator = assetController.currentAsset.GetComponent<Animator>();
		if (animateClass)
		{
			DoClassAnimation();
		}
	}

	private void DoClassAnimation()
	{
		if (!(animator == null))
		{
			animateClass = false;
			DoCombatIdle();
			switch (classID)
			{
			case 0:
				animator.Play("GroundPound");
				StartCoroutine(CreateClassParticle(warriorParticle, 0.55f, -1.5f));
				break;
			case 1:
				animator.Play("2H_CastUp");
				StartCoroutine(CreateClassParticle(mageParticle, 0.25f, -1.5f));
				break;
			case 2:
				animator.Play("CrossSlash");
				StartCoroutine(CreateClassParticle(rogueParticle, 0.5f, 0f));
				break;
			case 3:
				animator.Play("CastSelf");
				StartCoroutine(CreateClassParticle(healerParticle, 0.25f, -0.5f));
				break;
			}
		}
	}

	private IEnumerator CreateClassParticle(GameObject particlePrefab, float delay, float yOffset)
	{
		if (canMakeParticle)
		{
			canMakeParticle = false;
			yield return new WaitForSeconds(delay);
			GameObject gameObject = UnityEngine.Object.Instantiate(particlePrefab, playerGO.transform);
			gameObject.transform.position = new Vector3(gameObject.transform.position.x, yOffset, gameObject.transform.position.z);
			yield return new WaitForSeconds(1.25f);
			canMakeParticle = true;
		}
	}

	private void DoCombatIdle()
	{
		stopIdleBlend = true;
		animator?.SetFloat("IdleBlend", 0.5f);
	}

	private void ReturnToStanding()
	{
		stopIdleBlend = false;
		StartCoroutine(IdleBlendRoutine());
	}

	private IEnumerator IdleBlendRoutine()
	{
		while (animator != null && animator.GetFloat("IdleBlend") > 0f && !stopIdleBlend)
		{
			animator.SetFloat("IdleBlend", 0f, 0.2f, Time.deltaTime);
			yield return null;
		}
	}

	private void SetBeard(int index)
	{
		if (index < 0)
		{
			index = 0;
		}
		beardIndex = index;
		curAsset.Beard = BeardStyles[index].ID;
	}

	private void SetBraid(int index)
	{
		if (index < 0)
		{
			index = 0;
		}
		braidIndex = index;
		curAsset.Braid = BraidStyles[index].ID;
	}

	private void SetHair(int index)
	{
		if (index < 0)
		{
			index = 0;
		}
		hairIndex = index;
		curAsset.Hair = HairStyles[index].ID;
		if ((double)ArtixRandom.Range(0f, 1f) > 0.7 && animator != null)
		{
			animator.Play("CheckShoulders");
		}
	}

	private void SetStache(int index)
	{
		if (index < 0)
		{
			index = 0;
		}
		stacheIndex = index;
		curAsset.Stache = StacheStyles[index].ID;
	}

	private void SetClass(int index)
	{
		animateClass = true;
		classID = index;
		classIndex = index;
		curAsset.equips.Clear();
		switch (index)
		{
		case 0:
			curAsset.equips.Add(EquipItemSlot.Class, baseItems[2]);
			curAsset.equips.Add(EquipItemSlot.Boots, baseItems[3]);
			curAsset.equips.Add(EquipItemSlot.Weapon, baseItems[50]);
			curAsset.DualWield = false;
			lblClassDesc.text = "Build Fury to deliver devastating blows!";
			break;
		case 1:
			curAsset.equips.Add(EquipItemSlot.Class, baseItems[25]);
			curAsset.equips.Add(EquipItemSlot.Boots, baseItems[3]);
			curAsset.equips.Add(EquipItemSlot.Weapon, baseItems[52]);
			curAsset.DualWield = false;
			lblClassDesc.text = "Wield magic to control the battlefield!";
			break;
		case 2:
			curAsset.equips.Add(EquipItemSlot.Class, baseItems[48]);
			curAsset.equips.Add(EquipItemSlot.Boots, baseItems[3]);
			curAsset.equips.Add(EquipItemSlot.Weapon, baseItems[51]);
			curAsset.DualWield = true;
			lblClassDesc.text = "Strike down your enemies with Deadly Poison!";
			break;
		case 3:
			curAsset.equips.Add(EquipItemSlot.Class, baseItems[7503]);
			curAsset.equips.Add(EquipItemSlot.Boots, baseItems[3]);
			curAsset.equips.Add(EquipItemSlot.Weapon, baseItems[52]);
			curAsset.DualWield = false;
			lblClassDesc.text = "Heal and Empower your allies!";
			break;
		}
		lblClass.text = classNames[index];
	}

	private void OnGenderClick(GameObject go)
	{
		string text = "M";
		if (chkFemale.value)
		{
			text = "F";
		}
		if (curAsset.gender != text)
		{
			animateClass = true;
			curAsset.gender = text;
			assetController.Load();
		}
		ZoomCamera(zoomIn: false);
	}

	public void OnHairLeft()
	{
		SetHair((hairIndex + HairStyles.Count - 1) % HairStyles.Count);
		assetController.UpdateHair();
		ZoomCamera(zoomIn: true);
		HairCount.text = hairIndex + 1 + "/" + HairStyles.Count;
	}

	public void OnHairRight()
	{
		SetHair((hairIndex + 1) % HairStyles.Count);
		assetController.UpdateHair();
		ZoomCamera(zoomIn: true);
		HairCount.text = hairIndex + 1 + "/" + HairStyles.Count;
	}

	public void ResetHair()
	{
		SetHair(14);
		assetController.UpdateHair();
		ZoomCamera(zoomIn: true);
		HairCount.text = hairIndex + 1 + "/" + HairStyles.Count;
	}

	public void OnBraidLeft()
	{
		SetBraid((braidIndex + BraidStyles.Count - 1) % BraidStyles.Count);
		assetController.UpdateBraid();
		ZoomCamera(zoomIn: true);
		BraidCount.text = braidIndex + 1 + "/" + BraidStyles.Count;
	}

	public void OnBraidRight()
	{
		SetBraid((braidIndex + 1) % BraidStyles.Count);
		assetController.UpdateBraid();
		ZoomCamera(zoomIn: true);
		BraidCount.text = braidIndex + 1 + "/" + BraidStyles.Count;
	}

	public void ResetBraid()
	{
		SetBraid(0);
		assetController.UpdateBraid();
		ZoomCamera(zoomIn: true);
		BraidCount.text = braidIndex + 1 + "/" + BraidStyles.Count;
	}

	public void OnBeardLeft()
	{
		SetBeard((beardIndex + BeardStyles.Count - 1) % BeardStyles.Count);
		assetController.UpdateBeard();
		ZoomCamera(zoomIn: true);
		BeardCount.text = beardIndex + 1 + "/" + BeardStyles.Count;
	}

	public void OnBeardRight()
	{
		SetBeard((beardIndex + 1) % BeardStyles.Count);
		assetController.UpdateBeard();
		ZoomCamera(zoomIn: true);
		BeardCount.text = beardIndex + 1 + "/" + BeardStyles.Count;
	}

	public void ResetBeard()
	{
		SetBeard(0);
		assetController.UpdateBeard();
		ZoomCamera(zoomIn: true);
		BeardCount.text = beardIndex + 1 + "/" + BeardStyles.Count;
	}

	public void OnStacheLeft()
	{
		SetStache((stacheIndex + StacheStyles.Count - 1) % StacheStyles.Count);
		assetController.UpdateStache();
		ZoomCamera(zoomIn: true);
		StacheCount.text = stacheIndex + 1 + "/" + StacheStyles.Count;
	}

	public void OnStacheRight()
	{
		SetStache((stacheIndex + 1) % StacheStyles.Count);
		assetController.UpdateStache();
		ZoomCamera(zoomIn: true);
		StacheCount.text = stacheIndex + 1 + "/" + StacheStyles.Count;
	}

	public void ResetStache()
	{
		SetStache(0);
		assetController.UpdateStache();
		ZoomCamera(zoomIn: true);
		StacheCount.text = stacheIndex + 1 + "/" + StacheStyles.Count;
	}

	public void OnHairColorLeft()
	{
		curAsset.ColorHair = (curAsset.ColorHair + StylesUtil.EntityHairColors.Count - 1) % StylesUtil.EntityHairColors.Count;
		assetController.UpdateHairSkinColor();
		ZoomCamera(zoomIn: true);
		HairColor.color = StylesUtil.EntityHairColors[curAsset.ColorHair];
		HairColorCount.text = (curAsset.ColorHair + StylesUtil.EntityHairColors.Count - 1) % StylesUtil.EntityHairColors.Count + 1 + "/" + StylesUtil.EntityHairColors.Count;
	}

	public void OnHairColorRight()
	{
		curAsset.ColorHair = (curAsset.ColorHair + 1) % StylesUtil.EntityHairColors.Count;
		assetController.UpdateHairSkinColor();
		ZoomCamera(zoomIn: true);
		HairColor.color = StylesUtil.EntityHairColors[curAsset.ColorHair];
		HairColorCount.text = (curAsset.ColorHair + StylesUtil.EntityHairColors.Count - 1) % StylesUtil.EntityHairColors.Count + 1 + "/" + StylesUtil.EntityHairColors.Count;
	}

	public void OnSkinColorLeft()
	{
		curAsset.ColorSkin = (curAsset.ColorSkin + StylesUtil.EntitySkinColors.Count - 1) % StylesUtil.EntitySkinColors.Count;
		assetController.UpdateHairSkinColor();
		ZoomCamera(zoomIn: true);
		SkinColor.color = StylesUtil.EntitySkinColors[curAsset.ColorSkin].B;
		if ((double)UnityEngine.Random.value > 0.7)
		{
			animator.Play("Check2Hands");
		}
		SkinCount.text = (curAsset.ColorSkin + StylesUtil.EntitySkinColors.Count - 1) % StylesUtil.EntitySkinColors.Count + 1 + "/" + StylesUtil.EntitySkinColors.Count;
	}

	public void OnSkinColorRight()
	{
		curAsset.ColorSkin = (curAsset.ColorSkin + 1) % StylesUtil.EntitySkinColors.Count;
		assetController.UpdateHairSkinColor();
		ZoomCamera(zoomIn: true);
		SkinColor.color = StylesUtil.EntitySkinColors[curAsset.ColorSkin].B;
		if ((double)UnityEngine.Random.value > 0.7)
		{
			animator.Play("Check2Hands");
		}
		SkinCount.text = (curAsset.ColorSkin + StylesUtil.EntitySkinColors.Count - 1) % StylesUtil.EntitySkinColors.Count + 1 + "/" + StylesUtil.EntitySkinColors.Count;
	}

	public void OnEyeColorLeft()
	{
		curAsset.ColorEye = (curAsset.ColorEye + StylesUtil.EntityEyeColors.Count - 1) % StylesUtil.EntityEyeColors.Count;
		assetController.UpdateEyeColor();
		ZoomCamera(zoomIn: true);
		EyeColor.color = StylesUtil.EntityEyeColors[curAsset.ColorEye];
		EyeCount.text = (curAsset.ColorEye + StylesUtil.EntityEyeColors.Count - 1) % StylesUtil.EntityEyeColors.Count + 1 + "/" + StylesUtil.EntityEyeColors.Count;
	}

	public void OnEyeColorRight()
	{
		curAsset.ColorEye = (curAsset.ColorEye + 1) % StylesUtil.EntityEyeColors.Count;
		assetController.UpdateEyeColor();
		ZoomCamera(zoomIn: true);
		EyeColor.color = StylesUtil.EntityEyeColors[curAsset.ColorEye];
		EyeCount.text = (curAsset.ColorEye + StylesUtil.EntityEyeColors.Count - 1) % StylesUtil.EntityEyeColors.Count + 1 + "/" + StylesUtil.EntityEyeColors.Count;
	}

	public void OnLipColorLeft()
	{
		curAsset.ColorLip = (curAsset.ColorLip + StylesUtil.EntityLipColors.Count - 1) % StylesUtil.EntityLipColors.Count;
		assetController.UpdateLipColor();
		ZoomCamera(zoomIn: true);
		LipColor.color = StylesUtil.EntityLipColors[curAsset.ColorLip];
		LipCount.text = (curAsset.ColorLip + StylesUtil.EntityLipColors.Count - 1) % StylesUtil.EntityLipColors.Count + 1 + "/" + StylesUtil.EntityLipColors.Count;
	}

	public void OnLipColorRight()
	{
		curAsset.ColorLip = (curAsset.ColorLip + 1) % StylesUtil.EntityLipColors.Count;
		assetController.UpdateLipColor();
		ZoomCamera(zoomIn: true);
		LipColor.color = StylesUtil.EntityLipColors[curAsset.ColorLip];
		LipCount.text = (curAsset.ColorLip + StylesUtil.EntityLipColors.Count - 1) % StylesUtil.EntityLipColors.Count + 1 + "/" + StylesUtil.EntityLipColors.Count;
	}

	public void OnClassLeft()
	{
		SetClass((classIndex - 1) % classNames.Count);
		assetController.Load();
		ZoomCamera(zoomIn: false);
	}

	public void OnClassRight()
	{
		SetClass((classIndex + 1) % classNames.Count);
		assetController.Load();
		ZoomCamera(zoomIn: false);
	}

	public void SelectClass(int classIndex)
	{
		if (this.classIndex == classIndex)
		{
			DoClassAnimation();
			return;
		}
		SetClass(classIndex);
		assetController.Load();
		ZoomCamera(zoomIn: false);
	}

	public void MoveGlow(Transform pos)
	{
		Glow.transform.localPosition = new Vector3(pos.localPosition.x, -3f, 0f);
	}

	private void OnRandomNameClick(GameObject go)
	{
		if (fantasyNames.Count == 0)
		{
			SendRequestGetFantasyNames(populateInputField: true);
			return;
		}
		if (fantasyNames.Count < 10)
		{
			SendRequestGetFantasyNames(populateInputField: false);
		}
		txtCharname.value = fantasyNames.Dequeue();
	}

	private void SendRequestGetFantasyNames(bool populateInputField)
	{
		if (!isGettingFantasyNames)
		{
			StartCoroutine(WaitForRequestGetFantasyNames(populateInputField));
		}
	}

	private IEnumerator WaitForRequestGetFantasyNames(bool populateInputField)
	{
		isGettingFantasyNames = true;
		using (UnityWebRequest www = UnityWebRequest.Get(Main.WebServiceURL + "/Game/GetFantasyNames"))
		{
			string errorTitle = "Name Error";
			string friendlyMsg = "Failed to obtain a new character name.";
			string customContext = "URL: " + www.url;
			yield return www.SendWebRequest();
			customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
			if (www.isHttpError)
			{
				ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, null, customContext);
			}
			else if (www.isNetworkError)
			{
				ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, null, customContext);
			}
			else if (www.error != null)
			{
				ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, null, customContext);
			}
			else
			{
				try
				{
					string[] array = JsonConvert.DeserializeObject<string[]>(www.downloadHandler.text);
					foreach (string item in array)
					{
						fantasyNames.Enqueue(item);
					}
					if (populateInputField)
					{
						txtCharname.value = fantasyNames.Dequeue();
					}
				}
				catch (Exception ex)
				{
					errorTitle = "Loading Error";
					friendlyMsg = "Unable to process name list received from the server.";
					customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
					ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, null, customContext, ex);
				}
			}
		}
		isGettingFantasyNames = false;
	}

	private void OnRandomLookClick(GameObject go)
	{
		if (curAsset.gender == "M")
		{
			SetBeard((UnityEngine.Random.Range(0f, 3f) > 2f) ? UnityEngine.Random.Range(0, BeardStyles.Count - 1) : 0);
			SetStache((UnityEngine.Random.Range(0f, 3f) > 2f) ? UnityEngine.Random.Range(0, StacheStyles.Count - 1) : 0);
			SetBraid(0);
			curAsset.ColorLip = 0;
			LipColor.color = StylesUtil.EntityLipColors[curAsset.ColorLip];
		}
		else
		{
			SetBraid((UnityEngine.Random.Range(0f, 3f) > 2f) ? UnityEngine.Random.Range(0, BraidStyles.Count - 1) : 0);
			SetBeard(0);
			SetStache(0);
			curAsset.ColorLip = UnityEngine.Random.Range(0, StylesUtil.EntityLipColors.Count - 1);
			LipColor.color = StylesUtil.EntityLipColors[curAsset.ColorLip];
		}
		SetHair(UnityEngine.Random.Range(0, HairStyles.Count - 1));
		curAsset.ColorHair = UnityEngine.Random.Range(0, StylesUtil.EntityHairColors.Count - 1);
		HairColor.color = StylesUtil.EntityHairColors[curAsset.ColorHair];
		curAsset.ColorSkin = UnityEngine.Random.Range(0, StylesUtil.EntitySkinColors.Count - 1);
		SkinColor.color = StylesUtil.EntitySkinColors[curAsset.ColorSkin].B;
		curAsset.ColorEye = UnityEngine.Random.Range(0, StylesUtil.EntityEyeColors.Count - 1);
		EyeColor.color = StylesUtil.EntityEyeColors[curAsset.ColorEye];
		assetController.UpdateAll();
		ZoomCamera(zoomIn: true);
		HairCount.text = hairIndex + 1 + "/" + HairStyles.Count;
		BraidCount.text = braidIndex + 1 + "/" + BraidStyles.Count;
		BeardCount.text = beardIndex + 1 + "/" + BeardStyles.Count;
		StacheCount.text = stacheIndex + 1 + "/" + StacheStyles.Count;
		HairColorCount.text = (curAsset.ColorHair + StylesUtil.EntityHairColors.Count - 1) % StylesUtil.EntityHairColors.Count + 1 + "/" + StylesUtil.EntityHairColors.Count;
		SkinCount.text = (curAsset.ColorSkin + StylesUtil.EntitySkinColors.Count - 1) % StylesUtil.EntitySkinColors.Count + 1 + "/" + StylesUtil.EntitySkinColors.Count;
		EyeCount.text = (curAsset.ColorEye + StylesUtil.EntityEyeColors.Count - 1) % StylesUtil.EntityEyeColors.Count + 1 + "/" + StylesUtil.EntityEyeColors.Count;
		LipCount.text = (curAsset.ColorLip + StylesUtil.EntityLipColors.Count - 1) % StylesUtil.EntityLipColors.Count + 1 + "/" + StylesUtil.EntityLipColors.Count;
	}

	private void OnCreateClick(GameObject go)
	{
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharCreate_CreateClicked);
		txtCharname.value = txtCharname.value.TrimEnd();
		if (new ChatFilter().ProfanityCheck(txtCharname.value, shouldCleanSymbols: false).code > 0)
		{
			MessageBox.Show("Invalid Input", "Character name is offensive!");
			UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharCreate_ClientError_NameOffensive);
		}
		else if (txtCharname.value.Length < 3)
		{
			MessageBox.Show("Invalid Input", "Character name is too short!");
			UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharCreate_ClientError_NameLength);
		}
		else if (txtCharname.value.Length > 30)
		{
			MessageBox.Show("Invalid Input", "Character name is too long! (30 characters or less)");
			UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharCreate_ClientError_NameLength);
		}
		else
		{
			CharNew(txtCharname.value);
		}
	}

	private void CharNew(string name)
	{
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharCreate_CallWebApi);
		BusyDialog.Show("Connecting to server...");
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("uid", Session.Account.UserID);
		wWWForm.AddField("strToken", Session.Account.strToken);
		wWWForm.AddField("strCharName", name);
		wWWForm.AddField("strGender", curAsset.gender);
		wWWForm.AddField("intColorHair", curAsset.ColorHair);
		wWWForm.AddField("intColorSkin", curAsset.ColorSkin);
		wWWForm.AddField("intColorEye", curAsset.ColorEye);
		wWWForm.AddField("intColorLip", curAsset.ColorLip);
		wWWForm.AddField("iHair", curAsset.Hair);
		wWWForm.AddField("iStache", curAsset.Stache);
		wWWForm.AddField("iBeard", curAsset.Beard);
		wWWForm.AddField("iBraid", curAsset.Braid);
		wWWForm.AddField("iClass", classIndex);
		Debug.Log(Encoding.Default.GetString(wWWForm.data));
		StartCoroutine(WaitForRequestCharNew(wWWForm));
	}

	private void ZoomCamera(bool zoomIn)
	{
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharCreate_ZoomedIn);
		if (camRoutine != null)
		{
			StopCoroutine(camRoutine);
		}
		Vector3 moveToPosition;
		if (zoomIn)
		{
			ReturnToStanding();
			moveToPosition = endPosition;
		}
		else
		{
			moveToPosition = startPosition;
		}
		camRoutine = StartCoroutine(MoveCam(moveToPosition, 2f));
	}

	private IEnumerator MoveCam(Vector3 moveToPosition, float moveSpeed)
	{
		while (Camera.main != null && Vector3.Distance(Camera.main.transform.localPosition, moveToPosition) > 0.1f)
		{
			Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, moveToPosition, moveSpeed * Time.deltaTime);
			yield return null;
		}
	}

	private IEnumerator WaitForRequestCharNew(WWWForm form)
	{
		using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL + "/Game/CharCreate", form);
		string errorTitle = "Failed to Create Character";
		string friendlyMsg = "Unable to communicate with the server. Please try again or contact support at " + Main.SupportURL;
		string customContext = "URL: " + www.url;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		BusyDialog.Close();
		if (www.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, form, customContext);
			yield break;
		}
		if (www.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, form, customContext);
			yield break;
		}
		if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, form, customContext);
			yield break;
		}
		friendlyMsg = "Unable to process response from the server. Please try again or contact support at " + Main.SupportURL;
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharCreate_CreateOk);
		try
		{
			APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(www.downloadHandler.text);
			UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharCreate_DeserializeOk);
			if (aPIResponse.Account != null)
			{
				UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharCreate_AttemptLoadCharSelect);
				Session.Account = aPIResponse.Account;
				Session.AutoConnectServer = true;
				StateManager.Instance.LoadState("scene.charselect");
			}
			else if (aPIResponse.Error != null)
			{
				try
				{
					if (int.TryParse(aPIResponse.Error.Message.Split(' ').Last(), out var result))
					{
						switch (result)
						{
						case 2:
							UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharCreate_ServerError_CouldNotValidateUserAccount);
							MessageBox.Show("Error!", "Could not validate User account.");
							break;
						case 3:
							UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharCreate_ServerError_AlreadyHaveCharacterOnAccount);
							MessageBox.Show("Error!", "You already have a character on this account.");
							break;
						case 4:
							UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharCreate_ServerError_ContainsBadWord);
							MessageBox.Show("Error!", "Character name contains a bad word.");
							break;
						case 5:
						case 6:
							UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharCreate_ServerError_NameIsTaken);
							MessageBox.Show("Error!", "Character name is already taken.");
							break;
						default:
							UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharCreate_ServerError_Other);
							MessageBox.Show("Error!", "An error occurred while creating character.");
							break;
						}
					}
				}
				catch (Exception ex)
				{
					ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, null, customContext, ex);
				}
			}
			else
			{
				ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, "response.Error is null", null, customContext);
			}
		}
		catch (Exception ex2)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex2.Message, null, customContext, ex2);
		}
	}

	private void CharNewCallBack(string data)
	{
	}

	private char InputCharNameValidate(string text, int charIndex, char addedChar)
	{
		if (Regex.IsMatch(text + addedChar, "^([A-Za-z0-9]+[ _-]?)+$"))
		{
			return addedChar;
		}
		return '\0';
	}

	public void OnBackClick()
	{
		StateManager.Instance.LoadState("scene.login");
	}
}
