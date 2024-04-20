using UnityEngine;

public abstract class State : MonoBehaviour
{
	public virtual void Awake()
	{
	}

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
		UIManager.Reset();
	}

	protected void BackToLogin()
	{
		StateManager.Instance.LoadState("scene.login");
	}
}
