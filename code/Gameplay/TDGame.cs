
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public partial class TDGame : Sandbox.Game
{
	private TDHUD oldHud;
	private Translation _translation;
	
	public static new TDGame Current => Sandbox.Game.Current as TDGame;

	public List<LeaderboardResult.Entry> leaderboard;
	
	public TDGame()
	{
		if ( IsServer )
		{
			InitGameplay();
			leaderboard = new List<LeaderboardResult.Entry>();
		}

		if ( IsClient )
		{
			oldHud = new TDHUD();
			_translation = new Translation();
		}
	}

	[Event.Hotload]
	public void HotloadGame()
	{
		CurGameStatus = GameStatus.Idle;
		GameType = GamemodeType.Unspecified;

		oldHud?.Delete();

		if ( IsClient )
			oldHud = new TDHUD();
	}

	public override void DoPlayerSuicide( Client cl )
	{
		return;
	}

	public override void ClientJoined( Client client )
	{
		base.ClientJoined( client );

		var player = new TDPlayer(client);
		player.Spawn();

		client.Pawn = player;

		//Late joiners
		if ( CurGameStatus == GameStatus.Active )
		{
			player.InitStats();

			if ( CurWave > 3 )
				player.lateJoiner = true;

			if(GameType == GamemodeType.Competitive)
			{
				if ( player.CurrentBluePlayers().Count >= player.CurrentRedPlayers().Count )
					player.JoinTeam( TDPlayer.Teams.Red );
				else
					player.JoinTeam( TDPlayer.Teams.Blue );
			}
		}
	}

	public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
	{
		if ( cl.Pawn is TDPlayer player )
			if ( player.curTower != null )
			{
				player.curTower.DestoryPreview();
			}

		base.ClientDisconnect( cl, reason );
	}
}
