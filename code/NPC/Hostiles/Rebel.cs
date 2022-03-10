using Sandbox;
using System;
using System.Linq;

public partial class Rebel : TDNPCBase
{
	public override string NPCName => "Rebel";
	public override int BaseHealth => 75;
	public override float BaseSpeed => 25;
	public override string BaseModel => "models/citizen/citizen.vmdl";
	public override int minCash => 8;
	public override int maxCash => 19;
	public override float NPCScale => 0.35f;
	public override float CastleDamage => 13;

	public override void Spawn()
	{
		base.Spawn();
		
		var clothing = new ModelEntity();
		clothing.SetModel( "models/citizen_clothes/vest/Tactical_Vest/Models/tactical_vest.vmdl" );
		clothing.SetParent( this, true );
	}
}
