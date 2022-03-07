﻿using Sandbox;
using System;
using System.Linq;

public partial class Brute : TDNPCBase
{
	public override string NPCName => "Brute";
	public override int BaseHealth => 15;
	public override float BaseSpeed => 20;
	public override string BaseModel => "models/citizen/citizen.vmdl";
	public override int minCash => 4;
	public override int maxCash => 9;
	public override float NPCScale => 0.35f;
	public override float CastleDamage => 5;
	public override void Spawn()
	{
		base.Spawn();		

		RenderColor = Color.Orange;
	}
}
