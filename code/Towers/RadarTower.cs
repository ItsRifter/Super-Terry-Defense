using System;
using System.Collections.Generic;
using Sandbox;

public partial class RadarTower : TowerBase
{
	public override string TowerName => "Radar Tower";
	public override string TowerDesc => "A radar tower that enhances towers and reveal cloaked hostiles";
	public override string TowerModel => "models/towers/radar.vmdl";
	public override int Cost => 50;
	public override int MaxTier => 3;
	public override int[] UpgradeCosts => new[] { 55, 90, 120, 0 };

	public List<TowerBase> OldTowers;
	public override string[] UpgradeDesc => new[] { 
		"Nearby towers get 5+ DMG, +25 Range",
		"Nearby towers get 10+ DMG, +25 Range",
		"Nearby towers get 15+ DMG, +25 Range",
		"MAX LEVEL REACHED"
	};

	public override void Spawn()
	{
		base.Spawn();

		AttackRange = 50;
		AttackDamage = 0;
		AttackCooldown = 0;

		OldTowers = new List<TowerBase>();
	}

	public override void AttackNPC( TDNPCBase npc )
	{
		
	}

	public override void SellTower( TDPlayer seller )
	{
		foreach ( var tower in OldTowers )
		{
			tower.AttackDamage -= 5 * (CurTier + 1);
			tower.RadarEnhanced = false;
			tower.CanSeeCloaked = false;
		}
	
		base.SellTower( seller );
	}

	public override void SimulateTower()
	{
		base.SimulateTower();

		foreach ( var ent in FindInSphere( Position, AttackRange ) )
		{
			if ( ent is TowerBase tower && !tower.RadarEnhanced && !(tower == this || tower is RadarTower) )
			{
				tower.AttackDamage += 5 * (CurTier + 1);
				tower.RadarEnhanced = true;
				tower.CanSeeCloaked = true;

				OldTowers.Add( tower );
			}
		}

		if ( td_tower_drawoverlay )
			DebugOverlay.Sphere( Position, AttackRange, Color.Green );
	}

	public override void OnUpgrade()
	{
		base.OnUpgrade();

		AttackRange += 25;

		foreach ( var tower in OldTowers )
		{
			tower.AttackDamage -= 5 * CurTier;
			tower.RadarEnhanced = false;
			tower.CanSeeCloaked = false;
		}

		OldTowers.Clear();
	}
}
