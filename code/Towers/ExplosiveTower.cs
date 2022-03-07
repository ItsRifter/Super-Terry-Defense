using System;
using Sandbox;

public partial class ExplosiveTower : TowerBase
{
	public override string TowerName => "Explosive Tower";
	public override string TowerDesc => "A tower that fires explosive cannons";
	public override string TowerModel => "models/towers/explosive.vmdl";

	private int ExplosiveRange;
	public override int Cost => 40;
	public override int MaxTier => 4;
	public override int[] UpgradeCosts => new[] { 40, 55, 85, 125, 0 };
	public override string[] UpgradeDesc => new[] {
		"5+ DMG, 5+ Range, +0.35 FireRate, +12 Explosive Range",
		"5+ DMG, 5+ Range, +0.35 FireRate, +12 Explosive Range",
		"5+ DMG, 5+ Range, +0.35 FireRate, +12 Explosive Range",
		"5+ DMG, 5+ Range, +0.35 FireRate, +12 Explosive Range",
		"MAX LEVEL REACHED"
	};

	public override void Spawn()
	{
		base.Spawn();

		AttackRange = 75;
		AttackDamage = 15;
		AttackCooldown = 3.25f;
		ExplosiveRange = 48;
	}

	public override void AttackNPC( TDNPCBase npc )
	{
		base.AttackNPC( npc );

		foreach ( var ent in FindInSphere(npc.Position, ExplosiveRange ) )
		{
			if ( ent is TDNPCBase nearbyNPC )
			{
				DamageInfo dmgInfo = new DamageInfo();
				dmgInfo.Damage = AttackDamage;

				nearbyNPC.TakeDamage( dmgInfo );
			}	
		}
	}

	public override void OnUpgrade()
	{
		base.OnUpgrade();

		AttackRange += 5;
		AttackDamage += 5;
		AttackCooldown -= 0.35f;
		ExplosiveRange += 12;
	}

	public override void SimulateTower()
	{
		base.SimulateTower();

		if( td_tower_drawoverlay )
			DebugOverlay.Sphere( Position, AttackRange, Color.Green );
	}
}
