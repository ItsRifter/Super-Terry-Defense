using System;
using Sandbox;

public partial class ExplosiveTower : TowerBase
{
	public override string TowerName => "Explosive Tower";
	public override string TowerDesc => "A tower that fires explosive cannons";
	public override string TowerModel => "models/towers/explosive.vmdl";

/*	
	public override int AttackDamage => 15;
	public override float AttackCooldown => 0.45f;
	public override int AttackRange => 75;
*/
	public override int Cost => 50;
	public override int MaxTier => 4;
	public override int[] UpgradeCosts => new[] { 15, 30, 55, 85, 0 };
	public override string[] UpgradeDesc => new[] {
		"5+ DMG, 5+ Range, +0.1 FireRate",
		"5+ DMG, 5+ Range, +0.1 FireRate",
		"5+ DMG, 5+ Range, +0.1 FireRate",
		"5+ DMG, 5+ Range, +0.1 FireRate",
		"MAX LEVEL REACHED"
	};

	public override void Spawn()
	{
		base.Spawn();

		AttackRange = 75;
		AttackDamage = 15;
		AttackCooldown = 0.45f;
	}

	public override void OnUpgrade()
	{
		base.OnUpgrade();

		AttackRange += 5;
		AttackDamage += 5;
		AttackCooldown -= 0.1f;
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
