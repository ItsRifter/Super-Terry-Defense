using System;
using Sandbox;

public partial class ZombieBoss : TDNPCBase
{
	public override string NPCName => "Zombie Boss";
	public override int BaseHealth => 150;
	public override float BaseSpeed => 15;
	public override string BaseModel => "models/citizen/citizen.vmdl";
	public override int minCash => 25;
	public override int maxCash => 40;
	public override float NPCScale => 0.55f;
	public override float CastleDamage => 25;

	public override void Spawn()
	{
		base.Spawn();
		
		var clothing = new ModelEntity();
		clothing.SetModel( "models/citizen_clothes/jacket/labcoat.vmdl_c" );
		clothing.SetParent( this, true );

		RenderColor = new Color(0, 100, 0);
	}
}
