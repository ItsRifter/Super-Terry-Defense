using System;
using Sandbox;

public partial class SMGTower : TowerBase
{
	public override string TowerName => "SMG Tower";
	public override string TowerDesc => "An automatic shooting tower";
	public override string TowerModel => "models/towers/smg.vmdl";
	public override int AttackDamage => 4;
	public override float AttackCooldown => 1.25f;
	public override int AttackRange => 65;
	public override int Cost => 25;
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
