using System;
using Sandbox;

public partial class FrostTower : TowerBase
{
	public override string TowerName => "Frost Tower";
	public override string TowerDesc => "A tower that slows targets";
	public override string TowerModel => "models/towers/frost.vmdl";
	public override string AttackSound => "frost_shoot";
	public override AttackMethod MethodOfAttack => AttackMethod.Frost;
	public override int Cost => 45;
	public override int MaxTier => 4;
	public override int[] UpgradeCosts => new[] { 65, 85, 125, 165, 0 };


	public override string[] UpgradeDesc => new[] {
		"+5 Range, +0.25 FireRate",
		"+5 Range, +0.25 FireRate",
		"+5 Range, +0.25 FireRate",
		"+5 Range, +0.25 FireRate,",
		"MAX LEVEL REACHED"
	};

	public override void Spawn()
	{
		base.Spawn();

		AttackRange = 75;
		AttackDamage = 5;
		AttackCooldown = 6.0f;
	}

	public override void AttackNPC( TDNPCBase npc )
	{
		npc.Freeze();
		base.AttackNPC( npc );
	}

	public override void OnUpgrade()
	{
		base.OnUpgrade();

		AttackRange += 5;
		AttackCooldown -= 0.25f;
	}

	public override void SimulateTower()
	{
		base.SimulateTower();

		if( td_tower_drawoverlay )
			DebugOverlay.Sphere( Position, AttackRange, Color.Green );
	}
}
