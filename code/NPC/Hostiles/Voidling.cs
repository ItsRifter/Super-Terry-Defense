using Sandbox;
using System;
using System.Linq;

public partial class Voidling : TDNPCBase
{
	public override string NPCName => "Voidling";
	public override int BaseHealth => 40;
	public override float BaseSpeed => 45;
	public override string BaseModel => "models/citizen/citizen.vmdl";
	public override int minCash => 12;
	public override int maxCash => 20;
	public override float NPCScale => 0.30f;
	public override float CastleDamage => 15;

	public override void Spawn()
	{
		base.Spawn();

		RenderColor = new Color( 180, 0, 240 );
	}
}
