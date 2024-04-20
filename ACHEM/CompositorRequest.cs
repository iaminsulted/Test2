using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositorRequest : IEnumerator
{
	public List<CompositorTask> tasks = new List<CompositorTask>();

	public Texture tex;

	public bool isDone;

	public object Current => null;

	public event Action Complete;

	public CompositorRequest(Texture tex)
	{
		this.tex = tex;
	}

	public void OnComplete()
	{
		isDone = true;
		if (this.Complete != null)
		{
			this.Complete();
		}
	}

	public void Composite()
	{
		TextureCompositor.Composite(this);
	}

	public bool MoveNext()
	{
		return !isDone;
	}

	public void Reset()
	{
	}
}
