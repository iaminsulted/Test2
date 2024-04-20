using System;
using StatCurves;
using UnityEngine;

public class UIFishing : MonoBehaviour
{
	private const float GoalFillBase = 0.15f;

	private const float MiniGameCountdown = 30f;

	private const float MinArrowRotation = 15f;

	private const float CountdownFill = 0.2f;

	private const float MaxRotationThreshold = 179.9f;

	private const float MinRotationThreshold = 0.1f;

	private const string TutorialBitName = "ic2";

	private const int TutorialBitIndex = 47;

	public UIButton buttonFish;

	public UILabel labelNodePower;

	public UISprite spriteGoal;

	public UISprite spriteFish;

	public UISprite spriteTime;

	public GameObject root;

	public Transform goalPivot;

	public Transform markerPivot;

	public Transform goalNearPivot;

	public Transform goalFarPivot;

	private Quaternion currentRotation;

	private Quaternion targetRotation;

	private Color normal;

	private Color highlight;

	private bool miniGameActive;

	private float countdown;

	private float elapsed;

	private float speed;

	private float time;

	private float goalAngleNear;

	private float goalAngleFar;

	private float markerAngle;

	private float snapMin;

	private float snapMax;

	private float snapRate;

	private float speedMin;

	private float speedMax;

	private float speedModifier;

	private float timeMin;

	private float timeMax;

	private ResourceMachine machine;

	private RarityType itemRarity;

	private RarityType rodRarity;

