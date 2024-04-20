using System;
using System.Collections.Generic;
using UnityEngine;

public class UIMultiButton : MonoBehaviour
{
	public List<Texture2D> buttonOptions = new List<Texture2D>();

	public UIButton btnNext;

	public UITexture iconTex;

	public GameObject dotTemplate;

	public List<GameObject> dotList;

	public Action onClick;

	private Transform dotParent;

	public int CurrrentOption { get; private set; }

	private void Start()
	{
	}

	public void Init(int currentOption)
	{
		CurrrentOption = currentOption;
		UIEventListener uIEventListener = UIEventListener.Get(btnNext.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClick));
		if (dotTemplate != null)
		{
			dotParent = dotTemplate.transform.parent;
			for (int i = 0; i < buttonOptions.Count - 1; i++)
			{
				UnityEngine.Object.Instantiate(dotTemplate, dotParent);
			}
		}
		dotTemplate = null;
		refresh();
	}

	private void OnClick(GameObject go)
	{
		int currrentOption = CurrrentOption + 1;
		CurrrentOption = currrentOption;
		if (CurrrentOption >= buttonOptions.Count)
		{
			CurrrentOption = 0;
		}
		refresh();
		onClick?.Invoke();
	}

	private void refresh()
	{
		iconTex.mainTexture = buttonOptions[CurrrentOption];
		int count = buttonOptions.Count;
		for (int i = 0; i < count; i++)
		{
			UITexture component = dotParent.GetChild(i).GetComponent<UITexture>();
			if (i == CurrrentOption)
			{
				component.color = Color.white;
			}
			else
			{
				component.color = Color.gray;
			}
		}
	}

	private void Update()
	{
	}
}
