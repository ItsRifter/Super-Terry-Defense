using System;
using System.Collections.Generic;
using Sandbox;
public partial class ElectricTower : TowerBase
{
	public override string TowerName => "Electric Tower";
	public override string TowerDesc => "An electrifying tower";
	public override string TowerModel => "models/towers/electric.vmdl";
	public override int Cost => 65;
	public override int MaxTier => 3;
	public override int[] UpgradeCosts => new[] { 65, 145, 225, 0 };

	private int ShockTargets;

	public override string[] UpgradeDesc => new[] { 
		"5+ DMG, 15+ Range, +0.75 FireRate, +1 Shock Targets",
		"5+ DMG, 15+ Range, +0.75 FireRate, +1 Shock Targets",
		"5+ DMG, 15+ Range, +0.75 FireRate, +1 Shock Targets",
		"MAX LEVEL REACHED"
	};

	public override void Spawn()
	{
		base.Spawn();

		AttackRange = 50;
		AttackDamage = 5;
		AttackCooldown = 7;
		ShockTargets = 1;
	}

	public override void AttackNPC( TDNPCBase npc )
	{
		base.AttackNPC( npc );

		int totalShocked = 0;
		var lastShocked = npc;
		List<TDNPCBase> totalShockedNPC = new List<TDNPCBase>();
		totalShockedNPC.Add( lastShocked );

		while ( totalShocked < ShockTargets )
		{
			foreach ( var ent in FindInSphere( lastShocked.Position, 92 ) )
			{
				if ( ent is TDNPCBase nearbyNPC )
				{
					if ( totalShockedNPC[totalShocked] != null && totalShockedNPC[totalShocked] == nearbyNPC )
						return;

					DamageInfo dmgInfo = new DamageInfo();
					dmgInfo.Damage = AttackDamage;

					lastShocked.TakeDamage( dmgInfo );
					lastShocked = nearbyNPC;

					totalShockedNPC.Add( lastShocked );
				}
			}
			totalShocked++;
		}
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
		AttackDamage += 5;
		AttackCooldown -= 0.75f;
		ShockTargets += 1;
	}
}
