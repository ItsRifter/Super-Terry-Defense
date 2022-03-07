using Sandbox;
using System.Collections.Generic;

[Library( "info_supertd_npc_spawner" )]
[Hammer.EditorModel( "models/citizen/citizen.vmdl" )]
[Hammer.EntityTool( "NPC Spawnpoint", "Super TD", "Defines a point where NPCs can spawn" )]
public class HostileSpawner : Entity
{
	private TimeSince timeLastSpawn;

	public int spawnCooldown;

	public int spawnCount;

	public List<WaveSetup> WaveSetters;

	public List<TDNPCBase> aliveNPCs;

	public override void Spawn()
	{
		base.Spawn();
		aliveNPCs = new List<TDNPCBase>();
		WaveSetters = new List<WaveSetup>();

		foreach ( var logicEnt in All )
		{
			if ( logicEnt is WaveSetup waveSetter )
				WaveSetters.Add( waveSetter );
		}
	}

	[Event.Tick.Server]
	public void SpawnNPC()
	{
		if ( TDGame.Current.CurWaveStatus != TDGame.WaveStatus.Active )
			return;

		if ( spawnCount <= 0 && aliveNPCs.Count <= 0 )
			TDGame.Current.EndWave();
		else
		{
			if ( timeLastSpawn >= spawnCooldown && spawnCount > 0 )
			{
				var newNPC = Library.Create<TDNPCBase>( WaveSetters[TDGame.Current.CurWave - 1].NPCs_To_Spawn.ToString());
				newNPC.Position = Position;
				newNPC.Rotation = Rotation;

				aliveNPCs.Add( newNPC );
				spawnCount--;
				timeLastSpawn = 0;
			}
		}
	}

	[Event( "td_new_wave" )]
	public void UpdateSpawnerIndex()
	{
		for ( int i = 0; i < WaveSetters.Count; i++ )
		{
			if( WaveSetters[i].Wave_Order == TDGame.Current.CurWave )
			{
				spawnCount = WaveSetters[i].Spawn_Count;
				spawnCooldown = WaveSetters[i].NPC_Spawn_Rate;
			}
		}
	}
	[Event( "td_evnt_restart" )]
	public void ClearNPCs()
	{
		aliveNPCs.Clear();
		spawnCount = 0;
	}

	[Event("td_npckilled")]
	public void OnNPCKilled(TDNPCBase killedNPC)
	{
		aliveNPCs.Remove( killedNPC );
	}
}
