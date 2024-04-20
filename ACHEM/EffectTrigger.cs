using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectTrigger : MonoBehaviour
{
	[Serializable]
	public struct EffectInfo
	{
		public GameObject effect;

		public float disableDelay;

		public bool containsLoops;

		public int effectType;
	}

	public List<GameObject> effects = new List<GameObject>();

	[SerializeField]
	private List<EffectInfo> effectInfo = new List<EffectInfo>();

	private List<float> disableTimes = new List<float>();

	private List<bool> pauseStatus = new List<bool>();

	private void TryDisableEffectLater(int index)
	{
		if (Time.time > disableTimes[index])
		{
			StartCoroutine(DisableEffectLater(index));
		}
		disableTimes[index] = Time.time + effectInfo[index].disableDelay;
	}

	private IEnumerator DisableEffectLater(int index)
	{
		float lastTime = Time.time;
		yield return new WaitForEndOfFrame();
		while (Time.time < disableTimes[index])
		{
			if (pauseStatus[index])
			{
				disableTimes[index] += Time.time - lastTime;
			}
			lastTime = Time.time;
			yield return new WaitForFixedUpdate();
		}
		effectInfo[index].effect.SetActive(value: false);
	}

	private void DisableEffect(int index)
	{
		if (effectInfo.Count > index && effectInfo[index].effect.gameObject.activeSelf)
		{
			ParticleSystem component = effectInfo[index].effect.GetComponent<ParticleSystem>();
			if (component.particleCount <= 0)
			{
				component.gameObject.SetActive(value: false);
			}
		}
	}

	public void DisableObjectAfterTimeInit(GameObject go, float delay)
	{
		Debug.LogWarning("EffectTrigger.DisableObjectAfterTimeInit(): Let Korin know if you see this so he can keep the function in the script since it's clearly needed.");
		StartCoroutine(DisableObjectAfterTime(go, delay));
	}

	private IEnumerator DisableObjectAfterTime(GameObject go, float delay)
	{
		yield return new WaitForSeconds(delay);
		go.SetActive(value: false);
	}

	public void PlayEffect(int index)
	{
		if (effectInfo.Count <= index || effectInfo[index].effect == null || effectInfo[index].effectType <= 0)
		{
			return;
		}
		if (!effectInfo[index].effect.activeSelf)
		{
			effectInfo[index].effect.SetActive(value: true);
		}
		if (effectInfo[index].effectType == 1)
		{
			ParticleSystem component = effectInfo[index].effect.GetComponent<ParticleSystem>();
			if (component != null)
			{
				component.Play(withChildren: true);
				if (!effectInfo[index].containsLoops)
				{
					TryDisableEffectLater(index);
				}
			}
		}
		else if (effectInfo[index].effectType == 2)
		{
			LineRenderer component2 = effectInfo[index].effect.GetComponent<LineRenderer>();
			if (component2 != null)
			{
				component2.enabled = true;
			}
		}
		else if (effectInfo[index].effectType == 3)
		{
			TrailRenderer component3 = effectInfo[index].effect.GetComponent<TrailRenderer>();
			if (component3 != null)
			{
				component3.emitting = true;
			}
		}
	}

	public void PauseEffect(int index)
	{
		TogglePause(index, toggler: true);
		pauseStatus[index] = true;
	}

	public void UnpauseEffect(int index)
	{
		TogglePause(index, toggler: false);
		pauseStatus[index] = false;
	}

	private void TogglePause(int index, bool toggler)
	{
		if (effectInfo.Count <= index || !effectInfo[index].effect.activeSelf)
		{
			return;
		}
		ParticleSystem component = effectInfo[index].effect.GetComponent<ParticleSystem>();
		if (component != null)
		{
			if (toggler)
			{
				component.Pause(withChildren: true);
			}
			else
			{
				component.Play(withChildren: true);
			}
		}
	}

	public void StopEffect(int index)
	{
		UnpauseEffect(index);
		if (effectInfo.Count <= index)
		{
			return;
		}
		if (effectInfo[index].effectType == 1)
		{
			ParticleSystem component = effectInfo[index].effect.GetComponent<ParticleSystem>();
			if (component != null)
			{
				component.Stop(withChildren: true);
				TryDisableEffectLater(index);
			}
		}
		else if (effectInfo[index].effectType == 2)
		{
			LineRenderer component2 = effectInfo[index].effect.GetComponent<LineRenderer>();
			if (component2 != null)
			{
				component2.enabled = false;
			}
		}
		else if (effectInfo[index].effectType == 3)
		{
			TrailRenderer component3 = effectInfo[index].effect.GetComponent<TrailRenderer>();
			if (component3 != null)
			{
				component3.emitting = false;
				TryDisableEffectLater(index);
			}
		}
	}

	public void EnableObject(int index)
	{
		if (effectInfo.Count > 0 && effectInfo[index].effect != null)
		{
			effectInfo[index].effect.SetActive(value: true);
		}
	}

	public void DisableObject(int index)
	{
		if (effectInfo.Count > 0 && effectInfo[index].effect != null)
		{
			effectInfo[index].effect.SetActive(value: false);
		}
	}

	public void SwapParentsInMob(string stringValueOfObjectIndexesBeingSwapped)
	{
		if (stringValueOfObjectIndexesBeingSwapped.Length < 3)
		{
			Debug.LogWarning("String of " + stringValueOfObjectIndexesBeingSwapped + " doesn't look right for parsing in SwapParentsInMob()");
			return;
		}
		string[] array = stringValueOfObjectIndexesBeingSwapped.Replace(" ", "").Split(',');
		int result;
		if (array.Length != 2)
		{
			Debug.LogWarning("Effect Trigger is trying to Swap Parents In Mob and received " + array.Length + " indexes when it requires 2. Separate your two ints with a single comma.");
		}
		else if (int.TryParse(array[0], out result))
		{
			if (int.TryParse(array[1], out var result2))
			{
				effectInfo[result].effect.transform.SetParent(effectInfo[result2].effect.transform, worldPositionStays: false);
			}
			else
			{
				Debug.LogWarning("Second value of " + stringValueOfObjectIndexesBeingSwapped + " failed to parse to int. Confirm int was entered for SwapParentsInMob. Example: 4,1");
			}
		}
		else
		{
			Debug.LogWarning("First value of " + stringValueOfObjectIndexesBeingSwapped + " failed to parse to int. Confirm int was entered for SwapParentsInMob. Example: 4,1");
		}
	}

	public void AttachEffect(int index)
	{
		if (effectInfo.Count > index)
		{
			Spellfx component = effectInfo[index].effect.GetComponent<Spellfx>();
			EntityAssetData component2 = GetComponent<EntityAssetData>();
			Transform transform = base.transform;
			if (component.spot == Spellfx.AttachSpot.Cast)
			{
				transform = component2.CastSpot;
			}
			else if (component.spot == Spellfx.AttachSpot.Head)
			{
				transform = component2.HeadSpot;
			}
			else if (component.spot == Spellfx.AttachSpot.Hit)
			{
				transform = component2.HitSpot;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(effectInfo[index].effect, transform.position, transform.rotation);
			if (component.isSticky)
			{
				gameObject.transform.parent = transform;
			}
		}
	}

	public GameObject GetEffect(int index)
	{
		if (effects.Count >= 1 && index < effects.Count)
		{
			return effects[index];
		}
		if (effectInfo.Count > 1 && index < effectInfo.Count)
		{
			return effectInfo[index].effect;
		}
		return null;
	}

	private void ProcessLifetimes()
	{
		List<float> list = new List<float>(effectInfo.Count);
		List<float> list2 = new List<float>(effectInfo.Count);
		if (effectInfo.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < effectInfo.Count; i++)
		{
			EffectInfo value = default(EffectInfo);
			value.containsLoops = false;
			float num = 0f;
			if (effectInfo[i].effect.GetComponent<ParticleSystem>() != null)
			{
				if (effectInfo[i].effect.transform.childCount > 0)
				{
					if (effectInfo[i].effect.GetComponent<ParticleSystem>() != null)
					{
						ParticleSystem[] componentsInChildren = effectInfo[i].effect.GetComponentsInChildren<ParticleSystem>();
						foreach (ParticleSystem particleSystem in componentsInChildren)
						{
							if (particleSystem.main.loop)
							{
								value.containsLoops = true;
							}
							list.Add(particleSystem.main.duration + particleSystem.main.startDelay.constantMax);
							list2.Add(particleSystem.main.startLifetime.constantMax);
						}
						num += list.Max();
						num += list2.Max();
					}
				}
				else
				{
					ParticleSystem component = effectInfo[i].effect.GetComponent<ParticleSystem>();
					num = component.main.duration + component.main.startLifetime.constantMax;
				}
				value.effectType = 1;
				value.disableDelay = num;
			}
			else if (effectInfo[i].effect.GetComponent<TrailRenderer>() != null)
			{
				value.disableDelay = effectInfo[i].effect.GetComponent<TrailRenderer>().time;
				value.effectType = 3;
			}
			else if (effectInfo[i].effect.GetComponent<LineRenderer>() != null)
			{
				value.disableDelay = 0f;
				value.effectType = 2;
			}
			else
			{
				num = 0f;
				value.effectType = 0;
			}
			value.effect = effectInfo[i].effect;
			effectInfo[i] = value;
			if ((effectInfo[i].effectType == 1 && !effectInfo[i].effect.GetComponent<ParticleSystem>().isPlaying) || (effectInfo[i].effectType == 2 && !effectInfo[i].effect.GetComponent<LineRenderer>().enabled) || (effectInfo[i].effectType == 3 && !effectInfo[i].effect.GetComponent<TrailRenderer>().emitting))
			{
				DisableEffect(i);
			}
		}
		for (int k = 0; k < effectInfo.Count; k++)
		{
			disableTimes.Add(0f);
			pauseStatus.Add(item: false);
		}
	}

	private void ConvertLegacyEffects()
	{
		if (effects.Count > 0)
		{
			for (int i = 0; i < effects.Count; i++)
			{
				if (effects[i].gameObject == null)
				{
					EffectInfo item = default(EffectInfo);
					item.effect = new GameObject("MissingEffect" + i);
					effectInfo.Add(item);
				}
				else
				{
					EffectInfo item2 = default(EffectInfo);
					item2.effect = effects[i];
					effectInfo.Add(item2);
				}
			}
			effects.Clear();
		}
		ProcessLifetimes();
	}

	private void Start()
	{
		ConvertLegacyEffects();
	}

	private IEnumerator TestSequence()
	{
		yield return new WaitForSeconds(1f);
		Debug.Log("3");
		yield return new WaitForSeconds(1f);
		Debug.Log("2");
		yield return new WaitForSeconds(1f);
		Debug.Log("1");
		yield return new WaitForSeconds(1f);
		Debug.Log("PlayEffect");
		PlayEffect(0);
		yield return new WaitForSeconds(6f);
		Debug.Log("StopEffect");
		StopEffect(0);
		yield return new WaitForSeconds(3f);
		Debug.Log("Interrupting the stopeffect with another play call.");
		PlayEffect(0);
		yield return new WaitForSeconds(6f);
		Debug.Log("Stopping again");
		StopEffect(0);
	}
}
