﻿using Sandbox;
using System;
using System.Linq;

public partial class Zombie : TDNPCBase
{
	public override string NPCName => "Zombie";
	public override int BaseHealth => 30;
	public override float BaseSpeed => 10;
	public override string BaseModel => "models/citizen/citizen.vmdl";
	public override int minCash => 5;
	public override int maxCash => 12;
	public override float NPCScale => 0.35f;
	public override float CastleDamage => 7;

	public override void Spawn()
	{
		base.Spawn();		

		RenderColor = Color.Green;
	}
}
