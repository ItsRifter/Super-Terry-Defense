using Sandbox;
using System;
using System.Linq;

public partial class Brute : TDNPCBase
{
	public override string NPCName => "Brute";
	public override int BaseHealth => 12;
	public override float BaseSpeed => 30;
	public override string BaseModel => "models/citizen/citizen.vmdl";
	public override int minCash => 3;
	public override int maxCash => 6;
	public override float NPCScale => 0.35f;
	public override float CastleDamage => 5;
	public override void Spawn()
	{
		base.Spawn();		

		RenderColor = Color.Orange;
	}
}