	public void Awake()
	{
		root.SetActive(value: false);
		UIEventListener uIEventListener = UIEventListener.Get(buttonFish.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClick));
	}

	public void Update()
	{
		if (miniGameActive)
		{
			countdown -= Time.deltaTime;
			if (countdown <= 0f)
			{
				OnClick(null);
				return;
			}
			UpdateTimeFill();
			UpdateMarkerRotation();
			UpdateMarkerColor();
		}
	}

	public void OnDestroy()
	{
		UIEventListener uIEventListener = UIEventListener.Get(buttonFish.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClick));
	}

	public void Init(ResourceMachine machine, RarityType rodRarity, RarityType itemRarity)
	{
		miniGameActive = true;
		root.SetActive(value: true);
		countdown = 30f;
		this.machine = machine;
		this.itemRarity = itemRarity;
		this.rodRarity = rodRarity;
		labelNodePower.text = machine.power.ToString();
		DetermineSpriteColor();
		DetermineGoalFill();
		DetermineGoalPosts();
		DetermineMarkerLocation();
		DetermineSnapRange();
		DetermineSpeedRange();
		DetermineTimeRange();
		GenerateNextRotation();
	}

	public void Close()
	{
		miniGameActive = false;
		elapsed = 0f;
		countdown = 30f;
		root.SetActive(value: false);
		machine = null;
		UITutorialPopup.Clear();
	}

	private void OnClick(GameObject go)
	{
		if (IsInGoal())
		{
			machine.CollectResource();
			if (!HasFinishedTutorial() && IsTutorialComplete())
			{
				CompleteTutorial();
			}
		}
		else
		{
			machine.DropResource();
		}
		Close();
	}

	private void GenerateNextRotation()
	{
		CalculateNextAngle();
		CalculateNextSpeed();
		CalculateNextTime();
	}

	private void CalculateNextAngle()
	{
		float num = UnityEngine.Random.Range(0.1f, 179.9f);
		float num2 = markerAngle - num;
		if (Mathf.Approximately(num2, 0f))
		{
			num += ((UnityEngine.Random.Range(0f, 1f) > 0.5f) ? 15f : (-15f));
		}
		if (num2 > -15f && num2 > 0f)
		{
			num -= 15f;
		}
		else if (num2 < 15f && num2 < 0f)
		{
			num += 15f;
		}
		if (num < 0.1f)
		{
			num = 0.1f;
		}
		else if (num > 179.9f)
		{
			num = 179.9f;
		}
		if (num > markerAngle)
		{
			markerPivot.localScale = new Vector3(-1f, 1f, 1f);
		}
		else if (num < markerAngle)
		{
			markerPivot.localScale = new Vector3(1f, 1f, 1f);
		}
		targetRotation = Quaternion.Euler(0f, 0f, num);
	}

	private void CalculateNextSpeed()
	{
		speed = UnityEngine.Random.Range(speedMin, speedMax) * speedModifier;
		if (snapRate <= UnityEngine.Random.Range(0f, 1f))
		{
			speed *= UnityEngine.Random.Range(snapMin, snapMax);
		}
	}

	private void CalculateNextTime()
	{
		time = UnityEngine.Random.Range(timeMin, timeMax);
	}

	private void CompleteTutorial()
	{
		Game.Instance.SendBitFlagUpdateRequest("ic2", 47, value: true);
		Chat.AddMessage(InterfaceColors.Chat.Yellow.ToBBCode() + "You received the Fish Fry title![-]");
		FancyNotification.ShowText("Trade Skill Title Unlocked!", "You received the Fish Fry title!");
	}

	private void DetermineGoalFill()
	{
		float num = 1f;
		float num2 = 1f;
		switch (itemRarity)
		{
		case RarityType.Common:
			num = 1f;
			break;
		case RarityType.Uncommon:
			num = 0.97f;
			break;
		case RarityType.Rare:
			num = 0.92f;
			break;
		case RarityType.Epic:
			num = 0.86f;
			break;
		case RarityType.Legendary:
			num = 0.75f;
			break;
		}
		switch (rodRarity)
		{
		case RarityType.Common:
			num2 = 1f;
			break;
		case RarityType.Uncommon:
			num2 = 1.02f;
			break;
		case RarityType.Rare:
			num2 = 1.05f;
			break;
		case RarityType.Epic:
			num2 = 1.09f;
			break;
		case RarityType.Legendary:
			num2 = 1.12f;
			break;
		}
		spriteGoal.fillAmount = 0.15f * num * num2;
	}

	private void DetermineGoalPosts()
	{
		goalPivot.localRotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0.1f, 179.9f - spriteGoal.fillAmount * 360f));
		goalAngleNear = goalPivot.localRotation.eulerAngles.z;
		goalAngleFar = goalAngleNear + spriteGoal.fillAmount * 360f;
		goalNearPivot.localRotation = Quaternion.Euler(0f, 0f, goalAngleNear);
		goalFarPivot.localRotation = Quaternion.Euler(0f, 0f, goalAngleFar);
	}

	private void DetermineMarkerLocation()
	{
		markerPivot.localRotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0.1f, 179.9f));
		currentRotation = markerPivot.localRotation;
		markerAngle = markerPivot.localRotation.eulerAngles.z;
		if (IsInGoal())
		{
			DetermineMarkerLocation();
		}
	}

	private void DetermineSnapRange()
	{
		switch (itemRarity)
		{
		case RarityType.Common:
			snapMin = 1.02f;
			snapMax = 1.06f;
			snapRate = 0.019f;
			break;
		case RarityType.Uncommon:
			snapMin = 1.05f;
			snapMax = 1.11f;
			snapRate = 0.0031f;
			break;
		case RarityType.Rare:
			snapMin = 1.08f;
			snapMax = 1.15f;
			snapRate = 0.052f;
			break;
		case RarityType.Epic:
			snapMin = 1.13f;
			snapMax = 1.19f;
			snapRate = 0.065f;
			break;
		case RarityType.Legendary:
			snapMin = 1.18f;
			snapMax = 1.26f;
			snapRate = 0.086f;
			break;
		}
	}

	private void DetermineSpeedRange()
	{
		switch (itemRarity)
		{
		case RarityType.Common:
			speedMin = 2f;
			speedMax = 3.2f;
			break;
		case RarityType.Uncommon:
			speedMin = 2.4f;
			speedMax = 4f;
			break;
		case RarityType.Rare:
			speedMin = 3.1f;
			speedMax = 5.4f;
			break;
		case RarityType.Epic:
			speedMin = 3.9f;
			speedMax = 6.1f;
			break;
		case RarityType.Legendary:
			speedMin = 4.8f;
			speedMax = 8f;
			break;
		}
		switch (rodRarity)
		{
		case RarityType.Common:
			speedModifier = 1f;
			break;
		case RarityType.Uncommon:
			speedModifier = 0.94f;
			break;
		case RarityType.Rare:
			speedModifier = 0.88f;
			break;
		case RarityType.Epic:
			speedModifier = 0.79f;
			break;
		case RarityType.Legendary:
			speedModifier = 0.69f;
			break;
		}
	}

	private void DetermineSpriteColor()
	{
		switch (itemRarity)
		{
		case RarityType.Common:
			normal = new Color(0.52f, 0.52f, 0.52f, 1f);
			highlight = new Color(0.92f, 0.92f, 0.92f, 1f);
			break;
		case RarityType.Uncommon:
			normal = new Color(0.26f, 0.52f, 0.11f, 1f);
			highlight = new Color(0.41f, 0.75f, 0.19f, 1f);
			break;
		case RarityType.Rare:
			normal = new Color(0f, 0.23f, 0.7f, 1f);
			highlight = new Color(0f, 0.42f, 1f, 1f);
			break;
		case RarityType.Epic:
			normal = new Color(0.39f, 0f, 0.79f, 1f);
			highlight = new Color(0.51f, 0f, 1f, 1f);
			break;
		case RarityType.Legendary:
			normal = new Color(0.62f, 0.36f, 0f, 1f);
			highlight = new Color(0.88f, 0.55f, 0f, 1f);
			break;
		}
		spriteGoal.color = highlight;
		spriteTime.color = Color.green;
	}

	private void DetermineTimeRange()
	{
		switch (itemRarity)
		{
		case RarityType.Common:
			timeMin = 4f;
			timeMax = 8f;
			break;
		case RarityType.Uncommon:
			timeMin = 3.5f;
			timeMax = 7.4f;
			break;
		case RarityType.Rare:
			timeMin = 3.1f;
			timeMax = 6.8f;
			break;
		case RarityType.Epic:
			timeMin = 2.7f;
			timeMax = 6.1f;
			break;
		case RarityType.Legendary:
			timeMin = 2.2f;
			timeMax = 4.8f;
			break;
		}
	}

	private void UpdateMarkerColor()
	{
		markerAngle = markerPivot.localRotation.eulerAngles.z;
		if (IsInGoal())
		{
			spriteFish.color = highlight;
			if (!HasFinishedTutorial())
			{
				if (!UITutorialPopup.IsPopupShown(Tutorial.FishingCatch))
				{
					speed /= 2f;
					UITutorialPopup.Clear();
					UITutorialPopup.Show(Tutorial.FishingCatch);
				}
				if (countdown <= 20f)
				{
					Pause();
				}
			}
		}
		else
		{
			spriteFish.color = normal;
			if (!HasFinishedTutorial() && !UITutorialPopup.IsPopupShown(Tutorial.FishingWait))
			{
				speed *= 2f;
				UITutorialPopup.Clear();
				UITutorialPopup.Show(Tutorial.FishingWait);
			}
		}
	}

	private void UpdateMarkerRotation()
	{
		elapsed += Time.deltaTime * speed;
		markerPivot.localRotation = Quaternion.Slerp(currentRotation, targetRotation, elapsed / time);
		if (elapsed >= time)
		{
			elapsed = 0f;
			currentRotation = targetRotation;
			GenerateNextRotation();
		}
	}

	private void UpdateTimeFill()
	{
		spriteTime.fillAmount = countdown / 30f * 0.2f;
		if ((double)spriteTime.fillAmount >= 0.1)
		{
			spriteTime.color += new Color(Time.deltaTime / 15f, 0f, 0f);
			if (spriteTime.color.r >= 1f)
			{
				spriteTime.color = new Color(1f, 1f, 0f);
			}
		}
		else
		{
			spriteTime.color -= new Color(0f, Time.deltaTime / 15f, 0f);
			if (spriteTime.color.g <= 0f)
			{
				spriteTime.color = new Color(1f, 0f, 0f);
			}
		}
	}

	private bool IsInGoal()
	{
		if (markerAngle >= goalAngleNear)
		{
			return markerAngle <= goalAngleFar;
		}
		return false;
	}

	private bool IsTutorialComplete()
	{
		return Session.MyPlayerData.tradeSkillXP[TradeSkillType.Fishing] >= 35;
	}

	private bool HasFinishedTutorial()
	{
		return Session.MyPlayerData.CheckBitFlag("ic2", 47);
	}

	private void Pause()
	{
		miniGameActive = false;
	}
}
