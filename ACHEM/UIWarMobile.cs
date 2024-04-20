using System;

public class UIWarMobile : UIWar
{
	public UIPortrait TargetPortrait;

	public UIPortrait MobPortrait;

	public UIPortrait NPCPortrait;

	public UIPortrait BossPortrait;

	public UIPortraitResourceNode TargetResourceNode;

	protected override void Awake()
	{
		base.Awake();
		UIPortrait targetPortrait = TargetPortrait;
		targetPortrait.VisibleUpdated = (Action<bool>)Delegate.Combine(targetPortrait.VisibleUpdated, new Action<bool>(PortraitVisibleUpdated));
		UIPortrait mobPortrait = MobPortrait;
		mobPortrait.VisibleUpdated = (Action<bool>)Delegate.Combine(mobPortrait.VisibleUpdated, new Action<bool>(PortraitVisibleUpdated));
		UIPortrait bossPortrait = BossPortrait;
		bossPortrait.VisibleUpdated = (Action<bool>)Delegate.Combine(bossPortrait.VisibleUpdated, new Action<bool>(PortraitVisibleUpdated));
		UIPortrait nPCPortrait = NPCPortrait;
		nPCPortrait.VisibleUpdated = (Action<bool>)Delegate.Combine(nPCPortrait.VisibleUpdated, new Action<bool>(PortraitVisibleUpdated));
		UIPortraitResourceNode targetResourceNode = TargetResourceNode;
		targetResourceNode.VisibleUpdated = (Action<bool>)Delegate.Combine(targetResourceNode.VisibleUpdated, new Action<bool>(PortraitVisibleUpdated));
	}

	private void PortraitVisibleUpdated(bool visible)
	{
		base.gameObject.SetActive(!visible);
	}

	protected override void OnDestroy()
	{
		UIPortrait targetPortrait = TargetPortrait;
		targetPortrait.VisibleUpdated = (Action<bool>)Delegate.Remove(targetPortrait.VisibleUpdated, new Action<bool>(PortraitVisibleUpdated));
		UIPortrait mobPortrait = MobPortrait;
		mobPortrait.VisibleUpdated = (Action<bool>)Delegate.Remove(mobPortrait.VisibleUpdated, new Action<bool>(PortraitVisibleUpdated));
		UIPortrait bossPortrait = BossPortrait;
		bossPortrait.VisibleUpdated = (Action<bool>)Delegate.Remove(bossPortrait.VisibleUpdated, new Action<bool>(PortraitVisibleUpdated));
		UIPortrait nPCPortrait = NPCPortrait;
		nPCPortrait.VisibleUpdated = (Action<bool>)Delegate.Remove(nPCPortrait.VisibleUpdated, new Action<bool>(PortraitVisibleUpdated));
		UIPortraitResourceNode targetResourceNode = TargetResourceNode;
		targetResourceNode.VisibleUpdated = (Action<bool>)Delegate.Remove(targetResourceNode.VisibleUpdated, new Action<bool>(PortraitVisibleUpdated));
		base.OnDestroy();
	}
}
