using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

partial class TDPlayer : Player
{

	public Clothing.Container Clothing = new();

	public TowerBase curTower;

	public bool lateJoiner = false;

	private bool inUpgradeMode;
	private bool inSellMode;

	[ConVar.ClientData( "td_music" )]
	public bool Music { get; set; } = true;

	public enum Teams
	{
		Unspecified,
		Red,
		Blue
	}

	public Teams CurTeam;

	public TDPlayer()
	{
		
	}

	public TDPlayer( Client cl ) : this()
	{
		Clothing.LoadFromClient( cl );
	}

	[ClientCmd("td_togglemusic")]
	public static void UpdateMusicCMD()
	{
		Event.Run( "td_updatemusic" );
	}

	[Event("td_updatemusic")]
	public void UpdateMusic()
	{
		Music = !Music;

		if ( !Music )
			Log.Info( "Music off" );
		else
			Log.Info( "Music on" );

		ConsoleSystem.SetValue( "td_music", Music );
	}

	public override void Spawn()
	{
		base.Respawn();

		CurTeam = Teams.Unspecified;

		SetModel( "models/citizen/citizen.vmdl" );

		ConsoleSystem.SetValue( "td_currentlanguage", "EN" );
		ConsoleSystem.SetValue( "td_music", Music ); 

		Controller = new WalkController();
		Animator = new StandardPlayerAnimator();
		CameraMode = new FirstPersonCamera();

		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
		EnableAllCollisions = false;

		Clothing.DressEntity( this );

		inUpgradeMode = false;
		inSellMode = false;
	}

	public void JoinTeam(Teams newTeam)
	{
		CurTeam = newTeam;

		if (CurTeam == Teams.Red)
        {
			var compPoints = All.OfType<CompSpawn>();
			var randomSpawnPoint = compPoints.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();
			
			if ( randomSpawnPoint != null )
			{
				Position = randomSpawnPoint.Position;
				Rotation = randomSpawnPoint.Rotation;
			}
        }
	}

	public List<TDPlayer> CurrentRedPlayers()
	{
		var reds = new List<TDPlayer>();

		foreach ( var client in Client.All)
		{
			if ( client.Pawn is TDPlayer player && player.CurTeam == Teams.Red )
				reds.Add( player );
		}

		return reds;
	}

	public List<TDPlayer> CurrentBluePlayers()
	{
		var blues = new List<TDPlayer>();

		foreach ( var client in Client.All )
		{
			if ( client.Pawn is TDPlayer player && player.CurTeam == Teams.Blue )
				blues.Add( player );
		}

		return blues;
	}

	public override void Simulate( Client cl )
	{
		SimulateActiveChild( cl, ActiveChild );

		if (TDGame.Current.CurGameStatus == TDGame.GameStatus.Active)
			DoTowerInputs();

		TickPlayerUse();

		EyeRotation = Rotation;

		base.Simulate( cl );
	}

	public override void FrameSimulate( Client cl )
	{
		EyeRotation = Rotation;

		base.FrameSimulate( cl );
	}

	public void DoTowerInputs()
	{
		if ( IsClient )
			return;

		SelectingTowers();

		if ( !inSellMode && !inUpgradeMode )
			return;

		var targetTr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 150 )
			.Ignore( this )
			.Size( 2 )
			.Run();

