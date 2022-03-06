using System;
using Sandbox;

public partial class TDGame
{
	[Net] public int CurWave { get; private set; }
	[Net] public int MaxWave { get; } = 30;
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
		CurWave = 0;
		WaveTimer = 30.0f + Time.Now;
		CurWaveStatus = WaveStatus.Idle;
		CurGameStatus = GameStatus.Idle;
	}

	public void StartGame()
	{
		CurGameStatus = GameStatus.Active;
		CurWaveStatus = WaveStatus.Waiting;
		WaveTimer = 40.0f + Time.Now;

		foreach (var client in Client.All)
		{
			if ( client.Pawn is TDPlayer player )
				player.InitStats();
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
			WaveTimer = 40.0f + Time.Now;
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

	public void EndGame(bool playerWin = false)
	{
		Log.Info( "GAME OVER" );

		if (playerWin)
		{
			Log.Info( "Players successfully defended the castle" );
		} else
		{
			Log.Info( "Castle destroyed, Players have lost" );
		}
	}
}
