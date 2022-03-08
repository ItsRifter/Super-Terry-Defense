using System;
using Sandbox;

public partial class PistolTower : TowerBase
{
	public override string TowerName => "Pistol Tower";
	public override string TowerDesc => "A simple shooting tower";
	public override string TowerModel => "models/towers/pistol.vmdl";
	public override int Cost => 10;
	public override int MaxTier => 5;
	public override int[] UpgradeCosts => new[] { 15, 20, 30, 50, 100, 0 };

	public override string[] UpgradeDesc => new[] {
		"+1 DMG, +15 Range, +0.25 FireRate",
		"+1 DMG, +15 Range, +0.25 FireRate",
		"+1 DMG, +15 Range, +0.25 FireRate, Can see cloaked enemies",
		"+1 DMG, +15 Range, +0.25 FireRate",
		"+1 DMG, +15 Range, +0.25 FireRate",
		"MAX LEVEL REACHED"
	};

	public override void Spawn()
	{
		base.Spawn();

		AttackRange = 75;
		AttackDamage = 5;
		AttackCooldown = 4;
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

		AttackRange += 15;
		AttackDamage += 1;
		AttackCooldown -= 0.25f;

		if(CurTier == 3)
			CanSeeCloaked = true;
	}
}
