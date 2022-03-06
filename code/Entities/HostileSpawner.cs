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

	private int index;

	public List<WaveSetup> WaveSetters;

	public List<TDNPCBase> aliveNPCs;

	public override void Spawn()
	{
		base.Spawn();
		index = -1;
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
				var newNPC = Library.Create<TDNPCBase>( WaveSetters[index].NPCs_To_Spawn.ToString());
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
		index++;

		if( (WaveSetters.Count - 1) < index )
		{
			Log.Error( "This wave is missing logic, Tell the map designer!" );
			return;
		}

		spawnCount = WaveSetters[index].Spawn_Count;
		spawnCooldown = WaveSetters[index].NPC_Spawn_Rate;
	}

	[Event("td_npckilled")]
	public void OnNPCKilled(TDNPCBase killedNPC)
	{
		aliveNPCs.Remove( killedNPC );
	}
}
