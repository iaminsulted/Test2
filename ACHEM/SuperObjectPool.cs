using System;
using System.Collections.Generic;
using UnityEngine;

public class SuperObjectPool<T>
{
	private Func<T> generate;

	private Action<T> disposeCallback;

	private Action<T> borrowCallback;

	private Action<T> returnCallback;

	private Queue<T> pool = new Queue<T>();

	private bool init;

	public int Size => NumBorrowed + NumReturned;

	public int NumBorrowed { get; private set; }

	public int NumReturned => pool.Count;

	public int MinSize { get; private set; }

	public int MaxSize { get; private set; }

	public int MaxQueueSize { get; private set; }

	public SuperObjectPool()
		: this(0, int.MaxValue)
	{
	}

	public SuperObjectPool(int size)
		: this(size, size)
	{
	}

	public SuperObjectPool(int minSize, int maxSize)
		: this(minSize, maxSize, maxSize)
	{
	}

	public SuperObjectPool(int minSize, int maxSize, int maxQueueSize)
	{
		MinSize = minSize;
		MaxSize = maxSize;
		MaxQueueSize = maxQueueSize;
		borrowCallback = null;
		returnCallback = null;
		try
		{
			generate = Activator.CreateInstance<T>;
		}
		catch
		{
			generate = () => default(T);
		}
	}

	public T Borrow()
	{
		if (!init)
		{
			init = true;
			for (int i = 0; i < MinSize; i++)
			{
				pool.Enqueue(generate());
			}
		}
		NumBorrowed++;
		T val = default(T);
		if (pool.Count == 0)
		{
			if (Size <= MaxSize)
			{
				val = generate();
			}
			else
			{
				NumBorrowed--;
			}
		}
		else
		{
			val = pool.Dequeue();
		}
		if (borrowCallback != null && val != null)
		{
			if (CheckForUnityNull(val))
			{
				return Borrow();
			}
			borrowCallback(val);
		}
		return val;
	}

	public void Return(T item)
	{
		NumBorrowed--;
		if (pool.Count == MaxQueueSize - 1)
		{
			disposeCallback?.Invoke(item);
			return;
		}
		returnCallback?.Invoke(item);
		pool.Enqueue(item);
	}

	private bool CheckForUnityNull(T item)
	{
		if (item is MonoBehaviour monoBehaviour && monoBehaviour == null)
		{
			item = default(T);
			return true;
		}
		return false;
	}

	public SuperObjectPool<T> SetOnBorrow(Action<T> onBorrow)
	{
		borrowCallback = onBorrow;
		return this;
	}

	public SuperObjectPool<T> SetOnReturn(Action<T> onReturn)
	{
		returnCallback = onReturn;
		return this;
	}

	public SuperObjectPool<T> SetGenerate(Func<T> generate)
	{
		this.generate = generate;
		return this;
	}

	public SuperObjectPool<T> SetDispose(Action<T> dispose)
	{
		disposeCallback = dispose;
		return this;
	}

	public void Dispose()
	{
		pool.Clear();
		returnCallback = null;
		borrowCallback = null;
		generate = null;
	}
}
