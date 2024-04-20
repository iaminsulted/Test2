using System;
using UnityEngine;

[Serializable]
public class BoneFrame
{
	public string boneName;

	[HideInInspector]
	public Transform boneTransform;

	public bool usePos;

	public Vector3 minPos;

	public Vector3 maxPos;

	public bool useScale;

	public Vector3 minScale;

	public Vector3 maxScale;

	public bool useRot;

	public Vector3 minRot;

	public Vector3 maxRot;
}
