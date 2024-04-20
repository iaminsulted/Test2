using UnityEngine;

public abstract class UICustomActionButton : UIActionButton
{
	public CombatSpellSlot SlotNumber;

	public UISprite EmptyIcon;

	public UISprite SwapGlow;

	public UISprite SwapIcon;

	private bool isSwapActive;

	public Animation anim;

	protected abstract void OpenSelectMenu();

	protected override void Start()
	{
		base.Start();
		SwapGlow.gameObject.SetActive(value: false);
		SwapIcon.gameObject.SetActive(value: false);
	}

	public override void OnClick()
	{
		if (SlotNumber == CombatSpellSlot.Passive)
		{
			Debug.LogWarning("UIActionButton SlotNumber needs a value");
		}
		else if (!TrySelectMenu())
		{
			base.OnClick();
		}
	}

	public void ChangeSwapState(bool isSwapActive)
	{
		if (IsLocked())
		{
			return;
		}
		this.isSwapActive = isSwapActive;
		SwapIcon.gameObject.SetActive(isSwapActive);
		SwapGlow.gameObject.SetActive(isSwapActive);
		if (isSwapActive)
		{
			UITweener[] components = SwapGlow.gameObject.GetComponents<UITweener>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].ResetToBeginning();
			}
		}
	}

	private bool TrySelectMenu()
	{
		if (IsLocked())
		{
			return false;
		}
		if (!isSwapActive && UICamera.currentTouchID != -2 && IsAssigned)
		{
			return false;
		}
		OpenSelectMenu();
		return true;
	}

	protected override void UpdateLock()
	{
		base.UpdateLock();
		if (IsLocked())
		{
			ShowEmpty();
			EmptyIcon.gameObject.SetActive(value: false);
		}
	}

	protected override void ShowEmpty()
	{
		base.ShowEmpty();
		base.spellT = null;
		EmptyIcon.gameObject.SetActive(value: true);
	}
}
