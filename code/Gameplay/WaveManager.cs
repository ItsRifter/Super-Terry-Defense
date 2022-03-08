using System;
using System.Linq;
using Sandbox;

public partial class TDGame
{
	[Net] public int CurWave { get; private set; }
	[Net] public int MaxWave { get; private set; }
	[Net] public float WaveTimer { get; private set; }

	private string curSoundtrack;

	private Sound soundPlaying;

	public enum WaveStatus
	{
		Idle,
		Waiting,
		Active,
	}

	public enum GameStatus
	{
		Idle,
		Active,
		Post
	}

	[Net] public GameStatus CurGameStatus { get; private set; }
	[Net] public WaveStatus CurWaveStatus { get; private set; }

	public void InitGameplay()
	{
		WaveTimer = 45.0f + Time.Now;
		CurWaveStatus = WaveStatus.Idle;
		CurGameStatus = GameStatus.Idle;
	}

	public override void PostLevelLoaded()
	{
		base.PostLevelLoaded();

		int totalWaves = 0;

		foreach ( var counter in All )
		{
			if ( counter is WaveSetup )
				totalWaves++;
		}

		MaxWave = totalWaves;
	}

	[AdminCmd("td_restart")]
	public static void RestartGameCMD()
	{
		Event.Run( "td_evnt_restart" );

		foreach(var ents in All)
		{
			if ( ents is TDNPCBase npc )
				npc.Delete();

			if ( ents is TowerBase tower )
			{
				tower.DestroyClientPanel( To.Everyone );
				tower.Delete();
			}
		}

		var castleSpawn = All.OfType<CastleEntity>().FirstOrDefault();

		var castle = new Castle();

		castle.Position = castleSpawn.Position;
		castle.Rotation = castleSpawn.Rotation;
	}

	[Event( "td_evnt_restart" )]
	public async void StartGame()
	{
		StopMusicClient( To.Everyone );
		CurWave = 0;
		CurGameStatus = GameStatus.Active;
		CurWaveStatus = WaveStatus.Waiting;
		WaveTimer = 20.0f + Time.Now;

		foreach ( var client in Client.All )
		{
			if ( client.Pawn is TDPlayer player )
				player.InitStats();
		}

		var q = await GameServices.Leaderboard.Query( Global.GameIdent );
		var sorted = q.Entries.OrderBy( x => x.Rating ).Where( x => x.Rating > 0 ).ToList();
		leaderboard = sorted;
	}

	public void StartWave()
	{
		CurWaveStatus = WaveStatus.Active;
		CurWave++;

		Event.Run( "td_new_wave" );

		curSoundtrack = "music_" + Rand.Int( 1, 2 );
		PlayMusicClient( To.Everyone, curSoundtrack );
	}

	[ClientRpc]
	public void StopMusicClient()
	{
		soundPlaying.Stop();
	}

	[ClientRpc]
	public void PlayMusicClient(string soundtrack)
	{
		soundPlaying.Stop();
		soundPlaying = Sound.FromScreen( soundtrack );
	}

	public void EndWave()
	{
		if ( CurWave >= MaxWave )
			EndGame( true );
		else
		{
			WaveTimer = 20.0f + Time.Now;
			CurWaveStatus = WaveStatus.Waiting;
			PlayMusicClient( To.Everyone, curSoundtrack + "_end" );
		}
	}

	[Event.Tick.Server]
	public void UpdateTimer()
	{
		if ( CurGameStatus == GameStatus.Idle )
		{
			if ( (WaveTimer - Time.Now) <= 0 )
				StartGame();
		} else if ( CurGameStatus == GameStatus.Active )
		{
			if( CurWaveStatus == WaveStatus.Waiting )
				if ( (WaveTimer - Time.Now) <= 0 )
					StartWave();
		}
	}

	[AdminCmd("td_endgame")]
	public static void EndGameCMD()
	{
		Current.EndGame();
	}

	public void EndGame(bool playerWin = false)
	{
		if ( CurGameStatus != GameStatus.Active )
			return;

		if(IsServer)
			foreach ( var client in Client.All)
			{
				foreach ( var leader in leaderboard )
				{
					if(leader.DisplayName == client.Name)
						GameServices.SubmitScore( client.PlayerId, leader.Rating + CurWave );
					else
						GameServices.SubmitScore( client.PlayerId, CurWave );
				}
			}

		CurGameStatus = GameStatus.Post;
		Log.Info( "GAME OVER" );

		if (playerWin)
		{
			PlayMusicClient( To.Everyone, "music_win" );
			Log.Info( "Players successfully defended the castle" );
		} 
		else
		{
			PlayMusicClient( To.Everyone, "music_lost" );
			Log.Info( "Castle destroyed, Players have lost" );
		}

		GameServices.EndGame();
	}
}
