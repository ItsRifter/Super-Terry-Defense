using System;
using Sandbox;

public partial class VoidKing : TDNPCBase
{
	public override string NPCName => "Void King";
	public override int BaseHealth => 750;
	public override float BaseSpeed => 13;
	public override string BaseModel => "models/citizen/citizen.vmdl";
	public override int minCash => 65;
	public override int maxCash => 150;
	public override float NPCScale => 0.65f;
	public override float CastleDamage => 100;

	public override void Spawn()
	{
		base.Spawn();

		RenderColor = new Color( 80, 0, 110 );
	}
}
