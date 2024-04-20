using System.Collections.Generic;
using UnityEngine;

public class BoneCustomizer : MonoBehaviour
{
	public bool update;

	public bool apply;

	public List<BoneAdjustmentPresets> boneAdjustmentPresets = new List<BoneAdjustmentPresets>();

	[HideInInspector]
	public bool refreshValues = true;

	[HideInInspector]
	public List<BonePair> bonePairs = new List<BonePair>();

	public Transform rootBone;

	public List<Transform> preserveScales = new List<Transform>();

	private Dictionary<Transform, Bone> boneDefaults = new Dictionary<Transform, Bone>();

	private Dictionary<string, Transform> boneDictionary = new Dictionary<string, Transform>();

	private Vector3 TotalRootAdjustment = Vector3.zero;

	private Dictionary<Transform, Vector3> boneScale_Total = new Dictionary<Transform, Vector3>();

	private Dictionary<Transform, Vector3> bonePosition_Total = new Dictionary<Transform, Vector3>();

	private Dictionary<Transform, Vector3> boneRotation_Total = new Dictionary<Transform, Vector3>();

	private Animator anim;

	private RuntimeAnimatorController controller;

	private Dictionary<BoneAdjust, float> boneadjustWeightDictionary = new Dictionary<BoneAdjust, float>();

	public void GetBonePairs()
	{
		bonePairs.Clear();
		Transform[] componentsInChildren = rootBone.GetComponentsInChildren<Transform>();
		foreach (Transform transform in componentsInChildren)
		{
			bonePairs.Add(new BonePair
			{
				boneTransform = transform,
				bone = new Bone
				{
					pos = transform.localPosition,
					rot = transform.localEulerAngles,
					scale = transform.localScale
				}
			});
		}
	}

	public void RestoreDefaults()
	{
		anim = rootBone.GetComponentInParent<Animator>();
		controller = anim.runtimeAnimatorController;
		anim.runtimeAnimatorController = null;
		TotalRootAdjustment = Vector3.zero;
		foreach (BonePair bonePair in bonePairs)
		{
			if ((bool)bonePair.boneTransform)
			{
				bonePair.boneTransform.localPosition = bonePair.bone.pos;
				bonePair.boneTransform.localEulerAngles = bonePair.bone.rot;
				bonePair.boneTransform.localScale = bonePair.bone.scale;
			}
		}
		anim.runtimeAnimatorController = controller;
	}

	private void Awake()
	{
		anim = rootBone.GetComponentInParent<Animator>();
		controller = anim.runtimeAnimatorController;
		if (bonePairs == null || bonePairs.Count == 0)
		{
			if ((bool)rootBone)
			{
				if ((bool)rootBone.GetComponentInParent<Animator>())
				{
					anim.runtimeAnimatorController = null;
					GetBonePairs();
					anim.runtimeAnimatorController = controller;
				}
			}
			else
			{
				Debug.LogError("Bonepairs are not assigned and Rootbone is unavailable");
				Object.Destroy(this);
			}
		}
		foreach (BonePair bonePair in bonePairs)
		{
			if (!boneDefaults.ContainsKey(bonePair.boneTransform))
			{
				boneDefaults.Add(bonePair.boneTransform, new Bone
				{
					pos = bonePair.boneTransform.localPosition,
					rot = bonePair.boneTransform.localEulerAngles,
					scale = bonePair.boneTransform.localScale
				});
			}
			if (bonePair.boneTransform.name != null && !boneDictionary.ContainsKey(bonePair.boneTransform.name) && !boneDictionary.ContainsValue(bonePair.boneTransform))
			{
				boneDictionary.Add(bonePair.boneTransform.name, bonePair.boneTransform);
			}
		}
		for (int i = 0; i < boneAdjustmentPresets.Count; i++)
		{
			int index = 0;
			for (; i < boneAdjustmentPresets.Count; i++)
			{
				if (!boneadjustWeightDictionary.ContainsKey(boneAdjustmentPresets[i].boneAdjustments[index]))
				{
					boneadjustWeightDictionary.Add(boneAdjustmentPresets[i].boneAdjustments[index], boneAdjustmentPresets[i].boneAdjustments[index].amount);
				}
			}
		}
		boneScale_Total = new Dictionary<Transform, Vector3>();
		bonePosition_Total = new Dictionary<Transform, Vector3>();
		boneRotation_Total = new Dictionary<Transform, Vector3>();
	}

	private float GetBoneAdjustmentWeight(BoneAdjust boneAdjust)
	{
		if (!boneadjustWeightDictionary.ContainsKey(boneAdjust))
		{
			float amount = boneAdjust.amount;
			boneadjustWeightDictionary.Add(boneAdjust, amount);
		}
		return boneadjustWeightDictionary[boneAdjust];
	}

	public void ValueUpdate(float value, BoneAdjust boneAdjust)
	{
		refreshValues = true;
		boneadjustWeightDictionary[boneAdjust] = value;
	}

