using Sandbox;
using System.Collections.Generic;

[Library( "supertd_wave_setup" )]
[Hammer.SupportsSolid]
[Hammer.EntityTool( "Wave Setup", "Super TD", "Sets up the wave" )]
[Hammer.VisGroup( Hammer.VisGroup.Logic )]
public class WaveSetup : Entity
{
	[Property( "WaveOrder" ), Description( "What order should this go?" )]
	public int Wave_Order { get; set; }

	[Property( "SpawnCount" ), Description( "How many times should this spawn for that NPC" )]
	public int Spawn_Count { get; set; }

	public enum SpawnableNPC
	{
		Unspecified,
		//Normal NPCs
		Peasant,
		Zombie,
		Brute,
		Rebel,
		Rioter,
		Voidling,
		//Variant NPCs
		Hidden,
		//Boss NPCs
		ZombieBoss,
		VoidKing,
	}

	[Property( "NPCToSpawn" ), Description( "What NPC should this spawn" )]
	public SpawnableNPC NPCs_To_Spawn { get; set; }

	[Property( "NPCSpawnRate" ), Description( "How fast should this NPC spawn" )]
	public int NPC_Spawn_Rate { get; set; }

	public override void Spawn()
	{
		base.Spawn();
	}
}
