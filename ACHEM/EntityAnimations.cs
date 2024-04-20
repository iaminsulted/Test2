using System.Collections.Generic;

public static class EntityAnimations
{
	public const int Hit_Animation_Priority = 2;

	public const int Auto_Attack_Animation_Priority = 10;

	public const int Spell_Animation_Priority = 10;

	public const int Stun_Animation_Priority = 10;

	public const int Special_Animation_Priority = 500;

	public const int Death_Animation_Priority = 999;

	private static Dictionary<string, EntityAnimation> map;

	public const string Move_Run = "Run";

	public const string Move_Walk = "Walk";

	public const string Move_Jump_Default = "Jump";

	public const string Move_Jump_Flip = "Flip";

	public const string Move_Land = "Landing";

	public const string Pc_Melee_JumpAttack = "JumpAttack";

	public const string Pc_Cast1h = "Cast1HLoop";

	public const string Pc_Cast_1 = "Cast1";

	public const string Pc_Cast_2 = "Cast2";

	public const string Pc_Cast_3 = "Cast3";

	public const string Melee_1 = "Attack1";

	public const string Melee_2 = "Attack2";

	public const string Melee_3 = "Attack3";

	public const string React_Hit = "Hit";

	public const string State_Idle = "Idle";

	public const string State_Combat = "Fight";

	public const string State_Death = "Death";

	public const string State_Stun = "Stun";

	public const string Special_Daitengu = "ExitStrategy";

	public const string LayerSwapTo1 = "LayerSwapTo1";

	public const string LayerSwapTo2 = "LayerSwapTo2";

	static EntityAnimations()
	{
		map = new Dictionary<string, EntityAnimation>();
		AddAnimationToMap(new EntityAnimation
		{
			name = "Walk",
			priority = 0
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "Run",
			priority = 0
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "JumpAttack",
			priority = 10
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "Cast1HLoop",
			priority = 10
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "Cast1",
			priority = 10
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "Cast2",
			priority = 10
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "Cast3",
			priority = 10
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "Cast1H",
			crossfadeSpeed = 0.01f
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "2H_CastForward",
			crossfadeSpeed = 0.01f
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "2H_ChargeLong",
			crossfadeSpeed = 0.01f
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "2H_CastUp",
			crossfadeSpeed = 0.01f
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "Attack1",
			priority = 10
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "Attack2",
			priority = 10
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "Attack3",
			priority = 10
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "Hit",
			priority = 2
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "Idle",
			priority = 0
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "Fight",
			priority = 0
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "Stun",
			priority = 10
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "Death",
			priority = 999
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "Jump",
			priority = 0,
			crossfadeSpeed = 0.05f
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "Flip",
			priority = 0,
			crossfadeSpeed = 0.05f
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "Landing",
			priority = 0
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "ExitStrategy",
			priority = 500
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "LayerSwapTo1",
			priority = 500
		});
		AddAnimationToMap(new EntityAnimation
		{
			name = "LayerSwapTo2",
			priority = 500
		});
	}

	public static void AddAnimationToMap(EntityAnimation animation)
	{
		if (!map.ContainsKey(animation.name))
		{
			map.Add(animation.name, animation);
		}
	}

	public static EntityAnimation Get(string name)
	{
		if (map.ContainsKey(name))
		{
			return map[name];
		}
		EntityAnimation obj = new EntityAnimation
		{
			name = name,
			priority = 0
		};
		AddAnimationToMap(obj);
		return obj;
	}
}
