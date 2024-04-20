using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPvpMatchReward : MonoBehaviour
{
	private const float FloatAndAppearTime = 0.65f;

	private const float Offset = 100f;

	private const float ScaleTime = 0.25f;

	private const float ScaleMax = 1.5f;

	private const float ScaleMin = 1f;

	private readonly Vector3 InitialOffset = new Vector3(0f, 100f, 0f);

	private readonly Dictionary<PvpMatchRewardType, string> ToolTipPrefabPaths = new Dictionary<PvpMatchRewardType, string>
	{
		{
			PvpMatchRewardType.Gold,
			"UIPvpMatchToolTip_Gold"
		},
		{
			PvpMatchRewardType.MarkOfGlory,
			"UIPvpMatchToolTip_MarkOfGlory"
		},
		{
			PvpMatchRewardType.XP,
			"UIPvpMatchToolTip_XP"
		},
		{
			PvpMatchRewardType.ClassXP,
			"UIPvpMatchToolTip_ClassXP"
		},
		{
			PvpMatchRewardType.Glory,
			"UIPvpMatchToolTip_Glory"
		}
	};

	[SerializeField]
	private PvpMatchRewardType rewardType;

	[SerializeField]
	private UILabel label;

	[SerializeField]
	private UISprite sprite;

	private UIPvpMatchToolTip toolTip;

	private BoxCollider2D trigger;

	private GameObject toolTipPrefab;

	private Vector3 end;

	private Vector3 start;

	private bool resourcesLoaded;

	public void Init(int reward)
	{
		if (!resourcesLoaded)
		{
			LoadResources();
		}
		label.text = ArtixString.AddCommas(reward);
	}

	public void OnTooltip(bool show)
	{
		if (show)
		{
			toolTip.Show();
		}
		else
		{
			toolTip.Hide();
		}
	}

	public void Show(float delay)
	{
		StartCoroutine(FloatAndAppear(delay));
	}

	public void Hide()
	{
		StopAllCoroutines();
	}

	private void LoadResources()
	{
		toolTipPrefab = Object.Instantiate(Resources.Load("UIElements/Pvp/ToolTips/" + ToolTipPrefabPaths[rewardType]), base.transform) as GameObject;
		toolTip = toolTipPrefab.GetComponent<UIPvpMatchToolTip>();
		trigger = GetComponent<BoxCollider2D>();
		trigger.enabled = false;
		end = base.transform.localPosition;
		start = end - InitialOffset;
		resourcesLoaded = true;
	}

	private IEnumerator FloatAndAppear(float delay)
	{
		label.alpha = 0f;
		sprite.alpha = 0f;
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		base.transform.localPosition = start;
		yield return new WaitForSeconds(delay);
		while (true)
		{
			float num = Time.deltaTime / 0.65f;
			label.alpha += num;
			sprite.alpha += num;
			base.transform.localPosition += new Vector3(0f, num * 100f, 0f);
			if (label.alpha >= 1f)
			{
				break;
			}
			yield return null;
		}
		label.alpha = 1f;
		sprite.alpha = 1f;
		base.transform.localPosition = end;
		StartCoroutine(ScaleUp());
	}

	private IEnumerator ScaleUp()
	{
		while (true)
		{
			float num = Time.deltaTime / 0.25f;
			base.transform.localScale += new Vector3(num, num, num);
			if (base.transform.localScale.x >= 1.5f)
			{
				break;
			}
			yield return null;
		}
		base.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
		StartCoroutine(ScaleDown());
	}

	private IEnumerator ScaleDown()
	{
		while (true)
		{
			float num = Time.deltaTime / 0.25f;
			base.transform.localScale -= new Vector3(num, num, num);
			if (base.transform.localScale.x <= 1f)
			{
				break;
			}
			yield return null;
		}
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		trigger.enabled = true;
	}
}