		if (inSellMode)
		{
			if ( targetTr.Entity is TowerBase tower && targetTr.Entity is not Castle )
			{
				if ( Input.Pressed( InputButton.Attack1 ) && tower.Owner == this )
					tower.SellTower( this );
			}

		} else if (inUpgradeMode)
		{
			if ( targetTr.Entity is TowerBase tower && targetTr.Entity is not Castle )
			{
				if ( Input.Pressed( InputButton.Attack1 ) )
				{
					if ( tower.CanUpgrade( this ) )
						tower.UpgradeTower( this );
				}

			}
		}
	}

	private void SelectingTowers()
	{
		var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 150 )
				.Ignore( this )
				.Ignore( curTower )
				.Size( 2 )
				.Run();

		if ( curTower != null )
			curTower.UpdatePreview( tr, this );

		var nearbyTowers = FindInSphere( tr.EndPosition, 28 );
		bool badSpot = false;

		foreach ( var nearby in nearbyTowers )
		{
			if ( nearby is TowerBase || tr.Entity is TowerBlocker )
				badSpot = true;
		}

		if ( Input.Pressed( InputButton.Attack1 ) && curTower != null )
		{
			if ( tr.Entity != null && tr.Entity.GetType().ToString().Contains( "WorldEntity" ) && !badSpot )
			{
				if ( tr.Normal.z == 1 && curTower.CanAfford( this ) )
				{
					var newTower = Library.Create<TowerBase>( curTower.GetType().FullName );
					newTower.Position = tr.EndPosition;
					CurMoney -= curTower.Cost;
					newTower.Owner = this;
					newTower.CreateClientPanel( To.Everyone, newTower );
				}
			}
		}

		//Pistol
		if ( Input.Pressed( InputButton.Slot1 ) && curTower != null && curTower.GetType() != Library.Get<TowerBase>( "PistolTower" ) )
		{
			curTower.DestoryPreview();

			curTower = Library.Create<TowerBase>( "PistolTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}
		else if ( Input.Pressed( InputButton.Slot1 ) && curTower == null )
		{
			curTower = Library.Create<TowerBase>( "PistolTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}

		//SMG
		if ( Input.Pressed( InputButton.Slot2 ) && curTower != null && curTower.GetType() != Library.Get<TowerBase>( "SMGTower" ) )
		{
			curTower.DestoryPreview();

			curTower = Library.Create<TowerBase>( "SMGTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}
		else if ( Input.Pressed( InputButton.Slot2 ) && curTower == null )
		{
			curTower = Library.Create<TowerBase>( "SMGTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}

		//Explosive
		if ( Input.Pressed( InputButton.Slot3 ) && curTower != null && curTower.GetType() != Library.Get<TowerBase>( "ExplosiveTower" ) )
		{
			curTower.DestoryPreview();

			curTower = Library.Create<TowerBase>( "ExplosiveTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}
		else if ( Input.Pressed( InputButton.Slot3 ) && curTower == null )
		{
			curTower = Library.Create<TowerBase>( "ExplosiveTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}

		//Electric
		if ( Input.Pressed( InputButton.Slot4 ) && curTower != null && curTower.GetType() != Library.Get<TowerBase>( "ElectricTower" ) )
		{
			curTower.DestoryPreview();

			curTower = Library.Create<TowerBase>( "ElectricTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}
		else if ( Input.Pressed( InputButton.Slot4 ) && curTower == null )
		{
			curTower = Library.Create<TowerBase>( "ElectricTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}

		//Radar
		if ( Input.Pressed( InputButton.Slot5 ) && curTower != null && curTower.GetType() != Library.Get<TowerBase>( "RadarTower" ) )
		{
			curTower.DestoryPreview();

			curTower = Library.Create<TowerBase>( "RadarTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}
		else if ( Input.Pressed( InputButton.Slot5 ) && curTower == null )
		{
			curTower = Library.Create<TowerBase>( "RadarTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}

		//Sniper
		if ( Input.Pressed( InputButton.Slot6 ) && curTower != null && curTower.GetType() != Library.Get<TowerBase>( "SniperTower" ) )
		{
			curTower.DestoryPreview();

			curTower = Library.Create<TowerBase>( "SniperTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}
		else if ( Input.Pressed( InputButton.Slot6 ) && curTower == null )
		{
			curTower = Library.Create<TowerBase>( "SniperTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}

		//Frost
		if ( Input.Pressed( InputButton.Slot7 ) && curTower != null && curTower.GetType() != Library.Get<TowerBase>( "FrostTower" ) )
		{
			curTower.DestoryPreview();

			curTower = Library.Create<TowerBase>( "FrostTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}
		else if ( Input.Pressed( InputButton.Slot7 ) && curTower == null )
		{
			curTower = Library.Create<TowerBase>( "FrostTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}

		if ( Input.Pressed( InputButton.Slot8 ) && !inUpgradeMode )
		{
			if ( curTower != null )
			{
				curTower.DestoryPreview();
				curTower = null;
			}

			inSellMode = false;
			inUpgradeMode = true;
		}

		if ( Input.Pressed( InputButton.Slot9 ) && !inSellMode )
		{
			if ( curTower != null )
			{
				curTower.DestoryPreview();
				curTower = null;
			}

			inUpgradeMode = false;
			inSellMode = true;
		}

		if ( Input.Pressed( InputButton.Slot0 ) )
		{
			if ( curTower != null )
			{
				curTower.DestoryPreview();
				curTower = null;
			}

			inSellMode = false;
			inUpgradeMode = false;
		}

		if ( curTower != null )
			curTower.Owner = this;
	}
}
