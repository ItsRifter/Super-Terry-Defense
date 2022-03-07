using Sandbox;
using System;
using System.Linq;

public partial class Rebel : TDNPCBase
{
	public override string NPCName => "Rebel";
	public override int BaseHealth => 35;
	public override float BaseSpeed => 20;
	public override string BaseModel => "models/citizen/citizen.vmdl";
	public override int minCash => 8;
	public override int maxCash => 19;
	public override float NPCScale => 0.35f;
	public override float CastleDamage => 12;

	public override void Spawn()
	{
		base.Spawn();		
	}
}
