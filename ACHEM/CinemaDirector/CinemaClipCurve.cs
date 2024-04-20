using System;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector;

public abstract class CinemaClipCurve : TimelineAction
{
	[SerializeField]
	private List<MemberClipCurveData> curveData = new List<MemberClipCurveData>();

	public List<MemberClipCurveData> CurveData => curveData;

	protected virtual bool initializeClipCurves(MemberClipCurveData data, Component component)
	{
		return false;
	}

	public void AddClipCurveData(Component component, string name, bool isProperty, Type type)
	{
		MemberClipCurveData memberClipCurveData = new MemberClipCurveData();
		memberClipCurveData.Type = component.GetType().Name;
		memberClipCurveData.PropertyName = name;
		memberClipCurveData.IsProperty = isProperty;
		memberClipCurveData.PropertyType = UnityPropertyTypeInfo.GetMappedType(type);
		if (initializeClipCurves(memberClipCurveData, component))
		{
			curveData.Add(memberClipCurveData);
		}
		else
		{
			Debug.LogError("Could not initialize curve clip, invalid initial values.");
		}
	}

	protected object evaluate(MemberClipCurveData memberData, float time)
	{
		object result = null;
		switch (memberData.PropertyType)
		{
		case PropertyTypeInfo.Color:
		{
			Color color = default(Color);
			color.r = memberData.Curve1.Evaluate(time);
			color.g = memberData.Curve2.Evaluate(time);
			color.b = memberData.Curve3.Evaluate(time);
			color.a = memberData.Curve4.Evaluate(time);
			result = color;
			break;
		}
		case PropertyTypeInfo.Double:
		case PropertyTypeInfo.Float:
		case PropertyTypeInfo.Int:
		case PropertyTypeInfo.Long:
			result = memberData.Curve1.Evaluate(time);
			break;
		case PropertyTypeInfo.Quaternion:
		{
			Quaternion quaternion = default(Quaternion);
			quaternion.x = memberData.Curve1.Evaluate(time);
			quaternion.y = memberData.Curve2.Evaluate(time);
			quaternion.z = memberData.Curve3.Evaluate(time);
			quaternion.w = memberData.Curve4.Evaluate(time);
			result = quaternion;
			break;
		}
		case PropertyTypeInfo.Vector2:
		{
			Vector2 vector3 = default(Vector2);
			vector3.x = memberData.Curve1.Evaluate(time);
			vector3.y = memberData.Curve2.Evaluate(time);
			result = vector3;
			break;
		}
		case PropertyTypeInfo.Vector3:
		{
			Vector3 vector2 = default(Vector3);
			vector2.x = memberData.Curve1.Evaluate(time);
			vector2.y = memberData.Curve2.Evaluate(time);
			vector2.z = memberData.Curve3.Evaluate(time);
			result = vector2;
			break;
		}
		case PropertyTypeInfo.Vector4:
		{
			Vector4 vector = default(Vector4);
			vector.x = memberData.Curve1.Evaluate(time);
			vector.y = memberData.Curve2.Evaluate(time);
			vector.z = memberData.Curve3.Evaluate(time);
			vector.w = memberData.Curve4.Evaluate(time);
			result = vector;
			break;
		}
		}
		return result;
	}

	public void TranslateCurves(float amount)
	{
		base.Firetime += amount;
		for (int i = 0; i < curveData.Count; i++)
		{
			int curveCount = UnityPropertyTypeInfo.GetCurveCount(curveData[i].PropertyType);
			for (int j = 0; j < curveCount; j++)
			{
				AnimationCurve curve = curveData[i].GetCurve(j);
				if (amount > 0f)
				{
					for (int num = curve.length - 1; num >= 0; num--)
					{
						Keyframe keyframe = curve.keys[num];
						Keyframe key = new Keyframe(keyframe.time + amount, keyframe.value, keyframe.inTangent, keyframe.outTangent);
						curve.MoveKey(num, key);
					}
				}
				else
				{
					for (int k = 0; k < curve.length; k++)
					{
						Keyframe keyframe2 = curve.keys[k];
						Keyframe key2 = new Keyframe(keyframe2.time + amount, keyframe2.value, keyframe2.inTangent, keyframe2.outTangent);
						curve.MoveKey(k, key2);
					}
				}
			}
		}
	}
}
