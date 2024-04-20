using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sheathing
{
	public enum Type
	{
		OneHanded,
		DualWield,
		Bow,
		Guns,
		None
	}

	private static Sheathing instance;

	public DateTime lastToggle;

	private static Dictionary<Type, SheathType> map;

	public static Sheathing Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new Sheathing();
				instance.Init();
			}
			return instance;
		}
	}

	public void Init()
	{
		SettingsManager.AutoSheathingUpdated += OnAutoSheathingUpdated;
	}

	private void OnAutoSheathingUpdated(bool value)
	{
		if (Entities.Instance.me != null)
		{
			SendSheathingRequest(SettingsManager.AutoSheatheWeapons);
		}
	}

	public void Toggle()
	{
		_ = lastToggle;
		if (!(lastToggle > DateTime.UtcNow))
		{
			ChatCommands.RunCommand("/sheathe");
			lastToggle = DateTime.UtcNow.AddSeconds(0.5);
		}
	}

	public void SendSheathingRequest(bool sheathe)
	{
		RequestSheathing requestSheathing = new RequestSheathing();
		requestSheathing.sheathe = sheathe;
		requestSheathing.autoResheathe = SettingsManager.AutoSheatheWeapons;
		AEC.getInstance().sendRequest(requestSheathing);
	}

	public void AddSheatheTypes(List<SheathType> sheathTypes)
	{
		try
		{
			map = sheathTypes.ToDictionary((SheathType x) => x.WeaponMountType);
			foreach (SheathType sheathType in sheathTypes)
			{
				EntityAnimation animation = new EntityAnimation
				{
					name = sheathType.SheatheAnimation,
					priority = -1,
					canMix = true,
					animationType = EntityAnimation.Type.SHEATHING
				};
				EntityAnimation animation2 = new EntityAnimation
				{
					name = sheathType.UnsheatheAnimation,
					priority = 10,
					canMix = true,
					animationType = EntityAnimation.Type.UNSHEATHING
				};
				EntityAnimations.AddAnimationToMap(animation);
				EntityAnimations.AddAnimationToMap(animation2);
			}
		}
		catch (Exception exception)
		{
			Debug.LogError("Could not add sheathing animations");
			Debug.LogException(exception);
			map = null;
		}
	}

	public void Reset(Player player)
	{
		PlayerAssetController playerAssetController = player.assetController as PlayerAssetController;
		if (!(playerAssetController == null))
		{
			playerAssetController.SetWeaponsMounted(SettingsManager.AutoSheatheWeapons);
		}
	}

	public void Sheathe(Player player, bool sheathe)
	{
		if (player == null || player.IsSheathed == sheathe)
		{
			return;
		}
		player.IsSheathed = sheathe;
		try
		{
			PlayerAssetController playerAssetController = player.assetController as PlayerAssetController;
			if (playerAssetController == null)
			{
				return;
			}
			SheathType sheathingType = GetSheathingType(playerAssetController.GetMountingType());
			if (sheathingType == null)
			{
				return;
			}
			if (sheathe)
			{
				if (!player.PlayAnimation(EntityAnimations.Get(sheathingType.SheatheAnimation)))
				{
					playerAssetController.SetWeaponsMounted(toSheathed: true);
				}
			}
			else if (!player.PlayAnimation(EntityAnimations.Get(sheathingType.UnsheatheAnimation)))
			{
				playerAssetController.SetWeaponsMounted(toSheathed: false);
			}
		}
		catch (Exception exception)
		{
			Debug.LogError("Could not sheathe weapons");
			Debug.LogException(exception);
		}
	}

	public SheathType GetSheathingType(Type weaponMountType)
	{
		if (map == null || weaponMountType == Type.None)
		{
			return null;
		}
		if (map.TryGetValue(weaponMountType, out var value))
		{
			return value;
		}
		return null;
	}
}
