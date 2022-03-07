using Sandbox;
using System;
using System.Linq;

public partial class Hidden : TDNPCBase
{
	public override string NPCName => "Hidden";
	public override int BaseHealth => 15;
	public override float BaseSpeed => 30;
	public override string BaseModel => "models/citizen/citizen.vmdl";
	public override int minCash => 7;
	public override int maxCash => 13;
	public override float NPCScale => 0.35f;
	public override float CastleDamage => 6;
	public override SpecialType NPCType => SpecialType.Cloaked; 
	public override void Spawn()
	{
		base.Spawn();		

		RenderColor = new Color( 255, 255, 255, 0.35f );
	}
}
