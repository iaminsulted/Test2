using System;
using System.Runtime.Serialization;
using UnityEngine;

public class TypeNameSerializationBinder : SerializationBinder
{
	public override Type BindToType(string assemblyName, string typeName)
	{
		try
		{
			return Type.GetType(typeName, throwOnError: true);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		return null;
	}
}
