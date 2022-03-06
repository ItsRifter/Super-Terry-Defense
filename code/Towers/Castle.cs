using System;
using Sandbox;

public partial class Castle : TowerBase
{
	public override string TowerName => "Base Tower";
	public override string TowerDesc => "Home base, you should defend this";
	public override string TowerModel => "models/towers/castle.vmdl";
	public override int AttackDamage => -1;
	public override float AttackCooldown => -1;
	public override int Cost => -1;
	public override int Level => -1;
	public override int MaxTier => 0;
	public override int[] UpgradeCosts => new[] { 0 };

	public int CastleHealth = 100;
	public override void Spawn()
	{
		base.Spawn();
		Health = CastleHealth;
	}

	public override void SimulateTower()
	{

	}

	public override void TakeDamage( DamageInfo info )
	{
		base.TakeDamage( info );
	}

	public override void OnKilled()
	{
		base.OnKilled();
		TDGame.Current.EndGame();
	}
}
