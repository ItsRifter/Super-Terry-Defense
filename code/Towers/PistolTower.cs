using System;
using Sandbox;

public partial class PistolTower : TowerBase
{
	public override string TowerName => "Pistol Tower";
	public override string TowerDesc => "A simple shooting tower";
	public override string TowerModel => "models/towers/pistol.vmdl";
	public override int AttackDamage => 2;
	public override float AttackCooldown => 3;
	public override int AttackRange => 75;
	public override int Cost => 10;
	public override int Level => 0;
	public override int MaxTier => 5;
	public override int[] UpgradeCosts => new[] { 5, 10, 25, 50, 100 };

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
