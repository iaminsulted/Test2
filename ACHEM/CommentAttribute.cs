using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class CommentAttribute : PropertyAttribute
{
	public readonly string text;

	public CommentAttribute(string text)
	{
		this.text = text;
	}
}
