using System;
using System.Linq;
using System.Collections.Generic;
using Sandbox;
public partial class TDGame
{
	[Net] public int CurWave { get; private set; }
	[Net] public int MaxWave { get; private set; }
	[Net] public float WaveTimer { get; private set; }
	[Net] public int Difficulty { get; private set; }

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
		Starting,
		Active,
		Post
	}

	[Net, Predicted]
	public GameStatus CurGameStatus { get; private set; }

	[Net, Predicted] public WaveStatus CurWaveStatus { get; private set; }

	public void InitGameplay()
	{
		CurWaveStatus = WaveStatus.Idle;
		CurGameStatus = GameStatus.Idle;
		GameType = GamemodeType.Unspecified;
		Difficulty = 1;
		foreach ( var diffSetter in All )
		{
			if(diffSetter is DifficultySetter diff )
			{
				if ( diff.Difficulty == DifficultySetter.DiffEnum.Medium )
					Difficulty = 2;
				else if ( diff.Difficulty == DifficultySetter.DiffEnum.Hard )
					Difficulty = 3;
				if ( diff.Difficulty == DifficultySetter.DiffEnum.Impossible )
					Difficulty = 5;
			}
		}
	}

	public override void PostLevelLoaded()
	{
		base.PostLevelLoaded();

		List<WaveSetup> waves = new List<WaveSetup>();

		foreach ( var counter in All )
		{
			if ( counter is WaveSetup wave )
			{
				waves.Add( wave );
			}
		}

		int totalWaves = 0;

		foreach ( var duplicate in waves )
		{
			if ( duplicate.Wave_Order > totalWaves )
			{
				totalWaves = duplicate.Wave_Order;
			}
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
	public void StartGame()
	{
		StopMusicClient( To.Everyone );
		CurWave = 0;
		CurGameStatus = GameStatus.Active;
		CurWaveStatus = WaveStatus.Waiting;
		WaveTimer = 20.0f + Time.Now;

		foreach ( var client in Client.All )
		{
			if ( client.Pawn is TDPlayer player )
			{
				player.InitStats();

				if(GameType == GamemodeType.Competitive)
					SetUpTeams(player);
			}
		}
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
		if ( ConsoleSystem.GetValue( "td_music" ) == "False" )
			return;

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
			return;

		if ( CurGameStatus == GameStatus.Starting && (WaveTimer - Time.Now) <= 0 )
		{
			StartGame();
		}

		if ( CurGameStatus == GameStatus.Active )
		{
			if ( CurWaveStatus == WaveStatus.Waiting )
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

		if ( IsServer )
			foreach ( var client in Client.All)
			{
				if ( client.Pawn is TDPlayer player && player.lateJoiner && !client.IsBot )
					break;

				GameServices.SubmitScore( client.PlayerId, CurWave );
				
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
