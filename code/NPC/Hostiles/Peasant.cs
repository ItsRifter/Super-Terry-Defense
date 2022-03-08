using Sandbox;
using System;
using System.Linq;

public partial class Peasant : TDNPCBase
{
	public override string NPCName => "Peasant";
	public override int BaseHealth => 10;
	public override float BaseSpeed => 15;
	public override string BaseModel => "models/citizen/citizen.vmdl";
	public override int minCash => 3;
	public override int maxCash => 6;
	public override float NPCScale => 0.35f;
	public override float CastleDamage => 5;

	public override void Spawn()
	{
		base.Spawn();
	}
}
