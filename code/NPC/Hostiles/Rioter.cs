using Sandbox;
using System;
using System.Linq;

public partial class Rioter : TDNPCBase
{
	public override string NPCName => "Rioter";
	public override int BaseHealth => 85;
	public override float BaseSpeed => 15;
	public override string BaseModel => "models/citizen/citizen.vmdl";
	public override int minCash => 8;
	public override int maxCash => 12;
	public override float NPCScale => 0.40f;
	public override float CastleDamage => 15;
	public override void Spawn()
	{
		base.Spawn();

		var clothing = new ModelEntity();
		clothing.SetModel( "models/citizen_clothes/hat/Balaclava/Models/balaclava.vmdl" );
		clothing.SetParent( this, true );
	}
}
