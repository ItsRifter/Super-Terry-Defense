using Sandbox;
using System;
using System.Linq;

public partial class Brute : TDNPCBase
{
	public override string NPCName => "Brute";
	public override int BaseHealth => 30;
	public override float BaseSpeed => 25;
	public override string BaseModel => "models/citizen/citizen.vmdl";
	public override int minCash => 4;
	public override int maxCash => 9;
	public override float NPCScale => 0.35f;
	public override float CastleDamage => 9;
	public override void Spawn()
	{
		base.Spawn();

		var clothing = new ModelEntity();
		clothing.SetModel( "models/citizen_clothes/hat/Balaclava/Models/balaclava.vmdl" );
		clothing.SetParent( this, true );
	}
}
