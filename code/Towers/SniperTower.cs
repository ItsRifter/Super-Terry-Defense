using System;
using Sandbox;

public partial class SniperTower : TowerBase
{
	public override string TowerName => "Sniper Tower";
	public override string TowerDesc => "A long ranged tower";
	public override string TowerModel => "models/towers/sniper.vmdl";
	public override int Cost => 75;
	public override int MaxTier => 4;
	public override int[] UpgradeCosts => new[] { 85, 115, 145, 195, 0 };

	public override string[] UpgradeDesc => new[] { 
		"+5 DMG, +0.5 FireRate", 
		"+5 DMG, +0.5 FireRate, Can see cloaked enemies",
		"+5 DMG, +0.5 FireRate",
		"+5 DMG, +0.5 FireRate",
		"MAX LEVEL REACHED"
	};

	public override void Spawn()
	{
		base.Spawn();

		AttackRange = 175;
		AttackDamage = 7;
		AttackCooldown = 5;
	}

	public override void AttackNPC( TDNPCBase npc )
	{
		base.AttackNPC( npc );
	}

	public override void SimulateTower()
	{
		base.SimulateTower();

		if( td_tower_drawoverlay )
			DebugOverlay.Sphere( Position, AttackRange, Color.Green );
	}

	public override void OnUpgrade()
	{
		base.OnUpgrade();

		AttackDamage += 5;
		AttackCooldown -= 0.5f;

		if(CurTier == 2)
			CanSeeCloaked = true;
	}
}
