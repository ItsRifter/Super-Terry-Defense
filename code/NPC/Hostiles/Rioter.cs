using Sandbox;
using System;
using System.Linq;

public partial class Rioter : TDNPCBase
{
	public override string NPCName => "Rioter";
	public override int BaseHealth => 65;
	public override float BaseSpeed => 15;
	public override string BaseModel => "models/citizen/citizen.vmdl";
	public override int minCash => 8;
	public override int maxCash => 12;
	public override float NPCScale => 0.40f;
	public override float CastleDamage => 7;
	public override void Spawn()
	{
		base.Spawn();		

		RenderColor = Color.Red;
	}
}
