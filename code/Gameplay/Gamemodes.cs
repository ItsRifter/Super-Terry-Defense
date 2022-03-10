using System;
using Sandbox;

public partial class TDGame
{
	public enum GamemodeType
	{
		Unspecified,
		Cooperative,
		Competitive
	}

	[Net, Predicted]
	public GamemodeType GameType { get; private set; }

	[AdminCmd( "startcomp" )]
	public static void HostSelectComp()
	{
		Event.Run( "td_evnt_comp" );
	}

	[AdminCmd( "startcoop" )]
	public static void HostSelectCoop()
	{
		Event.Run( "td_evnt_coop" );
	}

	[Event( "td_evnt_coop" )]
	public void PlayCoop()
	{
		if ( CurGameStatus != GameStatus.Idle )
			return;

		GameType = GamemodeType.Cooperative;
		CurGameStatus = GameStatus.Starting;
		WaveTimer = 10.0f + Time.Now;
	}

	[Event( "td_evnt_comp" )]
	public void PlayComp()
	{
		if ( CurGameStatus != GameStatus.Idle )
			return;

		if ( !CanPlayCompetitive() )
		{
			Log.Info( "Can't play competitive with just yourself" );
			return;
		}

		GameType = GamemodeType.Competitive;
		CurGameStatus = GameStatus.Starting;
		WaveTimer = 10.0f + Time.Now;
	}

	[AdminCmd( "Test" )]
	public static void DoStuff()
	{
		Log.Info( "WORKS" );
	}

	public bool CanPlayCompetitive()
	{
		if ( Client.All.Count < 2 )
			return false;

		return true;
	}

	public void SetUpTeams(TDPlayer player )
	{
		if ( player.CurrentBluePlayers().Count == 0 && player.CurrentRedPlayers().Count == 0 )
		{
			if(Rand.Int(1, 2) == 1)
				player.JoinTeam( TDPlayer.Teams.Red );
			else
				player.JoinTeam( TDPlayer.Teams.Blue );
		}

		if ( player.CurrentBluePlayers().Count >= player.CurrentRedPlayers().Count )
			player.JoinTeam( TDPlayer.Teams.Red );
		else
			player.JoinTeam( TDPlayer.Teams.Blue );

	}

	public void NewGamePlus()
	{
		CurWave = 0;
	}

	public void AnnounceWinningTeam(string WinningTeam)
	{
		CurGameStatus = GameStatus.Post;

		if ( WinningTeam == "Red" )
			Log.Info( "Red team wins!" );
		else if ( WinningTeam == "Blue" )
			Log.Info( "Blue team wins!" );
	}
}
