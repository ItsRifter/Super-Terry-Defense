using System;
using Sandbox;

public partial class ExplosiveTower : TowerBase
{
	public override string TowerName => "Explosive Tower";
	public override string TowerDesc => "A tower that fires explosive cannons";
	public override string TowerModel => "models/towers/explosive.vmdl";
	public override int AttackDamage => 15;
	public override float AttackCooldown => 0.45f;
	public override int AttackRange => 75;
	public override int Cost => 50;
	public override int Level => 0;
	public override int MaxTier => 4;
	public override int[] UpgradeCosts => new[] { 15, 30, 55, 85 };

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
