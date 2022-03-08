using System;
using Sandbox;

public partial class FrostTower : TowerBase
{
	public override string TowerName => "Frost Tower";
	public override string TowerDesc => "A tower that slows targets";
	public override string TowerModel => "models/towers/frost.vmdl";

	private int FreezeRange;
	public override int Cost => 45;
	public override int MaxTier => 4;
	public override int[] UpgradeCosts => new[] { 40, 55, 85, 125, 0 };
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
		AttackCooldown = 6.50f;
		FreezeRange = 64;
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
		FreezeRange += 18;
	}

	public override void SimulateTower()
	{
		base.SimulateTower();

		if( td_tower_drawoverlay )
			DebugOverlay.Sphere( Position, AttackRange, Color.Green );
	}
}
