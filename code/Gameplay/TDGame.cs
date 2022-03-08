
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public partial class TDGame : Sandbox.Game
{
	private Castle castle;
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
	public void UpdateHUD()
	{
		oldHud?.Delete();

		if ( IsClient )
			oldHud = new TDHUD();
	}

	public void SpawnCastle()
	{
		var castleSpawn = All.OfType<CastleEntity>().FirstOrDefault();

		if ( castleSpawn == null )
		{
			Log.Error( "This map does not support Super Terry Defense!" );
			return;
		}

		castle = new Castle();

		castle.Position = castleSpawn.Position;
		castle.Rotation = castleSpawn.Rotation;
	}

	public void CreateNPCSpawner()
	{
		var npcSpawner = All.OfType<HostileSpawner>().FirstOrDefault();

		if ( npcSpawner == null )
			return;
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
		}

		if (castle == null)
			SpawnCastle();
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
