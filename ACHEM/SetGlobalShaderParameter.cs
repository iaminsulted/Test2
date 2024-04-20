using System;
using System.Collections.Generic;
using UnityEngine;

public class SetGlobalShaderParameter : MonoBehaviour
{
	[Serializable]
	public class GlobalShaderParameter
	{
		public enum Type
		{
			Color,
			Float,
			Int,
			Texture,
			Vector
		}

		public bool player;

		public bool rotation;

		public string propertyName;

		public Type globalType;

		public Color globalColor;

		public float globalFloat;

		public int globalInt;

		public Texture2D globalTexture;

		public Transform globalVector;
	}

	public List<GlobalShaderParameter> parameters = new List<GlobalShaderParameter>();

	private void Start()
	{
		foreach (GlobalShaderParameter parameter in parameters)
		{
			if (parameter.globalType == GlobalShaderParameter.Type.Color)
			{
				Shader.SetGlobalColor(parameter.propertyName, parameter.globalColor);
			}
			if (parameter.globalType == GlobalShaderParameter.Type.Float)
			{
				Shader.SetGlobalFloat(parameter.propertyName, parameter.globalFloat);
			}
			if (parameter.globalType == GlobalShaderParameter.Type.Int)
			{
				Shader.SetGlobalInt(parameter.propertyName, parameter.globalInt);
			}
			if (parameter.globalType == GlobalShaderParameter.Type.Texture)
			{
				Shader.SetGlobalTexture(parameter.propertyName, parameter.globalTexture);
			}
			if (parameter.globalType == GlobalShaderParameter.Type.Vector)
			{
				if (parameter.player && Entities.Instance != null && Entities.Instance.me != null)
				{
					parameter.globalVector = Entities.Instance.me.wrapper.transform;
				}
				if (parameter.globalVector != null)
				{
					Shader.SetGlobalVector(parameter.propertyName, parameter.globalVector.position);
				}
			}
		}
	}

	private void Update()
	{
		foreach (GlobalShaderParameter parameter in parameters)
		{
			if (parameter.globalType == GlobalShaderParameter.Type.Vector && parameter.globalVector != null)
			{
				if (!parameter.rotation)
				{
					Shader.SetGlobalVector(parameter.propertyName, parameter.globalVector.position);
				}
				else
				{
					Shader.SetGlobalVector(parameter.propertyName, parameter.globalVector.forward);
				}
			}
		}
	}
}
