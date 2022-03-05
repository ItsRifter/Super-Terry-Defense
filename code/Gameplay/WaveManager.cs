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

		curSoundtrack = "music_" + Rand.Int( 1, 1 );
		soundPlaying = Sound.FromScreen( curSoundtrack );
	}

	public void EndWave()
	{
		soundPlaying.Stop();

		if ( CurWave >= MaxWave )
			EndGame( true );
		else
		{
			WaveTimer = 40.0f + Time.Now;
			CurWaveStatus = WaveStatus.Waiting;
			
			Sound.FromScreen( curSoundtrack + "_end" );
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

			if ( CurWaveStatus == WaveStatus.Active )
			{
				if ( soundPlaying.Finished )
					soundPlaying = Sound.FromScreen( curSoundtrack );
			} else
			{
				soundPlaying.Stop();
			}
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
