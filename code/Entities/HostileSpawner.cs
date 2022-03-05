using Sandbox;
using System.Collections.Generic;

[Library( "info_supertd_npc_spawner" )]
[Hammer.EditorModel( "models/citizen/citizen.vmdl" )]
[Hammer.EntityTool( "NPC Spawnpoint", "Super TD", "Defines a point where NPCs can spawn" )]
public class HostileSpawner : Entity
{
	private TimeSince timeLastSpawn;

	public int spawnCooldown = 5;

	public int spawnCount = 5;
	public int baseSpawnCount = 5;

	public List<TDNPCBase> aliveNPCs;

	public override void Spawn()
	{
		base.Spawn();
		aliveNPCs = new List<TDNPCBase>();
	}

	[Event.Tick.Server]
	public void SpawnNPC()
	{
		if ( TDGame.Current.CurWaveStatus != TDGame.WaveStatus.Active )
			return;

		if ( spawnCount <= 0 && aliveNPCs.Count <= 0 )
		{
			baseSpawnCount *= 2;
			spawnCount = baseSpawnCount;

			TDGame.Current.EndWave();
		}
		else
		{
			if ( timeLastSpawn >= spawnCooldown && spawnCount > 0 )
			{
				var newNPC = new Zombie();
				newNPC.Position = Position;
				newNPC.Rotation = Rotation;

				aliveNPCs.Add( newNPC );
				spawnCount--;
				timeLastSpawn = 0;
			}
		}
	}

	[Event("td_npckilled")]
	public void OnNPCKilled(TDNPCBase killedNPC)
	{
		aliveNPCs.Remove( killedNPC );
	}
}
