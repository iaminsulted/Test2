using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bone Adjustment Presets")]
public class BoneAdjustmentPresets : ScriptableObject
{
	public List<BoneAdjust> boneAdjustments = new List<BoneAdjust>();
}