	private void LateUpdate()
	{
		if (!update)
		{
			if ((bool)rootBone && TotalRootAdjustment != Vector3.zero)
			{
				rootBone.localPosition = boneDefaults[rootBone].pos + TotalRootAdjustment;
			}
		}
		else
		{
			if (boneAdjustmentPresets == null || boneAdjustmentPresets.Count == 0)
			{
				return;
			}
			if (boneDictionary == null || boneDictionary.Count == 0)
			{
				update = false;
				return;
			}
			if (apply)
			{
				anim.runtimeAnimatorController = null;
			}
			if (refreshValues)
			{
				boneScale_Total.Clear();
				bonePosition_Total.Clear();
				boneRotation_Total.Clear();
				TotalRootAdjustment = Vector3.zero;
				foreach (BoneAdjustmentPresets boneAdjustmentPreset in boneAdjustmentPresets)
				{
					foreach (BoneAdjust boneAdjustment in boneAdjustmentPreset.boneAdjustments)
					{
						foreach (BoneFrame boneFrame in boneAdjustment.boneFrames)
						{
							if (string.IsNullOrEmpty(boneFrame.boneName) || !boneDictionary.ContainsKey(boneFrame.boneName))
							{
								continue;
							}
							if (boneFrame.useScale)
							{
								if (boneScale_Total.ContainsKey(boneDictionary[boneFrame.boneName]))
								{
									boneScale_Total[boneDictionary[boneFrame.boneName]] += Vector3.Lerp(boneFrame.minScale, boneFrame.maxScale, GetBoneAdjustmentWeight(boneAdjustment));
								}
								else
								{
									boneScale_Total.Add(boneDictionary[boneFrame.boneName], Vector3.Lerp(boneFrame.minScale, boneFrame.maxScale, GetBoneAdjustmentWeight(boneAdjustment)));
								}
							}
							if (boneFrame.usePos)
							{
								if (bonePosition_Total.ContainsKey(boneDictionary[boneFrame.boneName]))
								{
									bonePosition_Total[boneDictionary[boneFrame.boneName]] += Vector3.Lerp(boneFrame.minPos, boneFrame.maxPos, GetBoneAdjustmentWeight(boneAdjustment));
								}
								else
								{
									bonePosition_Total.Add(boneDictionary[boneFrame.boneName], Vector3.Lerp(boneFrame.minPos, boneFrame.maxPos, GetBoneAdjustmentWeight(boneAdjustment)));
								}
								if (boneDictionary[boneFrame.boneName] == rootBone)
								{
									TotalRootAdjustment += Vector3.Lerp(boneFrame.minPos, boneFrame.maxPos, GetBoneAdjustmentWeight(boneAdjustment));
								}
							}
							if (boneFrame.useRot)
							{
								if (boneRotation_Total.ContainsKey(boneDictionary[boneFrame.boneName]))
								{
									boneRotation_Total[boneDictionary[boneFrame.boneName]] += Vector3.Lerp(boneFrame.minRot, boneFrame.maxRot, GetBoneAdjustmentWeight(boneAdjustment));
								}
								else
								{
									boneRotation_Total.Add(boneDictionary[boneFrame.boneName], Vector3.Lerp(boneFrame.minRot, boneFrame.maxRot, GetBoneAdjustmentWeight(boneAdjustment)));
								}
							}
						}
					}
				}
				refreshValues = false;
			}
			foreach (KeyValuePair<Transform, Vector3> item in boneScale_Total)
			{
				item.Key.localScale = boneDefaults[item.Key].scale + item.Value;
			}
			foreach (KeyValuePair<Transform, Vector3> item2 in bonePosition_Total)
			{
				item2.Key.localPosition = boneDefaults[item2.Key].pos + item2.Value;
			}
			foreach (KeyValuePair<Transform, Vector3> item3 in boneRotation_Total)
			{
				if (boneDefaults[item3.Key].HasRotationAnims)
				{
					Quaternion quaternion = Quaternion.Euler(item3.Value);
					item3.Key.localRotation *= quaternion;
				}
				else
				{
					Vector3 localEulerAngles = boneDefaults[item3.Key].rot - item3.Value;
					item3.Key.localEulerAngles = localEulerAngles;
				}
			}
			if (preserveScales != null && preserveScales.Count > 0)
			{
				foreach (Transform preserveScale in preserveScales)
				{
					Transform parent = preserveScale.parent;
					GameObject obj = new GameObject(preserveScale.name + " Dummy");
					if (boneScale_Total.ContainsKey(preserveScale))
					{
						preserveScale.localScale = boneDefaults[preserveScale].scale + boneScale_Total[preserveScale];
					}
					else
					{
						preserveScale.localScale = boneDefaults[preserveScale].scale;
					}
					preserveScale.SetParent(parent);
					Object.Destroy(obj);
				}
			}
			if (apply)
			{
				anim.runtimeAnimatorController = controller;
				update = false;
				apply = false;
			}
		}
	}
}
