using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScrollviewPool<T> where T : class
{
	private class PoolEntry
	{
		public T data;

		public int index = -1;

		public bool isAvailable;

		public GameObject gameObject;

		public PoolEntry(GameObject templateGO, Transform parent)
		{
			gameObject = UnityEngine.Object.Instantiate(templateGO, parent);
			gameObject.transform.position = templateGO.transform.position;
			isAvailable = true;
		}
	}

	public IEnumerable<GameObject> PoolObjects;

	public IEnumerable<(T, GameObject, int)> PoolEntries;

	private int poolSize;

	private int listSize;

	private UIPooledScrollview uiPooledScrollview;

	private List<PoolEntry> pool;

	private Dictionary<int, T> datas;

	private IEnumerable<(int, T)> currentSelection;

	private Action<T, int, GameObject> onInitialize;

	private Action<T, GameObject> onRelease;

	public void Init(UIPooledScrollview uiPooledScrollview, Action<T, int, GameObject> onInitialize, Action<T, GameObject> onRelease, int poolSize)
	{
		this.poolSize = poolSize;
		this.uiPooledScrollview = uiPooledScrollview;
		this.onInitialize = onInitialize;
		this.onRelease = onRelease;
		pool = new List<PoolEntry>(poolSize);
		uiPooledScrollview.Init(OnGetNewSelection, poolSize);
		for (int i = 0; i < poolSize; i++)
		{
			PoolEntry item = new PoolEntry(uiPooledScrollview.TemplateGO, uiPooledScrollview.PoolRoot);
			pool.Add(item);
		}
		PoolObjects = pool.Select((PoolEntry x) => x.gameObject);
		PoolEntries = pool.Select((PoolEntry x) => (data: x.data, gameObject: x.gameObject, index: x.index));
	}

	public void SetDataCache(Dictionary<int, T> datas, int listSize, int itemOffset = 0)
	{
		ClearPool();
		bool flag = false;
		if (this.listSize == 0)
		{
			flag = true;
		}
		this.listSize = listSize;
		if (this.datas != null)
		{
			this.datas = datas;
			uiPooledScrollview.UpdateWidgetSize(listSize, itemOffset);
		}
		else
		{
			this.datas = datas;
			uiPooledScrollview.UpdateWidgetSize(listSize, itemOffset);
			uiPooledScrollview.StartRefreshOnUpdate();
		}
		uiPooledScrollview.scrollView.RestrictWithinBounds(instant: true);
		if (flag)
		{
			uiPooledScrollview.scrollView.ResetPosition();
		}
	}

	private void ClearPool()
	{
		foreach (PoolEntry item in pool)
		{
			item.isAvailable = true;
			item.gameObject?.SetActive(value: false);
		}
		currentSelection = null;
	}

	private GameObject GetAvailableGameObject(T data, int index)
	{
		PoolEntry poolEntry = pool.First((PoolEntry x) => x.isAvailable);
		poolEntry.data = data;
		poolEntry.isAvailable = false;
		poolEntry.index = index;
		return poolEntry.gameObject;
	}

	private PoolEntry GetPoolEntry(T data)
	{
		return pool.FirstOrDefault((PoolEntry x) => x.data == data);
	}

	private PoolEntry GetPoolEntry(int index)
	{
		return pool.FirstOrDefault((PoolEntry x) => x.index == index);
	}

	private void ReleasePoolEntry(PoolEntry entry)
	{
		entry.isAvailable = true;
		entry.gameObject.SetActive(value: false);
	}

	private void OnGetNewSelection(int lowerBound)
	{
		int num = lowerBound - listSize + poolSize;
		int num2 = ((num <= 0) ? poolSize : (poolSize - num));
		if (num2 < 0)
		{
			return;
		}
		IEnumerable<int> inner = Enumerable.Range(lowerBound, num2);
		IEnumerable<(int, T)> enumerable = from dEntry in datas
			join key in inner on dEntry.Key equals key
			select (key: key, Value: dEntry.Value);
		IEnumerable<(int, T)> enumerable2;
		if (currentSelection != null)
		{
			foreach (int item in currentSelection.Select(((int, T) x) => x.Item1).Except(enumerable.Select(((int, T) y) => y.Item1)))
			{
				PoolEntry poolEntry = GetPoolEntry(item);
				if (poolEntry != null)
				{
					onRelease?.Invoke(poolEntry.data, poolEntry.gameObject);
					ReleasePoolEntry(poolEntry);
				}
				else
				{
					Debug.LogWarning("Didn't release because the entry was null!");
				}
			}
			enumerable2 = enumerable.Except(currentSelection);
		}
		else
		{
			enumerable2 = enumerable;
		}
		try
		{
			foreach (var item2 in enumerable2)
			{
				GameObject availableGameObject = GetAvailableGameObject(item2.Item2, item2.Item1);
				uiPooledScrollview.MoveObjectToIndexLocation(availableGameObject, item2.Item1);
				onInitialize?.Invoke(item2.Item2, item2.Item1, availableGameObject);
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		currentSelection = enumerable;
	}
}
