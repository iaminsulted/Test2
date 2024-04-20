using System;
using System.Collections.Generic;
using UnityEngine;

namespace AQ3D.DialogueSystem;

[Serializable]
public class PointsOfInterest
{
	public List<Transform> pointsOfInterest;

	public Transform Point(int index)
	{
		int index2 = index % pointsOfInterest.Count;
		return pointsOfInterest[index2];
	}
}
