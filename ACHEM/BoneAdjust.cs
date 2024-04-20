using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoneAdjust
{
	public string name;

	[Range(0f, 1f)]
	public float amount;

	public List<BoneFrame> boneFrames;
}
