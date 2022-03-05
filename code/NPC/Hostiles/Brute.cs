using Sandbox;
using System;
using System.Linq;

public partial class Brute : TDNPCBase
{
	public override string NPCName => "Brute";
	public override int BaseHealth => 12;
	public override float BaseSpeed => 30;
	public override string BaseModel => "models/citizen/citizen.vmdl";
	public override int minCash => 7;
	public override int maxCash => 18;
	public override float NPCScale => 0.35f;

	public override void TakeDamage( DamageInfo info )
	{
		base.TakeDamage( info );
	}

	public override void Spawn()
	{
		base.Spawn();		

		RenderColor = Color.Gray;
	}

	public override void OnKilled()
	{
		base.OnKilled();
	}
}
