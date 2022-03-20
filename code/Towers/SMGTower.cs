using System;
using Sandbox;

public partial class SMGTower : TowerBase
{
	public override string TowerName => "SMG Tower";
	public override string TowerDesc => "An automatic shooting tower";
	public override string TowerModel => "models/towers/smg.vmdl";
	public override string AttackSound => "smg_shoot";
	public override int Cost => 25;
	public override int MaxTier => 5;
	public override int[] UpgradeCosts => new[] { 35, 55, 85, 100, 140, 0 };

	public override string[] UpgradeDesc => new[] { 
		"+1 DMG, +10 Range, +0.25 FireRate",
		"+1 DMG, +10 Range, +0.25 FireRate",
		"+1 DMG, +10 Range, +0.25 FireRate",
		"+1 DMG, +10 Range, +0.25 FireRate",
		"+1 DMG, +10 Range, +0.25 FireRate",
		"MAX LEVEL REACHED"
	};

	public override void Spawn()
	{
		base.Spawn();

		AttackRange = 65;
		AttackDamage = 3;
		AttackCooldown = 2.5f;
	}

	public override void OnUpgrade()
	{
		base.OnUpgrade();

		AttackRange += 10;
		AttackDamage += 1;
		AttackCooldown -= 0.25f;
	}

	public override void SimulateTower()
	{
		base.SimulateTower();

		if( td_tower_drawoverlay )
			DebugOverlay.Sphere( Position, AttackRange, Color.Green );
	}

	public override void TakeDamage( DamageInfo info )
	{
		base.TakeDamage( info );
	}
}
